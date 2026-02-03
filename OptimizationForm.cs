using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;
using Python.Runtime;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WisdomGrowth.DataBase;
using System.Diagnostics;

namespace WisdomGrowth
{
    public partial class OptimizationForm : Form
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
        private InferenceSession session;
        public OptimizationForm()
        {
            InitializeComponent();

            string onnx_path = string.Format("{0}", Application.StartupPath + "\\Model\\simple_model.onnx");
            //string onnx_path = @"F:\demo\PythonApplication1\PythonApplication1\model\simple_model.onnx";
            session = new InferenceSession(onnx_path);
            // 设置双缓冲以减少闪烁
            this.DoubleBuffered = true;
            // 初始化定时器
            animationTimer = new Timer();
            animationTimer.Interval = AnimationSpeed;
            animationTimer.Tick += Timer_Tick;
        }

        private void OptimizationForm_Load(object sender, EventArgs e)
        {
            asc.controllInitializeSize(this);
            ReportTxt.Text = ResultInfo.ShootingReportTxt;
            ResultMFC04textEdit.Text = ResultInfo.ShootingResultMFC04textEdit;
            ResultH2OtextEdit.Text = ResultInfo.ShootingResultH2OtextEdit;
            ResultMFC12textEdit.Text = ResultInfo.ShootingResultMFC12textEdit;
            ResultMFC13textEdit.Text = ResultInfo.ShootingResultMFC13textEdit;
            ResultMFC14textEdit.Text = ResultInfo.ShootingResultMFC14textEdit;
            ResultMFC15textEdit.Text = ResultInfo.ShootingResultMFC15textEdit;
            ResultMFC16textEdit.Text = ResultInfo.ShootingResultMFC16textEdit;
            ResultMOtextEdit.Text = ResultInfo.ShootingtMOtextEdit;
            ResultPressuretextEdit.Text = ResultInfo.ShootingPressuretextEdit;
            ResultTemperaturetextEdit.Text = ResultInfo.ShootingResultTemperaturetextEdit;
            ResultSpeedtextEdit.Text = ResultInfo.ShootingResultSpeedtextEdit;
        }

