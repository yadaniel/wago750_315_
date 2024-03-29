﻿using System;
using System.Text;
using System.IO.Ports;
using System.Windows.Forms;
using System.Collections.Generic;

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

/// <summary>
/// Description of ModbusCRC16
/// </summary>
public class ModbusCRC16 {
    private const UInt16 CRC_POLY = 0xA001;
    private const UInt16 CRC_START_MODBUS = 0xFFFF;
    private static readonly UInt16[] table = new UInt16[256];

    static ModbusCRC16() {
        UInt16 crc, c;
        for(UInt16 i=0; i<256; i++) {
            crc = 0;
            c   = i;
            for(UInt16 j=0; j<8; j++) {
                if( ((crc ^ c) & 0x0001) == 0x0001 ) {
                    crc = (UInt16) (( crc >> 1 ) ^ CRC_POLY);
                } else {
                    crc = (UInt16) (crc >> 1);
                }
                c = (UInt16) (c >> 1);
            }
            table[i] = crc;
        }
    }

    public UInt16 getCRC(byte[] input) {
        UInt16 crc, tmp, short_c;
        crc = CRC_START_MODBUS;
        for(int i=0; i<input.Length; i++) {
            short_c = (UInt16) (0x00FF & (UInt16) (input[i]));
            tmp     = (UInt16) (crc ^ short_c);
            crc     = (UInt16) ((crc >> 8) ^ table[ tmp & 0xff ]);
        }
        return crc;
    }
}

/// <summary>
/// Description of WagoModule
/// </summary>
public class WagoModule {
	public WagoModule(byte inResWords, byte inResBits, byte outResWords, byte outResBits) {
		this.inResWords = inResWords;
		this.inResBits = inResBits;
		this.outResWords = outResWords;
		this.outResBits = outResBits;
	}
	// each modules address space
    public readonly byte inResWords;
    public readonly byte inResBits;
    public readonly byte outResWords;
    public readonly byte outResBits;
    // position dependent address
    public byte inWords;
    public byte inBits;
    public byte outWords;
    public byte outBits;
    // constants
    public const byte READ_BIT = 0x01;
    public const byte READ_BITS = 0x01;
    public const byte WRITE_BIT = 0x05;
    public const byte WRITE_BITS = 0x0F;
    public const byte READ_WORDS = 0x03;
    public const byte WRITE_WORD = 0x06;
    // reference to main coupler
    public WagoCoupler_750315 coupler;
}

/// <summary>
/// Description of WagoCoupler_750315
/// </summary>
public class WagoCoupler_750315 : WagoModule {
    private static ModbusCRC16 crc = new ModbusCRC16();
	
    public byte modbusAddr = 0;
    private SerialPort serialPort;
    private List<WagoModule> moduleList = new List<WagoModule>();
	
    public WagoCoupler_750315(SerialPort serialPort, byte modbusAddr) : base(0,0,0,0) {
        this.serialPort = serialPort;
        this.modbusAddr = modbusAddr;
        this.coupler = this;
        moduleList.Add(this);
    }
	
    public void AddModule(WagoModule wagoModule) {
    	// assign added module the running address
    	wagoModule.inWords = inWords;
		wagoModule.inBits = inBits;
		wagoModule.outWords = outWords;
		wagoModule.outBits = outBits;
		wagoModule.coupler = this;
		// increment the running address
        inWords += wagoModule.inResWords;
        inBits += wagoModule.inResBits;
        outWords += wagoModule.outResWords;
        outBits += wagoModule.outResBits;
        moduleList.Add(wagoModule);
    }
	
    public static byte[] addChecksum(byte[] bytes) {
        UInt16 res = crc.getCRC(bytes);
        byte low = (byte) ((res) & 0xFF);
        byte high = (byte) ((res >> 8) & 0xFF);
        List<byte> lst = new List<byte>(bytes);
        lst.Add(low);
        lst.Add(high);
        return lst.ToArray();
    }
    
