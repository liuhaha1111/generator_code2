using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WisdomGrowth.ModelTraining;

namespace WisdomGrowth
{
    public partial class ModelTrainingForm : Form
    {
        public ModelTrainingForm()
        {
            InitializeComponent();
            Stopbtn();
            textEdit3.Visible = false;
        }
        AutoResizeForm asc = new AutoResizeForm();
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
            if (Stackingcount % 2 == 0)
            {
                //Stopbtn();
                //BPNNModel0Box.Checked = false;
                //SVRModel0Box.Checked = false;
                //GBRModel0Box.Checked = false;
                //RFRFModel0Box.Checked = false;
                //BPNNModel1Btn.Checked = false;
                //SVRModel1Btn.Checked = false;
                //GBRModel1Btn.Checked = false;
                //RFRFModel1Btn.Checked = false;
                selectedStackingModel0 = new List<string>();
            }

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
            }
        }
        private void PerformSomeOperation()
        {
            // 这里是你想要测量时间的操作代码
            // 例如，一个耗时的计算或数据处理任务
            if (ModelTrainingInfo.selectmodel == "BPNN")
            {
                int inputLayerSize = 2; // Example: 2 input neurons
                int hiddenLayerSize = 3; // Example: 3 hidden neurons
                int outputLayerSize = 1; // Example: 1 output neuron
                double learningRate = 0.5;

                BPNeuralNetwork nn = new BPNeuralNetwork(inputLayerSize, hiddenLayerSize, outputLayerSize, learningRate);

                // Define training data (inputs and targets)
                double[][] inputs = new double[][]
                {
        new double[] { 0, 0 },
        new double[] { 0, 1 },
        new double[] { 1, 0 },
        new double[] { 1, 1 }
                };

                double[][] targets = new double[][]
                {
        new double[] { 0 },
        new double[] { 1 },
        new double[] { 1 },
        new double[] { 0 }
                };

                // Train the neural network
                nn.Train(inputs, targets, 10000);

                // Test the neural network
                double[] testInput = new double[] { 0, 0 };
                double[] output = nn.FeedForward(testInput);
                textEdit2.Text = Math.Round(double.Parse(output[0].ToString()), 4).ToString() ;
            }
            if (ModelTrainingInfo.selectmodel == "SVR")
            {

            }
            if (ModelTrainingInfo.selectmodel == "GBR")
            {

            }
            if (ModelTrainingInfo.selectmodel == "RFR")
            {

            }
            if (ModelTrainingInfo.selectmodel == "Stacking")
            {
                foreach (string selectedstackingmodel in selectedStackingModel0)
                {

                }
                if (ModelTrainingInfo.selectstackingmodel1 == "BPNN")
                {

                }
                if (ModelTrainingInfo.selectstackingmodel1 == "SVR")
                {

                }
                if (ModelTrainingInfo.selectstackingmodel1 == "GBR")
                {

                }
                if (ModelTrainingInfo.selectstackingmodel1 == "RFR")
                {

                }
            }
            System.Threading.Thread.Sleep(500); // 模拟一个耗时操作
        }
        /// <summary>
        /// 开始训练
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TrainingBtn_Click(object sender, EventArgs e)
        {

            // 创建一个新的Stopwatch实例
            Stopwatch stopwatch = new Stopwatch();
            // 开始计时
            stopwatch.Start();

            // 执行你想要测量时间的操作
            PerformSomeOperation();

            // 停止计时
            stopwatch.Stop();

            // 获取操作花费的时间（以秒为单位）
            double elapsedSeconds = stopwatch.Elapsed.TotalSeconds;
            elapsedSeconds = Math.Round(elapsedSeconds, 3);
            // 显示一个简单的消息框
            textEdit3.Visible = true;
            textEdit3.Text = "训练完成！";
            // 在标签上显示结果
            textEdit1.Text = string.Format("{0}", elapsedSeconds);
        }

        private void ModelTrainingForm_Load(object sender, EventArgs e)
        {
            asc.controllInitializeSize(this);
        }

        private void ModelTrainingForm_SizeChanged(object sender, EventArgs e)
        {
            asc.controlAutoSize(this);
        }

        private void DrawBtn_Click(object sender, EventArgs e)
        {

        }
    }
}
