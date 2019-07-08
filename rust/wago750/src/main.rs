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
    if !INIT_DONE {
        let (mut crc, mut c): (u16, u16);
        for i in 0..256usize {
            c = i as u16;
            crc = 0;
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
}

// 0000 C0C1 C181 0140 C301 03C0 0280 C241 C601 06C0 0780 C741 0500 C5C1 C481 0440
// CC01 0CC0 0D80 CD41 0F00 CFC1 CE81 0E40 0A00 CAC1 CB81 0B40 C901 09C0 0880 C841
// D801 18C0 1980 D941 1B00 DBC1 DA81 1A40 1E00 DEC1 DF81 1F40 DD01 1DC0 1C80 DC41
// 1400 D4C1 D581 1540 D701 17C0 1680 D641 D201 12C0 1380 D341 1100 D1C1 D081 1040
// F001 30C0 3180 F141 3300 F3C1 F281 3240 3600 F6C1 F781 3740 F501 35C0 3480 F441
// 3C00 FCC1 FD81 3D40 FF01 3FC0 3E80 FE41 FA01 3AC0 3B80 FB41 3900 F9C1 F881 3840
// 2800 E8C1 E981 2940 EB01 2BC0 2A80 EA41 EE01 2EC0 2F80 EF41 2D00 EDC1 EC81 2C40
// E401 24C0 2580 E541 2700 E7C1 E681 2640 2200 E2C1 E381 2340 E101 21C0 2080 E041
// A001 60C0 6180 A141 6300 A3C1 A281 6240 6600 A6C1 A781 6740 A501 65C0 6480 A441
// 6C00 ACC1 AD81 6D40 AF01 6FC0 6E80 AE41 AA01 6AC0 6B80 AB41 6900 A9C1 A881 6840
// 7800 B8C1 B981 7940 BB01 7BC0 7A80 BA41 BE01 7EC0 7F80 BF41 7D00 BDC1 BC81 7C40
// B401 74C0 7580 B541 7700 B7C1 B681 7640 7200 B2C1 B381 7340 B101 71C0 7080 B041
// 5000 90C1 9181 5140 9301 53C0 5280 9241 9601 56C0 5780 9741 5500 95C1 9481 5440
// 9C01 5CC0 5D80 9D41 5F00 9FC1 9E81 5E40 5A00 9AC1 9B81 5B40 9901 59C0 5880 9841
// 8801 48C0 4980 8941 4B00 8BC1 8A81 4A40 4E00 8EC1 8F81 4F40 8D01 4DC0 4C80 8C41
// 4400 84C1 8581 4540 8701 47C0 4680 8641 8201 42C0 4380 8341 4100 81C1 8081 4040

fn print_table() {
    unsafe {
        init_table();
        for i in 0..256 {
            if i % 16 == 0 {
                println!();
            }
            print!("{:04X} ", TABLE[i]);
        }
        println!();
    }
}

fn checksum(vec: &Vec<u8>) -> u16 {
    let mut crc: u16 = CRC_START_MODBUS;
    unsafe {
        init_table();
        let (mut tmp, mut c): (u16, u16);
        for v in vec {
            c = 0x00FF & (*v as u16);
            tmp = crc ^ c;
            crc = (crc >> 8) ^ TABLE[(tmp & 0xff) as usize];
        }
    }
    crc
}

#[test]
fn test_checksum() {
    let vec: Vec<u8> = vec![0, 1, 2, 3, 4, 5, 6, 7, 8, 9];
    let crc: u16 = checksum(&vec);
    assert!(crc == 0x4574);
}

fn test_checksum_() {
    let vec: Vec<u8> = vec![0, 1, 2, 3, 4, 5, 6, 7, 8, 9];
    let crc: u16 = checksum(&vec);
    assert!(crc == 0x4574);
    println!(
        "checksum = 0x{:04X},0x{:04x},0b{:016b},o{:06o},{:06}",
        crc, crc, crc, crc, crc
    );
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
    test_checksum_();
    std::process::exit(0);
}
