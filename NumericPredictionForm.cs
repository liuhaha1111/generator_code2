using Accord.Math;
using Accord.Neuro;
using Accord.Neuro.Learning;
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
    public partial class NumericPredictionForm : Form
    {
        public NumericPredictionForm()
        {
            InitializeComponent();
            //DataTable redatatable = NumericHelper.QueryNumericToDb();
            //if (redatatable.Rows.Count > 0)
            //{
            //    UpdateDataGridViewDataSource1(redatatable);

            //    //UpdateDataGridViewDataSource(redatatable);
            //}
        }
        // 假设你已有一个绑定的数据源
        BindingSource bindingSource1 = new BindingSource();
        private void UpdateDataGridViewDataSource1(DataTable newDataSource)
        {
            // 将新的数据源绑定到BindingSource
            bindingSource1.DataSource = newDataSource;
            // 将BindingSource的数据源绑定到DataGridView
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = bindingSource1;
            // 强制DataGridView刷新显示
            dataGridView1.Refresh();
        }
        AutoResizeForm asc = new AutoResizeForm();
        //DataTable 的列转成double存储数据
        public DataTable ChangeColumn(DataTable dataTable)
        {
            // 创建一个新的 DataTable 用于存储转换后的数据
            DataTable newDataTable = new DataTable();
            foreach (DataColumn column in dataTable.Columns)
            {
                newDataTable.Columns.Add(column.ColumnName, typeof(double));
            }

            // 遍历原 DataTable 的每一行
            foreach (DataRow row in dataTable.Rows)
            {
                DataRow newRow = newDataTable.NewRow();
                // 遍历每一列
                for (int i = 0; i < dataTable.Columns.Count; i++)
                {
                    try
                    {
                        // 尝试将单元格的值转换为 double 类型
                        newRow[i] = Convert.ToDouble(row[i]);
                    }
                    catch (FormatException)
                    {
                        // 若转换失败，可根据需求处理，这里简单输出错误信息
                        Console.WriteLine($"无法将单元格 {row.Table.Columns[i].ColumnName} 的值 '{row[i]}' 转换为 double 类型。");
                        newRow[i] = double.NaN; // 用 NaN 表示转换失败
                    }
                }
                // 将新行添加到新的 DataTable 中
                newDataTable.Rows.Add(newRow);
            }
            return newDataTable;
        }
        private const int MaxEpochs = 5000;
        private const double LearningRate = 0.1;
        private const double Momentum = 0.0;
        private const double ErrorThreshold = 0.01;
        List<string[][]> listOfArrays = new List<string[][]>();
        private void NumericPredictionForm_Load(object sender, EventArgs e)
        {
            DataTable redatatable = NumericHelper.QueryNumericToDb();
            if (redatatable.Rows.Count > 0)
            {
                UpdateDataGridViewDataSource1(redatatable);
            }
            #region hide by 2025/10/20
            //asc.controllInitializeSize(this);

            //redatatable = RemoveSecondColumn(redatatable);
            //redatatable = ConvertColumnsToDouble(redatatable);
            //redatatable = RemoveFirstColumn(redatatable);

            //// 假设这里有一个 DataTable 对象
            //List<DataTable> dataTables = DataSelectInfo.outputsdatatable;
            //DataTable outputData = new DataTable();
            //for (int i = 0; i < dataTables.Count; i++)
            //{
            //    outputData = dataTables[i];
            //}
            //// 创建新的DataTable
            //outputData = ConvertColumnsToDouble(outputData);
            //outputData = RemoveFirstColumn(outputData);

            //// 准备一个输入和一个输出的 DataTable
            //var (inputDataTable, outputDataTable) = PrepareDataTables(redatatable, outputData);

            //// 将输入 DataTable 转换为输入数组
            //double[][] inputs = inputDataTable.ToJagged<double>();

            //// 将输出 DataTable 转换为输出数组
            //double[][] outputs = outputDataTable.ToJagged<double>();

            //// 配置神经网络
            //var activationFunction = new SigmoidFunction();
            //var network = new ActivationNetwork(activationFunction, inputDataTable.Columns.Count, 10, 10, outputDataTable.Columns.Count);

            //// 初始化权重
            //new NguyenWidrow(network).Randomize();

            //// 创建教师算法（反向传播）
            //var teacher = new BackPropagationLearning(network)
            //{
            //    LearningRate = LearningRate,
            //    Momentum = Momentum
            //};

            //// 训练模型
            //double error = double.PositiveInfinity;
            //for (int i = 0; i < MaxEpochs; i++)
            //{
            //    error = teacher.RunEpoch(inputs, outputs);
            //    if (error < ErrorThreshold) break;
            //}


            //// 预测值
            //double[][] predicted = new double[inputs.Length][];
            //for (int i = 0; i < inputs.Length; i++)
            //{
            //    predicted[i] = network.Compute(inputs[i]);
            //}

            //// 将预测结果转换为 DataTable
            //DataTable predictedDataTable = ConvertToDataTable(predicted, outputDataTable);
            //// 创建一个新的 DataTable 用于存储转换后的数据
            //DataTable newDataTable = ChangeColumn(predictedDataTable);
            //// 转换为一行四列的DataTable并计算平均值
            //DataTable resultTable = ConvertToSingleRow(newDataTable);
            //// 按列输出 DataTable
            //Visualization.pridectdata = PrintDataTableByColumn(resultTable);
            #endregion
        }
        static DataTable ConvertToSingleRow(DataTable originalTable)
        {
            DataTable resultTable = new DataTable();

            // 复制列结构
            foreach (DataColumn column in originalTable.Columns)
            {
                resultTable.Columns.Add(column.ColumnName, column.DataType);
            }

            // 创建新行
            DataRow newRow = resultTable.NewRow();

            // 计算每列的平均值
            for (int i = 0; i < originalTable.Columns.Count; i++)
            {
                double sum = 0;
                foreach (DataRow row in originalTable.Rows)
                {
                    sum += Convert.ToDouble(row[i]);
                }
                double average = sum / originalTable.Rows.Count;
                newRow[i] = average;
            }

            // 添加新行到结果表
            resultTable.Rows.Add(newRow);

            return resultTable;
        }

        static string PrintDataTableByColumn(DataTable dataTable)
        {
            string pridectdata = "";
            // 遍历每一列
            for (int colIndex = 0; colIndex < dataTable.Columns.Count; colIndex++)
            {
                string columnName = dataTable.Columns[colIndex].ColumnName;
                Console.Write($"{columnName}, : ");
                pridectdata = pridectdata + columnName + ":";
                // 获取该列在第一行的值
                double value = double.Parse( dataTable.Rows[0][colIndex].ToString());
                Console.WriteLine(value);
                pridectdata = pridectdata + Math.Round(value, 3) + "\n";


            }
            return pridectdata;
        }
        private DataTable ConvertToDataTable(double[][] data, DataTable templateTable)
        {
            DataTable resultTable = new DataTable();
            foreach (DataColumn column in templateTable.Columns)
            {
                resultTable.Columns.Add(column.ColumnName, column.DataType);
            }

            for (int i = 0; i < data.Length; i++)
            {
                DataRow newRow = resultTable.NewRow();
                for (int j = 0; j < data[i].Length; j++)
                {
                    newRow[j] = data[i][j];
                }
                resultTable.Rows.Add(newRow);
            }

            return resultTable;
        }

        private (DataTable input, DataTable output) PrepareDataTables(DataTable inputDataTable, DataTable outputDataTable)
        {
            return (inputDataTable, outputDataTable);
        }
        public DataTable RemoveSecondColumn(DataTable originalTable)
        {
            // 检查原表是否为 null 或者没有列
            if (originalTable == null || originalTable.Columns.Count < 2)
            {
                return null; // 或者可以选择返回一个空的 DataTable
            }

            // 克隆表结构，但不包括第二列
            DataTable newTable = originalTable.Clone();
            newTable.Columns.RemoveAt(1); // 移除克隆表的第二列，索引从 0 开始，所以第二列索引是 1

            // 复制行到新表，但不包括第二列的数据
            foreach (DataRow row in originalTable.Rows)
            {
                DataRow newRow = newTable.NewRow();
                int newColumnIndex = 0;
                for (int i = 0; i < originalTable.Columns.Count; i++)
                {
                    if (i == 1)
                    {
                        continue; // 跳过第二列
                    }
                    newRow[newColumnIndex] = row[i];
                    newColumnIndex++;
                }
                newTable.Rows.Add(newRow);
            }

            return newTable;
        }
        public DataTable RemoveFirstColumn(DataTable originalTable)
        {
            if (originalTable == null || originalTable.Columns.Count == 0)
                return null; // 或者可以选择返回一个空的DataTable

            // 克隆表结构，但不包括第一列
            DataTable newTable = originalTable.Clone();
            newTable.Columns.RemoveAt(0); // 移除克隆表的第一个列

            // 复制行到新表，但不包括第一列的数据
            foreach (DataRow row in originalTable.Rows)
            {
                DataRow newRow = newTable.NewRow();
                for (int i = 1; i < originalTable.Columns.Count; i++) // 从第二列开始复制
                {
                    newRow[i - 1] = row[i]; // 因为已经移除了第一列，索引需要调整
                }
                newTable.Rows.Add(newRow);
            }

            return newTable;
        }
        //datatable非数字单元格转成0
        static DataTable ConvertColumnsToDouble(DataTable original)
        {
            foreach (DataRow row in original.Rows)
            {
                for (int i = 1; i < original.Columns.Count; i++) // 从第二列开始（索引1）
                {
                    // 尝试转换并添加到新行，对于非数字字符串，这里可以选择抛出异常或使用特定值代替，例如0或double.NaN
                    try
                    {
                        row[i] = Convert.ToDouble(row[i]); // 注意，这里假定所有非数字字符串都可以转换为0或其他值，根据需要调整逻辑处理非数字字符串。
                    }
                    catch (FormatException) // 处理无法转换的情况，可以选择抛出异常或使用默认值，例如0或double.NaN。
                    {
                        row[i] = 0; // 或者使用 double.NaN 表示非数字值。
                    }
                }
            }
            return original;
        }
        private void NumericPredictionForm_SizeChanged(object sender, EventArgs e)
        {
            asc.controllInitializeSize(this);
        }
        /// <summary>
        /// 对数值进行修改操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_CurrentCellDirtyStateChanged_1(object sender, EventArgs e)
        {
            // 当前单元格 是否有未提交的更改
            if (dataGridView1.CurrentCell.OwningColumn.Name == "NumberColumn1")
            {
                // 标记单元格为未脏，以防进一步的处理
                dataGridView1.CommitEdit(DataGridViewDataErrorContexts.Commit);
                double number = double.Parse(this.dataGridView1.CurrentRow.Cells["NumberColumn1"].Value.ToString());
                int id = Convert.ToInt32(this.dataGridView1.CurrentRow.Cells["idColumn1"].Value.ToString());
                NumericHelper.UpdateNumericToDb(number, id);
            }
        }
        private DataTable DataGridViewToDataTable(DataGridView dgv)
        {
            DataTable dt = new DataTable();

            // 添加列
            foreach (DataGridViewColumn column in dgv.Columns)
            {
                dt.Columns.Add(column.HeaderText, column.ValueType);
            }

            // 添加行
            foreach (DataGridViewRow row in dgv.Rows)
            {
                if (!row.IsNewRow) // 忽略新行
                {
                    DataRow newRow = dt.NewRow();
                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        newRow[cell.ColumnIndex] = cell.Value;
                    }
                    dt.Rows.Add(newRow);
                }
            }

            return dt;
        }
        List<string > outputFileName = new List<string>();
        public DataTable resultDt = new DataTable();
        private void SureBtn_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            //this.Close();
            #region hide by 2025/10/20
            //DataTable resultdatable= DataGridViewToDataTable(dataGridView1);
            //resultDt = resultdatable;
            //List<DataTable> dataTables = DataSelectInfo.outputsdatatable;
            //// 使用for循环遍历List
            //for (int i = 0; i < dataTables.Count; i++)
            //{
            //    DataTable tables = dataTables[i];
            //    int Columnindex = 0;
            //    foreach (DataColumn column in tables.Columns)
            //    {
            //        Columnindex++;
            //        if (Columnindex == 1)
            //        {
            //            continue;
            //        }
            //        else
            //        {
            //            outputFileName.Add(column.ColumnName);
            //        }
            //    }
            //}
            //this.DialogResult = DialogResult.OK;
            #endregion

        }

        private void CancelBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
