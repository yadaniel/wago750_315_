using System;
using System.IO.Ports;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

/// <summary>
/// Description of MainForm.
/// </summary>
public partial class MainForm : Form {
	private CommandThread commandThread;
	private string comPort = string.Empty;
	private System.Threading.Timer timer100;
	private Random random;
	
	private int outValue530 = 0;
	private int outValue515 = 0;
	private int inValue430 = 0;
	private int inValue468_1 = 0;
	private int inValue468_2 = 0;
	private int inValue468_3 = 0;
	private int inValue468_4 = 0;
	
	private static void TimerTick100(object sender) {
		MainForm mainForm = (MainForm)sender;
		mainForm.Invoke(new Action(mainForm.updateValues));
	}
		
	private void updateValues() {
		if(commandThread.isConnected()) {
			// update commands
			if(commandThread.empty()) {
				// push only when empty
				// otherwise COM read timeout must match number of pushed commands times pushing freq
				commandThread.pushCommand("WRITE_WAGO515");
				commandThread.pushCommand("WRITE_WAGO530");
				commandThread.pushCommand("READ_WAGO430");
				commandThread.pushCommand("READ_WAGO468_1");
				commandThread.pushCommand("READ_WAGO468_2");
				commandThread.pushCommand("READ_WAGO468_3");
				commandThread.pushCommand("READ_WAGO468_4");
			}
			
			// update internal output states
			commandThread.wago530.state = (byte)outValue530;
			commandThread.wago515.state = (byte)outValue515;
			
			// update internal intput states
			inValue430 = commandThread.wago430.state;
			inValue468_1 = commandThread.wago468.state1;
			inValue468_2 = commandThread.wago468.state2;
			inValue468_3 = commandThread.wago468.state3;
			inValue468_4 = commandThread.wago468.state4;
			
			// update gui
			textBox1.Text = string.Format("{0:X04}", inValue468_1);
			textBox2.Text = string.Format("{0:X04}", inValue468_2);
			textBox3.Text = string.Format("{0:X04}", inValue468_3);
			textBox4.Text = string.Format("{0:X04}", inValue468_4);
			textBox5.Text = string.Format("{0:0.000}", ((double) inValue468_1)/0x7FFF*10.0);
			textBox6.Text = string.Format("{0:0.000}", ((double) inValue468_2)/0x7FFF*10.0);
			textBox7.Text = string.Format("{0:0.000}", ((double) inValue468_3)/0x7FFF*10.0);
			textBox8.Text = string.Format("{0:0.000}", ((double) inValue468_4)/0x7FFF*10.0);
			
			
			checkBox1.Checked = false;
			if((inValue430 & 0x01) == 0x01) {
				checkBox1.Checked = true;
			}
			checkBox2.Checked = false;
			if((inValue430 & 0x02) == 0x02) {
				checkBox2.Checked = true;
			}
			checkBox3.Checked = false;
			if((inValue430 & 0x04) == 0x04) {
				checkBox3.Checked = true;
			}
			checkBox4.Checked = false;
			if((inValue430 & 0x08) == 0x08) {
				checkBox4.Checked = true;
			}
			checkBox5.Checked = false;
			if((inValue430 & 0x10) == 0x10) {
				checkBox5.Checked = true;
			}
			checkBox6.Checked = false;
			if((inValue430 & 0x20) == 0x20) {
				checkBox6.Checked = true;
			}
			checkBox7.Checked = false;
			if((inValue430 & 0x40) == 0x40) {
				checkBox7.Checked = true;
			}
			checkBox8.Checked = false;
			if((inValue430 & 0x80) == 0x80) {
				checkBox8.Checked = true;
			}
		}
	}
	
	public MainForm() {
		InitializeComponent();
		random = new Random();
		commandThread = CommandThread.getInstance();
		timer100 = new System.Threading.Timer(TimerTick100, this, 1000, 100);
	}
	
	void MainFormDoubleClick(object sender, EventArgs e) {
    	commandThread.Dispose();
    	timer100.Dispose();
		Application.Exit();
	}
	
