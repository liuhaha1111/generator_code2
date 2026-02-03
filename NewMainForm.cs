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
    public partial class NewMainForm : Form
    {
        WindowZoom windowZoom = new WindowZoom();
        public NewMainForm()
        {
            InitializeComponent();
            windowZoom.SetForm(this);
            panel3.Dock = DockStyle.Fill;
        }
        public void LoginBtn_Click(object sender, EventArgs e)
        {

            EnergyManagementForm ds = new EnergyManagementForm();
            // 根据需要设置子窗体在Panel中的位置
            ds.TopLevel = false;
            windowZoom.SetForm(ds);
            ds.Dock = DockStyle.Fill;
            // 将子窗体添加到Panel中
            foreach (Control control in panel3.Controls)
            {
                if (control is Form childForm)
                {
                    // 关闭子窗体
                    childForm.Close();
                }
            }
            panel3.Controls.Clear();
            panel3.Controls.Add(ds);
            // 显示子窗体
            ds.Show();
            LoginBtn.BackgroundImage = global::WisdomGrowth.Properties.Resources.title_item_border_click;
            pictureBox1.Visible = true;
            WisdomMenuBtn.BackgroundImage = global::WisdomGrowth.Properties.Resources.title_item_border;
            MaterialMenuBtn.BackgroundImage = global::WisdomGrowth.Properties.Resources.title_item_border;
        }

        public void WisdomMenuBtn_Click(object sender, EventArgs e)
        {
            pictureBox1.Visible = false;
            MaterialMenuForm ds = new MaterialMenuForm();
            // 根据需要设置子窗体在Panel中的位置
            ds.TopLevel = false;
            windowZoom.SetForm(ds);
            ds.Dock = DockStyle.Fill;
            // 将子窗体添加到Panel中
            foreach (Control control in panel3.Controls)
            {
                if (control is Form childForm)
                {
                    // 关闭子窗体
                    childForm.Close();
                }
            }
            panel3.Controls.Clear();
            panel3.Controls.Add(ds);
            // 显示子窗体
            ds.Show();
            WisdomMenuBtn.BackgroundImage = global::WisdomGrowth.Properties.Resources.title_item_border_click;
            LoginBtn.BackgroundImage = global::WisdomGrowth.Properties.Resources.title_item_border;
            MaterialMenuBtn.BackgroundImage = global::WisdomGrowth.Properties.Resources.title_item_border;
        }
        
        private void MaterialMenuBtn_Click(object sender, EventArgs e)
        {
            pictureBox1.Visible = false;
            MaterialForm ds = new MaterialForm();
            // 根据需要设置子窗体在Panel中的位置
            ds.TopLevel = false;
            windowZoom.SetForm(ds);
            ds.Dock = DockStyle.Fill;
            // 将子窗体添加到Panel中
            foreach (Control control in panel3.Controls)
            {
                if (control is Form childForm)
                {
                    // 关闭子窗体
                    childForm.Close();
                }
            }
            panel3.Controls.Clear();
            panel3.Controls.Add(ds);
            // 显示子窗体
            ds.Show();
            MaterialMenuBtn.BackgroundImage = global::WisdomGrowth.Properties.Resources.title_item_border_click;
            LoginBtn.BackgroundImage = global::WisdomGrowth.Properties.Resources.title_item_border;
            WisdomMenuBtn.BackgroundImage = global::WisdomGrowth.Properties.Resources.title_item_border;
        }

        private void NewMainForm_Resize(object sender, EventArgs e)
        {
            windowZoom.SetReSize(this);
        }
        private int count = 0;
        private void amplifybtn_Click(object sender, EventArgs e)
        {
            count++;
            if (count % 2 == 1)
            {
                
                WindowState = FormWindowState.Maximized;//最大化
                //windowZoom.SetForm(this);
            }
            if (count % 2 == 0)
            {
                WindowState = FormWindowState.Normal;
            }
        }

        private void narrowBtn_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;//最小化
        }

        private void signoutBtn_Click(object sender, EventArgs e)
        {
            this.Close();
            Application.Exit();
        }
        //定义一个布尔变量，作为事件的开关。
        bool b = false;
        //定义一个‘点’的变量，接收鼠标的点位置。
        Point mousePonit;

        private void panel2_MouseDown(object sender, MouseEventArgs e)
        {
            //考虑是否鼠标左键按下，如果按下则开始做以下的事情。
            if (e.Button == MouseButtons.Left)
            {
                //给mousePonit定义为当前的鼠标位置坐标。
                mousePonit = new Point(-e.X, -e.Y);
                //设置变量b为布尔真值。
                b = true;
            }
        }

        private void panel2_MouseMove(object sender, MouseEventArgs e)
        {
            //如果获取b为真的时候，开始执行下面的语句。
            if (b)
            {
                //定义一个‘点’变量，为组件的鼠标光标位置
                Point p = Control.MousePosition;
                //平移mousePonit为p变量。
                p.Offset(mousePonit);
                //控件的位置，为p位置。
                this.Location = p;
            }
        }

        private void panel2_MouseUp(object sender, MouseEventArgs e)
        {
            b = false;
        }

        private void panel3_MouseDown(object sender, MouseEventArgs e)
        {
            //考虑是否鼠标左键按下，如果按下则开始做以下的事情。
            if (e.Button == MouseButtons.Left)
            {
                //给mousePonit定义为当前的鼠标位置坐标。
                mousePonit = new Point(-e.X, -e.Y);
                //设置变量b为布尔真值。
                b = true;
            }
        }

        private void panel3_MouseMove(object sender, MouseEventArgs e)
        {
            //如果获取b为真的时候，开始执行下面的语句。
            if (b)
            {
                //定义一个‘点’变量，为组件的鼠标光标位置
                Point p = Control.MousePosition;
                //平移mousePonit为p变量。
                p.Offset(mousePonit);
                //控件的位置，为p位置。
                this.Location = p;
            }
        }

        private void panel3_MouseUp(object sender, MouseEventArgs e)
        {
            b = false;
        }


        private void NewMainForm_Load(object sender, EventArgs e)
        {
            this.SuspendLayout();
            pictureBox1.Visible = true;
            EnergyManagementForm ds = new EnergyManagementForm();
            // 根据需要设置子窗体在Panel中的位置
            ds.TopLevel = false;
            windowZoom.SetForm(ds);
            ds.Dock = DockStyle.Fill;
            // 将子窗体添加到Panel中
            panel3.Controls.Clear();
            panel3.Controls.Add(ds);
            // 显示子窗体
            ds.Show();
            this.ResumeLayout();
            LoginBtn.BackgroundImage = global::WisdomGrowth.Properties.Resources.title_item_border_click;
            WisdomMenuBtn.BackgroundImage = global::WisdomGrowth.Properties.Resources.title_item_border;
            MaterialMenuBtn.BackgroundImage = global::WisdomGrowth.Properties.Resources.title_item_border;
        }

        public void LoginBtn_MouseEnter(object sender, EventArgs e)
        {
            LoginBtn.BackColor = Color.FromArgb(11, 40, 70); // 设置你想要的背景色;
        }
        public void WisdomMenuBtn_MouseEnter(object sender, EventArgs e)
        {
            WisdomMenuBtn.BackColor = Color.FromArgb(11, 40, 70); // 设置你想要的背景色;

        }
        public void MaterialMenuBtn_MouseEnter(object sender, EventArgs e)
        {
            MaterialMenuBtn.BackColor = Color.FromArgb(11, 40, 70); // 设置你想要的背景色;

        }

        private void LoginBtn_MouseLeave(object sender, EventArgs e)
        {
            LoginBtn.BackColor = Color.Transparent;
        }

        private void WisdomMenuBtn_MouseLeave(object sender, EventArgs e)
        {
            WisdomMenuBtn.BackColor = Color.Transparent;
        }

        private void MaterialMenuBtn_MouseLeave(object sender, EventArgs e)
        {
            MaterialMenuBtn.BackColor = Color.Transparent;
        }
    }
}