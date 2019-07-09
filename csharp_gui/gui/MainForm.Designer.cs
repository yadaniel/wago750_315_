partial class MainForm
{
	/// <summary>
	/// Designer variable used to keep track of non-visual components.
	/// </summary>
	private System.ComponentModel.IContainer components = null;
	
	/// <summary>
	/// Disposes resources used by the form.
	/// </summary>
	/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
	protected override void Dispose(bool disposing)
	{
		if (disposing) {
			if (components != null) {
				components.Dispose();
			}
		}
		base.Dispose(disposing);
	}
	
	/// <summary>
	/// This method is required for Windows Forms designer support.
	/// Do not change the method contents inside the source code editor. The Forms designer might
	/// not be able to load this method if it was changed manually.
	/// </summary>
	private void InitializeComponent()
	{
		this.components = new System.ComponentModel.Container();
		this.connectCOM = new System.Windows.Forms.Button();
		this.comboBoxSelectCOM = new System.Windows.Forms.ComboBox();
		this.groupBoxCOM = new System.Windows.Forms.GroupBox();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.textBox16 = new System.Windows.Forms.TextBox();
		this.textBox15 = new System.Windows.Forms.TextBox();
		this.textBox14 = new System.Windows.Forms.TextBox();
		this.textBox13 = new System.Windows.Forms.TextBox();
		this.textBox12 = new System.Windows.Forms.TextBox();
		this.textBox11 = new System.Windows.Forms.TextBox();
		this.textBox10 = new System.Windows.Forms.TextBox();
		this.textBox9 = new System.Windows.Forms.TextBox();
		this.checkBox8 = new System.Windows.Forms.CheckBox();
		this.checkBox7 = new System.Windows.Forms.CheckBox();
		this.checkBox6 = new System.Windows.Forms.CheckBox();
		this.checkBox5 = new System.Windows.Forms.CheckBox();
		this.checkBox4 = new System.Windows.Forms.CheckBox();
		this.checkBox3 = new System.Windows.Forms.CheckBox();
		this.checkBox2 = new System.Windows.Forms.CheckBox();
		this.checkBox1 = new System.Windows.Forms.CheckBox();
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.textBox17 = new System.Windows.Forms.TextBox();
		this.checkBox16 = new System.Windows.Forms.CheckBox();
		this.textBox18 = new System.Windows.Forms.TextBox();
		this.checkBox15 = new System.Windows.Forms.CheckBox();
		this.textBox19 = new System.Windows.Forms.TextBox();
		this.checkBox14 = new System.Windows.Forms.CheckBox();
		this.textBox20 = new System.Windows.Forms.TextBox();
		this.checkBox13 = new System.Windows.Forms.CheckBox();
		this.textBox21 = new System.Windows.Forms.TextBox();
		this.checkBox12 = new System.Windows.Forms.CheckBox();
		this.textBox22 = new System.Windows.Forms.TextBox();
		this.checkBox11 = new System.Windows.Forms.CheckBox();
		this.textBox23 = new System.Windows.Forms.TextBox();
		this.checkBox10 = new System.Windows.Forms.CheckBox();
		this.textBox24 = new System.Windows.Forms.TextBox();
		this.checkBox9 = new System.Windows.Forms.CheckBox();
		this.groupBox3 = new System.Windows.Forms.GroupBox();
		this.textBox25 = new System.Windows.Forms.TextBox();
		this.label8 = new System.Windows.Forms.Label();
		this.textBox26 = new System.Windows.Forms.TextBox();
		this.label7 = new System.Windows.Forms.Label();
		this.textBox27 = new System.Windows.Forms.TextBox();
		this.label6 = new System.Windows.Forms.Label();
		this.textBox28 = new System.Windows.Forms.TextBox();
		this.label5 = new System.Windows.Forms.Label();
		this.textBox8 = new System.Windows.Forms.TextBox();
		this.textBox7 = new System.Windows.Forms.TextBox();
		this.textBox6 = new System.Windows.Forms.TextBox();
		this.textBox5 = new System.Windows.Forms.TextBox();
		this.label4 = new System.Windows.Forms.Label();
		this.textBox4 = new System.Windows.Forms.TextBox();
		this.label3 = new System.Windows.Forms.Label();
		this.textBox3 = new System.Windows.Forms.TextBox();
		this.label2 = new System.Windows.Forms.Label();
		this.textBox2 = new System.Windows.Forms.TextBox();
		this.label1 = new System.Windows.Forms.Label();
		this.textBox1 = new System.Windows.Forms.TextBox();
		this.groupBox4 = new System.Windows.Forms.GroupBox();
		this.textBox29 = new System.Windows.Forms.TextBox();
		this.checkBox20 = new System.Windows.Forms.CheckBox();
		this.textBox30 = new System.Windows.Forms.TextBox();
		this.checkBox19 = new System.Windows.Forms.CheckBox();
		this.textBox31 = new System.Windows.Forms.TextBox();
		this.checkBox18 = new System.Windows.Forms.CheckBox();
		this.textBox32 = new System.Windows.Forms.TextBox();
		this.checkBox17 = new System.Windows.Forms.CheckBox();
		this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
		this.checkBoxLock = new System.Windows.Forms.CheckBox();
		this.groupBoxCOM.SuspendLayout();
		this.groupBox1.SuspendLayout();
		this.groupBox2.SuspendLayout();
		this.groupBox3.SuspendLayout();
		this.groupBox4.SuspendLayout();
		this.SuspendLayout();
		// 
		// connectCOM
		// 
		this.connectCOM.Location = new System.Drawing.Point(14, 63);
		this.connectCOM.Name = "connectCOM";
		this.connectCOM.Size = new System.Drawing.Size(121, 29);
		this.connectCOM.TabIndex = 2;
		this.connectCOM.Text = "connect";
		this.connectCOM.UseVisualStyleBackColor = true;
		this.connectCOM.Click += new System.EventHandler(this.ConnectCOMClick);
		// 
		// comboBoxSelectCOM
		// 
		this.comboBoxSelectCOM.FormattingEnabled = true;
		this.comboBoxSelectCOM.Location = new System.Drawing.Point(14, 33);
		this.comboBoxSelectCOM.Name = "comboBoxSelectCOM";
		this.comboBoxSelectCOM.Size = new System.Drawing.Size(121, 24);
		this.comboBoxSelectCOM.TabIndex = 3;
		// 
		// groupBoxCOM
		// 
		this.groupBoxCOM.Controls.Add(this.comboBoxSelectCOM);
		this.groupBoxCOM.Controls.Add(this.connectCOM);
		this.groupBoxCOM.Location = new System.Drawing.Point(541, 196);
		this.groupBoxCOM.Name = "groupBoxCOM";
		this.groupBoxCOM.Size = new System.Drawing.Size(148, 102);
		this.groupBoxCOM.TabIndex = 4;
		this.groupBoxCOM.TabStop = false;
		this.groupBoxCOM.Text = "select COM";
		this.groupBoxCOM.Enter += new System.EventHandler(this.GroupBoxCOMEnter);
		// 
		// groupBox1
		// 
		this.groupBox1.Controls.Add(this.textBox16);
		this.groupBox1.Controls.Add(this.textBox15);
		this.groupBox1.Controls.Add(this.textBox14);
		this.groupBox1.Controls.Add(this.textBox13);
		this.groupBox1.Controls.Add(this.textBox12);
		this.groupBox1.Controls.Add(this.textBox11);
		this.groupBox1.Controls.Add(this.textBox10);
		this.groupBox1.Controls.Add(this.textBox9);
		this.groupBox1.Controls.Add(this.checkBox8);
		this.groupBox1.Controls.Add(this.checkBox7);
		this.groupBox1.Controls.Add(this.checkBox6);
		this.groupBox1.Controls.Add(this.checkBox5);
		this.groupBox1.Controls.Add(this.checkBox4);
		this.groupBox1.Controls.Add(this.checkBox3);
		this.groupBox1.Controls.Add(this.checkBox2);
		this.groupBox1.Controls.Add(this.checkBox1);
		this.groupBox1.Location = new System.Drawing.Point(12, 12);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(240, 286);
		this.groupBox1.TabIndex = 5;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "750-430 (8DI)";
		// 
		// textBox16
		// 
		this.textBox16.Enabled = false;
		this.textBox16.Location = new System.Drawing.Point(70, 247);
		this.textBox16.Name = "textBox16";
		this.textBox16.Size = new System.Drawing.Size(155, 22);
		this.textBox16.TabIndex = 17;
		// 
		// textBox15
		// 
		this.textBox15.Enabled = false;
		this.textBox15.Location = new System.Drawing.Point(70, 217);
		this.textBox15.Name = "textBox15";
		this.textBox15.Size = new System.Drawing.Size(155, 22);
		this.textBox15.TabIndex = 16;
		// 
		// textBox14
		// 
		this.textBox14.Enabled = false;
		this.textBox14.Location = new System.Drawing.Point(70, 184);
		this.textBox14.Name = "textBox14";
		this.textBox14.Size = new System.Drawing.Size(155, 22);
		this.textBox14.TabIndex = 15;
		// 
		// textBox13
		// 
		this.textBox13.Enabled = false;
		this.textBox13.Location = new System.Drawing.Point(70, 152);
		this.textBox13.Name = "textBox13";
		this.textBox13.Size = new System.Drawing.Size(155, 22);
		this.textBox13.TabIndex = 14;
		// 
		// textBox12
		// 
		this.textBox12.Enabled = false;
		this.textBox12.Location = new System.Drawing.Point(70, 124);
		this.textBox12.Name = "textBox12";
		this.textBox12.Size = new System.Drawing.Size(155, 22);
		this.textBox12.TabIndex = 13;
		// 
		// textBox11
		// 
		this.textBox11.Enabled = false;
		this.textBox11.Location = new System.Drawing.Point(70, 96);
		this.textBox11.Name = "textBox11";
		this.textBox11.Size = new System.Drawing.Size(155, 22);
		this.textBox11.TabIndex = 12;
		// 
		// textBox10
		// 
		this.textBox10.Enabled = false;
		this.textBox10.Location = new System.Drawing.Point(70, 66);
		this.textBox10.Name = "textBox10";
		this.textBox10.Size = new System.Drawing.Size(155, 22);
		this.textBox10.TabIndex = 11;
		// 
		// textBox9
		// 
		this.textBox9.Enabled = false;
		this.textBox9.Location = new System.Drawing.Point(70, 36);
		this.textBox9.Name = "textBox9";
		this.textBox9.Size = new System.Drawing.Size(155, 22);
		this.textBox9.TabIndex = 10;
		// 
		// checkBox8
		// 
		this.checkBox8.Location = new System.Drawing.Point(14, 245);
		this.checkBox8.Name = "checkBox8";
		this.checkBox8.Size = new System.Drawing.Size(50, 24);
		this.checkBox8.TabIndex = 9;
		this.checkBox8.Text = "in7";
		this.toolTip1.SetToolTip(this.checkBox8, "E8");
		this.checkBox8.UseVisualStyleBackColor = true;
		// 
		// checkBox7
		// 
		this.checkBox7.Location = new System.Drawing.Point(14, 215);
		this.checkBox7.Name = "checkBox7";
		this.checkBox7.Size = new System.Drawing.Size(50, 24);
		this.checkBox7.TabIndex = 8;
		this.checkBox7.Text = "in6";
		this.toolTip1.SetToolTip(this.checkBox7, "E7");
		this.checkBox7.UseVisualStyleBackColor = true;
		// 
		// checkBox6
		// 
		this.checkBox6.Location = new System.Drawing.Point(14, 185);
		this.checkBox6.Name = "checkBox6";
		this.checkBox6.Size = new System.Drawing.Size(50, 24);
		this.checkBox6.TabIndex = 7;
		this.checkBox6.Text = "in5";
		this.toolTip1.SetToolTip(this.checkBox6, "E6");
		this.checkBox6.UseVisualStyleBackColor = true;
		// 
		// checkBox5
		// 
		this.checkBox5.Location = new System.Drawing.Point(14, 155);
		this.checkBox5.Name = "checkBox5";
		this.checkBox5.Size = new System.Drawing.Size(50, 24);
		this.checkBox5.TabIndex = 6;
		this.checkBox5.Text = "in4";
		this.toolTip1.SetToolTip(this.checkBox5, "E5");
		this.checkBox5.UseVisualStyleBackColor = true;
		// 
		// checkBox4
		// 
		this.checkBox4.Location = new System.Drawing.Point(14, 125);
		this.checkBox4.Name = "checkBox4";
		this.checkBox4.Size = new System.Drawing.Size(50, 24);
		this.checkBox4.TabIndex = 5;
		this.checkBox4.Text = "in3";
		this.toolTip1.SetToolTip(this.checkBox4, "E4");
		this.checkBox4.UseVisualStyleBackColor = true;
		// 
		// checkBox3
		// 
		this.checkBox3.Location = new System.Drawing.Point(14, 95);
		this.checkBox3.Name = "checkBox3";
		this.checkBox3.Size = new System.Drawing.Size(50, 24);
		this.checkBox3.TabIndex = 4;
		this.checkBox3.Text = "in2";
		this.toolTip1.SetToolTip(this.checkBox3, "E3");
		this.checkBox3.UseVisualStyleBackColor = true;
		// 
		// checkBox2
		// 
		this.checkBox2.Location = new System.Drawing.Point(14, 65);
		this.checkBox2.Name = "checkBox2";
		this.checkBox2.Size = new System.Drawing.Size(50, 24);
		this.checkBox2.TabIndex = 2;
		this.checkBox2.Text = "in1";
		this.toolTip1.SetToolTip(this.checkBox2, "E2");
		this.checkBox2.UseVisualStyleBackColor = true;
		// 
		// checkBox1
		// 
		this.checkBox1.Location = new System.Drawing.Point(14, 35);
		this.checkBox1.Name = "checkBox1";
		this.checkBox1.Size = new System.Drawing.Size(50, 24);
		this.checkBox1.TabIndex = 1;
		this.checkBox1.Text = "in0";
		this.toolTip1.SetToolTip(this.checkBox1, "E1");
		this.checkBox1.UseVisualStyleBackColor = true;
		// 
		// groupBox2
		// 
		this.groupBox2.Controls.Add(this.textBox17);
		this.groupBox2.Controls.Add(this.checkBox16);
		this.groupBox2.Controls.Add(this.textBox18);
		this.groupBox2.Controls.Add(this.checkBox15);
		this.groupBox2.Controls.Add(this.textBox19);
		this.groupBox2.Controls.Add(this.checkBox14);
		this.groupBox2.Controls.Add(this.textBox20);
		this.groupBox2.Controls.Add(this.checkBox13);
		this.groupBox2.Controls.Add(this.textBox21);
		this.groupBox2.Controls.Add(this.checkBox12);
		this.groupBox2.Controls.Add(this.textBox22);
		this.groupBox2.Controls.Add(this.checkBox11);
		this.groupBox2.Controls.Add(this.textBox23);
		this.groupBox2.Controls.Add(this.checkBox10);
		this.groupBox2.Controls.Add(this.textBox24);
		this.groupBox2.Controls.Add(this.checkBox9);
		this.groupBox2.Location = new System.Drawing.Point(275, 12);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Size = new System.Drawing.Size(248, 286);
		this.groupBox2.TabIndex = 6;
		this.groupBox2.TabStop = false;
		this.groupBox2.Text = "750-530 (8DO)";
		// 
		// textBox17
		// 
		this.textBox17.Enabled = false;
		this.textBox17.Location = new System.Drawing.Point(77, 248);
		this.textBox17.Name = "textBox17";
		this.textBox17.Size = new System.Drawing.Size(155, 22);
		this.textBox17.TabIndex = 25;
		// 
		// checkBox16
		// 
		this.checkBox16.Location = new System.Drawing.Point(17, 245);
		this.checkBox16.Name = "checkBox16";
		this.checkBox16.Size = new System.Drawing.Size(60, 24);
		this.checkBox16.TabIndex = 17;
		this.checkBox16.Text = "out7";
		this.toolTip1.SetToolTip(this.checkBox16, "A8");
		this.checkBox16.UseVisualStyleBackColor = true;
		this.checkBox16.CheckedChanged += new System.EventHandler(this.CheckBox16CheckedChanged);
		// 
		// textBox18
		// 
		this.textBox18.Enabled = false;
		this.textBox18.Location = new System.Drawing.Point(77, 218);
		this.textBox18.Name = "textBox18";
		this.textBox18.Size = new System.Drawing.Size(155, 22);
		this.textBox18.TabIndex = 24;
		// 
		// checkBox15
		// 
		this.checkBox15.Location = new System.Drawing.Point(17, 215);
		this.checkBox15.Name = "checkBox15";
		this.checkBox15.Size = new System.Drawing.Size(60, 24);
		this.checkBox15.TabIndex = 16;
		this.checkBox15.Text = "out6";
		this.toolTip1.SetToolTip(this.checkBox15, "A7");
		this.checkBox15.UseVisualStyleBackColor = true;
		this.checkBox15.CheckedChanged += new System.EventHandler(this.CheckBox15CheckedChanged);
		// 
		// textBox19
		// 
		this.textBox19.Enabled = false;
		this.textBox19.Location = new System.Drawing.Point(77, 185);
		this.textBox19.Name = "textBox19";
		this.textBox19.Size = new System.Drawing.Size(155, 22);
		this.textBox19.TabIndex = 23;
		// 
		// checkBox14
		// 
		this.checkBox14.Location = new System.Drawing.Point(17, 185);
		this.checkBox14.Name = "checkBox14";
		this.checkBox14.Size = new System.Drawing.Size(60, 24);
		this.checkBox14.TabIndex = 15;
		this.checkBox14.Text = "out5";
		this.toolTip1.SetToolTip(this.checkBox14, "A6");
		this.checkBox14.UseVisualStyleBackColor = true;
		this.checkBox14.CheckedChanged += new System.EventHandler(this.CheckBox14CheckedChanged);
		// 
		// textBox20
		// 
		this.textBox20.Enabled = false;
		this.textBox20.Location = new System.Drawing.Point(77, 153);
		this.textBox20.Name = "textBox20";
		this.textBox20.Size = new System.Drawing.Size(155, 22);
		this.textBox20.TabIndex = 22;
		// 
		// checkBox13
		// 
		this.checkBox13.Location = new System.Drawing.Point(17, 155);
		this.checkBox13.Name = "checkBox13";
		this.checkBox13.Size = new System.Drawing.Size(60, 24);
		this.checkBox13.TabIndex = 14;
		this.checkBox13.Text = "out4";
		this.toolTip1.SetToolTip(this.checkBox13, "A5");
		this.checkBox13.UseVisualStyleBackColor = true;
		this.checkBox13.CheckedChanged += new System.EventHandler(this.CheckBox13CheckedChanged);
		// 
		// textBox21
		// 
		this.textBox21.Enabled = false;
		this.textBox21.Location = new System.Drawing.Point(77, 125);
		this.textBox21.Name = "textBox21";
		this.textBox21.Size = new System.Drawing.Size(155, 22);
		this.textBox21.TabIndex = 21;
		// 
		// checkBox12
		// 
		this.checkBox12.Location = new System.Drawing.Point(17, 125);
		this.checkBox12.Name = "checkBox12";
		this.checkBox12.Size = new System.Drawing.Size(60, 24);
		this.checkBox12.TabIndex = 13;
		this.checkBox12.Text = "out3";
		this.toolTip1.SetToolTip(this.checkBox12, "A4");
		this.checkBox12.UseVisualStyleBackColor = true;
		this.checkBox12.CheckedChanged += new System.EventHandler(this.CheckBox12CheckedChanged);
		// 
		// textBox22
		// 
		this.textBox22.Enabled = false;
		this.textBox22.Location = new System.Drawing.Point(77, 97);
		this.textBox22.Name = "textBox22";
		this.textBox22.Size = new System.Drawing.Size(155, 22);
		this.textBox22.TabIndex = 20;
		// 
		// checkBox11
		// 
		this.checkBox11.Location = new System.Drawing.Point(17, 95);
		this.checkBox11.Name = "checkBox11";
		this.checkBox11.Size = new System.Drawing.Size(60, 24);
		this.checkBox11.TabIndex = 12;
		this.checkBox11.Text = "out2";
		this.toolTip1.SetToolTip(this.checkBox11, "A3");
		this.checkBox11.UseVisualStyleBackColor = true;
		this.checkBox11.CheckedChanged += new System.EventHandler(this.CheckBox11CheckedChanged);
		// 
		// textBox23
		// 
		this.textBox23.Enabled = false;
		this.textBox23.Location = new System.Drawing.Point(77, 67);
		this.textBox23.Name = "textBox23";
		this.textBox23.Size = new System.Drawing.Size(155, 22);
		this.textBox23.TabIndex = 19;
		// 
		// checkBox10
		// 
		this.checkBox10.Location = new System.Drawing.Point(17, 65);
		this.checkBox10.Name = "checkBox10";
		this.checkBox10.Size = new System.Drawing.Size(60, 24);
		this.checkBox10.TabIndex = 11;
		this.checkBox10.Text = "out1";
		this.toolTip1.SetToolTip(this.checkBox10, "A2");
		this.checkBox10.UseVisualStyleBackColor = true;
		this.checkBox10.CheckedChanged += new System.EventHandler(this.CheckBox10CheckedChanged);
		// 
		// textBox24
		// 
		this.textBox24.Enabled = false;
		this.textBox24.Location = new System.Drawing.Point(77, 37);
		this.textBox24.Name = "textBox24";
		this.textBox24.Size = new System.Drawing.Size(155, 22);
		this.textBox24.TabIndex = 18;
		// 
		// checkBox9
		// 
		this.checkBox9.Location = new System.Drawing.Point(17, 35);
		this.checkBox9.Name = "checkBox9";
		this.checkBox9.Size = new System.Drawing.Size(60, 24);
		this.checkBox9.TabIndex = 10;
		this.checkBox9.Text = "out0";
		this.toolTip1.SetToolTip(this.checkBox9, "A1");
		this.checkBox9.UseVisualStyleBackColor = true;
		this.checkBox9.CheckedChanged += new System.EventHandler(this.CheckBox9CheckedChanged);
		// 
		// groupBox3
		// 
		this.groupBox3.Controls.Add(this.textBox25);
		this.groupBox3.Controls.Add(this.label8);
		this.groupBox3.Controls.Add(this.textBox26);
		this.groupBox3.Controls.Add(this.label7);
		this.groupBox3.Controls.Add(this.textBox27);
		this.groupBox3.Controls.Add(this.label6);
		this.groupBox3.Controls.Add(this.textBox28);
		this.groupBox3.Controls.Add(this.label5);
		this.groupBox3.Controls.Add(this.textBox8);
		this.groupBox3.Controls.Add(this.textBox7);
		this.groupBox3.Controls.Add(this.textBox6);
		this.groupBox3.Controls.Add(this.textBox5);
		this.groupBox3.Controls.Add(this.label4);
		this.groupBox3.Controls.Add(this.textBox4);
		this.groupBox3.Controls.Add(this.label3);
		this.groupBox3.Controls.Add(this.textBox3);
		this.groupBox3.Controls.Add(this.label2);
		this.groupBox3.Controls.Add(this.textBox2);
		this.groupBox3.Controls.Add(this.label1);
		this.groupBox3.Controls.Add(this.textBox1);
		this.groupBox3.Location = new System.Drawing.Point(541, 12);
		this.groupBox3.Name = "groupBox3";
		this.groupBox3.Size = new System.Drawing.Size(337, 166);
		this.groupBox3.TabIndex = 7;
		this.groupBox3.TabStop = false;
		this.groupBox3.Text = "750-468 (4AI)";
		// 
		// textBox25
		// 
		this.textBox25.Enabled = false;
		this.textBox25.Location = new System.Drawing.Point(168, 122);
		this.textBox25.Name = "textBox25";
		this.textBox25.Size = new System.Drawing.Size(155, 22);
		this.textBox25.TabIndex = 29;
		// 
		// label8
		// 
		this.label8.Location = new System.Drawing.Point(58, 123);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(30, 23);
		this.label8.TabIndex = 15;
		this.label8.Text = "ai3";
		this.toolTip1.SetToolTip(this.label8, "E4");
		// 
		// textBox26
		// 
		this.textBox26.Enabled = false;
		this.textBox26.Location = new System.Drawing.Point(168, 94);
		this.textBox26.Name = "textBox26";
		this.textBox26.Size = new System.Drawing.Size(155, 22);
		this.textBox26.TabIndex = 28;
		// 
		// label7
		// 
		this.label7.Location = new System.Drawing.Point(58, 95);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(30, 23);
		this.label7.TabIndex = 14;
		this.label7.Text = "ai2";
		this.toolTip1.SetToolTip(this.label7, "E3");
		// 
		// textBox27
		// 
		this.textBox27.Enabled = false;
		this.textBox27.Location = new System.Drawing.Point(168, 64);
		this.textBox27.Name = "textBox27";
		this.textBox27.Size = new System.Drawing.Size(155, 22);
		this.textBox27.TabIndex = 27;
		// 
		// label6
		// 
		this.label6.Location = new System.Drawing.Point(58, 65);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(30, 23);
		this.label6.TabIndex = 13;
		this.label6.Text = "ai1";
		this.toolTip1.SetToolTip(this.label6, "E2");
		// 
		// textBox28
		// 
		this.textBox28.Enabled = false;
		this.textBox28.Location = new System.Drawing.Point(168, 34);
		this.textBox28.Name = "textBox28";
		this.textBox28.Size = new System.Drawing.Size(155, 22);
		this.textBox28.TabIndex = 26;
		// 
		// label5
		// 
		this.label5.Location = new System.Drawing.Point(57, 35);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(30, 23);
		this.label5.TabIndex = 12;
		this.label5.Text = "ai0";
		this.toolTip1.SetToolTip(this.label5, "E1");
		// 
		// textBox8
		// 
		this.textBox8.Location = new System.Drawing.Point(86, 125);
		this.textBox8.Name = "textBox8";
		this.textBox8.Size = new System.Drawing.Size(50, 22);
		this.textBox8.TabIndex = 11;
		// 
		// textBox7
		// 
		this.textBox7.Location = new System.Drawing.Point(86, 97);
		this.textBox7.Name = "textBox7";
		this.textBox7.Size = new System.Drawing.Size(50, 22);
		this.textBox7.TabIndex = 10;
		// 
		// textBox6
		// 
		this.textBox6.Location = new System.Drawing.Point(86, 67);
		this.textBox6.Name = "textBox6";
		this.textBox6.Size = new System.Drawing.Size(50, 22);
		this.textBox6.TabIndex = 9;
		// 
		// textBox5
		// 
		this.textBox5.Location = new System.Drawing.Point(86, 35);
		this.textBox5.Name = "textBox5";
		this.textBox5.Size = new System.Drawing.Size(50, 22);
		this.textBox5.TabIndex = 8;
		// 
		// label4
		// 
		this.label4.Location = new System.Drawing.Point(142, 122);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(35, 23);
		this.label4.TabIndex = 7;
		this.label4.Text = "[V]";
		// 
		// textBox4
		// 
		this.textBox4.Location = new System.Drawing.Point(17, 123);
		this.textBox4.Name = "textBox4";
		this.textBox4.Size = new System.Drawing.Size(35, 22);
		this.textBox4.TabIndex = 6;
		// 
		// label3
		// 
		this.label3.Location = new System.Drawing.Point(142, 94);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(35, 23);
		this.label3.TabIndex = 5;
		this.label3.Text = "[V]";
		// 
		// textBox3
		// 
		this.textBox3.Location = new System.Drawing.Point(17, 95);
		this.textBox3.Name = "textBox3";
		this.textBox3.Size = new System.Drawing.Size(35, 22);
		this.textBox3.TabIndex = 4;
		// 
		// label2
		// 
		this.label2.Location = new System.Drawing.Point(142, 66);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(35, 23);
		this.label2.TabIndex = 3;
		this.label2.Text = "[V]";
		// 
		// textBox2
		// 
		this.textBox2.Location = new System.Drawing.Point(17, 67);
		this.textBox2.Name = "textBox2";
		this.textBox2.Size = new System.Drawing.Size(35, 22);
		this.textBox2.TabIndex = 2;
		// 
		// label1
		// 
		this.label1.Location = new System.Drawing.Point(142, 34);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(35, 23);
		this.label1.TabIndex = 1;
		this.label1.Text = "[V]";
		// 
		// textBox1
		// 
		this.textBox1.Location = new System.Drawing.Point(17, 35);
		this.textBox1.Name = "textBox1";
		this.textBox1.Size = new System.Drawing.Size(35, 22);
		this.textBox1.TabIndex = 0;
		// 
		// groupBox4
		// 
		this.groupBox4.Controls.Add(this.textBox29);
		this.groupBox4.Controls.Add(this.checkBox20);
		this.groupBox4.Controls.Add(this.textBox30);
		this.groupBox4.Controls.Add(this.checkBox19);
		this.groupBox4.Controls.Add(this.textBox31);
		this.groupBox4.Controls.Add(this.checkBox18);
		this.groupBox4.Controls.Add(this.textBox32);
		this.groupBox4.Controls.Add(this.checkBox17);
		this.groupBox4.Location = new System.Drawing.Point(893, 12);
		this.groupBox4.Name = "groupBox4";
		this.groupBox4.Size = new System.Drawing.Size(252, 166);
		this.groupBox4.TabIndex = 8;
		this.groupBox4.TabStop = false;
		this.groupBox4.Text = "750-515 (4DO-Relay)";
		// 
		// textBox29
		// 
		this.textBox29.Enabled = false;
		this.textBox29.Location = new System.Drawing.Point(76, 123);
		this.textBox29.Name = "textBox29";
		this.textBox29.Size = new System.Drawing.Size(155, 22);
		this.textBox29.TabIndex = 29;
		// 
		// checkBox20
		// 
		this.checkBox20.Location = new System.Drawing.Point(18, 125);
		this.checkBox20.Name = "checkBox20";
		this.checkBox20.Size = new System.Drawing.Size(230, 24);
		this.checkBox20.TabIndex = 21;
		this.checkBox20.Text = "out3";
		this.toolTip1.SetToolTip(this.checkBox20, "43/44");
		this.checkBox20.UseVisualStyleBackColor = true;
		this.checkBox20.CheckedChanged += new System.EventHandler(this.CheckBox20CheckedChanged);
		// 
		// textBox30
		// 
		this.textBox30.Enabled = false;
		this.textBox30.Location = new System.Drawing.Point(76, 95);
		this.textBox30.Name = "textBox30";
		this.textBox30.Size = new System.Drawing.Size(155, 22);
		this.textBox30.TabIndex = 28;
		// 
		// checkBox19
		// 
		this.checkBox19.Location = new System.Drawing.Point(18, 95);
		this.checkBox19.Name = "checkBox19";
		this.checkBox19.Size = new System.Drawing.Size(230, 24);
		this.checkBox19.TabIndex = 20;
		this.checkBox19.Text = "out2";
		this.toolTip1.SetToolTip(this.checkBox19, "33/34");
		this.checkBox19.UseVisualStyleBackColor = true;
		this.checkBox19.CheckedChanged += new System.EventHandler(this.CheckBox19CheckedChanged);
		// 
		// textBox31
		// 
		this.textBox31.Enabled = false;
		this.textBox31.Location = new System.Drawing.Point(76, 65);
		this.textBox31.Name = "textBox31";
		this.textBox31.Size = new System.Drawing.Size(155, 22);
		this.textBox31.TabIndex = 27;
		// 
		// checkBox18
		// 
		this.checkBox18.Location = new System.Drawing.Point(18, 65);
		this.checkBox18.Name = "checkBox18";
		this.checkBox18.Size = new System.Drawing.Size(230, 24);
		this.checkBox18.TabIndex = 19;
		this.checkBox18.Text = "out1";
		this.toolTip1.SetToolTip(this.checkBox18, "23/24");
		this.checkBox18.UseVisualStyleBackColor = true;
		this.checkBox18.CheckedChanged += new System.EventHandler(this.CheckBox18CheckedChanged);
		// 
		// textBox32
		// 
		this.textBox32.Enabled = false;
		this.textBox32.Location = new System.Drawing.Point(76, 35);
		this.textBox32.Name = "textBox32";
		this.textBox32.Size = new System.Drawing.Size(155, 22);
		this.textBox32.TabIndex = 26;
		// 
		// checkBox17
		// 
		this.checkBox17.Location = new System.Drawing.Point(18, 35);
		this.checkBox17.Name = "checkBox17";
		this.checkBox17.Size = new System.Drawing.Size(230, 24);
		this.checkBox17.TabIndex = 18;
		this.checkBox17.Text = "out0";
		this.toolTip1.SetToolTip(this.checkBox17, "13/14");
		this.checkBox17.UseVisualStyleBackColor = true;
		this.checkBox17.CheckedChanged += new System.EventHandler(this.CheckBox17CheckedChanged);
		// 
		// checkBoxLock
		// 
		this.checkBoxLock.Checked = true;
		this.checkBoxLock.CheckState = System.Windows.Forms.CheckState.Checked;
		this.checkBoxLock.Location = new System.Drawing.Point(709, 207);
		this.checkBoxLock.Name = "checkBoxLock";
		this.checkBoxLock.Size = new System.Drawing.Size(169, 24);
		this.checkBoxLock.TabIndex = 9;
		this.checkBoxLock.Text = "uncheck to unlock names";
		this.checkBoxLock.UseVisualStyleBackColor = true;
		this.checkBoxLock.CheckedChanged += new System.EventHandler(this.CheckBoxLockCheckedChanged);
		// 
		// MainForm
		// 
		this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
		this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.ClientSize = new System.Drawing.Size(1161, 330);
		this.Controls.Add(this.checkBoxLock);
		this.Controls.Add(this.groupBox4);
		this.Controls.Add(this.groupBox3);
		this.Controls.Add(this.groupBox2);
		this.Controls.Add(this.groupBox1);
		this.Controls.Add(this.groupBoxCOM);
		this.Name = "MainForm";
		this.Text = "WAGO GUI 750-315/300-000";
		this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainFormFormClosing);
		this.DoubleClick += new System.EventHandler(this.MainFormDoubleClick);
		this.groupBoxCOM.ResumeLayout(false);
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		this.groupBox2.ResumeLayout(false);
		this.groupBox2.PerformLayout();
		this.groupBox3.ResumeLayout(false);
		this.groupBox3.PerformLayout();
		this.groupBox4.ResumeLayout(false);
		this.groupBox4.PerformLayout();
		this.ResumeLayout(false);
	}
	private System.Windows.Forms.CheckBox checkBoxLock;
	private System.Windows.Forms.TextBox textBox32;
	private System.Windows.Forms.TextBox textBox31;
	private System.Windows.Forms.TextBox textBox30;
	private System.Windows.Forms.TextBox textBox29;
	private System.Windows.Forms.TextBox textBox28;
	private System.Windows.Forms.TextBox textBox27;
	private System.Windows.Forms.TextBox textBox26;
	private System.Windows.Forms.TextBox textBox25;
	private System.Windows.Forms.TextBox textBox24;
	private System.Windows.Forms.TextBox textBox23;
	private System.Windows.Forms.TextBox textBox22;
	private System.Windows.Forms.TextBox textBox21;
	private System.Windows.Forms.TextBox textBox20;
	private System.Windows.Forms.TextBox textBox19;
	private System.Windows.Forms.TextBox textBox18;
	private System.Windows.Forms.TextBox textBox17;
	private System.Windows.Forms.TextBox textBox9;
	private System.Windows.Forms.TextBox textBox10;
	private System.Windows.Forms.TextBox textBox11;
	private System.Windows.Forms.TextBox textBox12;
	private System.Windows.Forms.TextBox textBox13;
	private System.Windows.Forms.TextBox textBox14;
	private System.Windows.Forms.TextBox textBox15;
	private System.Windows.Forms.TextBox textBox16;
	private System.Windows.Forms.ToolTip toolTip1;
	private System.Windows.Forms.TextBox textBox5;
	private System.Windows.Forms.TextBox textBox6;
	private System.Windows.Forms.TextBox textBox7;
	private System.Windows.Forms.TextBox textBox8;
	private System.Windows.Forms.Label label5;
	private System.Windows.Forms.Label label6;
	private System.Windows.Forms.Label label7;
	private System.Windows.Forms.Label label8;
	private System.Windows.Forms.CheckBox checkBox17;
	private System.Windows.Forms.CheckBox checkBox18;
	private System.Windows.Forms.CheckBox checkBox19;
	private System.Windows.Forms.CheckBox checkBox20;
	private System.Windows.Forms.TextBox textBox1;
	private System.Windows.Forms.Label label1;
	private System.Windows.Forms.TextBox textBox2;
	private System.Windows.Forms.Label label2;
	private System.Windows.Forms.TextBox textBox3;
	private System.Windows.Forms.Label label3;
	private System.Windows.Forms.TextBox textBox4;
	private System.Windows.Forms.Label label4;
	private System.Windows.Forms.GroupBox groupBox4;
	private System.Windows.Forms.GroupBox groupBox3;
	private System.Windows.Forms.CheckBox checkBox9;
	private System.Windows.Forms.CheckBox checkBox10;
	private System.Windows.Forms.CheckBox checkBox11;
	private System.Windows.Forms.CheckBox checkBox12;
	private System.Windows.Forms.CheckBox checkBox13;
	private System.Windows.Forms.CheckBox checkBox14;
	private System.Windows.Forms.CheckBox checkBox15;
	private System.Windows.Forms.CheckBox checkBox16;
	private System.Windows.Forms.GroupBox groupBox2;
	private System.Windows.Forms.CheckBox checkBox1;
	private System.Windows.Forms.CheckBox checkBox2;
	private System.Windows.Forms.CheckBox checkBox3;
	private System.Windows.Forms.CheckBox checkBox4;
	private System.Windows.Forms.CheckBox checkBox5;
	private System.Windows.Forms.CheckBox checkBox6;
	private System.Windows.Forms.CheckBox checkBox7;
	private System.Windows.Forms.CheckBox checkBox8;
	private System.Windows.Forms.GroupBox groupBox1;
	private System.Windows.Forms.GroupBox groupBoxCOM;
	private System.Windows.Forms.ComboBox comboBoxSelectCOM;
	private System.Windows.Forms.Button connectCOM;

}
