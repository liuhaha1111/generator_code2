namespace WisdomGrowth.ModelTraining
{
    partial class RFRForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RFRForm));
            this.CancelBtn = new System.Windows.Forms.Button();
            this.OKBtn = new System.Windows.Forms.Button();
            this.MaximumdepthTxt = new DevExpress.XtraEditors.TextEdit();
            this.MinimumpartitionnodeTxt = new DevExpress.XtraEditors.TextEdit();
            this.NumberoflearnersTxt = new DevExpress.XtraEditors.TextEdit();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.MaximumdepthTxt.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MinimumpartitionnodeTxt.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumberoflearnersTxt.Properties)).BeginInit();
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
            this.CancelBtn.Location = new System.Drawing.Point(395, 373);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(92, 36);
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
            this.OKBtn.Location = new System.Drawing.Point(265, 373);
            this.OKBtn.Name = "OKBtn";
            this.OKBtn.Size = new System.Drawing.Size(92, 36);
            this.OKBtn.TabIndex = 23;
            this.OKBtn.Text = "OK";
            this.OKBtn.UseVisualStyleBackColor = false;
            this.OKBtn.Click += new System.EventHandler(this.OKBtn_Click);
            // 
            // MaximumdepthTxt
            // 
            this.MaximumdepthTxt.EditValue = "3";
            this.MaximumdepthTxt.Location = new System.Drawing.Point(305, 293);
            this.MaximumdepthTxt.Name = "MaximumdepthTxt";
            this.MaximumdepthTxt.Properties.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(222)))), ((int)(((byte)(171)))));
            this.MaximumdepthTxt.Properties.Appearance.Font = new System.Drawing.Font("微软雅黑", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.MaximumdepthTxt.Properties.Appearance.ForeColor = System.Drawing.Color.Black;
            this.MaximumdepthTxt.Properties.Appearance.Options.UseBackColor = true;
            this.MaximumdepthTxt.Properties.Appearance.Options.UseFont = true;
            this.MaximumdepthTxt.Properties.Appearance.Options.UseForeColor = true;
            this.MaximumdepthTxt.Size = new System.Drawing.Size(182, 34);
            this.MaximumdepthTxt.TabIndex = 16;
            // 
            // MinimumpartitionnodeTxt
            // 
            this.MinimumpartitionnodeTxt.EditValue = "2";
            this.MinimumpartitionnodeTxt.Location = new System.Drawing.Point(305, 229);
            this.MinimumpartitionnodeTxt.Name = "MinimumpartitionnodeTxt";
            this.MinimumpartitionnodeTxt.Properties.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(222)))), ((int)(((byte)(171)))));
            this.MinimumpartitionnodeTxt.Properties.Appearance.Font = new System.Drawing.Font("微软雅黑", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.MinimumpartitionnodeTxt.Properties.Appearance.ForeColor = System.Drawing.Color.Black;
            this.MinimumpartitionnodeTxt.Properties.Appearance.Options.UseBackColor = true;
            this.MinimumpartitionnodeTxt.Properties.Appearance.Options.UseFont = true;
            this.MinimumpartitionnodeTxt.Properties.Appearance.Options.UseForeColor = true;
            this.MinimumpartitionnodeTxt.Size = new System.Drawing.Size(182, 34);
            this.MinimumpartitionnodeTxt.TabIndex = 17;
            // 
            // NumberoflearnersTxt
            // 
            this.NumberoflearnersTxt.EditValue = "100";
            this.NumberoflearnersTxt.Location = new System.Drawing.Point(305, 167);
            this.NumberoflearnersTxt.Name = "NumberoflearnersTxt";
            this.NumberoflearnersTxt.Properties.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(222)))), ((int)(((byte)(171)))));
            this.NumberoflearnersTxt.Properties.Appearance.Font = new System.Drawing.Font("微软雅黑", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.NumberoflearnersTxt.Properties.Appearance.ForeColor = System.Drawing.Color.Black;
            this.NumberoflearnersTxt.Properties.Appearance.Options.UseBackColor = true;
            this.NumberoflearnersTxt.Properties.Appearance.Options.UseFont = true;
            this.NumberoflearnersTxt.Properties.Appearance.Options.UseForeColor = true;
            this.NumberoflearnersTxt.Size = new System.Drawing.Size(182, 34);
            this.NumberoflearnersTxt.TabIndex = 18;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("微软雅黑", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(153, 291);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(96, 28);
            this.label4.TabIndex = 13;
            this.label4.Text = "最大深度";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("微软雅黑", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(153, 227);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(138, 28);
            this.label2.TabIndex = 14;
            this.label2.Text = "最小划分节点";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("微软雅黑", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(153, 165);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(117, 28);
            this.label3.TabIndex = 15;
            this.label3.Text = "学习器数量";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label1.Location = new System.Drawing.Point(35, 66);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(157, 31);
            this.label1.TabIndex = 11;
            this.label1.Text = "RFR参数设置";
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
            this.panel1.Controls.Add(this.MaximumdepthTxt);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.MinimumpartitionnodeTxt);
            this.panel1.Controls.Add(this.NumberoflearnersTxt);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(624, 491);
            this.panel1.TabIndex = 24;
            // 
            // RFRForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::WisdomGrowth.Properties.Resources.bg_1;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(624, 491);
            this.Controls.Add(this.panel1);
            this.DoubleBuffered = true;
            this.Name = "RFRForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "RFRForm";
            ((System.ComponentModel.ISupportInitialize)(this.MaximumdepthTxt.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MinimumpartitionnodeTxt.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumberoflearnersTxt.Properties)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button CancelBtn;
        private System.Windows.Forms.Button OKBtn;
        private DevExpress.XtraEditors.TextEdit MaximumdepthTxt;
        private DevExpress.XtraEditors.TextEdit MinimumpartitionnodeTxt;
        private DevExpress.XtraEditors.TextEdit NumberoflearnersTxt;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel1;
    }
}