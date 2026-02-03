using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WisdomGrowth.DataBase
{
    public class Visualization
    {
        //输入
        public static List<double> inputs = new List<double>(); //输入
        //选择模型路径
        public static string selectedmodelPath { get; set; }
        //输入数据
        public static List<double> inputdatas = new List<double>(); //输入
        //预测数据
        //public static List<double> pridectinputdatas = new List<double>(); //输入
        public static float[] pridectinputdatas = new float[] { }; //输入

        public static List<string[][]> listOfArrays = new List<string[][]>();

        //选择预测数据
        public static string pridectdata { get; set; }
        public static DataTable pridectdatatable = new DataTable();
    }
}
