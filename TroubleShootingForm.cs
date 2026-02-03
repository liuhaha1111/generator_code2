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
using Word = Microsoft.Office.Interop.Word;
using System.Diagnostics;
namespace WisdomGrowth
{
    public partial class TroubleShootingForm : Form
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
        public TroubleShootingForm()
        {
            InitializeComponent();
            // 设置双缓冲以减少闪烁
            this.DoubleBuffered = true;
            // 初始化定时器
            animationTimer = new Timer();
            animationTimer.Interval = AnimationSpeed;
            animationTimer.Tick += Timer_Tick;
        }

        private void TroubleShootingForm_Load(object sender, EventArgs e)
        {
            asc.controllInitializeSize(this);
            RepoetTxt.Text=ResultInfo.TroubleShootingReportTxt;
            ResultMFC04textEdit.Text=ResultInfo.TroubleShootingResultMFC04textEdit;
            ResultH2OtextEdit.Text=ResultInfo.TroubleShootingResultH2OtextEdit;
            ResultMFC12textEdit.Text=ResultInfo.TroubleShootingResultMFC12textEdit;
            ResultMFC13textEdit.Text=ResultInfo.TroubleShootingResultMFC13textEdit;
            ResultMFC14textEdit.Text=ResultInfo.TroubleShootingResultMFC14textEdit;
            ResultMFC15textEdit.Text=ResultInfo.TroubleShootingResultMFC15textEdit;
            ResultMFC16textEdit.Text=ResultInfo.TroubleShootingResultMFC16textEdit;
            ResultMOtextEdit.Text=ResultInfo.TroubleShootingtMOtextEdit;
            ResultPressuretextEdit.Text=ResultInfo.TroubleShootingPressuretextEdit;
            ResultTemperaturetextEdit.Text=ResultInfo.TroubleShootingResultTemperaturetextEdit;
            ResultSpeedtextEdit.Text=ResultInfo.TroubleShootingResultSpeedtextEdit;
        }
        private void TroubleShootingForm_SizeChanged(object sender, EventArgs e)
        {
            asc.controlAutoSize(this);
        }
        private async void ShootingBtn_Click(object sender, EventArgs e)
        {
            if (isLoading) return;
            isLoading = true;

            LoadingOverlayForm loadingOverlay = new LoadingOverlayForm(this);
            loadingOverlay.Show();

            btnToggleControls(false);
            try
            {
                RepoetTxt.Text = "";
                string onnx_path = string.Format("{0}", Application.StartupPath + "\\Model\\simple_model.onnx");

                // 获取界面输入
                double optMfc03 = double.Parse(MFC03textEdit.Text);
                #region add by wpz 2025/9/26
                string outPath = Path.Combine(Application.StartupPath, "File", "ycsy.txt");
                string modelFileName = string.Format("{0}", Application.StartupPath + "\\Model\\model");
                string pythonPath = Path.Combine(Application.StartupPath, "File", "ycsy.py");
                string inputPath = string.Format("{0}", Application.StartupPath + "\\File\\input-ms-2022-3-14.csv");
                string outputPath = string.Format("{0}", Application.StartupPath + "\\File\\output-2022-3-14.csv");
                StringBuilder frontStrBuilder = new StringBuilder();
                StringBuilder afterStrBuilder = new StringBuilder();
                frontStrBuilder.Append(UpMFC04textEdit.Text.ToString()).Append(',').Append(UpH2OtextEdit.Text.ToString()).Append(',').Append(UpMFC12textEdit.Text.ToString()).Append(',').Append(UpMFC13textEdit.Text.ToString()).Append(',').Append(UpMFC14textEdit.Text.ToString()).Append(',').Append(UpMFC15textEdit.Text.ToString()).Append(',').Append(UpMFC16textEdit.Text.ToString()).Append(',').Append(UpMOtextEdit.Text.ToString()).Append(',').Append(UpPressuretextEdit.Text.ToString()).Append(',').Append(UpTemperaturetextEdit.Text.ToString()).Append(',').Append(UpSpeedtextEdit.Text.ToString()).Append(',');
                afterStrBuilder.Append(I1textEdit.Text.ToString()).Append(',').Append(I2textEdit.Text.ToString()).Append(',').Append(I3textEdit.Text.ToString()).Append(',').Append(M1textEdit.Text.ToString()).Append(',').Append(M2textEdit.Text.ToString()).Append(',').Append(M3textEdit.Text.ToString()).Append(',').Append(O1textEdit.Text.ToString()).Append(',').Append(O2textEdit.Text.ToString()).Append(',').Append(O3textEdit.Text.ToString()).Append(',');
                string[] strArr = new string[7];
                strArr[0] = modelFileName;
                strArr[1] = MFC03textEdit.Text.ToString();
                strArr[2] = frontStrBuilder.ToString();
                strArr[3] = afterStrBuilder.ToString();
                strArr[4] = outPath.ToString();
                strArr[5] = inputPath.ToString();
                strArr[6] = outputPath.ToString();
                string sArguments = pythonPath + " " + strArr[0] + " " + strArr[1] + " " + strArr[2] + " " + strArr[3] + " " + strArr[4] + " " + strArr[5] + " " + strArr[6];
                
                #region test by wpz 
                string fullName = Path.Combine(Application.StartupPath, "File", "ycsytest.txt");
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
                                if (strCurrent.Contains("Shooting Finished"))
                                {
                                    string content = strCurrent;
                                    RepoetTxt.Text = content;
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
                                    ResultInfo.ShootingReportTxt = RepoetTxt.Text;
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

                #region hide by wpz 2025/9/27
                //        List<double> process = new List<double>
                //{
                //    double.Parse(UpMFC04textEdit.Text.ToString()),
                //    double.Parse(UpH2OtextEdit.Text.ToString()),
                //    double.Parse(UpMFC12textEdit.Text.ToString()),
                //    double.Parse(UpMFC13textEdit.Text.ToString()),
                //    double.Parse(UpMFC14textEdit.Text.ToString()),
                //    double.Parse(UpMFC15textEdit.Text.ToString()),
                //    double.Parse(UpMFC16textEdit.Text.ToString()),
                //    double.Parse(UpMOtextEdit.Text.ToString()),
                //    double.Parse(UpPressuretextEdit.Text.ToString()),
                //    double.Parse(UpTemperaturetextEdit.Text.ToString()),
                //    double.Parse(UpSpeedtextEdit.Text.ToString())
                //};

                //        List<double> result = new List<double>
                //{
                //    double.Parse(I1textEdit.Text.ToString()),
                //    double.Parse(I2textEdit.Text.ToString()),
                //    double.Parse(I3textEdit.Text.ToString()),
                //    double.Parse(M1textEdit.Text.ToString()),
                //    double.Parse(M2textEdit.Text.ToString()),
                //    double.Parse(M3textEdit.Text.ToString()),
                //    double.Parse(O1textEdit.Text.ToString()),
                //    double.Parse(O2textEdit.Text.ToString()),
                //    double.Parse(O3textEdit.Text.ToString())
                //};

                //        // 定义输入范围（与Python保持一致）
                //        List<int> upInput = new List<int> { 5000, 200, 1200, 200, 1200, 1200, 1200, 1200, 30, 800, 1000 };
                //        List<int> lowInput = new List<int> { 3000, 50, 50, 50, 50, 50, 50, 50, 1, 200, 0 };

                //        // 定义标准化参数（需从Python获取）
                //        List<double> featureMeans = new List<double> { /* 均值 */ };
                //        List<double> featureStds = new List<double> { /* 标准差 */ };

                //        using (var session = new InferenceSession(Path.Combine(Application.StartupPath, "Model", "simple_model.onnx")))
                //        {
                //            var inputName = session.InputMetadata.Keys.FirstOrDefault();
                //            if (string.IsNullOrEmpty(inputName))
                //            {
                //                MessageBox.Show("无法获取ONNX模型的输入名称");
                //                return;
                //            }

                //            var (bestValue, bestResult) = await Task.Run(() =>
                //            {
                //                List<double> localBestValue = new List<double>();
                //                List<double> localBestResult = new List<double>();

                //                for (int i = 0; i < process.Count; i++)
                //                {
                //                    // 创建DataTable存储所有结果
                //                    DataTable dataTable = new DataTable();
                //                    for (int col = 0; col < process.Count + 1; col++)
                //                    {
                //                        dataTable.Columns.Add($"Col{col}", typeof(double));
                //                    }

                //                    // 修正遍历方向：从lowInput到upInput
                //                    for (double t = lowInput[i]; t <= upInput[i]; t++)
                //                    {
                //                        List<double> x = new List<double>(process);
                //                        x[i] = t;

                //                        // 计算相关参数
                //                        double n2 = (x[0] - x[1]) / x[0];
                //                        double o2 = x[1] / x[0];
                //                        double vh2o = (1.39 * (optMfc03 + x[0] * n2) + x[0] * o2) * 0.000001 / 0.1988 / 60 * 760 * 300 / 273 / x[8];
                //                        double vmfc12 = x[2] * 1.39 * 0.000001 / 0.001195 / 60 * 760 * 300 / 273 / x[8];
                //                        double vmfc13 = x[4] * 1.39 * 0.000001 / 0.001195 / 60 * 760 * 300 / 273 / x[8];
                //                        double vmfc14 = x[5] * 1.39 * 0.000001 / 0.001195 / 60 * 760 * 300 / 273 / x[8];
                //                        double vmfc15 = x[6] * 1.39 * 0.000001 / 0.001195 / 60 * 760 * 300 / 273 / x[8];
                //                        double vmfc16 = x[7] * 1.39 * 0.000001 / 0.001195 / 60 * 760 * 300 / 273 / x[8];
                //                        double massMo = x[3] * 15.9645 / (600 - 15.9645) / (x[2] + x[4] + x[5] + x[6] + x[7]);
                //                        double massH2o = x[1] * 23.76 / (600 - 23.76) / x[0];
                //                        double pressure = x[8];
                //                        double ktemperature = x[9] + 273.15;
                //                        double rotationalSpeed = x[10];

                //                        List<double> y = new List<double>
                //                {
                //                    vh2o, massH2o, vmfc12, massMo, vmfc13,
                //                    vmfc14, vmfc15, vmfc16, pressure, ktemperature, rotationalSpeed
                //                };

                //                        //// 添加标准化处理
                //                        StandardScaler scaler = new StandardScaler();
                //                        double[] standardizedData = scaler.FitTransform(y);
                //                        List<double> normalizedY = new List<double>(standardizedData);
                //                        // 准备输入张量（使用标准化后的值）
                //                        var inputTensor = new DenseTensor<float>(new float[normalizedY.Count], new int[] { 1, normalizedY.Count });
                //                        for (int j = 0; j < normalizedY.Count; j++)
                //                        {
                //                            inputTensor[0, j] = (float)normalizedY[j];
                //                        }

                //                        var inputs = new List<NamedOnnxValue>
                //                {
                //                    NamedOnnxValue.CreateFromTensor(inputName, inputTensor)
                //                };

                //                        // 运行模型
                //                        using (var outputs = session.Run(inputs))
                //                        {
                //                            var output = outputs.First().AsTensor<float>();
                //                            List<double> nnPredict = new List<double>();
                //                            foreach (var value in output)
                //                            {
                //                                nnPredict.Add(value / 1500);  // 与Python一致
                //                            }

                //                            // 计算 MSE
                //                            double mse = CalculateMse(result, nnPredict);

                //                            // 添加到DataTable
                //                            DataRow row = dataTable.NewRow();
                //                            for (int j = 0; j < x.Count; j++)
                //                            {
                //                                row[j] = x[j];
                //                            }
                //                            row[x.Count] = mse;
                //                            dataTable.Rows.Add(row);
                //                        }
                //                    }

                //                    // 排序并获取最优值
                //                    DataView sortedView = new DataView(dataTable);
                //                    sortedView.Sort = "Col" + (process.Count) + " ASC";
                //                    DataTable sortedTable = sortedView.ToTable();

                //                    if (sortedTable.Rows.Count > 0)
                //                    {
                //                        localBestValue.Add(Convert.ToDouble(sortedTable.Rows[0][i]));
                //                        localBestResult.Add(Convert.ToDouble(sortedTable.Rows[0][process.Count]));
                //                    }
                //                }
                //                return (localBestValue, localBestResult);
                //            });

                //            // 结果处理（与Python完全一致）
                //            double[] bestResultArray = bestResult.Select(r => 1 / r).ToArray();
                //            double sum = bestResultArray.Sum();
                //            double[] normalizedResult = bestResultArray.Select(r => r / sum).ToArray();
                //            List<double> lcd = normalizedResult.Select(r => Math.Round(r * 100, 1)).ToList();
                //            double averageresult = lcd.Average();

                //            // 显示结果
                //            string plainTextEdit4Text = "Shooting Finished! The unit of result is " + Math.Round(averageresult, 2) + " %.";
                //            RepoetTxt.Text = plainTextEdit4Text;

                //            // 确保结果显示控件对应正确
                //            ResultMFC04textEdit.Text = lcd[0].ToString("0.0");
                //            ResultH2OtextEdit.Text = lcd[1].ToString("0.0");
                //            ResultMFC12textEdit.Text = lcd[2].ToString("0.0");
                //            ResultMFC13textEdit.Text = lcd[3].ToString("0.0");
                //            ResultMFC14textEdit.Text = lcd[4].ToString("0.0");
                //            ResultMFC15textEdit.Text = lcd[5].ToString("0.0");
                //            ResultMFC16textEdit.Text = lcd[6].ToString("0.0");
                //            ResultMOtextEdit.Text = lcd[7].ToString("0.0");
                //            ResultPressuretextEdit.Text = lcd[8].ToString("0.0");
                //            ResultTemperaturetextEdit.Text = lcd[9].ToString("0.0");
                //            ResultSpeedtextEdit.Text = lcd[10].ToString("0.0");

                //            ResultInfo.TroubleShootingReportTxt = RepoetTxt.Text;
                //            ResultInfo.TroubleShootingResultMFC04textEdit = ResultMFC04textEdit.Text;
                //            ResultInfo.TroubleShootingResultH2OtextEdit = ResultH2OtextEdit.Text;
                //            ResultInfo.TroubleShootingResultMFC12textEdit = ResultMFC12textEdit.Text;
                //            ResultInfo.TroubleShootingResultMFC13textEdit = ResultMFC13textEdit.Text;
                //            ResultInfo.TroubleShootingResultMFC14textEdit = ResultMFC14textEdit.Text;
                //            ResultInfo.TroubleShootingResultMFC15textEdit = ResultMFC15textEdit.Text;
                //            ResultInfo.TroubleShootingResultMFC16textEdit = ResultMFC16textEdit.Text;
                //            ResultInfo.TroubleShootingtMOtextEdit = ResultMOtextEdit.Text;
                //            ResultInfo.TroubleShootingPressuretextEdit = ResultPressuretextEdit.Text;
                //            ResultInfo.TroubleShootingResultTemperaturetextEdit = ResultTemperaturetextEdit.Text;
                //            ResultInfo.TroubleShootingResultSpeedtextEdit = ResultSpeedtextEdit.Text;

                //        }

                //        await SimulateLoadingAsync();
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
                loadingOverlay.Close();
                loadingOverlay.Dispose();
            }
        }
        //private async void ShootingBtn_Click(object sender, EventArgs e)
        //{
        //    if (isLoading) return;
        //    isLoading = true;

        //    // 创建并显示透明窗体
        //    LoadingOverlayForm loadingOverlay = new LoadingOverlayForm(this);
        //    loadingOverlay.Show();

        //    btnToggleControls(false);
        //    try
        //    {
        //        RepoetTxt.Text = "";
        //        string onnx_path = string.Format("{0}", Application.StartupPath + "\\Model\\simple_model.onnx");

        //        // 获取界面输入
        //        double optMfc03 = double.Parse(MFC03textEdit.Text);
        //        List<double> process = new List<double>
        //        {
        //            double.Parse(UpMFC04textEdit.Text.ToString()),
        //            double.Parse(UpH2OtextEdit.Text.ToString()),
        //            double.Parse(UpMFC12textEdit.Text.ToString()),
        //            double.Parse(UpMFC13textEdit.Text.ToString()),
        //            double.Parse(UpMFC14textEdit.Text.ToString()),
        //            double.Parse(UpMFC15textEdit.Text.ToString()),
        //            double.Parse(UpMFC16textEdit.Text.ToString()),
        //            double.Parse(UpMOtextEdit.Text.ToString()),
        //            double.Parse(UpPressuretextEdit.Text.ToString()),
        //            double.Parse(UpTemperaturetextEdit.Text.ToString()),
        //            double.Parse(UpSpeedtextEdit.Text.ToString())
        //        };

        //        List<double> result = new List<double>
        //        {
        //            double.Parse(I1textEdit.Text.ToString()),
        //            double.Parse(I2textEdit.Text.ToString()),
        //            double.Parse(I3textEdit.Text.ToString()),
        //            double.Parse(M1textEdit.Text.ToString()),
        //            double.Parse(M2textEdit.Text.ToString()),
        //            double.Parse(M3textEdit.Text.ToString()),
        //            double.Parse(O1textEdit.Text.ToString()),
        //            double.Parse(O2textEdit.Text.ToString()),
        //            double.Parse(O3textEdit.Text.ToString())
        //        };
        //        // 定义输入范围
        //        List<int> upInput = new List<int> { 5000, 200, 1200, 200, 1200, 1200, 1200, 1200, 30, 800, 1000 };
        //        List<int> lowInput = new List<int> { 3000, 50, 50, 50, 50, 50, 50, 50, 1, 200, 0 };

        //        // 创建 ONNX 会话
        //        using (var session = new InferenceSession(Path.Combine(Application.StartupPath, "Model", "simple_model.onnx")))
        //        {
        //            // 获取模型输入名称
        //            var inputName = session.InputMetadata.Keys.FirstOrDefault();
        //            if (string.IsNullOrEmpty(inputName))
        //            {
        //                MessageBox.Show("无法获取ONNX模型的输入名称");
        //                return;
        //            }
        //            // 异步执行优化计算
        //            var (bestValue, bestResult) = await Task.Run(() =>
        //            {
        //                List<double> localBestValue = new List<double>();
        //                List<double> localBestResult = new List<double>();
        //                for (int i = 0; i < process.Count; i++)
        //                {
        //                    double minMse = double.MaxValue;
        //                    double bestX = 0;

        //                    for (double t = upInput[i]; t <= lowInput[i]; t++)
        //                    {
        //                        List<double> x = new List<double>(process);
        //                        x[i] = t;

        //                        // 计算相关参数
        //                        double n2 = (x[0] - x[1]) / x[0];
        //                        double o2 = x[1] / x[0];
        //                        double vh2o = (1.39 * (optMfc03 + x[0] * n2) + x[0] * o2) * 0.000001 / 0.1988 / 60 * 760 * 300 / 273 / x[8];
        //                        double vmfc12 = x[2] * 1.39 * 0.000001 / 0.001195 / 60 * 760 * 300 / 273 / x[8];
        //                        double vmfc13 = x[4] * 1.39 * 0.000001 / 0.001195 / 60 * 760 * 300 / 273 / x[8];
        //                        double vmfc14 = x[5] * 1.39 * 0.000001 / 0.001195 / 60 * 760 * 300 / 273 / x[8];
        //                        double vmfc15 = x[6] * 1.39 * 0.000001 / 0.001195 / 60 * 760 * 300 / 273 / x[8];
        //                        double vmfc16 = x[7] * 1.39 * 0.000001 / 0.001195 / 60 * 760 * 300 / 273 / x[8];
        //                        double massMo = x[3] * 15.9645 / (600 - 15.9645) / (x[2] + x[4] + x[5] + x[6] + x[7]);
        //                        double massH2o = x[1] * 23.76 / (600 - 23.76) / x[0];
        //                        double pressure = x[8];
        //                        double ktemperature = x[9] + 273.15;
        //                        double rotationalSpeed = x[10];

        //                        List<double> y = new List<double>
        //                {
        //                        vh2o, massH2o, vmfc12, massMo, vmfc13,
        //                        vmfc14, vmfc15, vmfc16, pressure, ktemperature, rotationalSpeed
        //                };

        //                        // 准备输入张量
        //                        var inputTensor = new DenseTensor<float>(new float[y.Count], new int[] { 1, y.Count });
        //                        for (int j = 0; j < y.Count; j++)
        //                        {
        //                            inputTensor[0, j] = (float)y[j];
        //                        }

        //                        var inputs = new List<NamedOnnxValue>
        //                {
        //                        NamedOnnxValue.CreateFromTensor(inputName, inputTensor)
        //                };

        //                        // 运行模型
        //                        using (var outputs = session.Run(inputs))
        //                        {
        //                            var output = outputs.First().AsTensor<float>();
        //                            List<double> nnPredict = new List<double>();
        //                            foreach (var value in output)
        //                            {
        //                                nnPredict.Add(value / 1500);
        //                            }

        //                            // 计算 MSE
        //                            double mse = CalculateMse(result, nnPredict);

        //                            // 记录最小 MSE 对应的输入值
        //                            if (mse < minMse)
        //                            {
        //                                minMse = mse;
        //                                bestX = x[i];
        //                            }
        //                        }
        //                    }

        //                    localBestValue.Add(bestX);
        //                    localBestResult.Add(minMse);
        //                }
        //                return (localBestValue, localBestResult);
        //            });
        //            // 处理结果
        //            double[] bestResultArray = bestResult.Select(r => 1 / r).ToArray();
        //            double sum = bestResultArray.Sum();
        //            double[] normalizedResult = bestResultArray.Select(r => r / sum).ToArray();
        //            List<string> percent = normalizedResult.Select(r => $"{r:P2}").ToList();
        //            List<double> lcd = normalizedResult.Select(r => Math.Round(r * 100, 1)).ToList();
        //            double averageresult = CalculateAverage(lcd);
        //            // 显示结果
        //            string plainTextEdit4Text = "Shooting Finished! The unit of result is " + Math.Round(averageresult, 2) + " %.";
        //            RepoetTxt.Text = plainTextEdit4Text;
        //            // 显示优化结果
        //            ResultMFC04textEdit.Text = lcd[0].ToString("0.0");
        //            ResultH2OtextEdit.Text = lcd[1].ToString("0.0");
        //            ResultMFC12textEdit.Text = lcd[2].ToString("0.0");
        //            ResultMFC13textEdit.Text = lcd[3].ToString("0.0");
        //            ResultMFC14textEdit.Text = lcd[4].ToString("0.0");
        //            ResultMFC15textEdit.Text = lcd[5].ToString("0.0");
        //            ResultMFC16textEdit.Text = lcd[6].ToString("0.0");
        //            ResultMOtextEdit.Text = lcd[7].ToString("0.0");
        //            ResultPressuretextEdit.Text = lcd[8].ToString("0.0");
        //            ResultTemperaturetextEdit.Text = lcd[9].ToString("0.0");
        //            ResultSpeedtextEdit.Text = lcd[10].ToString("0.0");

        //            ResultInfo.TroubleShootingReportTxt = RepoetTxt.Text;
        //            ResultInfo.TroubleShootingResultMFC04textEdit = ResultMFC04textEdit.Text;
        //            ResultInfo.TroubleShootingResultH2OtextEdit = ResultH2OtextEdit.Text;
        //            ResultInfo.TroubleShootingResultMFC12textEdit = ResultMFC12textEdit.Text;
        //            ResultInfo.TroubleShootingResultMFC13textEdit = ResultMFC13textEdit.Text;
        //            ResultInfo.TroubleShootingResultMFC14textEdit = ResultMFC14textEdit.Text;
        //            ResultInfo.TroubleShootingResultMFC15textEdit = ResultMFC15textEdit.Text;
        //            ResultInfo.TroubleShootingResultMFC16textEdit = ResultMFC16textEdit.Text;
        //            ResultInfo.TroubleShootingtMOtextEdit = ResultMOtextEdit.Text;
        //            ResultInfo.TroubleShootingPressuretextEdit = ResultPressuretextEdit.Text;
        //            ResultInfo.TroubleShootingResultTemperaturetextEdit = ResultTemperaturetextEdit.Text;
        //            ResultInfo.TroubleShootingResultSpeedtextEdit = ResultSpeedtextEdit.Text;

        //        }

        //        // 异步加载 ONNX 模型
        //        // var session = await Task.Run(() => new InferenceSession(onnx_path));
        //        //// 异步执行优化计算
        //        //var (bestValue, bestResult) = await Task.Run(() =>
        //        //{
        //        //    List<double> localBestValue = new List<double>();
        //        //    List<double> localBestResult = new List<double>();
        //        //    for (int i = 0; i < process.Count; i++)
        //        //    {
        //        //        double minMse = double.MaxValue;
        //        //        double bestX = 0;

        //        //        for (int t = lowInput[i]; t <= upInput[i]; t++)
        //        //        {
        //        //            List<double> x = new List<double>(process);
        //        //            x[i] = t;

        //        //            // 计算相关参数
        //        //            double n2 = (x[0] - x[1]) / x[0];
        //        //            double o2 = x[1] / x[0];
        //        //            double vh2o = (1.39 * (optMfc03 + x[0] * n2) + x[0] * o2) * 0.000001 / 0.1988 / 60 * 760 * 300 / 273 / x[8];
        //        //            double vmfc12 = x[2] * 1.39 * 0.000001 / 0.001195 / 60 * 760 * 300 / 273 / x[8];
        //        //            double vmfc13 = x[4] * 1.39 * 0.000001 / 0.001195 / 60 * 760 * 300 / 273 / x[8];
        //        //            double vmfc14 = x[5] * 1.39 * 0.000001 / 0.001195 / 60 * 760 * 300 / 273 / x[8];
        //        //            double vmfc15 = x[6] * 1.39 * 0.000001 / 0.001195 / 60 * 760 * 300 / 273 / x[8];
        //        //            double vmfc16 = x[7] * 1.39 * 0.000001 / 0.001195 / 60 * 760 * 300 / 273 / x[8];
        //        //            double massMo = x[3] * 15.9645 / (600 - 15.9645) / (x[2] + x[4] + x[5] + x[6] + x[7]);
        //        //            double massH2o = x[1] * 23.76 / (600 - 23.76) / x[0];
        //        //            double pressure = x[8];
        //        //            double ktemperature = x[9] + 273.15;
        //        //            double rotationalSpeed = x[10];

        //        //            List<double> y = new List<double>
        //        //            {
        //        //                vh2o, massH2o, vmfc12, massMo, vmfc13,
        //        //                vmfc14, vmfc15, vmfc16, pressure, ktemperature, rotationalSpeed
        //        //            };

        //        //            // 准备输入张量
        //        //            var inputTensor = new DenseTensor<float>(new float[y.Count], new int[] { 1, y.Count });
        //        //            for (int j = 0; j < y.Count; j++)
        //        //            {
        //        //                inputTensor[0, j] = (float)y[j];
        //        //            }

        //        //            var inputs = new List<NamedOnnxValue>
        //        //            {
        //        //                NamedOnnxValue.CreateFromTensor("input", inputTensor)
        //        //            };

        //        //            // 运行模型
        //        //            using (var outputs = session.Run(inputs))
        //        //            {
        //        //                var output = outputs.First().AsTensor<float>();
        //        //                List<double> nnPredict = new List<double>();
        //        //                foreach (var value in output)
        //        //                {
        //        //                    nnPredict.Add(value / 1500);
        //        //                }

        //        //                // 计算 MSE
        //        //                double mse = CalculateMse(result, nnPredict);

        //        //                // 记录最小 MSE 对应的输入值
        //        //                if (mse < minMse)
        //        //                {
        //        //                    minMse = mse;
        //        //                    bestX = x[i];
        //        //                }
        //        //            }
        //        //        }

        //        //        localBestValue.Add(bestX);
        //        //        localBestResult.Add(minMse);
        //        //    }
        //        //    return (localBestValue, localBestResult);
        //        //});

        //        //// 处理结果
        //        //double[] bestResultArray = bestResult.Select(r => 1 / r).ToArray();
        //        //double sum = bestResultArray.Sum();
        //        //double[] normalizedResult = bestResultArray.Select(r => r / sum).ToArray();
        //        //List<string> percent = normalizedResult.Select(r => $"{r:P2}").ToList();
        //        //List<double> lcd = normalizedResult.Select(r => Math.Round(r * 100, 1)).ToList();
        //        //double averageresult = CalculateAverage(lcd);
        //        //// 显示结果
        //        //string plainTextEdit4Text = "Shooting Finished! The unit of result is " + Math.Round(averageresult, 2) + " %.";
        //        //RepoetTxt.Text = plainTextEdit4Text;
        //        //// 显示优化结果
        //        //ResultMFC04textEdit.Text = lcd[0].ToString("0.0");
        //        //ResultH2OtextEdit.Text = lcd[1].ToString("0.0");
        //        //ResultMFC12textEdit.Text = lcd[2].ToString("0.0");
        //        //ResultMFC13textEdit.Text = lcd[3].ToString("0.0");
        //        //ResultMFC14textEdit.Text = lcd[4].ToString("0.0");
        //        //ResultMFC15textEdit.Text = lcd[5].ToString("0.0");
        //        //ResultMFC16textEdit.Text = lcd[6].ToString("0.0");
        //        //ResultMOtextEdit.Text = lcd[7].ToString("0.0");
        //        //ResultPressuretextEdit.Text = lcd[8].ToString("0.0");
        //        //ResultTemperaturetextEdit.Text = lcd[9].ToString("0.0");
        //        //ResultSpeedtextEdit.Text = lcd[10].ToString("0.0");

        //        //ResultInfo.TroubleShootingReportTxt = RepoetTxt.Text;
        //        //ResultInfo.TroubleShootingResultMFC04textEdit = ResultMFC04textEdit.Text;
        //        //ResultInfo.TroubleShootingResultH2OtextEdit = ResultH2OtextEdit.Text;
        //        //ResultInfo.TroubleShootingResultMFC12textEdit = ResultMFC12textEdit.Text;
        //        //ResultInfo.TroubleShootingResultMFC13textEdit = ResultMFC13textEdit.Text;
        //        //ResultInfo.TroubleShootingResultMFC14textEdit = ResultMFC14textEdit.Text;
        //        //ResultInfo.TroubleShootingResultMFC15textEdit = ResultMFC15textEdit.Text;
        //        //ResultInfo.TroubleShootingResultMFC16textEdit = ResultMFC16textEdit.Text;
        //        //ResultInfo.TroubleShootingtMOtextEdit = ResultMOtextEdit.Text;
        //        //ResultInfo.TroubleShootingPressuretextEdit = ResultPressuretextEdit.Text;
        //        //ResultInfo.TroubleShootingResultTemperaturetextEdit = ResultTemperaturetextEdit.Text;
        //        //ResultInfo.TroubleShootingResultSpeedtextEdit = ResultSpeedtextEdit.Text;
        //        await SimulateLoadingAsync(); // 模拟加载
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show($"加载时出错: {ex.Message}");
        //    }
        //    finally
        //    {
        //        isLoading = false;
        //        btnToggleControls(true);

        //        // 关闭透明窗体
        //        loadingOverlay.Close();
        //        loadingOverlay.Dispose();
        //    }
        //}
        static double CalculateAverage(List<double> numbers)
        {
            if (numbers.Count == 0)
            {
                return 0;
            }

            double sum = 0;
            foreach (double number in numbers)
            {
                sum += number;
            }

            return sum / numbers.Count;
        }
        private double CalculateMse(List<double> result, List<double> nnPredict)
        {
            double sum = 0;
            for (int i = 0; i < result.Count; i++)
            {
                sum += Math.Pow(result[i] - nnPredict[i], 2);
            }
            return sum / result.Count;
        }
        private void ShootingBtn_MouseEnter(object sender, EventArgs e)
        {
            ShootingBtn.BackColor = Color.SteelBlue;
            ShootingBtn.ForeColor = Color.White;
        }

        private void ShootingBtn_MouseLeave(object sender, EventArgs e)
        {
            ShootingBtn.BackColor = Color.Transparent;
            ShootingBtn.ForeColor = Color.White;
        }
        private void ExportReportBtn_MouseEnter(object sender, EventArgs e)
        {
            ShootingBtn.BackColor = Color.SteelBlue;
            ShootingBtn.ForeColor = Color.White;
        }

        private void ExportReportBtn_MouseLeave(object sender, EventArgs e)
        {
            ShootingBtn.BackColor = Color.Transparent;
            ShootingBtn.ForeColor = Color.White;
        }
        private void ExportReportBtn_Click(object sender, EventArgs e)
        {

            try
            {
                Word.Application wordApp = new Word.Application();
                wordApp.Visible = false;

                Word.Document wordDoc = wordApp.Documents.Add();

                // 添加居中标题（取消标题后空行）
                Word.Paragraph titlePara = wordDoc.Content.Paragraphs.Add();
                titlePara.Range.Text = "溯源报告";
                titlePara.Range.Font.Size = 16;
                titlePara.Range.Font.Bold = 1;
                titlePara.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;

                // 添加工艺参数部分（取消所有空行）
                AddSection(wordDoc, "一、工艺参数", new string[]
                {
                    "MFC03: " + MFC03textEdit.Text,
                    "Sum of MFCO4-MFC09: " + UpMFC04textEdit.Text,
                    "H20: " + UpH2OtextEdit.Text,
                     "MFC12: " + UpMFC12textEdit.Text,
                    "MFC13: " + UpMFC13textEdit.Text,
                    "MFC14: " + UpMFC14textEdit.Text,
                     "MFC15: " + UpMFC15textEdit.Text,
                    "MFC16: " + UpMFC16textEdit.Text,
                    "MO: " + UpMOtextEdit.Text,
                     "Pressure: " + UpPressuretextEdit.Text,
                    "Temperature: " + UpTemperaturetextEdit.Text,
                    "Rotational speed: " + UpSpeedtextEdit.Text,
                });

                // 添加异常结果部分
                AddSection(wordDoc, "二、异常结果", new string[]
                {
                    "i1: " + I1textEdit.Text,
                    "i2: " + I2textEdit.Text,
                    "i3: " + I3textEdit.Text,
                    "m1: " + M1textEdit.Text,
                    "m2: " + M2textEdit.Text,
                    "m3: " + M3textEdit.Text,
                    "o1: " + O1textEdit.Text,
                    "o2: " + O2textEdit.Text,
                    "o3: " + O3textEdit.Text
                });

                // 添加报告部分
                AddSection(wordDoc, "三、报告", new string[]
                {
                    "报告: " + RepoetTxt.Text
                });

                // 添加溯源结果部分
                AddSection(wordDoc, "四、溯源结果", new string[]
                {
                     "Sum of MFCO4-MFC09: " + ResultMFC04textEdit.Text,
                    "H20: " + ResultH2OtextEdit.Text,
                     "MFC12: " + ResultMFC12textEdit.Text,
                    "MFC13: " + ResultMFC13textEdit.Text,
                    "MFC14: " + ResultMFC14textEdit.Text,
                     "MFC15: " + ResultMFC15textEdit.Text,
                    "MFC16: " + ResultMFC16textEdit.Text,
                    "MO: " + ResultMOtextEdit.Text,
                     "Pressure: " + ResultPressuretextEdit.Text,
                    "Temperature: " + ResultTemperaturetextEdit.Text,
                    "Rotational speed: " + ResultSpeedtextEdit.Text,
                });

                // 保存文档
                SaveFileDialog saveDialog = new SaveFileDialog();
                saveDialog.Filter = "Word文档 (*.docx)|*.docx|所有文件 (*.*)|*.*";
                saveDialog.Title = "保存溯源报告";
                saveDialog.FileName = "溯源报告_" + DateTime.Now.ToString("yyyyMMddHHmmss");

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    wordDoc.SaveAs2(saveDialog.FileName);
                    MessageBox.Show("文档已成功导出！", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                wordDoc.Close();
                wordApp.Quit();

                System.Runtime.InteropServices.Marshal.ReleaseComObject(wordDoc);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(wordApp);
            }
            catch (Exception ex)
            {
                MessageBox.Show("导出文档时发生错误: " + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AddSection(Word.Document doc, string title, string[] contentItems)
        {
            // 添加章节标题（左对齐，取消空行）
            Word.Paragraph sectionTitle = doc.Content.Paragraphs.Add();
            sectionTitle.Range.Text = title;
            sectionTitle.Range.Font.Size = 14;
            sectionTitle.Range.Font.Bold = 1;
            sectionTitle.Alignment = Word.WdParagraphAlignment.wdAlignParagraphLeft;

            // 添加内容项（取消所有空行，使用回车符连接）
            string contentText = string.Join(Environment.NewLine, contentItems);
            Word.Paragraph contentPara = doc.Content.Paragraphs.Add();
            contentPara.Range.Text = contentText;
            contentPara.Format.SpaceAfter = 0; // 确保段落间距为0
        }
    }
}