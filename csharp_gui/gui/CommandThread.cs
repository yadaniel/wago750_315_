using System;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Forms;

public class CommandThread {
	private List<string> commandList;
	private static CommandThread self;
	public wago750.WagoCoupler_750315 wago;
	public wago750.Wago_750430 wago430;
	public wago750.Wago_750530 wago530;
	public wago750.Wago_750468 wago468;
	public wago750.Wago_750515 wago515;
	private SerialPort serialPort;
	public bool threadRunning;
    private Thread thread;
    private string logFilename = "wago750.log";
    public string communicationLog = string.Empty;
    public bool valid;
	
	public CommandThread() {
    	serialPort = new SerialPort();
		wago = new wago750.WagoCoupler_750315(serialPort, modbusAddr: 1);
		wago430 = new wago750.Wago_750430();
		wago530 = new wago750.Wago_750530();
		wago468 = new wago750.Wago_750468();
		wago515 = new wago750.Wago_750515();
		wago.AddModule(wago430);
		wago.AddModule(wago530);
		wago.AddModule(wago468);
		wago.AddModule(wago515);
	}
    
    public void startThread() {
        if (thread == null) {
            commandList = new List<string>();
            thread = new Thread(runThread);
            thread.Start();
            threadRunning = true;
        }
    }
    
    public void stopThread() {
        if (thread != null) {
            this.Dispose();
        }
        threadRunning = false;
    }
    
    public void runThread() {
        while (true) {
    		if ((isConnected()) && (commandList.Count > 0)) {
                string nextCommand = popCommand();
                
                // fetch the request to be written to serial port
                byte[] request_bytes = null;
                switch (nextCommand) {
					case "WRITE_WAGO515":
                        request_bytes = wago515.send();
                        break;
					case "WRITE_WAGO530":
                		request_bytes = wago530.send();
                        break;
					case "READ_WAGO430":
                        request_bytes = wago430.send();
                        break;
                    case "READ_WAGO468_1":
                        request_bytes = wago468.send(1);
                        break;
                    case "READ_WAGO468_2":
                        request_bytes = wago468.send(2);
                        break;
                    case "READ_WAGO468_3":
                        request_bytes = wago468.send(3);
                        break;
                    case "READ_WAGO468_4":
                        request_bytes = wago468.send(4);
                        break;
                    default:
                        MessageBox.Show(nextCommand, "unknown command");
                        break;
                }
                
                // send the request to serial port
                //string tmp = Encoding.ASCII.GetString(request_bytes);
                //serialPort.Write(tmp);
                serialPort.Write(request_bytes, 0, request_bytes.Length);
                
                // read the response from serial port
                byte[] buffer = new byte[100];
                int read = 0;
                try {
                	while(read != 100) {
                		read += serialPort.Read(buffer, read, 100-read);
                	}
                }
                catch (TimeoutException) {
					//response = "timeout";
                }
                catch(Exception) {
                	MessageBox.Show("Exception while reading from COM");
                }
                
                // interpret the response
                byte[] response_bytes = new byte[read];
                Array.Copy(buffer, response_bytes, read);
                switch (nextCommand) {
					case "WRITE_WAGO515":
                        wago515.parse(response_bytes);
                        break;
					case "WRITE_WAGO530":
                		wago530.parse(response_bytes);
                        break;
					case "READ_WAGO430":
                        wago430.parse(response_bytes);
                        break;
                    case "READ_WAGO468_1":
                        wago468.parse(response_bytes, 1);
                        break;
                    case "READ_WAGO468_2":
                        wago468.parse(response_bytes, 2);
                        break;                        
                    case "READ_WAGO468_3":
                        wago468.parse(response_bytes, 3);
                        break;
                    case "READ_WAGO468_4":
                        wago468.parse(response_bytes, 4);
                        break;
                    default:
                        break;
                }
                
                // write out the log file
                using(StreamWriter tw = new StreamWriter(logFilename, append:true)) {
                	tw.WriteLine(string.Format("[{0}]: =>{1}, <={2}", 
                	                           DateTime.Now.ToString(), 
                	                           nextCommand + ":" + Encoding.ASCII.GetString(request_bytes), 
                	                           nextCommand + ":" + Encoding.ASCII.GetString(response_bytes)));
                }
								
    		} else {
				/* wago sends only reply telegrams - no autoread */
    		}
        }
    }
    
    public static CommandThread getInstance() {
        if (self == null) {
            self = new CommandThread();
        }
        return self;
    }
	
    public void Dispose() {
        thread?.Suspend();
        thread = null;
        if (serialPort.IsOpen) {
            serialPort.Close();
        }
    }
    
    public bool tryConnectCOM(string port) {
        // setup serial port
        serialPort.PortName = port;
        serialPort.ReadTimeout = 20;
        serialPort.BaudRate = 19200;
        serialPort.DataBits = 8;
        serialPort.StopBits = StopBits.One;
        serialPort.Parity = Parity.Even;
        serialPort.Handshake = Handshake.None;
        try {
            serialPort.Open();
        }
        catch {
            return false;
        }
        return true;
    }
    
    public bool isConnected() {
    	return serialPort.IsOpen;
    }
    
    public void pushCommandFirst(string command) {
        if (commandList != null) {
            lock (commandList) {
                commandList.Insert(0, command);
            }
        }
    }
	
    public void pushCommand(string command) {
        if(commandList != null) {
            lock (commandList) {
                commandList.Add(command);
            }
        }
    }
    
    public string popCommand() {
        string retValue = string.Empty;
        lock (commandList) {
            retValue = commandList.First();
            commandList.RemoveAt(0);
        }
        return retValue;
    }
    
    public bool empty() {
    	return commandList.Count == 0;
    }
}
