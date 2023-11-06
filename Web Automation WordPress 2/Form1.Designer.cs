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
            HotelUrlBox1 = new TextBox();
            FolderPath1Btn1 = new Button();
            FolderPath1 = new TextBox();
            groupBox7 = new GroupBox();
            label2 = new Label();
            crollBox1 = new TextBox();
            RenameBtn1 = new Button();
            CrollBtn1 = new Button();
            groupBox2 = new GroupBox();
            UrlBox1 = new TextBox();
            groupBox6 = new GroupBox();
            APIKeybox1 = new TextBox();
            groupBox4 = new GroupBox();
            IdBox1 = new TextBox();
            groupBox5 = new GroupBox();
            PwBox1 = new TextBox();
            groupBox12 = new GroupBox();
            LoadBtn1 = new Button();
            SaveBtn1 = new Button();
            label8 = new Label();
            SystemBox1 = new TextBox();
            label5 = new Label();
            groupBox10 = new GroupBox();
            label3 = new Label();
            AffiliateBox1 = new TextBox();
            button1 = new Button();
            CategoryBox1 = new TextBox();
            label7 = new Label();
            label1 = new Label();
            HotelListBox1 = new TextBox();
            groupBox1.SuspendLayout();
            groupBox7.SuspendLayout();
            groupBox2.SuspendLayout();
            groupBox6.SuspendLayout();
            groupBox4.SuspendLayout();
            groupBox5.SuspendLayout();
            groupBox12.SuspendLayout();
            groupBox10.SuspendLayout();
            SuspendLayout();
            // 
            // StartBtn1
            // 
            StartBtn1.BackColor = Color.Silver;
            StartBtn1.Cursor = Cursors.Hand;
            StartBtn1.Font = new Font("맑은 고딕", 12F, FontStyle.Bold, GraphicsUnit.Point);
            StartBtn1.ForeColor = Color.Black;
            StartBtn1.Location = new Point(185, 301);
            StartBtn1.Name = "StartBtn1";
            StartBtn1.Size = new Size(196, 43);
            StartBtn1.TabIndex = 9;
            StartBtn1.Text = "실행";
            StartBtn1.UseVisualStyleBackColor = false;
            StartBtn1.Click += StartBtn1_Click;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(LogBox1);
            groupBox1.Font = new Font("맑은 고딕", 9.75F, FontStyle.Bold, GraphicsUnit.Point);
            groupBox1.Location = new Point(11, 433);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(649, 151);
            groupBox1.TabIndex = 999;
            groupBox1.TabStop = false;
            groupBox1.Text = "작업로그";
            // 
            // LogBox1
            // 
            LogBox1.BackColor = Color.LightGray;
            LogBox1.BorderStyle = BorderStyle.FixedSingle;
            LogBox1.ForeColor = Color.Black;
            LogBox1.Location = new Point(6, 26);
            LogBox1.Multiline = true;
            LogBox1.Name = "LogBox1";
            LogBox1.ReadOnly = true;
            LogBox1.Size = new Size(633, 119);
            LogBox1.TabIndex = 0;
            // 
            // HotelUrlBox1
            // 
            HotelUrlBox1.BackColor = Color.FromArgb(224, 224, 224);
            HotelUrlBox1.BorderStyle = BorderStyle.FixedSingle;
            HotelUrlBox1.Cursor = Cursors.IBeam;
            HotelUrlBox1.Font = new Font("굴림", 9F, FontStyle.Regular, GraphicsUnit.Point);
            HotelUrlBox1.ForeColor = Color.Black;
            HotelUrlBox1.Location = new Point(91, 65);
            HotelUrlBox1.Multiline = true;
            HotelUrlBox1.Name = "HotelUrlBox1";
            HotelUrlBox1.Size = new Size(285, 47);
            HotelUrlBox1.TabIndex = 3;
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
            groupBox7.Controls.Add(label2);
            groupBox7.Controls.Add(crollBox1);
            groupBox7.Controls.Add(RenameBtn1);
            groupBox7.Controls.Add(CrollBtn1);
            groupBox7.Controls.Add(groupBox2);
            groupBox7.Controls.Add(groupBox6);
            groupBox7.Controls.Add(groupBox4);
            groupBox7.Controls.Add(groupBox5);
            groupBox7.Controls.Add(groupBox12);
            groupBox7.Font = new Font("맑은 고딕", 9F, FontStyle.Bold, GraphicsUnit.Point);
            groupBox7.Location = new Point(11, 11);
            groupBox7.Margin = new Padding(2);
            groupBox7.Name = "groupBox7";
            groupBox7.Padding = new Padding(2);
            groupBox7.Size = new Size(259, 349);
            groupBox7.TabIndex = 999;
            groupBox7.TabStop = false;
            groupBox7.Text = "API 옵션";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("맑은 고딕", 9F, FontStyle.Regular, GraphicsUnit.Point);
            label2.Location = new Point(9, 321);
            label2.Name = "label2";
            label2.Size = new Size(51, 15);
            label2.TabIndex = 1010;
            label2.Text = "크롤링*:";
            // 
            // crollBox1
            // 
            crollBox1.BackColor = Color.FromArgb(224, 224, 224);
            crollBox1.BorderStyle = BorderStyle.FixedSingle;
            crollBox1.Font = new Font("굴림", 9F, FontStyle.Regular, GraphicsUnit.Point);
            crollBox1.Location = new Point(77, 317);
            crollBox1.Margin = new Padding(2);
            crollBox1.Name = "crollBox1";
            crollBox1.Size = new Size(96, 21);
            crollBox1.TabIndex = 1009;
            // 
            // RenameBtn1
            // 
            RenameBtn1.Location = new Point(178, 244);
            RenameBtn1.Name = "RenameBtn1";
            RenameBtn1.Size = new Size(75, 43);
            RenameBtn1.TabIndex = 1008;
            RenameBtn1.Text = "파일 정리";
            RenameBtn1.UseVisualStyleBackColor = true;
            RenameBtn1.Click += RenameBtn1_Click;
            // 
            // CrollBtn1
            // 
            CrollBtn1.Location = new Point(178, 316);
            CrollBtn1.Name = "CrollBtn1";
            CrollBtn1.Size = new Size(75, 23);
            CrollBtn1.TabIndex = 1007;
            CrollBtn1.Text = "크롤링";
            CrollBtn1.UseVisualStyleBackColor = true;
            CrollBtn1.Click += CrollBtn1_Click;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(UrlBox1);
            groupBox2.Font = new Font("맑은 고딕", 9F, FontStyle.Bold, GraphicsUnit.Point);
            groupBox2.Location = new Point(4, 182);
            groupBox2.Margin = new Padding(2);
            groupBox2.Name = "groupBox2";
            groupBox2.Padding = new Padding(2);
            groupBox2.Size = new Size(249, 48);
            groupBox2.TabIndex = 999;
            groupBox2.TabStop = false;
            groupBox2.Text = "WP_URL(필수)";
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
            UrlBox1.TextChanged += UrlBox1_TextChanged;
            // 
            // groupBox6
            // 
            groupBox6.Controls.Add(APIKeybox1);
            groupBox6.Font = new Font("맑은 고딕", 9F, FontStyle.Bold, GraphicsUnit.Point);
            groupBox6.Location = new Point(4, 20);
            groupBox6.Margin = new Padding(2);
            groupBox6.Name = "groupBox6";
            groupBox6.Padding = new Padding(2);
            groupBox6.Size = new Size(249, 48);
            groupBox6.TabIndex = 999;
            groupBox6.TabStop = false;
            groupBox6.Text = "API Key(필수)";
            // 
            // APIKeybox1
            // 
            APIKeybox1.BackColor = Color.FromArgb(224, 224, 224);
            APIKeybox1.BorderStyle = BorderStyle.FixedSingle;
            APIKeybox1.Location = new Point(5, 19);
            APIKeybox1.Margin = new Padding(2);
            APIKeybox1.Name = "APIKeybox1";
            APIKeybox1.Size = new Size(240, 23);
            APIKeybox1.TabIndex = 0;
            APIKeybox1.TextChanged += APIKeybox1_TextChanged;
            // 
            // groupBox4
            // 
            groupBox4.Controls.Add(IdBox1);
            groupBox4.Font = new Font("맑은 고딕", 9F, FontStyle.Bold, GraphicsUnit.Point);
            groupBox4.Location = new Point(4, 72);
            groupBox4.Margin = new Padding(2);
            groupBox4.Name = "groupBox4";
            groupBox4.Padding = new Padding(2);
            groupBox4.Size = new Size(249, 48);
            groupBox4.TabIndex = 999;
            groupBox4.TabStop = false;
            groupBox4.Text = "WP_ID(필수)";
            // 
            // IdBox1
            // 
            IdBox1.BackColor = Color.FromArgb(224, 224, 224);
            IdBox1.BorderStyle = BorderStyle.FixedSingle;
            IdBox1.Location = new Point(5, 19);
            IdBox1.Margin = new Padding(2);
            IdBox1.Name = "IdBox1";
            IdBox1.Size = new Size(240, 23);
            IdBox1.TabIndex = 0;
            IdBox1.TextChanged += IdBox1_TextChanged;
            // 
            // groupBox5
            // 
            groupBox5.Controls.Add(PwBox1);
            groupBox5.Font = new Font("맑은 고딕", 9F, FontStyle.Bold, GraphicsUnit.Point);
            groupBox5.Location = new Point(4, 127);
            groupBox5.Margin = new Padding(2);
            groupBox5.Name = "groupBox5";
            groupBox5.Padding = new Padding(2);
            groupBox5.Size = new Size(249, 48);
            groupBox5.TabIndex = 999;
            groupBox5.TabStop = false;
            groupBox5.Text = "WP_PW(필수)";
            // 
            // PwBox1
            // 
            PwBox1.BackColor = Color.FromArgb(224, 224, 224);
            PwBox1.BorderStyle = BorderStyle.FixedSingle;
            PwBox1.Location = new Point(5, 19);
            PwBox1.Margin = new Padding(2);
            PwBox1.Name = "PwBox1";
            PwBox1.Size = new Size(240, 23);
            PwBox1.TabIndex = 0;
            PwBox1.TextChanged += PwBox1_TextChanged;
            // 
            // groupBox12
            // 
            groupBox12.Controls.Add(FolderPath1Btn1);
            groupBox12.Controls.Add(FolderPath1);
            groupBox12.Font = new Font("맑은 고딕", 9F, FontStyle.Bold, GraphicsUnit.Point);
            groupBox12.Location = new Point(4, 238);
            groupBox12.Margin = new Padding(2);
            groupBox12.Name = "groupBox12";
            groupBox12.Padding = new Padding(2);
            groupBox12.Size = new Size(169, 49);
            groupBox12.TabIndex = 7;
            groupBox12.TabStop = false;
            groupBox12.Text = "폴더 경로(필수)";
            // 
            // LoadBtn1
            // 
            LoadBtn1.Location = new Point(306, 272);
            LoadBtn1.Name = "LoadBtn1";
            LoadBtn1.Size = new Size(75, 23);
            LoadBtn1.TabIndex = 1006;
            LoadBtn1.Text = "불러오기";
            LoadBtn1.UseVisualStyleBackColor = true;
            LoadBtn1.Click += LoadBtn1_Click;
            // 
            // SaveBtn1
            // 
            SaveBtn1.Location = new Point(225, 272);
            SaveBtn1.Name = "SaveBtn1";
            SaveBtn1.Size = new Size(75, 23);
            SaveBtn1.TabIndex = 1005;
            SaveBtn1.Text = "저장";
            SaveBtn1.UseVisualStyleBackColor = true;
            SaveBtn1.Click += SaveBtn1_Click;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Font = new Font("맑은 고딕", 9F, FontStyle.Regular, GraphicsUnit.Point);
            label8.Location = new Point(9, 65);
            label8.Name = "label8";
            label8.Size = new Size(64, 15);
            label8.TabIndex = 1005;
            label8.Text = "호텔 URL*:";
            // 
            // SystemBox1
            // 
            SystemBox1.BackColor = Color.FromArgb(224, 224, 224);
            SystemBox1.BorderStyle = BorderStyle.FixedSingle;
            SystemBox1.Font = new Font("굴림", 9F, FontStyle.Regular, GraphicsUnit.Point);
            SystemBox1.Location = new Point(91, 31);
            SystemBox1.Margin = new Padding(2);
            SystemBox1.Name = "SystemBox1";
            SystemBox1.Size = new Size(96, 21);
            SystemBox1.TabIndex = 1;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("맑은 고딕", 9F, FontStyle.Regular, GraphicsUnit.Point);
            label5.Location = new Point(18, 35);
            label5.Name = "label5";
            label5.Size = new Size(55, 15);
            label5.TabIndex = 1003;
            label5.Text = "글 주제*:";
            // 
            // groupBox10
            // 
            groupBox10.Controls.Add(label1);
            groupBox10.Controls.Add(HotelListBox1);
            groupBox10.Controls.Add(label3);
            groupBox10.Controls.Add(AffiliateBox1);
            groupBox10.Controls.Add(button1);
            groupBox10.Controls.Add(CategoryBox1);
            groupBox10.Controls.Add(LoadBtn1);
            groupBox10.Controls.Add(label8);
            groupBox10.Controls.Add(SaveBtn1);
            groupBox10.Controls.Add(StartBtn1);
            groupBox10.Controls.Add(label5);
            groupBox10.Controls.Add(label7);
            groupBox10.Controls.Add(HotelUrlBox1);
            groupBox10.Controls.Add(SystemBox1);
            groupBox10.Font = new Font("맑은 고딕", 9F, FontStyle.Bold, GraphicsUnit.Point);
            groupBox10.Location = new Point(274, 11);
            groupBox10.Margin = new Padding(2);
            groupBox10.Name = "groupBox10";
            groupBox10.Padding = new Padding(2);
            groupBox10.Size = new Size(386, 349);
            groupBox10.TabIndex = 1000;
            groupBox10.TabStop = false;
            groupBox10.Text = "포스팅 옵션_Ver 2.0";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("맑은 고딕", 9F, FontStyle.Regular, GraphicsUnit.Point);
            label3.Location = new Point(17, 121);
            label3.Name = "label3";
            label3.Size = new Size(56, 15);
            label3.TabIndex = 1009;
            label3.Text = "Affiliate*:";
            // 
            // AffiliateBox1
            // 
            AffiliateBox1.BackColor = Color.Gray;
            AffiliateBox1.BorderStyle = BorderStyle.FixedSingle;
            AffiliateBox1.Font = new Font("굴림", 9F, FontStyle.Regular, GraphicsUnit.Point);
            AffiliateBox1.Location = new Point(91, 121);
            AffiliateBox1.Margin = new Padding(2);
            AffiliateBox1.Multiline = true;
            AffiliateBox1.Name = "AffiliateBox1";
            AffiliateBox1.ReadOnly = true;
            AffiliateBox1.Size = new Size(285, 47);
            AffiliateBox1.TabIndex = 4;
            // 
            // button1
            // 
            button1.Location = new Point(144, 272);
            button1.Name = "button1";
            button1.Size = new Size(75, 23);
            button1.TabIndex = 1006;
            button1.Text = "테스트";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // CategoryBox1
            // 
            CategoryBox1.BackColor = Color.FromArgb(224, 224, 224);
            CategoryBox1.BorderStyle = BorderStyle.FixedSingle;
            CategoryBox1.Cursor = Cursors.IBeam;
            CategoryBox1.Font = new Font("굴림", 9F, FontStyle.Regular, GraphicsUnit.Point);
            CategoryBox1.ForeColor = Color.Black;
            CategoryBox1.Location = new Point(280, 31);
            CategoryBox1.Name = "CategoryBox1";
            CategoryBox1.Size = new Size(96, 21);
            CategoryBox1.TabIndex = 2;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Font = new Font("맑은 고딕", 9F, FontStyle.Regular, GraphicsUnit.Point);
            label7.Location = new Point(199, 33);
            label7.Name = "label7";
            label7.Size = new Size(63, 15);
            label7.TabIndex = 1001;
            label7.Text = "카테고리*:";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("맑은 고딕", 9F, FontStyle.Regular, GraphicsUnit.Point);
            label1.Location = new Point(6, 209);
            label1.Name = "label1";
            label1.Size = new Size(67, 15);
            label1.TabIndex = 1011;
            label1.Text = "호텔 목록*:";
            // 
            // HotelListBox1
            // 
            HotelListBox1.BackColor = Color.FromArgb(224, 224, 224);
            HotelListBox1.BorderStyle = BorderStyle.FixedSingle;
            HotelListBox1.Cursor = Cursors.IBeam;
            HotelListBox1.Font = new Font("굴림", 9F, FontStyle.Regular, GraphicsUnit.Point);
            HotelListBox1.ForeColor = Color.Black;
            HotelListBox1.Location = new Point(91, 209);
            HotelListBox1.Multiline = true;
            HotelListBox1.Name = "HotelListBox1";
            HotelListBox1.Size = new Size(285, 47);
            HotelListBox1.TabIndex = 1010;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(674, 596);
            Controls.Add(groupBox10);
            Controls.Add(groupBox7);
            Controls.Add(groupBox1);
            Name = "Form1";
            Text = "Auto Posting for WordPress";
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            groupBox7.ResumeLayout(false);
            groupBox7.PerformLayout();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            groupBox6.ResumeLayout(false);
            groupBox6.PerformLayout();
            groupBox4.ResumeLayout(false);
            groupBox4.PerformLayout();
            groupBox5.ResumeLayout(false);
            groupBox5.PerformLayout();
            groupBox12.ResumeLayout(false);
            groupBox12.PerformLayout();
            groupBox10.ResumeLayout(false);
            groupBox10.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Button StartBtn1;
        private GroupBox groupBox1;
        private TextBox LogBox1;
        private TextBox HotelUrlBox1;
        private Button FolderPath1Btn1;
        private TextBox FolderPath1;
        private GroupBox groupBox7;
        private TextBox SystemBox1;
        private Label label5;
        private GroupBox groupBox12;
        private GroupBox groupBox6;
        private TextBox APIKeybox1;
        private GroupBox groupBox10;
        private Button LoadBtn1;
        private Button SaveBtn1;
        private Label label7;
        private GroupBox groupBox4;
        private TextBox IdBox1;
        private GroupBox groupBox5;
        private TextBox PwBox1;
        private Label label8;
        private GroupBox groupBox2;
        private TextBox UrlBox1;
        private Button RenameBtn1;
        private Button CrollBtn1;
        private Button button1;
        private TextBox CategoryBox1;
        private Label label2;
        private TextBox crollBox1;
        private Label label3;
        private TextBox AffiliateBox1;
        private Label label1;
        private TextBox HotelListBox1;
    }
}