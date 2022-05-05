using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyHotel
{
    public partial class Splash : Form
    {
        //constructeur
        public Splash()
        {
            InitializeComponent();

            //demarage du timer
            timer1.Start();
        }

        //PREPARATION DES COMPOSANTS AVANT AFFICHAGE
        int StartP = 0;

        private void timer1_Tick(object sender, EventArgs e)
        {
            StartP += 1;

            //INITILISATION DE LA VALEUR DE DEPART DU PROGRESS BAR
            SProgress.Value = StartP;

            if (SProgress.Value == 100)
            {
                SProgress.Value = 0;
                timer1.Stop();

                Login login = new Login();
                login.Show();
                this.Hide();
            }
        }
    }
}
