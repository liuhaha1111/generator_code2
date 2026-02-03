namespace WisdomGrowth
{
    partial class ModelTrainingForm
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea3 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend3 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series3 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.panel1 = new System.Windows.Forms.Panel();
            this.textEdit2 = new DevExpress.XtraEditors.TextEdit();
            this.textEdit1 = new DevExpress.XtraEditors.TextEdit();
            this.StackingModelBtn = new System.Windows.Forms.RadioButton();
            this.RFRFModel1Btn = new System.Windows.Forms.RadioButton();
            this.RFRModelBtn = new System.Windows.Forms.RadioButton();
            this.GBRModel1Btn = new System.Windows.Forms.RadioButton();
            this.GBRModelBtn = new System.Windows.Forms.RadioButton();
            this.SVRModel1Btn = new System.Windows.Forms.RadioButton();
            this.BPNNModel1Btn = new System.Windows.Forms.RadioButton();
            this.SVRModelBtn = new System.Windows.Forms.RadioButton();
            this.BPNNModelBtn = new System.Windows.Forms.RadioButton();
            this.RFRFModel0Box = new System.Windows.Forms.CheckBox();
            this.BPNNModel0Box = new System.Windows.Forms.CheckBox();
            this.SVRModel0Box = new System.Windows.Forms.CheckBox();
            this.GBRModel0Box = new System.Windows.Forms.CheckBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.DrawBtn = new System.Windows.Forms.Button();
            this.TrainingBtn = new System.Windows.Forms.Button();
            this.textEdit3 = new DevExpress.XtraEditors.TextEdit();
            this.groupBox1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.textEdit2.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEdit1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEdit3.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.Transparent;
            this.groupBox1.Controls.Add(this.panel2);
            this.groupBox1.Controls.Add(this.panel1);
            this.groupBox1.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox1.ForeColor = System.Drawing.Color.White;
            this.groupBox1.Location = new System.Drawing.Point(22, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1236, 695);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "模型训练";
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.chart1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(3, 274);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1230, 408);
            this.panel2.TabIndex = 5;
            // 
            // chart1
            // 
            this.chart1.BorderlineColor = System.Drawing.Color.Transparent;
            chartArea3.Name = "ChartArea1";
            this.chart1.ChartAreas.Add(chartArea3);
            this.chart1.Dock = System.Windows.Forms.DockStyle.Fill;
            legend3.Name = "Legend1";
            this.chart1.Legends.Add(legend3);
            this.chart1.Location = new System.Drawing.Point(0, 0);
            this.chart1.Name = "chart1";
            series3.ChartArea = "ChartArea1";
            series3.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series3.Legend = "Legend1";
            series3.Name = "Series1";
            this.chart1.Series.Add(series3);
            this.chart1.Size = new System.Drawing.Size(1228, 406);
            this.chart1.TabIndex = 0;
            this.chart1.Text = "chart1";
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.textEdit3);
            this.panel1.Controls.Add(this.textEdit2);
            this.panel1.Controls.Add(this.textEdit1);
            this.panel1.Controls.Add(this.StackingModelBtn);
            this.panel1.Controls.Add(this.RFRFModel1Btn);
            this.panel1.Controls.Add(this.RFRModelBtn);
            this.panel1.Controls.Add(this.GBRModel1Btn);
            this.panel1.Controls.Add(this.GBRModelBtn);
            this.panel1.Controls.Add(this.SVRModel1Btn);
            this.panel1.Controls.Add(this.BPNNModel1Btn);
            this.panel1.Controls.Add(this.SVRModelBtn);
            this.panel1.Controls.Add(this.BPNNModelBtn);
            this.panel1.Controls.Add(this.RFRFModel0Box);
            this.panel1.Controls.Add(this.BPNNModel0Box);
            this.panel1.Controls.Add(this.SVRModel0Box);
            this.panel1.Controls.Add(this.GBRModel0Box);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.DrawBtn);
            this.panel1.Controls.Add(this.TrainingBtn);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(3, 25);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1230, 249);
            this.panel1.TabIndex = 4;
            // 
            // textEdit2
            // 
            this.textEdit2.Location = new System.Drawing.Point(647, 171);
            this.textEdit2.Name = "textEdit2";
            this.textEdit2.Size = new System.Drawing.Size(206, 20);
            this.textEdit2.TabIndex = 7;
            // 
            // textEdit1
            // 
            this.textEdit1.Location = new System.Drawing.Point(190, 171);
            this.textEdit1.Name = "textEdit1";
            this.textEdit1.Size = new System.Drawing.Size(182, 20);
            this.textEdit1.TabIndex = 7;
            // 
            // StackingModelBtn
            // 
            this.StackingModelBtn.AutoSize = true;
            this.StackingModelBtn.Location = new System.Drawing.Point(866, 19);
            this.StackingModelBtn.Name = "StackingModelBtn";
            this.StackingModelBtn.Size = new System.Drawing.Size(98, 26);
            this.StackingModelBtn.TabIndex = 6;
            this.StackingModelBtn.TabStop = true;
            this.StackingModelBtn.Text = "Stacking";
            this.StackingModelBtn.UseVisualStyleBackColor = true;
            this.StackingModelBtn.CheckedChanged += new System.EventHandler(this.StackingModelBtn_CheckedChanged);
            // 
            // RFRFModel1Btn
            // 
            this.RFRFModel1Btn.AutoSize = true;
            this.RFRFModel1Btn.Location = new System.Drawing.Point(866, 110);
            this.RFRFModel1Btn.Name = "RFRFModel1Btn";
            this.RFRFModel1Btn.Size = new System.Drawing.Size(59, 26);
            this.RFRFModel1Btn.TabIndex = 6;
            this.RFRFModel1Btn.TabStop = true;
            this.RFRFModel1Btn.Text = "RFR";
            this.RFRFModel1Btn.UseVisualStyleBackColor = true;
            this.RFRFModel1Btn.CheckedChanged += new System.EventHandler(this.RFRFModel1Btn_CheckedChanged);
            // 
            // RFRModelBtn
            // 
            this.RFRModelBtn.AutoSize = true;
            this.RFRModelBtn.Location = new System.Drawing.Point(692, 19);
            this.RFRModelBtn.Name = "RFRModelBtn";
            this.RFRModelBtn.Size = new System.Drawing.Size(59, 26);
            this.RFRModelBtn.TabIndex = 6;
            this.RFRModelBtn.TabStop = true;
            this.RFRModelBtn.Text = "RFR";
            this.RFRModelBtn.UseVisualStyleBackColor = true;
            this.RFRModelBtn.CheckedChanged += new System.EventHandler(this.RFRModelBtn_CheckedChanged);
            // 
            // GBRModel1Btn
            // 
            this.GBRModel1Btn.AutoSize = true;
            this.GBRModel1Btn.Location = new System.Drawing.Point(692, 110);
            this.GBRModel1Btn.Name = "GBRModel1Btn";
            this.GBRModel1Btn.Size = new System.Drawing.Size(62, 26);
            this.GBRModel1Btn.TabIndex = 6;
            this.GBRModel1Btn.TabStop = true;
            this.GBRModel1Btn.Text = "GBR";
            this.GBRModel1Btn.UseVisualStyleBackColor = true;
            this.GBRModel1Btn.CheckedChanged += new System.EventHandler(this.GBRModel1Btn_CheckedChanged);
            // 
            // GBRModelBtn
            // 
            this.GBRModelBtn.AutoSize = true;
            this.GBRModelBtn.Location = new System.Drawing.Point(514, 19);
            this.GBRModelBtn.Name = "GBRModelBtn";
            this.GBRModelBtn.Size = new System.Drawing.Size(62, 26);
            this.GBRModelBtn.TabIndex = 6;
            this.GBRModelBtn.TabStop = true;
            this.GBRModelBtn.Text = "GBR";
            this.GBRModelBtn.UseVisualStyleBackColor = true;
            this.GBRModelBtn.CheckedChanged += new System.EventHandler(this.GBRModelBtn_CheckedChanged);
            // 
            // SVRModel1Btn
            // 
            this.SVRModel1Btn.AutoSize = true;
            this.SVRModel1Btn.Location = new System.Drawing.Point(514, 110);
            this.SVRModel1Btn.Name = "SVRModel1Btn";
            this.SVRModel1Btn.Size = new System.Drawing.Size(60, 26);
            this.SVRModel1Btn.TabIndex = 6;
            this.SVRModel1Btn.TabStop = true;
            this.SVRModel1Btn.Text = "SVR";
            this.SVRModel1Btn.UseVisualStyleBackColor = true;
            this.SVRModel1Btn.CheckedChanged += new System.EventHandler(this.SVRModel1Btn_CheckedChanged);
            // 
            // BPNNModel1Btn
            // 
            this.BPNNModel1Btn.AutoSize = true;
            this.BPNNModel1Btn.Location = new System.Drawing.Point(360, 110);
            this.BPNNModel1Btn.Name = "BPNNModel1Btn";
            this.BPNNModel1Btn.Size = new System.Drawing.Size(78, 26);
            this.BPNNModel1Btn.TabIndex = 6;
            this.BPNNModel1Btn.TabStop = true;
            this.BPNNModel1Btn.Text = "BPNN";
            this.BPNNModel1Btn.UseVisualStyleBackColor = true;
            this.BPNNModel1Btn.CheckedChanged += new System.EventHandler(this.BPNNModel1Btn_CheckedChanged);
            // 
            // SVRModelBtn
            // 
            this.SVRModelBtn.AutoSize = true;
            this.SVRModelBtn.Location = new System.Drawing.Point(360, 19);
            this.SVRModelBtn.Name = "SVRModelBtn";
            this.SVRModelBtn.Size = new System.Drawing.Size(60, 26);
            this.SVRModelBtn.TabIndex = 6;
            this.SVRModelBtn.TabStop = true;
            this.SVRModelBtn.Text = "SVR";
            this.SVRModelBtn.UseVisualStyleBackColor = true;
            this.SVRModelBtn.CheckedChanged += new System.EventHandler(this.SVRModelBtn_CheckedChanged);
            // 
            // BPNNModelBtn
            // 
            this.BPNNModelBtn.AutoSize = true;
            this.BPNNModelBtn.Location = new System.Drawing.Point(190, 21);
            this.BPNNModelBtn.Name = "BPNNModelBtn";
            this.BPNNModelBtn.Size = new System.Drawing.Size(78, 26);
            this.BPNNModelBtn.TabIndex = 6;
            this.BPNNModelBtn.TabStop = true;
            this.BPNNModelBtn.Text = "BPNN";
            this.BPNNModelBtn.UseVisualStyleBackColor = true;
            this.BPNNModelBtn.CheckedChanged += new System.EventHandler(this.BPNNModelBtn_CheckedChanged);
            // 
            // RFRFModel0Box
            // 
            this.RFRFModel0Box.AutoSize = true;
            this.RFRFModel0Box.Location = new System.Drawing.Point(866, 65);
            this.RFRFModel0Box.Name = "RFRFModel0Box";
            this.RFRFModel0Box.Size = new System.Drawing.Size(60, 26);
            this.RFRFModel0Box.TabIndex = 5;
            this.RFRFModel0Box.Text = "RFR";
            this.RFRFModel0Box.UseVisualStyleBackColor = true;
            this.RFRFModel0Box.CheckedChanged += new System.EventHandler(this.RFRFModel0Box_CheckedChanged);
            // 
            // BPNNModel0Box
            // 
            this.BPNNModel0Box.AutoSize = true;
            this.BPNNModel0Box.Location = new System.Drawing.Point(360, 65);
            this.BPNNModel0Box.Name = "BPNNModel0Box";
            this.BPNNModel0Box.Size = new System.Drawing.Size(79, 26);
            this.BPNNModel0Box.TabIndex = 5;
            this.BPNNModel0Box.Text = "BPNN";
            this.BPNNModel0Box.UseVisualStyleBackColor = true;
            this.BPNNModel0Box.CheckedChanged += new System.EventHandler(this.BPNNModel0Box_CheckedChanged);
            // 
            // SVRModel0Box
            // 
            this.SVRModel0Box.AutoSize = true;
            this.SVRModel0Box.Location = new System.Drawing.Point(514, 65);
            this.SVRModel0Box.Name = "SVRModel0Box";
            this.SVRModel0Box.Size = new System.Drawing.Size(61, 26);
            this.SVRModel0Box.TabIndex = 5;
            this.SVRModel0Box.Text = "SVR";
            this.SVRModel0Box.UseVisualStyleBackColor = true;
            this.SVRModel0Box.CheckedChanged += new System.EventHandler(this.SVRModel0Box_CheckedChanged);
            // 
            // GBRModel0Box
            // 
            this.GBRModel0Box.AutoSize = true;
            this.GBRModel0Box.Location = new System.Drawing.Point(692, 65);
            this.GBRModel0Box.Name = "GBRModel0Box";
            this.GBRModel0Box.Size = new System.Drawing.Size(63, 26);
            this.GBRModel0Box.TabIndex = 5;
            this.GBRModel0Box.Text = "GBR";
            this.GBRModel0Box.UseVisualStyleBackColor = true;
            this.GBRModel0Box.CheckedChanged += new System.EventHandler(this.GBRModel0Box_CheckedChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(507, 169);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(74, 22);
            this.label7.TabIndex = 4;
            this.label7.Text = "训练分数";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(26, 169);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(74, 22);
            this.label6.TabIndex = 4;
            this.label6.Text = "训练时间";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(26, 23);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(74, 22);
            this.label3.TabIndex = 4;
            this.label3.Text = "选择模型";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(186, 112);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(77, 22);
            this.label4.TabIndex = 4;
            this.label4.Text = "Model 1";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(186, 69);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 22);
            this.label1.TabIndex = 4;
            this.label1.Text = "Model 0";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(927, 206);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(122, 22);
            this.label5.TabIndex = 4;
            this.label5.Text = "测试集预测结果";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(26, 88);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(144, 22);
            this.label2.TabIndex = 4;
            this.label2.Text = "Stacking模型配置";
            // 
            // DrawBtn
            // 
            this.DrawBtn.BackColor = System.Drawing.Color.Transparent;
            this.DrawBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.DrawBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.DrawBtn.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.DrawBtn.ForeColor = System.Drawing.Color.White;
            this.DrawBtn.Location = new System.Drawing.Point(1106, 200);
            this.DrawBtn.Name = "DrawBtn";
            this.DrawBtn.Size = new System.Drawing.Size(92, 28);
            this.DrawBtn.TabIndex = 3;
            this.DrawBtn.Text = "点击绘制";
            this.DrawBtn.UseVisualStyleBackColor = false;
            this.DrawBtn.Click += new System.EventHandler(this.DrawBtn_Click);
            // 
            // TrainingBtn
            // 
            this.TrainingBtn.BackColor = System.Drawing.Color.Transparent;
            this.TrainingBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.TrainingBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.TrainingBtn.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.TrainingBtn.ForeColor = System.Drawing.Color.White;
            this.TrainingBtn.Location = new System.Drawing.Point(1106, 69);
            this.TrainingBtn.Name = "TrainingBtn";
            this.TrainingBtn.Size = new System.Drawing.Size(92, 28);
            this.TrainingBtn.TabIndex = 3;
            this.TrainingBtn.Text = "开始训练";
            this.TrainingBtn.UseVisualStyleBackColor = false;
            this.TrainingBtn.Click += new System.EventHandler(this.TrainingBtn_Click);
            // 
            // textEdit3
            // 
            this.textEdit3.Location = new System.Drawing.Point(1106, 116);
            this.textEdit3.Name = "textEdit3";
            this.textEdit3.Properties.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.textEdit3.Properties.Appearance.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textEdit3.Properties.Appearance.ForeColor = System.Drawing.Color.Firebrick;
            this.textEdit3.Properties.Appearance.Options.UseBackColor = true;
            this.textEdit3.Properties.Appearance.Options.UseFont = true;
            this.textEdit3.Properties.Appearance.Options.UseForeColor = true;
            this.textEdit3.Size = new System.Drawing.Size(92, 26);
            this.textEdit3.TabIndex = 7;
            // 
            // ModelTrainingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::WisdomGrowth.Properties.Resources.bg1;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1270, 710);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "ModelTrainingForm";
            this.Text = "ModelTrainingForm";
            this.Load += new System.EventHandler(this.ModelTrainingForm_Load);
            this.SizeChanged += new System.EventHandler(this.ModelTrainingForm_SizeChanged);
            this.groupBox1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.textEdit2.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEdit1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEdit3.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RadioButton StackingModelBtn;
        private System.Windows.Forms.RadioButton RFRModelBtn;
        private System.Windows.Forms.RadioButton GBRModelBtn;
        private System.Windows.Forms.RadioButton SVRModelBtn;
        private System.Windows.Forms.RadioButton BPNNModelBtn;
        private System.Windows.Forms.CheckBox RFRFModel0Box;
        private System.Windows.Forms.CheckBox BPNNModel0Box;
        private System.Windows.Forms.CheckBox SVRModel0Box;
        private System.Windows.Forms.CheckBox GBRModel0Box;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button DrawBtn;
        private System.Windows.Forms.Button TrainingBtn;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton RFRFModel1Btn;
        private System.Windows.Forms.RadioButton GBRModel1Btn;
        private System.Windows.Forms.RadioButton SVRModel1Btn;
        private System.Windows.Forms.RadioButton BPNNModel1Btn;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private DevExpress.XtraEditors.TextEdit textEdit2;
        private DevExpress.XtraEditors.TextEdit textEdit1;
        private System.Windows.Forms.Label label7;
        private DevExpress.XtraEditors.TextEdit textEdit3;
    }
}