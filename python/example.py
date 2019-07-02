#!/cygdrive/C/Python27/python

import wago
import serial

comSettings = {
    "port" : 25,
    #"port" : "com10",
    "baudrate" : 19200,
    "parity" : serial.PARITY_EVEN,
    "stopbits" : serial.STOPBITS_ONE,
    "bytesize" : serial.EIGHTBITS,
    "timeout" : 0.1
}

koppler = wago.WagoKoppler(1,comSettings)
do8x24V = wago.Wago_750530()
di8x24V = wago.Wago_750430()
ao4x10V = wago.Wago_750559()
ai4x10V = wago.Wago_750468()
do4xNO = wago.Wago_750515()
koppler.add(do8x24V)
koppler.add(di8x24V)
koppler.add(ao4x10V)
koppler.add(ai4x10V)
koppler.add(do4xNO)
koppler.info()

do8x24V.on(0)
do4xNO.on(0)


