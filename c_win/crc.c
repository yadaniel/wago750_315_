#include "crc.h"
#include <stdbool.h>
#include <stdlib.h>
#include <stdio.h>
#include <assert.h>

static uint16_t table[256] = {};
static bool initialized = false;
const uint16_t CRC_POLY = 0xA001;
const uint16_t CRC_START_MODBUS = 0xFFFF;

static void init_table() {
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

uint16_t get_crc(uint8_t * input, int length) {
    init_table();
    uint16_t crc, tmp, short_c;
    crc = CRC_START_MODBUS;
    if(input != NULL) {
        for(int i=0; i<length; i++) {
            short_c = (uint16_t) (0x00FF & (uint16_t) (input[i]));
            tmp     = (uint16_t) (crc ^ short_c);
            crc     = (uint16_t) ((crc >> 8) ^ table[ tmp & 0xff ]);
        }
    }
    return crc;
}


void test_crc(void) {
    uint8_t data[] = {0,1,2,3,4,5,6,7,8,9};
    uint16_t crc = get_crc(&data[0], 10);
    printf("0x%02X\n", crc);
    assert(crc == 0x4574);
}

