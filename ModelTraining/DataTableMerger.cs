using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WisdomGrowth.ModelTraining
{
    public static class DataTableMerger
    {
        public static string[] GetFirstColumnAsStringArray(DataTable dataTable)
        {
            if (dataTable == null || dataTable.Columns.Count == 0)
                throw new ArgumentException("DataTable为空或不包含任何列");

            var list = new List<string>();

            foreach (DataRow row in dataTable.Rows)
            {
                object value = row[0];
                list.Add(value == DBNull.Value ? string.Empty : value.ToString());
            }

            return list.ToArray();
        }
        public static DataTable RemoveFirstColumn(DataTable originalTable)
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
        public static bool CheckFirstColumnName(DataTable dataTable)
        {
            // 检查DataTable是否有列
            if (dataTable.Columns.Count > 0)
            {
                // 获取第一列的名称
                string firstColumnName = dataTable.Columns[0].ColumnName;
                // 比较列名是否为id（不区分大小写）
                return firstColumnName.Equals("ID", StringComparison.OrdinalIgnoreCase);
            }
            return false;
        }
        /// <summary>
        /// 将多个DataTable按照指定索引规则合并为一个表
        /// </summary>
        /// <param name="dataTables">要合并的DataTable列表</param>
        /// <param name="indexesList">每个DataTable对应的索引数组列表</param>
        /// <returns>合并后的DataTable</returns>
        public static DataTable MergeAllTables(List<DataTable> dataTables)
        {
            // 验证输入参数
            ValidateInputs(dataTables);

            // 从第一个表开始作为初始结果
            DataTable resultTable = dataTables[0].Copy();
            string[] resultIndexes = GetFirstColumnAsStringArray(resultTable);
            // 依次合并后续的每个表
            for (int i = 1; i < dataTables.Count; i++)
            {
                DataTable currentTable = dataTables[i];
                string[] currentIndexes = GetFirstColumnAsStringArray(currentTable);
                // 找出当前结果表和要合并表的共同索引
                var commonIndexes = resultIndexes.Intersect(currentIndexes).ToList();

                // 调用方法判断第一列名是否为id
                bool isFirstColumnNameId1 = CheckFirstColumnName(resultTable);

                if (isFirstColumnNameId1)
                {
                    resultTable = RemoveFirstColumn(resultTable);
                }
                // 调用方法判断第一列名是否为id
                bool iscurrentTableFirstColumnNameId1 = CheckFirstColumnName(currentTable);

                if (iscurrentTableFirstColumnNameId1)
                {
                    currentTable = RemoveFirstColumn(currentTable);
                }
                // 应用合并规则进行两两合并
                resultTable = MergeTwoTables(resultTable, currentTable,
                                           resultIndexes, currentIndexes, commonIndexes);

               
                resultIndexes = commonIndexes.Select(index => index.ToString()).ToArray();
            }

            return resultTable;
        }

        /// <summary>
        /// 验证输入参数的有效性
        /// </summary>
        private static void ValidateInputs(List<DataTable> dataTables)
        {
            if (dataTables == null)
                throw new ArgumentNullException(nameof(dataTables), "数据表格列表不能为null");

            if (dataTables.Count == 0)
                throw new ArgumentException("数据表格列表不能为空", nameof(dataTables));
        }

        /// <summary>
        /// 合并两个DataTable，基于索引标识而非列名
        /// </summary>
        private static DataTable MergeTwoTables(DataTable dataTable1, DataTable dataTable2,
                                              string[] dt1Indexes, string[] dt2Indexes,
                                              List<string> commonIndexes)
        {
            // 创建结果表
            DataTable resultTable = new DataTable();

            // 添加第一个表的列
            foreach (DataColumn col in dataTable1.Columns)
            {
                resultTable.Columns.Add(col.ColumnName, col.DataType);
            }

            // 添加第二个表的列
            foreach (DataColumn col in dataTable2.Columns)
            {
                if (!resultTable.Columns.Contains(col.ColumnName))
                {
                    resultTable.Columns.Add(col.ColumnName, col.DataType);
                }
            }

            // 为每个共同索引添加合并后的数据行
            foreach (string index in commonIndexes)
            {
                // 找到第一个表中对应索引的行
                int dt1RowIndex = Array.IndexOf(dt1Indexes, index);
                // 找到第二个表中对应索引的行
                int dt2RowIndex = Array.IndexOf(dt2Indexes, index);

                if (dt1RowIndex != -1 && dt2RowIndex != -1)
                {
                    DataRow newRow = resultTable.NewRow();

                    // 复制第一个表的数据
                    foreach (DataColumn col in dataTable1.Columns)
                    {
                        newRow[col.ColumnName] = dataTable1.Rows[dt1RowIndex][col];
                    }

                    // 复制第二个表的数据
                    foreach (DataColumn col in dataTable2.Columns)
                    {
                        newRow[col.ColumnName] = dataTable2.Rows[dt2RowIndex][col];
                    }

                    resultTable.Rows.Add(newRow);
                }
            }

            return resultTable;
        }

        /// <summary>
        /// 从单个表添加行到结果表（处理只存在于一个表的索引）
        /// </summary>
        private static void AddRowFromSingleTable(DataTable resultTable, DataTable sourceTable,
                                                string[] sourceIndexes, string index)
        {
            int rowIndex = Array.IndexOf(sourceIndexes, index);
            if (rowIndex != -1)
            {
                DataRow newRow = resultTable.NewRow();
                foreach (DataColumn col in sourceTable.Columns)
                {
                    newRow[col.ColumnName] = sourceTable.Rows[rowIndex][col];
                }
                resultTable.Rows.Add(newRow);
            }
        }
        /// <summary>
        /// 将DataTable按9:1比例拆分为两个DataTable
        /// </summary>
        /// <param name="sourceTable">源数据DataTable</param>
        /// <returns>包含两个DataTable的元组，第一个是90%数据，第二个是10%数据</returns>
        public static (DataTable table90, DataTable table10) SplitDataTableByRatio(DataTable sourceTable)
        {
            // 验证源表有效性
            if (sourceTable == null)
                throw new ArgumentNullException(nameof(sourceTable), "源DataTable不能为null");

            if (sourceTable.Rows.Count == 0)
                throw new ArgumentException("源DataTable不包含任何数据行", nameof(sourceTable));

            // 克隆表结构（不包含数据）
            DataTable table90 = sourceTable.Clone();
            DataTable table10 = sourceTable.Clone();

            int totalRows = sourceTable.Rows.Count;

            // 计算10%比例的行数（向上取整确保至少有1行）
            int tenPercentCount = (int)Math.Ceiling(totalRows * 0.1);
            tenPercentCount = Math.Max(1, tenPercentCount); // 确保至少1行
            tenPercentCount = Math.Min(tenPercentCount, totalRows - 1); // 确保不会超过总条数减1

            // 生成随机索引，用于选择10%的数据
            Random random = new Random();
            var randomIndexes = Enumerable.Range(0, totalRows)
                                         .OrderBy(x => random.Next())
                                         .Take(tenPercentCount)
                                         .ToHashSet();

            // 分配数据行
            for (int i = 0; i < totalRows; i++)
            {
                if (randomIndexes.Contains(i))
                {
                    table10.ImportRow(sourceTable.Rows[i]);
                }
                else
                {
                    table90.ImportRow(sourceTable.Rows[i]);
                }
            }

            return (table90, table10);
        }


        /// <summary>
        /// 将DataTable按1:1比例拆分为两个DataTable
        /// 若行数为奇数，舍弃最后一行
        /// </summary>
        /// <param name="sourceTable">源DataTable</param>
        /// <returns>包含两个DataTable的元组，分别为拆分后的结果</returns>
        public static (DataTable table1, DataTable table2) SplitDataTable(DataTable sourceTable)
        {
            // 初始化两个新的DataTable，复制源表的结构
            DataTable table1 = sourceTable?.Clone() ?? new DataTable();
            DataTable table2 = sourceTable?.Clone() ?? new DataTable();

            if (sourceTable == null || sourceTable.Rows.Count == 0)
            {
                return (table1, table2);
            }

            // 计算有效行数（如果是奇数则减1）
            int validRowCount = sourceTable.Rows.Count % 2 == 0
                ? sourceTable.Rows.Count
                : sourceTable.Rows.Count - 1;

            // 每个表应包含的行数
            int rowsPerTable = validRowCount / 2;

            // 填充第一个表（前半部分数据）
            for (int i = 0; i < rowsPerTable; i++)
            {
                table1.ImportRow(sourceTable.Rows[i]);
            }

            // 填充第二个表（后半部分数据）
            for (int i = rowsPerTable; i < validRowCount; i++)
            {
                table2.ImportRow(sourceTable.Rows[i]);
            }

            return (table1, table2);
        }


    }

    //// 假设的数据源类（根据您的实际情况调整）
    //public static class DataSelectInfo
    //{
    //    // 示例：存储要合并的DataTable列表
    //    public static List<DataTable> inputsdatatable { get; set; } = new List<DataTable>();
    //}
}
