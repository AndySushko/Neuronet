using System;
using System.IO;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        // массив "пикселей"
        private double[] inputPixels = new double[15] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

        NeyroNet.Network network = new NeyroNet.Network();  // нейросеть

        // Конструктор
        public Form1()
        {
            InitializeComponent();
        }

        // изменение цвета кнопок
        private void ChangeState(Button btn, int index)
        {
            if (btn.BackColor == Color.Black)
            {
                btn.BackColor = Color.White;
                inputPixels[index] = 0;
            }
            else
            {
                btn.BackColor = Color.Black;
                inputPixels[index] = 1;
            }
        }

        // клик на кнопки
        private void button1_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            ChangeState(btn, btn.TabIndex);
        }

        // сохранение обучающей выборки
        private void SaveTrain(decimal vale, double[] input)
        {
            string pathDir;
            string nameFileTrain;
            pathDir = AppDomain.CurrentDomain.BaseDirectory;
            nameFileTrain = pathDir + "train.txt";
            string[] StrArray = new string[1];
            StrArray[0] = vale.ToString();
            for (int i = 0; i < input.Length; i++)
            {
                StrArray[0] += " " + input[i].ToString();
            }

            File.AppendAllLines(nameFileTrain, StrArray);
        }

        // клик на кнопку сохранения выборки
        private void SaveTrainSample_Click(object sender, EventArgs e)
        {
            SaveTrain(numericUpDown1.Value, inputPixels);
        }

        private void buttonRecognize_Click(object sender, EventArgs e)
        {
            network.ForwardPass(network, inputPixels);
            labelAnswer.Text = network.fact.ToList().IndexOf(network.fact.Max()).ToString();
            labelProbability.Text = (100 * network.fact.Max()).ToString("0.00") + "%";
        }

        private void buttonTrain_Click(object sender, EventArgs e)
        {
            network.Train(network);
            for (int i = 0; i < network.E_error_avr.Length; i++)
            {
                chart_Eavr.Series[0].Points.AddY(network.E_error_avr[i]);
            }
            MessageBox.Show("Обучение успешно завершено.", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void SaveTest(decimal vale, double[] input)
        {
            string pathDir;
            string nameFileTrain;
            pathDir = AppDomain.CurrentDomain.BaseDirectory;
            nameFileTrain = pathDir + "test.txt";
            string[] StrArray = new string[1];
            StrArray[0] = vale.ToString();
            for (int i = 0; i < input.Length; i++)
            {
                StrArray[0] += " " + input[i].ToString();
            }

            File.AppendAllLines(nameFileTrain, StrArray);
        }

        private void button16_Click(object sender, EventArgs e)
        {
            SaveTest(numericUpDown1.Value, inputPixels);
        }

        private void button_Test_Click(object sender, EventArgs e)
        {
            network.Test(network);
            for (int i = 0; i<network.E_error_avr.Length; i++)
            {
                chart_Eavr.Series[0].Points.AddY(network.E_error_avr[i]);
            }
            MessageBox.Show("Тестирование успешно завершено.", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
