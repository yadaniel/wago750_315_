#!/cygdrive/C/Python27/python

import crccheck
import serial
import time
import sys

comSettings = {
    "port" : 25,
    #"port" : "com10",
    "baudrate" : 19200,
    "parity" : serial.PARITY_EVEN,
    "stopbits" : serial.STOPBITS_ONE,
    "bytesize" : serial.EIGHTBITS,
    "timeout" : 0.1
}

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

class WagoKoppler(object):
    "modbus coupler 750315"
    def __init__(self, modbusAddr, comSettings):
        try:
            self.com = serial.Serial(**comSettings)
        except:
            print "com open exception"
        self.modbusAddr = modbusAddr
        self.modules = []
        self.inwords = 0
        self.inbits = 0
        self.outwords = 0
        self.outbits = 0
    def add(self, mod):
        mod.koppler = self  # back reference
        mod.inwords = self.inwords
        mod.inbits = self.inbits
        mod.outwords = self.outwords
        mod.outbits = self.outbits
        self.inwords += mod.InReserveWords
        self.inbits += mod.InReserveBits
        self.outwords += mod.OutReserveWords
        self.outbits += mod.OutReserveBits
        self.modules.append(mod)
    def info(self):
        for mod in self.modules:
            mod.info()
    def sendRecv(self,cmd):
        crc16 = crccheck.crc.CrcModbus(0x1021)
        crc = crc16.calc(cmd)
        cmd.append(crc & 0xFF)
        cmd.append((crc >> 8) & 0xFF)
        sendStr = map(lambda i: "%02X" % i, cmd)
        self.com.write(cmd)
        ans = self.com.read(100)
        recvStr = map(lambda i: "%02X" % ord(ans[i]),range(len(ans)))
        return sendStr, recvStr
    
class Wago_750530(object):
    "8DO 24VDC"
    InReserveWords = 0  # read by add method of koppler class
    InReserveBits = 0   # read by add method of koppler class
    OutReserveWords = 0 # read by add method of koppler class
    OutReserveBits = 8  # read by add method of koppler class
    def on(self,DO):
        # send: [0]=modbus address, [1]=function code, [2]=bit address high, [3]=bit address low, [4,5]=0xFF00 (special value to set bit)
        # recv: returns the same byte sequence 
        self.koppler.sendRecv([self.koppler.modbusAddr,WRITE_BIT,0x00,DO+self.outbits,0xFF,0x00])
    def off(self,DO):
        # send: [0]=modbus address, [1]=function code, [2]=bit address high, [3]=bit address low, [4,5]=0x0000 (special value to clear bit)
        # recv: returns same byte sequence 
        self.koppler.sendRecv([self.koppler.modbusAddr,WRITE_BIT,0x00,DO+self.outbits,0x00,0x00])
    def isOn(self,DO):
        # send: [0]=modbus address, [1]=function code, [2]=bit address high, [3]=bit address low, [4]=number of points high, [5]=number of points low
        # note: bit address must add offset 0x200 when reading back the set driving values
        # recv: [0]=modbus address, [1]=function code, [2]=byte count, [3]=data with requested number of points
        sendStr, recvStr = self.koppler.sendRecv([self.koppler.modbusAddr,READ_BIT,0x02,DO+self.outbits,0x00,0x01])
        return int(recvStr[3],16) == 1
    def isOff(self,DO):
        # send: [0]=modbus address, [1]=function code, [2]=bit address high, [3]=bit address low, [4]=number of points high, [5]=number of points low
        # note: bit address must add offset 0x200 when reading back the set driving values
        # recv: [0]=modbus address, [1]=function code, [2]=byte count, [3]=data with requested number of points
        sendStr, recvStr = self.koppler.sendRecv([self.koppler.modbusAddr,READ_BIT,0x02,DO+self.outbits,0x00,0x01])
        return int(recvStr[3],16) == 0
    def get(self):
        # send: [0]=modbus address, [1]=function code, [2]=bit address high, [3]=bit address low, [4]=number of points high, [5]=number of points low
        # note: bit address must add offset 0x200 when reading back the set driving values
        # recv: [0]=modbus address, [1]=function code, [2]=byte count, [3]=byte with requested number of points
        # note: not requested bits are zeroed (number of points do not fill the whole byte)
        sendStr, recvStr = self.koppler.sendRecv([self.koppler.modbusAddr,READ_BIT,0x02,0x00+self.outbits,0x00,0x08])
        return int(recvStr[3],16)
    def set(self,val):
        # send: [0]=modbus address, [1]=function code, [2]=bit address high, [3]=bit address low, [4]=number of points high, [5]=number of points low, [6]=number of bytes to write, [7]=sequence of byte values to set number of points
        # recv: [0]=modbus address, [1]=function code, [2]=bit address high, [3]=bit address low, [4]=number of points high, [5]=number of points low
        sendStr, recvStr = self.koppler.sendRecv([self.koppler.modbusAddr,WRITE_BITS,0x00,0x00+self.outbits,0x00,0x08,0x01,val])
    def info(self):
        print("WAGO 750-530 (8DO 24V) inwords=%2s, inbits=%3s, outwords=%2s, outbits=%3s" % (self.inwords, self.inbits, self.outwords, self.outbits))

