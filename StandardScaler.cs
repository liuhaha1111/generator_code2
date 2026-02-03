using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace WisdomGrowth
{
    public class StandardScaler
    {
        private double mean;
        private double std;
        private bool isFitted = false;

        // 拟合数据，计算均值和标准差
        public void Fit(List<double> data)
        {
            if (data == null || !data.Any())
                throw new ArgumentException("数据不能为空");

            // 计算均值
            mean = data.Average();

            // 计算标准差
            double sumSquaredDiff = 0;
            foreach (var value in data)
            {
                sumSquaredDiff += Math.Pow(value - mean, 2);
            }
            //double variance = sumSquaredDiff / data.Count;
            double variance = sumSquaredDiff / (data.Count - 1);
            std = Math.Sqrt(variance);

            // 防止除零错误
            if (std == 0)
                std = 1;

            isFitted = true;
        }

        // 转换数据
        public double[] Transform(List<double> data)
        {
            if (!isFitted)
                throw new InvalidOperationException("在转换数据之前需要先拟合模型");

            if (data == null || !data.Any())
                throw new ArgumentException("数据不能为空");

            return data.Select(value => (value - mean) / std).ToArray();
        }

        // 拟合并转换数据
        public double[] FitTransform(List<double> data)
        {
            Fit(data);
            return Transform(data);
        }
    }
}
