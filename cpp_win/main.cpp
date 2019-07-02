#include <cassert>
#include "wago750.h"

using namespace wago750;

bool test_ModbusCRC16() {
    uint8_t buffer[] = {0,1,2,3,4,5,6,7,8,9};
    uint16_t crc = ModbusCRC16::getCRC(buffer, 10);
    std::cout << std::uppercase << std::hex << crc << std::endl;
    return crc == 0x4574;
}

bool test_WagoCoupler_750315() {
    WagoCoupler_750315 dut(0,0);
    std::vector<uint8_t> vec {0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0A,0xFF};
    std::vector<uint8_t> res = dut.hexlify(vec);
    std::string str(res.begin(), res.end());
    std::cout << str << std::endl;
    return str == "000102030405060708090AFF";
}

bool test_WagoCoupler_750315_COM() {
    WagoCoupler_750315 dut(27,1);
    // dut.sendRecv();
    return true;
}

bool test_all() {
    WagoCoupler_750315 wago(27,1);
    Wago_750430 wago430;
    Wago_750530 wago530;
    Wago_750468 wago468;
    Wago_750515 wago515;
    Wago_750559 wago559;
    wago.addModule(&wago430);
    wago.addModule(&wago530);
    wago.addModule(&wago468);
    wago.addModule(&wago515);
    wago.addModule(&wago559);
    uint8_t state = 0;
    uint16_t state16 = 0;

    // // wago430
    // if(wago430.get(state)) {
    //     std::cout << "430:get " << std::uppercase << std::hex << (uint32_t)state << std::endl;
    // }

    // // wago530
    // for(state16=0; state16<256; state16++) {
    //     Sleep(100);
    //     if(wago530.set(state16)) {
    //         std::cout << "530: set " << std::uppercase << std::hex << (uint32_t)state16 << std::endl;
    //     }
    // }
    // if(wago530.set(0xFF)) {
    //     std::cout << "530: set FF" << std::endl;
    // }

    // // wago515
    // for(state=0; state<16; state++) {
    //     Sleep(100);
    //     if(wago515.set(state)) {
    //         std::cout << "515: set " << std::uppercase << std::hex << (uint32_t)state << std::endl;
    //     }
    // }
    // if(wago515.set(0x00)) {
    //     std::cout << "515: set 0" << std::endl;
    // }

    // wago468
    for(int channel=0; channel<4; channel++) {
        double voltage = 0;
        if(wago468.get(channel, voltage)) {
            std::cout << "468: get[" << channel << "] " << voltage << std::endl;
        }
    }

    // done
    return true;
}

int main() {
    // assert(test_ModbusCRC16());
    // assert(test_WagoCoupler_750315());
    // assert(test_WagoCoupler_750315_COM());
    assert(test_all());
    return 0;
}

