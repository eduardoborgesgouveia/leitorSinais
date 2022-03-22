using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using SignalProcessing;

namespace Aula_FFT
{
    public partial class Form1 : Form
    {
        FFT2 fft;
        //sinal senoid
        double[] signal;
        double amplitude = 2;//V
        double freq = 30; //Hz
        double sampFreq = 1000;//Hz

        public Series Sinal, FFT;


        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void bt_FFT_Click(object sender, EventArgs e)
        {
            fft = new FFT2();
            double dt = 1.0 / sampFreq;
            double t = 0;
            int tf = 5;
            //Tamanho do vetor
            signal = new double[tf * (int)sampFreq];


            
            chart1.Series.Clear();
            Sinal = new Series("SeriesSinal");
            FFT = new Series("SeriesFFT");
            chart1.Series.Add(Sinal);
            chart1.Series.Add(FFT);
            Sinal.ChartType = SeriesChartType.FastLine;
            FFT.ChartType = SeriesChartType.FastLine;
            chart1.ChartAreas["ChartArea1"].AxisX.Maximum = tf;

            chart1.Series["SeriesSinal"].ChartArea = "ChartArea1";
            //Simular até tf
            for (int i = 0; i <signal.Length;i++)
            {
                signal[i] = amplitude * Math.Sin(2.0 * Math.PI * freq*t);
                t += dt;
                chart1.Series["SeriesSinal"].Points.AddXY(t,signal[i]);
            }

            //Calculando a FFT a partir da Classe FFT2
            //Ela retorna a parte real e imaginária
            
            uint fftN = 12; //número de bits
            int fftWindow = (int)Math.Pow(2, fftN); //2^n onde n é o número de bits nos dá a janela da FFT, porém o resultado de FFT's é metade desses valores.
            double[] realFFT = new double[fftWindow];
            double[] imFFT = new double[fftWindow];
            for (int i = 0; i < fftWindow; i++)
            {
                realFFT[i] = signal[i];
                imFFT[i] = 0;
            }
            fft.init(fftN);
            fft.run(realFFT, imFFT);
            
            //Definindo que a série da FFT será colocada no ChartArea2
            chart1.Series["SeriesFFT"].ChartArea = "ChartArea2";
            chart1.ChartAreas["ChartArea2"].AxisX.Maximum = 150;
            chart1.ChartAreas["ChartArea2"].AxisX.Minimum = 0;
            //Calculando o espectro de frequência de acordo com o retorno da Classe que calcula a FFT
            double[] pow = new double[fftWindow / 2];
            double[] xFFT = new double[fftWindow / 2];
            for (int i = 0; i < fftWindow/2; i++)
            {
                pow[i] = Math.Sqrt(realFFT[i] * realFFT[i] + imFFT[i] * imFFT[i]);
                xFFT[i] = (sampFreq * i)/fftWindow;
                chart1.Series["SeriesFFT"].Points.AddXY(xFFT[i],pow[i]);
            }
        }
    }
}
