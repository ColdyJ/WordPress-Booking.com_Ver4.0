namespace Web_Automation_WordPress_2
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

		#region Windows Form Designer generated code

		/// <summary>
		///  Required method for Designer support - do not modify
		///  the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			StartBtn1 = new Button();
			groupBox1 = new GroupBox();
			LogBox1 = new TextBox();
			FolderPath1Btn1 = new Button();
			FolderPath1 = new TextBox();
			groupBox7 = new GroupBox();
			button1 = new Button();
			groupBox4 = new GroupBox();
			HotelListBox1 = new TextBox();
			RenameBtn1 = new Button();
			groupBox2 = new GroupBox();
			UrlBox1 = new TextBox();
			groupBox12 = new GroupBox();
			groupBox3 = new GroupBox();
			LogBox2 = new TextBox();
			groupBox1.SuspendLayout();
			groupBox7.SuspendLayout();
			groupBox4.SuspendLayout();
			groupBox2.SuspendLayout();
			groupBox12.SuspendLayout();
			groupBox3.SuspendLayout();
			SuspendLayout();
			// 
			// StartBtn1
			// 
			StartBtn1.BackColor = Color.Silver;
			StartBtn1.Cursor = Cursors.Hand;
			StartBtn1.Font = new Font("맑은 고딕", 12F, FontStyle.Bold, GraphicsUnit.Point);
			StartBtn1.ForeColor = Color.Black;
			StartBtn1.Location = new Point(111, 275);
			StartBtn1.Name = "StartBtn1";
			StartBtn1.Size = new Size(140, 63);
			StartBtn1.TabIndex = 9;
			StartBtn1.Text = "실행";
			StartBtn1.UseVisualStyleBackColor = false;
			StartBtn1.Click += StartBtn1_Click;
			// 
			// groupBox1
			// 
			groupBox1.Controls.Add(LogBox1);
			groupBox1.Font = new Font("맑은 고딕", 9.75F, FontStyle.Bold, GraphicsUnit.Point);
			groupBox1.Location = new Point(275, 11);
			groupBox1.Name = "groupBox1";
			groupBox1.Size = new Size(344, 165);
			groupBox1.TabIndex = 999;
			groupBox1.TabStop = false;
			groupBox1.Text = "진행 로그(Detail)";
			// 
			// LogBox1
			// 
			LogBox1.BackColor = Color.LightGray;
			LogBox1.BorderStyle = BorderStyle.FixedSingle;
			LogBox1.ForeColor = Color.Black;
			LogBox1.Location = new Point(6, 23);
			LogBox1.Multiline = true;
			LogBox1.Name = "LogBox1";
			LogBox1.ReadOnly = true;
			LogBox1.Size = new Size(328, 136);
			LogBox1.TabIndex = 0;
			// 
			// FolderPath1Btn1
			// 
			FolderPath1Btn1.BackColor = Color.LightGray;
			FolderPath1Btn1.Cursor = Cursors.Hand;
			FolderPath1Btn1.Location = new Point(131, 20);
			FolderPath1Btn1.Name = "FolderPath1Btn1";
			FolderPath1Btn1.Size = new Size(33, 23);
			FolderPath1Btn1.TabIndex = 8;
			FolderPath1Btn1.Text = "...";
			FolderPath1Btn1.UseVisualStyleBackColor = false;
			FolderPath1Btn1.Click += FolderPath1Btn1_Click;
			// 
			// FolderPath1
			// 
			FolderPath1.BackColor = Color.LightGray;
			FolderPath1.BorderStyle = BorderStyle.FixedSingle;
			FolderPath1.ForeColor = Color.Black;
			FolderPath1.Location = new Point(5, 20);
			FolderPath1.Name = "FolderPath1";
			FolderPath1.ReadOnly = true;
			FolderPath1.Size = new Size(120, 23);
			FolderPath1.TabIndex = 7;
			// 
			// groupBox7
			// 
			groupBox7.Controls.Add(StartBtn1);
			groupBox7.Controls.Add(button1);
			groupBox7.Controls.Add(groupBox4);
			groupBox7.Controls.Add(RenameBtn1);
			groupBox7.Controls.Add(groupBox2);
			groupBox7.Controls.Add(groupBox12);
			groupBox7.Font = new Font("맑은 고딕", 9F, FontStyle.Bold, GraphicsUnit.Point);
			groupBox7.Location = new Point(11, 11);
			groupBox7.Margin = new Padding(2);
			groupBox7.Name = "groupBox7";
			groupBox7.Padding = new Padding(2);
			groupBox7.Size = new Size(259, 347);
			groupBox7.TabIndex = 999;
			groupBox7.TabStop = false;
			groupBox7.Text = "포스팅 옵션_Ver 3.0";
			// 
			// button1
			// 
			button1.Location = new Point(111, 237);
			button1.Name = "button1";
			button1.Size = new Size(144, 23);
			button1.TabIndex = 1006;
			button1.Text = "호텔 목록화";
			button1.UseVisualStyleBackColor = true;
			button1.Click += button1_Click;
			// 
			// groupBox4
			// 
			groupBox4.Controls.Add(HotelListBox1);
			groupBox4.Font = new Font("맑은 고딕", 9F, FontStyle.Bold, GraphicsUnit.Point);
			groupBox4.Location = new Point(6, 153);
			groupBox4.Margin = new Padding(2);
			groupBox4.Name = "groupBox4";
			groupBox4.Padding = new Padding(2);
			groupBox4.Size = new Size(249, 81);
			groupBox4.TabIndex = 1011;
			groupBox4.TabStop = false;
			groupBox4.Text = "호텔 목록화";
			// 
			// HotelListBox1
			// 
			HotelListBox1.BackColor = Color.FromArgb(224, 224, 224);
			HotelListBox1.BorderStyle = BorderStyle.FixedSingle;
			HotelListBox1.Cursor = Cursors.IBeam;
			HotelListBox1.Font = new Font("굴림", 9F, FontStyle.Regular, GraphicsUnit.Point);
			HotelListBox1.ForeColor = Color.Black;
			HotelListBox1.Location = new Point(5, 24);
			HotelListBox1.Multiline = true;
			HotelListBox1.Name = "HotelListBox1";
			HotelListBox1.Size = new Size(235, 47);
			HotelListBox1.TabIndex = 1010;
			// 
			// RenameBtn1
			// 
			RenameBtn1.Location = new Point(180, 95);
			RenameBtn1.Name = "RenameBtn1";
			RenameBtn1.Size = new Size(75, 43);
			RenameBtn1.TabIndex = 1008;
			RenameBtn1.Text = "파일 정리";
			RenameBtn1.UseVisualStyleBackColor = true;
			RenameBtn1.Click += RenameBtn1_Click;
			// 
			// groupBox2
			// 
			groupBox2.Controls.Add(UrlBox1);
			groupBox2.Font = new Font("맑은 고딕", 9F, FontStyle.Bold, GraphicsUnit.Point);
			groupBox2.Location = new Point(6, 23);
			groupBox2.Margin = new Padding(2);
			groupBox2.Name = "groupBox2";
			groupBox2.Padding = new Padding(2);
			groupBox2.Size = new Size(249, 48);
			groupBox2.TabIndex = 999;
			groupBox2.TabStop = false;
			groupBox2.Text = "인증 ID";
			// 
			// UrlBox1
			// 
			UrlBox1.BackColor = Color.FromArgb(224, 224, 224);
			UrlBox1.BorderStyle = BorderStyle.FixedSingle;
			UrlBox1.Location = new Point(5, 19);
			UrlBox1.Margin = new Padding(2);
			UrlBox1.Name = "UrlBox1";
			UrlBox1.Size = new Size(240, 23);
			UrlBox1.TabIndex = 0;
			// 
			// groupBox12
			// 
			groupBox12.Controls.Add(FolderPath1Btn1);
			groupBox12.Controls.Add(FolderPath1);
			groupBox12.Font = new Font("맑은 고딕", 9F, FontStyle.Bold, GraphicsUnit.Point);
			groupBox12.Location = new Point(6, 89);
			groupBox12.Margin = new Padding(2);
			groupBox12.Name = "groupBox12";
			groupBox12.Padding = new Padding(2);
			groupBox12.Size = new Size(169, 49);
			groupBox12.TabIndex = 7;
			groupBox12.TabStop = false;
			groupBox12.Text = "폴더 경로(필수)";
			// 
			// groupBox3
			// 
			groupBox3.Controls.Add(LogBox2);
			groupBox3.Font = new Font("맑은 고딕", 9.75F, FontStyle.Bold, GraphicsUnit.Point);
			groupBox3.Location = new Point(275, 181);
			groupBox3.Name = "groupBox3";
			groupBox3.Size = new Size(344, 177);
			groupBox3.TabIndex = 1001;
			groupBox3.TabStop = false;
			groupBox3.Text = "작업 로그";
			// 
			// LogBox2
			// 
			LogBox2.BackColor = Color.LightGray;
			LogBox2.BorderStyle = BorderStyle.FixedSingle;
			LogBox2.ForeColor = Color.Black;
			LogBox2.Location = new Point(6, 26);
			LogBox2.Multiline = true;
			LogBox2.Name = "LogBox2";
			LogBox2.ReadOnly = true;
			LogBox2.Size = new Size(328, 141);
			LogBox2.TabIndex = 0;
			// 
			// Form1
			// 
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			BackColor = Color.White;
			ClientSize = new Size(627, 364);
			Controls.Add(groupBox3);
			Controls.Add(groupBox7);
			Controls.Add(groupBox1);
			Name = "Form1";
			Text = "Auto Posting for WordPress";
			groupBox1.ResumeLayout(false);
			groupBox1.PerformLayout();
			groupBox7.ResumeLayout(false);
			groupBox4.ResumeLayout(false);
			groupBox4.PerformLayout();
			groupBox2.ResumeLayout(false);
			groupBox2.PerformLayout();
			groupBox12.ResumeLayout(false);
			groupBox12.PerformLayout();
			groupBox3.ResumeLayout(false);
			groupBox3.PerformLayout();
			ResumeLayout(false);
		}

		#endregion

		private Button StartBtn1;
        private GroupBox groupBox1;
        private TextBox LogBox1;
        private Button FolderPath1Btn1;
        private TextBox FolderPath1;
        private GroupBox groupBox7;
        private GroupBox groupBox12;
        private Button RenameBtn1;
        private Button button1;
        private TextBox HotelListBox1;
		private GroupBox groupBox3;
		private TextBox LogBox2;
		private GroupBox groupBox2;
		private TextBox UrlBox1;
		private GroupBox groupBox4;
	}
}