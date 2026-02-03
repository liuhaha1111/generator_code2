using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WisdomGrowth.ModelTraining
{
    class NeuralNetwork
    {

        private int inputNodes;
        private int hiddenNodes;
        private int outputNodes;
        private double[,] weightsInputHidden;
        private double[,] weightsHiddenOutput;
        private double[] biasesHidden;
        private double[] biasesOutput;
        private double learningRate;
        private double regularizationRate;

        public NeuralNetwork(int inputNodes, int hiddenNodes, int outputNodes, double learningRate, double regularizationRate)
        {
            this.inputNodes = inputNodes;
            this.hiddenNodes = hiddenNodes;
            this.outputNodes = outputNodes;
            this.learningRate = learningRate;
            this.regularizationRate = regularizationRate;

            // 随机初始化权重和偏置
            weightsInputHidden = new double[inputNodes, hiddenNodes];
            weightsHiddenOutput = new double[hiddenNodes, outputNodes];
            biasesHidden = new double[hiddenNodes];
            biasesOutput = new double[outputNodes];

            Random random = new Random();
            for (int i = 0; i < inputNodes; i++)
            {
                for (int j = 0; j < hiddenNodes; j++)
                {
                    weightsInputHidden[i, j] = random.NextDouble() * 2 - 1;
                }
            }
            for (int i = 0; i < hiddenNodes; i++)
            {
                for (int j = 0; j < outputNodes; j++)
                {
                    weightsHiddenOutput[i, j] = random.NextDouble() * 2 - 1;
                }
                biasesHidden[i] = random.NextDouble() * 2 - 1;
            }
            for (int i = 0; i < outputNodes; i++)
            {
                biasesOutput[i] = random.NextDouble() * 2 - 1;
            }
        }

        // 激活函数，这里使用Sigmoid函数
        private double Sigmoid(double x)
        {
            return 1 / (1 + Math.Exp(-x));
        }

        // 前向传播
        public double[] ForwardPropagation(double[] input)
        {
            double[] hiddenOutput = new double[hiddenNodes];
            double[] output = new double[outputNodes];

            // 计算隐藏层的输出
            for (int j = 0; j < hiddenNodes; j++)
            {
                double sum = 0;
                for (int i = 0; i < inputNodes; i++)
                {
                    sum += input[i] * weightsInputHidden[i, j];
                }
                sum += biasesHidden[j];
                hiddenOutput[j] = Sigmoid(sum);
            }

            // 计算输出层的输出
            for (int k = 0; k < outputNodes; k++)
            {
                double sum = 0;
                for (int j = 0; j < hiddenNodes; j++)
                {
                    sum += hiddenOutput[j] * weightsHiddenOutput[j, k];
                }
                sum += biasesOutput[k];
                output[k] = Sigmoid(sum);
            }

            return output;
        }

        // 反向传播
        public void BackPropagation(double[] input, double[] target)
        {
            double[] hiddenOutput = new double[hiddenNodes];
            double[] output = new double[outputNodes];

            // 前向传播，得到隐藏层和输出层的输出
            for (int j = 0; j < hiddenNodes; j++)
            {
                double sum = 0;
                for (int i = 0; i < inputNodes; i++)
                {
                    sum += input[i] * weightsInputHidden[i, j];
                }
                sum += biasesHidden[j];
                hiddenOutput[j] = Sigmoid(sum);
            }

            for (int k = 0; k < outputNodes; k++)
            {
                double sum = 0;
                for (int j = 0; j < hiddenNodes; j++)
                {
                    sum += hiddenOutput[j] * weightsHiddenOutput[j, k];
                }
                sum += biasesOutput[k];
                output[k] = Sigmoid(sum);
            }

            // 计算输出层的误差
            double[] outputError = new double[outputNodes];
            for (int k = 0; k < outputNodes; k++)
            {
                outputError[k] = output[k] * (1 - output[k]) * (target[k] - output[k]);
            }

            // 计算隐藏层的误差
            double[] hiddenError = new double[hiddenNodes];
            for (int j = 0; j < hiddenNodes; j++)
            {
                double sum = 0;
                for (int k = 0; k < outputNodes; k++)
                {
                    sum += outputError[k] * weightsHiddenOutput[j, k];
                }
                hiddenError[j] = hiddenOutput[j] * (1 - hiddenOutput[j]) * sum;
            }

            // 更新输出层的权重和偏置
            for (int j = 0; j < hiddenNodes; j++)
            {
                for (int k = 0; k < outputNodes; k++)
                {
                    weightsHiddenOutput[j, k] += learningRate * outputError[k] * hiddenOutput[j] - learningRate * regularizationRate * weightsHiddenOutput[j, k];
                }
            }
            for (int k = 0; k < outputNodes; k++)
            {
                biasesOutput[k] += learningRate * outputError[k];
            }

            // 更新隐藏层的权重和偏置
            for (int i = 0; i < inputNodes; i++)
            {
                for (int j = 0; j < hiddenNodes; j++)
                {
                    weightsInputHidden[i, j] += learningRate * hiddenError[j] * input[i] - learningRate * regularizationRate * weightsInputHidden[i, j];
                }
            }
            for (int j = 0; j < hiddenNodes; j++)
            {
                biasesHidden[j] += learningRate * hiddenError[j];
            }
        }

        // 训练函数
        public double Train(double[][] inputs, double[][] targets, int maxIterations)
        {
            for (int iteration = 0; iteration < maxIterations; iteration++)
            {
                double totalError = 0;
                for (int i = 0; i < inputs.Length; i++)
                {
                    double[] output = ForwardPropagation(inputs[i]);
                    double[] target = targets[i];
                    for (int j = 0; j < outputNodes; j++)
                    {
                        totalError += 0.5 * Math.Pow(target[j] - output[j], 2);
                    }
                    BackPropagation(inputs[i], targets[i]);
                }
                // 每100次迭代打印一次误差
                if (iteration % 100 == 0 && iteration != 0)
                {
                    Console.WriteLine($"Iteration {iteration}: Error = {totalError}");
                }
            }

            // 返回训练后的平均误差作为训练分数
            return  inputs.Length;
            //return totalError / inputs.Length;
        }
    }
}