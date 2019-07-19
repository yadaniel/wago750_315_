#include <stdio.h>
#include <stdlib.h>
#include <stdbool.h>
#include <assert.h>
#include "crc.h"
#include "wago.h"

void run() {
    plc_t wago = {.modbus_addr = 1, .com = 28, .comHandle = INVALID_HANDLE_VALUE};

    wago_module_t w315 = {.type = W315};
    wago_module_t w430 = {.type = W430};
    wago_module_t w530 = {.type = W530};
    wago_module_t w468 = {.type = W468};
    wago_module_t w515 = {.type = W515};

    bool ok = plc_init(&wago) &&
              plc_add(&wago, &w315) &&
              plc_add(&wago, &w430) &&
              plc_add(&wago, &w530) &&
              plc_add(&wago, &w468) &&
              plc_add(&wago, &w515);

    /* if(ok) { */
    /*     plc_print(&wago); */
    /*     exit(1); */
    /* } else { */
    /*     exit(2); */
    /* } */

    /* for(uint8_t val = 0; ok && (val<16); val += 1) { */
    /*     ok = (ok && plc_mod_set(&wago, &w515, val)); */
    /*     Sleep(1000); */
    /* } */
    /* ok = (ok && plc_mod_set(&wago, &w515, 0x00)); */

    /* for(uint16_t val = 0; ok && (val<256); val += 1) { */
    /*     ok = (ok && plc_mod_set(&wago, &w530, val)); */
    /*     Sleep(50); */
    /* } */
    /* ok = (ok && plc_mod_set(&wago, &w530, 0x00)); */

    /* uint8_t val = 0; */
    /* for(int i=0; i<512; i++) { */
    /*     ok = (ok && plc_mod_get(&wago, &w430, &val) && plc_mod_set(&wago, &w530, val)); */
    /*     printf("[%03i of 512]\r",i); */
    /*     fflush(stdout); */
    /*     Sleep(25); */
    /* } */

    for(int i=0; i<512; i++) {
        double voltage = 0;
        ok = (ok && plc_mod_get_volt(&wago, &w468, 0, &voltage));
        printf("[%03i of 512] => %.2f\r",i, voltage);
        fflush(stdout);
        Sleep(25);
    }

}

int main() {
    /* test_crc(); */
    run();
    return EXIT_SUCCESS;
}

