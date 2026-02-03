namespace WisdomGrowth.ModelTraining
{
    partial class SVRForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SVRForm));
            this.CancelBtn = new System.Windows.Forms.Button();
            this.OKBtn = new System.Windows.Forms.Button();
            this.polyRbtn = new System.Windows.Forms.RadioButton();
            this.lineaRbtn = new System.Windows.Forms.RadioButton();
            this.rb1Rbtn = new System.Windows.Forms.RadioButton();
            this.MaxIterationsTxt = new DevExpress.XtraEditors.TextEdit();
            this.regularizationparameterTxt = new DevExpress.XtraEditors.TextEdit();
            this.toleranceTxt = new DevExpress.XtraEditors.TextEdit();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.sigmoidRbtn = new System.Windows.Forms.RadioButton();
            this.precomputerRbtn = new System.Windows.Forms.RadioButton();
            this.panel1 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.MaxIterationsTxt.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.regularizationparameterTxt.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.toleranceTxt.Properties)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
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
            this.CancelBtn.Location = new System.Drawing.Point(395, 405);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(92, 35);
            this.CancelBtn.TabIndex = 22;
            this.CancelBtn.Text = "Cancel";
            this.CancelBtn.UseVisualStyleBackColor = false;
            this.CancelBtn.Click += new System.EventHandler(this.CancelBtn_Click);
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
            this.OKBtn.Location = new System.Drawing.Point(265, 405);
            this.OKBtn.Name = "OKBtn";
            this.OKBtn.Size = new System.Drawing.Size(92, 35);
            this.OKBtn.TabIndex = 23;
            this.OKBtn.Text = "OK";
            this.OKBtn.UseVisualStyleBackColor = false;
            this.OKBtn.Click += new System.EventHandler(this.OKBtn_Click);
            // 
            // polyRbtn
            // 
            this.polyRbtn.AutoSize = true;
            this.polyRbtn.BackColor = System.Drawing.Color.Transparent;
            this.polyRbtn.Font = new System.Drawing.Font("微软雅黑", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.polyRbtn.ForeColor = System.Drawing.Color.White;
            this.polyRbtn.Location = new System.Drawing.Point(431, 152);
            this.polyRbtn.Name = "polyRbtn";
            this.polyRbtn.Size = new System.Drawing.Size(76, 32);
            this.polyRbtn.TabIndex = 19;
            this.polyRbtn.Text = "poly";
            this.polyRbtn.UseVisualStyleBackColor = false;
            this.polyRbtn.CheckedChanged += new System.EventHandler(this.polyRbtn_CheckedChanged);
            // 
            // lineaRbtn
            // 
            this.lineaRbtn.AutoSize = true;
            this.lineaRbtn.BackColor = System.Drawing.Color.Transparent;
            this.lineaRbtn.Font = new System.Drawing.Font("微软雅黑", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lineaRbtn.ForeColor = System.Drawing.Color.White;
            this.lineaRbtn.Location = new System.Drawing.Point(337, 151);
            this.lineaRbtn.Name = "lineaRbtn";
            this.lineaRbtn.Size = new System.Drawing.Size(89, 32);
            this.lineaRbtn.TabIndex = 20;
            this.lineaRbtn.Text = "linear";
            this.lineaRbtn.UseVisualStyleBackColor = false;
            this.lineaRbtn.CheckedChanged += new System.EventHandler(this.lineaRbtn_CheckedChanged);
            // 
            // rb1Rbtn
            // 
            this.rb1Rbtn.AutoSize = true;
            this.rb1Rbtn.BackColor = System.Drawing.Color.Transparent;
            this.rb1Rbtn.Checked = true;
            this.rb1Rbtn.Font = new System.Drawing.Font("微软雅黑", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.rb1Rbtn.ForeColor = System.Drawing.Color.White;
            this.rb1Rbtn.Location = new System.Drawing.Point(247, 151);
            this.rb1Rbtn.Name = "rb1Rbtn";
            this.rb1Rbtn.Size = new System.Drawing.Size(62, 32);
            this.rb1Rbtn.TabIndex = 21;
            this.rb1Rbtn.TabStop = true;
            this.rb1Rbtn.Text = "rbf";
            this.rb1Rbtn.UseVisualStyleBackColor = false;
            this.rb1Rbtn.CheckedChanged += new System.EventHandler(this.rb1Rbtn_CheckedChanged);
            // 
            // MaxIterationsTxt
            // 
            this.MaxIterationsTxt.EditValue = "100";
            this.MaxIterationsTxt.Location = new System.Drawing.Point(247, 337);
            this.MaxIterationsTxt.Name = "MaxIterationsTxt";
            this.MaxIterationsTxt.Properties.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(222)))), ((int)(((byte)(171)))));
            this.MaxIterationsTxt.Properties.Appearance.Font = new System.Drawing.Font("微软雅黑", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.MaxIterationsTxt.Properties.Appearance.ForeColor = System.Drawing.Color.Black;
            this.MaxIterationsTxt.Properties.Appearance.Options.UseBackColor = true;
            this.MaxIterationsTxt.Properties.Appearance.Options.UseFont = true;
            this.MaxIterationsTxt.Properties.Appearance.Options.UseForeColor = true;
            this.MaxIterationsTxt.Size = new System.Drawing.Size(240, 34);
            this.MaxIterationsTxt.TabIndex = 16;
            // 
            // regularizationparameterTxt
            // 
            this.regularizationparameterTxt.EditValue = "1.0";
            this.regularizationparameterTxt.Location = new System.Drawing.Point(247, 289);
            this.regularizationparameterTxt.Name = "regularizationparameterTxt";
            this.regularizationparameterTxt.Properties.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(222)))), ((int)(((byte)(171)))));
            this.regularizationparameterTxt.Properties.Appearance.Font = new System.Drawing.Font("微软雅黑", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.regularizationparameterTxt.Properties.Appearance.ForeColor = System.Drawing.Color.Black;
            this.regularizationparameterTxt.Properties.Appearance.Options.UseBackColor = true;
            this.regularizationparameterTxt.Properties.Appearance.Options.UseFont = true;
            this.regularizationparameterTxt.Properties.Appearance.Options.UseForeColor = true;
            this.regularizationparameterTxt.Size = new System.Drawing.Size(240, 34);
            this.regularizationparameterTxt.TabIndex = 17;
            // 
            // toleranceTxt
            // 
            this.toleranceTxt.EditValue = "0.001";
            this.toleranceTxt.Location = new System.Drawing.Point(247, 236);
            this.toleranceTxt.Name = "toleranceTxt";
            this.toleranceTxt.Properties.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(222)))), ((int)(((byte)(171)))));
            this.toleranceTxt.Properties.Appearance.Font = new System.Drawing.Font("微软雅黑", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.toleranceTxt.Properties.Appearance.ForeColor = System.Drawing.Color.Black;
            this.toleranceTxt.Properties.Appearance.Options.UseBackColor = true;
            this.toleranceTxt.Properties.Appearance.Options.UseFont = true;
            this.toleranceTxt.Properties.Appearance.Options.UseForeColor = true;
            this.toleranceTxt.Size = new System.Drawing.Size(240, 34);
            this.toleranceTxt.TabIndex = 18;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Font = new System.Drawing.Font("微软雅黑", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label5.Location = new System.Drawing.Point(95, 335);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(138, 28);
            this.label5.TabIndex = 12;
            this.label5.Text = "最大迭代次数";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("微软雅黑", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(95, 287);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(117, 28);
            this.label4.TabIndex = 13;
            this.label4.Text = "正则化参数";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("微软雅黑", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(95, 234);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 28);
            this.label2.TabIndex = 14;
            this.label2.Text = "容忍度";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("微软雅黑", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(95, 151);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(75, 28);
            this.label3.TabIndex = 15;
            this.label3.Text = "核函数";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label1.Location = new System.Drawing.Point(31, 61);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(158, 31);
            this.label1.TabIndex = 11;
            this.label1.Text = "SVR参数设置";
            // 
            // sigmoidRbtn
            // 
            this.sigmoidRbtn.AutoSize = true;
            this.sigmoidRbtn.BackColor = System.Drawing.Color.Transparent;
            this.sigmoidRbtn.Font = new System.Drawing.Font("微软雅黑", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.sigmoidRbtn.ForeColor = System.Drawing.Color.White;
            this.sigmoidRbtn.Location = new System.Drawing.Point(247, 191);
            this.sigmoidRbtn.Name = "sigmoidRbtn";
            this.sigmoidRbtn.Size = new System.Drawing.Size(115, 32);
            this.sigmoidRbtn.TabIndex = 21;
            this.sigmoidRbtn.Text = "sigmoid";
            this.sigmoidRbtn.UseVisualStyleBackColor = false;
            this.sigmoidRbtn.CheckedChanged += new System.EventHandler(this.sigmoidRbtn_CheckedChanged);
            // 
            // precomputerRbtn
            // 
            this.precomputerRbtn.AutoSize = true;
            this.precomputerRbtn.BackColor = System.Drawing.Color.Transparent;
            this.precomputerRbtn.Font = new System.Drawing.Font("微软雅黑", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.precomputerRbtn.ForeColor = System.Drawing.Color.White;
            this.precomputerRbtn.Location = new System.Drawing.Point(368, 191);
            this.precomputerRbtn.Name = "precomputerRbtn";
            this.precomputerRbtn.Size = new System.Drawing.Size(169, 32);
            this.precomputerRbtn.TabIndex = 20;
            this.precomputerRbtn.Text = "precomputer";
            this.precomputerRbtn.UseVisualStyleBackColor = false;
            this.precomputerRbtn.CheckedChanged += new System.EventHandler(this.precomputerRbtn_CheckedChanged);
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
            this.panel1.Controls.Add(this.polyRbtn);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.precomputerRbtn);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.lineaRbtn);
            this.panel1.Controls.Add(this.toleranceTxt);
            this.panel1.Controls.Add(this.sigmoidRbtn);
            this.panel1.Controls.Add(this.regularizationparameterTxt);
            this.panel1.Controls.Add(this.rb1Rbtn);
            this.panel1.Controls.Add(this.MaxIterationsTxt);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(624, 491);
            this.panel1.TabIndex = 24;
            // 
            // SVRForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::WisdomGrowth.Properties.Resources.bg_1;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(624, 491);
            this.Controls.Add(this.panel1);
            this.DoubleBuffered = true;
            this.Name = "SVRForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SVRForm";
            this.Load += new System.EventHandler(this.SVRForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.MaxIterationsTxt.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.regularizationparameterTxt.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.toleranceTxt.Properties)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button CancelBtn;
        private System.Windows.Forms.Button OKBtn;
        private System.Windows.Forms.RadioButton polyRbtn;
        private System.Windows.Forms.RadioButton lineaRbtn;
        private System.Windows.Forms.RadioButton rb1Rbtn;
        private DevExpress.XtraEditors.TextEdit MaxIterationsTxt;
        private DevExpress.XtraEditors.TextEdit regularizationparameterTxt;
        private DevExpress.XtraEditors.TextEdit toleranceTxt;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton sigmoidRbtn;
        private System.Windows.Forms.RadioButton precomputerRbtn;
        private System.Windows.Forms.Panel panel1;
    }
}