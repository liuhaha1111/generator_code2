using Python.Runtime;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using WisdomGrowth.DataBase;
using WisdomGrowth.ModelTraining;
using Accord.Neuro;
using Accord.Neuro.Learning;
using Accord.MachineLearning;
using Accord.MachineLearning.DecisionTrees;
using Accord.MachineLearning.DecisionTrees.Learning;
using Accord.Statistics.Models.Regression.Linear;
using Accord.Math.Optimization.Losses;
using Accord.Math;

namespace WisdomGrowth
{
    public partial class MaterialForm : Form
    {
        private float angle = 0; // 旋转角度
        private bool isLoading = false; // 是否正在加载
        private const int CircleRadius = 30; // 旋转圆的半径
        private const int CircleSize = 10; // 圆的大小
        private const int AnimationSpeed = 16; // 定时器间隔（约 60 FPS）
        private const int LoadingDuration = 1000; // 模拟加载时间（毫秒）
        private readonly Timer animationTimer; // 动画定时器

        // 定时器事件，用于刷新动画
        private void Timer_Tick(object sender, EventArgs e)
        {
            if (isLoading)
            {
                angle += 4; // 每次增加 4 度（更平滑的旋转）
                if (angle >= 360) angle = 0; // 角度超过 360 度时重置
                this.Invalidate(); // 触发重绘
            }
        }

        // 模拟加载操作
        private async Task SimulateLoadingAsync()
        {
            await Task.Delay(LoadingDuration); // 延迟 3 秒
        }

        private void SetControlsEnabled(Control control, bool isEnabled)
        {
            // 设置当前控件的启用状态
            control.Enabled = isEnabled;

            // 如果当前控件是容器控件，递归设置其子控件的启用状态
            foreach (Control childControl in control.Controls)
            {
                SetControlsEnabled(childControl, isEnabled);
            }
        }

        // 调用方法，设置窗体中所有控件的启用状态
        private void btnToggleControls(bool enabled)
        {
            SetControlsEnabled(this, enabled);  // 启用所有控件
            // SetControlsEnabled(this, false); // 禁用所有控件
        }

        public class LoadingOverlayForm : Form
        {
            private float angle = 0;
            private const int CircleRadius = 30;
            private const int CircleSize = 10;
            private const int AnimationSpeed = 16;
            private readonly Timer animationTimer;

            public LoadingOverlayForm(Form parentForm)
            {
                // 设置窗体属性
                this.FormBorderStyle = FormBorderStyle.None;
                this.BackColor = Color.LightGray; // 背景色
                this.TransparencyKey = Color.LightGray; // 透明色
                this.StartPosition = FormStartPosition.Manual;
                this.Size = parentForm.ClientSize;
                this.Location = parentForm.PointToScreen(Point.Empty);
                this.TopMost = true;
                this.DoubleBuffered = true; // 开启双缓冲

                // 初始化定时器
                animationTimer = new Timer();
                animationTimer.Interval = AnimationSpeed;
                animationTimer.Tick += Timer_Tick;
                animationTimer.Start();
            }

            private void Timer_Tick(object sender, EventArgs e)
            {
                angle += 4;
                if (angle >= 360) angle = 0;
                this.Invalidate();
            }

            protected override void OnPaint(PaintEventArgs e)
            {
                base.OnPaint(e);
                Graphics g = e.Graphics;

                // 禁用抗锯齿
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;

                // 禁用文字的抗锯齿
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SingleBitPerPixelGridFit;

                int centerX = ClientSize.Width / 2;
                int centerY = ClientSize.Height / 2;

                // 绘制轨迹
                for (int i = 0; i < 10; i++)
                {
                    float trailAngle = angle - (i * 40); // 每个轨迹点相隔 40 度
                    float x = centerX + CircleRadius * (float)Math.Cos(trailAngle * Math.PI / 180);
                    float y = centerY + CircleRadius * (float)Math.Sin(trailAngle * Math.PI / 180);

                    // 使用纯色填充
                    using (Brush trailBrush = new SolidBrush(Color.FromArgb(229, 104, 127))) // 橙色
                    {
                        g.FillEllipse(trailBrush, x - CircleSize / 2, y - CircleSize / 2, CircleSize, CircleSize);
                    }
                }

                // 绘制当前圆
                float currentX = centerX + CircleRadius * (float)Math.Cos(angle * Math.PI / 180);
                float currentY = centerY + CircleRadius * (float)Math.Sin(angle * Math.PI / 180);
                using (Brush customBrush = new SolidBrush(Color.FromArgb(229, 104, 127))) // 使用 RGB 颜色 (229, 104, 127)
                {
                    g.FillEllipse(customBrush, currentX - CircleSize / 2, currentY - CircleSize / 2, CircleSize, CircleSize);
                }

                // 绘制文字“正在加载中，请稍等”
                using (Font font = new Font("微软雅黑", 15, FontStyle.Bold))
                using (Brush textBrush = new SolidBrush(Color.FromArgb(255, 154, 50))) // 使用 RGB 颜色 (255, 154, 50)
                {
                    string loadingText = "正在加载中，请稍等...";
                    SizeF textSize = g.MeasureString(loadingText, font);
                    float textX = centerX - textSize.Width / 2;
                    float textY = centerY + CircleRadius + 20; // 文字在圆的下方

                    // 绘制文字背景（可选，确保背景色一致）
                    using (Brush backgroundBrush = new SolidBrush(this.BackColor))
                    {
                        g.FillRectangle(backgroundBrush, textX, textY, textSize.Width, textSize.Height);
                    }

                    // 绘制文字
                    g.DrawString(loadingText, font, textBrush, textX, textY);
                }
            }
        }
        public MaterialForm()
        {
            InitializeComponent();
            // 设置双缓冲以减少闪烁
            this.DoubleBuffered = true;
            // 初始化定时器
            animationTimer = new Timer();
            animationTimer.Interval = AnimationSpeed;
            animationTimer.Tick += Timer_Tick;
        }
        AutoResizeForm asc = new AutoResizeForm();
        /// <summary>s
        /// 点击选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        List<string> selectedFilePaths = new List<string>();
        //输入
        List<DataTable> inputFilePathData = new List<DataTable>();
        //输出
        List<DataTable> outputFilePathData = new List<DataTable>();
        List<DataTable> selectedFilePathData = new List<DataTable>();
        string selectedFolderPath;
        private void ChoosePathBtn_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog())
            {
                folderBrowserDialog.Description = "选择文件夹";
                // 设置默认打开的路径
                folderBrowserDialog.SelectedPath = Application.StartupPath + "\\data"; // 替换为您需要的默认路径

                DialogResult dialogResult = folderBrowserDialog.ShowDialog();

                if (dialogResult == DialogResult.OK)
                {
                    selectedFolderPath = folderBrowserDialog.SelectedPath;
                    selectedFilePathtextEdit.Text = selectedFolderPath;
                    DataSelectInfo.selectedFolderPath = selectedFolderPath;
                }
            }
            //using (FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog())
            //{
            //    folderBrowserDialog.Description = "选择文件夹";
            //    DialogResult dialogResult = folderBrowserDialog.ShowDialog();

