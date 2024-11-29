using System;
using System.IO;
using WindowsFormsApp1.NeyroNet;

namespace WindowsFormsApp1.NeyroNet
{
    class InputLayer
    {
        private Random random = new Random();
        private double[,] _trainset = new double[100, 16];
        private double[,] _testset = new double[10, 16];
        private string path = AppDomain.CurrentDomain.BaseDirectory;
        string[] tmpArrStr;
        string[] tmpStr;
        public double[,] Trainset { get => _trainset; }
        public double[,] Testset { get => _testset; }

        public InputLayer(NetworkMode nm)
        {
            switch (nm)
            {
                case NetworkMode.Train:
                    // код считывания обучающей выборки из файла

                    tmpArrStr = File.ReadAllLines(path + "train.txt");
                    _trainset = new double[tmpArrStr.Length, 16];
                    for (int i = 0; i < tmpArrStr.Length; i++)
                    {
                        tmpStr = tmpArrStr[i].Split(' ');
                        for (int j = 0; j < 16; j++)
                        {
                            _trainset[i, j] = double.Parse(tmpStr[j]);
                        }
                    }
                    ShuffleDataset(_trainset);
                    break;

                case NetworkMode.Test:
                    // код считывания тестовой выборки из файла
                    tmpArrStr = File.ReadAllLines(path + "test.txt");
                    _trainset = new double[tmpArrStr.Length, 16];
                    for (int i = 0; i < tmpArrStr.Length; i++)
                    {
                        tmpStr = tmpArrStr[i].Split(' ');
                        for (int j = 0; j < 16; j++)
                        {
                            _testset[i, j] = double.Parse(tmpStr[j]);
                        }
                    }
                    ShuffleDataset(_testset);
                    break;

                case NetworkMode.Recogn:

                    break;
            }
        }

        private void ShuffleDataset(double[,] arr)
        {
            int j;
            Random random = new Random();
            double[] temp = new double[arr.GetLength(1)];
            for (int n = arr.GetLength(0) - 1; n >= 1; n--)
            {
                j = random.Next(n + 1);
                for (int i = 0; i < arr.GetLength(1); i++)
                {
                    temp[i] = arr[n, i];
                }
                for (int i = 0; i < arr.GetLength(1); i++)
                {
                    arr[n, i] = arr[j, i];
                    arr[j, i] = temp[i];
                }
            }
        }
    }
}