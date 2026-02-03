namespace WisdomGrowth
{
    partial class WisdomMenuForm
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.OutlierRemovalBtn = new System.Windows.Forms.Label();
            this.NumericPredictionBtn = new System.Windows.Forms.Label();
            this.ModelTrainingBtn = new System.Windows.Forms.Label();
            this.DataSelectionBtn = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackgroundImage = global::WisdomGrowth.Properties.Resources.top;
            this.panel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panel1.Controls.Add(this.OutlierRemovalBtn);
            this.panel1.Controls.Add(this.NumericPredictionBtn);
            this.panel1.Controls.Add(this.ModelTrainingBtn);
            this.panel1.Controls.Add(this.DataSelectionBtn);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1270, 30);
            this.panel1.TabIndex = 0;
            // 
            // OutlierRemovalBtn
            // 
            this.OutlierRemovalBtn.AutoSize = true;
            this.OutlierRemovalBtn.BackColor = System.Drawing.Color.Transparent;
            this.OutlierRemovalBtn.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.OutlierRemovalBtn.ForeColor = System.Drawing.SystemColors.InactiveCaption;
            this.OutlierRemovalBtn.Location = new System.Drawing.Point(121, 7);
            this.OutlierRemovalBtn.Name = "OutlierRemovalBtn";
            this.OutlierRemovalBtn.Size = new System.Drawing.Size(79, 20);
            this.OutlierRemovalBtn.TabIndex = 11;
            this.OutlierRemovalBtn.Text = "异常值去除";
            this.OutlierRemovalBtn.Click += new System.EventHandler(this.OutlierRemovalBtn_Click);
            // 
            // NumericPredictionBtn
            // 
            this.NumericPredictionBtn.AutoSize = true;
            this.NumericPredictionBtn.BackColor = System.Drawing.Color.Transparent;
            this.NumericPredictionBtn.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.NumericPredictionBtn.ForeColor = System.Drawing.SystemColors.InactiveCaption;
            this.NumericPredictionBtn.Location = new System.Drawing.Point(353, 7);
            this.NumericPredictionBtn.Name = "NumericPredictionBtn";
            this.NumericPredictionBtn.Size = new System.Drawing.Size(65, 20);
            this.NumericPredictionBtn.TabIndex = 11;
            this.NumericPredictionBtn.Text = "数值预测";
            this.NumericPredictionBtn.Click += new System.EventHandler(this.NumericPredictionBtn_Click);
            // 
            // ModelTrainingBtn
            // 
            this.ModelTrainingBtn.AutoSize = true;
            this.ModelTrainingBtn.BackColor = System.Drawing.Color.Transparent;
            this.ModelTrainingBtn.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ModelTrainingBtn.ForeColor = System.Drawing.SystemColors.InactiveCaption;
            this.ModelTrainingBtn.Location = new System.Drawing.Point(235, 7);
            this.ModelTrainingBtn.Name = "ModelTrainingBtn";
            this.ModelTrainingBtn.Size = new System.Drawing.Size(65, 20);
            this.ModelTrainingBtn.TabIndex = 11;
            this.ModelTrainingBtn.Text = "模型训练";
            this.ModelTrainingBtn.Click += new System.EventHandler(this.ModelTrainingBtn_Click);
            // 
            // DataSelectionBtn
            // 
            this.DataSelectionBtn.AutoSize = true;
            this.DataSelectionBtn.BackColor = System.Drawing.Color.Transparent;
            this.DataSelectionBtn.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.DataSelectionBtn.ForeColor = System.Drawing.SystemColors.InactiveCaption;
            this.DataSelectionBtn.Location = new System.Drawing.Point(22, 7);
            this.DataSelectionBtn.Name = "DataSelectionBtn";
            this.DataSelectionBtn.Size = new System.Drawing.Size(65, 20);
            this.DataSelectionBtn.TabIndex = 11;
            this.DataSelectionBtn.Text = "数据选取";
            this.DataSelectionBtn.Click += new System.EventHandler(this.DataSelectionBtn_Click);
            // 
            // panel2
            // 
            this.panel2.BackgroundImage = global::WisdomGrowth.Properties.Resources.bg_1;
            this.panel2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 30);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1270, 711);
            this.panel2.TabIndex = 1;
            // 
            // WisdomMenuForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::WisdomGrowth.Properties.Resources.bg1;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1270, 753);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "WisdomMenuForm";
            this.Text = "MenuForm1";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label NumericPredictionBtn;
        private System.Windows.Forms.Label OutlierRemovalBtn;
        private System.Windows.Forms.Label ModelTrainingBtn;
        private System.Windows.Forms.Label DataSelectionBtn;
    }
}