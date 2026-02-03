namespace WisdomGrowth
{
    partial class NewMainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panel2 = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.MaterialMenuBtn = new System.Windows.Forms.Button();
            this.signoutBtn = new System.Windows.Forms.PictureBox();
            this.narrowBtn = new System.Windows.Forms.PictureBox();
            this.amplifybtn = new System.Windows.Forms.PictureBox();
            this.WisdomMenuBtn = new System.Windows.Forms.Button();
            this.LoginBtn = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.signoutBtn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.narrowBtn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.amplifybtn)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.BackgroundImage = global::WisdomGrowth.Properties.Resources.top;
            this.panel2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panel2.Controls.Add(this.pictureBox1);
            this.panel2.Controls.Add(this.MaterialMenuBtn);
            this.panel2.Controls.Add(this.signoutBtn);
            this.panel2.Controls.Add(this.narrowBtn);
            this.panel2.Controls.Add(this.amplifybtn);
            this.panel2.Controls.Add(this.WisdomMenuBtn);
            this.panel2.Controls.Add(this.LoginBtn);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1270, 47);
            this.panel2.TabIndex = 0;
            this.panel2.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panel2_MouseDown);
            this.panel2.MouseMove += new System.Windows.Forms.MouseEventHandler(this.panel2_MouseMove);
            this.panel2.MouseUp += new System.Windows.Forms.MouseEventHandler(this.panel2_MouseUp);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1.BackgroundImage = global::WisdomGrowth.Properties.Resources.主页;
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pictureBox1.Location = new System.Drawing.Point(533, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(20, 20);
            this.pictureBox1.TabIndex = 9;
            this.pictureBox1.TabStop = false;
            // 
            // MaterialMenuBtn
            // 
            this.MaterialMenuBtn.BackColor = System.Drawing.Color.Transparent;
            this.MaterialMenuBtn.BackgroundImage = global::WisdomGrowth.Properties.Resources.title_item_border;
            this.MaterialMenuBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.MaterialMenuBtn.FlatAppearance.BorderSize = 0;
            this.MaterialMenuBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.MaterialMenuBtn.Font = new System.Drawing.Font("微软雅黑", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.MaterialMenuBtn.ForeColor = System.Drawing.SystemColors.InactiveBorder;
            this.MaterialMenuBtn.Location = new System.Drawing.Point(925, 5);
            this.MaterialMenuBtn.Name = "MaterialMenuBtn";
            this.MaterialMenuBtn.Size = new System.Drawing.Size(239, 36);
            this.MaterialMenuBtn.TabIndex = 6;
            this.MaterialMenuBtn.Text = "外延材料性能预测平台";
            this.MaterialMenuBtn.UseVisualStyleBackColor = false;
            this.MaterialMenuBtn.Click += new System.EventHandler(this.MaterialMenuBtn_Click);
            this.MaterialMenuBtn.MouseEnter += new System.EventHandler(this.MaterialMenuBtn_MouseEnter);
            this.MaterialMenuBtn.MouseLeave += new System.EventHandler(this.MaterialMenuBtn_MouseLeave);
            // 
            // signoutBtn
            // 
            this.signoutBtn.BackColor = System.Drawing.Color.Transparent;
            this.signoutBtn.BackgroundImage = global::WisdomGrowth.Properties.Resources.关机;
            this.signoutBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.signoutBtn.Location = new System.Drawing.Point(1237, 11);
            this.signoutBtn.Name = "signoutBtn";
            this.signoutBtn.Size = new System.Drawing.Size(25, 25);
            this.signoutBtn.TabIndex = 8;
            this.signoutBtn.TabStop = false;
            this.signoutBtn.Click += new System.EventHandler(this.signoutBtn_Click);
            // 
            // narrowBtn
            // 
            this.narrowBtn.BackColor = System.Drawing.Color.Transparent;
            this.narrowBtn.BackgroundImage = global::WisdomGrowth.Properties.Resources.Minimize_2;
            this.narrowBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.narrowBtn.Location = new System.Drawing.Point(1175, 10);
            this.narrowBtn.Name = "narrowBtn";
            this.narrowBtn.Size = new System.Drawing.Size(25, 25);
            this.narrowBtn.TabIndex = 9;
            this.narrowBtn.TabStop = false;
            this.narrowBtn.Click += new System.EventHandler(this.narrowBtn_Click);
            // 
            // amplifybtn
            // 
            this.amplifybtn.BackColor = System.Drawing.Color.Transparent;
            this.amplifybtn.BackgroundImage = global::WisdomGrowth.Properties.Resources.查看_列表;
            this.amplifybtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.amplifybtn.Location = new System.Drawing.Point(1206, 11);
            this.amplifybtn.Name = "amplifybtn";
            this.amplifybtn.Size = new System.Drawing.Size(25, 25);
            this.amplifybtn.TabIndex = 7;
            this.amplifybtn.TabStop = false;
            this.amplifybtn.Click += new System.EventHandler(this.amplifybtn_Click);
            // 
            // WisdomMenuBtn
            // 
            this.WisdomMenuBtn.BackColor = System.Drawing.Color.Transparent;
            this.WisdomMenuBtn.BackgroundImage = global::WisdomGrowth.Properties.Resources.title_item_border;
            this.WisdomMenuBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.WisdomMenuBtn.FlatAppearance.BorderSize = 0;
            this.WisdomMenuBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.WisdomMenuBtn.Font = new System.Drawing.Font("微软雅黑", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.WisdomMenuBtn.ForeColor = System.Drawing.SystemColors.InactiveBorder;
            this.WisdomMenuBtn.Location = new System.Drawing.Point(646, 5);
            this.WisdomMenuBtn.Name = "WisdomMenuBtn";
            this.WisdomMenuBtn.Size = new System.Drawing.Size(273, 36);
            this.WisdomMenuBtn.TabIndex = 6;
            this.WisdomMenuBtn.Text = "MOCVD外延智能生长平台";
            this.WisdomMenuBtn.UseVisualStyleBackColor = false;
            this.WisdomMenuBtn.Click += new System.EventHandler(this.WisdomMenuBtn_Click);
            this.WisdomMenuBtn.MouseEnter += new System.EventHandler(this.WisdomMenuBtn_MouseEnter);
            this.WisdomMenuBtn.MouseLeave += new System.EventHandler(this.WisdomMenuBtn_MouseLeave);
            // 
            // LoginBtn
            // 
            this.LoginBtn.BackColor = System.Drawing.Color.Transparent;
            this.LoginBtn.BackgroundImage = global::WisdomGrowth.Properties.Resources.title_item_border;
            this.LoginBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.LoginBtn.FlatAppearance.BorderSize = 0;
            this.LoginBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.LoginBtn.Font = new System.Drawing.Font("微软雅黑", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.LoginBtn.ForeColor = System.Drawing.SystemColors.InactiveBorder;
            this.LoginBtn.Location = new System.Drawing.Point(554, 5);
            this.LoginBtn.Name = "LoginBtn";
            this.LoginBtn.Size = new System.Drawing.Size(87, 36);
            this.LoginBtn.TabIndex = 6;
            this.LoginBtn.Text = "工作台";
            this.LoginBtn.UseVisualStyleBackColor = false;
            this.LoginBtn.Click += new System.EventHandler(this.LoginBtn_Click);
            this.LoginBtn.MouseEnter += new System.EventHandler(this.LoginBtn_MouseEnter);
            this.LoginBtn.MouseLeave += new System.EventHandler(this.LoginBtn_MouseLeave);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 21.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.ForeColor = System.Drawing.SystemColors.InactiveCaption;
            this.label1.Location = new System.Drawing.Point(-6, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(541, 39);
            this.label1.TabIndex = 1;
            this.label1.Text = "化合物半导体MOCVD外延智能生长系统";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panel3);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1270, 800);
            this.panel1.TabIndex = 0;
            // 
            // panel3
            // 
            this.panel3.BackgroundImage = global::WisdomGrowth.Properties.Resources.bg_1;
            this.panel3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 47);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1270, 753);
            this.panel3.TabIndex = 1;
            this.panel3.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panel3_MouseDown);
            this.panel3.MouseMove += new System.Windows.Forms.MouseEventHandler(this.panel3_MouseMove);
            this.panel3.MouseUp += new System.Windows.Forms.MouseEventHandler(this.panel3_MouseUp);
            // 
            // NewMainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1270, 800);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "NewMainForm";
            this.Text = "X";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.NewMainForm_Load);
            this.Resize += new System.EventHandler(this.NewMainForm_Resize);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.signoutBtn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.narrowBtn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.amplifybtn)).EndInit();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button MaterialMenuBtn;
        private System.Windows.Forms.PictureBox signoutBtn;
        private System.Windows.Forms.PictureBox narrowBtn;
        private System.Windows.Forms.PictureBox amplifybtn;
        private System.Windows.Forms.Button WisdomMenuBtn;
        private System.Windows.Forms.Button LoginBtn;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel3;
    }
}