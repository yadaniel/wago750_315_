#include "wago.h"
#include "crc.h"
#include <stdio.h>

const uint8_t READ_BIT = 0x01;
const uint8_t READ_BITS = 0x01;
const uint8_t WRITE_BIT = 0x05;
const uint8_t WRITE_BITS = 0x0F;
const uint8_t READ_WORDS = 0x03;
const uint8_t WRITE_WORD = 0x06;

bool plc_init(plc_t * self) {
    self->num_used = 0;
    self->sum_io = (addr_io_t) {
        .inBits = 0, .outBits = 0, .inWords = 0, .outWords = 0
    };

    char comName[16] = {0};
    sprintf(&comName[0], "\\\\.\\COM%i", self->com);

    self->comHandle = CreateFile(&comName[0], GENERIC_READ | GENERIC_WRITE, 0, 0, OPEN_EXISTING, FILE_ATTRIBUTE_NORMAL, 0);
    if(self->comHandle != INVALID_HANDLE_VALUE) {

        // Do some basic settings
        DCB serialParams = { 0 };
        serialParams.DCBlength = sizeof(serialParams);
        GetCommState(self->comHandle, &serialParams);
        serialParams.BaudRate = 19200;
        serialParams.ByteSize = 8;
        serialParams.StopBits = ONESTOPBIT;
        serialParams.Parity = EVENPARITY;
        SetCommState(self->comHandle, &serialParams);

        // Set timeouts
        COMMTIMEOUTS timeout = { 0 };
        timeout.ReadIntervalTimeout = 50;
        timeout.ReadTotalTimeoutConstant = 50;
        timeout.ReadTotalTimeoutMultiplier = 50;
        timeout.WriteTotalTimeoutConstant = 50;
        timeout.WriteTotalTimeoutMultiplier = 10;
        SetCommTimeouts(self->comHandle, &timeout);

        return true;
    }

    return false;
}

bool plc_add(plc_t * self, wago_module_t * mod) {
    if(self->num_used < 10) {
        self->mods[self->num_used] = mod;
        self->num_used += 1;
        mod->io = self->sum_io;
        uint8_t inBits = 0;
        uint8_t outBits = 0;
        uint8_t inWords = 0;
        uint8_t outWords = 0;
        switch(mod->type) {
        case W315:
            break;
        case W430:
            inBits = 8;
            break;
        case W530:
            outBits = 8;
            break;
        case W468:
            inWords = 2;
            break;
        case W515:
            outBits = 4;
            break;
        case W559:
            outWords = 2;
            break;
        default:
            return false;
        }
        self->sum_io.inBits += inBits;
        self->sum_io.outBits += outBits;
        self->sum_io.inWords += inWords;
        self->sum_io.outWords += outWords;

        return true;
    }

    return false;
}

bool plc_mod_set(plc_t * self, wago_module_t * mod, uint8_t val) {
    uint8_t send_data[32] = {};
    uint8_t send_length = 0;
    uint16_t crc = 0;
    switch(mod->type) {
    case W315:
        return false;
    case W430:
        return false;
    case W468:
        return false;
    case W559:
        return false;
    case W515: /* fallthrough */
    case W530:
        send_length = 0;
        send_data[send_length++] = self->modbus_addr;
        send_data[send_length++] = WRITE_BITS;
        send_data[send_length++] = 0x00;
        send_data[send_length++] = 0x00 + mod->io.outBits;
        send_data[send_length++] = 0x00;
        send_data[send_length++] = 0x08;
        send_data[send_length++] = 0x01;
        send_data[send_length++] = val;
        crc = get_crc(&send_data[0], send_length);
        send_data[send_length++] = crc & 0xFF;
        send_data[send_length++] = (crc >> 8) & 0xFF;
        break;
    default:
        return false;
    }

    // send
    HANDLE handle = self->comHandle;
    LPCVOID outBuffer = &send_data[0];
    DWORD size = send_length;
    DWORD written = 0;
    LPOVERLAPPED unknown = NULL;
    WriteFile(handle, outBuffer, size, &written, unknown);

    // receive
    uint8_t recvData[100] = {};
    DWORD read = 0;
    ReadFile(handle, (void *)&recvData[0], 100, &read, unknown);

    return true;
}

