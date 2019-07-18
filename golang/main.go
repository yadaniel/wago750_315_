package main

import "fmt"
import "log"
import "time"
import "github.com/tarm/serial"

const READ_BIT uint8 = 0x01
const READ_BITS uint8 = 0x01
const WRITE_BIT uint8 = 0x05
const WRITE_BITS uint8 = 0x0F
const READ_WORDS uint8 = 0x03
const WRITE_WORD uint8 = 0x06

type AddrIO struct {
	INRESWORDS  uint8
	INRESBITS   uint8
	OUTRESWORDS uint8
	OUTRESBITS  uint8
}

type WagoModule struct {
	serial_port    serial.Port
	modbus_address uint8
}

// modbus coupler
type Wago750315 struct {
	res_io AddrIO
	io     AddrIO
}

func (wago *Wago750315) init(addr *AddrIO) {
	wago.res_io.INRESWORDS = 0
	wago.res_io.INRESBITS = 0
	wago.res_io.OUTRESWORDS = 0
	wago.res_io.OUTRESBITS = 0
	wago.io = *addr
	addr.INRESWORDS += wago.res_io.INRESWORDS
	addr.INRESBITS += wago.res_io.INRESBITS
	addr.OUTRESWORDS += wago.res_io.OUTRESWORDS
	addr.OUTRESBITS += wago.res_io.OUTRESBITS
}

// 8DO 24VDC/0.5A
type Wago750530 struct {
	res_io AddrIO
	io     AddrIO
}

func (wago *Wago750530) init(addr *AddrIO) {
	wago.res_io.INRESWORDS = 0
	wago.res_io.INRESBITS = 0
	wago.res_io.OUTRESWORDS = 0
	wago.res_io.OUTRESBITS = 8
	wago.io = *addr
	addr.INRESWORDS += wago.res_io.INRESWORDS
	addr.INRESBITS += wago.res_io.INRESBITS
	addr.OUTRESWORDS += wago.res_io.OUTRESWORDS
	addr.OUTRESBITS += wago.res_io.OUTRESBITS
}

func (wago *Wago750530) set(value uint8) []uint8 {
	cmd := []uint8{1, WRITE_BITS, 0x00, 0x00 + wago.io.OUTRESBITS, 0x00, 0x08, 0x01, value}
	crc := checksum(cmd)
	cmd = append(cmd, uint8(crc&0xFF), uint8(crc>>8))
	fmt.Printf("%v\n", cmd)
	return cmd
}

// 8DI 24VDC
type Wago750430 struct {
	res_io AddrIO
	io     AddrIO
}

func (wago *Wago750430) init(addr *AddrIO) {
	wago.res_io.INRESWORDS = 0
	wago.res_io.INRESBITS = 8
	wago.res_io.OUTRESWORDS = 0
	wago.res_io.OUTRESBITS = 0
	wago.io = *addr
	addr.INRESWORDS += wago.res_io.INRESWORDS
	addr.INRESBITS += wago.res_io.INRESBITS
	addr.OUTRESWORDS += wago.res_io.OUTRESWORDS
	addr.OUTRESBITS += wago.res_io.OUTRESBITS
}

func (wago *Wago750430) get() []uint8 {
	cmd := []uint8{1, READ_BIT, 0x00, 0x00 + wago.io.INRESBITS, 0x00, 0x08}
	crc := checksum(cmd)
	cmd = append(cmd, uint8(crc&0xFF), uint8(crc>>8))
	fmt.Printf("%v\n", cmd)
	return cmd
}

// 4DO NO/2A
type Wago750515 struct {
	res_io AddrIO
	io     AddrIO
}

func (wago *Wago750515) init(addr *AddrIO) {
	wago.res_io.INRESWORDS = 0
	wago.res_io.INRESBITS = 0
	wago.res_io.OUTRESWORDS = 0
	wago.res_io.OUTRESBITS = 4
	wago.io = *addr
	addr.INRESWORDS += wago.res_io.INRESWORDS
	addr.INRESBITS += wago.res_io.INRESBITS
	addr.OUTRESWORDS += wago.res_io.OUTRESWORDS
	addr.OUTRESBITS += wago.res_io.OUTRESBITS
}

func (wago *Wago750515) set(value uint8) []uint8 {
	cmd := []uint8{1, WRITE_BITS, 0x00, 0x00 + wago.io.OUTRESBITS, 0x00, 0x08, 0x01, value}
	crc := checksum(cmd)
	cmd = append(cmd, uint8(crc&0xFF), uint8(crc>>8))
	fmt.Printf("%v\n", cmd)
	return cmd
}

// 4AI 10V
type Wago750468 struct {
	res_io AddrIO
	io     AddrIO
}

func (wago *Wago750468) init(addr *AddrIO) {
	wago.res_io.INRESWORDS = 2
	wago.res_io.INRESBITS = 0
	wago.res_io.OUTRESWORDS = 0
	wago.res_io.OUTRESBITS = 0
	wago.io = *addr
	addr.INRESWORDS += wago.res_io.INRESWORDS
	addr.INRESBITS += wago.res_io.INRESBITS
	addr.OUTRESWORDS += wago.res_io.OUTRESWORDS
	addr.OUTRESBITS += wago.res_io.OUTRESBITS
}

