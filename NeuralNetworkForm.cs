using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WisdomGrowth.DataBase;


using CsvHelper;
using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.Data;
using Microsoft.ML;
using Microsoft.ML.OnnxRuntime.Tensors;
//using MathNet.Numerics.Statistics;



namespace WisdomGrowth
{
    public partial class NeuralNetworkForm : Form
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
        AutoResizeForm asc = new AutoResizeForm();
        public NeuralNetworkForm()
        {
            InitializeComponent();
            // 设置双缓冲以减少闪烁
            this.DoubleBuffered = true;
            // 初始化定时器
            animationTimer = new Timer();
            animationTimer.Interval = AnimationSpeed;
            animationTimer.Tick += Timer_Tick;
        }
        
        private void NeuralNetworkForm_Load(object sender, EventArgs e)
        {
            asc.controllInitializeSize(this);
            inputfiletextEdit.Text = string.Format("{0}", Application.StartupPath + "\\File\\input-ms-2022-3-14.csv");
            outputfiletextEdit.Text = string.Format("{0}", Application.StartupPath + "\\File\\output-2022-3-14.csv");
        }

        private void NeuralNetworkForm_SizeChanged(object sender, EventArgs e)
        {
            asc.controlAutoSize(this);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofg = new OpenFileDialog();
            ofg.Filter = "所有数据(*.*)|*.*";
            ofg.Multiselect = true;
            if (ofg.ShowDialog() == DialogResult.OK)
            {
                string selectedFilePath = ofg.FileName;
                inputfiletextEdit.Text = selectedFilePath;

            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofg = new OpenFileDialog();
            ofg.Filter = "所有数据(*.*)|*.*";
            ofg.Multiselect = true;
            if (ofg.ShowDialog() == DialogResult.OK)
            {
                string selectedFilePath = ofg.FileName;
                outputfiletextEdit.Text = selectedFilePath;
            }
        }

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
                ReportTxt.Text = "";
                string inputFilePath = inputfiletextEdit.Text;
                string outputFilePath = outputfiletextEdit.Text;
                int batchSize = int.Parse(BatchSizetextEdit.Text);
                double learningRate = double.Parse(LearningRatetextEdit.Text);
                int epochs = int.Parse(EpochtextEdit.Text);
                try
                {
                    // 读取CSV文件
                    var mlContext = new MLContext();
                    var inputData = mlContext.Data.LoadFromTextFile<InputData>(inputFilePath, hasHeader: true);
                    var outputData = mlContext.Data.LoadFromTextFile<OutputData>(outputFilePath, hasHeader: true);

                    // 数据预处理
                    var dataTransform = mlContext.Transforms.Concatenate("Features", Enumerable.Range(0, 11).Select(i => $"Column{i + 1}").ToArray())
                                                   .Append(mlContext.Transforms.NormalizeMinMax("Features"))
                                                   .Append(mlContext.Transforms.Concatenate("Label", "Column1"))
                                                   .Append(mlContext.Transforms.NormalizeMinMax("Label"));

                    var transformedData = dataTransform.Fit(inputData).Transform(inputData);

                    // 划分训练集和测试集
                    var trainTestSplit = mlContext.Data.TrainTestSplit(transformedData, testFraction: 0.15);
                    var trainData = trainTestSplit.TrainSet;
                    var testData = trainTestSplit.TestSet;

                    // 加载ONNX模型
                    var sessionOptions = new SessionOptions();
                    string model1ONNXpath = string.Format("{0}", Application.StartupPath + "\\Model\\simple_model.onnx");
                    var session = new InferenceSession(model1ONNXpath, sessionOptions);

                    // 准备输入数据
                    var inputFeatures = new List<float[]>();
                    var inputDataView = mlContext.Data.CreateEnumerable<InputData>(inputData, reuseRowObject: false);
                    foreach (var item in inputDataView)
                    {
                        var features = Enumerable.Range(0, 11).Select(i => item.GetColumnValue(i)).ToArray();
                        inputFeatures.Add(features);
                    }

                    using (var session1 = new InferenceSession(Path.Combine(Application.StartupPath, "Model", "simple_model.onnx")))
                    {
                        var inputName = session1.InputMetadata.Keys.FirstOrDefault();
                        if (string.IsNullOrEmpty(inputName))
                        {
                            MessageBox.Show("无法获取ONNX模型的输入名称");
                            return;
                        }
                        // 进行推理
                        var outputList = new List<float>();
                        foreach (var features in inputFeatures)
                        {
                            var inputTensor = new DenseTensor<float>(features, new int[] { 1, 11 });
                            var inputs = new List<NamedOnnxValue>
                    {
                        NamedOnnxValue.CreateFromTensor(inputName, inputTensor)
                    };

                            using (var outputs = session.Run(inputs))
                            {
                                var outputTensor = outputs.First().AsTensor<float>();
                                outputList.Add(outputTensor[0]);
                            }
                        }
                        // 显示结果
                       // string txtResultText = $"Model inference finished. Output count: {outputList.Count}";
                        ReportTxt.Text = $"Model inference finished. Output count: {outputList.Count}";
                    }
                      
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred: {ex.Message}");
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
        public class InputData
        {
            [LoadColumn(0)] public float Column1;
            [LoadColumn(1)] public float Column2;
            [LoadColumn(2)] public float Column3;
            [LoadColumn(3)] public float Column4;
            [LoadColumn(4)] public float Column5;
            [LoadColumn(5)] public float Column6;
            [LoadColumn(6)] public float Column7;
            [LoadColumn(7)] public float Column8;
            [LoadColumn(8)] public float Column9;
            [LoadColumn(9)] public float Column10;
            [LoadColumn(10)] public float Column11;

            public float GetColumnValue(int index)
            {
                switch (index)
                {
                    case 0: return Column1;
                    case 1: return Column2;
                    case 2: return Column3;
                    case 3: return Column4;
                    case 4: return Column5;
                    case 5: return Column6;
                    case 6: return Column7;
                    case 7: return Column8;
                    case 8: return Column9;
                    case 9: return Column10;
                    case 10: return Column11;
                    default: throw new IndexOutOfRangeException();
                }
            }
        }

        public class OutputData
        {
            [LoadColumn(0)] public float Column1;
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

        private void ResetBtn_MouseEnter(object sender, EventArgs e)
        {
            ResetBtn.BackColor = Color.SteelBlue;
            ResetBtn.ForeColor = Color.White;
        }

        private void ResetBtn_MouseLeave(object sender, EventArgs e)
        {
            ResetBtn.BackColor = Color.Transparent;
            ResetBtn.ForeColor = Color.White;
        }

        private void button3_MouseEnter(object sender, EventArgs e)
        {
            button3.BackColor = Color.SteelBlue;
            button3.ForeColor = Color.White;
        }

        private void button3_MouseLeave(object sender, EventArgs e)
        {
            button3.BackColor = Color.Transparent;
            button3.ForeColor = Color.White;
        }

        private void button4_MouseEnter(object sender, EventArgs e)
        {
            button4.BackColor = Color.SteelBlue;
            button4.ForeColor = Color.White;
        }

        private void button4_MouseLeave(object sender, EventArgs e)
        {
            button4.BackColor = Color.Transparent;
            button4.ForeColor = Color.White;
        }
        string modelonnx_path = string.Format("{0}", Application.StartupPath + "\\Model\\simple_model.onnx");
        string baseonnx_path = string.Format("{0}", Application.StartupPath + "\\Model\\simple_base.onnx");
        private void ResetBtn_Click(object sender, EventArgs e)
        {
            try
            {
                File.Copy(baseonnx_path, modelonnx_path, true);
                // 进行 ONNX 推理
                RunOnnxInference();
                string content = "Model has been recovered to original base-model!";
                ReportTxt.Text = content;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        private void RunOnnxInference()
        {

            try
            {
                // 加载 ONNX 模型
                using (var session = new InferenceSession(modelonnx_path))
                {
                    float[] y = Visualization.pridectinputdatas;
                    // 进行 ONNX 推理
                    var inputTensor = new DenseTensor<float>(y.Select(v => (float)v).ToArray(), new int[] { 1, y.Length });
                    var inputs = new List<NamedOnnxValue>
                    {
                        NamedOnnxValue.CreateFromTensor("input", inputTensor) // "input" 是 ONNX 模型的输入名称，需要根据实际情况修改
                    };
                    // 运行推理
                    using (var results = session.Run(inputs))
                    {
                        // 处理推理结果，这里简单打印输出
                        foreach (var result in results)
                        {
                            var outputTensor = result.AsTensor<float>();
                            Console.WriteLine("Inference result:");
                            foreach (var value in outputTensor)
                            {
                                Console.WriteLine(value);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"ONNX inference error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

}
