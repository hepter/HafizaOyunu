using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace HafızaOyunu
{

    public partial class Form1 : Form
    {
        //DoulbeBuffered 
        protected override CreateParams CreateParams
        {
            get
            {
                var cp = base.CreateParams;
                cp.ExStyle |= 0x02000000;
                return cp;
            }
        }




        public Form1()
        {
            InitializeComponent();
        }


        oyun oyn;
        void YeniOyun()
        {
            oyn = new oyun();
            ArrayList sıra = new ArrayList();

            ResimAdaptör(oyn.Resimler, oyn.ResimSıra);
            label8.Text = "20";
            label6.Text = "0";
            label10.Text = "%100";
            label11.Text = "%100";
            label1.Text = "0";
            label2.Text = "0";
            isabet1 = 0;
            isabet2 = 0;
            button3.Enabled = false;
        }

        bool oyunDurum;
        string uyarı1 = "Oyun hala devam ediyor.\nYeniden Başlamak istediğinize emin misiniz?";

        private void button3_Click(object sender, EventArgs e)
        {

            if (!oyunDurum)
            {
                YeniOyun();
                button3.Text = "Yeniden Başlat";
                oyunDurum = true;
            }
            else
            {
                if (MessageBox.Show(uyarı1, "Uyarı", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    YeniOyun();
                }
            }

        }


        void ResimAdaptör(Bitmap[] resim, ArrayList Sıra)
        {
            Thread gecis = new Thread(a =>
            {



                Bitmap[] resimler = resim;
                for (int i = 0; i < 40; i++)
                {

                    PictureBox box = (PictureBox)tableLayoutPanel2.Controls["pictureBox" + (i + 1)];
                    try
                    {
                        box.Image = resimler[(int)Sıra[i]];
                    }
                    catch (Exception) { }

                }
                backgroundWorker1.RunWorkerAsync();
                Thread.Sleep(5000);

                for (int i = 0; i < 40; i++)
                {
                    PictureBox box = (PictureBox)tableLayoutPanel2.Controls[i];


                    box.Image = Properties.Resources.mark;


                }

            });

            gecis.Start();
        }


        void bitir(int kazanan)
        {
            int game = kazanan;
            string son = "Tebrikler oyunu {0}. Oyuncu kazandı\nYeniden Başlamak için Evet,Çıkış icin Hayır butonunun seçin!";

            DialogResult karar = MessageBox.Show(string.Format(son, game), "Uyarı", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (karar == DialogResult.Yes)
            {

                YeniOyun();

            }
            else if (karar == DialogResult.No)
            {
                Application.Exit();
            }


        }

        bool sıra = true;
        Color p1 = Color.Blue;
        Color p2 = Color.Red;
        float isabet1 = 0;
        float isabet2 = 0;

        Button yansönbtn2;
        void oyuncuGecis(bool bildimi = false)
        {

            if (bildimi)
            {
                if (sıra)
                {
                    int no = int.Parse(label1.Text) + 1;
                    label1.Text = (no).ToString();
                    if (no == 11)
                    {
                        bitir(1);
                        return;
                    }
                }
                else
                {
                    int no = int.Parse(label2.Text) + 1;
                    label2.Text = (no).ToString();
                    if (no == 11)
                    {
                        bitir(2);
                        return;
                    }

                }



                yansönbtn2 = (sıra) ? button2 : button1;
                Thread yansön = new Thread(a =>
                {

                    for (int i = 0; i < 3; i++)
                    {

                        yansönbtn2.Invoke((MethodInvoker)delegate { yansönbtn2.BackColor = Color.Green; });

                        Thread.Sleep(150);
                        yansönbtn2.Invoke((MethodInvoker)delegate { yansönbtn2.BackColor = Color.Yellow; });
                        Thread.Sleep(150);

                    }

                });
                yansön.Start();

            }

            else
            {
                sıra = !sıra;
            }


            if (sıra)
            {


                label3.ForeColor = p1;
                label5.ForeColor = Color.SlateGray;
                button2.BackColor = Color.Yellow;
                button1.BackColor = Color.Silver;
                isabet1++;
            }
            else
            {
                label3.ForeColor = Color.SlateGray;
                label5.ForeColor = p2;
                button1.BackColor = Color.Yellow;
                button2.BackColor = Color.Silver;
                isabet2++;
            }



            int hamlesayi = int.Parse(label6.Text);
            hamlesayi++;
            label6.Text = (hamlesayi).ToString();

            float p1dogru = float.Parse(label1.Text);
            float p2dogru = float.Parse(label2.Text);


            float oran1 = (p1dogru * 100 / isabet1); //((hamlesayi - (p1dogru + p2dogru)) / 2 )*100/( (hamlesayi - (p1dogru + p2dogru)) / 2+ p1dogru);
            float oran2 = (p2dogru * 100 / isabet2);

            if (float.IsInfinity(oran1)) oran1 = 0;
            if (float.IsInfinity(oran2)) oran2 = 0;

            label10.Text = "%" + (oran1).ToString("0.00");
            label11.Text = "%" + (oran2).ToString("0.00");
            if (label10.Text == "%NaN") label10.Text = "%0.00";
            if (label11.Text == "%NaN") label11.Text = "%0.00";
        }


        int aktifBox = 0;
        int eskiaktifBox = 0;
        private void pictureBoxTümClickler(object sender, EventArgs e)
        {
            if (!backgroundWorker1.IsBusy && oyunDurum)
            {

                aktifBox = int.Parse(((PictureBox)sender).Name.Replace("pictureBox", ""));
                ((PictureBox)sender).Image = oyn.Resimler[(int)oyn.ResimSıra[aktifBox - 1]];
                backgroundWorker1.RunWorkerAsync();

            }
            else if (aktifBox != 0 && progressBar1.Value != 1)
            {

                eskiaktifBox = aktifBox;
                aktifBox = int.Parse(((PictureBox)sender).Name.Replace("pictureBox", ""));

                if (aktifBox == eskiaktifBox)
                {
                    return;
                }


                ((PictureBox)sender).Image = oyn.Resimler[(int)oyn.ResimSıra[aktifBox - 1]];
                if (((int)oyn.ResimSıra[aktifBox - 1] == (int)oyn.ResimSıra[eskiaktifBox - 1]))
                {
                    backgroundWorker1.CancelAsync();
                    progressBar1.Value = 1;
                    label8.Text = (int.Parse(label8.Text) - 1).ToString();
                    bekle(100);
                    oyuncuGecis(true);
                    progressBar1.Value = 0;
                }
                else
                {
                    backgroundWorker1.CancelAsync();
                    progressBar1.Value = 1;
                    yansönbtn2 = (sıra) ? button2 : button1;
                    Thread yansön = new Thread(a =>
                    {

                        for (int i = 0; i < 5; i++)
                        {

                            yansönbtn2.Invoke((MethodInvoker)delegate { yansönbtn2.BackColor = Color.Red; });

                            Thread.Sleep(50);
                            yansönbtn2.Invoke((MethodInvoker)delegate { yansönbtn2.BackColor = Color.Yellow; });
                            Thread.Sleep(50);

                        }

                        yansönbtn2.Invoke((MethodInvoker)delegate { yansönbtn2.BackColor = Color.Silver; });
                    });


                    yansön.Start();
                    bekle(500);
                    progressBar1.Value = 0;
                    ((PictureBox)sender).Image = Properties.Resources.mark;
                    ((PictureBox)tableLayoutPanel2.Controls["pictureBox" + eskiaktifBox]).Image = Properties.Resources.mark;
                    oyuncuGecis(false);

                }



            }
        }
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
          
            Stopwatch st = Stopwatch.StartNew();
            do
            {
                long ms = st.ElapsedMilliseconds;
                if (((BackgroundWorker)sender).CancellationPending)
                {
                    break;
                }

                if (ms> 5000)
                {
                    st.Stop();
                    break;
                }            
                backgroundWorker1.ReportProgress((int)(5000-ms)/5);
                Thread.Sleep(1);
            } while (true);
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {


            progressBar1.Invoke((MethodInvoker)delegate { progressBar1.Value = e.ProgressPercentage; });
            label12.Invoke((MethodInvoker)delegate { label12.Text = ((e.ProgressPercentage) / 200 + 1).ToString(); });


        }

        Stopwatch bekletmr = null;
        void bekle(int ms)
        {
            do
            {
                if (bekletmr == null || !bekletmr.IsRunning)
                {
                    bekletmr = Stopwatch.StartNew();


                }
                if (bekletmr.ElapsedMilliseconds > ms)
                {
                    bekletmr.Stop();
                    break;

                }
                Application.DoEvents();
                Thread.Sleep(10);
            } while (true);
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (aktifBox != 0 && progressBar1.Value == 0)
            {
                ((PictureBox)tableLayoutPanel2.Controls["pictureBox" + aktifBox]).Image = Properties.Resources.mark;
                oyuncuGecis();
            }
            bekle(100);
            label12.Invoke((MethodInvoker)delegate { label12.Text = "0"; });
            button3.Invoke((MethodInvoker)delegate { button3.Enabled = true; });

        }



    }
}