class Wago_750430(object):
    "8DI 24VDC"
    InReserveWords = 0  # read by add method of koppler class
    InReserveBits = 8   # read by add method of koppler class
    OutReserveWords = 0 # read by add method of koppler class
    OutReserveBits = 0  # read by add method of koppler class
    def isOn(self,DI):
        # send: [0]=modbus address, [1]=function code, [2]=bit address high, [3]=bit address low, [4]=number of points high, [5]=number of points low
        # recv: [0]=modbus address, [1]=function code, [2]=byte count, [3]=data with requested number of points
        sendStr, recvStr = self.koppler.sendRecv([self.koppler.modbusAddr,READ_BIT,0x00,DI+self.inbits,0x00,0x01])
        return int(recvStr[3],16) == 1
    def isOff(self,DI):
        # send: [0]=modbus address, [1]=function code, [2]=bit address high, [3]=bit address low, [4]=number of points high, [5]=number of points low
        # recv: [0]=modbus address, [1]=function code, [2]=byte count, [3]=data with requested number of points
        sendStr, recvStr = self.koppler.sendRecv([self.koppler.modbusAddr,READ_BIT,0x00,DI+self.inbits,0x00,0x01])
        return int(recvStr[3],16) == 0
    def get(self):
        # send: [0]=modbus address, [1]=function code, [2]=bit address high, [3]=bit address low, [4]=number of points high, [5]=number of points low
        # recv: [0]=modbus address, [1]=function code, [2]=byte count, [3]=byte with requested number of points
        # note: not requested bits are zeroed (number of points do not fill the whole byte)
        sendStr, recvStr = self.koppler.sendRecv([self.koppler.modbusAddr,READ_BIT,0x00,0x00+self.inbits,0x00,0x08])
        return int(recvStr[3],16)
    def info(self):
        print("WAGO 750-430 (8DI 24V) inwords=%2s, inbits=%3s, outwords=%2s, outbits=%3s" % (self.inwords, self.inbits, self.outwords, self.outbits))

class Wago_750559(object):
    "4AO 10V"
    InReserveWords = 0  # read by add method of koppler class
    InReserveBits = 0   # read by add method of koppler class
    OutReserveWords = 2 # read by add method of koppler class
    OutReserveBits = 0  # read by add method of koppler class
    def get(self,channel):
        # send: [0]=modbus address, [1]=function code, [2]=word address high, [3]=word address low, [4]=number of points high, [5]=number of points low
        # recv: [0]=modbus address, [1]=function code, [2]=byte count, [3]=first byte high, [4]=first byte low, [5]=second byte high, [6]=second byte high, ...
        #sendStr, recvStr = self.koppler.sendRecv([self.koppler.modbusAddr,READ_WORDS,0x02,0x00+self.outwords,0x00,0x01])
        sendStr, recvStr = self.koppler.sendRecv([self.koppler.modbusAddr,READ_WORDS,0x02,0x00+self.outwords+channel,0x00,0x01])
        return int(recvStr[3],16)<<8 | int(recvStr[4],16)
    def set(self,channel,val):
        # send: [0]=modbus address, [1]=function code, [2]=word address high, [3]=word address low, [4]=byte high, [5]=byte low
        # recv: returns the same byte sequence
        #sendStr, recvStr = self.koppler.sendRecv([self.koppler.modbusAddr,WRITE_WORD,0x00,0x00+self.outwords,(val>>8) & 0xFF, val & 0xFF])
        sendStr, recvStr = self.koppler.sendRecv([self.koppler.modbusAddr,WRITE_WORD,0x00,0x00+self.outwords + channel,(val>>8) & 0xFF, val & 0xFF])
    def setVolt(self,channel,volt):
        self.set(channel,int(volt/10.0*0x7FFF))
    def getVolt(self,channel):
        return float(self.get(channel))/0x7FFF*10
    def info(self):
        print("WAGO 750-559 (4AO 10V) inwords=%2s, inbits=%3s, outwords=%2s, outbits=%3s" % (self.inwords, self.inbits, self.outwords, self.outbits))

