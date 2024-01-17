namespace IZYNG7_SnakeGameProject
{
    public partial class Form1 : Form
    {
        #region Deklaráció
        int lépésszám;
        int eltelt_ido;
        int fej_x = 90;
        int fej_y = 90;
        int irány_x=1;
        int irány_y=0;
        int mereg_x;
        int mereg_y;
        int kaja_x;
        int kaja_y;
        int kigyoHossz = 5;
        Random r = new Random();
        public Label l = new Label();
        public Label l2 = new Label();
        List<KígyóElem> kígyó = new List<KígyóElem>();
        List<Kaja> kaja = new List<Kaja>();
        List<Mereg> mereg = new List<Mereg>();
        #endregion

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            timer1.Tick += Timer1_Tick;
            timer2.Tick += Timer2_Tick;
            KeyDown += Form1_KeyDown;
            l.Width = 100;
            l.Height = 50;
            l2.Width = 100;
            l2.Height = 50;
            l2.Left = 100;
        }

        #region BillentyûLenyomás
        private void Form1_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode==Keys.Up)
            {
                irány_x = 0;
                irány_y = -1;
            }
            if (e.KeyCode == Keys.Down)
            {
                irány_x = 0;
                irány_y = 1;
            }
            if (e.KeyCode == Keys.Left)
            {
                irány_x = -1;
                irány_y = 0;
            }
            if (e.KeyCode == Keys.Right)
            {
                irány_x = 1;
                irány_y = 0;
            }

        }
        #endregion

        #region Timer1
        private void Timer1_Tick(object sender, EventArgs e)
        {
            lépésszám++;
            l.Text = $"Lépésszámok: {lépésszám}";
            Controls.Add(l);
            fej_x += irány_x * KígyóElem.KigyoMeret;
            fej_y += irány_y * KígyóElem.KigyoMeret;


            #region Kaja
            //Kaja megjelenés
            if (lépésszám % 7 == 0)
            {
               Kaja kaj = new Kaja();
               kaja_x = r.Next(Width / KígyóElem.KigyoMeret) * KígyóElem.KigyoMeret;
               kaja_y = r.Next(Height / KígyóElem.KigyoMeret) * KígyóElem.KigyoMeret;
               if (kaja_x <= 170)
               {
                  kaja_x += 180;
               }
               else if (kaja_y <= 130)
               {
                  kaja_y += 120;
               }
               kaj.Left = kaja_x;
               kaj.Top = kaja_y;
               kaja.Add(kaj);
               Controls.Add(kaj);
            }

            //Kígyó és az étel találkozása
            foreach (object item in Controls)
            {
                if (item is Kaja)
                {
                    Kaja food = (Kaja)item;
                    if (food.Top == fej_y && food.Left == fej_x)
                    {
                        food.Visible = false;
                        kigyoHossz++;
                    }
                }
            }
            #endregion

            #region Méreg
            //Méreg megjelenése
            if (lépésszám % 11 == 0)
            {
                Mereg mer = new Mereg();
                mereg_x = r.Next(Width / KígyóElem.KigyoMeret) * KígyóElem.KigyoMeret;
                mereg_y = r.Next(Height / KígyóElem.KigyoMeret) * KígyóElem.KigyoMeret;
                if (mereg_x <= 170)
                {
                    mereg_x += 180;
                }
                else if (kaja_y <= 130)
                {
                    mereg_y += 120;
                }
                mer.Left = mereg_x;
                mer.Top = mereg_y;
                mereg.Add(mer);
                Controls.Add(mer);
            }
            //Kígyó és a méreg találkozása
            foreach (object item in Controls)
            {
                if (item is Mereg)
                {
                    Mereg mereg = (Mereg)item;
                    if (mereg.Top == fej_y && mereg.Left == fej_x)
                    {
                        mereg.Visible = false;
                        timer1.Enabled = false;
                        MessageBox.Show("Vesztettél!");
                        //Application.Restart();
                        Environment.Exit(0);
                    }
                }
            }
            #endregion

            #region KígyóObjektum
            //Ütközésvizsgálat
            foreach (object item in Controls)
            {
                if (item is KígyóElem)
                {
                    KígyóElem k = (KígyóElem)item;
                    if (k.Top == fej_y && k.Left == fej_x)
                    {
                        timer1.Enabled = false;
                        timer2.Enabled = false;
                        MessageBox.Show("Vesztettél!");
                        //Application.Restart();
                        Environment.Exit(0);
                    }
                }
            }

            //Kígyóelem létrehozása
            KígyóElem ke = new KígyóElem();
            kígyó.Add(ke);
            ke.Top = fej_y;
            ke.Left = fej_x;
            Controls.Add(ke);

            if (kígyó.Count > kigyoHossz)
            {
                KígyóElem levágandó = kígyó[0];
                kígyó.RemoveAt(0);
                Controls.Remove(levágandó);
            }
            
            //Színváltás
            if (lépésszám % 2 == 0) ke.BackColor = Color.LightGreen;
            #endregion
        }
        #endregion

        #region Timer2
        private void Timer2_Tick(object? sender, EventArgs e)
        {
            eltelt_ido += timer2.Interval;
            l2.Text = $"Eltelt idõ(s): {(eltelt_ido / 1000).ToString()}";
            Controls.Add(l2);

            //Idõzítõ
            timer1.Start();
            if (lépésszám % 3 == 0 && timer1.Interval > 30)
            {
                timer1.Interval -= 30;
            }

            //Hibakezelés (Nem lehet 0 vagy negatív a timer1.Interval)
            else if (timer1.Interval == 30)
            {
                timer1.Interval = 30;
            }
        }
        #endregion

        #region Osztályok
        //Osztályok származtatása PictureBoxból
        class KígyóElem : PictureBox
        {
            public static int KigyoMeret=30;
            public KígyóElem()
            {
                Width = KigyoMeret;
                Height = KigyoMeret;
                BackColor = Color.Green;
            }
        }
        class Mereg : PictureBox
        {
            public static int MeregMeret=30;
            public Mereg()
            {
                Width = MeregMeret;
                Height=MeregMeret;
                BackColor=Color.Black;
            }
        }
        class Kaja: PictureBox
        {
              public static int KajaMeret = 30;
              public Kaja()
              {
                   Width=KajaMeret;
                   Height=KajaMeret;
                   BackColor=Color.Red;
              }
        }
        #endregion
    }
}