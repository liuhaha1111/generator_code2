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

namespace WisdomGrowth
{
    public partial class Form1 : Form
    {
        private string filePath = Application.StartupPath + "\\File\\password.txt";
        WindowZoom windowZoom = new WindowZoom();
        public Form1()
        {
            InitializeComponent();
            //windowZoom.SetForm(this);
            //WindowState = FormWindowState.Maximized;//最大化
        }

        private void label3_Click(object sender, EventArgs e)
        {
            //账号
            string userid = useridTextEdit.Text.ToString();
            DataTable table = userhelper.QueryUserByIdToDb(userid);
            if (table.Rows.Count > 0)
            {
                //密码
                string password = table.Rows[0]["Password"].ToString(); 
                MessageBox.Show(string.Format("您的账号密码为：{0}", password));
                passwordTextEdit.Text = password;
            }
        }
        private void LoginBtn_Click(object sender, EventArgs e)
        {
            //账号
            string userid = useridTextEdit.Text.ToString();
            //密码
            string password = passwordTextEdit.Text.ToString();
            DataTable table = userhelper.QueryUserByIdToDb(userid, password);
            if (table.Rows.Count > 0)
            {
                if (chkRememberPassword.Checked)
                {
                    string[] lines = { userid, password };
                    File.WriteAllLines(filePath, lines);
                }
                else
                {
                    if (File.Exists(filePath))
                    {
                        File.Delete(filePath);
                    }
                }
                this.Hide();
                NewMainForm ds = new NewMainForm();
                ds.Show();

            }
            else
            {
                MessageBox.Show(string.Format("{0}用户不存在！！！", userid));
            }
        }

        private void RegisterBtn_Click(object sender, EventArgs e)
        {
            RegisterForm registerform = new RegisterForm();
            registerform.Show();
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

        private void narrowBtn_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;//最小化
        }
        private int count = 0;
        private void amplifybtn_Click(object sender, EventArgs e)
        {
            count++;
            if (count % 2 == 1)
            {
                windowZoom.SetForm(this);
                WindowState = FormWindowState.Maximized;//最大化
            }
            if (count % 2 == 0)
            {
                WindowState = FormWindowState.Normal;
            }
        }

        private void signoutBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void LoginBtn_MouseEnter(object sender, EventArgs e)
        {
            LoginBtn.BackColor = Color.SteelBlue;
            LoginBtn.ForeColor = Color.White;
        }

        private void LoginBtn_MouseLeave(object sender, EventArgs e)
        {
            LoginBtn.BackColor = Color.Transparent;
            LoginBtn.ForeColor = Color.White;
        }
      
        private void Form1_Load(object sender, EventArgs e)
        {
            if (File.Exists(filePath))
            {
                string[] lines = File.ReadAllLines(filePath);
                if (lines.Length == 2)
                {
                    useridTextEdit.Text = lines[0];
                    passwordTextEdit.Text = lines[1];
                    chkRememberPassword.Checked = true;
                }
            }
        }

        private void Form1_SizeChanged(object sender, EventArgs e)
        {

        }

        private void chkRememberPassword_CheckedChanged(object sender, EventArgs e)
        {
           
        }
    }
}
