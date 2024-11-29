using WindowsFormsApp1.NeyroNet;

namespace WindowsFormsApp1.NeyroNet
{
    class HiddenLayer : Layer
    {
        //конструктор
        public HiddenLayer(int non, int nopn, NeuronType nt, string type) : base(non, nopn, nt, type) { }
        public override void Recognize(Network net, Layer nextlayer)
        {
            double[] hidden_out = new double[Neurons.Length];
            for (int i = 0; i<Neurons.Length; i++)
            {
                hidden_out[i]=Neurons[i].Output;
            }
            nextlayer.Data=hidden_out;
        }
        public override double[] BackwardPass(double[] gr_sums)//gr_sum локальный градиент поверхности ошибок
        {
            double[] gr_sum = new double[numofprevneurons];
            for (int j = 0; j < numofprevneurons; j++)
            {
                double sum = 0;
                for (int k = 0; k < numofneurons; k++)
                    sum += Neurons[k].Weights[j] * Neurons[k].Derivative *gr_sums[k];//через градиентные суммы
                gr_sum[j] = sum;

            }
            for (int i = 0; i < numofneurons; i++)
                for (int n = 0; n < numofprevneurons + 1; n++)
                {
                    double deltaw;
                    if (n == 0)
                        deltaw = momentum * lastdeltaweight[i, 0] + learninggrate * Neurons[i].Derivative * gr_sums[i];
                    else
                        deltaw = momentum * lastdeltaweight[i, n] + learninggrate * Neurons[i].Inputs[n - 1] * Neurons[i].Derivative * gr_sums[i];
                    lastdeltaweight[i, n] = deltaw;
                    Neurons[i].Weights[n] += deltaw;//коррекция веса
                }
            return gr_sum;
        }
    }
}
