using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace MyHotel
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        //methode d'action sur la base de données
        /**** ETABLISSEMNT VARIABLE DE CONNEXION A LA BASE DE DONNES ********/
        SqlConnection Con = new SqlConnection("Data Source=DESKTOP-DD2QERU;Initial Catalog=HotelDatabase;Integrated Security=True;Pooling=False");
        private void Connection()
        {
            try
            {
                if (UnameTb.Text == "" || UpasswdTb.Text == "")
                {
                    MessageBox.Show("Missing Information", "ALL Fiels Are Required", MessageBoxButtons.OK);

                }
                else
                {
                    //OUVERTURE DE CONNEXION
                    Con.Open();

                    //SQL REQUETE
                    string Query = "select count(*) from  Users where UName = '" + UnameTb.Text + "' and UPassword = '" + UpasswdTb.Text + "' ";

                    SqlDataAdapter sda = new SqlDataAdapter(Query, Con);
                    DataTable dt = new DataTable();

                    SqlCommandBuilder Builder = new SqlCommandBuilder(sda);
                    sda.Fill(dt);

                    //ON VERIFIE QUE LE RENDU EST EGAL A 1
                    if (dt.Rows[0][0].ToString() == "1")
                    {
                        //CELA SIGNIFIE QUE LE USER EXISTE, ON LUI OUVRE LA FENETRE AFIN QU'IL COMMENCE A TRAVAILLER
                        //NOTRE FENETRE D'ENTREE EST ROOMS
                        Rooms rooms = new Rooms();
                        rooms.Show();

                        this.Hide();

                        //FERMETURE DE CONNEXION A LA BASE DE DONNEES
                        Con.Close();
                    }
                    else
                    {
                        MessageBox.Show("Username or Password Invalid", "Incorrect Data", MessageBoxButtons.OK);
                    }

                    //FERMETURE DE CONNEXION A LA BASE DE DONNEES
                    Con.Close();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
        private void LoginBtn_Click(object sender, EventArgs e)
        {
            Connection();
        }

        private void ContinueLbl_Click(object sender, EventArgs e)
        {
            AdminLogin adminLogin = new AdminLogin();
            adminLogin.Show();
            this.Hide();
        }

        private void ClosePic_Click(object sender, EventArgs e)
        {
            //ON FERME L'APPLICATION TOUTE ENTIERE
            Application.Exit();
        }
    }
}