        private void OptimizationForm_SizeChanged(object sender, EventArgs e)
        {
            asc.controlAutoSize(this);
        }
        //生长速率优化
        private async void OptimizationGRBtn_Click(object sender, EventArgs e)
        {
            if (isLoading) return;
            isLoading = true;

            // 创建并显示透明窗体
            LoadingOverlayForm loadingOverlay = new LoadingOverlayForm(this);
            loadingOverlay.Show();
            ReportTxt.Text = "";
            btnToggleControls(false);
            try
            {
                #region add by wpz 2025/9/26
                ReportTxt.Text = "";
                string outPath = Path.Combine(Application.StartupPath, "File", "csyh.txt");
                string modelFileName = string.Format("{0}", Application.StartupPath + "\\Model\\model");
                string pythonPath = Path.Combine(Application.StartupPath, "File", "csyh.py");
                string inputPath = string.Format("{0}", Application.StartupPath + "\\File\\input-ms-2022-3-14.csv");
                string outputPath = string.Format("{0}", Application.StartupPath + "\\File\\output-2022-3-14.csv");
                StringBuilder upStrBuilder = new StringBuilder();
                StringBuilder downStrBuilder = new StringBuilder();
                StringBuilder stepStrBuiler = new StringBuilder();
                string[] strArr = new string[8];
                strArr[0] = modelFileName;
                strArr[1] = MFC03textEdit.Text.ToString();
                //strArr[2] = UpMFC04textEdit.Text.ToString();
                //strArr[3] = UpH2OtextEdit.Text.ToString();
                //strArr[4] = UpMFC12textEdit.Text.ToString();
                //strArr[5] = UpMFC13textEdit.Text.ToString();
                //strArr[6] = UpMFC14textEdit.Text.ToString();
                //strArr[7] = UpMFC15textEdit.Text.ToString();
                //strArr[8] = UpMFC16textEdit.Text.ToString();
                //strArr[9] = UpMOtextEdit.Text.ToString();
                //strArr[10] = UpPressuretextEdit.Text.ToString();
                //strArr[11] = UpTemperaturetextEdit.Text.ToString();
                //strArr[12] = UpSpeedtextEdit.Text.ToString();
                upStrBuilder.Append(UpMFC04textEdit.Text.ToString()).Append(',').Append(UpH2OtextEdit.Text.ToString()).Append(',').Append(UpMFC12textEdit.Text.ToString()).Append(',').Append(UpMFC13textEdit.Text.ToString()).Append(',').Append(UpMFC14textEdit.Text.ToString()).Append(',').Append(UpMFC15textEdit.Text.ToString()).Append(',').Append(UpMFC16textEdit.Text.ToString()).Append(',').Append(UpMOtextEdit.Text.ToString()).Append(',').Append(UpPressuretextEdit.Text.ToString()).Append(',').Append(UpTemperaturetextEdit.Text.ToString()).Append(',').Append(UpSpeedtextEdit.Text.ToString());
                strArr[2] = upStrBuilder.ToString();
                //strArr[13] = DownMFC04textEdit.Text.ToString();
                //strArr[14] = DownH2OtextEdit.Text.ToString();
                //strArr[15] = DownMFC12textEdit.Text.ToString();
                //strArr[16] = DownMFC13textEdit.Text.ToString();
                //strArr[17] = DownMFC14textEdit.Text.ToString();
                //strArr[18] = DownMFC15textEdit.Text.ToString();
                //strArr[19] = DownMFC16textEdit.Text.ToString();
                //strArr[20] = DownMOtextEdit.Text.ToString();
                //strArr[21] = DownPressuretextEdit.Text.ToString();
                //strArr[22] = DownTemperaturetextEdit.Text.ToString();
                //strArr[23] = DownSpeedtextEdit.Text.ToString();
                downStrBuilder.Append(DownMFC04textEdit.Text.ToString()).Append(',').Append(DownH2OtextEdit.Text.ToString()).Append(',').Append(DownMFC12textEdit.Text.ToString()).Append(',').Append(DownMFC13textEdit.Text.ToString()).Append(',').Append(DownMFC14textEdit.Text.ToString()).Append(',').Append(DownMFC15textEdit.Text.ToString()).Append(',').Append(DownMFC16textEdit.Text.ToString()).Append(',').Append(DownMOtextEdit.Text.ToString()).Append(',').Append(DownPressuretextEdit.Text.ToString()).Append(',').Append(DownTemperaturetextEdit.Text.ToString()).Append(',').Append(DownSpeedtextEdit.Text.ToString());
                strArr[3] = downStrBuilder.ToString();
                //strArr[24] = StepMFC04textEdit.Text.ToString();
                //strArr[25] = StepH2OtextEdit.Text.ToString();
                //strArr[26] = StepMFC12textEdit.Text.ToString();
                //strArr[27] = StepMFC13textEdit.Text.ToString();
                //strArr[28] = StepMFC14textEdit.Text.ToString();
                //strArr[29] = StepMFC15textEdit.Text.ToString();
                //strArr[30] = StepMFC16textEdit.Text.ToString();
                //strArr[31] = StepMOtextEdit.Text.ToString();
                //strArr[32] = StepPressuretextEdit.Text.ToString();
                //strArr[33] = StepTemperaturetextEdit.Text.ToString();
                //strArr[34] = StepSpeedtextEdit.Text.ToString();
                stepStrBuiler.Append(StepMFC04textEdit.Text.ToString()).Append(',').Append(StepH2OtextEdit.Text.ToString()).Append(',').Append(StepMFC12textEdit.Text.ToString()).Append(',').Append(StepMFC13textEdit.Text.ToString()).Append(',').Append(StepMFC14textEdit.Text.ToString()).Append(',').Append(StepMFC15textEdit.Text.ToString()).Append(',').Append(StepMFC16textEdit.Text.ToString()).Append(',').Append(StepMOtextEdit.Text.ToString()).Append(',').Append(StepPressuretextEdit.Text.ToString()).Append(',').Append(StepTemperaturetextEdit.Text.ToString()).Append(',').Append(StepSpeedtextEdit.Text.ToString());
                strArr[4] = stepStrBuiler.ToString();
                strArr[5] = outPath.ToString();
                strArr[6] = inputPath.ToString();
                strArr[7] = outputPath.ToString();
                //strArr[35] = outPath.ToString();
                //strArr[36] = inputPath.ToString();
                //strArr[37] = outputPath.ToString();
                string fullName = Path.Combine(Application.StartupPath, "File", "csyhtest.txt");
                //string sArguments = pythonPath + " " + strArr[0] + " " + strArr[1] + " " + strArr[2] + " " + strArr[3] + " " + strArr[4] + " " + strArr[5] + " " + strArr[6] + " " + strArr[7] + " " + strArr[8] + " " + strArr[9] + " " + strArr[10] + " " + strArr[11] + " " + strArr[12] + " " + strArr[13] + " " + strArr[14] +  " " + strArr[15] + " " + strArr[16] + " " + strArr[17] + " " + strArr[18] + " " + strArr[19] + " " + strArr[20] + " " + strArr[21] + " " + strArr[22] + " " + strArr[23] + " " + strArr[24] + " " + strArr[25] + " " + strArr[26] + " " + strArr[27] + " " + strArr[28] + " " + strArr[29] + " " + strArr[30] + " " + strArr[31] + " " + strArr[32] + " " + strArr[33] + " " + strArr[34] + " " + strArr[35] + " " + strArr[36] + " " + strArr[37];
                string sArguments = pythonPath + " " + strArr[0] + " " + strArr[1] + " " + strArr[2] + " " + strArr[3] + " " + strArr[4] + " " + strArr[5] + " " + strArr[6] + " " + strArr[7];
                using (FileStream fs = new FileStream(fullName, FileMode.Create, FileAccess.Write))
                {
                    StreamWriter sw = new StreamWriter(fs, Encoding.Default);
                    sw.Flush();
                    sw.Write(sArguments.ToString());
                    sw.Flush();
                    sw.Close();
                }
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
                    using (BufferedStream bufferedStream = new BufferedStream(new FileStream(outPath, FileMode.Open, FileAccess.Read), 4096000))
                    {
                        using (StreamReader streamReader = new StreamReader(bufferedStream))
                        {
                            while ((strCurrent = streamReader.ReadLine()) != null)
                            {
                                if (strCurrent.Contains("Optimized"))
                                {
                                    string content = strCurrent;
                                    ReportTxt.Text = content;
                                    strCurrent = streamReader.ReadLine();
                                    ResultMFC04textEdit.Text = strCurrent.ToString().Trim();
                                    strCurrent = streamReader.ReadLine();
                                    ResultH2OtextEdit.Text = strCurrent.ToString().Trim();
                                    strCurrent = streamReader.ReadLine();
                                    ResultMFC12textEdit.Text = strCurrent.ToString().Trim();
                                    strCurrent = streamReader.ReadLine();
                                    ResultMFC13textEdit.Text = strCurrent.ToString().Trim();
                                    strCurrent = streamReader.ReadLine();
                                    ResultMFC14textEdit.Text = strCurrent.ToString().Trim();
                                    strCurrent = streamReader.ReadLine();
                                    ResultMFC15textEdit.Text = strCurrent.ToString().Trim();
                                    strCurrent = streamReader.ReadLine();
                                    ResultMFC16textEdit.Text = strCurrent.ToString().Trim();
                                    strCurrent = streamReader.ReadLine();
                                    ResultMOtextEdit.Text = strCurrent.ToString().Trim();
                                    strCurrent = streamReader.ReadLine();
                                    ResultPressuretextEdit.Text = strCurrent.ToString().Trim();
                                    strCurrent = streamReader.ReadLine();
                                    ResultTemperaturetextEdit.Text = strCurrent.ToString().Trim();
                                    strCurrent = streamReader.ReadLine();
                                    ResultSpeedtextEdit.Text = strCurrent.ToString().Trim();
                                    ResultInfo.ShootingReportTxt = ReportTxt.Text;
                                    ResultInfo.ShootingResultMFC04textEdit = ResultMFC04textEdit.Text;
                                    ResultInfo.ShootingResultH2OtextEdit = ResultH2OtextEdit.Text;
                                    ResultInfo.ShootingResultMFC12textEdit = ResultMFC12textEdit.Text;
                                    ResultInfo.ShootingResultMFC13textEdit = ResultMFC13textEdit.Text;
                                    ResultInfo.ShootingResultMFC14textEdit = ResultMFC14textEdit.Text;
                                    ResultInfo.ShootingResultMFC15textEdit = ResultMFC15textEdit.Text;
                                    ResultInfo.ShootingResultMFC16textEdit = ResultMFC16textEdit.Text;
                                    ResultInfo.ShootingtMOtextEdit = ResultMOtextEdit.Text;
                                    ResultInfo.ShootingPressuretextEdit = ResultPressuretextEdit.Text;
                                    ResultInfo.ShootingResultTemperaturetextEdit = ResultTemperaturetextEdit.Text;
                                    ResultInfo.ShootingResultSpeedtextEdit = ResultSpeedtextEdit.Text;
                                }
                            }
                        }
                    }
                }
                #endregion
                #region hide by wpz 2025/9/26
                // 获取 Opt_MFC03 值
                //    double optMfc03 = Convert.ToDouble(MFC03textEdit.Text.ToString());

                //    // 获取上限值
                //    double[] upperBounds = new double[]
                //    {
                //Convert.ToDouble(UpMFC04textEdit.Text.ToString()),
                //Convert.ToDouble(UpH2OtextEdit.Text.ToString()),
                //Convert.ToDouble(UpMFC12textEdit.Text.ToString()),
                //Convert.ToDouble(UpMFC13textEdit.Text.ToString()),
                //Convert.ToDouble(UpMFC14textEdit.Text.ToString()),
                //Convert.ToDouble(UpMFC15textEdit.Text.ToString()),
                //Convert.ToDouble(UpMFC16textEdit.Text.ToString()),
                //Convert.ToDouble(UpMOtextEdit.Text.ToString()),
                //Convert.ToDouble(UpPressuretextEdit.Text.ToString()),
                //Convert.ToDouble(UpTemperaturetextEdit.Text.ToString()),
                //Convert.ToDouble(UpSpeedtextEdit.Text.ToString())
                //    };

                //    // 获取下限值
                //    double[] lowerBounds = new double[]
                //    {
                //Convert.ToDouble(DownMFC04textEdit.Text.ToString()),
                //Convert.ToDouble(DownH2OtextEdit.Text.ToString()),
                //Convert.ToDouble(DownMFC12textEdit.Text.ToString()),
                //Convert.ToDouble(DownMFC13textEdit.Text.ToString()),
                //Convert.ToDouble(DownMFC14textEdit.Text.ToString()),
                //Convert.ToDouble(DownMFC15textEdit.Text.ToString()),
                //Convert.ToDouble(DownMFC16textEdit.Text.ToString()),
                //Convert.ToDouble(DownMOtextEdit.Text.ToString()),
                //Convert.ToDouble(DownPressuretextEdit.Text.ToString()),
                //Convert.ToDouble(DownTemperaturetextEdit.Text.ToString()),
                //Convert.ToDouble(DownSpeedtextEdit.Text.ToString())
                //    };

                //    // 获取步长值
                //    double[] deltas = new double[]
                //    {
                //Convert.ToDouble(StepMFC04textEdit.Text.ToString()),
                //Convert.ToDouble(StepH2OtextEdit.Text.ToString()),
                //Convert.ToDouble(StepMFC12textEdit.Text.ToString()),
                //Convert.ToDouble(StepMFC13textEdit.Text.ToString()),
                //Convert.ToDouble(StepMFC14textEdit.Text.ToString()),
                //Convert.ToDouble(StepMFC15textEdit.Text.ToString()),
                //Convert.ToDouble(StepMFC16textEdit.Text.ToString()),
                //Convert.ToDouble(StepMOtextEdit.Text.ToString()),
                //Convert.ToDouble(StepPressuretextEdit.Text.ToString()),
                //Convert.ToDouble(StepTemperaturetextEdit.Text.ToString()),
                //Convert.ToDouble(StepSpeedtextEdit.Text.ToString())
                //    };    
                //    // 创建 ONNX 会话
                //    using (var session = new InferenceSession(Path.Combine(Application.StartupPath, "Model", "simple_model.onnx")))
                //    {
                //        // 获取模型输入名称
                //        var inputName = session.InputMetadata.Keys.FirstOrDefault();
                //        if (string.IsNullOrEmpty(inputName))
                //        {
                //            MessageBox.Show("无法获取ONNX模型的输入名称");
                //            return;
                //        }
                //       // 定义目标函数
                //       Func<double[], double> bestGR = (x) =>
                //        {
                //        // 计算各种参数
                //        double n2 = (x[0] - x[1]) / x[0];
                //            double o2 = x[1] / x[0];
                //            double vh2o = (1.39 * (optMfc03 + x[0] * n2) + x[0] * o2) * 0.000001 / 0.1988 / 60 * 760 * 300 / 273 / x[8];
                //            //double[] vmfcValues = CalculateVmfcValues(x, x[8]);
                //            double[] vmfcValues = new double[5];
                //            for (int i = 0; i < 5; i++)
                //            {
                //                if (i == 0)
                //                {
                //                    vmfcValues[i] = x[i + 2] * 1.39 * 0.000001 / 0.001195 / 60 * 760 * 300 / 273 / x[8];
                //                }
                //                else
                //                {
                //                    vmfcValues[i] = x[i + 3] * 1.39 * 0.000001 / 0.001195 / 60 * 760 * 300 / 273 / x[8];
                //                }
                //            }
                //            double massMo = x[3] * 15.9645 / (600 - 15.9645) / vmfcValues.Sum();
                //            double massH2o = x[1] * 23.76 / (600 - 23.76) / x[0];
                //            double pressure = x[8];
                //            double kTemperature = x[9] + 273.15;
                //            double rotationalSpeed = x[10];

                //            double[] y = { vh2o, massH2o, vmfcValues[0], massMo, vmfcValues[1], vmfcValues[2], vmfcValues[3], vmfcValues[4], pressure, kTemperature, rotationalSpeed };

                //        // 进行 ONNX 推理
                //        var inputTensor = new DenseTensor<float>(y.Select(v => (float)v).ToArray(), new int[] { 1, y.Length });
                //            var inputs = new List<NamedOnnxValue>
                //{
                //NamedOnnxValue.CreateFromTensor(inputName, inputTensor)
                //};

                //            using (var outputs = session.Run(inputs))
                //            {
                //                var output = outputs.First().AsTensor<float>();
                //                return -output.ToArray().Average() / 1500;
                //            }
                //        };
                //        var gaManual = new GeneticAlgorithmManual(
                //            bestGR,
                //            lowerBounds,
                //            upperBounds,
                //            deltas, // 传递步长参数
                //            populationSize: 500,
                //            generations: 1000,
                //            mutationRate: 0.005
                //        );
                //        gaManual.Run();

                //        // 获取最优解
                //        double[] bestX = gaManual.BestSolution;
                //        double bestY = gaManual.BestFitness; // 变异系数

                //        // 显示结果（转换为百分比，与Python一致）
                //        string content = $"Optimized Growth rate:{bestY * 100:F2}(um/h) ";

                //        ReportTxt.Text = content;
                //        // 显示优化结果
                //        ResultMFC04textEdit.Text = bestX[0].ToString("0");
                //        ResultH2OtextEdit.Text = bestX[1].ToString("0");
                //        ResultMFC12textEdit.Text = bestX[2].ToString("0");
                //        ResultMFC13textEdit.Text = bestX[4].ToString("0");
                //        ResultMFC14textEdit.Text = bestX[5].ToString("0");
                //        ResultMFC15textEdit.Text = bestX[6].ToString("0");
                //        ResultMFC16textEdit.Text = bestX[7].ToString("0");
                //        ResultMOtextEdit.Text = bestX[3].ToString("0");
                //        ResultPressuretextEdit.Text = bestX[8].ToString("0.0");
                //        ResultTemperaturetextEdit.Text = bestX[9].ToString("0");
                //        ResultSpeedtextEdit.Text = bestX[10].ToString("0");

                //        ResultInfo.ShootingReportTxt = ReportTxt.Text;
                //        ResultInfo.ShootingResultMFC04textEdit = ResultMFC04textEdit.Text;
                //        ResultInfo.ShootingResultH2OtextEdit = ResultH2OtextEdit.Text;
                //        ResultInfo.ShootingResultMFC12textEdit = ResultMFC12textEdit.Text;
                //        ResultInfo.ShootingResultMFC13textEdit = ResultMFC13textEdit.Text;
                //        ResultInfo.ShootingResultMFC14textEdit = ResultMFC14textEdit.Text;
                //        ResultInfo.ShootingResultMFC15textEdit = ResultMFC15textEdit.Text;
                //        ResultInfo.ShootingResultMFC16textEdit = ResultMFC16textEdit.Text;
                //        ResultInfo.ShootingtMOtextEdit = ResultMOtextEdit.Text;
                //        ResultInfo.ShootingPressuretextEdit = ResultPressuretextEdit.Text;
                //        ResultInfo.ShootingResultTemperaturetextEdit = ResultTemperaturetextEdit.Text;
                //        ResultInfo.ShootingResultSpeedtextEdit = ResultSpeedtextEdit.Text;
                //    }
                //    await SimulateLoadingAsync(); // 模拟加载
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
        // 新增：加载指定的ONNX模型
        private InferenceSession LoadOnnxModel(string modelPath)
        {
            try
            {
                // 确保模型文件存在
                if (!File.Exists(modelPath))
                {
                    throw new FileNotFoundException($"找不到ONNX模型文件: {modelPath}");
                }

                // 创建模型推理会话
                var sessionOptions = new SessionOptions();
                // 可以在这里配置会话选项，如启用GPU等

                return new InferenceSession(modelPath, sessionOptions);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载ONNX模型时出错: {ex.Message}");
                throw;
            }
        }
      

        //均匀性优化
        private async void OptimizationUnformityBtn_Click(object sender, EventArgs e)
        {
            if (isLoading) return;
            isLoading = true;

            // 创建并显示透明窗体
            LoadingOverlayForm loadingOverlay = new LoadingOverlayForm(this);
            loadingOverlay.Show();
            ReportTxt.Text = "";

            btnToggleControls(false);
            try
            {
                ReportTxt.Text = "";
                // 获取 Opt_MFC03 值
                double optMfc03 = Convert.ToDouble(MFC03textEdit.Text.ToString());
                #region add by wpz 2025/9/26
                ReportTxt.Text = "";
                string outPath = Path.Combine(Application.StartupPath, "File", "jyxyh.txt");
                string modelFileName = string.Format("{0}", Application.StartupPath + "\\Model\\model");
                string pythonPath = Path.Combine(Application.StartupPath, "File", "jyxyh.py");
                string inputPath = string.Format("{0}", Application.StartupPath + "\\File\\input-ms-2022-3-14.csv");
                string outputPath = string.Format("{0}", Application.StartupPath + "\\File\\output-2022-3-14.csv");
                StringBuilder upStrBuilder = new StringBuilder();
                StringBuilder downStrBuilder = new StringBuilder();
                StringBuilder stepStrBuiler = new StringBuilder();
                string[] strArr = new string[8];
                strArr[0] = modelFileName;
                strArr[1] = MFC03textEdit.Text.ToString();
                upStrBuilder.Append(UpMFC04textEdit.Text.ToString()).Append(',').Append(UpH2OtextEdit.Text.ToString()).Append(',').Append(UpMFC12textEdit.Text.ToString()).Append(',').Append(UpMFC13textEdit.Text.ToString()).Append(',').Append(UpMFC14textEdit.Text.ToString()).Append(',').Append(UpMFC15textEdit.Text.ToString()).Append(',').Append(UpMFC16textEdit.Text.ToString()).Append(',').Append(UpMOtextEdit.Text.ToString()).Append(',').Append(UpPressuretextEdit.Text.ToString()).Append(',').Append(UpTemperaturetextEdit.Text.ToString()).Append(',').Append(UpSpeedtextEdit.Text.ToString());
                strArr[2] = upStrBuilder.ToString();
                downStrBuilder.Append(DownMFC04textEdit.Text.ToString()).Append(',').Append(DownH2OtextEdit.Text.ToString()).Append(',').Append(DownMFC12textEdit.Text.ToString()).Append(',').Append(DownMFC13textEdit.Text.ToString()).Append(',').Append(DownMFC14textEdit.Text.ToString()).Append(',').Append(DownMFC15textEdit.Text.ToString()).Append(',').Append(DownMFC16textEdit.Text.ToString()).Append(',').Append(DownMOtextEdit.Text.ToString()).Append(',').Append(DownPressuretextEdit.Text.ToString()).Append(',').Append(DownTemperaturetextEdit.Text.ToString()).Append(',').Append(DownSpeedtextEdit.Text.ToString());
                strArr[3] = downStrBuilder.ToString();
                stepStrBuiler.Append(StepMFC04textEdit.Text.ToString()).Append(',').Append(StepH2OtextEdit.Text.ToString()).Append(',').Append(StepMFC12textEdit.Text.ToString()).Append(',').Append(StepMFC13textEdit.Text.ToString()).Append(',').Append(StepMFC14textEdit.Text.ToString()).Append(',').Append(StepMFC15textEdit.Text.ToString()).Append(',').Append(StepMFC16textEdit.Text.ToString()).Append(',').Append(StepMOtextEdit.Text.ToString()).Append(',').Append(StepPressuretextEdit.Text.ToString()).Append(',').Append(StepTemperaturetextEdit.Text.ToString()).Append(',').Append(StepSpeedtextEdit.Text.ToString());
                strArr[4] = stepStrBuiler.ToString();
                strArr[5] = outPath.ToString();
                strArr[6] = inputPath.ToString();
                strArr[7] = outputPath.ToString();                
                //string sArguments = pythonPath + " " + strArr[0] + " " + strArr[1] + " " + strArr[2] + " " + strArr[3] + " " + strArr[4] + " " + strArr[5] + " " + strArr[6] + " " + strArr[7] + " " + strArr[8] + " " + strArr[9] + " " + strArr[10] + " " + strArr[11] + " " + strArr[12] + " " + strArr[13] + " " + strArr[14] +  " " + strArr[15] + " " + strArr[16] + " " + strArr[17] + " " + strArr[18] + " " + strArr[19] + " " + strArr[20] + " " + strArr[21] + " " + strArr[22] + " " + strArr[23] + " " + strArr[24] + " " + strArr[25] + " " + strArr[26] + " " + strArr[27] + " " + strArr[28] + " " + strArr[29] + " " + strArr[30] + " " + strArr[31] + " " + strArr[32] + " " + strArr[33] + " " + strArr[34] + " " + strArr[35] + " " + strArr[36] + " " + strArr[37];
                string sArguments = pythonPath + " " + strArr[0] + " " + strArr[1] + " " + strArr[2] + " " + strArr[3] + " " + strArr[4] + " " + strArr[5] + " " + strArr[6] + " " + strArr[7];
                #region test by wpz 
                string fullName = Path.Combine(Application.StartupPath, "File", "jyxyhtest.txt");
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
                    using (BufferedStream bufferedStream = new BufferedStream(new FileStream(outPath, FileMode.Open, FileAccess.Read), 4096000))
                    {
                        using (StreamReader streamReader = new StreamReader(bufferedStream))
                        {
                            while ((strCurrent = streamReader.ReadLine()) != null)
                            {
                                if (strCurrent.Contains("Optimized"))
                                {
                                    string content = strCurrent;
                                    ReportTxt.Text = content;
                                    strCurrent = streamReader.ReadLine();
                                    ResultMFC04textEdit.Text = strCurrent.ToString().Trim();
                                    strCurrent = streamReader.ReadLine();
                                    ResultH2OtextEdit.Text = strCurrent.ToString().Trim();
                                    strCurrent = streamReader.ReadLine();
                                    ResultMFC12textEdit.Text = strCurrent.ToString().Trim();
                                    strCurrent = streamReader.ReadLine();
                                    ResultMFC13textEdit.Text = strCurrent.ToString().Trim();
                                    strCurrent = streamReader.ReadLine();
                                    ResultMFC14textEdit.Text = strCurrent.ToString().Trim();
                                    strCurrent = streamReader.ReadLine();
                                    ResultMFC15textEdit.Text = strCurrent.ToString().Trim();
                                    strCurrent = streamReader.ReadLine();
                                    ResultMFC16textEdit.Text = strCurrent.ToString().Trim();
                                    strCurrent = streamReader.ReadLine();
                                    ResultMOtextEdit.Text = strCurrent.ToString().Trim();
                                    strCurrent = streamReader.ReadLine();
                                    ResultPressuretextEdit.Text = strCurrent.ToString().Trim();
                                    strCurrent = streamReader.ReadLine();
                                    ResultTemperaturetextEdit.Text = strCurrent.ToString().Trim();
                                    strCurrent = streamReader.ReadLine();
                                    ResultSpeedtextEdit.Text = strCurrent.ToString().Trim();
                                    ResultInfo.ShootingReportTxt = ReportTxt.Text;
                                    ResultInfo.ShootingResultMFC04textEdit = ResultMFC04textEdit.Text;
                                    ResultInfo.ShootingResultH2OtextEdit = ResultH2OtextEdit.Text;
                                    ResultInfo.ShootingResultMFC12textEdit = ResultMFC12textEdit.Text;
                                    ResultInfo.ShootingResultMFC13textEdit = ResultMFC13textEdit.Text;
                                    ResultInfo.ShootingResultMFC14textEdit = ResultMFC14textEdit.Text;
                                    ResultInfo.ShootingResultMFC15textEdit = ResultMFC15textEdit.Text;
                                    ResultInfo.ShootingResultMFC16textEdit = ResultMFC16textEdit.Text;
                                    ResultInfo.ShootingtMOtextEdit = ResultMOtextEdit.Text;
                                    ResultInfo.ShootingPressuretextEdit = ResultPressuretextEdit.Text;
                                    ResultInfo.ShootingResultTemperaturetextEdit = ResultTemperaturetextEdit.Text;
                                    ResultInfo.ShootingResultSpeedtextEdit = ResultSpeedtextEdit.Text;
                                }
                            }
                        }
                    }
                }
                #endregion
                #region hide by wpz 2025/9/26
                // 获取上限值
                //    double[] upperBounds = new double[]
                //    {
                //Convert.ToDouble(UpMFC04textEdit.Text.ToString()),
                //Convert.ToDouble(UpH2OtextEdit.Text.ToString()),
                //Convert.ToDouble(UpMFC12textEdit.Text.ToString()),
                //Convert.ToDouble(UpMFC13textEdit.Text.ToString()),
                //Convert.ToDouble(UpMFC14textEdit.Text.ToString()),
                //Convert.ToDouble(UpMFC15textEdit.Text.ToString()),
                //Convert.ToDouble(UpMFC16textEdit.Text.ToString()),
                //Convert.ToDouble(UpMOtextEdit.Text.ToString()),
                //Convert.ToDouble(UpPressuretextEdit.Text.ToString()),
                //Convert.ToDouble(UpTemperaturetextEdit.Text.ToString()),
                //Convert.ToDouble(UpSpeedtextEdit.Text.ToString())
                //    };

                //    // 获取下限值
                //    double[] lowerBounds = new double[]
                //    {
                //Convert.ToDouble(DownMFC04textEdit.Text.ToString()),
                //Convert.ToDouble(DownH2OtextEdit.Text.ToString()),
                //Convert.ToDouble(DownMFC12textEdit.Text.ToString()),
                //Convert.ToDouble(DownMFC13textEdit.Text.ToString()),
                //Convert.ToDouble(DownMFC14textEdit.Text.ToString()),
                //Convert.ToDouble(DownMFC15textEdit.Text.ToString()),
                //Convert.ToDouble(DownMFC16textEdit.Text.ToString()),
                //Convert.ToDouble(DownMOtextEdit.Text.ToString()),
                //Convert.ToDouble(DownPressuretextEdit.Text.ToString()),
                //Convert.ToDouble(DownTemperaturetextEdit.Text.ToString()),
                //Convert.ToDouble(DownSpeedtextEdit.Text.ToString())
                //    };

                //    // 获取步长值
                //    double[] deltas = new double[]
                //    {
                //Convert.ToDouble(StepMFC04textEdit.Text.ToString()),
                //Convert.ToDouble(StepH2OtextEdit.Text.ToString()),
                //Convert.ToDouble(StepMFC12textEdit.Text.ToString()),
                //Convert.ToDouble(StepMFC13textEdit.Text.ToString()),
                //Convert.ToDouble(StepMFC14textEdit.Text.ToString()),
                //Convert.ToDouble(StepMFC15textEdit.Text.ToString()),
                //Convert.ToDouble(StepMFC16textEdit.Text.ToString()),
                //Convert.ToDouble(StepMOtextEdit.Text.ToString()),
                //Convert.ToDouble(StepPressuretextEdit.Text.ToString()),
                //Convert.ToDouble(StepTemperaturetextEdit.Text.ToString()),
                //Convert.ToDouble(StepSpeedtextEdit.Text.ToString())
                //    };

                //    // 创建 ONNX 会话
                //    using (var session = new InferenceSession(Path.Combine(Application.StartupPath, "Model", "simple_model.onnx")))
                //    {
                //        // 获取模型输入名称
                //        var inputName = session.InputMetadata.Keys.FirstOrDefault();
                //        if (string.IsNullOrEmpty(inputName))
                //        {
                //            MessageBox.Show("无法获取ONNX模型的输入名称");
                //            return;
                //        }
                //        // 定义目标函数
                //        Func<double[], double> bestGR = (x) =>
                //        {
                //            double n2 = (x[0] - x[1]) / x[0];
                //            double o2 = x[1] / x[0];
                //            double vh2o = (1.39 * (optMfc03 + x[0] * n2) + x[0] * o2) * 0.000001 / 0.1988 / 60 * 760 * 300 / 273 / x[8];
                //            //double[] vmfcValues = new double[5];
                //            //for (int i = 0; i < 5; i++)
                //            //{
                //            //    vmfcValues[i] = x[i + 2] * 1.39 * 0.000001 / 0.001195 / 60 * 760 * 300 / 273 / x[8];
                //            //}
                //            //double massMo = x[3] * 15.9645 / (600 - 15.9645) / vmfcValues.Sum();
                //            double vmfcValues = x[2] * 1.39 * 0.000001 / 0.001195 / 60 * 760 * 300 / 273 / x[8];
                //            double massMo = x[3] * 15.9645 / (600 - 15.9645) / vmfcValues;
                //            double massH2o = x[1] * 23.76 / (600 - 23.76) / x[0];
                //            double pressure = x[8];
                //            double kTemperature = x[9] + 273.15;
                //            double rotationalSpeed = x[10];
                //            // double[] y = { massH2o, vh2o, (float)vmfcValues[0], (float)vmfcValues[1], (float)vmfcValues[2], (float)vmfcValues[3], (float)vmfcValues[4], (float)massMo, (float)pressure, (float)kTemperature, (float)rotationalSpeed };
                //            //double[] y = { vh2o, massH2o, (float)vmfcValues[0], massMo, (float)vmfcValues[1], (float)vmfcValues[2], (float)vmfcValues[3], (float)vmfcValues[4], (float)pressure, (float)kTemperature, (float)rotationalSpeed };
                //            double[] y = { vh2o, massH2o, (float)vmfcValues, massMo, (float)vmfcValues, (float)vmfcValues, (float)vmfcValues, (float)vmfcValues, (float)pressure, (float)kTemperature, (float)rotationalSpeed };
                //            // 进行 ONNX 推理
                //            var inputTensor = new DenseTensor<float>(y.Select(v => (float)v).ToArray(), new int[] { 1, y.Length });
                //            var inputs = new List<NamedOnnxValue>
                //{
                //    NamedOnnxValue.CreateFromTensor(inputName, inputTensor) // "input" 是 ONNX 模型的输入名称，需要根据实际情况修改
                //};
                //            using (var outputs = session.Run(inputs))
                //            {
                //                var output = outputs.First().AsTensor<float>();
                //                var outputArray = output.ToArray();
                //                // 计算平均值
                //                double average = outputArray.Average();
                //                // 计算标准差
                //                double sumOfSquaredDifferences = outputArray.Sum(v => Math.Pow(v - average, 2));
                //                double std = Math.Sqrt(sumOfSquaredDifferences / (outputArray.Length - 1));

                //                // 计算变异系数
                //                double var = std / average;
                //                return var;
                //            }
                //        };
                //        // 使用精度参数创建遗传算法实例
                //        var gaManual = new GeneticAlgorithmManual(
                //            bestGR,
                //            lowerBounds,
                //            upperBounds,
                //            deltas, // 传递精度参数
                //            populationSize: 500,
                //            generations: 1000,
                //            mutationRate: 0.005
                //        );
                //        gaManual.Run();

                //        double[] bestX = gaManual.BestSolution;
                //        //double bestY = -gaManual.BestFitness;
                //        double bestY = Math.Abs(gaManual.BestFitness);
                //        //MessageBox.Show(bestY.ToString());

                //        string content = "Optimized Uniformity (Coefficient of variation):" + Math.Round(bestY, 2) + "%";
                //        ReportTxt.Text = content;
                //        // 显示优化结果
                //        ResultMFC04textEdit.Text = bestX[0].ToString("0");
                //        ResultH2OtextEdit.Text = bestX[1].ToString("0");
                //        ResultMFC12textEdit.Text = bestX[2].ToString("0");
                //        ResultMFC13textEdit.Text = bestX[4].ToString("0");
                //        ResultMFC14textEdit.Text = bestX[5].ToString("0");
                //        ResultMFC15textEdit.Text = bestX[6].ToString("0");
                //        ResultMFC16textEdit.Text = bestX[7].ToString("0");
                //        ResultMOtextEdit.Text = bestX[3].ToString("0");
                //        ResultPressuretextEdit.Text = bestX[8].ToString("0.0");
                //        ResultTemperaturetextEdit.Text = bestX[9].ToString("0");
                //        ResultSpeedtextEdit.Text = bestX[10].ToString("0");

                //        ResultInfo.ShootingReportTxt = ReportTxt.Text;
                //        ResultInfo.ShootingResultMFC04textEdit = ResultMFC04textEdit.Text;
                //        ResultInfo.ShootingResultH2OtextEdit = ResultH2OtextEdit.Text;
                //        ResultInfo.ShootingResultMFC12textEdit = ResultMFC12textEdit.Text;
                //        ResultInfo.ShootingResultMFC13textEdit = ResultMFC13textEdit.Text;
                //        ResultInfo.ShootingResultMFC14textEdit = ResultMFC14textEdit.Text;
                //        ResultInfo.ShootingResultMFC15textEdit = ResultMFC15textEdit.Text;
                //        ResultInfo.ShootingResultMFC16textEdit = ResultMFC16textEdit.Text;
                //        ResultInfo.ShootingtMOtextEdit = ResultMOtextEdit.Text;
                //        ResultInfo.ShootingPressuretextEdit = ResultPressuretextEdit.Text;
                //        ResultInfo.ShootingResultTemperaturetextEdit = ResultTemperaturetextEdit.Text;
                //        ResultInfo.ShootingResultSpeedtextEdit = ResultSpeedtextEdit.Text;
                //    }
                //    await SimulateLoadingAsync(); // 模拟加载
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
        public double CalculateAverage(double[] bestX)
        {
            if (bestX == null || bestX.Length == 0)
                return 0.0; // 处理空数组或null的情况

            return bestX.Average(); // 使用LINQ的Average方法
        }
        static bool IsFiniteNumber(double value)
        {
            return !double.IsInfinity(value) && !double.IsNaN(value);
        }


        private void OptimizationGRBtn_MouseEnter(object sender, EventArgs e)
        {
            OptimizationGRBtn.BackColor = Color.SteelBlue;
            OptimizationGRBtn.ForeColor = Color.White;
        }

        private void OptimizationGRBtn_MouseLeave(object sender, EventArgs e)
        {
            OptimizationGRBtn.BackColor = Color.Transparent;
            OptimizationGRBtn.ForeColor = Color.White;
        }

        private void OptimizationUnformityBtn_MouseEnter(object sender, EventArgs e)
        {
            OptimizationUnformityBtn.BackColor = Color.SteelBlue;
            OptimizationUnformityBtn.ForeColor = Color.White;
        }

        private void OptimizationUnformityBtn_MouseLeave(object sender, EventArgs e)
        {
            OptimizationUnformityBtn.BackColor = Color.Transparent;
            OptimizationUnformityBtn.ForeColor = Color.White;
        }

        private void TransferBtn_MouseEnter(object sender, EventArgs e)
        {
            TransferBtn.BackColor = Color.SteelBlue;
            TransferBtn.ForeColor = Color.White;
        }

        private void TransferBtn_MouseLeave(object sender, EventArgs e)
        {
            TransferBtn.BackColor = Color.Transparent;
            TransferBtn.ForeColor = Color.White;
        }

        private async void TransferBtn_Click(object sender, EventArgs e)
        {
            if (isLoading) return;
            isLoading = true;

            // 创建并显示透明窗体
            LoadingOverlayForm loadingOverlay = new LoadingOverlayForm(this);
            loadingOverlay.Show();

            btnToggleControls(false);
            try
            {
                double ResultMFC = double.Parse(ResultMFC04textEdit.Text);
                //int ResultMFC = int.Parse(ResultMFC04textEdit.Text);
                ResultMFC = ResultMFC / 6;

                double ResultMFC4 = ResultMFC;
                double ResultMFC05 = ResultMFC;
                double ResultMFC06 = ResultMFC;
                double ResultMFC07 = ResultMFC;
                double ResultMFC08 = ResultMFC;
                double ResultMFC09 = ResultMFC;

                double ResultMFC03 = double.Parse(MFC03textEdit.Text);
                double ResultMFC12 = double.Parse(ResultMFC12textEdit.Text);
                double ResultMFC13 = double.Parse(ResultMFC13textEdit.Text);
                double ResultMFC14 = double.Parse(ResultMFC14textEdit.Text);
                double ResultMFC15 = double.Parse(ResultMFC15textEdit.Text);
                double ResultMFC16 = double.Parse(ResultMFC16textEdit.Text);
                double ResultH2O = double.Parse(ResultH2OtextEdit.Text);
                double ResultMO = double.Parse(ResultMOtextEdit.Text);
                double ResultPressuret = double.Parse(ResultPressuretextEdit.Text);
                double ResultTemperaturetext = double.Parse(ResultTemperaturetextEdit.Text);
                double ResultSpeedtext = double.Parse(ResultSpeedtextEdit.Text);

                List<double> datalist = new List<double>
                    {
                        ResultMFC4, ResultMFC05, ResultMFC06, ResultMFC07, ResultMFC08,ResultMFC09, ResultMFC03, ResultMFC12, ResultMFC13,
                        ResultMFC14, ResultMFC15, ResultMFC16, ResultH2O,ResultMO, ResultPressuret, ResultTemperaturetext, ResultSpeedtext
                    };
                Visualization.inputdatas = datalist;
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
    }
}