class Wago_750468(object):
    "4AI 10V"
    InReserveWords = 2  # read by add method of koppler class
    InReserveBits = 0   # read by add method of koppler class
    OutReserveWords = 0 # read by add method of koppler class
    OutReserveBits = 0  # read by add method of koppler class
    def get(self,channel):
        # send: [0]=modbus address, [1]=function code, [2]=word address high, [3]=word address low, [4]=number of points high, [5]=number of points low
        # recv: [0]=modbus address, [1]=function code, [2]=byte count, [3]=first byte high, [4]=first byte low, [5]=second byte high, [6]=second byte high, ...
        #sendStr, recvStr = self.koppler.sendRecv([self.koppler.modbusAddr,READ_WORDS,0x02,0x00+self.outwords,0x00,0x01])
        sendStr, recvStr = self.koppler.sendRecv([self.koppler.modbusAddr,READ_WORDS,0x00,0x00+self.inwords+channel,0x00,0x01])
        return int(recvStr[3],16)<<8 | int(recvStr[4],16)
    def getVolt(self,channel):
        return float(self.get(channel))/0x7FFF*10
    def info(self):
        print("WAGO 750-468 (4AI 10V) inwords=%2s, inbits=%3s, outwords=%2s, outbits=%3s" % (self.inwords, self.inbits, self.outwords, self.outbits))

class Wago_750515(object):
    "4DO NO/2A"
    InReserveWords = 0  # read by add method of koppler class
    InReserveBits = 0   # read by add method of koppler class
    OutReserveWords = 0 # read by add method of koppler class
    OutReserveBits = 4  # read by add method of koppler class
    def on(self,DO):
        # send: [0]=modbus address, [1]=function code, [2]=bit address high, [3]=bit address low, [4,5]=0xFF00 (special value to set bit)
        # recv: returns same byte sequence 
        self.koppler.sendRecv([self.koppler.modbusAddr,WRITE_BIT,0x00,DO+self.outbits,0xFF,0x00])
    def off(self,DO):
        # send: [0]=modbus address, [1]=function code, [2]=bit address high, [3]=bit address low, [4,5]=0x0000 (special value to clear bit)
        # recv: returns same byte sequence 
        self.koppler.sendRecv([self.koppler.modbusAddr,WRITE_BIT,0x00,DO+self.outbits,0x00,0x00])
    def isOn(self,DO):
        # send: [0]=modbus address, [1]=function code, [2]=bit address high, [3]=bit address low, [4]=number of points high, [5]=number of points low
        # note: bit address must add offset 0x200 when reading back the set driving values
        # recv: [0]=modbus address, [1]=function code, [2]=byte count, [3]=data with requested number of points
        sendStr, recvStr = self.koppler.sendRecv([self.koppler.modbusAddr,READ_BIT,0x02,DO+self.outbits,0x00,0x01])
        return int(recvStr[3],16) == 1
    def isOff(self,DO):
        # send: [0]=modbus address, [1]=function code, [2]=bit address high, [3]=bit address low, [4]=number of points high, [5]=number of points low
        # note: bit address must add offset 0x200 when reading back the set driving values
        # recv: [0]=modbus address, [1]=function code, [2]=byte count, [3]=data with requested number of points
        sendStr, recvStr = self.koppler.sendRecv([self.koppler.modbusAddr,READ_BIT,0x02,DO+self.outbits,0x00,0x01])
        return int(recvStr[3],16) == 0
    def get(self):
        # send: [0]=modbus address, [1]=function code, [2]=bit address high, [3]=bit address low, [4]=number of points high, [5]=number of points low
        # note: bit address must add offset 0x200 when reading back the set driving values
        # recv: [0]=modbus address, [1]=function code, [2]=byte count, [3]=byte with requested number of points
        # note: not requested bits are zeroed (number of points do not fill the whole byte)
        sendStr, recvStr = self.koppler.sendRecv([self.koppler.modbusAddr,READ_BIT,0x02,0x00+self.outbits,0x00,0x08])
        return int(recvStr[3],16)
    def set(self,val):
        # send: [0]=modbus address, [1]=function code, [2]=bit address high, [3]=bit address low, [4]=number of points high, [5]=number of points low, [6]=number of bytes to write, [7]=sequence of byte values to set number of points
        # recv: [0]=modbus address, [1]=function code, [2]=bit address high, [3]=bit address low, [4]=number of points high, [5]=number of points low
        sendStr, recvStr = self.koppler.sendRecv([self.koppler.modbusAddr,WRITE_BITS,0x00,0x00+self.outbits,0x00,0x08,0x01,val])
    def info(self):
        print("WAGO 750-515 (4DO NO) inwords=%2s, inbits=%3s, outwords=%2s, outbits=%3s" % (self.inwords, self.inbits, self.outwords, self.outbits))

