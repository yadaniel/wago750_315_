#![allow(unused)]

mod wago;
use wago::WagoModule::*;
use wago::*;

fn main() {
    let mut w315 = Wago750315 {
        res_io: AddrIO(0, 0, 0, 0),
        io: AddrIO(0, 0, 0, 0),
    };
    let mut w430 = Wago750430 {
        res_io: AddrIO(0, 8, 0, 0),
        io: AddrIO(0, 0, 0, 0),
    };
    let mut w530 = Wago750530 {
        res_io: AddrIO(0, 0, 0, 8),
        io: AddrIO(0, 0, 0, 0),
    };
    let mut w468 = Wago750468 {
        res_io: AddrIO(2, 0, 0, 0),
        io: AddrIO(0, 0, 0, 0),
    };
    let mut w515 = Wago750515 {
        res_io: AddrIO(0, 0, 0, 4),
        io: AddrIO(0, 0, 0, 0),
    };
    let mut w559 = WagoModule::Wago750559 {
        res_io: AddrIO(0, 0, 2, 0),
        io: AddrIO(0, 0, 0, 0),
    };

    let mut wago = Wago {
        init: false,
        mods: vec![w315, w430, w530, w468, w515],
        modbus_address: 1,
        port: None,
    };

    Wago::init(&mut wago);
    wago.print();
    w315 = wago.mods[0];
    w430 = wago.mods[1];
    w530 = wago.mods[2];
    w468 = wago.mods[3];
    w515 = wago.mods[4];

    if wago.init_serial(27) {
        // interface ideas
        // wago530.at(wago).set(0xFF);
        // wago.set(wago530).set(0xFF);

        // test 530
        for v in 0..256 {
            wago.set(w530, v as u8);
            std::thread::sleep(std::time::Duration::from_millis(50));
        }
        wago.set(w530, 0x00);

        // test 515
        for v in 0..16 {
            wago.set(w515, v as u8);
            std::thread::sleep(std::time::Duration::from_millis(250));
        }
        wago.set(w515, 0x00);

        // test 430
        loop {
            let v = wago.get(w430);
            println!("w430=0b{0:08b}", v);
            std::thread::sleep(std::time::Duration::from_millis(100));
            if v != 0 {
                break;
            }
        }

        // test 468
        loop {
            for channel in 0..4 {
                // let volt: f64 = wago.get_volt(w468, channel);
                // println!("w468[{}]={:.2}", channel, volt);
                let volt: f64 = wago.get_volt(w468, 0);
                println!("w468[{}]={:.2}", 0, volt);
            }
            std::thread::sleep(std::time::Duration::from_millis(100));
        }

    //
    } else {
        println!("init_serial: failed");
    }
}
