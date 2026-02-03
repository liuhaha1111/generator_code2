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
    public partial class WorkbenchesForm : Form
    {
        AutoResizeForm asc = new AutoResizeForm();
        WindowZoom windowZoom = new WindowZoom();
        public WorkbenchesForm()
        {
            InitializeComponent();
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            Timetxt.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Maximized;//最大化

            //windowZoom.SetForm(this);
            asc.controllInitializeSize(this);
            Timetxt.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }
        private void LoginForm_Resize(object sender, EventArgs e)
        {
           // windowZoom.SetReSize(this);
        }
        private void LoginForm_SizeChanged(object sender, EventArgs e)
        {
            asc.controlAutoSize(this);
        }
    }
}