###
###
###

def test_Wago_750530(mod):
    ### test set, get methods
    mod.set(0x0F); time.sleep(0.1); assert mod.get() == 0x0F
    mod.set(0xF0); time.sleep(0.1); assert mod.get() == 0xF0
    mod.set(0xFF); time.sleep(0.1); assert mod.get() == 0xFF
    mod.set(0x00); time.sleep(0.1); assert mod.get() == 0x00
    ### test on, isOn methods
    for do in range(8):
        mod.on(do); time.sleep(0.1); assert mod.isOn(do)
    ### test off, isOff methods
    for do in range(8):
        mod.off(do); time.sleep(0.1); assert mod.isOff(do)
    ###
    print "test_Wago_750530 passed"
    sys.stdout.flush()

def test_Wago_750430(modOut,mod):
    ### note: connect modOut DO0-7 to mod DI0-7 (1:1)
    ### test get method
    modOut.set(0x0F); time.sleep(0.1); assert mod.get() == 0x0F
    modOut.set(0xF0); time.sleep(0.1); assert mod.get() == 0xF0
    modOut.set(0xFF); time.sleep(0.1); assert mod.get() == 0xFF
    modOut.set(0x00); time.sleep(0.1); assert mod.get() == 0x00
    ### test isOn method
    for di in range(8):
        modOut.on(DO=di); time.sleep(0.1); assert mod.isOn(di)
    ### test isOff method
    for di in range(8):
        modOut.off(DO=di); time.sleep(0.1); assert mod.isOff(di)
    ###
    print "test_Wago_750430 passed"
    sys.stdout.flush()

def test_Wago_750515(mod):
    ### test set, get methods
    mod.set(0x03); time.sleep(0.5); assert mod.get() == 0x03
    mod.set(0x0C); time.sleep(0.5); assert mod.get() == 0x0C
    mod.set(0x03); time.sleep(0.5); assert mod.get() == 0x03
    mod.set(0x0F); time.sleep(0.5); assert mod.get() == 0x0F
    mod.set(0x00); time.sleep(0.5); assert mod.get() == 0x00
    ### test on, isOn methods
    for do in range(4):
        mod.on(do); time.sleep(0.5); assert mod.isOn(do)
    ### test off, isOff methods
    for do in range(4):
        mod.off(do); time.sleep(0.5); assert mod.isOff(do)
    ###
    print "test_Wago_750515 passed"
    sys.stdout.flush()

