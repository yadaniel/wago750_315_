#ifndef CRC_INCLUDED
#define CRC_INCLUDED

#include <stdint.h>

uint16_t get_crc(uint8_t * input, int length);

void test_crc(void);

#endif

