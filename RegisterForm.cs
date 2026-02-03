using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WisdomGrowth.DataBase;

namespace WisdomGrowth
{
    public partial class RegisterForm : Form
    {
        public RegisterForm()
        {
            InitializeComponent();
        }

        private void LoginBtn_Click(object sender, EventArgs e)
        {
            //用户名
            string username = usernameTextEdit.Text.ToString();
            //账号
            string userid = useridTextEdit.Text.ToString();
            //密码
            string password = passwordTextEdit.Text.ToString();
            DataTable table = userhelper.QueryUserToDb(userid, username);
            if (table.Rows.Count > 0)
            {
                MessageBox.Show(string.Format("{0}用户已存在！！！", username));
            }
            else {
                userhelper.ImportUserToDb(userid,username, password);
                MessageBox.Show(string.Format("注册成功！！！"));
                this.Hide(); 
            }
        }
    }
}
