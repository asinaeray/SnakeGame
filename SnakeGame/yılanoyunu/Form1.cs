using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace yılanoyunu
{
    public partial class Form1 : Form
    {
        private Label _yilankafasi;
        private int _yilanparçasiarasimesafe=2;
        private int _YılanParçasiSayisi;
        private int _yılanboyutu=20;
        private int _yemboyutu = 20;
        private Label _yem;
        private Random _random;
        private HarekeYonu _yon;
        public Form1()
        {
            InitializeComponent();
            _random = new Random();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            sifirla();
        }

        private void yenidenbaslat()
        {
            lblpuan.Text = "0";
            lblsure.Text = "0";

            sifirla();
           
            
        }
        public void sifirla()
        {
            pnl.Controls.Clear();
            _YılanParçasiSayisi = 0;
            yemolustur();
            yeminyerinidegistir();
            yilaniyerlestir();
            _yon = HarekeYonu.saga;
            timerYilanHareket.Enabled = true;
            timersaat.Enabled = true;
        }

        private Label YılanParcasiOlustur(int locationX, int locationY)
        {
            _YılanParçasiSayisi++;
            Label lbl = new Label()
            {
                Name = "yılan parçası"+_YılanParçasiSayisi,
                BackColor=Color.Red,
                Width=_yılanboyutu,
                Height=_yılanboyutu,
                Location=new Point(locationX, locationY)
            };
            this.pnl.Controls.Add(lbl);
            return lbl;

        }
        private void yilaniyerlestir()
        {
            _yilankafasi = YılanParcasiOlustur(0, 0);
            _yilankafasi.Text = ":";
            _yilankafasi.TextAlign = ContentAlignment.MiddleCenter;
            _yilankafasi.ForeColor = Color.White;
            var locationX = (pnl.Width / 2) - (_yilankafasi.Width / 2);
            var locationY = (pnl.Height / 2) - (_yilankafasi.Height / 2);
            _yilankafasi.Location = new Point(locationX, locationY);
        }
        private void yemolustur()
        {
            Label lbl = new Label()
            {
                Name = "yem",
                BackColor = Color.Yellow,
                Width = _yemboyutu,
                Height = _yemboyutu,
                
            };
            _yem=lbl;
            this.pnl.Controls.Add(lbl);
           

        }
        private void yeminyerinidegistir()
        {
            var locationX = 0;
            var locationY = 0;

            bool durum ;
            do
            {
                durum = false;
                 locationX = _random.Next(0, pnl.Width - _yemboyutu);
                 locationY = _random.Next(0, pnl.Height - _yemboyutu);
                var rect1 = new Rectangle(new Point(locationX, locationY), _yem.Size);
                foreach (Control control in pnl.Controls)
                {
                    if (control is Label && control.Name.Contains("yılan parçası"))
                    {
                        var rect2 = new Rectangle(control.Location, control.Size);
                        if (rect1.IntersectsWith(rect2))
                        {
                            durum = true;
                            break;
                        }
                    }
                }
            } while (durum);
           

            _yem.Location = new Point(locationX, locationY);
        }
        private enum HarekeYonu
        {
            yukari,
            asagi,
            sola,
            saga
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            var keyCode = e.KeyCode;
            if (_yon==HarekeYonu.sola&&keyCode==Keys.D
                || _yon == HarekeYonu.saga && keyCode == Keys.A
                || _yon == HarekeYonu.yukari && keyCode == Keys.S
                || _yon == HarekeYonu.asagi && keyCode == Keys.W)
            {
                return;
            }

            switch (keyCode)                    
            {
                case Keys.W:
                    _yon = HarekeYonu.yukari;
                    break;
                case Keys.S:
                    _yon = HarekeYonu.asagi;
                    break;
                case Keys.A:
                    _yon = HarekeYonu.sola;
                    break;
                case Keys.D:
                    _yon = HarekeYonu.saga;
                    break;
                case Keys.P:
                    timersaat.Enabled = false;
                    timerYilanHareket.Enabled = false;
                    break;
                case Keys.C:
                    timersaat.Enabled = true;
                    timerYilanHareket.Enabled = true;
                    break;
                default:
                    break;
            }
        }

        private void timerYilanHareket_Tick(object sender, EventArgs e)
        {
            yilankafasinitakipet();
            yilaniyurut();
            oyunbitimi();
            YilanYemiYedimi();
        }
        private void yilaniyurut()
        {
            var locationX = _yilankafasi.Location.X;
            var locationY = _yilankafasi.Location.Y;
            switch (_yon)
            {
                case HarekeYonu.yukari:
                    _yilankafasi.Location = new Point(locationX, locationY - (_yilankafasi.Width + _yilanparçasiarasimesafe));
                    break;
                case HarekeYonu.asagi:
                    _yilankafasi.Location = new Point(locationX, locationY + (_yilankafasi.Width + _yilanparçasiarasimesafe));
                    break;
                case HarekeYonu.sola:
                    _yilankafasi.Location = new Point(locationX - (_yilankafasi.Width + _yilanparçasiarasimesafe), locationY);
                    break;
                case HarekeYonu.saga:
                    _yilankafasi.Location = new Point(locationX + (_yilankafasi.Width + _yilanparçasiarasimesafe), locationY);
                    break;
                default:
                    break;
        }
        
        }

        private void oyunbitimi()
        {
            bool oyunbittimi = false;
            var rect1 = new Rectangle(_yilankafasi.Location, _yilankafasi.Size);

            foreach (Control control in pnl.Controls)
            {
                if (control is Label&&control.Name.Contains("yılan parça") && control.Name != _yilankafasi.Name)
                {
                    var rect2 = new Rectangle(control.Location, control.Size);
                    if (rect1.IntersectsWith(rect2))
                    {
                        oyunbittimi = true;
                        break;
                    }
                }
            }
            if (oyunbittimi)
            {
                timerYilanHareket.Enabled = false;
                timersaat.Enabled = false;
               DialogResult sonuc= MessageBox.Show("puanınız :" + lblpuan.Text,"oyun bitti!",MessageBoxButtons.OKCancel);

               if (sonuc==DialogResult.OK)
               {
                   yenidenbaslat();
               }
            }
        }



        private void  YilanYemiYedimi()
        {
            var rect1 = new Rectangle(_yilankafasi.Location, _yilankafasi.Size);
            var rect2 = new Rectangle(_yem.Location, _yem.Size);

            if (rect1.IntersectsWith(rect2))
            {
                lblpuan.Text = (Convert.ToInt32(lblpuan.Text) + 10).ToString();
                yeminyerinidegistir();
                YılanParcasiOlustur(- _yılanboyutu, - _yılanboyutu);
            }
        }
        private void  yilankafasinitakipet()
        {
            if (_YılanParçasiSayisi < 1) return;
            for (int i = _YılanParçasiSayisi; i > 1; i--)
            {
                var sonrakiParca = (Label)pnl.Controls[i];
                var oncekiParca = (Label)pnl.Controls[i-1];
                sonrakiParca.Location = oncekiParca.Location;
            }
        }

        private void timersaat_Tick(object sender, EventArgs e)
        {
            lblsure.Text = (Convert.ToInt32(lblsure.Text)+1) .ToString();
        }

    }
}
