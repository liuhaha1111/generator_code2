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
    public partial class OutlierRemovalForm : Form
    {
        public OutlierRemovalForm()
        {
            InitializeComponent();
        }
        AutoResizeForm asc = new AutoResizeForm();
        private void SureBtn_Click(object sender, EventArgs e)
        {
            DataSelectInfo.threshold = double.Parse(thresholdTxt.Text.ToString());
            List<DataTable> dataTables = DataSelectInfo.selectedDataTables;
            DataSelectInfo.selectedDataTables = null;
            // 使用for循环遍历List
            for (int i = 0; i < dataTables.Count; i++)
            {
                DataTable tables = dataTables[i];
                DataTable resulttables = null;
                DataTable column1dataTable = null;
                DataTable columndataTable = null;
                int Columnindex = 0;
                foreach (DataColumn column in tables.Columns)
                {
                    Columnindex++;
                    if (Columnindex == 1)
                    {
                        column1dataTable = CreateNewDataTable(tables, column.ColumnName);
                        continue;
                    }
                    else
                    {
                        columndataTable = RemoveOutliers(tables, column.ColumnName, DataSelectInfo.threshold);

                        //var maxValue = columndataTable.AsEnumerable().Max(row => row.Field<double>(column.ColumnName));
                        //var minValue = columndataTable.AsEnumerable().Min(row => row.Field<double>(column.ColumnName));


                        if (Columnindex == 2)
                        {
                            resulttables = MergeDataTables(column1dataTable, columndataTable);
                        }
                        else
                        {
                            resulttables = MergeDataTables(resulttables, columndataTable);
                        }
                    }
                }
                DataSelectInfo.selectedDataTables.Add(resulttables);
            }
        }
        public static DataTable MergeDataTables(DataTable dt1, DataTable dt2)
        {
            // 创建一个新的DataTable
            DataTable mergedTable = dt1.Clone();

            // 将dt2的列复制到mergedTable
            foreach (DataColumn column in dt2.Columns)
            {
                mergedTable.Columns.Add(column.ColumnName);
            }

            // 将dt1的数据复制到mergedTable
            foreach (DataRow row in dt1.Rows)
            {
                DataRow newRow = mergedTable.NewRow();
                foreach (DataColumn column in dt1.Columns)
                {
                    newRow[column.ColumnName] = row[column.ColumnName];
                }
                mergedTable.Rows.Add(newRow);
            }
            int i = 0;
            // 将dt2的数据复制到mergedTable
            foreach (DataRow row in dt2.Rows)
            {
                DataRow newRow = mergedTable.NewRow();

                foreach (DataColumn column in dt2.Columns)
                {
                    mergedTable.Rows[i][1] = row[column.ColumnName];
                }
                i++;
            }

            return mergedTable;
        }
        public static DataTable CreateNewDataTable(DataTable originalTable, string column1Name)
        {
            DataTable newTable = new DataTable();
            // 添加新列
            newTable.Columns.Add(column1Name, originalTable.Columns[column1Name].DataType);
            // 复制数据
            foreach (DataRow originalRow in originalTable.Rows)
            {
                DataRow newRow = newTable.NewRow();
                newRow[column1Name] = originalRow[column1Name];
                newTable.Rows.Add(newRow);
            }
            return newTable;
        }
        public static double[] ConvertToDoubleArray(DataTable dataTable, string columnName)
        {
            // 假设DataTable只有一列，且每个单元格的值都可以转换为double
            return dataTable.AsEnumerable().Select(row => Convert.ToDouble(row[columnName])).ToArray();
        }
        public DataTable RemoveOutliers(DataTable dataTable, string columnName, double factor)
        {
            //// 转换DataTable为double类型的数组
            //var data = dataTable.AsEnumerable().Select(row => Convert.ToDouble(row[columnName])).ToArray();
            double[] data = ConvertToDoubleArray(dataTable, columnName);
            // 计算四分位数
            double q1 = data.OrderBy(x => x).ElementAt((int)Math.Floor(data.Length * 0.25));
            double q3 = data.OrderBy(x => x).ElementAt((int)Math.Floor(data.Length * 0.75));
            double iqr = q3 - q1; // 四分位差

            // 计算阈值
            double lowerBound = q1 - (iqr * factor);
            double upperBound = q3 + (iqr * factor);
            // 筛选出不是异常值的行
            var nonOutliers = dataTable.AsEnumerable().Where(row =>
            {
                var value = Convert.ToDouble(row[columnName]);
                return value >= lowerBound && value <= upperBound;
            });

            // 创建一个新的DataTable并添加筛选后的数据
            var newDataTable = dataTable.Clone();
            foreach (var row in nonOutliers)
            {
                newDataTable.ImportRow(row);
            }

            return newDataTable;
        }

        private void OutlierRemovalForm_Load(object sender, EventArgs e)
        {
            asc.controllInitializeSize(this);
        }

        private void OutlierRemovalForm_SizeChanged(object sender, EventArgs e)
        {
            asc.controlAutoSize(this);
        }
    }
}