bool plc_mod_get(plc_t * self, wago_module_t * mod, uint8_t * val) {
    uint8_t send_data[32] = {};
    uint8_t send_length = 0;
    uint16_t crc = 0;
    switch(mod->type) {
    case W315:
        break;
    case W430:
        send_length = 0;
        send_data[send_length++] = self->modbus_addr;
        send_data[send_length++] = READ_BIT;
        send_data[send_length++] = 0x00;
        send_data[send_length++] = 0x00 + mod->io.inBits;
        send_data[send_length++] = 0x00;
        send_data[send_length++] = 0x08;
        crc = get_crc(&send_data[0], send_length);
        send_data[send_length++] = crc & 0xFF;
        send_data[send_length++] = (crc >> 8) & 0xFF;
        break;
    case W530:
        break;
    case W468:
        break;
    case W559:
        break;
    case W515:
        break;
    default:
        return false;
    }

    // send
    HANDLE handle = self->comHandle;
    LPCVOID outBuffer = &send_data[0];
    DWORD size = send_length;
    DWORD written = 0;
    LPOVERLAPPED unknown = NULL;
    WriteFile(handle, outBuffer, size, &written, unknown);

    // receive
    uint8_t recvData[100] = {};
    DWORD read = 0;
    ReadFile(handle, (void *)&recvData[0], 100, &read, unknown);

    switch(mod->type) {
    case W315:
        break;
    case W430:
        if(read == 6) {
            *val = recvData[3];
        }
        break;
    case W530:
        break;
    case W468:
        break;
    case W559:
        break;
    case W515:
        break;
    default:
        return false;
    }

    return true;
}

bool plc_mod_get_volt(plc_t * self, wago_module_t * mod, uint8_t channel, double * val) {
    uint8_t send_data[32] = {};
    uint8_t send_length = 0;
    uint16_t crc = 0;
    switch(mod->type) {
    case W468:
        send_length = 0;
        send_data[send_length++] = self->modbus_addr;
        send_data[send_length++] = READ_WORDS;
        send_data[send_length++] = 0x00;
        send_data[send_length++] = 0x00 + mod->io.inWords + channel;
        send_data[send_length++] = 0x00;
        send_data[send_length++] = 0x01;
        crc = get_crc(&send_data[0], send_length);
        send_data[send_length++] = crc & 0xFF;
        send_data[send_length++] = (crc >> 8) & 0xFF;
        break;
    case W315:
    case W530:
    case W430:
    case W515:
        break;
    case W559:  // TODO
        break;
    default:
        return false;
    }

    // send
    HANDLE handle = self->comHandle;
    LPCVOID outBuffer = &send_data[0];
    DWORD size = send_length;
    DWORD written = 0;
    LPOVERLAPPED unknown = NULL;
    WriteFile(handle, outBuffer, size, &written, unknown);

    // receive
    uint8_t recvData[100] = {};
    DWORD read = 0;
    ReadFile(handle, (void *)&recvData[0], 100, &read, unknown);

    switch(mod->type) {
    case W468:
        if(read == 7) {
            uint8_t high = recvData[3];
            uint8_t low = recvData[4];
            uint16_t hexValue = (high << 8) | low;
            double voltage = ((double) hexValue)/0x7FFF*10.0;
            *val = voltage;
        }
        break;
    case W315:
    case W530:
    case W430:
    case W515:
        break;
    case W559:
        // TODO
        break;
    default:
        return false;
    }

    return true;
}

bool plc_mod_set_volt(plc_t * self, wago_module_t * mod, uint8_t channel, double val) {
    uint8_t send_data[32] = {};
    uint8_t send_length = 0;
    uint16_t crc = 0;
    uint16_t hexValue = (uint16_t)val/10*0x7FFF;
    uint8_t high = (byte)((hexValue >> 8) & 0xFF);
    uint8_t low = (byte)(hexValue & 0xFF);
    switch(mod->type) {
    case W559:
        send_length = 0;
        send_data[send_length++] = self->modbus_addr;
        send_data[send_length++] = WRITE_WORD;
        send_data[send_length++] = 0x00;
        send_data[send_length++] = 0x00 + mod->io.outWords + channel;
        send_data[send_length++] = high;
        send_data[send_length++] = low;
        crc = get_crc(&send_data[0], send_length);
        send_data[send_length++] = crc & 0xFF;
        send_data[send_length++] = (crc >> 8) & 0xFF;
        break;
    case W315:
    case W530:
    case W430:
    case W515:
    case W468:
        break;
    default:
        return false;
    }

    // send
    HANDLE handle = self->comHandle;
    LPCVOID outBuffer = &send_data[0];
    DWORD size = send_length;
    DWORD written = 0;
    LPOVERLAPPED unknown = NULL;
    WriteFile(handle, outBuffer, size, &written, unknown);

    // receive
    uint8_t recvData[100] = {};
    DWORD read = 0;
    ReadFile(handle, (void *)&recvData[0], 100, &read, unknown);

    switch(mod->type) {
    case W559:
        // TODO
        break;
    case W315:
    case W530:
    case W430:
    case W515:
    case W468:
        break;
    default:
        return false;
    }

    return true;
}

void plc_print(plc_t * self) {
    wago_module_t * mod = NULL;
    for(int i=0; i<self->num_used; i++) {
        mod = self->mods[i];
        printf("mods[%i].io => %i,%i,%i,%i\n", i, mod->io.inBits, mod->io.outBits, mod->io.inWords, mod->io.outWords);
    }
}


