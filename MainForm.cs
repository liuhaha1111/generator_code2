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
    public partial class MainForm : Form
    {
        WindowZoom windowZoom = new WindowZoom(); 
        public MainForm()
        {
            InitializeComponent();
            //instance = this;
            // 设置窗体属性
            this.AutoScaleMode = AutoScaleMode.Dpi;
            this.AutoScroll = true;
        }

        
    }
}
