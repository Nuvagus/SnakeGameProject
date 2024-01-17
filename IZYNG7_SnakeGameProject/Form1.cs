namespace IZYNG7_SnakeGameProject
{
    public partial class Form1 : Form
    {
        #region Deklar�ci�
        int l�p�ssz�m;
        int eltelt_ido;
        int fej_x = 90;
        int fej_y = 90;
        int ir�ny_x=1;
        int ir�ny_y=0;
        int mereg_x;
        int mereg_y;
        int kaja_x;
        int kaja_y;
        int kigyoHossz = 5;
        Random r = new Random();
        public Label l = new Label();
        public Label l2 = new Label();
        List<K�gy�Elem> k�gy� = new List<K�gy�Elem>();
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

        #region Billenty�Lenyom�s
        private void Form1_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode==Keys.Up)
            {
                ir�ny_x = 0;
                ir�ny_y = -1;
            }
            if (e.KeyCode == Keys.Down)
            {
                ir�ny_x = 0;
                ir�ny_y = 1;
            }
            if (e.KeyCode == Keys.Left)
            {
                ir�ny_x = -1;
                ir�ny_y = 0;
            }
            if (e.KeyCode == Keys.Right)
            {
                ir�ny_x = 1;
                ir�ny_y = 0;
            }

        }
        #endregion

        #region Timer1
        private void Timer1_Tick(object sender, EventArgs e)
        {
            l�p�ssz�m++;
            l.Text = $"L�p�ssz�mok: {l�p�ssz�m}";
            Controls.Add(l);
            fej_x += ir�ny_x * K�gy�Elem.KigyoMeret;
            fej_y += ir�ny_y * K�gy�Elem.KigyoMeret;


            #region Kaja
            //Kaja megjelen�s
            if (l�p�ssz�m % 7 == 0)
            {
               Kaja kaj = new Kaja();
               kaja_x = r.Next(Width / K�gy�Elem.KigyoMeret) * K�gy�Elem.KigyoMeret;
               kaja_y = r.Next(Height / K�gy�Elem.KigyoMeret) * K�gy�Elem.KigyoMeret;
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

            //K�gy� �s az �tel tal�lkoz�sa
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

            #region M�reg
            //M�reg megjelen�se
            if (l�p�ssz�m % 11 == 0)
            {
                Mereg mer = new Mereg();
                mereg_x = r.Next(Width / K�gy�Elem.KigyoMeret) * K�gy�Elem.KigyoMeret;
                mereg_y = r.Next(Height / K�gy�Elem.KigyoMeret) * K�gy�Elem.KigyoMeret;
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
            //K�gy� �s a m�reg tal�lkoz�sa
            foreach (object item in Controls)
            {
                if (item is Mereg)
                {
                    Mereg mereg = (Mereg)item;
                    if (mereg.Top == fej_y && mereg.Left == fej_x)
                    {
                        mereg.Visible = false;
                        timer1.Enabled = false;
                        MessageBox.Show("Vesztett�l!");
                        //Application.Restart();
                        Environment.Exit(0);
                    }
                }
            }
            #endregion

            #region K�gy�Objektum
            //�tk�z�svizsg�lat
            foreach (object item in Controls)
            {
                if (item is K�gy�Elem)
                {
                    K�gy�Elem k = (K�gy�Elem)item;
                    if (k.Top == fej_y && k.Left == fej_x)
                    {
                        timer1.Enabled = false;
                        timer2.Enabled = false;
                        MessageBox.Show("Vesztett�l!");
                        //Application.Restart();
                        Environment.Exit(0);
                    }
                }
            }

            //K�gy�elem l�trehoz�sa
            K�gy�Elem ke = new K�gy�Elem();
            k�gy�.Add(ke);
            ke.Top = fej_y;
            ke.Left = fej_x;
            Controls.Add(ke);

            if (k�gy�.Count > kigyoHossz)
            {
                K�gy�Elem lev�gand� = k�gy�[0];
                k�gy�.RemoveAt(0);
                Controls.Remove(lev�gand�);
            }
            
            //Sz�nv�lt�s
            if (l�p�ssz�m % 2 == 0) ke.BackColor = Color.LightGreen;
            #endregion
        }
        #endregion

        #region Timer2
        private void Timer2_Tick(object? sender, EventArgs e)
        {
            eltelt_ido += timer2.Interval;
            l2.Text = $"Eltelt id�(s): {(eltelt_ido / 1000).ToString()}";
            Controls.Add(l2);

            //Id�z�t�
            timer1.Start();
            if (l�p�ssz�m % 3 == 0 && timer1.Interval > 30)
            {
                timer1.Interval -= 30;
            }

            //Hibakezel�s (Nem lehet 0 vagy negat�v a timer1.Interval)
            else if (timer1.Interval == 30)
            {
                timer1.Interval = 30;
            }
        }
        #endregion

        #region Oszt�lyok
        //Oszt�lyok sz�rmaztat�sa PictureBoxb�l
        class K�gy�Elem : PictureBox
        {
            public static int KigyoMeret=30;
            public K�gy�Elem()
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