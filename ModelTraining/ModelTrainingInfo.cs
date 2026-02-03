using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WisdomGrowth.ModelTraining
{
    public class ModelTrainingInfo
    {
        //选择模型
        public static string selectmodel { get; set; }
        //Stacking模型配置Model1
        public static string selectstackingmodel1 { get; set; }
        //BPNN参数设置
        public static double BPNNHiddennode { get; set; }
        public static double BPNNPenaltyfactor { get; set; }
        public static double BPNNMaxIterations { get; set; }
        public static string BPNNGradientDescent { get; set; }

        //GBR参数设置
        public static double GBRLearningrate { get; set; }
        public static double GBRNumberoflearners { get; set; }
        public static double GBRMinimumpartitionnode { get; set; }
        public static double GBRMaximumdepth { get; set; }
        public static string GBRLossfunction { get; set; }

        //RFR参数设置
        public static double RFRNumberoflearners { get; set; }
        public static double RFRMinimumpartitionnode { get; set; }
        public static double RFRMaximumdepth { get; set; }

        //SVR参数设置
        public static double SVRtolerance { get; set; }
        public static double SVRregularizationparameter { get; set; }
        public static double SVRMaxIterations { get; set; }
        public static string SVRkernelFunction { get; set; }
        //实测数据
        public static DataTable actualDataTable = new DataTable();
        //预测数据
        public static DataTable outputsdatatable = new DataTable();
        //训练分数
        public static string trainingscore { get; set; }
    }
}
