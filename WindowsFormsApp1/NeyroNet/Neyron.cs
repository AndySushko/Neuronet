using System;
using WindowsFormsApp1.NeyroNet;
using static System.Math;

namespace WindowsFormsApp1.NeyroNet
{
    internal class Neuron
    {
        private NeuronType _type;// тип нейрона
        private double[] _weights;//его веса
        private double[] _inputs;//его входы
        private double _output;//его выход
        private double _derivative;

        //константы для функции активации
        private double a = 0.01;
        public double[] Weights { get => _weights; set => _weights = value; }
        public double[] Inputs
        {
            get { return _inputs; }
            set { _inputs = value; }
        }
        public double Output { get => _output; }
        public double Derivative { get => _derivative; }

        //конструктор
        public Neuron(double[] weights, NeuronType type)
        {
            _type = type;
            _weights = weights;
        }
        public void Activator(double[] i, double[] w) // Нелинейные преобразования
        {
            double sum = w[0]; // Аффинное преобразование через смещение (вес нулевой)
            for (int m = 0; m < i.Length; m++)
                sum += i[m] * w[m + 1]; // Линейные преобразования

            switch (_type)
            {
                case NeuronType.Hidden:
                    _output = LeakyReLu(sum);
                    _derivative = LeakyReLu_Derivativator(sum);
                    break;

                case NeuronType.Output:
                    _output = Exp(sum); // Если требуется оставить экспоненциальную функцию
                    break;
            }
        }

        private double LeakyReLu(double sum) => (sum >= 0) ? sum : a * sum;

        private double LeakyReLu_Derivativator(double sum) => (sum >= 0) ? 1 : a; // ?

    }
}
