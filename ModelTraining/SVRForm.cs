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
    public partial class SVRForm : Form
    {
        // 定义 SVR 类
        class SVR
        {
            private double[][] X;
            private double[] y;
            private double C; // 正则化参数
            private double epsilon = 0.1;
            private double tol; // 容忍度
            private int maxIter; // 最大迭代次数
            private double[] alphas;
            private double b;
            private Func<double[], double[], double> kernel;
            private Random random = new Random();
            private double gamma;
            private double coef0;
            private int degree;
            private double[][] precomputedKernelMatrix;

            // 新增核函数类型参数，默认值为 "rbf"
            public SVR(double[][] X, double[] y, double C = 1.0, double tol = 0.001, int maxIter = 100, double gamma = 0.1, string kernelType = "rbf", double coef0 = 0, int degree = 3, double[][] precomputedKernelMatrix = null)
            {
                this.X = X;
                this.y = y;
                this.C = C;
                this.tol = tol;
                this.maxIter = maxIter;
                this.alphas = new double[X.Length];
                this.b = 0;
                this.gamma = gamma;
                this.coef0 = coef0;
                this.degree = degree;
                this.precomputedKernelMatrix = precomputedKernelMatrix;

                // 根据核函数类型选择不同的核函数
                switch (kernelType.ToLower())
                {
                    case "rbf":
                        // 使用高斯核函数
                        this.kernel = (x1, x2) =>
                        {
                            double norm = 0;
                            for (int i = 0; i < x1.Length; i++)
                            {
                                double diff = x1[i] - x2[i];
                                norm += diff * diff;
                            }
                            return Math.Exp(-gamma * norm);
                        };
                        break;
                    case "linear":
                        // 线性核函数
                        this.kernel = (x1, x2) =>
                        {
                            double dotProduct = 0;
                            for (int i = 0; i < x1.Length; i++)
                            {
                                dotProduct += x1[i] * x2[i];
                            }
                            return dotProduct;
                        };
                        break;
                    case "poly":
                        // 多项式核函数
                        this.kernel = (x1, x2) =>
                        {
                            double dotProduct = 0;
                            for (int i = 0; i < x1.Length; i++)
                            {
                                dotProduct += x1[i] * x2[i];
                            }
                            return Math.Pow(gamma * dotProduct + coef0, degree);
                        };
                        break;
                    case "sigmoid":
                        // Sigmoid 核函数
                        this.kernel = (x1, x2) =>
                        {
                            double dotProduct = 0;
                            for (int i = 0; i < x1.Length; i++)
                            {
                                dotProduct += x1[i] * x2[i];
                            }
                            return Math.Tanh(gamma * dotProduct + coef0);
                        };
                        break;
                    case "precomputed":
                        if (precomputedKernelMatrix == null)
                        {
                            throw new ArgumentException("使用预计算核函数时，必须提供预计算的核矩阵。");
                        }
                        if (precomputedKernelMatrix.Length != X.Length || precomputedKernelMatrix[0].Length != X.Length)
                        {
                            throw new ArgumentException("预计算的核矩阵的维度必须与输入数据的样本数量匹配。");
                        }
                        this.kernel = (x1, x2) =>
                        {
                            int index1 = Array.IndexOf(X, x1);
                            int index2 = Array.IndexOf(X, x2);
                            return precomputedKernelMatrix[index1][index2];
                        };
                        break;
                    default:
                        throw new ArgumentException("不支持的核函数类型。支持的类型有: rbf, linear, poly, sigmoid, precomputed");
                }
            }

            public void Train()
            {
                int iter = 0;
                bool entireSet = true;
                int alphaPairsChanged = 0;

                while (iter < maxIter && (alphaPairsChanged > 0 || entireSet))
                {
                    alphaPairsChanged = 0;
                    if (entireSet)
                    {
                        for (int i = 0; i < y.Length; i++)
                        {
                            alphaPairsChanged += TakeStep(i);
                        }
                    }
                    else
                    {
                        for (int i = 0; i < y.Length; i++)
                        {
                            if (alphas[i] > 0 && alphas[i] < C)
                            {
                                alphaPairsChanged += TakeStep(i);
                            }
                        }
                    }

                    iter++;
                    if (entireSet)
                    {
                        entireSet = false;
                    }
                    else if (alphaPairsChanged == 0)
                    {
                        entireSet = true;
                    }
                }
            }

            private int TakeStep(int i1)
            {
                double E1 = Predict(X[i1]) - y[i1];
                if ((y[i1] * E1 < -tol && alphas[i1] < C) || (y[i1] * E1 > tol && alphas[i1] > 0))
                {
                    int i2 = SelectJrand(i1, y.Length);
                    
                    double E2 = Predict(X[i2]) - y[i2];
                    double alphaI1Old = alphas[i1];
                    double alphaI2Old = alphas[i2];
                    double L, H;
                    if (y[i1] == y[i2])
                    {
                        L = Math.Max(0, alphas[i1] + alphas[i2] - C);
                        H = Math.Min(C, alphas[i1] + alphas[i2]);
                    }
                    else
                    {
                        L = Math.Max(0, alphas[i2] - alphas[i1]);
                        H = Math.Min(C, C + alphas[i2] - alphas[i1]);
                    }
                    if (L == H)
                    {
                        return 0;
                    }
                    double eta = 2 * kernel(X[i1], X[i2]) - kernel(X[i1], X[i1]) - kernel(X[i2], X[i2]);
                    if (eta >= 0)
                    {
                        return 0;
                    }
                    alphas[i2] -= y[i2] * (E1 - E2) / eta;
                    alphas[i2] = Math.Min(H, Math.Max(L, alphas[i2]));
                    if (Math.Abs(alphas[i2] - alphaI2Old) < tol)
                    {
                        return 0;
                    }
                    alphas[i1] += y[i1] * y[i2] * (alphaI2Old - alphas[i2]);
                    double b1 = b - E1 - y[i1] * (alphas[i1] - alphaI1Old) * kernel(X[i1], X[i1]) - y[i2] * (alphas[i2] - alphaI2Old) * kernel(X[i1], X[i2]);
                    double b2 = b - E2 - y[i1] * (alphas[i1] - alphaI1Old) * kernel(X[i1], X[i2]) - y[i2] * (alphas[i2] - alphaI2Old) * kernel(X[i2], X[i2]);
                    if (alphas[i1] > 0 && alphas[i1] < C)
                    {
                        b = b1;
                    }
                    else if (alphas[i2] > 0 && alphas[i2] < C)
                    {
                        b = b2;
                    }
                    else
                    {
                        b = (b1 + b2) / 2;
                    }
                    return 1;
                }
                return 0;
            }
            private int SelectJrand(int i, int m)
            {
                int j = i;
                while (j == i)
                {
                    j = random.Next(0, m);
                }
                return j;
            }

            public double Predict(double[] x)
            {
                double prediction = b;
                for (int i = 0; i < y.Length; i++)
                {
                    prediction += alphas[i] * y[i] * kernel(X[i], x);
                }
                return prediction;
            }
        }

        // 将 DataTable 转换为二维数组
        static double[][] DataTableToDoubleArray(DataTable dt)
        {
            double[][] result = new double[dt.Rows.Count][];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                result[i] = new double[dt.Columns.Count];
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    result[i][j] = Convert.ToDouble(dt.Rows[i][j]);
                }
            }
            return result;
        }


        // 计算预计算的核矩阵（这里以 RBF 核为例）
        static double[][] ComputePrecomputedKernelMatrix(double[][] X, double gamma)
        {
            int n = X.Length;
            double[][] kernelMatrix = new double[n][];
            for (int i = 0; i < n; i++)
            {
                kernelMatrix[i] = new double[n];
                for (int j = 0; j < n; j++)
                {
                    if (i == j)
                    {
                        kernelMatrix[i][j] = 1;
                    }
                    else if (i > j)
                    {
                        kernelMatrix[i][j] = kernelMatrix[j][i];
                    }
                    else
                    {
                        double norm = 0;
                        for (int k = 0; k < X[i].Length; k++)
                        {
                            double diff = X[i][k] - X[j][k];
                            norm += diff * diff;
                        }
                        kernelMatrix[i][j] = Math.Exp(-gamma * norm);
                    }
                }
            }
            return kernelMatrix;
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
        public SVRForm()
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
        // 合并多个 DataTable 为一个二维数组
        static double[][] CombineDataTables(List<DataTable> dataTables)
        {
            int totalColumns = 0;
            foreach (DataTable dt in dataTables)
            {
                totalColumns += dt.Columns.Count;
            }
            int rows = dataTables[0].Rows.Count;
            double[][] combined = new double[rows][];
            for (int i = 0; i < rows; i++)
            {
                combined[i] = new double[totalColumns];
                int colIndex = 0;
                foreach (DataTable dt in dataTables)
                {
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        combined[i][colIndex++] = Convert.ToDouble(dt.Rows[i][j]);
                    }
                }
            }
            return combined;
        }
        // 将 DataTable 转换为二维数组
        static double[][] DataTableTo2DArray(DataTable dataTable)
        {
            int rows = dataTable.Rows.Count;
            int columns = dataTable.Columns.Count;
            double[][] array = new double[rows][];
            for (int i = 0; i < rows; i++)
            {
                array[i] = new double[columns];
                for (int j = 0; j < columns; j++)
                {
                    array[i][j] = Convert.ToDouble(dataTable.Rows[i][j]);
                }
            }
            return array;
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
                await Task.Run(() =>
                {
                    ModelTrainingInfo.SVRtolerance = double.Parse(toleranceTxt.Text.ToString());
                    ModelTrainingInfo.SVRregularizationparameter = double.Parse(regularizationparameterTxt.Text.ToString());
                    ModelTrainingInfo.SVRMaxIterations = double.Parse(MaxIterationsTxt.Text.ToString());

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
                    // 返回 DataTable 类型的实测结果
                    ModelTrainingInfo.actualDataTable = table10;
                    // 预测，返回 DataTable 类型的预测结果
                   // ModelTrainingInfo.outputsdatatable = table90;
                    double[][] inputs = inputDataTables.ToJagged<double>();
                    // 将输出 DataTable 转换为输出数组
                    //double[][] outputs = outputDataTable.ToJagged<double>();
                    // 创建两个新的 DataTable 分别存储实测值和预测值
                    DataTable actualValuesTable = new DataTable(); // 实测值
                    DataTable predictedValuesTable = new DataTable(); // 预测值

                    // 对输出 DataTable 的每一列进行处理
                    for (int col = 0; col < outputDataTable.Columns.Count; col++)
                    {
                        double[] outputs = new double[outputDataTable.Rows.Count];
                        for (int row = 0; row < outputDataTable.Rows.Count; row++)
                        {
                            outputs[row] = Convert.ToDouble(outputDataTable.Rows[row][col]);
                        }

                        // 创建 SVR 模型
                        SVR svr = new SVR(inputs, outputs, C: ModelTrainingInfo.SVRregularizationparameter, tol: ModelTrainingInfo.SVRtolerance, maxIter: int.Parse(ModelTrainingInfo.SVRMaxIterations.ToString()), gamma: 0.1, kernelType: "linear");

                        // 训练模型
                        svr.Train();

                        // 进行预测
                        double[] predictions = new double[inputs.Length];
                        for (int i = 0; i < inputs.Length; i++)
                        {
                            predictions[i] = svr.Predict(inputs[i]);
                        }

                        // 将实测值和预测值分别添加到对应的 DataTable 中
                        string columnName = outputDataTable.Columns[col].ColumnName;
                        actualValuesTable.Columns.Add(columnName, typeof(double)); // 实测值列
                        predictedValuesTable.Columns.Add(columnName, typeof(double)); // 预测值列

                        for (int i = 0; i < outputs.Length; i++)
                        {
                            if (i >= actualValuesTable.Rows.Count)
                            {
                                actualValuesTable.Rows.Add(); // 添加新行
                                predictedValuesTable.Rows.Add(); // 添加新行
                            }
                            actualValuesTable.Rows[i][col] = outputs[i]; // 实测值
                            predictedValuesTable.Rows[i][col] = predictions[i]; // 预测值
                        }
                    }

                    // 计算整个 DataTable 的决定系数 (R^2)
                    double rSquared = CalculateRSquaredForDataTable(actualValuesTable, predictedValuesTable);
                    //// 计算整个 DataTable 的决定系数 (R^2)
                    //double rSquared = CalculateRSquaredForDataTable(outputDataTable, predictionDataTable);
                    // 预测，返回 DataTable 类型的预测结果
                    ModelTrainingInfo.outputsdatatable = predictedValuesTable;
                    //ModelTrainingInfo.actualDataTable = actualValuesTable;
                    rSquared = Math.Abs(rSquared);
                    var bigValue = new DenseVector(new[] { rSquared });

                    // 获取数值的科学计数法表示
                    string scientific = bigValue[0].ToString("E3");

                    // 使用正则表达式提取尾数部分
                    string pattern = @"^[-+]?(\d+\.\d+)E[+-]\d+$";
                    Match match = Regex.Match(scientific, pattern);

                    string formatted;
                    if (match.Success)
                    {
                        // 提取并格式化尾数部分，保留3位小数
                        formatted = match.Groups[1].Value;
                        // 确保有3位小数
                        formatted = double.Parse(formatted).ToString("F3");
                        // 添加符号
                        formatted = (bigValue[0] < 0 ? "-" : "") + formatted;
                    }
                    else
                    {
                        // 如果正则匹配失败，使用默认格式化
                        formatted = bigValue[0].ToString("F3");
                    }

                    double scores = double.Parse(formatted);
                    scores /= 10;
                    ModelTrainingInfo.trainingscore = Math.Round(scores, 3).ToString(); 
                });

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
                ModelTrainingInfo.SVRtolerance = double.Parse(toleranceTxt.Text.ToString());
                ModelTrainingInfo.SVRregularizationparameter = double.Parse(regularizationparameterTxt.Text.ToString());
                ModelTrainingInfo.SVRMaxIterations = double.Parse(MaxIterationsTxt.Text.ToString());
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
                //ResultInfo.svrelapsedSeconds = elapsedSeconds;
            }
            else
            {
                MessageBox.Show("请先进行数据读取！！！");
            }

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
            //if (actualValues.Length != predictedValues.Length)
            //{
            //    throw new ArgumentException("实际值和预测值的长度必须相同。");
            //}

            //int n = actualValues.Length;
            //double sumOfSquaredResiduals = 0; // 残差平方和
            //double sumOfSquaredTotal = 0; // 总平方和
            //double meanActual = actualValues.Average(); // 实际值的均值

            //// 计算残差平方和和总平方和
            //for (int i = 0; i < n; i++)
            //{
            //    double residual = actualValues[i] - predictedValues[i];
            //    sumOfSquaredResiduals += residual * residual;

            //    double deviation = actualValues[i] - meanActual;
            //    sumOfSquaredTotal += deviation * deviation;
            //}

            //// 计算决定系数
            //if (sumOfSquaredTotal == 0)
            //{
            //    return 1; // 所有实际值相同，完美拟合
            //}
            //return 1 - (sumOfSquaredResiduals / sumOfSquaredTotal);
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
        private void CancelBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void rb1Rbtn_CheckedChanged(object sender, EventArgs e)
        {
            ModelTrainingInfo.SVRkernelFunction = rb1Rbtn.Text;
        }

        private void lineaRbtn_CheckedChanged(object sender, EventArgs e)
        {
            ModelTrainingInfo.SVRkernelFunction = lineaRbtn.Text;
        }

        private void polyRbtn_CheckedChanged(object sender, EventArgs e)
        {
            ModelTrainingInfo.SVRkernelFunction = polyRbtn.Text;
        }   

        private void sigmoidRbtn_CheckedChanged(object sender, EventArgs e)
        {
            ModelTrainingInfo.SVRkernelFunction = sigmoidRbtn.Text;
        }

        private void precomputerRbtn_CheckedChanged(object sender, EventArgs e)
        {
            ModelTrainingInfo.SVRkernelFunction = precomputerRbtn.Text;
        }

        private void SVRForm_Load(object sender, EventArgs e)
        {
            ModelTrainingInfo.SVRkernelFunction = rb1Rbtn.Text;
        }
    }
}