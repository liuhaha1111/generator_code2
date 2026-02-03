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
    public partial class WisdomMenuForm : Form
    {
        WindowZoom windowZoom = new WindowZoom();
        public WisdomMenuForm()
        {
            InitializeComponent();
            windowZoom.SetForm(this);
        }

        private void DataSelectionBtn_Click(object sender, EventArgs e)
        {
            DataSelectionForm ds = new DataSelectionForm();
            // 根据需要设置子窗体在Panel中的位置
            ds.TopLevel = false;
            windowZoom.SetForm(ds);
            ds.Dock = DockStyle.Fill;
            // 将子窗体添加到Panel中
            panel2.Controls.Clear();
            panel2.Controls.Add(ds);
            // 显示子窗体
            ds.Show();
        }

        private void ModelTrainingBtn_Click(object sender, EventArgs e)
        {
            ModelTrainingForm ds = new ModelTrainingForm();
            // 根据需要设置子窗体在Panel中的位置
            ds.TopLevel = false;
            ds.Dock = DockStyle.Fill;
            // 将子窗体添加到Panel中
            panel2.Controls.Clear();
            panel2.Controls.Add(ds);
            // 显示子窗体
            ds.Show();
        }

        private void OutlierRemovalBtn_Click(object sender, EventArgs e)
        {
            OutlierRemovalForm ds = new OutlierRemovalForm();
            // 根据需要设置子窗体在Panel中的位置
            ds.TopLevel = false;
            ds.Dock = DockStyle.Fill;
            // 将子窗体添加到Panel中
            panel2.Controls.Clear();
            panel2.Controls.Add(ds);
            // 显示子窗体
            ds.Show();
        }

        private void NumericPredictionBtn_Click(object sender, EventArgs e)
        {
            NumericPredictionForm ds = new NumericPredictionForm();
            // 根据需要设置子窗体在Panel中的位置
            ds.TopLevel = false;
            ds.Dock = DockStyle.Fill;
            // 将子窗体添加到Panel中
            panel2.Controls.Clear();
            panel2.Controls.Add(ds);
            // 显示子窗体
            ds.Show();
        }
    }
}
