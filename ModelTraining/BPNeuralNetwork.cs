using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WisdomGrowth.ModelTraining
{


    public class BPNeuralNetwork
    {
        private int inputLayerSize;
        private int hiddenLayerSize;
        private int outputLayerSize;
        private double[,] weightsInputHidden;
        private double[,] weightsHiddenOutput;
        private double learningRate;

        public BPNeuralNetwork(int inputLayerSize, int hiddenLayerSize, int outputLayerSize, double learningRate)
        {
            this.inputLayerSize = inputLayerSize;
            this.hiddenLayerSize = hiddenLayerSize;
            this.outputLayerSize = outputLayerSize;
            this.learningRate = learningRate;

            // Initialize weights with random values
            weightsInputHidden = new double[inputLayerSize, hiddenLayerSize];
            weightsHiddenOutput = new double[hiddenLayerSize, outputLayerSize];
            Random rand = new Random();
            for (int i = 0; i < inputLayerSize; i++)
            {
                for (int j = 0; j < hiddenLayerSize; j++)
                {
                    weightsInputHidden[i, j] = rand.NextDouble() - 0.5;
                }
            }
            for (int i = 0; i < hiddenLayerSize; i++)
            {
                for (int j = 0; j < outputLayerSize; j++)
                {
                    weightsHiddenOutput[i, j] = rand.NextDouble() - 0.5;
                }
            }
        }

        public double[] FeedForward(double[] inputs)
        {
            double[] hiddenLayerOutputs = new double[hiddenLayerSize];
            double[] finalOutputs = new double[outputLayerSize];

            // Calculate hidden layer outputs
            for (int i = 0; i < hiddenLayerSize; i++)
            {
                double activation = 0.0;
                for (int j = 0; j < inputLayerSize; j++)
                {
                    activation += inputs[j] * weightsInputHidden[j, i];
                }
                hiddenLayerOutputs[i] = Sigmoid(activation);
            }

            // Calculate final outputs
            for (int i = 0; i < outputLayerSize; i++)
            {
                double activation = 0.0;
                for (int j = 0; j < hiddenLayerSize; j++)
                {
                    activation += hiddenLayerOutputs[j] * weightsHiddenOutput[j, i];
                }
                finalOutputs[i] = Sigmoid(activation);
            }

            return finalOutputs;
        }

        public void Train(double[][] inputs, double[][] targets, int epochs)
        {
            for (int epoch = 0; epoch < epochs; epoch++)
            {
                for (int i = 0; i < inputs.Length; i++)
                {
                    double[] input = inputs[i];
                    double[] target = targets[i];

                    // Feed forward
                    double[] hiddenLayerOutputs = new double[hiddenLayerSize];
                    double[] finalOutputs = new double[outputLayerSize];

                    for (int j = 0; j < hiddenLayerSize; j++)
                    {
                        double activation = 0.0;
                        for (int k = 0; k < inputLayerSize; k++)
                        {
                            activation += input[k] * weightsInputHidden[k, j];
                        }
                        hiddenLayerOutputs[j] = Sigmoid(activation);
                    }

                    for (int j = 0; j < outputLayerSize; j++)
                    {
                        double activation = 0.0;
                        for (int k = 0; k < hiddenLayerSize; k++)
                        {
                            activation += hiddenLayerOutputs[k] * weightsHiddenOutput[k, j];
                        }
                        finalOutputs[j] = Sigmoid(activation);
                    }

                    // Backpropagation
                    double[] outputErrors = new double[outputLayerSize];
                    for (int j = 0; j < outputLayerSize; j++)
                    {
                        outputErrors[j] = target[j] - finalOutputs[j];
                    }

                    double[] hiddenErrors = new double[hiddenLayerSize];
                    for (int j = 0; j < hiddenLayerSize; j++)
                    {
                        double error = 0.0;
                        for (int k = 0; k < outputLayerSize; k++)
                        {
                            error += outputErrors[k] * weightsHiddenOutput[j, k];
                        }
                        hiddenErrors[j] = error;
                    }

                    // Update weights hidden to output
                    for (int j = 0; j < hiddenLayerSize; j++)
                    {
                        for (int k = 0; k < outputLayerSize; k++)
                        {
                            weightsHiddenOutput[j, k] += learningRate * outputErrors[k] * hiddenLayerOutputs[j];
                        }
                    }

                    // Update weights input to hidden
                    for (int j = 0; j < inputLayerSize; j++)
                    {
                        for (int k = 0; k < hiddenLayerSize; k++)
                        {
                            weightsInputHidden[j, k] += learningRate * hiddenErrors[k] * input[j];
                        }
                    }
                }
            }
        }

        private double Sigmoid(double x)
        {
            return 1.0 / (1.0 + Math.Exp(-x));
        }
    }
}
