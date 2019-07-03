#![allow(unused)]
#[allow(unused)]

struct Wago750315; // modbus coupler
struct Wago750530; // 8DO 24VDC/0.5A
struct Wago750430; // 8DI 24VDC
struct Wago750515; // 4DO NO/2A
struct Wago750468; // 4AI 10V
struct Wago750559; // 4AO 10V

static mut TABLE: [u16; 256] = [0; 256];
static mut INIT_DONE: bool = false;
const CRC_POLY: u16 = 0xA001;
const CRC_START_MODBUS: u16 = 0xFFFF;

unsafe fn init_table() {
    let (mut crc, mut c): (u16, u16);
    for i in 0..256usize {
        crc = 0;
        c = i as u16;
        for j in 0..=7 {
            if (crc ^ c) & 0x0001 == 0x0001 {
                crc = (crc >> 1) ^ CRC_POLY;
            } else {
                crc >>= 1;
            }
            c >>= 1;
        }
        TABLE[i] = crc;
    }
    INIT_DONE = true;
}

fn print_table() {
    unsafe {
        println!("{}", INIT_DONE);
        if INIT_DONE {
            for i in 0..256 {
                if i % 16 == 0 {
                    println!();
                }
                print!("{:6} ", TABLE[i]);
                TABLE[i] = 1;
            }
            println!();
        }
    }
}

fn checksum(vec: &Vec<u8>) -> u16 {
    let mut crc: u16 = CRC_START_MODBUS;
    unsafe {
        if !INIT_DONE {
            init_table();
        }
        let (mut tmp, mut c): (u16, u16);
        for v in vec {
            c = 0x00FF & (*v as u16);
            tmp = crc ^ c;
            crc = (crc >> 8) ^ TABLE[(tmp & 0xff) as usize];
        }
    }
    crc
}

fn test_checksum() -> bool {
    let vec: Vec<u8> = vec![0, 1, 2, 3, 4, 5, 6, 7, 8, 9];
    let crc: u16 = checksum(&vec);
    // let crc = 0b11110000;
    println!("checksum = {:X},{:x},{:b},{}", crc, crc, crc, crc);
    true
}

fn testing_area() {
    // works
    let mut vec: Vec<u8> = (0u8..=255u8).step_by(1).map(|i| i * 2).collect::<Vec<u8>>();
    // works
    let mut arr: [u16; 256] = {
        let mut tmp: [u16; 256] = [0; 256];
        for i in 0..=255 {
            tmp[i] = 0
        }
        tmp
    };
}

fn main() {
    println!("Hello, world!");
    print_table();
    unsafe {
        init_table();
    }
    print_table();
    test_checksum();
}
