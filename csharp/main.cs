#define DBG

using System;
using System.IO.Ports;
using System.Threading;
using wago750;

class App : IDisposable {
    private int port = 26;
    private SerialPort serialPort;
    private WagoCoupler_750315 wago;
    private Wago_750430 wago430;
    private Wago_750530 wago530;
    private Wago_750468 wago468;
    private Wago_750515 wago515;
    private Wago_750559 wago559;

    static int Main(string[] args) {
        // wago750.ModbusCRC16.printTable();
        // return 0;
        App app = new App(args);
        return app.run();
    }

    public App(string[] args) {
        if(args.Length == 1) {
            port = Convert.ToInt32(args[0]);
        }
        serialPort = new SerialPort();
        serialPort.PortName = string.Format($"COM{port}");
        serialPort.ReadTimeout = 35;
        serialPort.BaudRate = 19200;
        serialPort.DataBits = 8;
        serialPort.StopBits = StopBits.One;
        serialPort.Parity = Parity.Even;
        serialPort.Handshake = Handshake.None;
    }

    // void IDisposable.Dispose() {
    public void Dispose() {
        Console.WriteLine("Dispose()");
        if((serialPort != null) && serialPort.IsOpen) {
            Console.WriteLine($"closing COM{port}");
            serialPort.Close();
        }
    }

    public int run() {
        try {
            Console.WriteLine($"trying to open COM{port}");
            serialPort.Open();
        } catch {
            Console.WriteLine($"failed to open COM{port}");
            return 1;
        }

        // check the module order in datasheet
        wago = new WagoCoupler_750315(serialPort, modbusAddr: 1);
        wago430 = new Wago_750430();
        wago530 = new Wago_750530();
        wago468 = new Wago_750468();
        wago515 = new Wago_750515();
        wago559 = new Wago_750559();
        wago.AddModule(wago430);
        wago.AddModule(wago530);
        wago.AddModule(wago468);
        wago.AddModule(wago515);
        wago.AddModule(wago559);

        // // operate wago515
        // for(byte state=0; state<16; state++) {
        //     wago515.set(state);
        //     Thread.Sleep(500);
        // }
        // Thread.Sleep(1000);
        // wago515.set(0);

        // // operate wago530
        // for(int state=0; state<256; state++) {
        //     wago530.set((byte)state);
        //     Thread.Sleep(50);
        // }
        // Thread.Sleep(1000);
        // wago530.set(0);

        // // operate wago430
        // byte inState;
        // if(wago430.get(out inState)) {
        //     Console.WriteLine("{0:X02}", inState);
        // } else {
        //     Console.WriteLine("no response");
        // }

        // operate wago468
        for(int channel=0; channel<4; channel++) {
            double inVoltage;
            if(wago468.get(out inVoltage, channel)) {
                Console.WriteLine($"channel[{channel}] = {inVoltage:0.00}");
            } else {
                Console.WriteLine($"channel[{channel}] no response");
            }
        }

        // // operate wago559
        // for(int channel=0; channel<4; channel++) {
        //     for(double outVoltage=0; outVoltage<10; outVoltage+=2.5) {
        //         wago559.set(outVoltage, channel);
        //         Thread.Sleep(3000);
        //     }
        // }

        // do not rely on ~App and cleanup resources
        Dispose();
        // ((IDisposable)this).Dispose();
        return 0;
    }
}