	void MainFormFormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e) {
		commandThread.Dispose();
    	timer100.Dispose();
		Application.Exit();
	}
	
	private void GroupBoxCOMEnter(object sender, EventArgs e) {
        string[] listCOM = SerialPort.GetPortNames();
        comboBoxSelectCOM.Items.Clear();
        foreach (string com in listCOM) {
            comboBoxSelectCOM.Items.Add(com);
        }
	}
	
	void ConnectCOMClick(object sender, EventArgs e) {
		Button connect = (Button)sender;
    	if (connect.Text == "connect") {
        	// previously disconnected
        	string comPort = string.Empty;
        	try {
            	comPort = comboBoxSelectCOM.SelectedItem.ToString();
        	} catch {
            	MessageBox.Show(text: "connect SDS011 and select COM port", caption: "COM list error", buttons: MessageBoxButtons.OK);
        	}
        	if (comPort != string.Empty) {
            	bool connectionEstablished = commandThread.tryConnectCOM(comPort);
            	if (connectionEstablished) {
                	commandThread.startThread();
                	connect.Text = "disconnect";
                	connect.BackColor = Color.LightGreen;
            	}
        	} else { /* something went wrong, refresh and connect again */ }
    	} else {
        	// previously connected
        	if (commandThread.threadRunning) {
            	commandThread.stopThread();
            	connect.Text = "connect";
            	connect.BackColor = Control.DefaultBackColor;
        	}
    	}
	}
	
	void CheckBox9CheckedChanged(object sender, EventArgs e) {
		CheckBox checkBox = (CheckBox)sender;
		if(checkBox.Checked) {
			outValue530 |= 0x01;
		} else {
			outValue530 &= ~0x01;
		}
		//MessageBox.Show(string.Format("{0:X02}", value530));
	}
	
	void CheckBox10CheckedChanged(object sender, EventArgs e) {
		CheckBox checkBox = (CheckBox)sender;
		if(checkBox.Checked) {
			outValue530 |= 0x02;
		} else {
			outValue530 &= ~0x02;
		}
		//MessageBox.Show(string.Format("{0:X02}", value530));
	}
	
	void CheckBox11CheckedChanged(object sender, EventArgs e) {
		CheckBox checkBox = (CheckBox)sender;
		if(checkBox.Checked) {
			outValue530 |= 0x04;
		} else {
			outValue530 &= ~0x04;
		}
		//MessageBox.Show(string.Format("{0:X02}", value530));
	}
	
	void CheckBox12CheckedChanged(object sender, EventArgs e) {
		CheckBox checkBox = (CheckBox)sender;
		if(checkBox.Checked) {
			outValue530 |= 0x08;
		} else {
			outValue530 &= ~0x08;
		}
		//MessageBox.Show(string.Format("{0:X02}", value530));
	}
	
	void CheckBox13CheckedChanged(object sender, EventArgs e) {
		CheckBox checkBox = (CheckBox)sender;
		if(checkBox.Checked) {
			outValue530 |= 0x10;
		} else {
			outValue530 &= ~0x10;
		}
		//MessageBox.Show(string.Format("{0:X02}", value530));
	}
	
	void CheckBox14CheckedChanged(object sender, EventArgs e) {
		CheckBox checkBox = (CheckBox)sender;
		if(checkBox.Checked) {
			outValue530 |= 0x20;
		} else {
			outValue530 &= ~0x20;
		}
		//MessageBox.Show(string.Format("{0:X02}", value530));
	}
	
	void CheckBox15CheckedChanged(object sender, EventArgs e) {
		CheckBox checkBox = (CheckBox)sender;
		if(checkBox.Checked) {
			outValue530 |= 0x40;
		} else {
			outValue530 &= ~0x40;
		}
		//MessageBox.Show(string.Format("{0:X02}", value530));
	}
	
	void CheckBox16CheckedChanged(object sender, EventArgs e) {
		CheckBox checkBox = (CheckBox)sender;
		if(checkBox.Checked) {
			outValue530 |= 0x80;
		} else {
			outValue530 &= ~0x80;
		}
		//MessageBox.Show(string.Format("{0:X02}", value530));
	}
	
	void CheckBox17CheckedChanged(object sender, EventArgs e) {
		CheckBox checkBox = (CheckBox)sender;
		if(checkBox.Checked) {
			outValue515 |= 0x01;
		} else {
			outValue515 &= ~0x01;
		}
		//MessageBox.Show(string.Format("{0:X02}", value515));
	}
	
	void CheckBox18CheckedChanged(object sender, EventArgs e) {
		CheckBox checkBox = (CheckBox)sender;
		if(checkBox.Checked) {
			outValue515 |= 0x02;
		} else {
			outValue515 &= ~0x02;
		}
		//MessageBox.Show(string.Format("{0:X02}", value515));
	}
	
	void CheckBox19CheckedChanged(object sender, EventArgs e) {
		CheckBox checkBox = (CheckBox)sender;
		if(checkBox.Checked) {
			outValue515 |= 0x04;
		} else {
			outValue515 &= ~0x04;
		}
		//MessageBox.Show(string.Format("{0:X02}", value515));
	}
	
	void CheckBox20CheckedChanged(object sender, EventArgs e) {
		CheckBox checkBox = (CheckBox)sender;
		if(checkBox.Checked) {
			outValue515 |= 0x08;
		} else {
			outValue515 &= ~0x08;
		}
		//MessageBox.Show(string.Format("{0:X02}", value515));
	}
	
	
	void CheckBoxLockCheckedChanged(object sender, EventArgs e) {
		CheckBox checkBox = (CheckBox)sender;
		if(checkBox.Checked) {
			checkBox.Text = "uncheck to unlock names";
		} else {
			checkBox.Text = "check to lock names";
		}
		// 
		textBox9.Enabled = !checkBox.Checked;
		textBox10.Enabled = !checkBox.Checked;
		textBox11.Enabled = !checkBox.Checked;
		textBox12.Enabled = !checkBox.Checked;
		textBox13.Enabled = !checkBox.Checked;
		textBox14.Enabled = !checkBox.Checked;
		textBox15.Enabled = !checkBox.Checked;
		textBox16.Enabled = !checkBox.Checked;
		// 
		textBox17.Enabled = !checkBox.Checked;
		textBox18.Enabled = !checkBox.Checked;
		textBox19.Enabled = !checkBox.Checked;
		textBox20.Enabled = !checkBox.Checked;
		textBox21.Enabled = !checkBox.Checked;
		textBox22.Enabled = !checkBox.Checked;
		textBox23.Enabled = !checkBox.Checked;
		textBox24.Enabled = !checkBox.Checked;
		// 
		textBox25.Enabled = !checkBox.Checked;
		textBox26.Enabled = !checkBox.Checked;
		textBox27.Enabled = !checkBox.Checked;
		textBox28.Enabled = !checkBox.Checked;
		//
		textBox29.Enabled = !checkBox.Checked;
		textBox30.Enabled = !checkBox.Checked;
		textBox31.Enabled = !checkBox.Checked;
		textBox32.Enabled = !checkBox.Checked;
	}
}
