namespace WisdomGrowth.ModelTraining
{
    partial class GBRForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GBRForm));
            this.CancelBtn = new System.Windows.Forms.Button();
            this.OKBtn = new System.Windows.Forms.Button();
            this.quantileRbtn = new System.Windows.Forms.RadioButton();
            this.huberRbtn = new System.Windows.Forms.RadioButton();
            this.absolute_errRbtn = new System.Windows.Forms.RadioButton();
            this.squared_erRbtn = new System.Windows.Forms.RadioButton();
            this.MinimumpartitionnodeTxt = new DevExpress.XtraEditors.TextEdit();
            this.NumberoflearnersTxt = new DevExpress.XtraEditors.TextEdit();
            this.LearningrateTxt = new DevExpress.XtraEditors.TextEdit();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.MaximumdepthTxt = new DevExpress.XtraEditors.TextEdit();
            this.panel1 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.MinimumpartitionnodeTxt.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumberoflearnersTxt.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LearningrateTxt.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MaximumdepthTxt.Properties)).BeginInit();
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
            this.CancelBtn.Location = new System.Drawing.Point(402, 404);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(92, 34);
            this.CancelBtn.TabIndex = 37;
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
            this.OKBtn.Location = new System.Drawing.Point(272, 404);
            this.OKBtn.Name = "OKBtn";
            this.OKBtn.Size = new System.Drawing.Size(92, 34);
            this.OKBtn.TabIndex = 38;
            this.OKBtn.Text = "OK";
            this.OKBtn.UseVisualStyleBackColor = false;
            this.OKBtn.Click += new System.EventHandler(this.OKBtn_Click);
            // 
            // quantileRbtn
            // 
            this.quantileRbtn.AutoSize = true;
            this.quantileRbtn.BackColor = System.Drawing.Color.Transparent;
            this.quantileRbtn.Font = new System.Drawing.Font("微软雅黑", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.quantileRbtn.ForeColor = System.Drawing.Color.White;
            this.quantileRbtn.Location = new System.Drawing.Point(396, 153);
            this.quantileRbtn.Name = "quantileRbtn";
            this.quantileRbtn.Size = new System.Drawing.Size(117, 32);
            this.quantileRbtn.TabIndex = 33;
            this.quantileRbtn.Text = "quantile";
            this.quantileRbtn.UseVisualStyleBackColor = false;
            this.quantileRbtn.CheckedChanged += new System.EventHandler(this.quantileRbtn_CheckedChanged);
            // 
            // huberRbtn
            // 
            this.huberRbtn.AutoSize = true;
            this.huberRbtn.BackColor = System.Drawing.Color.Transparent;
            this.huberRbtn.Font = new System.Drawing.Font("微软雅黑", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.huberRbtn.ForeColor = System.Drawing.Color.White;
            this.huberRbtn.Location = new System.Drawing.Point(396, 115);
            this.huberRbtn.Name = "huberRbtn";
            this.huberRbtn.Size = new System.Drawing.Size(93, 32);
            this.huberRbtn.TabIndex = 34;
            this.huberRbtn.Text = "huber";
            this.huberRbtn.UseVisualStyleBackColor = false;
            this.huberRbtn.CheckedChanged += new System.EventHandler(this.huberRbtn_CheckedChanged);
            // 
            // absolute_errRbtn
            // 
            this.absolute_errRbtn.AutoSize = true;
            this.absolute_errRbtn.BackColor = System.Drawing.Color.Transparent;
            this.absolute_errRbtn.Font = new System.Drawing.Font("微软雅黑", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.absolute_errRbtn.ForeColor = System.Drawing.Color.White;
            this.absolute_errRbtn.Location = new System.Drawing.Point(256, 153);
            this.absolute_errRbtn.Name = "absolute_errRbtn";
            this.absolute_errRbtn.Size = new System.Drawing.Size(160, 32);
            this.absolute_errRbtn.TabIndex = 35;
            this.absolute_errRbtn.Text = "absolute_err";
            this.absolute_errRbtn.UseVisualStyleBackColor = false;
            this.absolute_errRbtn.CheckedChanged += new System.EventHandler(this.absolute_errRbtn_CheckedChanged);
            // 
            // squared_erRbtn
            // 
            this.squared_erRbtn.AutoSize = true;
            this.squared_erRbtn.BackColor = System.Drawing.Color.Transparent;
            this.squared_erRbtn.Checked = true;
            this.squared_erRbtn.Font = new System.Drawing.Font("微软雅黑", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.squared_erRbtn.ForeColor = System.Drawing.Color.White;
            this.squared_erRbtn.Location = new System.Drawing.Point(256, 113);
            this.squared_erRbtn.Name = "squared_erRbtn";
            this.squared_erRbtn.Size = new System.Drawing.Size(145, 32);
            this.squared_erRbtn.TabIndex = 36;
            this.squared_erRbtn.TabStop = true;
            this.squared_erRbtn.Text = "squared_er";
            this.squared_erRbtn.UseVisualStyleBackColor = false;
            this.squared_erRbtn.CheckedChanged += new System.EventHandler(this.squared_erRbtn_CheckedChanged);
            // 
            // MinimumpartitionnodeTxt
            // 
            this.MinimumpartitionnodeTxt.EditValue = "2";
            this.MinimumpartitionnodeTxt.Location = new System.Drawing.Point(254, 296);
            this.MinimumpartitionnodeTxt.Name = "MinimumpartitionnodeTxt";
            this.MinimumpartitionnodeTxt.Properties.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(222)))), ((int)(((byte)(171)))));
            this.MinimumpartitionnodeTxt.Properties.Appearance.Font = new System.Drawing.Font("微软雅黑", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.MinimumpartitionnodeTxt.Properties.Appearance.ForeColor = System.Drawing.Color.Black;
            this.MinimumpartitionnodeTxt.Properties.Appearance.Options.UseBackColor = true;
            this.MinimumpartitionnodeTxt.Properties.Appearance.Options.UseFont = true;
            this.MinimumpartitionnodeTxt.Properties.Appearance.Options.UseForeColor = true;
            this.MinimumpartitionnodeTxt.Size = new System.Drawing.Size(240, 34);
            this.MinimumpartitionnodeTxt.TabIndex = 29;
            // 
            // NumberoflearnersTxt
            // 
            this.NumberoflearnersTxt.EditValue = "100";
            this.NumberoflearnersTxt.Location = new System.Drawing.Point(254, 245);
            this.NumberoflearnersTxt.Name = "NumberoflearnersTxt";
            this.NumberoflearnersTxt.Properties.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(222)))), ((int)(((byte)(171)))));
            this.NumberoflearnersTxt.Properties.Appearance.Font = new System.Drawing.Font("微软雅黑", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.NumberoflearnersTxt.Properties.Appearance.ForeColor = System.Drawing.Color.Black;
            this.NumberoflearnersTxt.Properties.Appearance.Options.UseBackColor = true;
            this.NumberoflearnersTxt.Properties.Appearance.Options.UseFont = true;
            this.NumberoflearnersTxt.Properties.Appearance.Options.UseForeColor = true;
            this.NumberoflearnersTxt.Size = new System.Drawing.Size(240, 34);
            this.NumberoflearnersTxt.TabIndex = 30;
            // 
            // LearningrateTxt
            // 
            this.LearningrateTxt.EditValue = "0.1";
            this.LearningrateTxt.Location = new System.Drawing.Point(254, 198);
            this.LearningrateTxt.Name = "LearningrateTxt";
            this.LearningrateTxt.Properties.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(222)))), ((int)(((byte)(171)))));
            this.LearningrateTxt.Properties.Appearance.Font = new System.Drawing.Font("微软雅黑", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.LearningrateTxt.Properties.Appearance.ForeColor = System.Drawing.Color.Black;
            this.LearningrateTxt.Properties.Appearance.Options.UseBackColor = true;
            this.LearningrateTxt.Properties.Appearance.Options.UseFont = true;
            this.LearningrateTxt.Properties.Appearance.Options.UseForeColor = true;
            this.LearningrateTxt.Size = new System.Drawing.Size(240, 34);
            this.LearningrateTxt.TabIndex = 31;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Font = new System.Drawing.Font("微软雅黑", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label5.Location = new System.Drawing.Point(102, 294);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(138, 28);
            this.label5.TabIndex = 25;
            this.label5.Text = "最小划分节点";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("微软雅黑", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(102, 243);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(117, 28);
            this.label4.TabIndex = 26;
            this.label4.Text = "学习器数量";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("微软雅黑", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(104, 196);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 28);
            this.label2.TabIndex = 27;
            this.label2.Text = "学习率";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("微软雅黑", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(104, 113);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(96, 28);
            this.label3.TabIndex = 28;
            this.label3.Text = "损失函数";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label1.Location = new System.Drawing.Point(26, 48);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(161, 31);
            this.label1.TabIndex = 24;
            this.label1.Text = "GBR参数设置";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Font = new System.Drawing.Font("微软雅黑", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label6.ForeColor = System.Drawing.Color.White;
            this.label6.Location = new System.Drawing.Point(102, 346);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(96, 28);
            this.label6.TabIndex = 25;
            this.label6.Text = "最大深度";
            // 
            // MaximumdepthTxt
            // 
            this.MaximumdepthTxt.EditValue = "3";
            this.MaximumdepthTxt.Location = new System.Drawing.Point(254, 348);
            this.MaximumdepthTxt.Name = "MaximumdepthTxt";
            this.MaximumdepthTxt.Properties.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(222)))), ((int)(((byte)(171)))));
            this.MaximumdepthTxt.Properties.Appearance.Font = new System.Drawing.Font("微软雅黑", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.MaximumdepthTxt.Properties.Appearance.ForeColor = System.Drawing.Color.Black;
            this.MaximumdepthTxt.Properties.Appearance.Options.UseBackColor = true;
            this.MaximumdepthTxt.Properties.Appearance.Options.UseFont = true;
            this.MaximumdepthTxt.Properties.Appearance.Options.UseForeColor = true;
            this.MaximumdepthTxt.Size = new System.Drawing.Size(240, 34);
            this.MaximumdepthTxt.TabIndex = 29;
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
            this.panel1.Controls.Add(this.quantileRbtn);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.huberRbtn);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.absolute_errRbtn);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.squared_erRbtn);
            this.panel1.Controls.Add(this.LearningrateTxt);
            this.panel1.Controls.Add(this.MaximumdepthTxt);
            this.panel1.Controls.Add(this.NumberoflearnersTxt);
            this.panel1.Controls.Add(this.MinimumpartitionnodeTxt);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(624, 491);
            this.panel1.TabIndex = 39;
            // 
            // GBRForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::WisdomGrowth.Properties.Resources.bg_1;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(624, 491);
            this.Controls.Add(this.panel1);
            this.DoubleBuffered = true;
            this.Name = "GBRForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "GBRForm";
            this.Load += new System.EventHandler(this.GBRForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.MinimumpartitionnodeTxt.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumberoflearnersTxt.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LearningrateTxt.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MaximumdepthTxt.Properties)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button CancelBtn;
        private System.Windows.Forms.Button OKBtn;
        private System.Windows.Forms.RadioButton quantileRbtn;
        private System.Windows.Forms.RadioButton huberRbtn;
        private System.Windows.Forms.RadioButton absolute_errRbtn;
        private System.Windows.Forms.RadioButton squared_erRbtn;
        private DevExpress.XtraEditors.TextEdit MinimumpartitionnodeTxt;
        private DevExpress.XtraEditors.TextEdit NumberoflearnersTxt;
        private DevExpress.XtraEditors.TextEdit LearningrateTxt;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label6;
        private DevExpress.XtraEditors.TextEdit MaximumdepthTxt;
        private System.Windows.Forms.Panel panel1;
    }
}