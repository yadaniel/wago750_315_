#![allow(unused)]

mod modbus_checksum;
use modbus_checksum::checksum;

extern crate serial;
use serial::prelude::*;
use serial::*;
use std::env;
use std::io;
use std::io::prelude::*;
use std::time::Duration;

/*
### input and output addresses start with 0 and are wordwise organized
### addresses start with bus terminals which have 1+ words/channel
### they are followed by addresses of bus terminals which have 1+ bits/channel
### bus terminals which have 1+ bits/channel are collected into 16bits words

### modbus functions
### 0x01 = read coil status  ---||      ... read input and output bits as sequence of bytes
### 0x02 = read input status ---||      ... read input bits as sequence of bytes
### 0x03 = read holding register ---||  ... read number of input words
### 0x04 = read input registers  ---||  ... read number of input words
### 0x05 = force single coil            ... write output bit
### 0x06 = preset single register       ... write output word
### 0x0B = fetch comm event ctr         ... read status word and event counter
### 0x0F = force multiple coils         ... write number of output bits
### 0x10 = preset multiple regs         ... write number of output words
READ_BIT = 0x01
READ_BITS = 0x01
WRITE_BIT = 0x05
WRITE_BITS = 0x0F
READ_WORDS = 0x03
WRITE_WORD = 0x06
*/

const READ_BIT: u8 = 0x01;
const READ_BITS: u8 = 0x01;
const WRITE_BIT: u8 = 0x05;
const WRITE_BITS: u8 = 0x0F;
const READ_WORDS: u8 = 0x03;
const WRITE_WORD: u8 = 0x06;

// index of AddrIO
const INRESBITS: u8 = 1;
const OUTRESBITS: u8 = 3;
const INRESWORDS: u8 = 0;
const OUTRESWORDS: u8 = 2;

// std::ops::{Add,AddAssign} can not be implemented
// type AddressIO = (u8, u8, u8, u8);

#[derive(Debug, Copy, Clone, Default)]
pub struct AddrIO(pub u8, pub u8, pub u8, pub u8);

impl std::ops::Add for AddrIO {
    type Output = Self;
    fn add(self, other: Self) -> Self {
        Self(
            self.0 + other.0,
            self.1 + other.1,
            self.2 + other.2,
            self.3 + other.3,
        )
    }
}

impl std::ops::AddAssign for AddrIO {
    fn add_assign(&mut self, other: Self) {
        self.0 += other.0;
        self.1 += other.1;
        self.2 += other.2;
        self.3 += other.3;
    }
}

#[derive(Debug, Copy, Clone)]
pub enum WagoModule {
    Wago750315 { res_io: AddrIO, io: AddrIO }, // modbus coupler
    Wago750530 { res_io: AddrIO, io: AddrIO }, // 8DO 24VDC/0.5A
    Wago750430 { res_io: AddrIO, io: AddrIO }, // 8DI 24VDC
    Wago750515 { res_io: AddrIO, io: AddrIO }, // 4DO NO/2A
    Wago750468 { res_io: AddrIO, io: AddrIO }, // 4AI 10V
    Wago750559 { res_io: AddrIO, io: AddrIO }, // 4AO 10V
}
use WagoModule::*;

