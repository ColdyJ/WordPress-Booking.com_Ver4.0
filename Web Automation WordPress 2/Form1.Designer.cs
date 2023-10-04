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
            comboBox1 = new ComboBox();
            dalleBox1 = new TextBox();
            titleBox1 = new TextBox();
            gptBox1 = new TextBox();
            FolderPath1Btn1 = new Button();
            FolderPath1 = new TextBox();
            groupBox7 = new GroupBox();
            label8 = new Label();
            label2 = new Label();
            SystemBox1 = new TextBox();
            label5 = new Label();
            groupBox12 = new GroupBox();
            groupBox6 = new GroupBox();
            APIKeybox1 = new TextBox();
            groupBox10 = new GroupBox();
            groupBox2 = new GroupBox();
            UrlBox1 = new TextBox();
            LoadBtn1 = new Button();
            SaveBtn1 = new Button();
            label6 = new Label();
            label7 = new Label();
            groupBox4 = new GroupBox();
            IdBox1 = new TextBox();
            groupBox5 = new GroupBox();
            PwBox1 = new TextBox();
            groupBox1.SuspendLayout();
            groupBox7.SuspendLayout();
            groupBox12.SuspendLayout();
            groupBox6.SuspendLayout();
            groupBox10.SuspendLayout();
            groupBox2.SuspendLayout();
            groupBox4.SuspendLayout();
            groupBox5.SuspendLayout();
            SuspendLayout();
            // 
            // StartBtn1
            // 
            StartBtn1.BackColor = Color.Silver;
            StartBtn1.Cursor = Cursors.Hand;
            StartBtn1.Font = new Font("맑은 고딕", 12F, FontStyle.Bold, GraphicsUnit.Point);
            StartBtn1.ForeColor = Color.Black;
            StartBtn1.Location = new Point(180, 232);
            StartBtn1.Name = "StartBtn1";
            StartBtn1.Size = new Size(197, 35);
            StartBtn1.TabIndex = 990;
            StartBtn1.Text = "실행";
            StartBtn1.UseVisualStyleBackColor = false;
            StartBtn1.Click += StartBtn1_Click;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(LogBox1);
            groupBox1.Font = new Font("맑은 고딕", 9.75F, FontStyle.Bold, GraphicsUnit.Point);
            groupBox1.Location = new Point(11, 295);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(649, 151);
            groupBox1.TabIndex = 2;
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
            LogBox1.TabIndex = 995;
            // 
            // comboBox1
            // 
            comboBox1.BackColor = Color.LightGray;
            comboBox1.Font = new Font("굴림", 9F, FontStyle.Bold, GraphicsUnit.Point);
            comboBox1.ForeColor = Color.Black;
            comboBox1.FormattingEnabled = true;
            comboBox1.Items.AddRange(new object[] { "오희 리빙템", "오희 면접준비", "오희 잡담", "오희 지원정보" });
            comboBox1.Location = new Point(86, 222);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(163, 20);
            comboBox1.TabIndex = 997;
            // 
            // dalleBox1
            // 
            dalleBox1.BackColor = Color.LightGray;
            dalleBox1.BorderStyle = BorderStyle.FixedSingle;
            dalleBox1.Cursor = Cursors.IBeam;
            dalleBox1.Font = new Font("굴림", 9F, FontStyle.Regular, GraphicsUnit.Point);
            dalleBox1.ForeColor = Color.Black;
            dalleBox1.Location = new Point(117, 100);
            dalleBox1.Name = "dalleBox1";
            dalleBox1.Size = new Size(260, 21);
            dalleBox1.TabIndex = 3;
            // 
            // titleBox1
            // 
            titleBox1.BackColor = Color.LightGray;
            titleBox1.BorderStyle = BorderStyle.FixedSingle;
            titleBox1.Cursor = Cursors.IBeam;
            titleBox1.Font = new Font("굴림", 9F, FontStyle.Regular, GraphicsUnit.Point);
            titleBox1.ForeColor = Color.Black;
            titleBox1.Location = new Point(86, 188);
            titleBox1.Name = "titleBox1";
            titleBox1.Size = new Size(163, 21);
            titleBox1.TabIndex = 1;
            // 
            // gptBox1
            // 
            gptBox1.BackColor = Color.LightGray;
            gptBox1.BorderStyle = BorderStyle.FixedSingle;
            gptBox1.Cursor = Cursors.IBeam;
            gptBox1.Font = new Font("굴림", 9F, FontStyle.Regular, GraphicsUnit.Point);
            gptBox1.ForeColor = Color.Black;
            gptBox1.Location = new Point(117, 132);
            gptBox1.Multiline = true;
            gptBox1.Name = "gptBox1";
            gptBox1.Size = new Size(260, 77);
            gptBox1.TabIndex = 4;
            // 
            // FolderPath1Btn1
            // 
            FolderPath1Btn1.BackColor = Color.LightGray;
            FolderPath1Btn1.Cursor = Cursors.Hand;
            FolderPath1Btn1.Location = new Point(131, 20);
            FolderPath1Btn1.Name = "FolderPath1Btn1";
            FolderPath1Btn1.Size = new Size(33, 23);
            FolderPath1Btn1.TabIndex = 991;
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
            FolderPath1.TabIndex = 996;
            // 
            // groupBox7
            // 
            groupBox7.Controls.Add(label8);
            groupBox7.Controls.Add(label2);
            groupBox7.Controls.Add(gptBox1);
            groupBox7.Controls.Add(SystemBox1);
            groupBox7.Controls.Add(label5);
            groupBox7.Controls.Add(dalleBox1);
            groupBox7.Controls.Add(groupBox12);
            groupBox7.Controls.Add(groupBox6);
            groupBox7.Controls.Add(StartBtn1);
            groupBox7.Font = new Font("맑은 고딕", 9F, FontStyle.Bold, GraphicsUnit.Point);
            groupBox7.Location = new Point(11, 11);
            groupBox7.Margin = new Padding(2);
            groupBox7.Name = "groupBox7";
            groupBox7.Padding = new Padding(2);
            groupBox7.Size = new Size(386, 279);
            groupBox7.TabIndex = 999;
            groupBox7.TabStop = false;
            groupBox7.Text = "GPT 옵션";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Font = new Font("맑은 고딕", 9F, FontStyle.Regular, GraphicsUnit.Point);
            label8.Location = new Point(35, 132);
            label8.Name = "label8";
            label8.Size = new Size(64, 15);
            label8.TabIndex = 1005;
            label8.Text = "GPT 요청*:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("맑은 고딕", 9F, FontStyle.Regular, GraphicsUnit.Point);
            label2.Location = new Point(8, 106);
            label2.Name = "label2";
            label2.Size = new Size(91, 15);
            label2.TabIndex = 1004;
            label2.Text = "이미지 키워드*:";
            // 
            // SystemBox1
            // 
            SystemBox1.BackColor = Color.FromArgb(224, 224, 224);
            SystemBox1.BorderStyle = BorderStyle.FixedSingle;
            SystemBox1.Font = new Font("맑은 고딕", 9F, FontStyle.Regular, GraphicsUnit.Point);
            SystemBox1.Location = new Point(117, 72);
            SystemBox1.Margin = new Padding(2);
            SystemBox1.Name = "SystemBox1";
            SystemBox1.Size = new Size(260, 23);
            SystemBox1.TabIndex = 1;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("맑은 고딕", 9F, FontStyle.Regular, GraphicsUnit.Point);
            label5.Location = new Point(44, 76);
            label5.Name = "label5";
            label5.Size = new Size(55, 15);
            label5.TabIndex = 1003;
            label5.Text = "글 주제*:";
            // 
            // groupBox12
            // 
            groupBox12.Controls.Add(FolderPath1Btn1);
            groupBox12.Controls.Add(FolderPath1);
            groupBox12.Font = new Font("맑은 고딕", 9F, FontStyle.Bold, GraphicsUnit.Point);
            groupBox12.Location = new Point(6, 222);
            groupBox12.Margin = new Padding(2);
            groupBox12.Name = "groupBox12";
            groupBox12.Padding = new Padding(2);
            groupBox12.Size = new Size(169, 49);
            groupBox12.TabIndex = 1001;
            groupBox12.TabStop = false;
            groupBox12.Text = "폴더 경로(필수)";
            // 
            // groupBox6
            // 
            groupBox6.Controls.Add(APIKeybox1);
            groupBox6.Font = new Font("맑은 고딕", 9F, FontStyle.Bold, GraphicsUnit.Point);
            groupBox6.Location = new Point(4, 20);
            groupBox6.Margin = new Padding(2);
            groupBox6.Name = "groupBox6";
            groupBox6.Padding = new Padding(2);
            groupBox6.Size = new Size(377, 48);
            groupBox6.TabIndex = 997;
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
            APIKeybox1.Size = new Size(368, 23);
            APIKeybox1.TabIndex = 0;
            APIKeybox1.TextChanged += APIKeybox1_TextChanged;
            // 
            // groupBox10
            // 
            groupBox10.Controls.Add(groupBox2);
            groupBox10.Controls.Add(comboBox1);
            groupBox10.Controls.Add(LoadBtn1);
            groupBox10.Controls.Add(titleBox1);
            groupBox10.Controls.Add(SaveBtn1);
            groupBox10.Controls.Add(label6);
            groupBox10.Controls.Add(label7);
            groupBox10.Controls.Add(groupBox4);
            groupBox10.Controls.Add(groupBox5);
            groupBox10.Font = new Font("맑은 고딕", 9F, FontStyle.Bold, GraphicsUnit.Point);
            groupBox10.Location = new Point(401, 11);
            groupBox10.Margin = new Padding(2);
            groupBox10.Name = "groupBox10";
            groupBox10.Padding = new Padding(2);
            groupBox10.Size = new Size(259, 279);
            groupBox10.TabIndex = 1000;
            groupBox10.TabStop = false;
            groupBox10.Text = "워드프레스 옵션";
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(UrlBox1);
            groupBox2.Font = new Font("맑은 고딕", 9F, FontStyle.Bold, GraphicsUnit.Point);
            groupBox2.Location = new Point(4, 130);
            groupBox2.Margin = new Padding(2);
            groupBox2.Name = "groupBox2";
            groupBox2.Padding = new Padding(2);
            groupBox2.Size = new Size(249, 48);
            groupBox2.TabIndex = 1007;
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
            UrlBox1.TabIndex = 4;
            UrlBox1.TextChanged += UrlBox1_TextChanged;
            // 
            // LoadBtn1
            // 
            LoadBtn1.Location = new Point(167, 248);
            LoadBtn1.Name = "LoadBtn1";
            LoadBtn1.Size = new Size(75, 23);
            LoadBtn1.TabIndex = 1006;
            LoadBtn1.Text = "불러오기";
            LoadBtn1.UseVisualStyleBackColor = true;
            LoadBtn1.Click += LoadBtn1_Click;
            // 
            // SaveBtn1
            // 
            SaveBtn1.Location = new Point(86, 248);
            SaveBtn1.Name = "SaveBtn1";
            SaveBtn1.Size = new Size(75, 23);
            SaveBtn1.TabIndex = 1005;
            SaveBtn1.Text = "저장";
            SaveBtn1.UseVisualStyleBackColor = true;
            SaveBtn1.Click += SaveBtn1_Click;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("맑은 고딕", 9F, FontStyle.Regular, GraphicsUnit.Point);
            label6.Location = new Point(26, 192);
            label6.Name = "label6";
            label6.Size = new Size(39, 15);
            label6.TabIndex = 1002;
            label6.Text = "제목*:";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Font = new Font("맑은 고딕", 9F, FontStyle.Regular, GraphicsUnit.Point);
            label7.Location = new Point(2, 222);
            label7.Name = "label7";
            label7.Size = new Size(63, 15);
            label7.TabIndex = 1001;
            label7.Text = "카테고리*:";
            // 
            // groupBox4
            // 
            groupBox4.Controls.Add(IdBox1);
            groupBox4.Font = new Font("맑은 고딕", 9F, FontStyle.Bold, GraphicsUnit.Point);
            groupBox4.Location = new Point(4, 20);
            groupBox4.Margin = new Padding(2);
            groupBox4.Name = "groupBox4";
            groupBox4.Padding = new Padding(2);
            groupBox4.Size = new Size(249, 48);
            groupBox4.TabIndex = 995;
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
            IdBox1.TabIndex = 3;
            IdBox1.TextChanged += IdBox1_TextChanged;
            // 
            // groupBox5
            // 
            groupBox5.Controls.Add(PwBox1);
            groupBox5.Font = new Font("맑은 고딕", 9F, FontStyle.Bold, GraphicsUnit.Point);
            groupBox5.Location = new Point(4, 75);
            groupBox5.Margin = new Padding(2);
            groupBox5.Name = "groupBox5";
            groupBox5.Padding = new Padding(2);
            groupBox5.Size = new Size(249, 48);
            groupBox5.TabIndex = 996;
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
            PwBox1.TabIndex = 4;
            PwBox1.TextChanged += PwBox1_TextChanged;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(670, 460);
            Controls.Add(groupBox10);
            Controls.Add(groupBox7);
            Controls.Add(groupBox1);
            Name = "Form1";
            Text = "Auto Posting for WordPress";
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            groupBox7.ResumeLayout(false);
            groupBox7.PerformLayout();
            groupBox12.ResumeLayout(false);
            groupBox12.PerformLayout();
            groupBox6.ResumeLayout(false);
            groupBox6.PerformLayout();
            groupBox10.ResumeLayout(false);
            groupBox10.PerformLayout();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            groupBox4.ResumeLayout(false);
            groupBox4.PerformLayout();
            groupBox5.ResumeLayout(false);
            groupBox5.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Button StartBtn1;
        private GroupBox groupBox1;
        private TextBox LogBox1;
        private ComboBox comboBox1;
        private TextBox dalleBox1;
        private TextBox titleBox1;
        private TextBox gptBox1;
        private Button FolderPath1Btn1;
        private TextBox FolderPath1;
        private GroupBox groupBox7;
        private Label label2;
        private TextBox SystemBox1;
        private Label label5;
        private GroupBox groupBox12;
        private GroupBox groupBox6;
        private TextBox APIKeybox1;
        private GroupBox groupBox10;
        private Button LoadBtn1;
        private Button SaveBtn1;
        private Label label6;
        private Label label7;
        private GroupBox groupBox4;
        private TextBox IdBox1;
        private GroupBox groupBox5;
        private TextBox PwBox1;
        private Label label8;
        private GroupBox groupBox2;
        private TextBox UrlBox1;
    }
}