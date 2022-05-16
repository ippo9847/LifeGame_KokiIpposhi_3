using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
//using InoueLab;

namespace new_lifegame
{
    public partial class Form1 : Form
    {
        /// <summary>
        /// PictureBoxにはりつける画像
        /// </summary>
        Bitmap bmp;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public Form1()
        {
            InitializeComponent();
            pictureBox1.Size = new Size(250, 250);
            bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);

            //pictureBoxに，bmpをはりつける
            this.pictureBox1.Image = bmp;
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
        }

        const int H = 50;
        const int W = 50;
        public enum Status { None, Plant, Animal, Count };
        Status[] beforematrix = new Status[H * W];//50×50マスの盤面を作成
        Status[] aftermatrix = new Status[H * W];
        Random rnd = new Random();

        public void firstbefore()
        {
            //Fisher–Yates shuffle
            int[] fys = new int[H * W];
            for (int i = 0; i < fys.Length; i++) fys[i] = i;
            for (int i = fys.Length - 1; i >= 0; i--)
            {
                int j = rnd.Next(i + 1);
                (fys[i], fys[j]) = (fys[j], fys[i]);
            }
            for (int i = 0; i < fys.Length; i++)
            {
                int xx = fys[i] % W;
                int yy = fys[i] / W;
                Status s = Status.None;
                if (i < fys.Length * 0.1) s = Status.Animal;
                else if (i < fys.Length * 0.3) s = Status.Plant;
                beforematrix[yy * W + xx] = s;
            }
        }

        public void coloring()
        {
            for (int i = 0; i < H; i++)//色塗り
            {
                for (int j = 0; j < W; j++)
                {
                    Color c;
                    switch (beforematrix[i * W + j])
                    {
                        case Status.Animal:
                            c = Color.Red;
                            break;
                        case Status.Plant:
                            c = Color.Green;
                            break;
                        default:
                            c = Color.Black;
                            break;
                    }
                    int y0 = i * 5;
                    int x0 = j * 5;
                    for (int y = 0; y < 5; y++)
                    {
                        for (int x = 0; x < 5; x++) bmp.SetPixel(x0 + x, y0 + y, c);
                    }
                }
            }
            InterThreadRefresh(this.pictureBox1.Refresh);
        }

        public void decidecolor() //色判別システム
        {
            for (int j = 0; j < H; j++)
            {
                for (int k = 0; k < W; k++)
                {
                    var count = new int[(int)Status.Count];
                    for (int p = -1; p <= 1; p++)//y
                    {
                        for (int q = -1; q <= 1; q++)//x
                        {
                            if (p == 0 && q == 0) continue;
                            int y = (j + p + H) % H;
                            int x = (k + q + W) % W;
                            Status s = beforematrix[y * W + x];
                            count[(int)s]++;
                        }
                    }
                    Status ss = beforematrix[j * W + k];

                    //遷移条件
                    Boolean Animal_None = count[(int)Status.Animal] == 0 || count[(int)Status.Plant] <= (count[(int)Status.Animal] / 2);
                    Boolean Plant_None = (count[(int)Status.Plant] / 2) <= count[(int)Status.Animal] || (count[(int)Status.Animal] == 0 && count[(int)Status.Plant] >= 6) || (count[(int)Status.Animal] == 1 && count[(int)Status.Plant] == 7);
                    Boolean None_Plant = (count[(int)Status.Animal] == 0 && count[(int)Status.Plant] >= 2) || (count[(int)Status.Animal] == 1 && count[(int)Status.Plant] >= 5);
                    Boolean None_Animal = count[(int)Status.Animal] >= 2 && count[(int)Status.Plant] >= count[(int)Status.Animal];

                    switch (ss)
                    {
                        case Status.Animal:
                            if (Animal_None) aftermatrix[j * W + k] = Status.None;
                            break;
                        case Status.Plant:
                            if (Plant_None) aftermatrix[j * W + k] = Status.None;
                            break;
                        default:
                            if (None_Plant) aftermatrix[j * W + k] = Status.Plant;
                            else if (None_Animal) aftermatrix[j * W + k] = Status.Animal;
                            break;
                    }
                }
            }
        }

        public void nextbefore()
        {
            for (int i = 0; i < H; i++)
            {
                for (int j = 0; j < W; j++) beforematrix[i * W + j] = aftermatrix[i * W + j];
            }
        }


        private void mainProcess()
        {
            var sw = new System.Diagnostics.Stopwatch();
            sw.Start();
            //初期配置before生成
            firstbefore();
            int roop = int.Parse(textBox1.Text);
            for (int n = 0; n < roop; n++)
            {
                coloring();
                decidecolor();
                nextbefore();
                //System.Threading.Thread.Sleep(1000);
            }

            sw.Stop();
            textBox2.Text = Convert.ToString(sw.ElapsedMilliseconds);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            mainProcess();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click_1(object sender, EventArgs e)
        {

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click_1(object sender, EventArgs e)
        {

        }
    }
}

