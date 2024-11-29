using System;
using System.IO;
using System.Windows.Forms;
using WindowsFormsApp1.NeyroNet;

namespace WindowsFormsApp1.NeyroNet
{
    abstract class Layer
    {
        protected string name_Layer;        // Наименование слоя
        string pathDirWeights;              // Путь к директории синаптических весов
        string pathFileWeights;             // Путь к файлу синаптических весов
        protected int numofneurons;         // Число нейронов текущего слоя
        protected int numofprevneurons;     // Число нейронов предыдущего слоя
        protected const double learninggrate = 0.01d;       // Скорость обучения
        protected const double momentum = 0.05d;            // Момент инерции
        protected double[,] lastdeltaweight;               // Веса предыдущей итерации
        Neuron[] _neurons;                                  // Массив нейронов текущего слоя

        // Свойства
        public Neuron[] Neurons { get => _neurons; set => _neurons = value; }
        public double[] Data
        {
            set
            {
                for (int i = 0; i < Neurons.Length; i++)
                {
                    Neurons[i].Inputs = value;
                    Neurons[i].Activator(Neurons[i].Inputs, Neurons[i].Weights);
                }
            }
        }

        // Конструктор
        protected Layer(int non, int nopn, NeuronType nt, string nm_layer)
        {
            int i, j;                             //счетчик циклов 
            numofneurons = non;                   //количество нейронов текущего слоя
            numofprevneurons = nopn;              //количество нейронов предыдущего слоя
            Neurons = new Neuron[non];            //определение массива нейронов
            name_Layer = nm_layer;                //наименование слоя, которое используется для связи

            pathDirWeights = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "memory\\");
            pathFileWeights = Path.Combine(pathDirWeights, name_Layer + "_memory.csv");

            double[,] Weights;//временный массив синаптических весов 

            if (File.Exists(pathFileWeights))
                Weights = WeightInitialize(MemoryMode.GET, pathFileWeights);
            else
            {
                Directory.CreateDirectory(pathDirWeights);
                Weights = WeightInitialize(MemoryMode.INIT, pathFileWeights);
            }

            lastdeltaweight = new double[non, nopn + 1];

            for (i = 0; i < non; i++)
            {
                double[] tmp_weights = new double[nopn + 1];
                for (j = 0; j < nopn + 1; j++)
                {
                    tmp_weights[j] = Weights[i, j];
                }
                Neurons[i] = new Neuron(tmp_weights, nt);
            }
        }

        public double[,] WeightInitialize(MemoryMode _type, string pathFileWeights)
        {
            char delim = ';'; // Разделитель
            string[] tmpStrWeights;
            double[,] weights = new double[numofneurons, numofprevneurons + 1];

            switch (_type)
            {
                case MemoryMode.GET:
                    // Считываем веса из файла
                    tmpStrWeights = File.ReadAllLines(pathFileWeights); // Считывание строк текстового массива весов
                    string[] memory_element;
                    for (int i = 0; i < numofneurons; i++)
                    {
                        memory_element = tmpStrWeights[i].Split(delim); // Разбиваем строку на элемент взависимости от символов в массиве
                        for (int j = 0; j < numofprevneurons + 1; j++)
                        {
                            weights[i, j] = double.Parse(memory_element[j].Replace(',', '.'),
                                System.Globalization.CultureInfo.InvariantCulture); // Преобразование строкового значения числа в само число двойной точности
                        }
                    }
                    break;

                case MemoryMode.SET:
                    // Меняем веса и сохраняем их в файл
                    using (StreamWriter sw = new StreamWriter(pathFileWeights))
                    {
                        for (int i = 0; i < numofneurons; i++)
                        {
                            for (int j = 0; j < numofprevneurons + 1; j++)
                            {
                                weights[i, j] = Neurons[i].Weights[j];
                                sw.Write(weights[i, j].ToString(System.Globalization.CultureInfo.InvariantCulture));
                                if (j < numofprevneurons)
                                {
                                    sw.Write(delim);
                                }
                            }
                            sw.WriteLine();
                        }
                    }
                    break;

                case MemoryMode.INIT:
                    // Инициализируем начальные веса случайными значениями и сохраняем их в файл
                    Random rand = new Random();
                    using (StreamWriter sw = new StreamWriter(pathFileWeights))
                    {
                        for (int i = 0; i < numofneurons; i++)
                        {
                            for (int j = 0; j < numofprevneurons + 1; j++)
                            {
                                double weight = rand.NextDouble() * 2 - 1; // Генерация случайного числа в диапазоне [-1, 1]
                                weights[i, j] = weight;
                                sw.Write(weight.ToString(System.Globalization.CultureInfo.InvariantCulture));
                                if (j < numofprevneurons)
                                {
                                    sw.Write(delim);
                                }
                            }
                            sw.WriteLine();
                        }
                    }
                    break;
            }
            return weights;
        }
        abstract public void Recognize(Network net, Layer nextLayer); // Для прямых проходов
        abstract public double[] BackwardPass(double[] stuff); // Для обратных проходов


    }
}
