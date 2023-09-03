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
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            dalleBox1 = new TextBox();
            titleBox1 = new TextBox();
            tagBox1 = new TextBox();
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
            StartBtn1.BackColor = Color.DimGray;
            StartBtn1.Cursor = Cursors.Hand;
            StartBtn1.Font = new Font("맑은 고딕", 14.25F, FontStyle.Bold, GraphicsUnit.Point);
            StartBtn1.ForeColor = Color.Gold;
            StartBtn1.Location = new Point(6, 336);
            StartBtn1.Name = "StartBtn1";
            StartBtn1.Size = new Size(460, 46);
            StartBtn1.TabIndex = 990;
            StartBtn1.Text = "실행";
            StartBtn1.UseVisualStyleBackColor = false;
            StartBtn1.Click += StartBtn1_Click;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(LogBox1);
            groupBox1.Font = new Font("맑은 고딕", 9.75F, FontStyle.Bold, GraphicsUnit.Point);
            groupBox1.Location = new Point(12, 12);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(224, 120);
            groupBox1.TabIndex = 2;
            groupBox1.TabStop = false;
            groupBox1.Text = "작업로그";
            // 
            // LogBox1
            // 
            LogBox1.BackColor = Color.White;
            LogBox1.ForeColor = Color.Black;
            LogBox1.Location = new Point(6, 26);
            LogBox1.Multiline = true;
            LogBox1.Name = "LogBox1";
            LogBox1.ReadOnly = true;
            LogBox1.Size = new Size(207, 83);
            LogBox1.TabIndex = 995;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(comboBox1);
            groupBox2.Font = new Font("맑은 고딕", 9.75F, FontStyle.Bold, GraphicsUnit.Point);
            groupBox2.Location = new Point(242, 72);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(224, 60);
            groupBox2.TabIndex = 3;
            groupBox2.TabStop = false;
            groupBox2.Text = "카테고리";
            // 
            // comboBox1
            // 
            comboBox1.BackColor = Color.DimGray;
            comboBox1.Font = new Font("굴림", 9F, FontStyle.Bold, GraphicsUnit.Point);
            comboBox1.ForeColor = Color.White;
            comboBox1.FormattingEnabled = true;
            comboBox1.Items.AddRange(new object[] { "오희 리빙템", "오희 면접준비", "오희 잡담", "오희 지원정보" });
            comboBox1.Location = new Point(5, 24);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(213, 20);
            comboBox1.TabIndex = 997;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("맑은 고딕", 9.75F, FontStyle.Bold, GraphicsUnit.Point);
            label1.Location = new Point(56, 146);
            label1.Name = "label1";
            label1.Size = new Size(37, 17);
            label1.TabIndex = 4;
            label1.Text = "제목:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("맑은 고딕", 9.75F, FontStyle.Bold, GraphicsUnit.Point);
            label2.Location = new Point(56, 176);
            label2.Name = "label2";
            label2.Size = new Size(37, 17);
            label2.TabIndex = 5;
            label2.Text = "태그:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("맑은 고딕", 9.75F, FontStyle.Bold, GraphicsUnit.Point);
            label3.Location = new Point(12, 206);
            label3.Name = "label3";
            label3.Size = new Size(81, 17);
            label3.TabIndex = 6;
            label3.Text = "이미지 요청:";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("맑은 고딕", 9.75F, FontStyle.Bold, GraphicsUnit.Point);
            label4.Location = new Point(26, 236);
            label4.Name = "label4";
            label4.Size = new Size(67, 17);
            label4.TabIndex = 7;
            label4.Text = "GTP 요청:";
            // 
            // dalleBox1
            // 
            dalleBox1.BackColor = Color.DimGray;
            dalleBox1.Cursor = Cursors.IBeam;
            dalleBox1.Font = new Font("굴림", 9F, FontStyle.Bold, GraphicsUnit.Point);
            dalleBox1.ForeColor = Color.White;
            dalleBox1.Location = new Point(108, 204);
            dalleBox1.Name = "dalleBox1";
            dalleBox1.Size = new Size(358, 21);
            dalleBox1.TabIndex = 3;
            // 
            // titleBox1
            // 
            titleBox1.BackColor = Color.DimGray;
            titleBox1.Cursor = Cursors.IBeam;
            titleBox1.Font = new Font("굴림", 9F, FontStyle.Bold, GraphicsUnit.Point);
            titleBox1.ForeColor = Color.White;
            titleBox1.Location = new Point(108, 144);
            titleBox1.Name = "titleBox1";
            titleBox1.Size = new Size(358, 21);
            titleBox1.TabIndex = 1;
            // 
            // tagBox1
            // 
            tagBox1.BackColor = Color.DimGray;
            tagBox1.Cursor = Cursors.IBeam;
            tagBox1.Font = new Font("굴림", 9F, FontStyle.Bold, GraphicsUnit.Point);
            tagBox1.ForeColor = Color.White;
            tagBox1.Location = new Point(108, 174);
            tagBox1.Name = "tagBox1";
            tagBox1.Size = new Size(358, 21);
            tagBox1.TabIndex = 2;
            // 
            // gptBox1
            // 
            gptBox1.BackColor = Color.DimGray;
            gptBox1.Cursor = Cursors.IBeam;
            gptBox1.Font = new Font("굴림", 9F, FontStyle.Bold, GraphicsUnit.Point);
            gptBox1.ForeColor = Color.White;
            gptBox1.Location = new Point(108, 234);
            gptBox1.Multiline = true;
            gptBox1.Name = "gptBox1";
            gptBox1.Size = new Size(358, 96);
            gptBox1.TabIndex = 4;
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(FolderPath1Btn1);
            groupBox3.Controls.Add(FolderPath1);
            groupBox3.Font = new Font("맑은 고딕", 9.75F, FontStyle.Bold, GraphicsUnit.Point);
            groupBox3.Location = new Point(242, 12);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new Size(224, 60);
            groupBox3.TabIndex = 991;
            groupBox3.TabStop = false;
            groupBox3.Text = "폴더";
            // 
            // FolderPath1Btn1
            // 
            FolderPath1Btn1.Cursor = Cursors.Hand;
            FolderPath1Btn1.Location = new Point(176, 24);
            FolderPath1Btn1.Name = "FolderPath1Btn1";
            FolderPath1Btn1.Size = new Size(42, 25);
            FolderPath1Btn1.TabIndex = 991;
            FolderPath1Btn1.Text = "...";
            FolderPath1Btn1.UseVisualStyleBackColor = true;
            FolderPath1Btn1.Click += FolderPath1Btn1_Click;
            // 
            // FolderPath1
            // 
            FolderPath1.BackColor = Color.White;
            FolderPath1.ForeColor = Color.Black;
            FolderPath1.Location = new Point(6, 24);
            FolderPath1.Name = "FolderPath1";
            FolderPath1.ReadOnly = true;
            FolderPath1.Size = new Size(164, 25);
            FolderPath1.TabIndex = 996;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(475, 394);
            Controls.Add(groupBox3);
            Controls.Add(gptBox1);
            Controls.Add(tagBox1);
            Controls.Add(titleBox1);
            Controls.Add(dalleBox1);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(groupBox2);
            Controls.Add(groupBox1);
            Controls.Add(StartBtn1);
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
        private Label label2;
        private Label label3;
        private Label label4;
        private TextBox dalleBox1;
        private TextBox titleBox1;
        private TextBox tagBox1;
        private TextBox gptBox1;
        private GroupBox groupBox3;
        private Button FolderPath1Btn1;
        private TextBox FolderPath1;
    }
}