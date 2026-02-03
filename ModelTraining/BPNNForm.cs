using Accord.Math;
using Accord.Neuro;
using Accord.Neuro.Learning;
using MathNet.Numerics.LinearAlgebra.Double;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using WisdomGrowth.DataBase;

namespace WisdomGrowth.ModelTraining
{
    public partial class BPNNForm : Form
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
        public BPNNForm()
        {
            InitializeComponent();
            // 设置双缓冲以减少闪烁
            this.DoubleBuffered = true;
            // 初始化定时器
            animationTimer = new Timer();
            animationTimer.Interval = AnimationSpeed;
            animationTimer.Tick += Timer_Tick;
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
        private async void PerformSomeOperation()
        {
            if (isLoading) return;
            isLoading = true;
            // 创建并显示透明窗体
            LoadingOverlayForm loadingOverlay = new LoadingOverlayForm(this);
            loadingOverlay.Show();
            btnToggleControls(false);
            try
            {
                ModelTrainingInfo.BPNNHiddennode = double.Parse(HiddennodeTxt.Text.ToString());
                ModelTrainingInfo.BPNNPenaltyfactor = double.Parse(PenaltyfactorTxt.Text.ToString());
                ModelTrainingInfo.BPNNMaxIterations = double.Parse(MaxIterationsTxt.Text.ToString());

                // 假设这里有一个 DataTable 对象
                List<DataTable> dataTables = DataSelectInfo.outputsdatatable;
                List<DataTable> inputdataTables = DataSelectInfo.inputsdatatable;

                // 创建一个新的列表并添加第一个列表的所有元素
                List<DataTable> combinedTables = new List<DataTable>(dataTables);
                // 添加第二个列表的所有元素
                combinedTables.AddRange(inputdataTables);
                DataTable mergedTable = DataTableMerger.MergeAllTables(combinedTables);
                var (table90, table10) = DataTableMerger.SplitDataTableByRatio(mergedTable);
                // 准备输入和输出的 DataTable
                var (inputDataTables, outputDataTable) = PrepareDataTables(table90, table10);
                

                double[][] inputs = inputDataTables.ToJagged<double>();
                // 将输出 DataTable 转换为输出数组
                double[][] outputs = outputDataTable.ToJagged<double>();
                // 配置神经网络
                var activationFunction = new SigmoidFunction();
                var network = new ActivationNetwork(activationFunction, inputDataTables.Columns.Count, 10, 10, outputDataTable.Columns.Count);
                // 初始化权重
                new NguyenWidrow(network).Randomize();
                if (ModelTrainingInfo.BPNNGradientDescent == "lbfgs")
                {
                    // 创建 L-BFGS 学习器
                    var lbfgs = new LevenbergMarquardtLearning(network);
                }
                if (ModelTrainingInfo.BPNNGradientDescent == "sgd")
                {
                    // 创建 SGD 学习器
                    var sgd = new BackPropagationLearning(network)
                    {
                        LearningRate = 0.1, // 学习率
                        Momentum = 0.2      // 动量
                    };
                }
                if (ModelTrainingInfo.BPNNGradientDescent == "adam")
                {
                    // 手动实现 Adam 优化算法的参数
                    double learningRate = 0.001;
                    double beta1 = 0.9;
                    double beta2 = 0.999;
                    double epsilon = 1e-8;
                }

                // 训练模型
                int maxEpochs = int.Parse(ModelTrainingInfo.BPNNMaxIterations.ToString()); // 最大训练轮数
                // 预测值
                double[][] predicted = Predict(network, inputs);
                // 将实测结果转换为 DataTable
                DataTable actualDataTable = ConvertToDataTable(outputs, outputDataTable);
                // 将预测结果转换为 DataTable
                DataTable predictedDataTable = ConvertToDataTable(predicted, outputDataTable);
                // 计算评估指标
                double r2 = CalculateR2(outputs, predicted);
                // 返回 DataTable 类型的实测结果
                ModelTrainingInfo.actualDataTable = table10;
                // 预测，返回 DataTable 类型的预测结果
                //ModelTrainingInfo.outputsdatatable = table90;
                // 预测，返回 DataTable 类型的预测结果
                ModelTrainingInfo.outputsdatatable = predictedDataTable;

                r2 = Math.Abs(r2);
                string formatted = Math.Round(r2, 3).ToString();
                ModelTrainingInfo.trainingscore = formatted;
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
            this.Close();
        }
        private async void OKBtn_Click(object sender, EventArgs e)
        {
            if (DataSelectInfo.inputsdatatable.Count > 0 && DataSelectInfo.outputsdatatable.Count > 0)
            {
                ModelTrainingInfo.BPNNHiddennode = double.Parse(HiddennodeTxt.Text.ToString());
                ModelTrainingInfo.BPNNPenaltyfactor = double.Parse(PenaltyfactorTxt.Text.ToString());
                ModelTrainingInfo.BPNNMaxIterations = double.Parse(MaxIterationsTxt.Text.ToString());
                this.Close();
                //// 创建一个新的Stopwatch实例
                //Stopwatch stopwatch = new Stopwatch();
                //// 开始计时
                //stopwatch.Start();

                //// 执行你想要测量时间的操作
                //PerformSomeOperation();
                //// 停止计时
                //stopwatch.Stop();

                //// 获取操作花费的时间（以秒为单位）
                //double elapsedSeconds = stopwatch.Elapsed.TotalSeconds;
                //elapsedSeconds = Math.Round(elapsedSeconds, 3);
                //ResultInfo.bpnnelapsedSeconds = elapsedSeconds;
            }
            else
            {
                MessageBox.Show("请先进行数据读取！！！");
            }
        }

        private (DataTable input, DataTable output) PrepareDataTables(DataTable inputDataTable1, DataTable inputDataTable2)
        {
           // List<DataTable> inputDataTables = new List<DataTable> { inputDataTable1, inputDataTable1 };
            return (inputDataTable1, inputDataTable2);
        }
        private double[][] CombineInputDataTables(List<DataTable> inputDataTables)
        {
            int numRows = inputDataTables[0].Rows.Count;
            int totalColumns = GetTotalInputColumns(inputDataTables);
            double[][] combinedInputs = new double[numRows][];

            for (int i = 0; i < numRows; i++)
            {
                combinedInputs[i] = new double[totalColumns];
                int currentColumnIndex = 0;
                foreach (DataTable inputTable in inputDataTables)
                {
                    for (int j = 0; j < inputTable.Columns.Count; j++)
                    {
                        combinedInputs[i][currentColumnIndex++] = Convert.ToDouble(inputTable.Rows[i][j]);
                    }
                }
            }

            return combinedInputs;
        }

        private int GetTotalInputColumns(List<DataTable> inputDataTables)
        {
            int totalColumns = 0;
            foreach (DataTable inputTable in inputDataTables)
            {
                totalColumns += inputTable.Columns.Count;
            }
            return totalColumns;
        }

        private DataTable ConvertToDataTable(double[][] data, DataTable templateTable)
        {
            DataTable resultTable = new DataTable();
            foreach (DataColumn column in templateTable.Columns)
            {
                resultTable.Columns.Add(column.ColumnName, column.DataType);
            }

            for (int i = 0; i < data.Length; i++)
            {
                DataRow newRow = resultTable.NewRow();
                for (int j = 0; j < data[i].Length; j++)
                {
                    newRow[j] = data[i][j];
                }
                resultTable.Rows.Add(newRow);
            }

            return resultTable;

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

        // 预测的通用方法
        private double[][] Predict(ActivationNetwork network, double[][] inputs)
        {
            double[][] predicted = new double[inputs.Length][];
            for (int i = 0; i < inputs.Length; i++)
            {
                predicted[i] = network.Compute(inputs[i]);
            }
            return predicted;
        }

        private void CancelBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void lbfgsRbtn_CheckedChanged(object sender, EventArgs e)
        {
            ModelTrainingInfo.BPNNGradientDescent = lbfgsRbtn.Text;
        }

        private void sgdRbtn_CheckedChanged(object sender, EventArgs e)
        {
            ModelTrainingInfo.BPNNGradientDescent = sgdRbtn.Text;
        }

        private void adamRbtn_CheckedChanged(object sender, EventArgs e)
        {
            ModelTrainingInfo.BPNNGradientDescent = adamRbtn.Text;
        }

        private void BPNNForm_Load(object sender, EventArgs e)
        {
            ModelTrainingInfo.BPNNGradientDescent = lbfgsRbtn.Text;
        }
    }
}
