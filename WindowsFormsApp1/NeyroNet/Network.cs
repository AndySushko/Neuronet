using System;
using WindowsFormsApp1.NeyroNet;

namespace WindowsFormsApp1.NeyroNet
{
    class Network
    {
        private InputLayer input_layer = null;
        private HiddenLayer hidden_layer1 = new HiddenLayer(70, 15, NeuronType.Hidden, nameof(hidden_layer1));
        private HiddenLayer hidden_layer2 = new HiddenLayer(31, 70, NeuronType.Hidden, nameof(hidden_layer2));
        private OutputLayer output_layer = new OutputLayer(10, 31, NeuronType.Output, nameof(output_layer));
        //массив для хранения выхода сети
        public double[] fact = new double[10];


        //среднее значение энергии ошибки эпохи обучения
        private double[] e_error_avr;

        public double[] E_error_avr { get => e_error_avr; set => e_error_avr = value; }
        //public event EventHandler<EventArgs> UpDateChart;

        //конструктор
        public Network() { }

        public void Train(Network net)
        {
            int epoches = 90; // количество эпох обучения
            net.input_layer = new InputLayer(NetworkMode.Train);
            double tmpSumError; // временная переменная  суммы ошибок
            double[] errors;    // вектор (массив) сигнала ошибки выходного слоя
            double[] temp_gsums1; // вектор градиента 1-го скрытого слоя
            double[] temp_gsums2; // вектор градиента 2-го скрытого слоя

            e_error_avr = new double[epoches];
            for (int k = 0; k < epoches; k++)
            {
                e_error_avr[k] = 0;
                for (int i = 0; i < net.input_layer.Trainset.GetLength(0); i++)
                {
                    // Прямой проход
                    double[] inputData = new double[15];

                    for (int j = 1; j < net.input_layer.Trainset.GetLength(1); j++)
                        inputData[j - 1] = net.input_layer.Trainset[i, j];


                    int desiredOutput = (int)net.input_layer.Trainset[i, 0]; // извлекаем метку из первого столбца
                    ForwardPass(net, inputData); // передаем входные данные в метод прямого прохода

                    tmpSumError = 0; // вычисление сумм ошибок
                    errors = new double[net.fact.Length];

                    for (int x = 0; x < errors.Length; x++) // цикл перебора выходных нейронов
                    {
                        errors[x] = (x == desiredOutput) ? -(net.fact[x] - 1.0d) : -net.fact[x]; // вычисляем ошибку
                        tmpSumError += errors[x] * errors[x] / 2;
                    }
                    e_error_avr[k] += tmpSumError / errors.Length; // суммарное значение энергии ошибки

                    // Обратный проход и коррекция весов
                    temp_gsums2 = net.output_layer.BackwardPass(errors);
                    temp_gsums1 = net.hidden_layer2.BackwardPass(temp_gsums2);
                    net.hidden_layer1.BackwardPass(temp_gsums1);
                }
                e_error_avr[k] /= net.input_layer.Trainset.GetLength(0); // усреднение ошибки по всем примерам
            }
            net.input_layer = null; // обнуление входного слоя (уборка)

            net.hidden_layer1.WeightInitialize(MemoryMode.SET, nameof(hidden_layer1) + "_memory.csv");
            net.hidden_layer2.WeightInitialize(MemoryMode.SET, nameof(hidden_layer2) + "_memory.csv");
            net.output_layer.WeightInitialize(MemoryMode.SET, nameof(output_layer) + "_memory.csv");
        }

        // Прямой проход нейросети
        public void ForwardPass(Network net, double[] netInput)
        {
            net.hidden_layer1.Data = netInput;
            net.hidden_layer1.Recognize(null, net.hidden_layer2);
            net.hidden_layer2.Recognize(null, net.output_layer);
            net.output_layer.Recognize(net, null);
        }

    }
}
