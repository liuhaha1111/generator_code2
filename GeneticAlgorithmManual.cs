using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WisdomGrowth
{
    //// 手动实现遗传算法类
    //public class GeneticAlgorithmManual
    //{
    //    private Func<double[], double> fitnessFunction;
    //    private double[] lowerBounds;
    //    private double[] upperBounds;
    //    private double[] precision;
    //    private int populationSize;
    //    private int generations;
    //    private double mutationRate;
    //    private Random random;

    //    // 种群相关变量
    //    private double[][] population;
    //    private double[] fitnessValues;
    //    private double[][] newPopulation;

    //    // 最优解
    //    public double[] BestSolution { get; private set; }
    //    public double BestFitness { get; private set; }

    //    // 构造函数
    //    public GeneticAlgorithmManual(
    //        Func<double[], double> fitnessFunction,
    //        double[] lowerBounds,
    //        double[] upperBounds,
    //        double[] precision,
    //        int populationSize = 100,
    //        int generations = 100,
    //        double mutationRate = 0.01)
    //    {
    //        this.fitnessFunction = fitnessFunction;
    //        this.lowerBounds = lowerBounds;
    //        this.upperBounds = upperBounds;
    //        this.precision = precision;
    //        this.populationSize = populationSize;
    //        this.generations = generations;
    //        this.mutationRate = mutationRate;
    //        this.random = new Random(42); // 固定随机种子，确保可重复性

    //        // 验证输入参数
    //        if (lowerBounds.Length != upperBounds.Length)
    //            throw new ArgumentException("Lower and upper bounds must have the same length.");

    //        if (precision.Length != lowerBounds.Length)
    //            throw new ArgumentException("Precision array must have the same length as bounds.");

    //        for (int i = 0; i < lowerBounds.Length; i++)
    //        {
    //            if (lowerBounds[i] >= upperBounds[i])
    //                throw new ArgumentException($"Lower bound at index {i} must be less than upper bound.");

    //            if (precision[i] <= 0)
    //                throw new ArgumentException($"Precision at index {i} must be positive.");
    //        }
    //    }

    //    // 应用精度约束
    //    private double ApplyPrecision(double value, double min, double max, double prec)
    //    {
    //        // 确保值在边界内
    //        value = Math.Max(min, Math.Min(max, value));

    //        // 应用精度约束 - 四舍五入到指定精度
    //        double multiplier = 1.0 / prec;
    //        double roundedValue = Math.Round(value * multiplier) / multiplier;

    //        // 确保结果在边界内
    //        return Math.Max(min, Math.Min(max, roundedValue));
    //    }

    //    // 初始化种群
    //    private void InitializePopulation()
    //    {
    //        int dimensions = lowerBounds.Length;
    //        population = new double[populationSize][];
    //        fitnessValues = new double[populationSize];
    //        newPopulation = new double[populationSize][];

    //        // 确保BestSolution已初始化
    //        BestSolution = new double[dimensions];
    //        BestFitness = double.MaxValue;

    //        for (int i = 0; i < populationSize; i++)
    //        {
    //            population[i] = new double[dimensions];
    //            newPopulation[i] = new double[dimensions];

    //            bool isZeroSolution = true; // 检查是否为零解

    //            for (int j = 0; j < dimensions; j++)
    //            {
    //                // 随机生成初始值，并应用精度约束
    //                double randomValue = lowerBounds[j] + random.NextDouble() * (upperBounds[j] - lowerBounds[j]);
    //                population[i][j] = ApplyPrecision(randomValue, lowerBounds[j], upperBounds[j], precision[j]);

    //                // 检查是否为零解
    //                if (Math.Abs(population[i][j]) > 1e-10)
    //                {
    //                    isZeroSolution = false;
    //                }
    //            }

    //            // 如果是零解，重新生成
    //            if (isZeroSolution)
    //            {
    //                i--; // 回退，重新生成这个个体
    //            }
    //        }
    //    }

    //    // 评估种群
    //    private void EvaluatePopulation()
    //    {
    //        for (int i = 0; i < populationSize; i++)
    //        {
    //            try
    //            {
    //                // 计算适应度
    //                double fitness = fitnessFunction(population[i]);

    //                // 检查适应度是否为无穷大或NaN
    //                if (double.IsInfinity(fitness) || double.IsNaN(fitness))
    //                {
    //                    // 如果是无效值，给予一个很大的惩罚值
    //                    fitness = double.MaxValue / 2;
    //                }

    //                fitnessValues[i] = fitness;

    //                // 更新最佳解
    //                if (fitness < BestFitness)
    //                {
    //                    BestFitness = fitness;
    //                    Array.Copy(population[i], BestSolution, population[i].Length);
    //                }
    //            }
    //            catch (Exception ex)
    //            {
    //                // 处理异常，给予一个很大的惩罚值
    //                Console.WriteLine($"Error evaluating individual {i}: {ex.Message}");
    //                fitnessValues[i] = double.MaxValue / 2;
    //            }
    //        }
    //    }

    //    // 选择父代（锦标赛选择）
    //    private int TournamentSelection()
    //    {
    //        const int tournamentSize = 3;
    //        int bestIndex = random.Next(populationSize);

    //        for (int i = 1; i < tournamentSize; i++)
    //        {
    //            int candidateIndex = random.Next(populationSize);
    //            if (fitnessValues[candidateIndex] < fitnessValues[bestIndex])
    //            {
    //                bestIndex = candidateIndex;
    //            }
    //        }

    //        return bestIndex;
    //    }

    //    // 交叉操作（算术交叉）
    //    private void Crossover(int parent1Index, int parent2Index, int childIndex)
    //    {
    //        int dimensions = lowerBounds.Length;

    //        if (random.NextDouble() < 0.8) // 交叉概率
    //        {
    //            for (int i = 0; i < dimensions; i++)
    //            {
    //                // 算术交叉
    //                double alpha = random.NextDouble();
    //                double childValue = alpha * population[parent1Index][i] + (1 - alpha) * population[parent2Index][i];

    //                // 应用精度约束
    //                newPopulation[childIndex][i] = ApplyPrecision(childValue, lowerBounds[i], upperBounds[i], precision[i]);
    //            }
    //        }
    //        else
    //        {
    //            // 不交叉，直接复制一个父代
    //            Array.Copy(population[parent1Index], newPopulation[childIndex], dimensions);
    //        }
    //    }

    //    // 变异操作
    //    private void Mutate(int index)
    //    {
    //        int dimensions = lowerBounds.Length;

    //        for (int i = 0; i < dimensions; i++)
    //        {
    //            if (random.NextDouble() < mutationRate)
    //            {
    //                // 高斯变异
    //                double mutationValue = newPopulation[index][i] + random.NextGaussian(0, 1) * (upperBounds[i] - lowerBounds[i]) * 0.1;

    //                // 应用精度约束
    //                newPopulation[index][i] = ApplyPrecision(mutationValue, lowerBounds[i], upperBounds[i], precision[i]);
    //            }
    //        }
    //    }

    //    // 运行遗传算法
    //    public void Run()
    //    {
    //        InitializePopulation();
    //        EvaluatePopulation();

    //        for (int gen = 0; gen < generations; gen++)
    //        {
    //            // 确保newPopulation已正确初始化
    //            for (int i = 0; i < populationSize; i++)
    //            {
    //                if (newPopulation[i] == null)
    //                {
    //                    newPopulation[i] = new double[lowerBounds.Length];
    //                }
    //            }

    //            // 精英保留
    //            int bestIndex = Array.IndexOf(fitnessValues, BestFitness);

    //            if (bestIndex >= 0 && bestIndex < populationSize)
    //            {
    //                if (0 < populationSize)
    //                {
    //                    Array.Copy(population[bestIndex], newPopulation[0], population[bestIndex].Length);
    //                }
    //            }

    //            // 生成新一代
    //            for (int i = 1; i < populationSize; i++)
    //            {
    //                int parent1 = TournamentSelection();
    //                int parent2 = TournamentSelection();

    //                Crossover(parent1, parent2, i);
    //                Mutate(i);
    //            }

    //            // 更新种群
    //            double[][] temp = population;
    //            population = newPopulation;
    //            newPopulation = temp;

    //            // 评估新种群
    //            EvaluatePopulation();

    //            // 输出当前代的最佳适应度
    //            if ((gen + 1) % 100 == 0 || gen == 0)
    //            {
    //                Console.WriteLine($"Generation {gen + 1}: Best Fitness = {BestFitness}");

    //                // 输出最佳解的前几个值，用于调试
    //                if (BestSolution.Length > 0)
    //                {
    //                    Console.Write("Best Solution: [");
    //                    for (int i = 0; i < Math.Min(5, BestSolution.Length); i++)
    //                    {
    //                        Console.Write($"{BestSolution[i]:F6}{(i < Math.Min(4, BestSolution.Length - 1) ? ", " : "")}");
    //                    }
    //                    if (BestSolution.Length > 5)
    //                    {
    //                        Console.Write("...");
    //                    }
    //                    Console.WriteLine("]");
    //                }
    //            }
    //        }
    //    }
    //}

    //// 扩展Random类以支持高斯分布d
    //public static class RandomExtensions
    //{
    //    public static double NextGaussian(this Random r, double mu = 0, double sigma = 1)
    //    {
    //        // 使用Box-Muller变换生成高斯分布随机数
    //        double u1 = r.NextDouble();
    //        double u2 = r.NextDouble();

    //        double z0 = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Cos(2.0 * Math.PI * u2);

    //        return mu + sigma * z0;
    //    }
    //}


    public class GeneticAlgorithmManual
    {
        private readonly Func<double[], double> _fitnessFunction;
        private readonly double[] _lowerBounds;
        private readonly double[] _upperBounds;
        private readonly int _populationSize;
        private readonly int _generations;
        private readonly double _mutationRate;

        public GeneticAlgorithmManual(Func<double[], double> fitnessFunction, double[] lowerBounds, double[] upperBounds, double[] precision, int populationSize, int generations, double mutationRate)
        {
            _fitnessFunction = fitnessFunction;
            _lowerBounds = lowerBounds;
            _upperBounds = upperBounds;
            _populationSize = populationSize;
            _generations = generations;
            _mutationRate = mutationRate;
        }

        public double[] BestSolution { get; private set; }
        public double BestFitness { get; private set; }

        public void Run()
        {
            Random random = new Random();
            double[][] population = new double[_populationSize][];
            for (int i = 0; i < _populationSize; i++)
            {
                population[i] = new double[_lowerBounds.Length];
                for (int j = 0; j < _lowerBounds.Length; j++)
                {
                    population[i][j] = random.NextDouble() * (_upperBounds[j] - _lowerBounds[j]) + _lowerBounds[j];
                }
            }

            for (int generation = 0; generation < _generations; generation++)
            {
                double[] fitnessValues = new double[_populationSize];
                for (int i = 0; i < _populationSize; i++)
                {
                    fitnessValues[i] = _fitnessFunction(population[i]);
                }

                int bestIndex = Array.IndexOf(fitnessValues, fitnessValues.Min());
                if (generation == 0 || fitnessValues[bestIndex] < BestFitness)
                {
                    BestFitness = fitnessValues[bestIndex];
                    BestSolution = population[bestIndex];
                }

                double[][] newPopulation = new double[_populationSize][];
                for (int i = 0; i < _populationSize; i++)
                {
                    int parent1Index = SelectParent(fitnessValues, random);
                    int parent2Index = SelectParent(fitnessValues, random);
                    double[] child = Crossover(population[parent1Index], population[parent2Index], random);
                    Mutate(child, random);
                    newPopulation[i] = child;
                }

                population = newPopulation;
            }
        }

        private int SelectParent(double[] fitnessValues, Random random)
        {
            double totalFitness = fitnessValues.Sum();
            double r = random.NextDouble() * totalFitness;
            double sum = 0;
            for (int i = 0; i < fitnessValues.Length; i++)
            {
                sum += fitnessValues[i];
                if (sum >= r)
                {
                    return i;
                }
            }
            return fitnessValues.Length - 1;
        }

        private double[] Crossover(double[] parent1, double[] parent2, Random random)
        {
            double[] child = new double[parent1.Length];
            int crossoverPoint = random.Next(parent1.Length);
            for (int i = 0; i < crossoverPoint; i++)
            {
                child[i] = parent1[i];
            }
            for (int i = crossoverPoint; i < parent1.Length; i++)
            {
                child[i] = parent2[i];
            }
            return child;
        }

        private void Mutate(double[] individual, Random random)
        {
            for (int i = 0; i < individual.Length; i++)
            {
                if (random.NextDouble() < _mutationRate)
                {
                    individual[i] = random.NextDouble() * (_upperBounds[i] - _lowerBounds[i]) + _lowerBounds[i];
                }
            }
        }
    }
}

