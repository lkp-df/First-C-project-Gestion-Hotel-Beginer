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
using System.Globalization;

namespace MyHotel
{
    public partial class Dashboard : Form
    {
        public Dashboard()
        {
            InitializeComponent();
            GetCustumers();
            countRoom();
            countCustomer();
            SumAmountBooking();

        }

        //methode d'action sur la base de données
        /**** ETABLISSEMNT VARIABLE DE CONNEXION A LA BASE DE DONNES ********/
        SqlConnection Con = new SqlConnection("Data Source=DESKTOP-DD2QERU;Initial Catalog=HotelDatabase;Integrated Security=True;Pooling=False");

        //COUNTER LE TOTAL DES CHAMBRES
        private void countRoom()
        {
            try
            {
                //OUVERTURE DE CONNEXION
                Con.Open();

                string Query = "select count(*) from Room";
                SqlDataAdapter sda = new SqlDataAdapter(Query, Con);

                //ON CREE UNE NOUVELLE TABLE DE DONNEES EN MEMOIRE POUR STOCKER LE RENDU DE LA REQUETE
                DataTable dt = new DataTable();

                //ON REMPLIT LES DONNEES, MAIS ON AURA QU UN SEUL RESULTAT
                sda.Fill(dt);


                //STOCK LE NOMBRE DES CHAMBRES RETOURNER PAR LA REQUETE SQL, L INDICIE 0 0 REPRESENTE NOTRE VALAUER RETORUNEE PAR LA REQUETE

                if (Convert.ToInt32(dt.Rows[0][0].ToString()) > 1)
                {
                    DRoomsLbl.Text = dt.Rows[0][0].ToString() + "  Rooms";

                }
                else
                {
                    DRoomsLbl.Text = dt.Rows[0][0].ToString() + "  Room";
                }

                //FERMETURE DE CONNEXION A LA BASE DE DONNEES
                Con.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                //FERMETURE DE CONNEXION A LA BASE DE DONNEES
                Con.Close();
            }
        }

        //COUNTER LE TOTAL DES CLIENTS
        private void countCustomer()
        {
            try
            {
                //OUVERTURE DE CONNEXION
                Con.Open();

                string Query = "select count(*) from Custumer";
                SqlDataAdapter sda = new SqlDataAdapter(Query, Con);

                //ON CREE UNE NOUVELLE TABLE DE DONNEES EN MEMOIRE POUR STOCKER LE RENDU DE LA REQUETE
                DataTable dt = new DataTable();

                //ON REMPLIT LES DONNEES, MAIS ON AURA QU UN SEUL RESULTAT
                sda.Fill(dt);


                //STOCK LE NOMBRE DES CHAMBRES RETOURNER PAR LA REQUETE SQL, L INDICIE 0 0 REPRESENTE NOTRE VALAUER RETORUNEE PAR LA REQUETE

                if (Convert.ToInt32(dt.Rows[0][0].ToString()) > 1)
                {
                    DCustomerLbl.Text = dt.Rows[0][0].ToString() + "  Customers";

                }
                else
                {
                    DCustomerLbl.Text = dt.Rows[0][0].ToString() + "  Customer";
                }

                //FERMETURE DE CONNEXION A LA BASE DE DONNEES
                Con.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                //FERMETURE DE CONNEXION A LA BASE DE DONNEES
                Con.Close();
            }
        }

        //COUNTER LE TOTAL DES RESERVATIONS
        private void countBooking()
        {
            try
            {
                //OUVERTURE DE CONNEXION
                Con.Open();

                string Query = "select count(*) from Booking";
                SqlDataAdapter sda = new SqlDataAdapter(Query, Con);

                //ON CREE UNE NOUVELLE TABLE DE DONNEES EN MEMOIRE POUR STOCKER LE RENDU DE LA REQUETE
                DataTable dt = new DataTable();

                //ON REMPLIT LES DONNEES, MAIS ON AURA QU UN SEUL RESULTAT
                sda.Fill(dt);


                //STOCK LE NOMBRE DES CHAMBRES RETOURNER PAR LA REQUETE SQL, L INDICIE 0 0 REPRESENTE NOTRE VALAUER RETORUNEE PAR LA REQUETE
                if (Convert.ToInt32(dt.Rows[0][0].ToString()) > 1)
                {
                    DBookingLbl.Text = dt.Rows[0][0].ToString() + "  Bookings";

                }
                else
                {
                    DBookingLbl.Text = dt.Rows[0][0].ToString() + "  Booking";
                }

                //FERMETURE DE CONNEXION A LA BASE DE DONNEES
                Con.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                //FERMETURE DE CONNEXION A LA BASE DE DONNEES
                Con.Close();
            }
        }

        //FAIT LA SOMME TOTALE DES RESERVATIONS
        private void SumAmountBooking()
        {
            try
            {
                //OUVERTURE DE CONNEXION
                Con.Open();

                string Query = "select sum(Cost) from Booking";
                SqlDataAdapter sda = new SqlDataAdapter(Query, Con);

                //ON CREE UNE NOUVELLE TABLE DE DONNEES EN MEMOIRE POUR STOCKER LE RENDU DE LA REQUETE
                DataTable dt = new DataTable();

                //ON REMPLIT LES DONNEES, MAIS ON AURA QU UN SEUL RESULTAT
                sda.Fill(dt);


                //STOCK LE NOMBRE DES CHAMBRES RETOURNER PAR LA REQUETE SQL, L INDICIE 0 0 REPRESENTE NOTRE VALAUER RETORUNEE PAR LA REQUETE
                if (Convert.ToInt32(dt.Rows[0][0].ToString()) > 0)
                {
                    DBookingLbl.Text = "Rs " + dt.Rows[0][0].ToString();

                }
                else
                {
                    DBookingLbl.Text = "No  Booking";
                }

                //FERMETURE DE CONNEXION A LA BASE DE DONNEES
                Con.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                //FERMETURE DE CONNEXION A LA BASE DE DONNEES
                Con.Close();
            }
        }


