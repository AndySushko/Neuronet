using WindowsFormsApp1.NeyroNet;

namespace WindowsFormsApp1.NeyroNet
{
    class OutputLayer : Layer
    {
        //конструктор
        public OutputLayer(int non, int nopn, NeuronType nt, string type) : base(non, nopn, nt, type) { }

        //метод прямого прохода
        public override void Recognize(Network net, Layer nextLayer)
        {
            //реализаця функции SoftMax
            double e_sum = 0;
            for (int i = 0; i < Neurons.Length; i++)
                e_sum += Neurons[i].Output;
            for (int i = 0; i < Neurons.Length; i++)
                net.fact[i] = Neurons[i].Output / e_sum;
        }
        public override double[] BackwardPass(double[] errors)
        {
            double[] gr_sum = new double[numofprevneurons + 1]; //локальный градиент функции активациии
            for (int j = 0; j < numofprevneurons + 1; j++)
            {
                double sum = 0;
                for (int k = 0; k < numofneurons; k++)
                    sum += Neurons[k].Weights[j] * errors[k];
                gr_sum[j] = sum;
            }

            for (int i = 0; i < numofneurons; i++)
            {
                for (int n = 0; n < numofprevneurons + 1; n++)
                {
                    double deltaw = 0;
                    if (n == 0)
                    {
                        deltaw = momentum * lastdeltaweight[i, 0] + learninggrate * errors[i];
                    }
                    else
                    {
                        deltaw = momentum * lastdeltaweight[i, n] + learninggrate * Neurons[i].Inputs[n - 1] * errors[i];
                    }
                    lastdeltaweight[i, n] = deltaw;
                    Neurons[i].Weights[n] += deltaw; //коррекция весов нейронов
                }
            }
            // код обучения нейронной сети
            return gr_sum;
        }
    }
}
