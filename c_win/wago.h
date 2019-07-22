#ifndef WAGO_INCLUDED
#define WAGO_INCLUDED

#include <stdint.h>
#include <stdbool.h>
#include <windows.h>

typedef enum { W315, W430, W530, W468, W515, W559} wago750_modtype_t;

typedef struct {
    uint8_t inBits;
    uint8_t outBits;
    uint8_t inWords;
    uint8_t outWords;
} addr_io_t;

typedef struct {
    wago750_modtype_t type;
    addr_io_t io;

} wago_module_t;

typedef struct {
    uint8_t com;
    HANDLE comHandle;
    addr_io_t sum_io;
    uint8_t num_used;
    uint8_t modbus_addr;
    wago_module_t *mods[10];
} plc_t;

bool plc_init(plc_t *);
bool plc_add(plc_t *, wago_module_t *);
bool plc_mod_set(plc_t *, wago_module_t *, uint8_t);
bool plc_mod_get(plc_t *, wago_module_t *, uint8_t *);
bool plc_mod_get_volt(plc_t *, wago_module_t *, uint8_t, double *);
void plc_print(plc_t *);

#endif

