namespace WisdomGrowth.ModelTraining
{
    partial class BPNNForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BPNNForm));
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.HiddennodeTxt = new DevExpress.XtraEditors.TextEdit();
            this.PenaltyfactorTxt = new DevExpress.XtraEditors.TextEdit();
            this.MaxIterationsTxt = new DevExpress.XtraEditors.TextEdit();
            this.lbfgsRbtn = new System.Windows.Forms.RadioButton();
            this.sgdRbtn = new System.Windows.Forms.RadioButton();
            this.adamRbtn = new System.Windows.Forms.RadioButton();
            this.OKBtn = new System.Windows.Forms.Button();
            this.CancelBtn = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.HiddennodeTxt.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PenaltyfactorTxt.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MaxIterationsTxt.Properties)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label1.Location = new System.Drawing.Point(12, 52);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(182, 31);
            this.label1.TabIndex = 1;
            this.label1.Text = "BPNN参数设置";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("微软雅黑", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(132, 116);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(96, 28);
            this.label3.TabIndex = 5;
            this.label3.Text = "隐藏节点";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("微软雅黑", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(132, 160);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(96, 28);
            this.label2.TabIndex = 5;
            this.label2.Text = "惩罚系数";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("微软雅黑", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(132, 213);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(138, 28);
            this.label4.TabIndex = 5;
            this.label4.Text = "最大迭代次数";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Font = new System.Drawing.Font("微软雅黑", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label5.Location = new System.Drawing.Point(132, 261);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(138, 28);
            this.label5.TabIndex = 5;
            this.label5.Text = "梯度下降算法";
            // 
            // HiddennodeTxt
            // 
            this.HiddennodeTxt.EditValue = "10";
            this.HiddennodeTxt.Location = new System.Drawing.Point(284, 118);
            this.HiddennodeTxt.Name = "HiddennodeTxt";
            this.HiddennodeTxt.Properties.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(222)))), ((int)(((byte)(171)))));
            this.HiddennodeTxt.Properties.Appearance.Font = new System.Drawing.Font("微软雅黑", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.HiddennodeTxt.Properties.Appearance.ForeColor = System.Drawing.Color.Black;
            this.HiddennodeTxt.Properties.Appearance.Options.UseBackColor = true;
            this.HiddennodeTxt.Properties.Appearance.Options.UseFont = true;
            this.HiddennodeTxt.Properties.Appearance.Options.UseForeColor = true;
            this.HiddennodeTxt.Size = new System.Drawing.Size(182, 34);
            this.HiddennodeTxt.TabIndex = 8;
            // 
            // PenaltyfactorTxt
            // 
            this.PenaltyfactorTxt.EditValue = "0.0001";
            this.PenaltyfactorTxt.Location = new System.Drawing.Point(284, 162);
            this.PenaltyfactorTxt.Name = "PenaltyfactorTxt";
            this.PenaltyfactorTxt.Properties.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(222)))), ((int)(((byte)(171)))));
            this.PenaltyfactorTxt.Properties.Appearance.Font = new System.Drawing.Font("微软雅黑", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.PenaltyfactorTxt.Properties.Appearance.ForeColor = System.Drawing.Color.Black;
            this.PenaltyfactorTxt.Properties.Appearance.Options.UseBackColor = true;
            this.PenaltyfactorTxt.Properties.Appearance.Options.UseFont = true;
            this.PenaltyfactorTxt.Properties.Appearance.Options.UseForeColor = true;
            this.PenaltyfactorTxt.Size = new System.Drawing.Size(182, 34);
            this.PenaltyfactorTxt.TabIndex = 8;
            // 
            // MaxIterationsTxt
            // 
            this.MaxIterationsTxt.EditValue = "5000";
            this.MaxIterationsTxt.Location = new System.Drawing.Point(284, 215);
            this.MaxIterationsTxt.Name = "MaxIterationsTxt";
            this.MaxIterationsTxt.Properties.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(222)))), ((int)(((byte)(171)))));
            this.MaxIterationsTxt.Properties.Appearance.Font = new System.Drawing.Font("微软雅黑", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.MaxIterationsTxt.Properties.Appearance.ForeColor = System.Drawing.Color.Black;
            this.MaxIterationsTxt.Properties.Appearance.Options.UseBackColor = true;
            this.MaxIterationsTxt.Properties.Appearance.Options.UseFont = true;
            this.MaxIterationsTxt.Properties.Appearance.Options.UseForeColor = true;
            this.MaxIterationsTxt.Size = new System.Drawing.Size(182, 34);
            this.MaxIterationsTxt.TabIndex = 8;
            // 
            // lbfgsRbtn
            // 
            this.lbfgsRbtn.AutoSize = true;
            this.lbfgsRbtn.BackColor = System.Drawing.Color.Transparent;
            this.lbfgsRbtn.Checked = true;
            this.lbfgsRbtn.Font = new System.Drawing.Font("微软雅黑", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbfgsRbtn.ForeColor = System.Drawing.Color.White;
            this.lbfgsRbtn.Location = new System.Drawing.Point(284, 266);
            this.lbfgsRbtn.Name = "lbfgsRbtn";
            this.lbfgsRbtn.Size = new System.Drawing.Size(83, 32);
            this.lbfgsRbtn.TabIndex = 9;
            this.lbfgsRbtn.TabStop = true;
            this.lbfgsRbtn.Text = "lbfgs";
            this.lbfgsRbtn.UseVisualStyleBackColor = false;
            this.lbfgsRbtn.CheckedChanged += new System.EventHandler(this.lbfgsRbtn_CheckedChanged);
            // 
            // sgdRbtn
            // 
            this.sgdRbtn.AutoSize = true;
            this.sgdRbtn.BackColor = System.Drawing.Color.Transparent;
            this.sgdRbtn.Font = new System.Drawing.Font("微软雅黑", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.sgdRbtn.ForeColor = System.Drawing.Color.White;
            this.sgdRbtn.Location = new System.Drawing.Point(284, 302);
            this.sgdRbtn.Name = "sgdRbtn";
            this.sgdRbtn.Size = new System.Drawing.Size(68, 32);
            this.sgdRbtn.TabIndex = 9;
            this.sgdRbtn.Text = "sgd";
            this.sgdRbtn.UseVisualStyleBackColor = false;
            this.sgdRbtn.CheckedChanged += new System.EventHandler(this.sgdRbtn_CheckedChanged);
            // 
            // adamRbtn
            // 
            this.adamRbtn.AutoSize = true;
            this.adamRbtn.BackColor = System.Drawing.Color.Transparent;
            this.adamRbtn.Font = new System.Drawing.Font("微软雅黑", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.adamRbtn.ForeColor = System.Drawing.Color.White;
            this.adamRbtn.Location = new System.Drawing.Point(284, 343);
            this.adamRbtn.Name = "adamRbtn";
            this.adamRbtn.Size = new System.Drawing.Size(89, 32);
            this.adamRbtn.TabIndex = 9;
            this.adamRbtn.Text = "adam";
            this.adamRbtn.UseVisualStyleBackColor = false;
            this.adamRbtn.CheckedChanged += new System.EventHandler(this.adamRbtn_CheckedChanged);
            // 
            // OKBtn
            // 
            this.OKBtn.BackColor = System.Drawing.Color.Transparent;
            this.OKBtn.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("OKBtn.BackgroundImage")));
            this.OKBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.OKBtn.FlatAppearance.BorderSize = 0;
            this.OKBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.OKBtn.Font = new System.Drawing.Font("微软雅黑", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.OKBtn.ForeColor = System.Drawing.Color.White;
            this.OKBtn.Location = new System.Drawing.Point(244, 403);
            this.OKBtn.Name = "OKBtn";
            this.OKBtn.Size = new System.Drawing.Size(92, 37);
            this.OKBtn.TabIndex = 10;
            this.OKBtn.Text = "OK";
            this.OKBtn.UseVisualStyleBackColor = false;
            this.OKBtn.Click += new System.EventHandler(this.OKBtn_Click);
            // 
            // CancelBtn
            // 
            this.CancelBtn.BackColor = System.Drawing.Color.Transparent;
            this.CancelBtn.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("CancelBtn.BackgroundImage")));
            this.CancelBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.CancelBtn.FlatAppearance.BorderSize = 0;
            this.CancelBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.CancelBtn.Font = new System.Drawing.Font("微软雅黑", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.CancelBtn.ForeColor = System.Drawing.Color.White;
            this.CancelBtn.Location = new System.Drawing.Point(374, 403);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(92, 37);
            this.CancelBtn.TabIndex = 10;
            this.CancelBtn.Text = "Cancel";
            this.CancelBtn.UseVisualStyleBackColor = false;
            this.CancelBtn.Click += new System.EventHandler(this.CancelBtn_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Transparent;
            this.panel1.BackgroundImage = global::WisdomGrowth.Properties.Resources.right_border1;
            this.panel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.CancelBtn);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.OKBtn);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.adamRbtn);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.sgdRbtn);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.lbfgsRbtn);
            this.panel1.Controls.Add(this.HiddennodeTxt);
            this.panel1.Controls.Add(this.MaxIterationsTxt);
            this.panel1.Controls.Add(this.PenaltyfactorTxt);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(624, 491);
            this.panel1.TabIndex = 11;
            // 
            // BPNNForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImage = global::WisdomGrowth.Properties.Resources.bg_1;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(624, 491);
            this.Controls.Add(this.panel1);
            this.DoubleBuffered = true;
            this.ForeColor = System.Drawing.Color.Black;
            this.Name = "BPNNForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "BPNNForm";
            this.Load += new System.EventHandler(this.BPNNForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.HiddennodeTxt.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PenaltyfactorTxt.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MaxIterationsTxt.Properties)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private DevExpress.XtraEditors.TextEdit HiddennodeTxt;
        private DevExpress.XtraEditors.TextEdit PenaltyfactorTxt;
        private DevExpress.XtraEditors.TextEdit MaxIterationsTxt;
        private System.Windows.Forms.RadioButton lbfgsRbtn;
        private System.Windows.Forms.RadioButton sgdRbtn;
        private System.Windows.Forms.RadioButton adamRbtn;
        private System.Windows.Forms.Button OKBtn;
        private System.Windows.Forms.Button CancelBtn;
        private System.Windows.Forms.Panel panel1;
    }
}