impl WagoModule {
    pub fn set(&self, modbus_address: u8, port: &mut SystemPort, value: u8) {
        match *self {
            Wago750315 { res_io: _, io: _ } => (),
            Wago750530 { res_io: _, io: io_ } => {
                let mut buf: Vec<u8> = vec![
                    modbus_address,
                    WRITE_BITS,
                    0x00,
                    0x00 + io_.3,
                    0x00,
                    0x08,
                    0x01,
                    value,
                ];
                let tmp: u16 = checksum(&buf);
                buf.push((tmp & 0xFF) as u8);
                buf.push((tmp >> 8) as u8);
                port.write(&buf[..]);
                println!("debug 530 {:02X?} = {}", buf, value);
                // empty serial buffer
                let mut buf: [u8; 10] = [0; 10];
                port.read(&mut buf[..]);
            }
            Wago750430 { res_io: _, io: _ } => (),
            Wago750515 { res_io: _, io: io_ } => {
                let mut buf: Vec<u8> = vec![
                    modbus_address,
                    WRITE_BITS,
                    0x00,
                    0x00 + io_.3,
                    0x00,
                    0x08,
                    0x01,
                    value,
                ];
                let tmp: u16 = checksum(&buf);
                buf.push((tmp & 0xFF) as u8);
                buf.push((tmp >> 8) as u8);
                port.write(&buf[..]);
                println!("debug 515 {:02X?} = {}", buf, value);
                // empty serial buffer
                let mut buf: [u8; 10] = [0; 10];
                port.read(&mut buf[..]);
            }
            Wago750468 { res_io: _, io: _ } => (),
            Wago750559 { res_io: _, io: _ } => (),
        }
    }

    pub fn get(&self, modbus_address: u8, port: &mut SystemPort) -> u8 {
        match *self {
            Wago750315 { res_io: _, io: _ } => 0,
            Wago750530 { res_io: _, io: _ } => 0,
            Wago750430 { res_io: _, io: io_ } => {
                // write
                let mut buf: Vec<u8> =
                    vec![modbus_address, READ_BIT, 0x00, 0x00 + io_.1, 0x00, 0x08];
                let tmp: u16 = checksum(&buf);
                buf.push((tmp & 0xFF) as u8);
                buf.push((tmp >> 8) as u8);
                port.write(&buf[..]);
                print!("debug 430 {:02X?}", buf);
                // read
                let mut buf: [u8; 10] = [0; 10];
                match port.read(&mut buf[..]) {
                    Ok(readn) if readn == 6 => {
                        println!(" <= 430 {:02X?}", &buf[0..6]);
                        buf[3]
                    }
                    _ => {
                        println!();
                        0
                    }
                }
            }
            Wago750515 { res_io: _, io: _ } => 0,
            Wago750468 { res_io: _, io: _ } => 0,
            Wago750559 { res_io: _, io: _ } => 0,
        }
    }

    pub fn get_volt(&self, modbus_address: u8, port: &mut SystemPort, channel: u8) -> f64 {
        match *self {
            Wago750468 { res_io: _, io: io_ } => {
                // write
                let mut buf: Vec<u8> = vec![
                    modbus_address,
                    READ_WORDS,
                    0x00,
                    0x00 + io_.0 + channel,
                    0x00,
                    0x01,
                ];
                let tmp: u16 = checksum(&buf);
                buf.push((tmp & 0xFF) as u8);
                buf.push((tmp >> 8) as u8);
                port.write(&buf[..]);
                print!("debug 468 {:02X?}", buf);
                // read
                let mut buf: [u8; 10] = [0; 10];
                match port.read(&mut buf[..]) {
                    Ok(readn) if readn == 7 => {
                        println!(" <= 468 {:02X?}", &buf[0..7]);
                        let tmp: u16 = ((buf[3] as u16) << 8) | buf[4] as u16;
                        (tmp as f64) / (0x7FFF as f64) * 10.0
                    }
                    _ => {
                        println!();
                        0.0
                    }
                }
            }
            _ => 0.0,
        }
    }
}

pub struct Wago {
    pub init: bool,
    pub mods: Vec<WagoModule>,
    pub port: Option<SystemPort>,
    pub modbus_address: u8,
}

