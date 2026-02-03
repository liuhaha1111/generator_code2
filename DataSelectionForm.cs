using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using WisdomGrowth.DataBase;
using Python.Runtime;
using Razorvine.Pickle;
using System.Windows.Forms.DataVisualization.Charting;

namespace WisdomGrowth
{
    public partial class DataSelectionForm : Form
    {
        public DataSelectionForm()
        {
            InitializeComponent();



        }
        AutoResizeForm asc = new AutoResizeForm();
        /// <summary>s
        /// 点击选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        List<string> selectedFilePaths = new List<string>();
        //输入
        List<DataTable> inputFilePathData = new List<DataTable>();
        //输出
        List<DataTable> outputFilePathData = new List<DataTable>();
        List<DataTable> selectedFilePathData = new List<DataTable>();
        string selectedFolderPath;
        
        /// <summary>
        /// 调用Python内的pandas.read_pickle()方法打开Dataframe数据类型的文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private DataTable readpickle(string projectFolderPath)
        {
            Runtime.PythonDLL = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), @"C:\Program Files\Python38\python38.dll");
            // 初始化Python引擎
            PythonEngine.Initialize();
            DataTable dataTable = new DataTable();
            using (Py.GIL()) // 初始化Python引擎并获取全局锁
            {
                dynamic pandas = Py.Import("pandas");
                dynamic df = pandas.read_pickle(projectFolderPath);
                string dataAsString = df.ToString();
                string[] stringArray = dataAsString.Split('\n');
                string[] dataChartstexts = stringArray[0].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                // 假设第一行包含列标题
                string[] headers = stringArray[1].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                dataTable.Columns.Add(headers[0]);
                foreach (string header in dataChartstexts)
                {
                    dataTable.Columns.Add(header);
                }
                // 去掉标题行
                for (int i = 2; i < stringArray.Length - 2; i++)
                {
                    string[] items = stringArray[i].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    DataRow row = dataTable.NewRow();
                    for (int j = 0; j < items.Length - 2; j++)
                    {
                        row[j] = items[j];
                    }
                    dataTable.Rows.Add(row);
                }
            }
            // 关闭Python引擎
            PythonEngine.Shutdown();
            return dataTable;
        }
        /// <summary>
        /// 温度
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        int temperaturecount = 0;
       
        /// <summary>
        /// 速率
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        int speedcount = 0; 
        
        /// <summary>
        /// MO流量
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        int mocount = 0;
        
        /// <summary>
        /// 掺杂
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        int dopingcount = 0;
  
        /// <summary>
        /// 输出速率
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
       

        private void DataSelectionForm_Load(object sender, EventArgs e)
        {
            asc.controllInitializeSize(this);
           
        }

        private void DataSelectionForm_SizeChanged(object sender, EventArgs e)
        {
            asc.controlAutoSize(this);
        }

        
        
    }
}