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
            groupBox2 = new GroupBox();
            comboBox1 = new ComboBox();
            label1 = new Label();
            label3 = new Label();
            label4 = new Label();
            dalleBox1 = new TextBox();
            titleBox1 = new TextBox();
            gptBox1 = new TextBox();
            groupBox3 = new GroupBox();
            FolderPath1Btn1 = new Button();
            FolderPath1 = new TextBox();
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
            groupBox3.SuspendLayout();
            SuspendLayout();
            // 
            // StartBtn1
            // 
            StartBtn1.BackColor = Color.Silver;
            StartBtn1.Cursor = Cursors.Hand;
            StartBtn1.Font = new Font("맑은 고딕", 12F, FontStyle.Bold, GraphicsUnit.Point);
            StartBtn1.ForeColor = Color.Black;
            StartBtn1.Location = new Point(8, 387);
            StartBtn1.Margin = new Padding(4);
            StartBtn1.Name = "StartBtn1";
            StartBtn1.Size = new Size(522, 47);
            StartBtn1.TabIndex = 990;
            StartBtn1.Text = "실행";
            StartBtn1.UseVisualStyleBackColor = false;
            StartBtn1.Click += StartBtn1_Click;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(LogBox1);
            groupBox1.Font = new Font("맑은 고딕", 9.75F, FontStyle.Bold, GraphicsUnit.Point);
            groupBox1.Location = new Point(15, 16);
            groupBox1.Margin = new Padding(4);
            groupBox1.Name = "groupBox1";
            groupBox1.Padding = new Padding(4);
            groupBox1.Size = new Size(250, 151);
            groupBox1.TabIndex = 2;
            groupBox1.TabStop = false;
            groupBox1.Text = "작업로그";
            // 
            // LogBox1
            // 
            LogBox1.BackColor = Color.LightGray;
            LogBox1.BorderStyle = BorderStyle.FixedSingle;
            LogBox1.ForeColor = Color.Black;
            LogBox1.Location = new Point(8, 35);
            LogBox1.Margin = new Padding(4);
            LogBox1.Multiline = true;
            LogBox1.Name = "LogBox1";
            LogBox1.ReadOnly = true;
            LogBox1.Size = new Size(227, 100);
            LogBox1.TabIndex = 995;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(comboBox1);
            groupBox2.Font = new Font("맑은 고딕", 9.75F, FontStyle.Bold, GraphicsUnit.Point);
            groupBox2.Location = new Point(273, 96);
            groupBox2.Margin = new Padding(4);
            groupBox2.Name = "groupBox2";
            groupBox2.Padding = new Padding(4);
            groupBox2.Size = new Size(257, 71);
            groupBox2.TabIndex = 3;
            groupBox2.TabStop = false;
            groupBox2.Text = "카테고리";
            // 
            // comboBox1
            // 
            comboBox1.BackColor = Color.LightGray;
            comboBox1.Font = new Font("굴림", 9F, FontStyle.Bold, GraphicsUnit.Point);
            comboBox1.ForeColor = Color.Black;
            comboBox1.FormattingEnabled = true;
            comboBox1.Items.AddRange(new object[] { "오희 리빙템", "오희 면접준비", "오희 잡담", "오희 지원정보" });
            comboBox1.Location = new Point(6, 32);
            comboBox1.Margin = new Padding(4);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(243, 23);
            comboBox1.TabIndex = 997;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("맑은 고딕", 9.75F, FontStyle.Bold, GraphicsUnit.Point);
            label1.Location = new Point(72, 178);
            label1.Margin = new Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new Size(54, 23);
            label1.TabIndex = 4;
            label1.Text = "제목 :";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("맑은 고딕", 9.75F, FontStyle.Bold, GraphicsUnit.Point);
            label3.Location = new Point(15, 217);
            label3.Margin = new Padding(4, 0, 4, 0);
            label3.Name = "label3";
            label3.Size = new Size(111, 23);
            label3.TabIndex = 6;
            label3.Text = "이미지 요청 :";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("맑은 고딕", 9.75F, FontStyle.Bold, GraphicsUnit.Point);
            label4.Location = new Point(33, 257);
            label4.Margin = new Padding(4, 0, 4, 0);
            label4.Name = "label4";
            label4.Size = new Size(92, 23);
            label4.TabIndex = 7;
            label4.Text = "GTP 요청 :";
            // 
            // dalleBox1
            // 
            dalleBox1.BackColor = Color.LightGray;
            dalleBox1.BorderStyle = BorderStyle.FixedSingle;
            dalleBox1.Cursor = Cursors.IBeam;
            dalleBox1.Font = new Font("굴림", 9F, FontStyle.Regular, GraphicsUnit.Point);
            dalleBox1.ForeColor = Color.Black;
            dalleBox1.Location = new Point(139, 214);
            dalleBox1.Margin = new Padding(4);
            dalleBox1.Name = "dalleBox1";
            dalleBox1.Size = new Size(390, 25);
            dalleBox1.TabIndex = 3;
            // 
            // titleBox1
            // 
            titleBox1.BackColor = Color.LightGray;
            titleBox1.BorderStyle = BorderStyle.FixedSingle;
            titleBox1.Cursor = Cursors.IBeam;
            titleBox1.Font = new Font("굴림", 9F, FontStyle.Regular, GraphicsUnit.Point);
            titleBox1.ForeColor = Color.Black;
            titleBox1.Location = new Point(139, 175);
            titleBox1.Margin = new Padding(4);
            titleBox1.Name = "titleBox1";
            titleBox1.Size = new Size(390, 25);
            titleBox1.TabIndex = 1;
            // 
            // gptBox1
            // 
            gptBox1.BackColor = Color.LightGray;
            gptBox1.BorderStyle = BorderStyle.FixedSingle;
            gptBox1.Cursor = Cursors.IBeam;
            gptBox1.Font = new Font("굴림", 9F, FontStyle.Regular, GraphicsUnit.Point);
            gptBox1.ForeColor = Color.Black;
            gptBox1.Location = new Point(139, 254);
            gptBox1.Margin = new Padding(4);
            gptBox1.Multiline = true;
            gptBox1.Name = "gptBox1";
            gptBox1.Size = new Size(390, 127);
            gptBox1.TabIndex = 4;
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(FolderPath1Btn1);
            groupBox3.Controls.Add(FolderPath1);
            groupBox3.Font = new Font("맑은 고딕", 9.75F, FontStyle.Bold, GraphicsUnit.Point);
            groupBox3.Location = new Point(273, 16);
            groupBox3.Margin = new Padding(4);
            groupBox3.Name = "groupBox3";
            groupBox3.Padding = new Padding(4);
            groupBox3.Size = new Size(257, 72);
            groupBox3.TabIndex = 991;
            groupBox3.TabStop = false;
            groupBox3.Text = "폴더";
            // 
            // FolderPath1Btn1
            // 
            FolderPath1Btn1.BackColor = Color.LightGray;
            FolderPath1Btn1.Cursor = Cursors.Hand;
            FolderPath1Btn1.Location = new Point(195, 30);
            FolderPath1Btn1.Margin = new Padding(4);
            FolderPath1Btn1.Name = "FolderPath1Btn1";
            FolderPath1Btn1.Size = new Size(54, 33);
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
            FolderPath1.Location = new Point(8, 32);
            FolderPath1.Margin = new Padding(4);
            FolderPath1.Name = "FolderPath1";
            FolderPath1.ReadOnly = true;
            FolderPath1.Size = new Size(179, 29);
            FolderPath1.TabIndex = 996;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(9F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(542, 449);
            Controls.Add(groupBox3);
            Controls.Add(gptBox1);
            Controls.Add(titleBox1);
            Controls.Add(dalleBox1);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label1);
            Controls.Add(groupBox2);
            Controls.Add(groupBox1);
            Controls.Add(StartBtn1);
            Margin = new Padding(4);
            Name = "Form1";
            Text = "Auto Posting for WordPress";
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            groupBox2.ResumeLayout(false);
            groupBox3.ResumeLayout(false);
            groupBox3.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button StartBtn1;
        private GroupBox groupBox1;
        private GroupBox groupBox2;
        private TextBox LogBox1;
        private ComboBox comboBox1;
        private Label label1;
        private Label label3;
        private Label label4;
        private TextBox dalleBox1;
        private TextBox titleBox1;
        private TextBox gptBox1;
        private GroupBox groupBox3;
        private Button FolderPath1Btn1;
        private TextBox FolderPath1;
    }
}