def test_Wago_750559(mod):
    ### test set, get methods
    for channel in range(4):
        mod.set(channel, 0x0000); time.sleep(1); assert mod.get(channel) == 0x0000
        mod.set(channel, 0x00FF); time.sleep(1); assert mod.get(channel) == 0x00FF
        mod.set(channel, 0x0FFF); time.sleep(1); assert mod.get(channel) == 0x0FFF
        mod.set(channel, 0x7FFF); time.sleep(1); assert mod.get(channel) == 0x7FFF
        mod.set(channel, 0x0000); time.sleep(1); assert mod.get(channel) == 0x0000
    # upper left to 1.25V
    mod.set(0, 0x1000); time.sleep(1); assert mod.get(0) == 0x1000
    # upper right to 2.50V
    mod.set(1, 0x2000); time.sleep(1); assert mod.get(1) == 0x2000
    # lower left to 3.75V
    mod.set(2, 0x3000); time.sleep(1); assert mod.get(2) == 0x3000
    # lower right to 5.0V
    mod.set(3, 0x4000); time.sleep(1); assert mod.get(3) == 0x4000
    ### test setVolt, getVolt method
    mod.setVolt(0,1.0)
    mod.setVolt(1,2.0)
    mod.setVolt(2,3.0)
    mod.setVolt(3,4.0)
    print "    1.00V expected ...  got %2.2f" % mod.getVolt(0)
    print "    2.00V expected ...  got %2.2f" % mod.getVolt(1)
    print "    3.00V expected ...  got %2.2f" % mod.getVolt(2)
    print "    4.00V expected ...  got %2.2f" % mod.getVolt(3)
    ###
    print "test_Wago_750559 passed"
    sys.stdout.flush()

def test_Wago_750468(modOut,mod):
    ### note connect modOut AO0-3 to mod AI0-3 (1:1)
    ### test get method
    for channel in range(4):
        modOut.set(channel, 0x0000); time.sleep(1); assert abs(mod.get(channel) - 0x0000) < 0x00FF
        modOut.set(channel, 0x00FF); time.sleep(1); assert abs(mod.get(channel) - 0x00FF) < 0x00FF
        modOut.set(channel, 0x0FFF); time.sleep(1); assert abs(mod.get(channel) - 0x0FFF) < 0x00FF
        modOut.set(channel, 0x7FFF); time.sleep(1); assert abs(mod.get(channel) - 0x7FFF) < 0x00FF
        modOut.set(channel, 0x0000); time.sleep(1); assert abs(mod.get(channel) - 0x0000) < 0x00FF
    # upper left to 1.25V
    modOut.set(0, 0x1000); time.sleep(1); assert abs(mod.get(0) == 0x1000) < 0x00FF
    # upper right to 2.50V
    modOut.set(1, 0x2000); time.sleep(1); assert abs(mod.get(1) == 0x2000) < 0x00FF
    # lower left to 3.25V
    modOut.set(2, 0x3000); time.sleep(1); assert abs(mod.get(2) == 0x3000) < 0x00FF
    # lower right to 5.0V
    modOut.set(3, 0x4000); time.sleep(1); assert abs(mod.get(3) == 0x4000) < 0x00FF
    ### test getVolt method
    print "    1.25V expected ...  got %2.2f" % mod.getVolt(0)
    print "    2.50V expected ...  got %2.2f" % mod.getVolt(1)
    print "    3.75V expected ...  got %2.2f" % mod.getVolt(2)
    print "    5.00V expected ...  got %2.2f" % mod.getVolt(3)
    ###
    print "test_Wago_750468 passed"
    sys.stdout.flush()

if __name__ == "__main__":
    # default settings
    comSettings = {
        "port" : 25,
        #"port" : "com10",
        "baudrate" : 19200,
        "parity" : serial.PARITY_EVEN,
        "stopbits" : serial.STOPBITS_ONE,
        "bytesize" : serial.EIGHTBITS,
        "timeout" : 0.1
    }
    koppler = WagoKoppler(1, comSettings)
    do8x24V_1 = Wago_750530()
    di8x24V_1 = Wago_750430()
    do8x24V_2 = Wago_750530()
    di8x24V_2 = Wago_750430()
    ao4x10V = Wago_750559()
    ai4x10V = Wago_750468()
    do4xNO_1 = Wago_750515()
    do4xNO_2 = Wago_750515()
    koppler.add(do8x24V_1)
    koppler.add(do8x24V_2)
    koppler.add(di8x24V_1)
    koppler.add(di8x24V_2)
    koppler.add(ao4x10V)
    koppler.add(ai4x10V)
    koppler.add(do4xNO_1)
    koppler.add(do4xNO_2)
    koppler.info()
    ###
    t0 = time.time()
    test_Wago_750530(do8x24V_1)
    test_Wago_750530(do8x24V_2)
    test_Wago_750515(do4xNO_1)
    test_Wago_750515(do4xNO_2)
    test_Wago_750559(ao4x10V)
    test_Wago_750430(do8x24V_2,di8x24V_2)
    test_Wago_750468(ao4x10V,ai4x10V)
    t1 = time.time()
    print "tests finished in %.2f seconds" % (t1-t0)

