using Accord.Math;
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
    public partial class GBRForm : Form
    {
        // 定义损失函数接口
        public interface ILossFunction
        {
            double[] Gradient(double[] actual, double[] predicted);
        }

        // 均方误差损失函数
        public class SquaredErrorLoss : ILossFunction
        {
            public double[] Gradient(double[] actual, double[] predicted)
            {
                int n = actual.Length;
                double[] gradients = new double[n];
                for (int i = 0; i < n; i++)
                {
                    gradients[i] = 2 * (predicted[i] - actual[i]);
                }
                return gradients;
            }
        }
        // Huber 损失函数
        public class HuberLoss : ILossFunction
        {
            private double delta;

            public HuberLoss(double delta)
            {
                this.delta = delta;
            }

            public double[] Gradient(double[] actual, double[] predicted)
            {
                int n = actual.Length;
                double[] gradients = new double[n];
                for (int i = 0; i < n; i++)
                {
                    double error = predicted[i] - actual[i];
                    if (Math.Abs(error) <= delta)
                    {
                        gradients[i] = error;
                    }
                    else
                    {
                        gradients[i] = delta * Math.Sign(error);
                    }
                }
                return gradients;
            }
        }
        // 绝对误差损失函数
        public class AbsoluteErrorLoss : ILossFunction
        {
            public double[] Gradient(double[] actual, double[] predicted)
            {
                int n = actual.Length;
                double[] gradients = new double[n];
                for (int i = 0; i < n; i++)
                {
                    if (actual[i] > predicted[i])
                    {
                        gradients[i] = -1;
                    }
                    else if (actual[i] < predicted[i])
                    {
                        gradients[i] = 1;
                    }
                    else
                    {
                        gradients[i] = 0;
                    }
                }
                return gradients;
            }
        }
        // 分位数损失函数
        public class QuantileLoss : ILossFunction
        {
            private double tau;

            public QuantileLoss(double tau)
            {
                this.tau = tau;
            }

            public double[] Gradient(double[] actual, double[] predicted)
            {
                int n = actual.Length;
                double[] gradients = new double[n];
                for (int i = 0; i < n; i++)
                {
                    if (actual[i] >= predicted[i])
                    {
                        gradients[i] = -tau;
                    }
                    else
                    {
                        gradients[i] = 1 - tau;
                    }
                }
                return gradients;
            }
        }
        public class ManualGradientBoostingRegression
        {
            private int learners;
            private double learningRate;
            private int maxDepth;
            private int minSamplesSplit;
            private List<List<DecisionTree>> treesList;
            private double[] initialPredictions;
            private ILossFunction lossFunction;

            public ManualGradientBoostingRegression(int learners, double learningRate, int maxDepth, int minSamplesSplit, ILossFunction lossFunction)
            {
                this.learners = learners;
                this.learningRate = learningRate;
                this.maxDepth = maxDepth;
                this.minSamplesSplit = minSamplesSplit;
                this.treesList = new List<List<DecisionTree>>();
                this.lossFunction = lossFunction;
            }
            static DataTable ConvertToSingleRow(DataTable originalTable)
            {
                DataTable resultTable = new DataTable();

                // 复制列结构
                foreach (DataColumn column in originalTable.Columns)
                {
                    resultTable.Columns.Add(column.ColumnName, column.DataType);
                }

                // 创建新行
                DataRow newRow = resultTable.NewRow();

                // 计算每列的平均值
                for (int i = 0; i < originalTable.Columns.Count; i++)
                {
                    double sum = 0;
                    foreach (DataRow row in originalTable.Rows)
                    {
                        sum += Convert.ToDouble(row[i]);
                    }
                    double average = sum / originalTable.Rows.Count;
                    newRow[i] = average;
                }

                // 添加新行到结果表
                resultTable.Rows.Add(newRow);

                return resultTable;
            }
            static double[] ConvertDataTableToSingleRowArray(DataTable dataTable)
            {
                // 检查 DataTable 是否为空或行数不为 1 或列数不为 4
                if (dataTable == null || dataTable.Rows.Count != 1 || dataTable.Columns.Count != 4)
                {
                    return null;
                }

                // 获取第一行
                DataRow row = dataTable.Rows[0];

                // 创建一个大小为 4 的数组
                double[] array = new double[4];

                // 将 DataRow 中的数据复制到数组中
                for (int i = 0; i < 4; i++)
                {
                    //array[i] = row[i];
                    array[i] = Convert.ToDouble(row[i]);
                }

                return array;
            }
            public void Fit(double[][] inputs, double[][] outputs)
            {
                int rows = outputs.Length;
                int columns = outputs[0].Length;

                // 初始化预测值为目标值的均值
                initialPredictions = new double[columns];
                for (int col = 0; col < columns; col++)
                {
                    double sum = 0;
                    for (int i = 0; i < rows; i++)
                    {
                        sum += outputs[i][col];
                    }
                    initialPredictions[col] = sum / rows;
                }

                double[][] currentPredictions = new double[rows][];
                for (int i = 0; i < rows; i++)
                {
                    currentPredictions[i] = new double[columns];
                    Array.Copy(initialPredictions, currentPredictions[i], columns);
                }

                Parallel.For(0, columns, col =>
                {
                    var trees = new List<DecisionTree>();
                    double[] currentColPredictions = new double[rows];
                    double[] currentOutputs = new double[rows];
                    for (int i = 0; i < rows; i++)
                    {
                        currentColPredictions[i] = currentPredictions[i][col];
                        currentOutputs[i] = outputs[i][col];
                    }

                    for (int i = 0; i < learners; i++)
                    {
                        // 计算负梯度（残差），使用损失函数的梯度
                        double[] gradients = lossFunction.Gradient(currentOutputs, currentColPredictions);
                        double[] residuals = new double[rows];
                        for (int j = 0; j < rows; j++)
                        {
                            residuals[j] = -gradients[j];
                        }

                        // 训练决策树来拟合残差
                        var tree = new DecisionTree(inputs[0].Length, maxDepth, minSamplesSplit);
                        tree.Fit(inputs, residuals);
                        trees.Add(tree);

                        // 更新当前预测值
                        for (int j = 0; j < rows; j++)
                        {
                            currentColPredictions[j] += learningRate * tree.Predict(inputs[j]);
                        }
                    }

                    lock (treesList)
                    {
                        treesList.Add(trees);
                    }
                });
            }
            private double[][] ConvertDataTableToDoubleArray(DataTable inputTable)
            {
                int rows = inputTable.Rows.Count;
                int columns = inputTable.Columns.Count;

                double[][] array = new double[rows][];
                for (int i = 0; i < rows; i++)
                {
                    array[i] = new double[columns];
                    for (int j = 0; j < columns; j++)
                    {
                        array[i][j] = inputTable.Rows[i].Field<double>(j);
                    }
                }

                return array;
            }
            public DataTable Predict(double[][] inputs)
            {
                int rows = inputs.Length;
                int columns = initialPredictions.Length;

                DataTable predictionsTable = new DataTable();
                for (int col = 0; col < columns; col++)
                {
                    predictionsTable.Columns.Add($"Prediction{col + 1}", typeof(double));
                }

                for (int i = 0; i < rows; i++)
                {
                    DataRow row = predictionsTable.NewRow();
                    for (int col = 0; col < columns; col++)
                    {
                        double prediction = initialPredictions[col];
                        foreach (var tree in treesList[col])
                        {
                            prediction += learningRate * tree.Predict(inputs[i]);
                        }
                        row[col] = prediction;
                    }
                    predictionsTable.Rows.Add(row);
                }

                return predictionsTable;
            }
            private double[][] CombineInputTables(List<DataTable> inputTables)
            {
                int rows = inputTables[0].Rows.Count;
                int totalColumns = inputTables.Sum(table => table.Columns.Count);

                double[][] combinedInputs = new double[rows][];
                for (int i = 0; i < rows; i++)
                {
                    combinedInputs[i] = new double[totalColumns];
                    int colIndex = 0;
                    foreach (var table in inputTables)
                    {
                        for (int col = 0; col < table.Columns.Count; col++)
                        {
                            combinedInputs[i][colIndex++] = table.Rows[i].Field<double>(col);
                        }
                    }
                }

                return combinedInputs;
            }
        }

        public class DecisionTree
        {
            private int featureCount;
            private int maxDepth;
            private int minSamplesSplit;
            private TreeNode root;

            public DecisionTree(int featureCount, int maxDepth, int minSamplesSplit)
            {
                this.featureCount = featureCount;
                this.maxDepth = maxDepth;
                this.minSamplesSplit = minSamplesSplit;
            }

            public void Fit(double[][] inputs, double[] outputs)
            {
                root = BuildTree(inputs, outputs, 0);
            }
            private TreeNode BuildTree(double[][] inputs, double[] outputs, int depth)
            {
                if (inputs.Length < minSamplesSplit || depth >= maxDepth)
                {
                    return new TreeNode { Value = outputs.Average() };
                }

                int bestFeature = -1;
                double bestThreshold = 0;
                double bestMse = double.MaxValue;

                // 随机选择部分特征
                var random = new Random();
                var selectedFeatures = Enumerable.Range(0, featureCount)
                                                 .OrderBy(x => random.Next())
                                                 .Take((int)Math.Sqrt(featureCount))
                                                 .ToList();

                foreach (int featureIndex in selectedFeatures)
                {
                    // 对特征进行排序以减少阈值计算
                    var sortedIndices = Enumerable.Range(0, inputs.Length)
                                                  .OrderBy(i => inputs[i][featureIndex])
                                                  .ToList();
                    var thresholds = new List<double>();
                    for (int i = 0; i < sortedIndices.Count - 1; i++)
                    {
                        if (inputs[sortedIndices[i]][featureIndex] != inputs[sortedIndices[i + 1]][featureIndex])
                        {
                            thresholds.Add((inputs[sortedIndices[i]][featureIndex] + inputs[sortedIndices[i + 1]][featureIndex]) / 2);
                        }
                    }

                    foreach (var threshold in thresholds)
                    {
                        var leftIndices = new List<int>();
                        var rightIndices = new List<int>();
                        for (int i = 0; i < inputs.Length; i++)
                        {
                            if (inputs[i][featureIndex] < threshold)
                            {
                                leftIndices.Add(i);
                            }
                            else
                            {
                                rightIndices.Add(i);
                            }
                        }

                        if (leftIndices.Count == 0 || rightIndices.Count == 0)
                        {
                            continue;
                        }

                        double leftMse = CalculateMse(outputs.Where((_, i) => leftIndices.Contains(i)).ToArray());
                        double rightMse = CalculateMse(outputs.Where((_, i) => rightIndices.Contains(i)).ToArray());
                        double mse = (leftMse * leftIndices.Count + rightMse * rightIndices.Count) / outputs.Length;

                        if (mse < bestMse)
                        {
                            bestMse = mse;
                            bestFeature = featureIndex;
                            bestThreshold = threshold;
                        }
                    }
                }

                if (bestFeature == -1)
                {
                    return new TreeNode { Value = outputs.Average() };
                }

                var leftInputs = new List<double[]>();
                var leftOutputs = new List<double>();
                var rightInputs = new List<double[]>();
                var rightOutputs = new List<double>();
                for (int i = 0; i < inputs.Length; i++)
                {
                    if (inputs[i][bestFeature] < bestThreshold)
                    {
                        leftInputs.Add(inputs[i]);
                        leftOutputs.Add(outputs[i]);
                    }
                    else
                    {
                        rightInputs.Add(inputs[i]);
                        rightOutputs.Add(outputs[i]);
                    }
                }

                return new TreeNode
                {
                    FeatureIndex = bestFeature,
                    Threshold = bestThreshold,
                    Left = BuildTree(leftInputs.ToArray(), leftOutputs.ToArray(), depth + 1),
                    Right = BuildTree(rightInputs.ToArray(), rightOutputs.ToArray(), depth + 1)
                };
            }
            private double CalculateMse(double[] values)
            {
                if (values.Length == 0)
                {
                    return 0;
                }
                double mean = values.Average();
                return values.Sum(x => Math.Pow(x - mean, 2)) / values.Length;
            }

            public double Predict(double[] input)
            {
                return TraverseTree(root, input);
            }

            private double TraverseTree(TreeNode node, double[] input)
            {
                if (node.IsLeaf)
                {
                    return node.Value;
                }
                if (input[node.FeatureIndex] < node.Threshold)
                {
                    return TraverseTree(node.Left, input);
                }
                else
                {
                    return TraverseTree(node.Right, input);
                }
            }
        }

        public class TreeNode
        {
            public int FeatureIndex { get; set; }
            public double Threshold { get; set; }
            public double Value { get; set; }
            public TreeNode Left { get; set; }
            public TreeNode Right { get; set; }

            public bool IsLeaf => Left == null && Right == null;
        }
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
        public GBRForm()
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
        static DataTable ConvertColumns1ToDouble(DataTable originalTable)
        {
            // 创建一个新的 DataTable
            DataTable newTable = new DataTable();

            // 遍历原始 DataTable 的每一列
            foreach (DataColumn column in originalTable.Columns)
            {
                // 在新 DataTable 中添加同名的 double 类型列
                newTable.Columns.Add(column.ColumnName, typeof(double));
            }

            // 遍历原始 DataTable 的每一行
            foreach (DataRow row in originalTable.Rows)
            {
                // 创建一个新的 DataRow
                DataRow newRow = newTable.NewRow();

                // 遍历每一列，将数据转换为 double 类型并存储到新行中
                for (int i = 0; i < originalTable.Columns.Count; i++)
                {
                    // 尝试将数据转换为 double 类型
                    if (double.TryParse(row[i].ToString(), out double result))
                    {
                        newRow[i] = result;
                    }
                    else
                    {
                        // 如果转换失败，可以设置默认值或抛出异常
                        newRow[i] = 0.0; // 这里设置为 0.0 作为默认值
                    }
                }

                // 将新行添加到新的 DataTable 中
                newTable.Rows.Add(newRow);
            }

            return newTable;
        }
        private (DataTable input, DataTable output) PrepareDataTables(DataTable inputDataTable1, DataTable inputDataTable2)
        {
            // List<DataTable> inputDataTables = new List<DataTable> { inputDataTable1, inputDataTable1 };
            return (inputDataTable1, inputDataTable2);
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
                ModelTrainingInfo.GBRLearningrate = double.Parse(LearningrateTxt.Text.ToString());
                ModelTrainingInfo.GBRNumberoflearners = double.Parse(NumberoflearnersTxt.Text.ToString());
                ModelTrainingInfo.GBRMinimumpartitionnode = double.Parse(MinimumpartitionnodeTxt.Text.ToString());
                ModelTrainingInfo.GBRMaximumdepth = double.Parse(MaximumdepthTxt.Text.ToString());


                // 超参数设置
                int learners = int.Parse(ModelTrainingInfo.GBRNumberoflearners.ToString());
                double learningRate = ModelTrainingInfo.GBRLearningrate;
                int maxDepth = int.Parse(ModelTrainingInfo.GBRMaximumdepth.ToString());
                int minSamplesSplit = int.Parse(ModelTrainingInfo.GBRMinimumpartitionnode.ToString());


                // 假设这里有一个 DataTable 对象
                List<DataTable> dataTables = DataSelectInfo.outputsdatatable;
                List<DataTable> inputdataTables = DataSelectInfo.inputsdatatable;
                // 创建一个新的列表并添加第一个列表的所有元素
                List<DataTable> combinedTables = new List<DataTable>(dataTables);
                // 添加第二个列表的所有元素
                combinedTables.AddRange(inputdataTables);
                DataTable mergedTable = DataTableMerger.MergeAllTables(combinedTables);
                var (table90, table10) = DataTableMerger.SplitDataTable(mergedTable);
                // 准备输入和输出的 DataTable
                var (inputDataTables, outputDataTable) = PrepareDataTables(table90, table10);
                // 返回 DataTable 类型的实测结果
                ModelTrainingInfo.actualDataTable = table10;
                // 预测，返回 DataTable 类型的预测结果
                //ModelTrainingInfo.outputsdatatable = table90;
                double[][] inputs = inputDataTables.ToJagged<double>();
                // 将输出 DataTable 转换为输出数组
                double[][] outputs = outputDataTable.ToJagged<double>();
                //// 提前转换数据
                //double[][] inputs = ConvertDataTableToDoubleArray(outputTable);
                //double[][] outputs = ConvertDataTableToDoubleArray(outputTable);
                DataTable predictionsTable = new DataTable();
                double rSquared = 0;

                // 数据采样
              //  int sampleSize = Math.Min(100, inputs.Length); // 采样 100 个样本
                int sampleSize = Math.Min(50, inputs.Length); // 采样 100 个样本
                double[][] sampledInputs = new double[sampleSize][];
                double[][] sampledOutputs = new double[sampleSize][];
                Random random = new Random();
                for (int i = 0; i < sampleSize; i++)
                {
                    int index = random.Next(0, inputs.Length);
                    sampledInputs[i] = inputs[index];
                    sampledOutputs[i] = outputs[index];
                }

                // 在后台线程中执行 Fit 方法
                await Task.Run(() =>
                {
                    try
                    {
                        if (ModelTrainingInfo.GBRLossfunction == "squared_er")
                        {
                            // 手动创建 GBR 模型
                            var gbrModel = new ManualGradientBoostingRegression(learners, learningRate, maxDepth, minSamplesSplit, new SquaredErrorLoss());
                            gbrModel.Fit(sampledInputs, sampledOutputs);
                            // 进行预测，并获取预测值的 DataTable
                            predictionsTable = gbrModel.Predict(inputs);

                            // 手动计算决定系数 (R^2)
                            // 计算整个 DataTable 的决定系数 (R^2)
                             rSquared = CalculateRSquaredForDataTable(outputDataTable, predictionsTable);
                        }
                        if (ModelTrainingInfo.GBRLossfunction == "huber")
                        {
                            // 手动创建GBR模型，默认使用均方误差损失函数
                            double huberDelta = 1.0; // Huber 损失函数的 delta 参数
                            var gbrModel = new ManualGradientBoostingRegression(learners, learningRate, maxDepth, minSamplesSplit, new HuberLoss(huberDelta));
                            gbrModel.Fit(sampledInputs, sampledOutputs);


                            // 进行预测，并获取预测值的 DataTable
                            predictionsTable = gbrModel.Predict(inputs);

                            // 手动计算决定系数 (R^2)
                            // 计算整个 DataTable 的决定系数 (R^2)
                            rSquared = CalculateRSquaredForDataTable(outputDataTable, predictionsTable);
                        }
                        if (ModelTrainingInfo.GBRLossfunction == "absolute_err")
                        {
                            // 手动创建GBR模型，默认使用均方误差损失函数
                            var gbrModel = new ManualGradientBoostingRegression(learners, learningRate, maxDepth, minSamplesSplit, new AbsoluteErrorLoss());
                            gbrModel.Fit(sampledInputs, sampledOutputs);


                            // 进行预测，并获取预测值的 DataTable
                            predictionsTable = gbrModel.Predict(inputs);

                            // 计算整个 DataTable 的决定系数 (R^2)
                            rSquared = CalculateRSquaredForDataTable(outputDataTable, predictionsTable);
                        }
                        if (ModelTrainingInfo.GBRLossfunction == "quantile")
                        {
                            // 手动创建GBR模型，默认使用absolute_err绝对误差损失函数
                            double quantile = 0.5; // 默认分位数为 0.5，即中位数
                                                   // 手动创建 GBR 模型，默认使用quantile分位数损失函数
                            var gbrModel = new ManualGradientBoostingRegression(learners, learningRate, maxDepth, minSamplesSplit, new QuantileLoss(quantile));
                            gbrModel.Fit(sampledInputs, sampledOutputs);


                            // 进行预测，并获取预测值的 DataTable
                            predictionsTable = gbrModel.Predict(inputs);

                            // 计算整个 DataTable 的决定系数 (R^2)
                            rSquared = CalculateRSquaredForDataTable(outputDataTable, predictionsTable);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error in Fit method: {ex.Message}");
                    }
                });

                //// 返回 DataTable 类型的实测结果
                //ModelTrainingInfo.actualDataTable = outputDataTable;
                //// 预测，返回 DataTable 类型的预测结果
                ModelTrainingInfo.outputsdatatable = predictionsTable;

                rSquared = Math.Abs(rSquared);
                string formatted = Math.Round(rSquared, 3).ToString();
                //var bigValue = new DenseVector(new[] { rSquared });

                //// 获取数值的科学计数法表示
                //string scientific = bigValue[0].ToString("E3");

                //// 使用正则表达式提取尾数部分
                //string pattern = @"^[-+]?(\d+\.\d+)E[+-]\d+$";
                //Match match = Regex.Match(scientific, pattern);

                //string formatted;
                //if (match.Success)
                //{
                //    // 提取并格式化尾数部分，保留3位小数
                //    formatted = match.Groups[1].Value;
                //    // 确保有3位小数
                //    formatted = double.Parse(formatted).ToString("F3");
                //    // 添加符号
                //    formatted = (bigValue[0] < 0 ? "-" : "") + formatted;
                //}
                //else
                //{
                //    // 如果正则匹配失败，使用默认格式化
                //    formatted = bigValue[0].ToString("F3");
                //}
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
        private  void OKBtn_Click(object sender, EventArgs e)
        {
            if (DataSelectInfo.inputsdatatable.Count > 0 && DataSelectInfo.outputsdatatable.Count > 0)
            {
                ModelTrainingInfo.GBRLearningrate = double.Parse(LearningrateTxt.Text.ToString());
                ModelTrainingInfo.GBRNumberoflearners = double.Parse(NumberoflearnersTxt.Text.ToString());
                ModelTrainingInfo.GBRMinimumpartitionnode = double.Parse(MinimumpartitionnodeTxt.Text.ToString());
                ModelTrainingInfo.GBRMaximumdepth = double.Parse(MaximumdepthTxt.Text.ToString());

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
                //ResultInfo.gbrelapsedSeconds = elapsedSeconds;
            }
            else
            {
                MessageBox.Show("请先进行数据读取！！！");
            }
        }
        private double[][] ConvertDataTableToDoubleArray(DataTable inputTable)
        {
            int rows = inputTable.Rows.Count;
            int columns = inputTable.Columns.Count;

            double[][] array = new double[rows][];
            for (int i = 0; i < rows; i++)
            {
                array[i] = new double[columns];
                for (int j = 0; j < columns; j++)
                {
                    array[i][j] = inputTable.Rows[i].Field<double>(j);
                }
            }

            return array;
        }
        // 计算整个 DataTable 的决定系数 (R^2) 的方法
        private double CalculateRSquaredForDataTable(DataTable actualValuesTable, DataTable predictedValuesTable)
        {
            if (actualValuesTable.Rows.Count != predictedValuesTable.Rows.Count || actualValuesTable.Columns.Count != predictedValuesTable.Columns.Count)
            {
                throw new ArgumentException("实际值和预测值的 DataTable 必须具有相同的行数和列数。");
            }

            // 将 DataTable 转换为数组
            List<double> actualValues = new List<double>();
            List<double> predictedValues = new List<double>();

            for (int col = 0; col < actualValuesTable.Columns.Count; col++)
            {
                for (int row = 0; row < actualValuesTable.Rows.Count; row++)
                {
                    actualValues.Add(Convert.ToDouble(actualValuesTable.Rows[row][col]));
                    predictedValues.Add(Convert.ToDouble(predictedValuesTable.Rows[row][col]));
                }
            }

            // 调用 CalculateRSquared 方法计算 R^2
            return CalculateRSquared(actualValues.ToArray(), predictedValues.ToArray());
        }

        // 计算决定系数 (R^2) 的方法
        private double CalculateRSquared(double[] actualValues, double[] predictedValues)
        {
            if (actualValues.Length != predictedValues.Length)
            {
                throw new ArgumentException("实际值和预测值的长度必须相同。");
            }

            int n = actualValues.Length;
            double sumOfSquaredResiduals = 0; // 残差平方和
            double sumOfSquaredTotal = 0; // 总平方和
            double meanActual = actualValues.Average(); // 实际值的均值

            // 计算残差平方和和总平方和
            for (int i = 0; i < n; i++)
            {
                double residual = actualValues[i] - predictedValues[i];
                sumOfSquaredResiduals += residual * residual;

                double deviation = actualValues[i] - meanActual;
                sumOfSquaredTotal += deviation * deviation;
            }

            // 计算决定系数
            if (Math.Abs(sumOfSquaredTotal) < double.Epsilon)
            {
                return 1; // 所有实际值相同，完美拟合
            }
            return 1 - (sumOfSquaredResiduals / sumOfSquaredTotal);
        }
        private void CancelBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void squared_erRbtn_CheckedChanged(object sender, EventArgs e)
        {
            ModelTrainingInfo.GBRLossfunction = squared_erRbtn.Text;
        }

        private void huberRbtn_CheckedChanged(object sender, EventArgs e)
        {
            ModelTrainingInfo.GBRLossfunction = huberRbtn.Text;
        }

        private void absolute_errRbtn_CheckedChanged(object sender, EventArgs e)
        {
            ModelTrainingInfo.GBRLossfunction = absolute_errRbtn.Text;
        }

        private void quantileRbtn_CheckedChanged(object sender, EventArgs e)
        {
            ModelTrainingInfo.GBRLossfunction = quantileRbtn.Text;
        }

        private void GBRForm_Load(object sender, EventArgs e)
        {
            ModelTrainingInfo.GBRLossfunction = squared_erRbtn.Text;
        }
    }
}