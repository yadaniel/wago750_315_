#include "wago750.h"

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

namespace wago750 {

/// implementation ModbusCRC16
///
///
uint16_t ModbusCRC16::table[256] = {};
bool ModbusCRC16::initialized = false;

void ModbusCRC16::init() {
    if(initialized == false) {
        uint16_t crc, c;
        for(uint16_t i=0; i<256; i++) {
            crc = 0;
            c   = i;
            for(uint16_t j=0; j<8; j++) {
                if( ((crc ^ c) & 0x0001) == 0x0001 ) {
                    crc = (uint16_t) (( crc >> 1 ) ^ CRC_POLY);
                } else {
                    crc = (uint16_t) (crc >> 1);
                }
                c = (uint16_t) (c >> 1);
            }
            table[i] = crc;
        }
        initialized = true;
    }
}

uint16_t ModbusCRC16::getCRC(uint8_t * input, int length) {
    if(initialized == false) {
        init();
    }
    uint16_t crc, tmp, short_c;
    crc = CRC_START_MODBUS;
    if(input != nullptr) {
        for(int i=0; i<length; i++) {
            short_c = (uint16_t) (0x00FF & (uint16_t) (input[i]));
            tmp     = (uint16_t) (crc ^ short_c);
            crc     = (uint16_t) ((crc >> 8) ^ table[ tmp & 0xff ]);
        }
    }
    return crc;
}

/// implementation WagoModule
///
///
std::vector<uint8_t> WagoModule::sendRecv(std::vector<uint8_t> sendBytes)const {
    HANDLE handle = coupler->serialHandle;
    LPCVOID outBuffer = sendBytes.data();
    DWORD size = sendBytes.size();
    DWORD written = 0;
    LPOVERLAPPED unknown = nullptr;
    WriteFile(handle, outBuffer, size, &written, unknown);
    // receive
    uint8_t inBuffer[100] = {};
    DWORD read = 0;
    ReadFile(handle, (void *)&inBuffer[0], 100, &read, unknown);
    std::vector<uint8_t> recvBytes(inBuffer, inBuffer + read);
    return recvBytes;
}

/// implementation WagoCoupler_750315
///
///
WagoCoupler_750315::WagoCoupler_750315(uint8_t serialPortInit, uint8_t modbusAddrInit) :
    WagoModule(0,0,0,0),
    serialPort(serialPortInit),
    modbusAddr(modbusAddrInit) {
    coupler = this;

    std::string portName = "\\\\.\\COM" + std::to_string(serialPort);
    serialHandle = CreateFile(portName.c_str(), GENERIC_READ | GENERIC_WRITE, 0, 0, OPEN_EXISTING, FILE_ATTRIBUTE_NORMAL, 0);
    if(serialHandle != INVALID_HANDLE_VALUE) {

        // Do some basic settings
        DCB serialParams = { 0 };
        serialParams.DCBlength = sizeof(serialParams);

        GetCommState(serialHandle, &serialParams);
        serialParams.BaudRate = 19200;
        serialParams.ByteSize = 8;
        serialParams.StopBits = ONESTOPBIT;
        serialParams.Parity = EVENPARITY;
        SetCommState(serialHandle, &serialParams);

        // Set timeouts
        COMMTIMEOUTS timeout = { 0 };
        timeout.ReadIntervalTimeout = 50;
        timeout.ReadTotalTimeoutConstant = 50;
        timeout.ReadTotalTimeoutMultiplier = 50;
        timeout.WriteTotalTimeoutConstant = 50;
        timeout.WriteTotalTimeoutMultiplier = 10;
        SetCommTimeouts(serialHandle, &timeout);
    }
}

WagoCoupler_750315::~WagoCoupler_750315() {
    if(serialHandle != INVALID_HANDLE_VALUE) {
        CloseHandle(serialHandle);
    }
}

void WagoCoupler_750315::addModule(WagoModule * wagoModule) {
    // assign added module the running address
    wagoModule->inWords = inWords;
    wagoModule->inBits = inBits;
    wagoModule->outWords = outWords;
    wagoModule->outBits = outBits;
    wagoModule->coupler = this;
    // increment the running address
    inWords += wagoModule->InResWords;
    inBits += wagoModule->InResBits;
    outWords += wagoModule->OutResWords;
    outBits += wagoModule->OutResBits;
    moduleList.push_back(wagoModule);
}

std::vector<uint8_t> WagoCoupler_750315::addChecksum(std::vector<uint8_t> bytes)const {
    uint16_t crc = ModbusCRC16::getCRC(bytes.data(), bytes.size());
    uint8_t low =  (uint8_t) ((crc) & 0xFF);
    uint8_t high = (uint8_t) ((crc >> 8) & 0xFF);
    bytes.push_back(low);
    bytes.push_back(high);
    return bytes;
}

std::vector<uint8_t> WagoCoupler_750315::hexlify(std::vector<uint8_t> bytes)const {
    std::vector<uint8_t> vec;
    for(uint8_t b : bytes) {
        switch(b & 0xF0) {
        case 0x00:
            vec.push_back('0');
            break;
        case 0x10:
            vec.push_back('1');
            break;
        case 0x20:
            vec.push_back('2');
            break;
        case 0x30:
            vec.push_back('3');
            break;
        case 0x40:
            vec.push_back('4');
            break;
        case 0x50:
            vec.push_back('5');
            break;
        case 0x60:
            vec.push_back('6');
            break;
        case 0x70:
            vec.push_back('7');
            break;
        case 0x80:
            vec.push_back('8');
            break;
        case 0x90:
            vec.push_back('9');
            break;
        case 0xA0:
            vec.push_back('A');
            break;
        case 0xB0:
            vec.push_back('B');
            break;
        case 0xC0:
            vec.push_back('C');
            break;
        case 0xD0:
            vec.push_back('D');
            break;
        case 0xE0:
            vec.push_back('E');
            break;
        case 0xF0:
            vec.push_back('F');
            break;
        }
        switch(b & 0x0F) {
        case 0x00:
            vec.push_back('0');
            break;
        case 0x01:
            vec.push_back('1');
            break;
        case 0x02:
            vec.push_back('2');
            break;
        case 0x03:
            vec.push_back('3');
            break;
        case 0x04:
            vec.push_back('4');
            break;
        case 0x05:
            vec.push_back('5');
            break;
        case 0x06:
            vec.push_back('6');
            break;
        case 0x07:
            vec.push_back('7');
            break;
        case 0x08:
            vec.push_back('8');
            break;
        case 0x09:
            vec.push_back('9');
            break;
        case 0x0A:
            vec.push_back('A');
            break;
        case 0x0B:
            vec.push_back('B');
            break;
        case 0x0C:
            vec.push_back('C');
            break;
        case 0x0D:
            vec.push_back('D');
            break;
        case 0x0E:
            vec.push_back('E');
            break;
        case 0x0F:
            vec.push_back('F');
            break;
        }
    }
    return vec;
}

/// implementation Wago_750530
///
///
bool Wago_750530::set(uint8_t state) {
    std::vector<uint8_t> cmd {coupler->modbusAddr, WRITE_BITS, 0x00, (uint8_t)(0x00 + outBits),0x00, 0x08, 0x01, state};
    // std::vector<uint8_t> dbgSendBytes = coupler->hexlify(coupler->addChecksum(cmd));
    // std::cout << std::string(dbgSendBytes.begin(), dbgSendBytes.end()) << std::endl;
    std::vector<uint8_t> recvBytes = sendRecv(coupler->addChecksum(cmd));
    // std::vector<uint8_t> dbgRecvBytes = coupler->hexlify(recvBytes);
    // std::cout << "recvBytes: " << std::string(dbgRecvBytes.begin(), dbgRecvBytes.end()) << std::endl;
    return recvBytes.size() == 8;
}

/// implementation Wago_750430
///
///
bool Wago_750430::get(uint8_t & state)const {
    std::vector<uint8_t> cmd {coupler->modbusAddr, READ_BIT, 0x00, (uint8_t)(0x00 + inBits),0x00, 0x08};
    // std::vector<uint8_t> dbgSendBytes = coupler->hexlify(coupler->addChecksum(cmd));
    // std::cout << "sendBytes: " << std::string(dbgSendBytes.begin(), dbgSendBytes.end()) << std::endl;
    std::vector<uint8_t> recvBytes = sendRecv(coupler->addChecksum(cmd));
    // std::vector<uint8_t> dbgRecvBytes = coupler->hexlify(recvBytes);
    // std::cout << "recvBytes: " << std::string(dbgRecvBytes.begin(), dbgRecvBytes.end()) << std::endl;
    if(recvBytes.size() == 6) {
        state = recvBytes[3];
        return true;
    } else {
        state = 0;
        return false;
    }
    return false;
}

/// implementation Wago_750515
///
///
bool Wago_750515::set(uint8_t state) {
    std::vector<uint8_t> cmd {coupler->modbusAddr, WRITE_BITS, 0x00, (uint8_t)(0x00 + outBits),0x00, 0x08, 0x01, state};
    // std::vector<uint8_t> dbgSendBytes = coupler->hexlify(coupler->addChecksum(cmd));
    // std::cout << std::string(dbgSendBytes.begin(), dbgSendBytes.end()) << std::endl;
    std::vector<uint8_t> recvBytes = sendRecv(coupler->addChecksum(cmd));
    // std::vector<uint8_t> dbgRecvBytes = coupler->hexlify(recvBytes);
    // std::cout << "recvBytes: " << std::string(dbgRecvBytes.begin(), dbgRecvBytes.end()) << std::endl;
    return recvBytes.size() == 8;
}

/// implementation Wago_750468
///
///
bool Wago_750468::get(uint8_t channel, double & voltage)const {
    std::vector<uint8_t> cmd {coupler->modbusAddr, READ_WORDS, 0x00, (uint8_t)(0x00 + inWords + channel),0x00, 0x01};
    // std::vector<uint8_t> dbgSendBytes = coupler->hexlify(coupler->addChecksum(cmd));
    // std::cout << std::string(dbgSendBytes.begin(), dbgSendBytes.end()) << std::endl;
    std::vector<uint8_t> recvBytes = sendRecv(coupler->addChecksum(cmd));
    // std::vector<uint8_t> dbgRecvBytes = coupler->hexlify(recvBytes);
    // std::cout << "recvBytes: " << std::string(dbgRecvBytes.begin(), dbgRecvBytes.end()) << std::endl;
    if(recvBytes.size() == 7) {
        int high = recvBytes[3];
        int low = recvBytes[4];
        int hexValue = (high << 8) | low;
        voltage = ((double) hexValue)/0x7FFF*10.0;
        return true;
    } else {
        voltage = 0;
        return false;
    }
    return false;
}

/// implementation Wago_750559
///
///
bool Wago_750559::set(uint8_t channel, double voltage) {
    int hexValue = (int)voltage/10*0x7FFF;
    uint8_t high = (byte)((hexValue >> 8) & 0xFF);
    uint8_t low = (byte)(hexValue & 0xFF);
    std::vector<uint8_t> cmd {coupler->modbusAddr, WRITE_WORD, 0x00, (uint8_t)(0x00 + outWords + channel), high, low};
    // std::vector<uint8_t> dbg = coupler->hexlify(coupler->addChecksum(cmd));
    // std::cout << std::string(dbg.begin(), dbg.end()) << std::endl;
    std::vector<uint8_t> recvBytes = sendRecv(coupler->addChecksum(cmd));
    return true; // TODO
}

} // wago750

