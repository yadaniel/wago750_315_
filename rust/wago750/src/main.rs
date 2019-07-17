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

    //
    } else {
        println!("init_serial: failed");
    }
}

// 01 0F 00 08 00 08 01 00 1F 54
// 010F000800080101DE94
// 010F0008000801029E95
// 010F0008000801035F55
// 010F0008000801041E97
// 010F000800080105DF57
// 010F0008000801069F56
// 010F0008000801075E96
// 010F0008000801081E92
// 010F000800080109DF52
// 010F00080008010A9F53
// 010F00080008010B5E93
// 010F00080008010C1F51
// 010F00080008010DDE91
// 010F00080008010E9E90
// 010F00080008010F5F50
// 010F0008000801001F54
