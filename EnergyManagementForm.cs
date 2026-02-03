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
    public partial class EnergyManagementForm : Form
    {
        AutoResizeForm asc = new AutoResizeForm();
        public EnergyManagementForm()
        {
            InitializeComponent();
        }

        private void EnergyManagementForm_Load(object sender, EventArgs e)
        {
           
            asc.controllInitializeSize(this);
            Timetxt.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }

        private void EnergyManagementForm_SizeChanged(object sender, EventArgs e)
        {
            asc.controlAutoSize(this);
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            Timetxt.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }
    }
}
