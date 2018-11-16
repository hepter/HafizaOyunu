using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

namespace HafızaOyunu
{
    class oyun
    {

        string AppDir = AppDomain.CurrentDomain.BaseDirectory;
        public oyun()
        {
            resimler = null;

        }

        Bitmap[] resimler;

        public Bitmap[] Resimler
        {
            get
            {
                if (resimler == null)
                {
                    resimler = new Bitmap[20];
                    resimYükle();
                    resimKarıştır();
                }
                return resimler;
            }
        }

        public ArrayList ResimSıra { get => resimSıra; set => resimSıra = value; }

        public void resimYükle(string yol = "\\fruits")
        {
            string[] list = Directory.GetFiles(AppDir + yol);
            for (int i = 0; i < 20; i++)
            {
                Image resim = Image.FromFile(list[i]);
                Bitmap izlenenBolge = new Bitmap(resim, resim.Size);
                Rectangle aaa = new Rectangle(new Point(0, 0), resim.Size);
                izlenenBolge = izlenenBolge.Clone(aaa, izlenenBolge.PixelFormat);


                resimler[i] = izlenenBolge;

                resim.Dispose();
                //resimler[i] = Image.FromFile( list[i]);
            }
        }

        private ArrayList resimSıra = new ArrayList();



        public void resimKarıştır()
        {

            for (int i = 0; i < 20; i++)
            {
                resimSıra.Add(i);
                resimSıra.Add(i);
            }
            ArrayList yeniResimSıra = new ArrayList();
            Random r = new Random();
            int rnd = 0;

            while (resimSıra.Count > 0)
            {
                rnd = r.Next(0, resimSıra.Count);
                yeniResimSıra.Add(resimSıra[rnd]);
                resimSıra.RemoveAt(rnd);
            }

            while (yeniResimSıra.Count > 0)
            {
                rnd = r.Next(0, yeniResimSıra.Count);
                resimSıra.Add(yeniResimSıra[rnd]);
                yeniResimSıra.RemoveAt(rnd);
            }

          


        }

    }
}