            //    if (dialogResult == DialogResult.OK)
            //    {
            //        selectedFolderPath = folderBrowserDialog.SelectedPath;
            //        selectedFilePathtextEdit.Text = selectedFolderPath;
            //        DataSelectInfo.selectedFolderPath = selectedFolderPath;
            //    }
            //}
        }
        static DataTable TxtToDataTable(string filePath)
        {
            DataTable dataTable = new DataTable();
            // 读取文件的所有行
            string[] stringArray = File.ReadAllLines(filePath);
            string[] dataChartstexts = stringArray[0].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            // 假设第一行包含列标题
            string[] headers = stringArray[1].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            dataTable.Columns.Add(headers[0]);
            foreach (string header in dataChartstexts)
            {
                dataTable.Columns.Add(header);
            }
            for (int i = 2; i < stringArray.Length - 2; i++)
            {
                string[] items = stringArray[i].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                DataRow row = dataTable.NewRow();
                for (int j = 0; j < items.Length; j++)
                {
                    row[j] = items[j];
                }
                dataTable.Rows.Add(row);
            }
            return dataTable;
        }
        /// <summary>
        /// 调用Python内的pandas.read_pickle()方法打开Dataframe数据类型的文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private DataTable readpickle(string projectFolderPath)
        {
            DataTable dataTable = TxtToDataTable(projectFolderPath);
            return dataTable;
        }
        /// <summary>
        /// 温度
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        int temperaturecount = 0;
        private void temperatureckBox_CheckedChanged(object sender, EventArgs e)
        {
            string filepath1 = DataSelectInfo.selectedFolderPath + "\\temp";
            string filepath = string.Format("{0}", Application.StartupPath + "\\data\\temp.txt");
            temperaturecount++;
            //DataTable datatable = readpickle(filepath);
            if (temperaturecount % 2 == 1)
            {
                selectedFilePaths.Add(filepath);
                //inputFilePathData.Add(datatable);
                DataSelectInfo.outputs.Add(temperatureckBox.Text);
            }
            if (temperaturecount % 2 == 0)
            {
                selectedFilePaths.Remove(filepath);
                //selectedFilePathData.Remove(datatable);
                DataSelectInfo.outputs.Remove(temperatureckBox.Text);
            }
        }
        /// <summary>
        /// 速率
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        int speedcount = 0;
        private void speedckBox_CheckedChanged(object sender, EventArgs e)
        {
            string filepath1 = DataSelectInfo.selectedFolderPath + "\\rate";
            string filepath = string.Format("{0}", Application.StartupPath + "\\data\\rate.txt");
            speedcount++;
            //DataTable datatable = readpickle(filepath);
            if (speedcount % 2 == 1)
            {
                selectedFilePaths.Add(filepath);
                //inputFilePathData.Add(datatable);
                DataSelectInfo.outputs.Add(speedckBox.Text);
                speedBtn.Enabled = false;
            }
            if (speedcount % 2 == 0)
            {
                selectedFilePaths.Remove(filepath);
                //selectedFilePathData.Remove(datatable);
                speedBtn.Enabled = true;
                DataSelectInfo.outputs.Remove(speedckBox.Text);
            }
        }
        /// <summary>
        /// MO流量
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        int mocount = 0;
        private void mockBox_CheckedChanged(object sender, EventArgs e)
        {
            string filepath1 = DataSelectInfo.selectedFolderPath + "\\mo";
            string filepath = string.Format("{0}", Application.StartupPath + "\\data\\mo.txt");
            mocount++;
            //DataTable datatable = readpickle(filepath);
            if (mocount % 2 == 1)
            {
                selectedFilePaths.Add(filepath);
                //inputFilePathData.Add(datatable);
                DataSelectInfo.outputs.Add(mockBox.Text);
            }
            if (mocount % 2 == 0)
            {
                selectedFilePaths.Remove(filepath);
                //selectedFilePathData.Remove(datatable);
                DataSelectInfo.outputs.Remove(mockBox.Text);
            }
        }
        /// <summary>
        /// 掺杂
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        int dopingcount = 0;
        private void dopingckBox_CheckedChanged(object sender, EventArgs e)
        {
            string filepath1 = DataSelectInfo.selectedFolderPath + "\\dope";
            string filepath = string.Format("{0}", Application.StartupPath + "\\data\\dope.txt");
            dopingcount++;
            //DataTable datatable = readpickle(filepath);
            if (dopingcount % 2 == 1)
            {
                selectedFilePaths.Add(filepath);
                //inputFilePathData.Add(datatable);
                DataSelectInfo.outputs.Add(dopingckBox.Text);
            }
            if (dopingcount % 2 == 0)
            {
                selectedFilePaths.Remove(filepath);
                //selectedFilePathData.Remove(datatable);
                DataSelectInfo.outputs.Remove(dopingckBox.Text);
            }
        }
        /// <summary>
        /// huzhxxvc aax xzx xde
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void speedBtn_CheckedChanged(object sender, EventArgs e)
        {
            DataSelectInfo.input = speedBtn.Text;
        }
        /// <summary>
        /// 电性
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void electricalBtn_CheckedChanged(object sender, EventArgs e)
        {
            DataSelectInfo.input = electricalBtn.Text;
        }
        /// <summary>
        /// XRD
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void xrdBtn_CheckedChanged(object sender, EventArgs e)
        {
            DataSelectInfo.input = xrdBtn.Text;
        }
        /// <summary>
        /// PLq
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void plBtn_CheckedChanged(object sender, EventArgs e)
        {
            DataSelectInfo.input = plBtn.Text;
        }
        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static List<string> DataChartlist = new List<string>();
        private void roaddataBtn_Click(object sender, EventArgs e)
        {
            inputFilePathData = new List<DataTable>();
            outputFilePathData = new List<DataTable>();
            DataChartlist = new List<string>();
            if (selectedFilePathtextEdit.Text == null || selectedFilePathtextEdit.Text == "")
            {
                MessageBox.Show("请先选择目录！");
            }
            else
            {
                string filepath = "";
                // 清空ComboBox中的所有项
                DataCharts.Items.Clear();
                // 将SelectedIndex设置为-1，清空选中项显示
                DataCharts.SelectedIndex = -1;
                foreach (string outputsname in DataSelectInfo.outputs)
                {
                    if (outputsname == "温度")
                    {
                        filepath = string.Format("{0}", Application.StartupPath + "\\data\\temp.txt");
                        selectedFilePaths.Add(filepath);
                        DataTable datatable = readpickle(filepath);
                        inputFilePathData.Add(datatable);
                    }
                    if (outputsname == "速率")
                    {
                        filepath = string.Format("{0}", Application.StartupPath + "\\data\\rate.txt");
                        selectedFilePaths.Add(filepath);
                        DataTable datatable = readpickle(filepath);
                        inputFilePathData.Add(datatable);
                    }
                    if (outputsname == "MO流量")
                    {
                        filepath = string.Format("{0}", Application.StartupPath + "\\data\\mo.txt");
                        selectedFilePaths.Add(filepath);
                        DataTable datatable = readpickle(filepath);
                        inputFilePathData.Add(datatable);
                    }
                    if (outputsname == "掺杂")
                    {
                        filepath = string.Format("{0}", Application.StartupPath + "\\data\\dope.txt");
                        selectedFilePaths.Add(filepath);
                        DataTable datatable = readpickle(filepath);
                        inputFilePathData.Add(datatable);
                    }
                }
                if (DataSelectInfo.input == "速率")
                {
                    //filepath1 = DataSelectInfo.selectedFolderPath + "\\rate";
                    filepath = string.Format("{0}", Application.StartupPath + "\\data\\rate.txt");
                    selectedFilePaths.Add(filepath);
                    DataTable datatable = readpickle(filepath);
                    outputFilePathData.Add(datatable);
                }
                if (DataSelectInfo.input == "电性")
                {
                    //filepath = DataSelectInfo.selectedFolderPath + "\\elect";
                    filepath = string.Format("{0}", Application.StartupPath + "\\data\\elect.txt");

                    selectedFilePaths.Add(filepath);
                    DataTable datatable = readpickle(filepath);
                    outputFilePathData.Add(datatable);
                }
                if (DataSelectInfo.input == "XRD")
                {
                    //filepath = DataSelectInfo.selectedFolderPath + "\\xrd";
                    filepath = string.Format("{0}", Application.StartupPath + "\\data\\xrd.txt");

                    selectedFilePaths.Add(filepath);
                    DataTable datatable = readpickle(filepath);
                    outputFilePathData.Add(datatable);
                }
                if (DataSelectInfo.input == "PL")
                {
                    //filepath = DataSelectInfo.selectedFolderPath + "\\pl";
                    filepath = string.Format("{0}", Application.StartupPath + "\\data\\pl.txt");

                    selectedFilePaths.Add(filepath);
                    DataTable datatable = readpickle(filepath);
                    outputFilePathData.Add(datatable);
                }
                DataSelectInfo.inputsdatatable = inputFilePathData;
                DataSelectInfo.outputsdatatable = outputFilePathData;
                selectedFilePathData = inputFilePathData.Concat(outputFilePathData).ToList();
                DataSelectInfo.selectedDataTables = selectedFilePathData;
                DataTable selectePathData = new DataTable();
                for (int i = 0; i < selectedFilePathData.Count; i++)
                {
                    selectePathData = selectedFilePathData[i];
                    foreach (DataColumn column in selectePathData.Columns)
                    {
                        if (column.ColumnName != "ID")
                        {
                            DataCharts.Items.Add(column.ColumnName);
                            DataChartlist.Add(column.ColumnName);
                        }
                    }
                    DataCharts.SelectedIndex = 0;
                }
                DataSelectInfo.DataCharts = DataChartlist;
            }
        }
        /// <summary>
        /// 绘制图像
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DrawLineBtn_Click(object sender, EventArgs e)
        {
            if (DataCharts.Text == "" || DataCharts.Text == null)
            {
                MessageBox.Show("请选择需要数据绘图的数据！！！");

            }
            else
            {
                //pictureBox1.Visible = true;
                //pictureBox3.Visible = true;
                //pictureBox4.Visible = true;
                panel12.Controls.Clear();

                // 创建一个新的Chart控件
                Chart chart1 = new Chart();
                chart1.Dock = DockStyle.Fill;
                chart1.Palette = ChartColorPalette.EarthTones;
                chart1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(222)))), ((int)(((byte)(171)))));
                chart1.BorderlineColor = System.Drawing.Color.IndianRed;
                chart1.BorderSkin.BackColor = System.Drawing.Color.Transparent;
                chart1.BorderSkin.BackImageTransparentColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
                chart1.BorderSkin.BorderColor = System.Drawing.Color.Transparent;
                chart1.BorderSkin.PageColor = System.Drawing.Color.Transparent;
                //chart1.Visible = true;
                //pictureBox1.Visible = false;
                //chart1.Series.Clear(); // 清空所有系列（Series）

                string selectedText = DataCharts.SelectedItem.ToString();
                string id = "";
                DataTable newTable = null;
                selectedFilePathData = DataSelectInfo.selectedDataTables;
                // 使用for循环遍历List
                for (int i = 0; i < selectedFilePathData.Count; i++)
                {
                    DataTable tables = selectedFilePathData[i];
                    // 获取并打印表头
                    foreach (DataColumn column in tables.Columns)
                    {
                        if (selectedText == column.ColumnName)
                        {
                            id = tables.Columns[0].ColumnName.ToString();
                            newTable = CreateNewDataTable(tables, id, selectedText);

                            // 创建折线图
                            chart1.Series.Clear();
                            Series series = chart1.Series.Add("LineSeries");
                            series.ChartType = SeriesChartType.Line;

                            // 目标列索引（第二列，索引为 1）
                            int targetColumnIndex = 1;

                            // 遍历 DataTable 的每一行
                            foreach (DataRow row in newTable.Rows)
                            {
                                object cellValue = row[targetColumnIndex];
                                if (cellValue != DBNull.Value)
                                {
                                    string cellText = cellValue.ToString();
                                    if (double.TryParse(cellText, out _))
                                    {
                                        series.Points.AddXY(row[0], Convert.ToDouble(row[1]));
                                    }
                                }
                            }
                            // 配置X轴和Y轴
                            //chart1.ChartAreas[0].AxisX.Title = "炉次";
                            //chart1.ChartAreas[0].AxisY.Title = selectedText;
                            ChartArea chartArea = new ChartArea("ChartArea1");

                            // 设置横坐标标题
                            chartArea.AxisX.Title = "炉次";
                            // 设置纵坐标标题
                            chartArea.AxisY.Title = selectedText;
                            chart1.ChartAreas.Add(chartArea);
                            // 创建一个Series并添加到Chart中
                            //chart1.Series.Add(series);
                            chartArea.BackColor = System.Drawing.Color.SteelBlue;
                            chartArea.BorderColor = System.Drawing.Color.White;
                            // 将Chart控件添加到窗体中
                            panel12.Controls.Add(chart1);
                            return;
                        }
                    }
                }
            }

        }
        public static DataTable CreateNewDataTable(DataTable originalTable, string column1Name, string column2Name)
        {
            DataTable newTable = new DataTable();
            // 添加新列
            newTable.Columns.Add(column1Name, originalTable.Columns[column1Name].DataType);
            //newTable.Columns.Add(column1Name, originalTable.Columns[column1Name].DataType);
            newTable.Columns.Add(column2Name, originalTable.Columns[column2Name].DataType);
            // 复制数据
            foreach (DataRow originalRow in originalTable.Rows)
            {
                DataRow newRow = newTable.NewRow();
                newRow[column1Name] = originalRow[column1Name];
                newRow[column2Name] = originalRow[column2Name];
                newTable.Rows.Add(newRow);
            }
            return newTable;
        }
        private void MaterialForm_Load(object sender, EventArgs e)
        {
            Timetxt.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            asc.controllInitializeSize(this);
            pictureBox1.Visible = true;
            pictureBox3.Visible = true;
            pictureBox4.Visible = true;


            selectedStackingModel0 = new List<string>();
            Predictiondatatables = new List<DataTable>();
            trainingscores = new List<string>();
            selectedFilePaths = new List<string>();
            inputFilePathData = new List<DataTable>();
            outputFilePathData = new List<DataTable>();
            selectedFilePathData = new List<DataTable>();
            DataChartlist = new List<string>();
            DataSelectInfo.outputs = new List<string>();
            DataSelectInfo.inputsdatatable = new List<DataTable>();
            DataSelectInfo.outputsdatatable = new List<DataTable>();
            DataSelectInfo.DataCharts = new List<string>();
            //chart1.Visible = false;
            //pictureBox1.Visible = true;
            //panel11.Controls.Clear();
            //// 创建 PictureBox 控件实例
            //PictureBox pictureBox1 = new PictureBox();

            //// 设置 PictureBox 的位置
            ////pictureBox1.Location = new Point(3, 65);

            //// 设置 PictureBox 的大小
            //pictureBox1.Size = new Size(601, 473);
            //// 设置图片显示模式，这里使用 Zoom 模式，图片会按比例缩放以适应 PictureBox 大小
            //pictureBox1.BackgroundImageLayout = ImageLayout.Stretch;
            //pictureBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(222)))), ((int)(((byte)(171)))));
            //pictureBox1.BackgroundImage = global::WisdomGrowth.Properties.Resources.neuralnetwork;
            //// 将 PictureBox 控件添加到当前窗体的控件集合中
            //panel11.Controls.Add(pictureBox1);
        }

        private void MaterialForm_SizeChanged(object sender, EventArgs e)
        {
            asc.controlAutoSize(this);
        }
        /// <summary>
        /// 禁用按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Stopbtn()
        {
            BPNNModel0Box.Enabled = false;
            SVRModel0Box.Enabled = false;
            GBRModel0Box.Enabled = false;
            RFRFModel0Box.Enabled = false;
            BPNNModel1Btn.Enabled = false;
            SVRModel1Btn.Enabled = false;
            GBRModel1Btn.Enabled = false;
            RFRFModel1Btn.Enabled = false;
        }
        /// <summary>
        /// 选择模型BPNN
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        int BPNNcount = 0;
        private void BPNNModelBtn_CheckedChanged(object sender, EventArgs e)
        {
            Stopbtn();
            BPNNcount++;
            if (BPNNcount % 2 == 1)
            {
                BPNNForm ds = new BPNNForm();
                // 显示子窗体
                ds.Show();
                ModelTrainingInfo.selectmodel = BPNNModelBtn.Text;
            }
        }
        /// <summary>
        /// 选择模型SVR
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        int SVRcount = 0;
        private void SVRModelBtn_CheckedChanged(object sender, EventArgs e)
        {
            Stopbtn();
            SVRcount++;
            if (SVRcount % 2 == 1)
            {
                SVRForm ds = new SVRForm();
                // 显示子窗体
                ds.Show();
                ModelTrainingInfo.selectmodel = SVRModelBtn.Text;
            }
        }
        /// <summary>
        /// 选择模型GBR
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        int GBRcount = 0;
        private void GBRModelBtn_CheckedChanged(object sender, EventArgs e)
        {
            Stopbtn();
            GBRcount++;
            if (GBRcount % 2 == 1)
            {
                GBRForm ds = new GBRForm();
                // 显示子窗体
                ds.Show();
                ModelTrainingInfo.selectmodel = GBRModelBtn.Text;
            }
        }
        /// <summary>
        /// 选择模型RFR
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        int RFRcount = 0;
        private void RFRModelBtn_CheckedChanged(object sender, EventArgs e)
        {
            Stopbtn();
            RFRcount++;
            if (RFRcount % 2 == 1)
            {
                RFRForm ds = new RFRForm();
                // 显示子窗体
                ds.Show();
                ModelTrainingInfo.selectmodel = RFRModelBtn.Text;
            }
        }
        List<string> selectedStackingModel0 = new List<string>();
        /// <summary>
        /// 选择模型Stacking
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        int Stackingcount = 0;
        private void StackingModelBtn_CheckedChanged(object sender, EventArgs e)
        {
            // 获取触发事件的单选框
            //RadioButton selectedRadioButton = (RadioButton)sender;


            Stackingcount++;
            if (Stackingcount % 2 == 1)
            {
                ModelTrainingInfo.selectmodel = StackingModelBtn.Text;
                BPNNModel0Box.Enabled = true;
                SVRModel0Box.Enabled = true;
                GBRModel0Box.Enabled = true;
                RFRFModel0Box.Enabled = true;
                BPNNModel1Btn.Enabled = true;
                SVRModel1Btn.Enabled = true;
                GBRModel1Btn.Enabled = true;
                RFRFModel1Btn.Enabled = true;
            }

            //if (Stackingcount % 2 == 0 && (selectedRadioButton.Name == "BPNNModelBtn" || selectedRadioButton.Name == "SVRModelBtn" || selectedRadioButton.Name == "GBRModelBtn" || selectedRadioButton.Name == "RFRModelBtn"))
            //{
            //    //Stopbtn();
            //    //BPNNModel0Box.Checked = false;
            //    //SVRModel0Box.Checked = false;
            //    //GBRModel0Box.Checked = false;
            //    //RFRFModel0Box.Checked = false;
            //    //BPNNModel1Btn.Checked = false;
            //    //SVRModel1Btn.Checked = false;
            //    //GBRModel1Btn.Checked = false;
            //    //RFRFModel1Btn.Checked = false;

            //}

        }

        /// <summary>
        /// Stacking模型配置Model0BPNN
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        int BPNNModel0count = 0;
        private void BPNNModel0Box_CheckedChanged(object sender, EventArgs e)
        {
            BPNNModel0count++;
            if (BPNNModel0count % 2 == 1)
            {
                BPNNForm ds = new BPNNForm();
                // 显示子窗体
                ds.Show();
                selectedStackingModel0.Add(BPNNModel0Box.Text);
            }
            if (BPNNModel0count % 2 == 0)
            {
                selectedStackingModel0.Remove(BPNNModel0Box.Text);
            }
        }
        /// <summary>
        /// Stacking模型配置Model0SVR
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        int SVRModel0count = 0;
        private void SVRModel0Box_CheckedChanged(object sender, EventArgs e)
        {
            SVRModel0count++;
            if (SVRModel0count % 2 == 1)
            {
                SVRForm ds = new SVRForm();
                // 显示子窗体
                ds.Show();
                selectedStackingModel0.Add(SVRModel0Box.Text);
            }
            if (SVRModel0count % 2 == 0)
            {
                selectedStackingModel0.Remove(SVRModel0Box.Text);
            }
        }
        /// <summary>
        /// Stacking模型配置Model0GBR
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        int GBRModel0count = 0;
        private void GBRModel0Box_CheckedChanged(object sender, EventArgs e)
        {
            GBRModel0count++;
            if (GBRModel0count % 2 == 1)
            {
                GBRForm ds = new GBRForm();
                // 显示子窗体
                ds.Show();
                selectedStackingModel0.Add(GBRModel0Box.Text);
            }
            if (GBRModel0count % 2 == 0)
            {
                selectedStackingModel0.Remove(GBRModel0Box.Text);
            }
        }
        /// <summary>
        /// Stacking模型配置Model0RFR
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        int RFRModel0count = 0;
        private void RFRFModel0Box_CheckedChanged(object sender, EventArgs e)
        {
            RFRModel0count++;
            if (RFRModel0count % 2 == 1)
            {
                RFRForm ds = new RFRForm();
                // 显示子窗体
                ds.Show();
                selectedStackingModel0.Add(RFRFModel0Box.Text);
            }
            if (RFRModel0count % 2 == 0)
            {
                selectedStackingModel0.Remove(RFRFModel0Box.Text);
            }
        }
        /// <summary>
        /// Stacking模型配置Model1BPNN
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        int BPNNModel1count = 0;
        private void BPNNModel1Btn_CheckedChanged(object sender, EventArgs e)
        {
            BPNNModel1count++;
            if (BPNNModel1count % 2 == 1)
            {
                BPNNForm ds = new BPNNForm();
                // 显示子窗体
                ds.Show();
                ModelTrainingInfo.selectstackingmodel1 = BPNNModel1Btn.Text;
                selectedStackingModel0.Add(BPNNModel1Btn.Text);
            }
            else
            {
                selectedStackingModel0.Remove(BPNNModel1Btn.Text);

            }
        }
        /// <summary>
        /// Stacking模型配置Model1SVR
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        int SVRModel1count = 0;
        private void SVRModel1Btn_CheckedChanged(object sender, EventArgs e)
        {
            SVRModel1count++;
            if (SVRModel1count % 2 == 1)
            {
                SVRForm ds = new SVRForm();
                // 显示子窗体
                ds.Show();
                ModelTrainingInfo.selectstackingmodel1 = SVRModel1Btn.Text;
                selectedStackingModel0.Add(SVRModel1Btn.Text);

            }
            else
            {
                selectedStackingModel0.Remove(SVRModel1Btn.Text);

            }
        }
        /// <summary>
        /// Stacking模型配置Model1GBR
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        int GBRModel1count = 0;
        private void GBRModel1Btn_CheckedChanged(object sender, EventArgs e)
        {
            GBRModel1count++;
            if (GBRModel1count % 2 == 1)
            {
                GBRForm ds = new GBRForm();
                // 显示子窗体
                ds.Show();
                ModelTrainingInfo.selectstackingmodel1 = GBRModel1Btn.Text;
                selectedStackingModel0.Add(GBRModel1Btn.Text);

            }
            else
            {
                selectedStackingModel0.Remove(GBRModel1Btn.Text);

            }
        }
        /// <summary>
        /// Stacking模型配置Model1RFR
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        int RFRFModel1count = 0;
        private void RFRFModel1Btn_CheckedChanged(object sender, EventArgs e)
        {
            RFRFModel1count++;
            if (RFRFModel1count % 2 == 1)
            {
                RFRForm ds = new RFRForm();
                // 显示子窗体
                ds.Show();
                ModelTrainingInfo.selectstackingmodel1 = RFRFModel1Btn.Text;
                selectedStackingModel0.Add(RFRFModel1Btn.Text);

            }
            else
            {
                selectedStackingModel0.Remove(RFRFModel1Btn.Text);

            }
        }
        // 定义常量
        private const int BPNN_MAX_EPOCHS = 10000;
        private const double BPNN_ERROR_THRESHOLD = 1e-5;
        private const int GBR_NUMBER_OF_MODELS = 100;
        private const double GBR_LEARNING_RATE = 0.1;
        private const int RFR_NUMBER_OF_TREES = 100;
        private DataTable CreateSampleDataTable()
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("Input1", typeof(double));
            dataTable.Columns.Add("Input2", typeof(double));
            dataTable.Columns.Add("Output", typeof(double));

            dataTable.Rows.Add(0, 0, 0);
            dataTable.Rows.Add(1, 0, 1);
            dataTable.Rows.Add(0, 1, 1);
            dataTable.Rows.Add(1, 1, 0);

            return dataTable;
        }

        private void ConvertDataTableToArrays(DataTable dataTable, out double[][] inputs, out double[] outputs)
        {
            int numRows = dataTable.Rows.Count;
            int numInputs = dataTable.Columns.Count - 1;

            inputs = new double[numRows][];
            outputs = new double[numRows];

            for (int i = 0; i < numRows; i++)
            {
                DataRow row = dataTable.Rows[i];
                inputs[i] = new double[numInputs];
                for (int j = 0; j < numInputs; j++)
                {
                    inputs[i][j] = Convert.ToDouble(row[j]);
                }
                outputs[i] = Convert.ToDouble(row[numInputs]);
            }
        }

        static ActivationNetwork TrainBPNN(double[][] inputs, double[] outputs, out double score)
        {
            double[][] outputs2D = outputs.Select(o => new double[] { o }).ToArray();
            var network = new ActivationNetwork(new SigmoidFunction(), inputs[0].Length, 2, 1);
            var teacher = new BackPropagationLearning(network);

            double error = double.PositiveInfinity;
            int epochs = 0;
            while (error > BPNN_ERROR_THRESHOLD && epochs < BPNN_MAX_EPOCHS)
            {
                error = teacher.RunEpoch(inputs, outputs2D);
                epochs++;
            }

            // 计算训练分数（均方误差）
            var predictions = PredictBPNN(network, inputs);
            score = new SquareLoss(outputs).Loss(predictions);

            return network;
        }

        static double[] PredictBPNN(ActivationNetwork network, double[][] inputs)
        {
            return inputs.Select(input => network.Compute(input)[0]).ToArray();
        }

        static MultipleLinearRegression TrainLinearRegression(double[][] inputs, double[] outputs, out double score)
        {
            // 创建线性回归学习器
            var ols = new OrdinaryLeastSquares();

            // 训练线性回归模型
            var model = ols.Learn(inputs, outputs);

            // 进行预测
            double[] predictions = model.Transform(inputs);

            // 计算训练分数（均方误差）
            score = new SquareLoss(outputs).Loss(predictions);

            return model;
        }

        static double[] PredictLinearRegression(MultipleLinearRegression model, double[][] inputs)
        {
            return model.Transform(inputs);
        }

        static MultipleLinearRegression[] TrainGBR(double[][] inputs, double[] outputs, out double score)
        {
            const int numberOfModels = GBR_NUMBER_OF_MODELS;
            const double learningRate = GBR_LEARNING_RATE;

            // 初始化残差为原始输出
            double[] residuals = outputs.Copy();

            // 存储每一个线性回归模型
            MultipleLinearRegression[] models = new MultipleLinearRegression[numberOfModels];

            for (int i = 0; i < numberOfModels; i++)
            {
                // 创建线性回归学习器
                var ols = new OrdinaryLeastSquares();

                // 训练线性回归模型
                models[i] = ols.Learn(inputs, residuals);

                // 预测当前模型的结果
                double[] modelPredictions = models[i].Transform(inputs);

                // 更新残差
                for (int j = 0; j < residuals.Length; j++)
                {
                    residuals[j] -= learningRate * modelPredictions[j];
                }
            }

            // 计算最终预测结果
            double[] predictions = new double[inputs.Length];
            for (int i = 0; i < inputs.Length; i++)
            {
                for (int j = 0; j < numberOfModels; j++)
                {
                    predictions[i] += learningRate * models[j].Transform(new double[][] { inputs[i] })[0];
                }
            }

            // 计算训练分数（均方误差）
            score = new SquareLoss(outputs).Loss(predictions);

            return models;
        }

        static double[] PredictGBR(MultipleLinearRegression[] models, double[][] inputs)
        {
            const double learningRate = GBR_LEARNING_RATE;
            double[] predictions = new double[inputs.Length];

            for (int i = 0; i < inputs.Length; i++)
            {
                for (int j = 0; j < models.Length; j++)
                {
                    predictions[i] += learningRate * models[j].Transform(new double[][] { inputs[i] })[0];
                }
            }

            return predictions;
        }

        static RandomForest TrainRFR(double[][] inputs, int[] outputs, out double score)
        {
            var teacher = new RandomForestLearning()
            {
                NumberOfTrees = RFR_NUMBER_OF_TREES
            };

            var rfr = teacher.Learn(inputs, outputs);

            // 计算训练分数（准确率）
            var predictions = PredictRFR(rfr, inputs);
            score = 1 - new ZeroOneLoss(outputs).Loss(predictions.Select(p => (int)p).ToArray());

            return rfr;
        }

        static double[] PredictRFR(RandomForest forest, double[][] inputs)
        {
            return inputs.Select(input => (double)forest.Decide(input)).ToArray();
        }

        static double[] EnsemblePredictions(double[] bpnn, double[] linearRegression, double[] gbr, double[] rfr)
        {
            double[] weights = { 0.25, 0.25, 0.25, 0.25 }; // 初始权重
            return bpnn.Select((b, i) => b * weights[0] + linearRegression[i] * weights[1] + gbr[i] * weights[2] + rfr[i] * weights[3])
                       .ToArray();
        }
        public DataTable RemoveFirstColumn(DataTable originalTable)
        {
            if (originalTable == null || originalTable.Columns.Count == 0)
                return null; // 或者可以选择返回一个空的DataTable

            // 克隆表结构，但不包括第一列
            DataTable newTable = originalTable.Clone();
            newTable.Columns.RemoveAt(0); // 移除克隆表的第一个列

            // 复制行到新表，但不包括第一列的数据
            foreach (DataRow row in originalTable.Rows)
            {
                DataRow newRow = newTable.NewRow();
                for (int i = 1; i < originalTable.Columns.Count; i++) // 从第二列开始复制
                {
                    newRow[i - 1] = row[i]; // 因为已经移除了第一列，索引需要调整
                }
                newTable.Rows.Add(newRow);
            }

            return newTable;
        }
        private (DataTable input, DataTable output) PrepareDataTables(DataTable inputDataTable, DataTable outputDataTable)
        {
            return (inputDataTable, outputDataTable);
        }
        private (double[][], double[][]) ConvertDataTablesToArrays(DataTable inputDataTable, DataTable outputDataTable)
        {
            double[][] inputs = inputDataTable.ToJagged<double>();
            double[][] outputs = outputDataTable.ToJagged<double>();

            return (inputs, outputs);
        }

        // 计算均方误差 (MSE)
        private double CalculateMSE(double[][] actual, double[][] predicted)
        {
            double sum = 0;
            int totalElements = 0;
            for (int i = 0; i < actual.Length; i++)
            {
                for (int j = 0; j < actual[i].Length; j++)
                {
                    sum += Math.Pow(actual[i][j] - predicted[i][j], 2);
                    totalElements++;
                }
            }
            return sum / totalElements;
        }

        // 计算决定系数 (R^2)
        private double CalculateR2(double[][] actual, double[][] predicted)
        {
            double totalSum = 0;
            double residualSum = 0;
            double overallMean = CalculateOverallMean(actual);
            int totalElements = 0;

            for (int i = 0; i < actual.Length; i++)
            {
                for (int j = 0; j < actual[i].Length; j++)
                {
                    totalSum += Math.Pow(actual[i][j] - overallMean, 2);
                    residualSum += Math.Pow(actual[i][j] - predicted[i][j], 2);
                    totalElements++;
                }
            }

            if (totalSum == 0)
            {
                return 1; // 避免除零错误
            }

            return 1 - (residualSum / totalSum);
        }

        // 计算所有实际值的平均值
        private double CalculateOverallMean(double[][] actual)
        {
            double sum = 0;
            int totalElements = 0;
            for (int i = 0; i < actual.Length; i++)
            {
                for (int j = 0; j < actual[i].Length; j++)
                {
                    sum += actual[i][j];
                    totalElements++;
                }
            }
            return sum / totalElements;
        }
        DataTable Predictiondatatable = new DataTable();
        string trainingscore = "";
        List<DataTable> Predictiondatatables = new List<DataTable>();
        List<string> trainingscores = new List<string>();
        private void PerformSomeOperation()
        {
            if (ModelTrainingInfo.selectmodel == "BPNN")
            {
                Predictiondatatable = ModelTrainingInfo.outputsdatatable;
                trainingscore = ModelTrainingInfo.trainingscore;
            }
            if (ModelTrainingInfo.selectmodel == "SVR")
            {
                Predictiondatatable = ModelTrainingInfo.outputsdatatable;
                trainingscore = ModelTrainingInfo.trainingscore;
            }
            if (ModelTrainingInfo.selectmodel == "GBR")
            {
                Predictiondatatable = ModelTrainingInfo.outputsdatatable;
                trainingscore = ModelTrainingInfo.trainingscore;
            }
            if (ModelTrainingInfo.selectmodel == "RFR")
            {
                Predictiondatatable = ModelTrainingInfo.outputsdatatable;
                trainingscore = ModelTrainingInfo.trainingscore;
            }
            if (ModelTrainingInfo.selectmodel == "Stacking")
            {
                foreach (string selectedstackingmodel in selectedStackingModel0)
                {
                    if (selectedstackingmodel == "BPNN")
                    {
                        Predictiondatatables.Add(ModelTrainingInfo.outputsdatatable);
                        trainingscores.Add(ModelTrainingInfo.trainingscore);
                    }
                    if (selectedstackingmodel == "SVR")
                    {
                        Predictiondatatables.Add(ModelTrainingInfo.outputsdatatable);
                        trainingscores.Add(ModelTrainingInfo.trainingscore);
                    }
                    if (selectedstackingmodel == "GBR")
                    {
                        Predictiondatatables.Add(ModelTrainingInfo.outputsdatatable);
                        trainingscores.Add(ModelTrainingInfo.trainingscore);
                    }
                    if (selectedstackingmodel == "RFR")
                    {
                        Predictiondatatables.Add(ModelTrainingInfo.outputsdatatable);
                        trainingscores.Add(ModelTrainingInfo.trainingscore);
                    }
                }
                // 合并 DataTable
                Predictiondatatable = MergeDataTablesWithDifferentSchema(Predictiondatatables);
                List<double> trainingScores1 = trainingscores.Select(s => double.Parse(s)).ToList();
                trainingscore = trainingScores1.Average().ToString();
                selectedStackingModel0 = new List<string>();
            }
            //System.Threading.Thread.Sleep(500); // 模拟一个耗时操作
        }
        // 合并 DataTable
        static DataTable MergeDataTablesWithDifferentSchema(List<DataTable> dataTables)
        {
            if (dataTables == null || dataTables.Count == 0)
                throw new ArgumentException("DataTable 列表不能为空");

            // 收集所有列
            HashSet<string> allColumns = new HashSet<string>();
            foreach (var dataTable in dataTables)
            {
                foreach (DataColumn column in dataTable.Columns)
                {
                    allColumns.Add(column.ColumnName);
                }
            }

            // 创建新的 DataTable，包含所有列
            DataTable resultTable = new DataTable();
            foreach (var columnName in allColumns)
            {
                resultTable.Columns.Add(columnName, typeof(double)); // 假设所有列都是 double 类型
            }

            // 合并数据
            foreach (var dataTable in dataTables)
            {
                foreach (DataRow row in dataTable.Rows)
                {
                    DataRow newRow = resultTable.NewRow();

                    // 复制数据
                    foreach (DataColumn column in dataTable.Columns)
                    {
                        newRow[column.ColumnName] = row[column];
                    }

                    // 处理缺失的列（用默认值填充）
                    foreach (DataColumn column in resultTable.Columns)
                    {
                        if (!dataTable.Columns.Contains(column.ColumnName))
                        {
                            newRow[column.ColumnName] = 0.0; // 默认值
                        }
                    }

                    resultTable.Rows.Add(newRow);
                }
            }

            return resultTable;
        }
        static bool CheckFirstColumnName(DataTable dataTable)
        {
            // 检查DataTable是否有列
            if (dataTable.Columns.Count > 0)
            {
                // 获取第一列的名称
                string firstColumnName = dataTable.Columns[0].ColumnName;
                // 比较列名是否为id（不区分大小写）
                return firstColumnName.Equals("ID", StringComparison.OrdinalIgnoreCase);
            }
            return false;
        }
        /// <summary>   
        /// 开始训练
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void TrainingBtn_Click(object sender, EventArgs e)
        {
            if (isLoading) return;
            isLoading = true;

            // 创建并显示透明窗体
            LoadingOverlayForm loadingOverlay = new LoadingOverlayForm(this);
            loadingOverlay.Show();

            btnToggleControls(false);
            try
            {
                if (ResultInfo.bpnnelapsedSeconds == 0 && ResultInfo.svrelapsedSeconds == 0 && ResultInfo.gbrelapsedSeconds == 0 && ResultInfo.rfrelapsedSeconds == 0)
                {
                    //return;//hide by wpz 2025/9/26
                }
                if (DataSelectInfo.inputsdatatable.Count > 0 && DataSelectInfo.outputsdatatable.Count > 0)
                {
                    // 创建一个新的Stopwatch实例
                    Stopwatch stopwatch = new Stopwatch();
                    // 开始计时
                    stopwatch.Start();

                    #region add by wpz 2025/9/27
                    string outPath = Path.Combine(Application.StartupPath, "File", "ksxl.txt");
                    string modelFileName = string.Format("{0}", Application.StartupPath + "\\Model\\model");
                    string pythonPath = Path.Combine(Application.StartupPath, "File", "ksxl.py");
                    string inputPath = string.Format("{0}", Application.StartupPath + "\\File\\input-ms-2022-3-14.csv");
                    string outputPath = string.Format("{0}", Application.StartupPath + "\\File\\output-2022-3-14.csv");
                    string intputStr = "";
                    string outputStr = "";
                    string trainStr = "";
                    string otherStr = "";
                    double iqrMultiplier = double.Parse(thresholdTxt.Text.ToString());
                    StringBuilder selectStr = new StringBuilder();
                    string filePath = DataSelectInfo.selectedFolderPath;
                    if (temperatureckBox.Checked)
                    {
                        intputStr = temperatureckBox.Text.ToString();
                    }
                    if (speedckBox.Checked)
                    {
                        intputStr = speedckBox.Text.ToString();
                    }
                    if (mockBox.Checked)
                    {
                        intputStr = mockBox.Text.ToString();
                    }
                    if (dopingckBox.Checked)
                    {
                        intputStr = dopingckBox.Text.ToString();
                    }
                    if (speedBtn.Checked)
                    {
                        outputStr = speedBtn.Text.ToString();
                    }
                    if (electricalBtn.Checked)
                    {
                        outputStr = electricalBtn.Text.ToString();
                    }
                    if (xrdBtn.Checked)
                    {
                        outputStr = xrdBtn.Text.ToString();
                    }
                    if (plBtn.Checked)
                    {
                        outputStr = plBtn.Text.ToString();
                    }
                    if (BPNNModelBtn.Checked)
                    {
                        trainStr = BPNNModelBtn.Text.ToString();
                        selectStr.Append(ModelTrainingInfo.BPNNHiddennode.ToString()).Append(',').Append(ModelTrainingInfo.BPNNPenaltyfactor.ToString()).Append(',').Append(ModelTrainingInfo.BPNNMaxIterations.ToString()).Append(',').Append(ModelTrainingInfo.BPNNGradientDescent.ToString());

                    }
                    if (SVRModelBtn.Checked)
                    {
                        trainStr = SVRModelBtn.Text.ToString();
                        selectStr.Append(ModelTrainingInfo.SVRkernelFunction.ToString()).Append(',').Append(ModelTrainingInfo.SVRtolerance.ToString()).Append(',').Append(ModelTrainingInfo.SVRregularizationparameter.ToString()).Append(',').Append(ModelTrainingInfo.SVRMaxIterations);
                    }
                    if (GBRModelBtn.Checked)
                    {
                        trainStr = GBRModelBtn.Text.ToString();
                        selectStr.Append(ModelTrainingInfo.GBRLossfunction.ToString()).Append(',').Append(ModelTrainingInfo.GBRLearningrate.ToString()).Append(',').Append(ModelTrainingInfo.GBRNumberoflearners.ToString()).Append(',').Append(ModelTrainingInfo.GBRMinimumpartitionnode.ToString()).Append(',').Append(ModelTrainingInfo.GBRMaximumdepth.ToString());
                    }
                    if (RFRModelBtn.Checked)
                    {
                        trainStr = RFRModelBtn.Text.ToString();
                        selectStr.Append(ModelTrainingInfo.RFRNumberoflearners.ToString()).Append(',').Append(ModelTrainingInfo.RFRMinimumpartitionnode.ToString()).Append(',').Append(ModelTrainingInfo.RFRMaximumdepth.ToString());

                    }
                    if (StackingModelBtn.Checked)
                    {
                        trainStr = StackingModelBtn.Text.ToString();
                        selectStr.Append(BPNNModel0Box.Checked.ToString()).Append(';').Append(ModelTrainingInfo.BPNNHiddennode.ToString()).Append(';').Append(ModelTrainingInfo.BPNNPenaltyfactor.ToString()).Append(';').Append(ModelTrainingInfo.BPNNMaxIterations.ToString()).Append(';').Append(ModelTrainingInfo.BPNNGradientDescent.ToString()).Append(',').Append(SVRModel0Box.Checked.ToString()).Append(';').Append(ModelTrainingInfo.SVRkernelFunction.ToString()).Append(';').Append(ModelTrainingInfo.SVRtolerance.ToString()).Append(';').Append(ModelTrainingInfo.SVRregularizationparameter.ToString()).Append(';').Append(ModelTrainingInfo.SVRMaxIterations).Append(',').Append(GBRModel0Box.Checked.ToString()).Append(';').Append(ModelTrainingInfo.GBRLossfunction.ToString()).Append(';').Append(ModelTrainingInfo.GBRLearningrate.ToString()).Append(';').Append(ModelTrainingInfo.GBRNumberoflearners.ToString()).Append(';').Append(ModelTrainingInfo.GBRMinimumpartitionnode.ToString()).Append(';').Append(ModelTrainingInfo.GBRMaximumdepth.ToString()).Append(',').Append(RFRFModel0Box.Checked.ToString()).Append(';').Append(ModelTrainingInfo.RFRNumberoflearners.ToString()).Append(';').Append(ModelTrainingInfo.RFRMinimumpartitionnode.ToString()).Append(';').Append(ModelTrainingInfo.RFRMaximumdepth.ToString()).Append(',').Append(BPNNModel1Btn.Checked.ToString()).Append(';').Append(ModelTrainingInfo.BPNNHiddennode.ToString()).Append(';').Append(ModelTrainingInfo.BPNNPenaltyfactor.ToString()).Append(';').Append(ModelTrainingInfo.BPNNMaxIterations.ToString()).Append(';').Append(ModelTrainingInfo.BPNNGradientDescent.ToString()).Append(',').Append(SVRModel1Btn.Checked.ToString()).Append(';').Append(ModelTrainingInfo.SVRkernelFunction.ToString()).Append(';').Append(ModelTrainingInfo.SVRtolerance.ToString()).Append(';').Append(ModelTrainingInfo.SVRregularizationparameter.ToString()).Append(';').Append(ModelTrainingInfo.SVRMaxIterations).Append(',').Append(GBRModel1Btn.Checked.ToString()).Append(';').Append(ModelTrainingInfo.GBRLossfunction.ToString()).Append(';').Append(ModelTrainingInfo.GBRLearningrate.ToString()).Append(';').Append(ModelTrainingInfo.GBRNumberoflearners.ToString()).Append(';').Append(ModelTrainingInfo.GBRMinimumpartitionnode.ToString()).Append(';').Append(ModelTrainingInfo.GBRMaximumdepth.ToString()).Append(',').Append(RFRFModel1Btn.Checked.ToString()).Append(';').Append(ModelTrainingInfo.RFRNumberoflearners.ToString()).Append(';').Append(ModelTrainingInfo.RFRMinimumpartitionnode.ToString()).Append(';').Append(ModelTrainingInfo.RFRMaximumdepth.ToString());
                    }
                    string[] strArr = new string[8];
                    strArr[0] = intputStr;
                    strArr[1] = outputStr;
                    strArr[2] = trainStr;
                    strArr[3] = "123";
                    strArr[4] = filePath;
                    strArr[5] = selectStr.ToString();
                    strArr[6] = outPath.ToString();
                    strArr[7] = iqrMultiplier.ToString();
                    string sArguments = pythonPath + " " + strArr[0] + " " + strArr[1] + " " + strArr[2] + " " + strArr[3] + " " + strArr[4] + " " + strArr[5] + " " + strArr[6] + " " + strArr[7];
                    #region test by wpz 
                    string fullName = Path.Combine(Application.StartupPath, "File", "ksxltest.txt");
                    using (FileStream fs = new FileStream(fullName, FileMode.Create, FileAccess.Write))
                    {
                        StreamWriter sw = new StreamWriter(fs, Encoding.Default);
                        sw.Flush();
                        sw.Write(sArguments.ToString());
                        sw.Flush();
                        sw.Close();
                    }
                    #endregion
                    ProcessStartInfo start = new ProcessStartInfo();
                    start.FileName = @"python.exe";
                    start.Arguments = sArguments;//参数以空格分隔，如果某个参数为空，可以传入
                    start.UseShellExecute = false; //必需
                    start.RedirectStandardOutput = true;//输出参数设定
                    start.RedirectStandardInput = true;//传入参数设定
                    start.RedirectStandardError = true;
                    start.CreateNoWindow = true;
                    using (Process pc = Process.Start(start))
                    {
                        pc.BeginOutputReadLine();
                        pc.BeginErrorReadLine();
                        pc.WaitForExit();
                    }
                    double elapsedSeconds = 0d;

                    if (File.Exists(outPath))//取值
                    {
                        string strCurrent = "";
                        using (BufferedStream bufferedStream = new BufferedStream(new FileStream(outPath, FileMode.Open, FileAccess.Read), 4096000))
                        {
                            using (StreamReader streamReader = new StreamReader(bufferedStream))
                            {
                                while ((strCurrent = streamReader.ReadLine()) != null)
                                {
                                    elapsedSeconds = Convert.ToDouble(strCurrent.ToString());
                                    strCurrent = streamReader.ReadLine();
                                    trainingscore = strCurrent.ToString();
                                    break;
                                }
                                textEdit4.Text = string.Format("{0}", elapsedSeconds);
                                textEdit2.Text = string.Format("{0}", trainingscore);
                            }
                        }

                        #region add 2025/10/20
                        NumericHelper.DeleteNumericPredictionToDb();


                        List<DataTable> dataTables = DataSelectInfo.inputsdatatable;
                        DataTable resulttables = new DataTable();
                        // 使用for循环遍历List
                        for (int i = 0; i < dataTables.Count; i++)
                        {
                            DataTable tables = dataTables[i];
                            DataTable inputresulttables = new DataTable();
                            inputresulttables = ConvertColumnsToDouble(tables);
                            // 调用方法判断第一列名是否为id
                            bool isFirstColumnNameId = CheckFirstColumnName(inputresulttables);
                            if (isFirstColumnNameId)
                            {
                                if (i == 0)
                                {
                                    resulttables = RemoveFirstColumn(inputresulttables);
                                }
                                else
                                {
                                    resulttables = MergeByMinRows(resulttables, inputresulttables);
                                }
                            }
                        }
                        if (DataSelectInfo.input != "速率")
                        {
                            List<DataTable> outdataTables = DataSelectInfo.outputsdatatable;
                            DataTable outresulttables = new DataTable();
                            for (int i = 0; i < outdataTables.Count; i++)
                            {
                                DataTable tables = outdataTables[i];
                                outresulttables = ConvertColumnsToDouble(tables);
                                outresulttables = GetSingleColumnTable(outresulttables, "position");
                            }
                            resulttables = MergeByMinRows(resulttables, outresulttables);
                        }
                        // 创建一个新的 DataTable 用于存储转换后的数据
                        DataTable newDataTable = ChangeColumn(resulttables);
                        // 遍历每一列
                        foreach (DataColumn column in newDataTable.Columns)
                        {
                            string columnName = column.ColumnName;
                            if (column.DataType == typeof(double))
                            {
                                double maxValue = double.MinValue;
                                double minValue = double.MaxValue;

                                // 遍历每一行
                                foreach (DataRow row in newDataTable.Rows)
                                {
                                    double value = double.Parse(row[columnName].ToString());
                                    if (column.ColumnName == "position")
                                    {
                                        if (value > maxValue)
                                        {
                                            maxValue = value;
                                        }
                                        if (value < minValue)
                                        {
                                            minValue = value;
                                        }
                                    }
                                    else
                                    {
                                        if (value > maxValue)
                                        {
                                            maxValue = value;
                                        }
                                        if (value < minValue && value != 0)
                                        {
                                            minValue = value;
                                        }
                                    }
                                }
                                string name = column.ColumnName;
                                string Valuerange = string.Format("[{0},{1}]", Convert.ToInt32(minValue), Convert.ToInt32(maxValue));
                                double number = (maxValue + minValue) / 2;
                                number = Math.Round(number, 2);
                                NumericHelper.ImportNumericToDb(name, Valuerange, number);
                                // MessageBox.Show("请先进行数据读取！！！");
                            }
                        }

                        #endregion
                    }
                    #endregion

                    #region hide by wpz 2025/9/28
                    // 执行你想要测量时间的操作s
                    //PerformSomeOperation();
                    //// 停止计时
                    //stopwatch.Stop();

                    //// 获取操作花费的时间（以秒为单位）
                    //double elapsedSeconds = stopwatch.Elapsed.TotalSeconds;
                    //elapsedSeconds = Math.Round(elapsedSeconds, 3);
                    //if (ModelTrainingInfo.selectmodel == "BPNN")
                    //{
                    //    elapsedSeconds = elapsedSeconds + ResultInfo.bpnnelapsedSeconds;
                    //}
                    //if (ModelTrainingInfo.selectmodel == "SVR")
                    //{
                    //    elapsedSeconds = elapsedSeconds + ResultInfo.svrelapsedSeconds;
                    //}
                    //if (ModelTrainingInfo.selectmodel == "GBR")
                    //{
                    //    elapsedSeconds = elapsedSeconds + ResultInfo.gbrelapsedSeconds;
                    //}
                    //if (ModelTrainingInfo.selectmodel == "RFR")
                    //{
                    //    elapsedSeconds = elapsedSeconds + ResultInfo.rfrelapsedSeconds;
                    //}
                    //if (ModelTrainingInfo.selectmodel == "Stacking")
                    //{
                    //    foreach (string selectedstackingmodel in selectedStackingModel0)
                    //    {
                    //        if (selectedstackingmodel == "BPNN")
                    //        {
                    //            elapsedSeconds = elapsedSeconds + ResultInfo.bpnnelapsedSeconds;
                    //        }
                    //        if (selectedstackingmodel == "SVR")
                    //        {
                    //            elapsedSeconds = elapsedSeconds + ResultInfo.svrelapsedSeconds;
                    //        }
                    //        if (selectedstackingmodel == "GBR")
                    //        {
                    //            elapsedSeconds = elapsedSeconds + ResultInfo.gbrelapsedSeconds;
                    //        }
                    //        if (selectedstackingmodel == "RFR")
                    //        {
                    //            elapsedSeconds = elapsedSeconds + ResultInfo.rfrelapsedSeconds;
                    //        }
                    //    }
                    //}
                    ////// 显示一个简单的消息框
                    ////textEdit3.Visible = true;
                    ////textEdit3.Text = "训练完成！";
                    //// trainingscore = Math.Round(trainingscore, 3);
                    //textEdit4.Text = string.Format("{0}", elapsedSeconds);
                    //textEdit2.Text = string.Format("{0}", trainingscore);

                    //NumericHelper.DeleteNumericPredictionToDb();
                    //List<DataTable> dataTables = DataSelectInfo.inputsdatatable;
                    //// 使用for循环遍历List
                    //for (int i = 0; i < dataTables.Count; i++)
                    //{
                    //    DataTable tables = dataTables[i];
                    //    DataTable resulttables = new DataTable();
                    //    resulttables = ConvertColumnsToDouble(tables);

                    //    // 调用方法判断第一列名是否为id
                    //    bool isFirstColumnNameId = CheckFirstColumnName(resulttables);

                    //    if (isFirstColumnNameId)
                    //    {
                    //        resulttables = RemoveFirstColumn(resulttables);
                    //    }
                    //    // 创建一个新的 DataTable 用于存储转换后的数据
                    //    DataTable newDataTable = ChangeColumn(resulttables);
                    //    // 遍历每一列
                    //    foreach (DataColumn column in newDataTable.Columns)
                    //    {
                    //        string columnName = column.ColumnName;
                    //        if (column.DataType == typeof(double))
                    //        {
                    //            double maxValue = double.MinValue;
                    //            double minValue = double.MaxValue;

                    //            // 遍历每一行
                    //            foreach (DataRow row in newDataTable.Rows)
                    //            {
                    //                double value = double.Parse(row[columnName].ToString());
                    //                if (value > maxValue)
                    //                {
                    //                    maxValue = value;
                    //                }
                    //                if (value < minValue && value != 0)
                    //                {
                    //                    minValue = value;
                    //                }
                    //            }
                    //            string name = column.ColumnName;
                    //            string Valuerange = string.Format("[{0},{1}]", Convert.ToInt32(minValue), Convert.ToInt32(maxValue));
                    //            double number = (maxValue + minValue) / 2;
                    //            number = Math.Round(number, 2);
                    //            NumericHelper.ImportNumericToDb(name, Valuerange, number);
                    //           // MessageBox.Show("请先进行数据读取！！！");
                    //        }
                    //    }
                    //}
                    #endregion
                }
                await SimulateLoadingAsync(); // 模拟加载

            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载时出错: {ex.Message}");
            }
            finally
            {
                isLoading = false;
                btnToggleControls(true);

                // 关闭透明窗体
                loadingOverlay.Close();
                loadingOverlay.Dispose();
            }

        }
        /// <summary>
        /// 按两个表的最小行数合并（忽略超出部分）
        /// </summary>
        static DataTable MergeByMinRows(DataTable dt1, DataTable dt2)
        {
            // 创建结果表结构（合并两表所有列）
            DataTable result = dt1.Clone();
            foreach (DataColumn col in dt2.Columns)
            {
                if (!result.Columns.Contains(col.ColumnName))
                {
                    result.Columns.Add(col.ColumnName, col.DataType);
                }
            }

            // 取两个表的最小行数作为合并范围（忽略超出的行）
            int minRowCount = Math.Min(dt1.Rows.Count, dt2.Rows.Count);

            for (int i = 0; i < minRowCount; i++)
            {
                DataRow newRow = result.NewRow();

                // 填充dt1的第i行数据
                foreach (DataColumn col in dt1.Columns)
                {
                    newRow[col.ColumnName] = dt1.Rows[i][col];
                }

                // 填充dt2的第i行数据
                foreach (DataColumn col in dt2.Columns)
                {
                    newRow[col.ColumnName] = dt2.Rows[i][col];
                }

                result.Rows.Add(newRow);
            }

            return result;
        }

        /// <summary>
        /// 从原DataTable中提取指定列，生成新的DataTable
        /// </summary>
        /// <param name="originalTable">原始DataTable</param>
        /// <param name="columnName">要提取的列名</param>
        /// <returns>只包含指定列的新DataTable</returns>
        static DataTable GetSingleColumnTable(DataTable originalTable, string columnName)
        {
            // 验证参数（替换nameof为字符串，兼容VS2012）
            if (originalTable == null)
                throw new ArgumentNullException("originalTable"); // 原nameof(originalTable)
            if (string.IsNullOrEmpty(columnName))
                throw new ArgumentException("列名不能为空", "columnName"); // 原nameof(columnName)
            if (!originalTable.Columns.Contains(columnName))
                throw new ArgumentException(string.Format("表中不存在名为'{0}'的列", columnName), "columnName"); // 原nameof(columnName)

            // 创建新的DataTable
            DataTable newTable = new DataTable();

            // 复制原列的结构（包括列名、数据类型、约束等）
            DataColumn originalColumn = originalTable.Columns[columnName];
            DataColumn newColumn = new DataColumn(
                originalColumn.ColumnName,
                originalColumn.DataType
            );
            // 复制其他属性（如允许为空、默认值等）
            newColumn.AllowDBNull = originalColumn.AllowDBNull;
            newColumn.DefaultValue = originalColumn.DefaultValue;
            newTable.Columns.Add(newColumn);

            // 复制数据行
            foreach (DataRow originalRow in originalTable.Rows)
            {
                DataRow newRow = newTable.NewRow();
                newRow[0] = originalRow[columnName]; // 将指定列的值复制到新行
                newTable.Rows.Add(newRow);
            }

            return newTable;
        }
        /// <summary>
        /// 绘制图像
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DrawBtn_Click(object sender, EventArgs e)
        {
            if (textEdit4.Text == "" || textEdit2.Text == "" || textEdit4.Text == null || textEdit2.Text == null)
            {
                MessageBox.Show("请先进行模型开始训练！！！");
            }
            else
            {
                panel12.Controls.Clear();

                #region add by wpz 2025/10/13
                string outPath = Path.Combine(Application.StartupPath, "File", "ksxl.txt");
                if (File.Exists(outPath))//取值
                {
                    string strCurrent = "";
                    DataTable actualDataTable = new DataTable();
                    DataTable dataTable = new DataTable();
                    string tempColumn = "";
                    Dictionary<string, List<double>> actualDic = new Dictionary<string, List<double>>();
                    Dictionary<string, List<double>> preditDic = new Dictionary<string, List<double>>();
                    using (BufferedStream bufferedStream = new BufferedStream(new FileStream(outPath, FileMode.Open, FileAccess.Read), 4096000))
                    {
                        using (StreamReader streamReader = new StreamReader(bufferedStream))
                        {
                            while ((strCurrent = streamReader.ReadLine()) != null)
                            {
                                if (strCurrent.Contains("实际值"))
                                {
                                    while ((strCurrent = streamReader.ReadLine()) != null)
                                    {
                                        if (strCurrent.Contains("column"))
                                        {
                                            tempColumn = strCurrent.Trim().ToString().Split(',')[0];
                                            //actualDataTable.Columns.Add(tempColumn);
                                        }
                                        else
                                        {
                                            if (strCurrent.Contains("预测值"))
                                            {
                                                while ((strCurrent = streamReader.ReadLine()) != null)
                                                {
                                                    if (strCurrent.Contains("column"))
                                                    {
                                                        tempColumn = strCurrent.Trim().ToString().Split(',')[0];
                                                        //preditDic.Columns.Add(tempColumn);
                                                    }
                                                    else
                                                    {
                                                        string[] strArr = strCurrent.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                                                        List<double> dataList = new List<double>();
                                                        foreach (string str in strArr)
                                                            dataList.Add(double.Parse(str));
                                                        preditDic.Add(tempColumn, dataList);
                                                    }
                                                }

                                            }
                                            else
                                            {
                                                string[] strArr = strCurrent.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                                                List<double> dataList = new List<double>();
                                                foreach (string str in strArr)
                                                    dataList.Add(double.Parse(str));
                                                actualDic.Add(tempColumn, dataList);
                                            }

                                        }
                                    }
                                }
                            }
                            foreach (KeyValuePair<string, List<double>> item in actualDic)
                            {
                                actualDataTable.Columns.Add(item.Key);
                            }
                            int valueNo = actualDic[actualDic.Keys.First()].Count;
                            for (int kkk = 0; kkk < valueNo; kkk++)
                            {
                                DataRow dr = actualDataTable.NewRow();
                                foreach (KeyValuePair<string, List<double>> item in actualDic)
                                {
                                    dr[item.Key] = item.Value[kkk];
                                }
                                actualDataTable.Rows.Add(dr);
                            }
                            foreach (KeyValuePair<string, List<double>> item in preditDic)
                            {
                                dataTable.Columns.Add(item.Key);
                            }
                            for (int kkk = 0; kkk < valueNo; kkk++)
                            {
                                DataRow dr = dataTable.NewRow();
                                foreach (KeyValuePair<string, List<double>> item in preditDic)
                                {
                                    dr[item.Key] = item.Value[kkk];
                                }
                                dataTable.Rows.Add(dr);
                            }
                        }
                    }


                    DrawChartsForEachColumn(actualDataTable, dataTable);
                }
                #endregion



                #region hide by wpz 2025/10/13
                // 创建一个示例 DataTableZz  
                //DataTable actualDataTable = ModelTrainingInfo.actualDataTable;
                //actualDataTable = ConvertDataTableColumns(actualDataTable);
                //// 创建一个示例 DataTable

                //DataTable dataTable = ModelTrainingInfo.outputsdatatable;
                //dataTable = ConvertDataTableColumns(dataTable);
                //// 为每个列绘制单独的 Chart
                //DrawChartsForEachColumn(actualDataTable, dataTable);
                //// 动态创建多个 Chart 控件
                //CreateChartsFromDataTable(dataTable); await SimulateLoadingAsync();
                #endregion
            }
        }
        public static DataTable ConvertDataTableColumns(DataTable dataTable)
        {
            // 创建一个新的 DataTable 用于存储转换后的数据
            DataTable newDataTable = new DataTable();

            // 第一列转换为 string 类型
            newDataTable.Columns.Add(dataTable.Columns[0].ColumnName, typeof(string));

            // 其他列转换为 double 类型
            for (int i = 1; i < dataTable.Columns.Count; i++)
            {
                newDataTable.Columns.Add(dataTable.Columns[i].ColumnName, typeof(double));
            }

            // 遍历原 DataTable 的每一行
            foreach (DataRow oldRow in dataTable.Rows)
            {
                DataRow newRow = newDataTable.NewRow();

                // 第一列转换为 string
                newRow[0] = oldRow[0].ToString();

                // 其他列转换为 double
                for (int i = 1; i < dataTable.Columns.Count; i++)
                {
                    try
                    {
                        newRow[i] = Convert.ToDouble(oldRow[i]);
                    }
                    catch (FormatException)
                    {
                        // 处理转换失败的情况
                        newRow[i] = 0;
                    }
                }

                newDataTable.Rows.Add(newRow);
            }

            // 清空原 DataTable 并复制新 DataTable 的结构和数据
            dataTable.Clear();
            dataTable.Columns.Clear();
            foreach (DataColumn column in newDataTable.Columns)
            {
                dataTable.Columns.Add(column.ColumnName, column.DataType);
            }
            foreach (DataRow row in newDataTable.Rows)
            {
                dataTable.Rows.Add(row.ItemArray);
            }
            return dataTable;
        }
        // 为每个列绘制单独的 Chart
        private void DrawChartsForEachColumn(DataTable actualDataTable, DataTable predictedDataTable)
        {
            // 使用 FlowLayoutPanel 动态布局
            FlowLayoutPanel flowLayoutPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                WrapContents = true
            };
            panel12.Controls.Add(flowLayoutPanel);

            // 遍历每一列，为每个列创建一个 Chart
            for (int colIndex = 0; colIndex < actualDataTable.Columns.Count; colIndex++)
            {
                string columnName = actualDataTable.Columns[colIndex].ColumnName;

                // 创建 Chart 控件
                Chart chart = new Chart
                {
                    Name = $"Chart_{columnName}",
                    Width = 600,
                    Height = 500,
                    Palette = ChartColorPalette.EarthTones,
                    BackColor = Color.FromArgb(100, 222, 171),
                    BorderlineColor = Color.IndianRed
                };

                // 创建 ChartArea
                ChartArea chartArea = new ChartArea($"ChartArea_{columnName}");
                chart.ChartAreas.Add(chartArea);

                // 创建实测值折线图
                Series actualSeries = new Series
                {
                    Name = $"Actual_{columnName}",
                    ChartType = SeriesChartType.Line,
                    BorderWidth = 2,
                    Color = Color.Blue,
                    BorderDashStyle = ChartDashStyle.Solid
                };

                // 创建预测值折线图
                Series predictedSeries = new Series
                {
                    Name = $"Predicted_{columnName}",
                    ChartType = SeriesChartType.Line,
                    BorderWidth = 2,
                    Color = Color.Red,
                    //BorderDashStyle = ChartDashStyle.Dash
                    BorderDashStyle = ChartDashStyle.Solid
                };

                // 填充实测值和预测值数据
                for (int rowIndex = 0; rowIndex < actualDataTable.Rows.Count; rowIndex++)
                {
                    double actualValue = Convert.ToDouble(actualDataTable.Rows[rowIndex][colIndex]);
                    double predictedValue = Convert.ToDouble(predictedDataTable.Rows[rowIndex][colIndex]);
                    actualSeries.Points.AddXY(rowIndex, actualValue);
                    predictedSeries.Points.AddXY(rowIndex, predictedValue);
                }

                // 将折线图添加到 Chart 控件
                chart.Series.Add(actualSeries);
                chart.Series.Add(predictedSeries);

                // 添加图例
                chart.Legends.Add(new Legend("Legend"));
                actualSeries.LegendText = "实测值";
                predictedSeries.LegendText = "预测值";

                // 设置标题和轴标签
                //chart.Titles.Add($"Comparison of Actual vs Predicted for {columnName}");
                //chart.ChartAreas[0].AxisX.Title = "Index";
                chart.ChartAreas[0].AxisY.Title = $"{columnName}";

                // 将 Chart 添加到 FlowLayoutPanel
                flowLayoutPanel.Controls.Add(chart);
            }
        }
        private void CreateChartsFromDataTable(DataTable dataTable)
        {
            // 使用 FlowLayoutPanel 动态布局
            FlowLayoutPanel flowLayoutPanel = new FlowLayoutPanel();
            flowLayoutPanel.Dock = DockStyle.Fill;
            flowLayoutPanel.AutoScroll = true; // 启用滚动条
            flowLayoutPanel.WrapContents = true; // 自动换行
            panel12.Controls.Add(flowLayoutPanel);

            // 第一列是横坐标，其他列是纵坐标
            string xColumnName = dataTable.Columns[0].ColumnName;

            // 循环创建 Chart
            for (int i = 1; i < dataTable.Columns.Count; i++)
            {
                string yColumnName = dataTable.Columns[i].ColumnName;

                // 创建 Chart 控件
                Chart chart = new Chart();
                chart.Width = 400; // 设置 Chart 宽度
                chart.Height = 380; // 设置 Chart 高度
                chart.Palette = ChartColorPalette.EarthTones;
                chart.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(222)))), ((int)(((byte)(171)))));
                chart.BorderlineColor = System.Drawing.Color.IndianRed;
                chart.BorderSkin.BackColor = System.Drawing.Color.Transparent;
                chart.BorderSkin.BackImageTransparentColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
                chart.BorderSkin.BorderColor = System.Drawing.Color.Transparent;
                chart.BorderSkin.PageColor = System.Drawing.Color.Transparent;
                // 创建 ChartArea
                ChartArea chartArea = new ChartArea();
                chart.ChartAreas.Add(chartArea);

                // 创建 Series
                Series series = new Series();
                series.ChartType = SeriesChartType.Line; // 设置图表类型为折线图
                chart.Series.Add(series);
                // 绑定数据
                foreach (DataRow row in dataTable.Rows)
                {
                    string xValue = row[xColumnName].ToString();
                    int yValue = Convert.ToInt32(row[yColumnName]);
                    series.Points.AddXY(xValue, yValue);
                }

                // 设置横坐标标题
                chartArea.AxisX.Title = "炉次";
                // 设置纵坐标标题
                chartArea.AxisY.Title = yColumnName;
                // 创建一个Series并添加到Chart中
                //chart1.Series.Add(series);
                chartArea.BackColor = System.Drawing.Color.SteelBlue;
                chartArea.BorderColor = System.Drawing.Color.White;
                // 将Chart控件添加到窗体中
                panel12.Controls.Add(chart);
                // 将 Chart 添加到 FlowLayoutPanel
                flowLayoutPanel.Controls.Add(chart);
            }
        }
        private void ChoosePathBtn_MouseEnter(object sender, EventArgs e)
        {
            ChoosePathBtn.BackColor = Color.SteelBlue;
            ChoosePathBtn.ForeColor = Color.White;
        }

        private void ChoosePathBtn_MouseLeave(object sender, EventArgs e)
        {
            ChoosePathBtn.BackColor = Color.Transparent;
            ChoosePathBtn.ForeColor = Color.White;
        }

        private void roaddataBtn_MouseEnter(object sender, EventArgs e)
        {
            roaddataBtn.BackColor = Color.SteelBlue;
            roaddataBtn.ForeColor = Color.White;
        }

        private void roaddataBtn_MouseLeave(object sender, EventArgs e)
        {
            roaddataBtn.BackColor = Color.Transparent;
            roaddataBtn.ForeColor = Color.White;
        }

        private void DrawLineBtn_MouseEnter(object sender, EventArgs e)
        {
            DrawLineBtn.BackColor = Color.SteelBlue;
            DrawLineBtn.ForeColor = Color.White;
        }

        private void DrawLineBtn_MouseLeave(object sender, EventArgs e)
        {
            DrawLineBtn.BackColor = Color.Transparent;
            DrawLineBtn.ForeColor = Color.White;
        }

        private void TrainingBtn_MouseEnter(object sender, EventArgs e)
        {
            TrainingBtn.BackColor = Color.SteelBlue;
            TrainingBtn.ForeColor = Color.White;
        }

        private void TrainingBtn_MouseLeave(object sender, EventArgs e)
        {
            TrainingBtn.BackColor = Color.Transparent;
            TrainingBtn.ForeColor = Color.White;
        }

        private void DrawBtn_MouseEnter(object sender, EventArgs e)
        {
            DrawBtn.BackColor = Color.SteelBlue;
            DrawBtn.ForeColor = Color.White;
        }

        private void DrawBtn_MouseLeave(object sender, EventArgs e)
        {
            DrawBtn.BackColor = Color.Transparent;
            DrawBtn.ForeColor = Color.White;
        }

        private void SureBtn_MouseEnter(object sender, EventArgs e)
        {
            SureBtn.BackColor = Color.SteelBlue;
            SureBtn.ForeColor = Color.White;
        }

        private void SureBtn_MouseLeave(object sender, EventArgs e)
        {
            SureBtn.BackColor = Color.Transparent;
            SureBtn.ForeColor = Color.White;
        }

        private void button4_MouseLeave(object sender, EventArgs e)
        {
            InputDataBtn.BackColor = Color.Transparent;
            InputDataBtn.ForeColor = Color.White;
        }

        private void button4_MouseEnter(object sender, EventArgs e)
        {
            InputDataBtn.BackColor = Color.SteelBlue;
            InputDataBtn.ForeColor = Color.White;
        }

        private void Timetxt_MouseEnter(object sender, EventArgs e)
        {
            Timetxt.BackColor = Color.FromArgb(13, 65, 113); // 设置你想要的背景色;
        }

        private void Timetxt_MouseLeave(object sender, EventArgs e)
        {
            Timetxt.BackColor = Color.Transparent;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Timetxt.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }

        /// <summary>
        /// 异常值去除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void SureBtn_Click(object sender, EventArgs e)
        {
            if (DataSelectInfo.selectedDataTables.Count > 0)
            {
                if (thresholdTxt.Text == "" || thresholdTxt.Text == null)
                {
                    MessageBox.Show("请输入阈值(箱线图四分位差倍数)!!!!");
                }
                else
                {
                    if (isLoading) return;
                    isLoading = true;

                    // 创建并显示透明窗体
                    LoadingOverlayForm loadingOverlay = new LoadingOverlayForm(this);
                    loadingOverlay.Show();

                    btnToggleControls(false);
                    try
                    {
                        #region hide by wpz 2025/10/13
                        double iqrMultiplier = double.Parse(thresholdTxt.Text.ToString());
                        DataSelectInfo.threshold = double.Parse(thresholdTxt.Text.ToString());
                        List<DataTable> dataTables = DataSelectInfo.selectedDataTables;
                        DataSelectInfo.selectedDataTables = new List<DataTable>();

                        // 并行处理每个 DataTable
                        Parallel.ForEach(dataTables, tables =>
                        {
                            // 创建新的DataTable
                            tables = ConvertColumnsToDouble(tables);
                            // 设置四分位差倍数
                            //double iqrMultiplier = double.Parse(thresholdTxt.Text.ToString());
                            // 去除异常值
                            DataTable resulttables = RemoveOutliersOptimized(tables, iqrMultiplier);
                            lock (DataSelectInfo.selectedDataTables)
                            {
                                DataSelectInfo.selectedDataTables.Add(resulttables);
                            }
                        });

                        await SimulateLoadingAsync(); // 模拟加载
                        #endregion 
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"加载时出错: {ex.Message}");
                    }
                    finally
                    {
                        isLoading = false;
                        btnToggleControls(true);

                        // 关闭透明窗体
                        loadingOverlay.Close();
                        loadingOverlay.Dispose();
                    }
                }
            }
            else
            {
                MessageBox.Show("请先进行数据读取！！！");
            }
        }

        public static DataTable RemoveOutliersOptimized(DataTable inputTable, double iqrMultiplier)
        {
            // 创建一个新的 DataTable 用于存储处理后的数据
            DataTable outputTable = inputTable.Clone();

            // 提前计算每列的上下限
            double[][] bounds = new double[inputTable.Columns.Count][];
            for (int colIndex = 1; colIndex < inputTable.Columns.Count; colIndex++)
            {
                // 获取当前列的数据
                double[] columnData = new double[inputTable.Rows.Count];
                for (int i = 0; i < inputTable.Rows.Count; i++)
                {
                    columnData[i] = Convert.ToDouble(inputTable.Rows[i][colIndex]);
                }

                // 对数据进行排序
                Array.Sort(columnData);

                // 计算第一四分位数（Q1）和第三四分位数（Q3）
                int n = columnData.Length;
                double q1 = GetMedian(columnData, 0, n / 2 - 1);
                double q3 = GetMedian(columnData, (n + 1) / 2, n - 1);

                // 计算四分位距（IQR）
                double iqr = q3 - q1;

                // 计算下限和上限
                double lowerBound = q1 - iqrMultiplier * iqr;
                double upperBound = q3 + iqrMultiplier * iqr;

                bounds[colIndex] = new double[] { lowerBound, upperBound };
            }

            // 遍历输入表的每一行
            foreach (DataRow inputRow in inputTable.Rows)
            {
                bool isValidRow = true;

                // 从第二列开始检查每一列
                for (int colIndex = 1; colIndex < inputTable.Columns.Count; colIndex++)
                {
                    double value = Convert.ToDouble(inputRow[colIndex]);
                    if (value < bounds[colIndex][0] || value > bounds[colIndex][1])
                    {
                        isValidRow = false;
                        break;
                    }
                }

                // 如果当前行没有异常值，则将其添加到输出表中
                if (isValidRow)
                {
                    outputTable.ImportRow(inputRow);
                }
            }

            return outputTable;
        }
        //private async void SureBtn_Click(object sender, EventArgs e)
        //{
        //    if (thresholdTxt.Text == "" || thresholdTxt.Text == null)
        //    {
        //        MessageBox.Show("请输入阈值(箱线图四分位差倍数)!!!!");
        //    }
        //    else {

        //        if (isLoading) return;
        //        isLoading = true;

        //        // 创建并显示透明窗体
        //        LoadingOverlayForm loadingOverlay = new LoadingOverlayForm(this);
        //        loadingOverlay.Show();

        //        btnToggleControls(false);
        //        try
        //        {
        //            DataSelectInfo.threshold = double.Parse(thresholdTxt.Text.ToString());
        //            List<DataTable> dataTables = DataSelectInfo.selectedDataTables;
        //            DataSelectInfo.selectedDataTables = new List<DataTable>(); ;
        //            // 使用for循环遍历List
        //            for (int i = 0; i < dataTables.Count; i++)
        //            {
        //                DataTable tables = dataTables[i];
        //                // 创建新的DataTable
        //                tables = ConvertColumnsToDouble(tables);
        //                // 设置四分位差倍数
        //                double iqrMultiplier = double.Parse(thresholdTxt.Text.ToString());
        //                // 去除异常值
        //                DataTable resulttables = RemoveOutliers(tables, iqrMultiplier);
        //                DataSelectInfo.selectedDataTables.Add(resulttables);
        //            }
        //            await SimulateLoadingAsync(); // 模拟加载
        //        }
        //        catch (Exception ex)
        //        {
        //            MessageBox.Show($"加载时出错: {ex.Message}");
        //        }
        //        finally
        //        {
        //            isLoading = false;
        //            btnToggleControls(true);

        //            // 关闭透明窗体
        //            loadingOverlay.Close();
        //            loadingOverlay.Dispose();
        //        }
        //    }
        //}
        //datatable非数字单元格转成0
        static DataTable ConvertColumnsToDouble(DataTable original)
        {
            foreach (DataRow row in original.Rows)
            {
                for (int i = 1; i < original.Columns.Count; i++) // 从第二列开始（索引1）
                {
                    // 尝试转换并添加到新行，对于非数字字符串，这里可以选择抛出异常或使用特定值代替，例如0或double.NaN
                    try
                    {
                        row[i] = Convert.ToDouble(row[i]); // 注意，这里假定所有非数字字符串都可以转换为0或其他值，根据需要调整逻辑处理非数字字符串。
                    }
                    catch (FormatException) // 处理无法转换的情况，可以选择抛出异常或使用默认值，例如0或double.NaN。
                    {
                        row[i] = 0; // 或者使用 double.NaN 表示非数字值。
                    }
                }
            }
            return original;
        }
        public static DataTable RemoveOutliers(DataTable inputTable, double iqrMultiplier)
        {
            // 创建一个新的 DataTable 用于存储处理后的数据
            DataTable outputTable = inputTable.Clone();

            // 遍历输入表的每一行
            foreach (DataRow inputRow in inputTable.Rows)
            {
                bool isValidRow = true;

                // 从第二列开始检查每一列
                for (int colIndex = 1; colIndex < inputTable.Columns.Count; colIndex++)
                {
                    // 获取当前列的数据
                    DataColumn currentColumn = inputTable.Columns[colIndex];
                    double[] columnData = new double[inputTable.Rows.Count];
                    for (int i = 0; i < inputTable.Rows.Count; i++)
                    {
                        columnData[i] = Convert.ToDouble(inputTable.Rows[i][colIndex]);
                    }

                    // 对数据进行排序
                    Array.Sort(columnData);

                    // 计算第一四分位数（Q1）和第三四分位数（Q3）
                    int n = columnData.Length;
                    double q1 = GetMedian(columnData, 0, n / 2 - 1);
                    double q3 = GetMedian(columnData, (n + 1) / 2, n - 1);

                    // 计算四分位距（IQR）
                    double iqr = q3 - q1;

                    // 计算下限和上限
                    double lowerBound = q1 - iqrMultiplier * iqr;
                    double upperBound = q3 + iqrMultiplier * iqr;

                    // 检查当前行的值是否在上下限范围内
                    double value = Convert.ToDouble(inputRow[colIndex]);
                    if (value < lowerBound || value > upperBound)
                    {
                        isValidRow = false;
                        break;
                    }
                }

                // 如果当前行没有异常值，则将其添加到输出表中
                if (isValidRow)
                {
                    outputTable.ImportRow(inputRow);
                }
            }

            return outputTable;
        }

        private static double GetMedian(double[] data, int start, int end)
        {
            int n = end - start + 1;
            if (n % 2 == 1)
            {
                return data[start + n / 2];
            }
            else
            {
                return (data[start + n / 2 - 1] + data[start + n / 2]) / 2;
            }
        }
        /// <summary>
        /// 数值预测
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void InputDataBtn_Click(object sender, EventArgs e)
        {
            if (textEdit4.Text != "" && textEdit2.Text != "")
            {
                NumericPredictionForm numericPrediction = new NumericPredictionForm();
                if (numericPrediction.ShowDialog() == DialogResult.OK)
                {
                    DataTable tempDt = numericPrediction.resultDt;
                    #region add by wpz 2025/9/27
                    string outPath = Path.Combine(Application.StartupPath, "File", "szyc.txt");
                    string modelFileName = string.Format("{0}", Application.StartupPath + "\\Model\\model");
                    string pythonPath = Path.Combine(Application.StartupPath, "File", "szyc.py");
                    string inputPath = string.Format("{0}", Application.StartupPath + "\\File\\input-ms-2022-3-14.csv");
                    string outputPath = string.Format("{0}", Application.StartupPath + "\\File\\output-2022-3-14.csv");
                    string intputStr = "";
                    string outputStr = "";
                    string trainStr = "";
                    string otherStr = "";
                    StringBuilder otherSb = new StringBuilder();
                    if (tempDt != null && tempDt.Rows.Count > 0)
                    {
                        for (int k = 0; k < tempDt.Rows.Count; k++)
                        {
                            if (k != tempDt.Rows.Count - 1)
                            {
                                otherSb.Append(tempDt.Rows[k]["Column1"].ToString()).Append('=').Append(tempDt.Rows[k]["数值"].ToString()).Append(';');
                            }
                            else
                            {
                                otherSb.Append(tempDt.Rows[k]["Column1"].ToString()).Append('=').Append(tempDt.Rows[k]["数值"].ToString()).Append(';');
                            }

                        }
                        otherSb.Append("position").Append('=').Append("1");
                        //otherStr = string.Join(";", tempDt.AsEnumerable().Select(row => row.Field<string>("数值")));
                    }

                    StringBuilder selectStr = new StringBuilder();
                    string filePath = DataSelectInfo.selectedFolderPath;
                    if (temperatureckBox.Checked)
                    {
                        intputStr = temperatureckBox.Text.ToString();
                    }
                    if (speedckBox.Checked)
                    {
                        intputStr = speedckBox.Text.ToString();
                    }
                    if (mockBox.Checked)
                    {
                        intputStr = mockBox.Text.ToString();
                    }
                    if (dopingckBox.Checked)
                    {
                        intputStr = dopingckBox.Text.ToString();
                    }
                    if (speedBtn.Checked)
                    {
                        outputStr = speedBtn.Text.ToString();
                    }
                    if (electricalBtn.Checked)
                    {
                        outputStr = electricalBtn.Text.ToString();
                    }
                    if (xrdBtn.Checked)
                    {
                        outputStr = xrdBtn.Text.ToString();
                    }
                    if (plBtn.Checked)
                    {
                        outputStr = plBtn.Text.ToString();
                    }
                    if (BPNNModelBtn.Checked)
                    {
                        trainStr = BPNNModelBtn.Text.ToString();
                        selectStr.Append(ModelTrainingInfo.BPNNHiddennode.ToString()).Append(',').Append(ModelTrainingInfo.BPNNPenaltyfactor.ToString()).Append(',').Append(ModelTrainingInfo.BPNNMaxIterations.ToString()).Append(',').Append(ModelTrainingInfo.BPNNGradientDescent.ToString());

                    }
                    if (SVRModelBtn.Checked)
                    {
                        trainStr = SVRModelBtn.Text.ToString();
                        selectStr.Append(ModelTrainingInfo.SVRkernelFunction.ToString()).Append(',').Append(ModelTrainingInfo.SVRtolerance.ToString()).Append(',').Append(ModelTrainingInfo.SVRregularizationparameter.ToString()).Append(',').Append(ModelTrainingInfo.SVRMaxIterations);
                    }
                    if (GBRModelBtn.Checked)
                    {
                        trainStr = GBRModelBtn.Text.ToString();
                        selectStr.Append(ModelTrainingInfo.GBRLossfunction.ToString()).Append(',').Append(ModelTrainingInfo.GBRLearningrate.ToString()).Append(',').Append(ModelTrainingInfo.GBRNumberoflearners.ToString()).Append(',').Append(ModelTrainingInfo.GBRMinimumpartitionnode.ToString()).Append(',').Append(ModelTrainingInfo.GBRMaximumdepth.ToString());
                    }
                    if (RFRModelBtn.Checked)
                    {
                        trainStr = RFRModelBtn.Text.ToString();
                        selectStr.Append(ModelTrainingInfo.RFRNumberoflearners.ToString()).Append(',').Append(ModelTrainingInfo.RFRMinimumpartitionnode.ToString()).Append(',').Append(ModelTrainingInfo.RFRMaximumdepth.ToString());

                    }
                    if (StackingModelBtn.Checked)
                    {
                        trainStr = StackingModelBtn.Text.ToString();
                        selectStr.Append(BPNNModel0Box.Checked.ToString()).Append(';').Append(ModelTrainingInfo.BPNNHiddennode.ToString()).Append(';').Append(ModelTrainingInfo.BPNNPenaltyfactor.ToString()).Append(';').Append(ModelTrainingInfo.BPNNMaxIterations.ToString()).Append(';').Append(ModelTrainingInfo.BPNNGradientDescent.ToString()).Append(',').Append(SVRModel0Box.Checked.ToString()).Append(';').Append(ModelTrainingInfo.SVRkernelFunction.ToString()).Append(';').Append(ModelTrainingInfo.SVRtolerance.ToString()).Append(';').Append(ModelTrainingInfo.SVRregularizationparameter.ToString()).Append(';').Append(ModelTrainingInfo.SVRMaxIterations).Append(',').Append(GBRModel0Box.Checked.ToString()).Append(';').Append(ModelTrainingInfo.GBRLossfunction.ToString()).Append(';').Append(ModelTrainingInfo.GBRLearningrate.ToString()).Append(';').Append(ModelTrainingInfo.GBRNumberoflearners.ToString()).Append(';').Append(ModelTrainingInfo.GBRMinimumpartitionnode.ToString()).Append(';').Append(ModelTrainingInfo.GBRMaximumdepth.ToString()).Append(',').Append(RFRFModel0Box.Checked.ToString()).Append(';').Append(ModelTrainingInfo.RFRNumberoflearners.ToString()).Append(';').Append(ModelTrainingInfo.RFRMinimumpartitionnode.ToString()).Append(';').Append(ModelTrainingInfo.RFRMaximumdepth.ToString()).Append(',').Append(BPNNModel1Btn.Checked.ToString()).Append(';').Append(ModelTrainingInfo.BPNNHiddennode.ToString()).Append(';').Append(ModelTrainingInfo.BPNNPenaltyfactor.ToString()).Append(';').Append(ModelTrainingInfo.BPNNMaxIterations.ToString()).Append(';').Append(ModelTrainingInfo.BPNNGradientDescent.ToString()).Append(',').Append(SVRModel1Btn.Checked.ToString()).Append(';').Append(ModelTrainingInfo.SVRkernelFunction.ToString()).Append(';').Append(ModelTrainingInfo.SVRtolerance.ToString()).Append(';').Append(ModelTrainingInfo.SVRregularizationparameter.ToString()).Append(';').Append(ModelTrainingInfo.SVRMaxIterations).Append(',').Append(GBRModel1Btn.Checked.ToString()).Append(';').Append(ModelTrainingInfo.GBRLossfunction.ToString()).Append(';').Append(ModelTrainingInfo.GBRLearningrate.ToString()).Append(';').Append(ModelTrainingInfo.GBRNumberoflearners.ToString()).Append(';').Append(ModelTrainingInfo.GBRMinimumpartitionnode.ToString()).Append(';').Append(ModelTrainingInfo.GBRMaximumdepth.ToString()).Append(',').Append(RFRFModel1Btn.Checked.ToString()).Append(';').Append(ModelTrainingInfo.RFRNumberoflearners.ToString()).Append(';').Append(ModelTrainingInfo.RFRMinimumpartitionnode.ToString()).Append(';').Append(ModelTrainingInfo.RFRMaximumdepth.ToString());
                    }
                    string[] strArr = new string[7];
                    strArr[0] = intputStr;
                    strArr[1] = outputStr;
                    strArr[2] = trainStr;
                    strArr[3] = otherSb.ToString();
                    strArr[4] = filePath;
                    strArr[5] = selectStr.ToString();
                    strArr[6] = outPath.ToString();
                    string sArguments = pythonPath + " " + strArr[0] + " " + strArr[1] + " " + strArr[2] + " " + strArr[3] + " " + strArr[4] + " " + strArr[5] + " " + strArr[6];
                    #region test by wpz 
                    string fullName = Path.Combine(Application.StartupPath, "File", "szyctest.txt");
                    using (FileStream fs = new FileStream(fullName, FileMode.Create, FileAccess.Write))
                    {
                        StreamWriter sw = new StreamWriter(fs, Encoding.Default);
                        sw.Flush();
                        sw.Write(sArguments.ToString());
                        sw.Flush();
                        sw.Close();
                    }
                    #endregion
                    ProcessStartInfo start = new ProcessStartInfo();
                    start.FileName = @"python.exe";
                    start.Arguments = sArguments;//参数以空格分隔，如果某个参数为空，可以传入
                    start.UseShellExecute = false; //必需
                    start.RedirectStandardOutput = true;//输出参数设定
                    start.RedirectStandardInput = true;//传入参数设定
                    start.RedirectStandardError = true;
                    start.CreateNoWindow = true;
                    using (Process pc = Process.Start(start))
                    {
                        pc.BeginOutputReadLine();
                        pc.BeginErrorReadLine();
                        pc.WaitForExit();
                    }
                    if (File.Exists(outPath))//取值
                    {
                        string strCurrent = "";
                        this.resulttextBox.Text = "";
                        using (BufferedStream bufferedStream = new BufferedStream(new FileStream(outPath, FileMode.Open, FileAccess.Read), 4096000))
                        {
                            using (StreamReader streamReader = new StreamReader(bufferedStream))
                            {
                                while ((strCurrent = streamReader.ReadLine()) != null)
                                {
                                    resulttextBox.AppendText(strCurrent + Environment.NewLine);
                                }
                                resulttextBox.SelectionStart = 0;
                                resulttextBox.SelectionLength = 0;
                                resulttextBox.ScrollToCaret();
                            }
                        }
                    }
                    #endregion
                }



                //numericPrediction.Show();                
                //this.resulttextBox.Text = "";
                //// 创建一个带有换行符的字符串
                //string multiLineString = Visualization.pridectdata;
                ///// 按 \n 分割字符串
                //string[] lines = multiLineString.Split('\n');
                //// 将分割后的每一行添加到 TextBox 中
                //foreach (string line in lines)
                //{
                //    resulttextBox.AppendText(line + Environment.NewLine);
                //}
                //resulttextBox.SelectionStart = 0;
                //resulttextBox.SelectionLength = 0;
                //resulttextBox.ScrollToCaret();
            }
            else
            {
                MessageBox.Show("请先进行模型训练！！！");
            }
        }
        //DataTable 的列转成double存储数据
        public DataTable ChangeColumn(DataTable dataTable)
        {
            // 创建一个新的 DataTable 用于存储转换后的数据
            DataTable newDataTable = new DataTable();
            foreach (DataColumn column in dataTable.Columns)
            {
                newDataTable.Columns.Add(column.ColumnName, typeof(double));
            }

            // 遍历原 DataTable 的每一行
            foreach (DataRow row in dataTable.Rows)
            {
                DataRow newRow = newDataTable.NewRow();
                // 遍历每一列
                for (int i = 0; i < dataTable.Columns.Count; i++)
                {
                    try
                    {
                        // 尝试将单元格的值转换为 double 类型
                        newRow[i] = Convert.ToDouble(row[i]);
                    }
                    catch (FormatException)
                    {
                        // 若转换失败，可根据需求处理，这里简单输出错误信息
                        Console.WriteLine($"无法将单元格 {row.Table.Columns[i].ColumnName} 的值 '{row[i]}' 转换为 double 类型。");
                        newRow[i] = double.NaN; // 用 NaN 表示转换失败
                    }
                }
                // 将新行添加到新的 DataTable 中
                newDataTable.Rows.Add(newRow);
            }
            return newDataTable;
        }


    }
}
