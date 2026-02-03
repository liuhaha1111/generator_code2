using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WisdomGrowth
{
    public partial class MaterialMenuForm : Form
    {
        WindowZoom windowZoom = new WindowZoom();
        public MaterialMenuForm()
        {
            InitializeComponent();
            windowZoom.SetForm(this);
        }

        private void MainSystemBtn_Click(object sender, EventArgs e)
        {
            MainSystemForm ds = new MainSystemForm();
            // 根据需要设置子窗体在Panel中的位置
            ds.TopLevel = false;
            ds.Dock = DockStyle.Fill;
            // 将子窗体添加到Panel中
            // 遍历 Panel2 中的所有控件
            foreach (Control control in panel2.Controls)
            {
                if (control is Form childForm)
                {
                    //// 调用子窗体的清空数据方法
                    //if (childForm is MainSystemForm mainSystemForm)
                    //{
                    //    mainSystemForm.ClearData();
                    //}
                    // 关闭子窗体
                    childForm.Close();
                }
            }
            panel2.Controls.Clear();
            panel2.Controls.Add(ds);
            // 显示子窗体
            ds.Show();
            MainSystemBtn.BackgroundImage = global::WisdomGrowth.Properties.Resources.渐变背景框_01_8_;
            OptimizationBtn.BackgroundImage = global::WisdomGrowth.Properties.Resources.渐变背景框_01_1_;
            TroubleShootingBtn.BackgroundImage = global::WisdomGrowth.Properties.Resources.渐变背景框_01_1_;
            Timetxt.BackgroundImage = global::WisdomGrowth.Properties.Resources.渐变背景框_01_1_;
            NeuralNetworkBtn.BackgroundImage = global::WisdomGrowth.Properties.Resources.渐变背景框_01_1_;
        }

        private void OptimizationBtn_Click(object sender, EventArgs e)
        {
            OptimizationForm ds = new OptimizationForm();
            // 根据需要设置子窗体在Panel中的位置
            ds.TopLevel = false;
            ds.Dock = DockStyle.Fill;
            // 将子窗体添加到Panel中
            foreach (Control control in panel2.Controls)
            {
                if (control is Form childForm)
                {
                    //// 调用子窗体的清空数据方法
                    //if (childForm is MainSystemForm mainSystemForm)
                    //{
                    //    mainSystemForm.ClearData();
                    //}
                    // 关闭子窗体
                    childForm.Close();
                }
            }
            panel2.Controls.Clear();
            panel2.Controls.Add(ds);
            // 显示子窗体
            ds.Show();
            OptimizationBtn.BackgroundImage = global::WisdomGrowth.Properties.Resources.渐变背景框_01_8_;
            MainSystemBtn.BackgroundImage = global::WisdomGrowth.Properties.Resources.渐变背景框_01_1_;
            TroubleShootingBtn.BackgroundImage = global::WisdomGrowth.Properties.Resources.渐变背景框_01_1_;
            Timetxt.BackgroundImage = global::WisdomGrowth.Properties.Resources.渐变背景框_01_1_;
            NeuralNetworkBtn.BackgroundImage = global::WisdomGrowth.Properties.Resources.渐变背景框_01_1_;
        }

        private void TroubleShootingBtn_Click(object sender, EventArgs e)
        {
            TroubleShootingForm ds = new TroubleShootingForm();
            // 根据需要设置子窗体在Panel中的位置
            ds.TopLevel = false;
            ds.Dock = DockStyle.Fill;
            // 将子窗体添加到Panel中
            foreach (Control control in panel2.Controls)
            {
                if (control is Form childForm)
                {
                    //// 调用子窗体的清空数据方法
                    //if (childForm is MainSystemForm mainSystemForm)
                    //{
                    //    mainSystemForm.ClearData();
                    //}
                    // 关闭子窗体
                    childForm.Close();
                }
            }
            panel2.Controls.Clear();
            panel2.Controls.Add(ds);
            // 显示子窗体
            ds.Show();
            TroubleShootingBtn.BackgroundImage = global::WisdomGrowth.Properties.Resources.渐变背景框_01_8_;
            OptimizationBtn.BackgroundImage = global::WisdomGrowth.Properties.Resources.渐变背景框_01_1_;
            MainSystemBtn.BackgroundImage = global::WisdomGrowth.Properties.Resources.渐变背景框_01_1_;
            Timetxt.BackgroundImage = global::WisdomGrowth.Properties.Resources.渐变背景框_01_1_;
            NeuralNetworkBtn.BackgroundImage = global::WisdomGrowth.Properties.Resources.渐变背景框_01_1_;
        }

        private void NeuralNetworkBtn_Click(object sender, EventArgs e)
        {
            NeuralNetworkForm ds = new NeuralNetworkForm();
            // 根据需要设置子窗体在Panel中的位置
            ds.TopLevel = false;
            ds.Dock = DockStyle.Fill;
            // 将子窗体添加到Panel中
            foreach (Control control in panel2.Controls)
            {
                if (control is Form childForm)
                {
                    //// 调用子窗体的清空数据方法
                    //if (childForm is MainSystemForm mainSystemForm)
                    //{
                    //    mainSystemForm.ClearData();
                    //}
                    // 关闭子窗体
                    childForm.Close();
                }
            }
            panel2.Controls.Clear();
            panel2.Controls.Add(ds);
            // 显示子窗体
            ds.Show();
            NeuralNetworkBtn.BackgroundImage = global::WisdomGrowth.Properties.Resources.渐变背景框_01_8_;
            OptimizationBtn.BackgroundImage = global::WisdomGrowth.Properties.Resources.渐变背景框_01_1_;
            TroubleShootingBtn.BackgroundImage = global::WisdomGrowth.Properties.Resources.渐变背景框_01_1_;
            MainSystemBtn.BackgroundImage = global::WisdomGrowth.Properties.Resources.渐变背景框_01_1_;
            Timetxt.BackgroundImage = global::WisdomGrowth.Properties.Resources.渐变背景框_01_1_;
        }

        private void MaterialMenuForm_Load(object sender, EventArgs e)
        {
            Timetxt.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            MainSystemForm ds = new MainSystemForm();
            // 根据需要设置子窗体在Panel中的位置
            ds.TopLevel = false;
            ds.Dock = DockStyle.Fill;
            // 将子窗体添加到Panel中
            panel2.Controls.Clear();
            panel2.Controls.Add(ds);
            // 显示子窗体
            ds.Show();
            MainSystemBtn.BackColor = Color.FromArgb(13, 65, 113); // 设置你想要的背景色;
            MainSystemBtn.BackgroundImage = global::WisdomGrowth.Properties.Resources.渐变背景框_01_8_;
            NeuralNetworkBtn.BackgroundImage = global::WisdomGrowth.Properties.Resources.渐变背景框_01_1_;
            OptimizationBtn.BackgroundImage = global::WisdomGrowth.Properties.Resources.渐变背景框_01_1_;
            TroubleShootingBtn.BackgroundImage = global::WisdomGrowth.Properties.Resources.渐变背景框_01_1_;
            Timetxt.BackgroundImage = global::WisdomGrowth.Properties.Resources.渐变背景框_01_1_;
        }
        private void MainSystemBtn_MouseEnter(object sender, EventArgs e)
        {
            MainSystemBtn.BackColor = Color.FromArgb(13, 65, 113); // 设置你想要的背景色;
        }
        private void OptimizationBtn_MouseEnter(object sender, EventArgs e)
        {
            OptimizationBtn.BackColor = Color.FromArgb(13, 65, 113); // 设置你想要的背景色;
        }

        private void TroubleShootingBtn_MouseEnter(object sender, EventArgs e)
        {
            TroubleShootingBtn.BackColor = Color.FromArgb(13, 65, 113); // 设置你想要的背景色;
        }

        private void NeuralNetworkBtn_MouseEnter(object sender, EventArgs e)
        {
            NeuralNetworkBtn.BackColor = Color.FromArgb(13, 65, 113); // 设置你想要的背景色;
        }

        private void MainSystemBtn_MouseLeave(object sender, EventArgs e)
        {
            MainSystemBtn.BackColor = Color.Transparent;
        }

        private void OptimizationBtn_MouseLeave(object sender, EventArgs e)
        {
            OptimizationBtn.BackColor = Color.Transparent;
        }

        private void TroubleShootingBtn_MouseLeave(object sender, EventArgs e)
        {
            TroubleShootingBtn.BackColor = Color.Transparent;
        }

        private void NeuralNetworkBtn_MouseLeave(object sender, EventArgs e)
        {
            NeuralNetworkBtn.BackColor = Color.Transparent;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Timetxt.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }

        private void Timetxt_MouseEnter(object sender, EventArgs e)
        {
            Timetxt.BackColor = Color.FromArgb(11, 40, 74); // 设置你想要的背景色;
        }

        private void Timetxt_MouseLeave(object sender, EventArgs e)
        {
            Timetxt.BackColor = Color.Transparent;
        }
    }
}
