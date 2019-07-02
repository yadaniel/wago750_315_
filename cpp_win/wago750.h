#ifndef WAGO750_INCLUDED
#define WAGO750_INCLUDED

#include <vector>
#include <cstdint>
#include <iostream>
#include <windows.h>

namespace wago750 {

class ModbusCRC16 {
private:
    static const uint16_t CRC_POLY = 0xA001;
    static const uint16_t CRC_START_MODBUS = 0xFFFF;
    static uint16_t table[256];
    static bool initialized;
    static void init();
public:
    static uint16_t getCRC(uint8_t * input, int length);
};

// forward declaration
class WagoCoupler_750315;

class WagoModule {
public:
    WagoModule(uint8_t inResWords, uint8_t inResBits, uint8_t outResWords, uint8_t outResBits) :
        InResWords(inResWords),
        InResBits(inResBits),
        OutResWords(outResWords),
        OutResBits(outResBits)
    {}
    // each modules address space
    const uint8_t InResWords;
    const uint8_t InResBits;
    const uint8_t OutResWords;
    const uint8_t OutResBits;
    // position dependent address
    uint8_t inWords = 0;
    uint8_t inBits = 0;
    uint8_t outWords = 0;
    uint8_t outBits = 0;
    // constants
    const uint8_t READ_BIT = 0x01;
    const uint8_t READ_BITS = 0x01;
    const uint8_t WRITE_BIT = 0x05;
    const uint8_t WRITE_BITS = 0x0F;
    const uint8_t READ_WORDS = 0x03;
    const uint8_t WRITE_WORD = 0x06;
    // reference to main coupler
    WagoCoupler_750315 * coupler = nullptr;
    // send to and receive from COM
    std::vector<uint8_t> sendRecv(std::vector<uint8_t> sendBytes)const;
};

/// modbus coupler module
class WagoCoupler_750315 : public WagoModule {
public:
    WagoCoupler_750315(uint8_t serialPortInit, uint8_t modbusAddrInit);
    ~WagoCoupler_750315();
    void addModule(WagoModule * wagoModule);
    std::vector<uint8_t> addChecksum(std::vector<uint8_t> bytes)const;
    std::vector<uint8_t> hexlify(std::vector<uint8_t> bytes)const;
    std::vector<WagoModule*> moduleList;
    uint8_t serialPort;
    uint8_t modbusAddr;
    HANDLE serialHandle = INVALID_HANDLE_VALUE;
};

/// 8DO 24VDC/0.5A
class Wago_750530 : public WagoModule {
public:
    Wago_750530() : WagoModule(0,0,0,8) {}
    bool set(uint8_t state);
};

/// 8DI 24VDC
class Wago_750430 : public WagoModule {
public:
    Wago_750430() : WagoModule(0,8,0,0) {}
    bool get(uint8_t & state)const;
};

/// 4DO NO/2A
class Wago_750515 : public WagoModule {
public:
    Wago_750515() : WagoModule(0,0,0,4) {}
    bool set(uint8_t state);
};

/// 4AI 10V
class Wago_750468 : public WagoModule {
public:
    Wago_750468() : WagoModule(2,0,0,0) {}
    bool get(uint8_t channel, double & voltage)const;
};

/// 4AO 10V
class Wago_750559 : public WagoModule {
public:
    Wago_750559() : WagoModule(0,0,2,0) {}
    bool set(uint8_t channel, double voltage);
};

}

#endif