    public static byte[] hexlify(byte[] bytes) {
        List<byte> lst = new List<byte>();
        foreach(byte b in bytes) {
        	switch(b & 0xF0) {
            	case 0x00: lst.Add(Convert.ToByte('0')); break;
	            case 0x10: lst.Add(Convert.ToByte('1')); break;
            	case 0x20: lst.Add(Convert.ToByte('2')); break;
            	case 0x30: lst.Add(Convert.ToByte('3')); break;
            	case 0x40: lst.Add(Convert.ToByte('4')); break;
            	case 0x50: lst.Add(Convert.ToByte('5')); break;
            	case 0x60: lst.Add(Convert.ToByte('6')); break;
            	case 0x70: lst.Add(Convert.ToByte('7')); break;
            	case 0x80: lst.Add(Convert.ToByte('8')); break;
            	case 0x90: lst.Add(Convert.ToByte('9')); break;
            	case 0xA0: lst.Add(Convert.ToByte('A')); break;
            	case 0xB0: lst.Add(Convert.ToByte('B')); break;
            	case 0xC0: lst.Add(Convert.ToByte('C')); break;
            	case 0xD0: lst.Add(Convert.ToByte('D')); break;
            	case 0xE0: lst.Add(Convert.ToByte('E')); break;
            	case 0xF0: lst.Add(Convert.ToByte('F')); break;
            }
        	switch(b & 0x0F) {
            	case 0x00: lst.Add(Convert.ToByte('0')); break;
	            case 0x01: lst.Add(Convert.ToByte('1')); break;
            	case 0x02: lst.Add(Convert.ToByte('2')); break;
            	case 0x03: lst.Add(Convert.ToByte('3')); break;
            	case 0x04: lst.Add(Convert.ToByte('4')); break;
            	case 0x05: lst.Add(Convert.ToByte('5')); break;
            	case 0x06: lst.Add(Convert.ToByte('6')); break;
            	case 0x07: lst.Add(Convert.ToByte('7')); break;
            	case 0x08: lst.Add(Convert.ToByte('8')); break;
            	case 0x09: lst.Add(Convert.ToByte('9')); break;
            	case 0x0A: lst.Add(Convert.ToByte('A')); break;
            	case 0x0B: lst.Add(Convert.ToByte('B')); break;
            	case 0x0C: lst.Add(Convert.ToByte('C')); break;
            	case 0x0D: lst.Add(Convert.ToByte('D')); break;
            	case 0x0E: lst.Add(Convert.ToByte('E')); break;
            	case 0x0F: lst.Add(Convert.ToByte('F')); break;
            }
		}
        return lst.ToArray();
    }
}

/// <summary>
/// Description of Wago_750530
/// 8DO 24VDC/0.5A
/// </summary>
public class Wago_750530 : WagoModule {
	public Wago_750530() : base(inResWords: 0, inResBits: 0, outResWords: 0, outResBits: 8) {}
    public byte state = 0;

    public byte[] send() {
        byte[] cmd = new byte[] {coupler.modbusAddr, WRITE_BITS, 0x00, (byte) (0x00 + outBits),0x00, 0x08, 0x01, state};
        //byte[] dbg = WagoCoupler_750315.hexlify(WagoCoupler_750315.addChecksum(cmd));
        //MessageBox.Show(Encoding.ASCII.GetString(dbg));
        return WagoCoupler_750315.addChecksum(cmd);
    }
	
    public void parse(byte[] response) {
    }
}

/// <summary>
/// Description of Wago_750430
/// 8DI 24VDC
/// </summary>
public class Wago_750430 : WagoModule {
    public Wago_750430() : base(inResWords: 0, inResBits: 8, outResWords: 0, outResBits: 0) {} 
    public byte state = 0;

    public byte[] send() {
        byte[] cmd = new byte[] {coupler.modbusAddr, READ_BIT, 0x00, (byte) (0x00 + inBits),0x00, 0x08};
        //byte[] dbg = WagoCoupler_750315.hexlify(WagoCoupler_750315.addChecksum(cmd));
        //MessageBox.Show(Encoding.ASCII.GetString(dbg));
        return WagoCoupler_750315.addChecksum(cmd);
    }
	
    public void parse(byte[] response) {
    	if(response.Length == 6) {
    		state = response[3];
    	}
    }
}

/// <summary>
/// Description of Wago_750515
/// 4DO NO/2A
/// </summary>
public class Wago_750515 : WagoModule {
    public Wago_750515() : base(inResWords: 0, inResBits: 0, outResWords: 0, outResBits: 4) {}
    public byte state = 0 ;

    public byte[] send() {
        byte[] cmd = new byte[] {coupler.modbusAddr, WRITE_BITS, 0x00, (byte) (0x00 + outBits),0x00, 0x08, 0x01, state};
        //byte[] dbg = WagoCoupler_750315.hexlify(WagoCoupler_750315.addChecksum(cmd));
        //MessageBox.Show(Encoding.ASCII.GetString(dbg));
        return WagoCoupler_750315.addChecksum(cmd);
    }

    public void parse(byte[] response) {
    }
}

/// <summary>
/// Description of Wago_750468
/// 4AI 10V
/// </summary>
public class Wago_750468 : WagoModule {
    public Wago_750468() : base(inResWords: 2, inResBits: 0, outResWords: 0, outResBits: 0) {}
    public int state1;
    public int state2;
    public int state3;
    public int state4;

    public byte[] send(int channel) {
    	byte[] cmd = new byte[] {coupler.modbusAddr, READ_WORDS, 0x00, (byte) (0x00 + inWords + (channel-1)),0x00, 0x01};
        //byte[] dbg = WagoCoupler_750315.hexlify(WagoCoupler_750315.addChecksum(cmd));
        //MessageBox.Show(Encoding.ASCII.GetString(dbg));
        return WagoCoupler_750315.addChecksum(cmd);
    }
	
    public void parse(byte[] response, int channel) {
    	if(response.Length == 7) {
    		int high = response[3];
    		int low = response[4];
    		int hexValue = (high << 8) | low;
	    	switch(channel) {
	    		case 1: state1 = hexValue; break;
	    		case 2: state2 = hexValue; break;
	    		case 3: state3 = hexValue; break;
	    		case 4: state4 = hexValue; break;
	    		default: break;
	    	}
		}
    }

}

}
