using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;
using Microsoft.VisualBasic.FileIO;
using Python.Runtime;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using Tensorflow;
using WisdomGrowth.DataBase;
using Microsoft.ML;
using Microsoft.ML.Data;
using System.Diagnostics;
namespace WisdomGrowth
{

    public partial class MainSystemForm : Form
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
        int currentImageIndex = 0;
        Image[] images = new Image[3];
        int currentImageIndex1 = 0;
        Image[] images1 = new Image[3];
        AutoResizeForm asc = new AutoResizeForm();
        public MainSystemForm()
        {
            InitializeComponent();
            // 设置双缓冲以减少闪烁
            this.DoubleBuffered = true;
            // 初始化定时器
            animationTimer = new Timer();
            animationTimer.Interval = AnimationSpeed;
            animationTimer.Tick += Timer_Tick;
            DirectoryInfo directoryInfo = new DirectoryInfo(Application.StartupPath);
            DirectoryInfo parentDirectory = directoryInfo.Parent;
            string parentPath = parentDirectory.FullName;
            //string imageresult = parentPath + "\\Debug\\File";
            string imageresult = Application.StartupPath + "\\File";
            // 加载图片资源
            images[0] = Image.FromFile(imageresult + "\\1.gif"); // 替换为图片1的路径
            images[1] = Image.FromFile(imageresult + "\\1-t.bmp"); // 替换为图片2的路径
            images[2] = Image.FromFile(imageresult + "\\1-v.bmp"); // 替换为图片3的路径

            images1[0] = Image.FromFile(imageresult + "\\downresult3.png"); // 替换为图片1的路径
            images1[1] = Image.FromFile(imageresult + "\\downresult2.png"); // 替换为图片2的路径
            // 初始显示第一张图片
            FlowpictureBox.Image = images[0];
            CrowthpictureBox.BackgroundImage = images1[0];

        }
        private InferenceSession _onnxSession; // ONNX模型会话
         // 从Python模型导出的标准化参数（需替换为实际值）
        private readonly double[] Means = { 0.023, 0.156, 0.089, 0.221, 0.112, 0.098, 0.145, 0.076, 1.2, 300.0, 500.0 };
        private readonly double[] Stds = { 0.005, 0.032, 0.021, 0.045, 0.033, 0.027, 0.038, 0.022, 0.3, 50.0, 200.0 };
        private readonly double Scale = 1.0;  // 模型输出逆转换参数（需与Python一致）
        private readonly double Offset = 0.0;
        // 标准化输入（匹配Python的StandardScaler）
        private double[] StandardizeInput(double[] input, double[] means, double[] stds)
        {
            if (input.Length != means.Length)
                throw new ArgumentException("输入特征数量与标准化参数不匹配");

            return input.Zip(means, (x, m) => x - m)
                        .Zip(stds, (xMinusM, s) => xMinusM / s)
                        .ToArray();
        }
        // 查找最相似行（欧氏距离，与Python的KNN逻辑一致）
        private int FindClosestRow(DataTable dataTable, double[] target)
        {
            double minDistance = double.MaxValue;
            //minDistance = 10000000;
            int closestIndex = -1;

            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                DataRow row = dataTable.Rows[i];
                double[] rowValues = new double[11];
                for (int j = 0; j < 11; j++)
                {
                    rowValues[j] = double.Parse(row[j].ToString());
                    if (j == 0)
                    {
                        if (Convert.ToDouble(row[j]) > 1.2916 && Convert.ToDouble(row[j]) < 1.2917)
                        {
                            //closestIndex = i;
                            //double tempd = Convert.ToDouble(row[j]);
                        }
                    }
                }
                List<double> tempList = rowValues.ToList();
                StandardScaler scaler = new StandardScaler();
                double[] standardizedData = scaler.FitTransform(tempList);
                rowValues = standardizedData.ToArray();
                // 计算欧氏距离（与Python的euclidean_distances一致）
                //double distance = Math.Sqrt(rowValues.Zip(target, (v, t) => Math.Pow(v - t, 2)).Sum());
                double distance = Math.Sqrt(rowValues.Zip(target, (v, t) => Math.Pow(v - t, 2)).Sum());
                if (distance < minDistance)
                {
                    minDistance = distance;
                    closestIndex = i;
                }
            }
            return closestIndex;
        }
        // ONNX模型预测（匹配Python的PyTorch模型输出）
        // 模型预测方法（兼容 C# 7.3）
        private double[] PredictWithOnnxModel1(double[] input)
        {
            // 创建 ONNX 会话
            using (var session = new InferenceSession(Path.Combine(Application.StartupPath, "Model", "simple_model.onnx")))
            {
                // 准备输入张量
                var tensor = new DenseTensor<float>(new[] { 1, 11 });
                for (int i = 0; i < 11; i++)
                {
                    tensor[0, i] = (float)input[i];
                }

                // 运行模型推理
                var inputs = new List<NamedOnnxValue> { NamedOnnxValue.CreateFromTensor("input", tensor) };

                using (var outputs = session.Run(inputs))
                {
                    // 获取输出并转换为数组
                    float[] rawOutput = outputs.First().AsTensor<float>().ToArray();

                    // 后处理（确保类型转换正确）
                    double[] processedOutput = new double[rawOutput.Length];
                    for (int i = 0; i < rawOutput.Length; i++)
                    {
                        processedOutput[i] = ((rawOutput[i] * (float)Scale) + (float)Offset) / 1500.0f;
                    }

                    return processedOutput;
                }
            }
        }
        private async void PredictBtn_Click(object sender, EventArgs e)
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
                // 异步获取输入值
                var inputValuesTask = Task.Run(() =>
                {
                    double MFC04text = double.Parse(MFC04textEdit.Text.ToString());
                    double MFC05text = double.Parse(MFC05textEdit.Text.ToString());
                    double MFC06text = double.Parse(MFC06textEdit.Text.ToString());
                    double MFC07text = double.Parse(MFC07textEdit.Text.ToString());
                    double MFC08text = double.Parse(MFC08textEdit.Text.ToString());
                    double MFC09text = double.Parse(MFC09textEdit.Text.ToString());
                    double MFC03text = double.Parse(MFC03textEdit.Text.ToString());
                    double MFC12text = double.Parse(MFC12textEdit.Text.ToString());
                    double MFC13text = double.Parse(MFC13textEdit.Text.ToString());
                    double MFC14text = double.Parse(MFC14textEdit.Text.ToString());
                    double MFC15text = double.Parse(MFC15textEdit.Text.ToString());
                    double MFC16text = double.Parse(MFC16textEdit.Text.ToString());
                    double MOtext = double.Parse(MOtextEdit.Text.ToString());
                    double H2Otext = double.Parse(H2OtextEdit.Text.ToString());
                    double Pressuretext = double.Parse(PressuretextEdit.Text.ToString());
                    double Temperaturetext = double.Parse(TemperaturetextEdit.Text.ToString());
                    double Rotationaltext = double.Parse(RotationaltextEdit.Text.ToString());
                    return new
                    {
                        MFC04text,
                        MFC05text,
                        MFC06text,
                        MFC07text,
                        MFC08text,
                        MFC09text,
                        MFC03text,
                        MFC12text,
                        MFC13text,
                        MFC14text,
                        MFC15text,
                        MFC16text,
                        MOtext,
                        H2Otext,
                        Pressuretext,
                        Temperaturetext,
                        Rotationaltext
                    };
                });


                var inputValues = await inputValuesTask;

                // 异步计算中间值
                var calculationTask = Task.Run(() =>
                {
                    double sumMFC0409 = inputValues.MFC04text + inputValues.MFC05text + inputValues.MFC06text + inputValues.MFC07text + inputValues.MFC08text + inputValues.MFC09text;
                    double N2 = (sumMFC0409 - inputValues.H2Otext) / sumMFC0409;
                    double O2 = inputValues.H2Otext / sumMFC0409;
                    double VH2O = (1.39 * (inputValues.MFC03text + sumMFC0409 * N2) + sumMFC0409 * O2) * 0.000001 / 0.1988 / 60 * 760 * 300 / 273 / inputValues.Pressuretext;
                    double VMFC12 = inputValues.MFC12text * 1.39 * 0.000001 / 0.001195 / 60 * 760 * 300 / 273 / inputValues.Pressuretext;
                    double VMFC13 = inputValues.MFC13text * 1.39 * 0.000001 / 0.001195 / 60 * 760 * 300 / 273 / inputValues.Pressuretext;
                    double VMFC14 = inputValues.MFC14text * 1.39 * 0.000001 / 0.001195 / 60 * 760 * 300 / 273 / inputValues.Pressuretext;
                    double VMFC15 = inputValues.MFC15text * 1.39 * 0.000001 / 0.001195 / 60 * 760 * 300 / 273 / inputValues.Pressuretext;
                    double VMFC16 = inputValues.MFC16text * 1.39 * 0.000001 / 0.001195 / 60 * 760 * 300 / 273 / inputValues.Pressuretext;
                    double sumMFC1216 = inputValues.MFC12text + inputValues.MFC13text + inputValues.MFC14text + inputValues.MFC15text + inputValues.MFC16text;
                    double MASS_MO = inputValues.MOtext * 15.9645 / (600 - 15.9645) / sumMFC1216;
                    double MASS_H2O = inputValues.H2Otext * 23.76 / (600 - 23.76) / sumMFC0409;
                    double KTemperature = inputValues.Temperaturetext + 273.15;
                    return new
                    {
                        N2,
                        O2,
                        VH2O,
                        VMFC12,
                        VMFC13,
                        VMFC14,
                        VMFC15,
                        VMFC16,
                        MASS_MO,
                        MASS_H2O,
                        KTemperature,
                        sumMFC0409
                    };
                });

                var calculationResult = await calculationTask;

                List<double> list = new List<double>
                {
                    calculationResult.VH2O,
                    calculationResult.MASS_H2O,
                    calculationResult.VMFC12,
                    calculationResult.MASS_MO,
                    calculationResult.VMFC13,
                    calculationResult.VMFC14,
                    calculationResult.VMFC15,
                    calculationResult.VMFC16,
                    inputValues.Pressuretext,
                    calculationResult.KTemperature,
                    inputValues.Rotationaltext
                };
                List<double> inputList = new List<double>
                {
                    calculationResult.VH2O,
                    calculationResult.MASS_H2O,
                    calculationResult.VMFC12,
                    calculationResult.MASS_MO,
                    calculationResult.VMFC13,
                    calculationResult.VMFC14,
                    calculationResult.VMFC15,
                    calculationResult.VMFC16,
                    inputValues.Pressuretext,
                    calculationResult.KTemperature,
                    inputValues.Rotationaltext
                };
                //// 4. 标准化输入（匹配Python的KNN_transfer）
                // 创建并使用标准化器
                StandardScaler scaler = new StandardScaler();
                double[] standardizedData = scaler.FitTransform(inputList);



                //double[] inputD = inputList.ToArray();
                //var trainData = new List<DataPoint>
                //{
                //    new DataPoint { Features =  inputD}
                //};

                //var context = new MLContext();
                //var trainDataView = context.Data.LoadFromEnumerable(trainData);
                //var pipeline = context.Transforms.Concatenate("Features", nameof(DataPoint.Features))
                //                        .Append(context.Transforms.NormalizeMinMax("Features"))
                //                        .Append(context.Transforms.Conversion.MapValueToKey("Label"))
                //                        .Append(context.Transforms.KNearestNeighbors("KNNOutput", "Label", "Features", k: 2));
                //var model = pipeline.Fit(trainDataView);




                // 5. 读取KNN参考数据（flow.csv）并查找最相似行
                //string csvPath = Path.Combine(Application.StartupPath, "File", "flow.csv");
                //DataTable knnData = await Task.Run(() => ConvertCsvToDataTable(csvPath));


                #region add by wpz 2025/9/25
                string csvPath = Path.Combine(Application.StartupPath, "File", "flow.csv");
                string pythonPath = Path.Combine(Application.StartupPath, "File", "predict.py");
                string outPath = Path.Combine(Application.StartupPath, "File", "temp.txt");
                string modelFileName = string.Format("{0}", Application.StartupPath + "\\Model\\model");
                string inputPath = string.Format("{0}", Application.StartupPath + "\\File\\input-ms-2022-3-14.csv");
                string outputPath = string.Format("{0}", Application.StartupPath + "\\File\\output-2022-3-14.csv");
                string[] strArr = new string[16];
                strArr[0] = csvPath;
                strArr[1] = modelFileName;
                strArr[2] = outPath;
                strArr[3] = calculationResult.VH2O.ToString();
                strArr[4] = calculationResult.MASS_H2O.ToString();
                strArr[5] = calculationResult.VMFC12.ToString();
                strArr[6] = calculationResult.MASS_MO.ToString();
                strArr[7] = calculationResult.VMFC13.ToString();
                strArr[8] = calculationResult.VMFC14.ToString();
                strArr[9] = calculationResult.VMFC15.ToString();
                strArr[10] = calculationResult.VMFC16.ToString();
                strArr[11] = inputValues.Pressuretext.ToString();
                strArr[12] = calculationResult.KTemperature.ToString();
                strArr[13] = inputValues.Rotationaltext.ToString();
                strArr[14] = inputPath;
                strArr[15] = outputPath;
                string sArguments = pythonPath + " " + strArr[0] + " " + strArr[1] + " " + strArr[2] + " " + strArr[3] + " " + strArr[4] + " " + strArr[5] + " " + strArr[6] + " " + strArr[7] + " " + strArr[8] + " " + strArr[9] + " " + strArr[10] + " " + strArr[11] + " " + strArr[12] + " " + strArr[13] + " " + strArr[14] + " " + strArr[15];

                #region test by 2025/11/4 
                string fullName = Path.Combine(Application.StartupPath, "File", "kshsztest.txt");
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
                int closestRowIndex1 = -1;
                double flowIndicator = 0d;
                string flowState = "";
                double NN_averagetext = 0d;
                double NN_Variable_Coefficienttext = 0d;
                double[] NN_predict = new double[9];
                int calcIndex = 0;
                if (File.Exists(outPath))
                {
                    string strCurrent = "";
                    using (BufferedStream bufferedStream = new BufferedStream(new FileStream(outPath, FileMode.Open, FileAccess.Read), 4096000))
                    {
                        using (StreamReader streamReader = new StreamReader(bufferedStream))
                        {
                            while ((strCurrent = streamReader.ReadLine()) != null)
                            {
                                if (strCurrent.Contains("indicator"))
                                    flowIndicator = Convert.ToDouble(strCurrent.ToString().Split(new char[] { ','})[1]);
                                if (strCurrent.Contains("picture"))
                                    closestRowIndex1 = Convert.ToInt32(strCurrent.ToString().Split(new char[] { ',' })[1]);
                                if (strCurrent.Contains("flowstate"))
                                    flowState = strCurrent.ToString().Split(new char[] { ',' })[1];
                                if (strCurrent.Contains("rate"))
                                    NN_averagetext = Convert.ToDouble(strCurrent.ToString().Split(new char[] { ',' })[1]);
                                if (strCurrent.Contains("uniformity"))
                                    NN_Variable_Coefficienttext = Convert.ToDouble(strCurrent.ToString().Split(new char[] { ',' })[1]);
                                if (strCurrent.Contains("predict"))
                                {
                                    if (strCurrent.Contains("]"))
                                    {
                                        string tempStr = strCurrent.ToString().Split(new char[] { ',' })[1];
                                        tempStr = tempStr.Replace("[", "").Replace("]", "");
                                        string[] subStr = tempStr.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                                        calcIndex = subStr.Length;
                                        for (int p = 0; p < subStr.Length; p++)
                                            NN_predict[p] = Convert.ToDouble(subStr[p]);
                                    }
                                    else
                                    {
                                        string tempStr = strCurrent.ToString().Split(new char[] { ',' })[1];
                                        tempStr = tempStr.Replace("[", "").Replace("]", "");
                                        string[] subStr = tempStr.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                                        calcIndex = subStr.Length;
                                        for (int p = 0; p < subStr.Length; p++)
                                            NN_predict[p] = Convert.ToDouble(subStr[p]);
                                        strCurrent = streamReader.ReadLine();
                                        tempStr = tempStr.Replace("[", "").Replace("]", "");
                                        subStr = tempStr.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                                        for(int p = calcIndex;p < 9;p++)
                                            NN_predict[p] = Convert.ToDouble(subStr[p-calcIndex]);

                                    }
                                }
                            }
                        }
                    }
                }
                #endregion


                //int closestRowIndex1 = await Task.Run(() => FindClosestRow(knnData, standardizedData));
                //if (closestRowIndex1 == -1)
                //{
                //    MessageBox.Show("未找到匹配数据，请检查输入！");
                //    return;
                //}
                // 6. 解析流场结果（与Python标签映射一致）
                //DataRow closestRow1 = knnData.Rows[closestRowIndex1];
                //double flowIndicator = Math.Round(double.Parse(closestRow1[0].ToString()), 4);
                //string flowType = closestRow1[11].ToString();
                //string flowState;
                //switch (flowType)
                //{
                //    case "A":
                //        flowState = "Plug flow";
                //        break;
                //    case "B":
                //        flowState = "Buoyancy flow";
                //        break;
                //    case "C":
                //        flowState = "Rotational induced flow";
                //        break;
                //    case "V":
                //        flowState = "Transition flow";
                //        break;
                //    default:
                //        flowState = "Error";
                //        break;
                //}
               
                //int imageclosestRowIndex = closestRowIndex1 + 2;
                int imageclosestRowIndex = closestRowIndex1;
                string imageresult1name = string.Format("{0}.gif", imageclosestRowIndex);
                string imageresul2tname = string.Format("{0}-t.bmp", imageclosestRowIndex);
                string imageresul3tname = string.Format("{0}-v.bmp", imageclosestRowIndex);
                DirectoryInfo directoryInfo = new DirectoryInfo(Application.StartupPath);
                DirectoryInfo parentDirectory = directoryInfo.Parent;
                string parentPath = parentDirectory.FullName;
                string imageresult1 = Application.StartupPath + "\\gif";
                string imageresult2 = Application.StartupPath + "\\pic-T";
                string imageresult3 = Application.StartupPath + "\\pic-V";
                // 加载图片资源
                images[0] = Image.FromFile(imageresult1 + "\\" + imageresult1name); // 替换为图片1的路径
                images[1] = Image.FromFile(imageresult2 + "\\" + imageresul2tname); // 替换为图片2的路径
                images[2] = Image.FromFile(imageresult3 + "\\" + imageresul3tname); // 替换为图片3的路径

                FlowpictureBox.Image.Dispose();
                FlowpictureBox.Image = null;
                FlowpictureBox.Image = images[0];
                FlowpictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
                //string modelFileName = string.Format("{0}", Application.StartupPath + "\\Model\\model");
                Visualization.selectedmodelPath = modelFileName;

                ////  ONNX模型预测沉积速率（匹配Python的NN模型）
                //double[] NN_predict = await Task.Run(() => PredictWithOnnxModel1(standardizedData));
                // 一维绘图
                chart1.Series.Clear();
                Series series1 = new Series("Deposition Rate");
                series1.ChartType = SeriesChartType.Line;
                series1.Color = Color.Black;
                series1.BorderWidth = 2;
                series1.BorderDashStyle = ChartDashStyle.DashDot;
                //for (int i = 0; i < 11; i++) // temp hide by wpz 2025/9/26
                //{
                //    series1.Points.AddXY(i + 1, NN_predict[i]);
                //}
                for (int i = 0; i < 9; i++)
                {
                    series1.Points.AddXY(i + 1, NN_predict[i]);
                }
                chart1.Series.Add(series1);
                chart1.ChartAreas[0].AxisY.Minimum = 0;
                chart1.ChartAreas[0].AxisY.Maximum = 1.5 * NN_predict.Max();
                // 设置纵轴标签保留一位小数
                chart1.ChartAreas[0].AxisY.LabelStyle.Format = "0.0000";
                chart1.ChartAreas[0].AxisX.Title = "9 points deposition rate distribution";
                chart1.ChartAreas[0].AxisY.Title = "Deposition rate (μm/h)";
                // 隐藏图表的图例
                chart1.Legends[0].Enabled = false;
                string savePath = Application.StartupPath + "\\File\\Distribution.jpg";

                if (File.Exists(savePath))
                {
                    if (CrowthpictureBox.BackgroundImage == images1[2])
                    {
                        CrowthpictureBox.BackgroundImage.Dispose();
                        CrowthpictureBox.BackgroundImage = null;
                    }
                    else
                    {
                        if (images1[2] != null)
                        {
                            images1[2].Dispose();
                            images1[2] = null;
                        }
                    }
                    File.Delete(savePath);
                }

                // 异步保存图表
                var saveChartTask = Task.Run(() => chart1.SaveImage(savePath, ChartImageFormat.Jpeg));
                await saveChartTask;

                // 计算平均值、标准差和变异系数 
                //double NN_average = NN_predict.Average();
                //double NN_std = Math.Sqrt(NN_predict.Select(x => Math.Pow(x - NN_average, 2)).Sum() / (NN_predict.Length - 1));
                //double NN_Variable_Coefficient = NN_std / NN_average;

                string imageresult = Application.StartupPath + "\\File";
                images1[0] = Image.FromFile(imageresult + "\\downresult3.png"); // 替换为图片1的路径
                images1[1] = Image.FromFile(imageresult + "\\downresult2.png"); // 替换为图片2的路径
                images1[2] = Image.FromFile(savePath); // 替换为图片2的路径
                                                       // 显示图片
                CrowthpictureBox.BackgroundImage = images1[2];
                CrowthpictureBox.SizeMode = PictureBoxSizeMode.StretchImage;


                //double NN_averagetext = Math.Round(NN_average, 2);
                //NN_averagetext = Math.Abs(NN_averagetext);
                //double NN_Variable_Coefficienttext = Math.Round(NN_Variable_Coefficient, 2);
                //NN_Variable_Coefficienttext = Math.Abs(NN_Variable_Coefficienttext);
                this.ReportTxt.Text = "";
                this.ReportTxt.Text += "Flow Field indicator:" + flowIndicator.ToString() + "\r\n" + flowState + "\r\n" + "Deposition rate:" + NN_averagetext + "um/h" + "\r\n" + "Uniformity (Coefficient of variation):" + NN_Variable_Coefficienttext + "%" + "\r\n";
                //this.ReportTxt.Text += "Flow Field Indicator:" + flowIndicator.ToString("F4") + "\r\n"+ flowState + "\r\n"+ "Deposition rate:" + NN_average.ToString("F2") + "μm/h\r\n"+ "Uniformity (Coefficient of variation):" + NN_Variable_Coefficient.ToString("F2") + "%\r\n";
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
        public class DataPoint
        {
            public double[] Features
            {
                get;set;
            }
        }
        public static DataTable ConvertCsvToDataTable(string filePath)
        {
            DataTable dataTable = new DataTable();
            try
            {
                using (TextFieldParser csvReader = new TextFieldParser(filePath))
                {
                    csvReader.SetDelimiters(new string[] { "," });
                    csvReader.HasFieldsEnclosedInQuotes = true;
                    // 读取并解析标题行
                    string[] colFields = csvReader.ReadFields();
                    foreach (string columnTitle in colFields)
                    {
                        DataColumn column = new DataColumn(columnTitle);
                        dataTable.Columns.Add(column);
                    }
                    // 读取并解析每一行
                    while (!csvReader.EndOfData)
                    {
                        string[] fieldData = csvReader.ReadFields();
                        // 假定每行数据的列数相同
                        DataRow row = dataTable.NewRow();
                        for (int i = 0; i < fieldData.Length; i++)
                        {
                            row[i] = fieldData[i];
                        }
                        dataTable.Rows.Add(row);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            return dataTable;
        }

        private void MainSystemForm_Load(object sender, EventArgs e)
        {
            asc.controllInitializeSize(this);
            List<double> datalist = Visualization.inputdatas;
            if (datalist.Count > 0 && datalist != null)
            {
                // 获取第一个元素
                double firstElement = datalist[0];

                MFC04textEdit.Text = datalist[0].ToString();
                MFC05textEdit.Text = datalist[1].ToString();
                MFC06textEdit.Text = datalist[2].ToString();
                MFC07textEdit.Text = datalist[3].ToString();
                MFC08textEdit.Text = datalist[4].ToString();
                MFC09textEdit.Text = datalist[5].ToString();
                MFC03textEdit.Text = datalist[6].ToString();
                MFC12textEdit.Text = datalist[7].ToString();
                MFC13textEdit.Text = datalist[8].ToString();
                MFC14textEdit.Text = datalist[9].ToString();
                MFC15textEdit.Text = datalist[10].ToString();
                MFC16textEdit.Text = datalist[11].ToString();
                MOtextEdit.Text = datalist[12].ToString();
                H2OtextEdit.Text = datalist[13].ToString();
                PressuretextEdit.Text = datalist[14].ToString();
                TemperaturetextEdit.Text = datalist[15].ToString();
                RotationaltextEdit.Text = datalist[16].ToString();
            }
            float MFC04text = float.Parse(MFC04textEdit.Text.ToString());
            float MFC05text = float.Parse(MFC05textEdit.Text.ToString());
            float MFC06text = float.Parse(MFC06textEdit.Text.ToString());
            float MFC07text = float.Parse(MFC07textEdit.Text.ToString());
            float MFC08text = float.Parse(MFC08textEdit.Text.ToString());
            float MFC09text = float.Parse(MFC09textEdit.Text.ToString());
            float MFC03text = float.Parse(MFC03textEdit.Text.ToString());
            float MFC12text = float.Parse(MFC12textEdit.Text.ToString());
            float MFC13text = float.Parse(MFC13textEdit.Text.ToString());
            float MFC14text = float.Parse(MFC14textEdit.Text.ToString());
            float MFC15text = float.Parse(MFC15textEdit.Text.ToString());
            float MFC16text = float.Parse(MFC16textEdit.Text.ToString());
            float MOtext = float.Parse(MOtextEdit.Text.ToString());
            float H2Otext = float.Parse(H2OtextEdit.Text.ToString());
            float Pressuretext = float.Parse(PressuretextEdit.Text.ToString());
            float Temperaturetext = float.Parse(TemperaturetextEdit.Text.ToString());
            float Rotationaltext = float.Parse(RotationaltextEdit.Text.ToString());

            float N2 = (MFC04text + MFC05text + MFC06text + MFC07text + MFC08text + MFC09text - H2Otext) / (MFC04text + MFC05text + MFC06text + MFC07text + MFC08text + MFC09text);
            float O2 = (H2Otext) / (MFC04text + MFC05text + MFC06text + MFC07text + MFC08text + MFC09text);
            float VH2O = (float)((1.39 * (MFC03text + (MFC04text + MFC05text + MFC06text + MFC07text + MFC08text + MFC09text) * N2) + (MFC04text + MFC05text + MFC06text + MFC07text + MFC08text + MFC09text) * O2) * 0.000001 / 0.1988 / 60 * 760 * 300 / 273 / Pressuretext);
            float VMFC12 = (float)(MFC12text * 1.39 * 0.000001 / 0.001195 / 60 * 760 * 300 / 273 / Pressuretext);
            float VMFC13 = (float)(MFC13text * 1.39 * 0.000001 / 0.001195 / 60 * 760 * 300 / 273 / Pressuretext);
            float VMFC14 = (float)(MFC14text * 1.39 * 0.000001 / 0.001195 / 60 * 760 * 300 / 273 / Pressuretext);
            float VMFC15 = (float)(MFC15text * 1.39 * 0.000001 / 0.001195 / 60 * 760 * 300 / 273 / Pressuretext);
            float VMFC16 = (float)(MFC16text * 1.39 * 0.000001 / 0.001195 / 60 * 760 * 300 / 273 / Pressuretext);
            float MASS_MO = (float)(MOtext * 15.9645 / (600 - 15.9645) / (MFC12text + MFC13text + MFC14text + MFC15text + MFC16text));
            float MASS_H2O = (float)(H2Otext * 23.76 / (600 - 23.76) / (MFC04text + MFC05text + MFC06text + MFC07text + MFC08text + MFC09text));
            float KTemperature = (float)(Temperaturetext + 273.15);

            float[] y = { VH2O, MASS_H2O, VMFC12, MASS_MO, VMFC13, VMFC14, VMFC15, VMFC16, Pressuretext, KTemperature, Rotationaltext };

            List<float> Predictdatalist = new List<float>
                    {
                         VH2O,MASS_H2O, VMFC12,  MASS_MO, VMFC13, VMFC14, VMFC15, VMFC16,Pressuretext, KTemperature, Rotationaltext
                    };
            Visualization.pridectinputdatas = y;
        }

        private void MainSystemForm_SizeChanged(object sender, EventArgs e)
        {
            asc.controlAutoSize(this);
        }
        private void ViewBtn_Click(object sender, EventArgs e)
        {
            // 根据按钮点击切换图
            currentImageIndex = (currentImageIndex + 1) % 3;
            FlowpictureBox.Image = images[currentImageIndex];
        }

        private void ViewdownBtn_Click(object sender, EventArgs e)
        {
            int imageCount = images1.Length;
            if (images1.Length > 0 && images1[images1.Length - 1] == null)
            {
                // 根据按钮点击切换图片
                currentImageIndex1 = (currentImageIndex1 + 1) % 2;
                CrowthpictureBox.BackgroundImage = images1[currentImageIndex1];
            }
            else
            {
                // 根据按钮点击切换图片
                currentImageIndex1 = (currentImageIndex1 + 1) % 3;
                CrowthpictureBox.BackgroundImage = images1[currentImageIndex1];
            }
        }

        private void ViewBtn_MouseEnter(object sender, EventArgs e)
        {
            ViewBtn.BackColor = Color.SteelBlue;
            ViewBtn.ForeColor = Color.White;
        }

        private void ViewBtn_MouseLeave(object sender, EventArgs e)
        {
            ViewBtn.BackColor = Color.Transparent;
            ViewBtn.ForeColor = Color.White;
        }

        private void ViewdownBtn_MouseEnter(object sender, EventArgs e)
        {
            ViewdownBtn.BackColor = Color.SteelBlue;
            ViewdownBtn.ForeColor = Color.White;
        }

        private void ViewdownBtn_MouseLeave(object sender, EventArgs e)
        {
            ViewdownBtn.BackColor = Color.Transparent;
            ViewdownBtn.ForeColor = Color.White;
        }

        private void PredictBtn_MouseEnter(object sender, EventArgs e)
        {
            PredictBtn.BackColor = Color.SteelBlue;
            PredictBtn.ForeColor = Color.White;
        }

        private void PredictBtn_MouseLeave(object sender, EventArgs e)
        {
            PredictBtn.BackColor = Color.Transparent;
            PredictBtn.ForeColor = Color.White;
        }

        private void button1_MouseEnter(object sender, EventArgs e)
        {
            button1.BackColor = Color.SteelBlue;
            ViewBtn.ForeColor = Color.White;
        }

        private void button1_MouseLeave(object sender, EventArgs e)
        {
            button1.BackColor = Color.Transparent;
            button1.ForeColor = Color.White;
        }

        private void MainSystemForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (CrowthpictureBox.BackgroundImage != null)
            {
                CrowthpictureBox.BackgroundImage.Dispose();
                CrowthpictureBox.BackgroundImage = null;
            }
        }
    }
}