func (wago *Wago750468) get(channel uint8) []uint8 {
	cmd := []uint8{1, READ_WORDS, 0x00, 0x00 + wago.io.INRESWORDS + channel, 0x00, 0x01}
	crc := checksum(cmd)
	cmd = append(cmd, uint8(crc&0xFF), uint8(crc>>8))
	fmt.Printf("%v\n", cmd)
	return cmd
}

// 4AO 10V
type Wago750559 struct {
	res_io AddrIO
	io     AddrIO
}

func (wago *Wago750559) init(addr *AddrIO) {
	wago.res_io.INRESWORDS = 0
	wago.res_io.INRESBITS = 0
	wago.res_io.OUTRESWORDS = 2
	wago.res_io.OUTRESBITS = 0
	wago.io = *addr
	addr.INRESWORDS += wago.res_io.INRESWORDS
	addr.INRESBITS += wago.res_io.INRESBITS
	addr.OUTRESWORDS += wago.res_io.OUTRESWORDS
	addr.OUTRESBITS += wago.res_io.OUTRESBITS
}

var TABLE [256]uint16
var INIT_DONE bool = false

const CRC_POLY uint16 = 0xA001
const CRC_START_MODBUS uint16 = 0xFFFF

func initTable() {
	if !INIT_DONE {
		var c, crc uint16
		for i := 0; i < 256; i++ {
			c = uint16(i)
			crc = 0
			for j := 0; j < 8; j++ {
				if (crc^c)&0x0001 == 0x0001 {
					crc = (crc >> 1) ^ CRC_POLY
				} else {
					crc >>= 1
				}
				c >>= 1
			}
			TABLE[i] = crc
		}
		INIT_DONE = true
	}
}

func printTable() {
	initTable()
	for i := 0; i < 256; i++ {
		if i%16 == 0 {
			fmt.Println()
		}
		fmt.Printf("%04X ", TABLE[i])
	}
	fmt.Println()
}

func checksum(vec []uint8) uint16 {
	initTable()
	crc := CRC_START_MODBUS
	var tmp, c uint16
	for _, val := range vec {
		c = 0x00FF & uint16(val)
		tmp = crc ^ c
		crc = (crc >> 8) ^ TABLE[(tmp&0xff)]
	}
	return crc
}

func TestChecksum() {
	crc := checksum([]uint8{0, 1, 2, 3, 4, 5, 6, 7, 8, 9})
	fmt.Printf("0x%04X\n", crc)
	if crc != 0x4574 {
		panic("TestCheckum failed")
	}
}

var s *serial.Port
var err error

func openSerial(com string) {
	c := &serial.Config{Name: com, Baud: 19200,
		Size:        8,
		Parity:      serial.ParityEven,
		StopBits:    serial.Stop1,
		ReadTimeout: time.Millisecond * 35}
	s, err = serial.OpenPort(c)
	if err != nil {
		log.Fatal(err)
	} else {
		// defer s.Close()
	}
}

// var buf []byte = make([]byte, 256)

func sendRecvSerial(data []uint8) []uint8 {
	n, err := s.Write(data)
	if err != nil {
		log.Fatal(err)
	}
	// sleep otherwise read is not complete
	time.Sleep(time.Millisecond * 50)
	buf := make([]byte, 256)
	n, err = s.Read(buf)
	if err != nil {
		log.Fatal(err)
	}
	log.Printf("=> %q", buf[:n])
	return buf[:n]
}

func run() {
	wago := WagoModule{}
	w315 := Wago750315{}
	w430 := Wago750430{}
	w530 := Wago750530{}
	w468 := Wago750468{}
	w515 := Wago750515{}
	_ = wago
	addr := AddrIO{0, 0, 0, 0}
	w315.init(&addr)
	w315.init(&addr)
	w430.init(&addr)
	w530.init(&addr)
	w515.init(&addr)
	w468.init(&addr)
	fmt.Printf("%v\n", addr)

	openSerial("COM27")

	// for val := 0; val < 256; val += 1 {
	// 	sendRecvSerial(w530.set(uint8(val)))
	// 	time.Sleep(time.Millisecond * 10)
	// }
	// sendRecvSerial(w530.set(0x00))

	// for val := 0; val < 16; val += 1 {
	// 	sendRecvSerial(w515.set(uint8(val)))
	// 	time.Sleep(time.Millisecond * 3000)
	// }
	// sendRecvSerial(w515.set(0x00))

	recv := sendRecvSerial(w430.get())
	if len(recv) == 6 {
		fmt.Printf("w430: %02X\n", recv[3])
	}

	for channel := 0; channel < 4; channel += 1 {
		recv := sendRecvSerial(w468.get(uint8(channel)))
		if len(recv) == 7 {
			tmp := (uint16(recv[3]) << 8) | uint16(recv[4])
			voltage := float64(tmp) / 0x7FFF * 10.0
			fmt.Printf("w468[%v]: %.2f\n", channel, voltage)
		}

	}
}

func main() {
	// printTable()
	// TestChecksum()
	run()
}