        //FAIT LA SOMME TOTALE DES RESERVATIONS PAR JOUR CHOISI PAR L'ADMIN DANS LE CALENDRIER
        private void SumDailyIncome()
        {
            try
            {
                //OUVERTURE DE CONNEXION
                Con.Open(); 
                MessageBox.Show(DdatePicker.Value.Date.ToString(CultureInfo.CreateSpecificCulture("fr-FR")));
               

                // string Query = "select sum(Cost) from Booking where BookDate = '"+ DdatePicker.Value.Date +"'";
                SqlDataAdapter sda = new SqlDataAdapter("select sum(Cost) from Booking where BookDate='"+DdatePicker.Value.Date+"' ", Con);

                //ON CREE UNE NOUVELLE TABLE DE DONNEES EN MEMOIRE POUR STOCKER LE RENDU DE LA REQUETE
                DataTable dt = new DataTable();

                //ON REMPLIT LES DONNEES, MAIS ON AURA QU UN SEUL RESULTAT
                sda.Fill(dt);
                DIncomeLbl.Text = "Rs " + dt.Rows[0][0].ToString();
                if (Convert.ToInt32(dt.Rows[0][0].ToString()) > 0)
                {
                    MessageBox.Show(dt.Rows[0][0].ToString());
                }
                else
                {
                    MessageBox.Show("rien");
                }
                    

                //FERMETURE DE CONNEXION A LA BASE DE DONNEES
                Con.Close();

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                //FERMETURE DE CONNEXION A LA BASE DE DONNEES
                Con.Close();
            }
        }

        //FAIT LA SOMME TOTALE DES RESERVATIONS PAR CLIENT
        private void SumDailyIncomeByCustomer()
        {
            try
            {
                //OUVERTURE DE CONNEXION
                Con.Open();

                // string Query = "select sum(Cost) from Booking where BookDate = '"+ DdatePicker.Value.Date +"'";
                SqlDataAdapter sda = new SqlDataAdapter("select sum(Cost) from Booking where Custumer = " + DcustumerCb.SelectedValue.ToString() + " ", Con);

                //ON CREE UNE NOUVELLE TABLE DE DONNEES EN MEMOIRE POUR STOCKER LE RENDU DE LA REQUETE
                DataTable dt = new DataTable();

                //ON REMPLIT LES DONNEES, MAIS ON AURA QU UN SEUL RESULTAT
                sda.Fill(dt);
                DCIncomeLbl.Text = "Rs " + dt.Rows[0][0].ToString();

                //FERMETURE DE CONNEXION A LA BASE DE DONNEES
                Con.Close();

            }
            catch (Exception e)
            {
                //throw e;
                MessageBox.Show(e.Message);
                //FERMETURE DE CONNEXION A LA BASE DE DONNEES
                Con.Close();
            }
        }

        //AFFICHER TOUTES LES CLIENTS 
        private void GetCustumers()
        {
            try
            {
                //OUVERTURE DE CONNEXION
                Con.Open();

                //SQL REQUETE
                SqlCommand sql = new SqlCommand("select * from Custumer", Con);

                //POUR LIRE LES donnees RECUPEREs
                SqlDataReader rdr;
                rdr = sql.ExecuteReader();

                //MISE DES DONNEES DANS UNE TABLE
                DataTable dt = new DataTable();
                dt.Columns.Add("CustNum", typeof(int));
                dt.Load(rdr);

                //RECUPPERER LES VALEURS QUI SERONT DES ENTIERS AFIN DE FACILITER LA CLE ETRANGERER
                DcustumerCb.ValueMember = "CustNum";

                //ALIMENTE LES DONNEES VISIBLES PAR LE USER QUI PROVIENNENT DE LA TABLE TYPE :: ON POUVAIT LE FAIRE DEPUIS LE COMBOWBOX
                DcustumerCb.DataSource = dt;

                //FERMETURE DE CONNEXION A LA BASE DE DONNEES
                Con.Close();
            }
            catch (Exception e)
            {
                //ON LEVE L'EXCEPTION ON AFFICHANT LE MESSAGE D'ERREUR
                MessageBox.Show(e.Message);
            }

        }

        private void panel5_Paint(object sender, PaintEventArgs e)
        {

        }

        private void DdatePicker_ValueChanged(object sender, EventArgs e)
        {
            SumDailyIncome();
        }

        private void DcustumerCb_SelectionChangeCommitted(object sender, EventArgs e)
        {
            SumDailyIncomeByCustomer();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Login login = new Login();
            login.Show();
            this.Hide();
        }

        private void label7_Click(object sender, EventArgs e)
        {
            Bookings bookings = new Bookings();
            bookings.Show();
            this.Hide();
        }

        private void label6_Click(object sender, EventArgs e)
        {
            Customers customers = new Customers();
            customers.Show();
            this.Hide();
        }

        private void label5_Click(object sender, EventArgs e)
        {
            Users users = new Users();
            users.Show();
            this.Hide();
        }

        private void label4_Click(object sender, EventArgs e)
        {
            Types types = new Types();
            types.Show();
            this.Hide();
        }

        private void label3_Click(object sender, EventArgs e)
        {
            Rooms rooms = new Rooms();
            rooms.Show();
            this.Hide();
        }
    }
}
