using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WisdomGrowth.DataBase
{
    public class DataSelectInfo
    {
        // 数据选取
        //选择目录
        public static string selectedFolderPath { get; set; }
        //输入
        public static List<string> outputs = new List<string>();
        //输出
        public static string input { get; set; }
        //输入
        public static List<DataTable> inputsdatatable = new List<DataTable>();
        //输出
        public static List<DataTable> outputsdatatable = new List<DataTable>();
        //总共输入输出表
        public static List<DataTable> selectedDataTables = new List<DataTable>(); 
        public static List<string> DataCharts = new List<string>(); 
        //异常值去除
        //阈值
        public static double threshold { get; set; }
    }
}
