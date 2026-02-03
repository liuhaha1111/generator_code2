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
    public partial class RFRForm : Form
    {
        // 决策树节点类
        class DecisionTreeNode
        {
            public int FeatureIndex { get; set; }
            public double Threshold { get; set; }
            public DecisionTreeNode Left { get; set; }
            public DecisionTreeNode Right { get; set; }
            public int Label { get; set; }
            public bool IsLeaf { get; set; }
        }

        // 决策树类
        class DecisionTree
        {
            private DecisionTreeNode root;
            private int maxDepth;

            public DecisionTree(int maxDepth)
            {
                this.maxDepth = maxDepth;
            }

            public void Train(double[][] inputs, int[] outputs)
            {
                root = BuildTree(inputs, outputs, 0);
            }

            private DecisionTreeNode BuildTree(double[][] inputs, int[] outputs, int depth)
            {
                if (depth >= maxDepth || outputs.Distinct().Count() == 1)
                {
                    return new DecisionTreeNode
                    {
                        IsLeaf = true,
                        Label = outputs.GroupBy(x => x).OrderByDescending(g => g.Count()).First().Key
                    };
                }

                int bestFeatureIndex = 0;
                double bestThreshold = 0;
                double bestGini = double.MaxValue;

                for (int featureIndex = 0; featureIndex < inputs[0].Length; featureIndex++)
                {
                    var thresholds = inputs.Select(x => x[featureIndex]).Distinct().ToList();
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

                        double gini = CalculateGini(leftIndices.Select(i => outputs[i]).ToArray(), rightIndices.Select(i => outputs[i]).ToArray());
                        if (gini < bestGini)
                        {
                            bestGini = gini;
                            bestFeatureIndex = featureIndex;
                            bestThreshold = threshold;
                        }
                    }
                }

                var leftIndicesFinal = new List<int>();
                var rightIndicesFinal = new List<int>();
                for (int i = 0; i < inputs.Length; i++)
                {
                    if (inputs[i][bestFeatureIndex] < bestThreshold)
                    {
                        leftIndicesFinal.Add(i);
                    }
                    else
                    {
                        rightIndicesFinal.Add(i);
                    }
                }

                return new DecisionTreeNode
                {
                    IsLeaf = false,
                    FeatureIndex = bestFeatureIndex,
                    Threshold = bestThreshold,
                    Left = BuildTree(leftIndicesFinal.Select(i => inputs[i]).ToArray(), leftIndicesFinal.Select(i => outputs[i]).ToArray(), depth + 1),
                    Right = BuildTree(rightIndicesFinal.Select(i => inputs[i]).ToArray(), rightIndicesFinal.Select(i => outputs[i]).ToArray(), depth + 1)
                };
            }

            private double CalculateGini(int[] leftOutputs, int[] rightOutputs)
            {
                double leftGini = CalculateGiniImpurity(leftOutputs);
                double rightGini = CalculateGiniImpurity(rightOutputs);
                double total = leftOutputs.Length + rightOutputs.Length;
                return (leftOutputs.Length / total) * leftGini + (rightOutputs.Length / total) * rightGini;
            }

            private double CalculateGiniImpurity(int[] outputs)
            {
                if (outputs.Length == 0)
                {
                    return 0;
                }
                var labelCounts = outputs.GroupBy(x => x).ToDictionary(g => g.Key, g => g.Count());
                double impurity = 1;
                foreach (var count in labelCounts.Values)
                {
                    double probability = (double)count / outputs.Length;
                    impurity -= probability * probability;
                }
                return impurity;
            }

            public int Predict(double[] input)
            {
                var currentNode = root;
                while (!currentNode.IsLeaf)
                {
                    if (input[currentNode.FeatureIndex] < currentNode.Threshold)
                    {
                        currentNode = currentNode.Left;
                    }
                    else
                    {
                        currentNode = currentNode.Right;
                    }
                }
                return currentNode.Label;
            }
        }

        // 随机森林类
        class RandomForest
        {
            private List<DecisionTree> trees;
            private int numberOfTrees;
            private double sampleRatio;
            private int maxDepth;

            public RandomForest(int numberOfTrees, double sampleRatio, int maxDepth)
            {
                this.numberOfTrees = numberOfTrees;
                this.sampleRatio = sampleRatio;
                this.maxDepth = maxDepth;
                trees = new List<DecisionTree>();
            }

            public void AddTree(DecisionTree tree)
            {
                trees.Add(tree);
            }

            public void Train(double[][] inputs, int[] outputs)
            {
                var random = new Random();
                for (int i = 0; i < numberOfTrees; i++)
                {
                    var tree = new DecisionTree(maxDepth);
                    var sampleIndices = Enumerable.Range(0, inputs.Length).OrderBy(x => random.Next()).Take((int)(inputs.Length * sampleRatio)).ToList();
                    var sampleInputs = sampleIndices.Select(index => inputs[index]).ToArray();
                    var sampleOutputs = sampleIndices.Select(index => outputs[index]).ToArray();
                    tree.Train(sampleInputs, sampleOutputs);
                    trees.Add(tree);
                }
            }

            public int Decide(double[] input)
            {
                var predictions = trees.Select(tree => tree.Predict(input)).ToList();
                return predictions.GroupBy(x => x).OrderByDescending(g => g.Count()).First().Key;
            }
        }

        // 原代码中的其他辅助方法保持不变
        class DataHelper
        {
            // 合并多个 DataTable 为一个二维数组
            public static double[][] CombineDataTables(System.Data.DataTable[] dataTables)
            {
                int numRows = dataTables[0].Rows.Count;
                int totalColumns = dataTables.Sum(dt => dt.Columns.Count);

                double[][] combinedInputs = new double[numRows][];
                for (int i = 0; i < numRows; i++)
                {
                    combinedInputs[i] = new double[totalColumns];
                    int currentIndex = 0;
                    foreach (System.Data.DataTable dt in dataTables)
                    {
                        for (int j = 0; j < dt.Columns.Count; j++)
                        {
                            try
                            {
                                combinedInputs[i][currentIndex++] = Convert.ToDouble(dt.Rows[i][j]);
                            }
                            catch (FormatException)
                            {
                                Console.WriteLine($"无法将单元格 {dt.Columns[j].ColumnName} 的值 '{dt.Rows[i][j]}' 转换为 double 类型。");
                                combinedInputs[i][currentIndex++] = double.NaN;
                            }
                        }
                    }
                }

                return combinedInputs;
            }

            // 将 DataTable 转换为一维数组
            public static double[] ConvertDataTableToArray(System.Data.DataTable dataTable)
            {
                return dataTable.AsEnumerable().Select(row =>
                {
                    try
                    {
                        return Convert.ToDouble(row[0]);
                    }
                    catch (FormatException)
                    {
                        Console.WriteLine($"无法将单元格 {dataTable.Columns[0].ColumnName} 的值 '{row[0]}' 转换为 double 类型。");
                        return double.NaN;
                    }
                }).ToArray();
            }

            // 计算 R² 值
            public static double CalculateR2(double[] actual, double[] predicted)
            {
                double meanActual = actual.Average();
                double ssTotal = actual.Sum(x => Math.Pow(x - meanActual, 2));
                double ssResidual = actual.Zip(predicted, (x, y) => Math.Pow(x - y, 2)).Sum();
                return 1 - (ssResidual / ssTotal);
            }
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
        public RFRForm()
        {
            InitializeComponent();
            // 设置双缓冲以减少闪烁
            this.DoubleBuffered = true;
            // 初始化定时器
            animationTimer = new Timer();
            animationTimer.Interval = AnimationSpeed;
            animationTimer.Tick += Timer_Tick;
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
            using (LoadingOverlayForm loadingOverlay = new LoadingOverlayForm(this))
            {
                loadingOverlay.Show();
                btnToggleControls(false);
                int numberOfLearners = (int)double.Parse(NumberoflearnersTxt.Text);
                double sampleRatio = double.Parse(NumberoflearnersTxt.Text);
                int maxDepth = (int)double.Parse(NumberoflearnersTxt.Text);

                ModelTrainingInfo.RFRNumberoflearners = numberOfLearners;
                ModelTrainingInfo.RFRMinimumpartitionnode = sampleRatio;
                ModelTrainingInfo.RFRMaximumdepth = maxDepth;
                #region hide by wpz 
                //try
                //{
                //    // 提前解析输入值
                //    int numberOfLearners = (int)double.Parse(NumberoflearnersTxt.Text);
                //    double sampleRatio = double.Parse(NumberoflearnersTxt.Text);
                //    int maxDepth = (int)double.Parse(NumberoflearnersTxt.Text);

                //    ModelTrainingInfo.RFRNumberoflearners = numberOfLearners;
                //    ModelTrainingInfo.RFRMinimumpartitionnode = sampleRatio;
                //    ModelTrainingInfo.RFRMaximumdepth = maxDepth;
                //    // 假设这里有一个 DataTable 对象
                //    List<DataTable> dataTables = DataSelectInfo.outputsdatatable;
                //    List<DataTable> inputdataTables = DataSelectInfo.inputsdatatable;
                //    // 创建一个新的列表并添加第一个列表的所有元素
                //    List<DataTable> combinedTables = new List<DataTable>(dataTables);
                //    // 添加第二个列表的所有元素
                //    combinedTables.AddRange(inputdataTables);
                //    DataTable mergedTable = DataTableMerger.MergeAllTables(combinedTables);
                //    var (table90, table10) = DataTableMerger.SplitDataTable(mergedTable);
                //    // 准备输入和输出的 DataTable
                //    var (inputDataTables, outputDataTable) = PrepareDataTables(table90, table10);
                //    // 返回 DataTable 类型的实测结果
                //    ModelTrainingInfo.actualDataTable = table10;
                //    // 预测，返回 DataTable 类型的预测结果
                //    //ModelTrainingInfo.outputsdatatable = table90;

                //    double[][] inputs = inputDataTables.ToJagged<double>();
                //    int[] outputs = DataHelper.ConvertDataTableToArray(outputDataTable).Select(o => (int)o).ToArray();
                //    //MessageBox.Show("请先进行数据读取！！！");
                //    // 训练模型并计算R²
                //    var rfr = await TrainRFRAsync(inputs, outputs, numberOfLearners, sampleRatio, maxDepth);
                //    double rfrR2 = DataHelper.CalculateR2(outputs.Select(o => (double)o).ToArray(), inputs.Select(input => (double)rfr.Decide(input)).ToArray());

                //    // 并行预测，返回 DataTable 类型的预测结果
                //    System.Data.DataTable rfrPredictions = await PredictRFRParallelAsync(rfr, inputs);
                //    ModelTrainingInfo.outputsdatatable = rfrPredictions;
                //    //ModelTrainingInfo.actualDataTable = outputDataTable;
                //    rfrR2 = Math.Abs(rfrR2);
                //    string formatted = Math.Round(rfrR2, 3).ToString();
                //    ModelTrainingInfo.trainingscore = formatted;


                //    await SimulateLoadingAsync(); // 模拟加载
                //}
                //catch (Exception ex)
                //{
                //    MessageBox.Show($"加载时出错: {ex.Message}");
                //}
                //finally
                //{
                //    isLoading = false;
                //    btnToggleControls(true);
                //    loadingOverlay.Close();
                //}
                #endregion
            }
            this.Close();
        }
        private (DataTable input, DataTable output) PrepareDataTables(DataTable inputDataTable1, DataTable inputDataTable2)
        {
            // List<DataTable> inputDataTables = new List<DataTable> { inputDataTable1, inputDataTable1 };
            return (inputDataTable1, inputDataTable2);
        }
        private async void OKBtn_Click(object sender, EventArgs e)
        {
            if (DataSelectInfo.inputsdatatable.Count > 0 && DataSelectInfo.outputsdatatable.Count > 0)
            {
                #region hide by wpz 2025/9/27
                // 创建一个新的Stopwatch实例
                //Stopwatch stopwatch = new Stopwatch();
                //// 开始计时
                //stopwatch.Start();

                //// 执行你想要测量时间的操作
                PerformSomeOperation();
                //// 停止计时
                //stopwatch.Stop();

                //// 获取操作花费的时间（以秒为单位）
                //double elapsedSeconds = stopwatch.Elapsed.TotalSeconds;
                //elapsedSeconds = Math.Round(elapsedSeconds, 3);
                //ResultInfo.rfrelapsedSeconds = elapsedSeconds;
                #endregion
            }
            else
            {
                MessageBox.Show("请先进行数据读取！！！");
            }
        }

        private async Task<RandomForest> TrainRFRAsync(double[][] inputs, int[] outputs, int numberOfLearners, double sampleRatio, int maxDepth)
        {
            return await Task.Run(() =>
            {
                var rfr = new RandomForest(
                    numberOfTrees: numberOfLearners,
                    sampleRatio: sampleRatio,
                    maxDepth: maxDepth
                );

                // 并行训练树
                Parallel.For(0, numberOfLearners, i =>
                {
                    var random = new Random(i); // 使用不同的种子确保随机性
                    var tree = new DecisionTree(maxDepth);
                    var sampleIndices = Enumerable.Range(0, inputs.Length).OrderBy(x => random.Next()).Take((int)(inputs.Length * sampleRatio)).ToList();
                    var sampleInputs = new double[sampleIndices.Count][];
                    var sampleOutputs = new int[sampleIndices.Count];
                    for (int j = 0; j < sampleIndices.Count; j++)
                    {
                        sampleInputs[j] = inputs[sampleIndices[j]];
                        sampleOutputs[j] = outputs[sampleIndices[j]];
                    }
                    tree.Train(sampleInputs, sampleOutputs);
                    rfr.AddTree(tree);
                });

                return rfr;
            });
        }

        private async Task<System.Data.DataTable> PredictRFRParallelAsync(RandomForest forest, double[][] inputs)
        {
            return await Task.Run(() =>
            {
                System.Data.DataTable result = new System.Data.DataTable();

                // 添加列，每一列对应一个特征的预测值
                for (int i = 0; i < inputs[0].Length; i++)
                {
                    result.Columns.Add($"Feature_{i}_Prediction", typeof(double));
                }

                // 并行预测
                var predictions = inputs.AsParallel().Select(input => (double)forest.Decide(input)).ToArray();

                // 将预测值添加到 DataTable
                for (int i = 0; i < inputs.Length; i++)
                {
                    var row = result.NewRow();
                    for (int j = 0; j < inputs[0].Length; j++)
                    {
                        row[$"Feature_{j}_Prediction"] = predictions[i];
                    }
                    result.Rows.Add(row);
                }

                return result;
            });
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

        private void CancelBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}