impl Wago {
    pub fn init(&mut self) {
        if self.init == false {
            self.init = true;

            let mut addr_io = AddrIO(0, 0, 0, 0);

            for module in self.mods.iter_mut() {
                match *module {
                    Wago750315 {
                        res_io: res_io_,
                        io: _,
                    } => {
                        *module = Wago750315 {
                            res_io: res_io_,
                            io: addr_io,
                        };
                        addr_io += res_io_;
                    }

                    Wago750430 {
                        res_io: res_io_,
                        io: _,
                    } => {
                        *module = Wago750430 {
                            res_io: res_io_,
                            io: addr_io,
                        };
                        addr_io += res_io_;
                    }

                    Wago750530 {
                        res_io: res_io_,
                        io: _,
                    } => {
                        *module = Wago750530 {
                            res_io: res_io_,
                            io: addr_io,
                        };
                        addr_io += res_io_;
                    }

                    Wago750515 {
                        res_io: res_io_,
                        io: _,
                    } => {
                        *module = Wago750515 {
                            res_io: res_io_,
                            io: addr_io,
                        };
                        addr_io += res_io_;
                    }

                    Wago750468 {
                        res_io: res_io_,
                        io: _,
                    } => {
                        *module = Wago750468 {
                            res_io: res_io_,
                            io: addr_io,
                        };
                        addr_io += res_io_;
                    }

                    Wago750559 {
                        res_io: res_io_,
                        io: _,
                    } => {
                        *module = Wago750559 {
                            res_io: res_io_,
                            io: addr_io,
                        };
                        addr_io += res_io_;
                    }
                };
            }
        }
    }

    pub fn init_serial(&mut self, num: u8) -> bool {
        match serial::open(&format!("com{}", num)) {
            Err(_) => return false,
            Ok(mut port) => {
                port.reconfigure(&|settings| {
                    settings.set_baud_rate(serial::Baud19200);
                    settings.set_char_size(serial::Bits8);
                    settings.set_parity(serial::ParityEven);
                    settings.set_stop_bits(serial::Stop1);
                    settings.set_flow_control(serial::FlowNone);
                    Ok(())
                });
                // port.set_timeout(Duration::from_millis(20));
                port.set_timeout(Duration::from_millis(35));
                self.port = Some(port);
                return true;
            }
        }
    }

    pub fn set(&mut self, module: WagoModule, value: u8) {
        if let Some(port) = &mut self.port {
            module.set(self.modbus_address, port, value);
        }
    }

    pub fn get(&mut self, module: WagoModule) -> u8 {
        if let Some(port) = &mut self.port {
            module.get(self.modbus_address, port)
        } else {
            0
        }
    }

    pub fn get_volt(&mut self, module: WagoModule, channel: u8) -> f64 {
        if let Some(port) = &mut self.port {
            module.get_volt(self.modbus_address, port, channel)
        } else {
            0.0
        }
    }

    pub fn print(&self) {
        println!("init={}", self.init);
        for module in self.mods.iter() {
            match *module {
                x @ _ => println!("{:?}", x),
            }
        }
    }
}

#[test]
fn test_modules() {
    let mut w0 = Wago750315 {
        res_io: AddrIO(0, 0, 0, 0),
        io: AddrIO(0, 0, 0, 0),
    };
    let mut w1 = Wago750430 {
        res_io: AddrIO(0, 8, 0, 0),
        io: AddrIO(0, 0, 0, 0),
    };
    let mut w2 = Wago750530 {
        res_io: AddrIO(0, 0, 0, 8),
        io: AddrIO(0, 0, 0, 0),
    };
    let mut w3 = Wago750468 {
        res_io: AddrIO(2, 0, 0, 0),
        io: AddrIO(0, 0, 0, 0),
    };
    let mut w4 = Wago750515 {
        res_io: AddrIO(0, 0, 0, 4),
        io: AddrIO(0, 0, 0, 0),
    };
    let mut w5 = WagoModule::Wago750559 {
        res_io: AddrIO(0, 0, 2, 0),
        io: AddrIO(0, 0, 0, 0),
    };

    let mut wago = Wago {
        init: false,
        mods: vec![w0, w1, w2, w3, w4],
        port: None,
        modbus_address: 1,
    };

    Wago::init(&mut wago);
    wago.init_serial(27);
    wago.print();
}
