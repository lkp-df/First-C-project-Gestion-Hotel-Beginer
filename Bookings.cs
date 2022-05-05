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
    public partial class Bookings : Form
    {
        public Bookings()
        {
            InitializeComponent();
            this.popullate();
            GetCustumers();
            GetRoomsAvailable();
        }

        //methode d'action sur la base de données
        /**** ETABLISSEMNT VARIABLE DE CONNEXION A LA BASE DE DONNES ********/
        SqlConnection Con = new SqlConnection("Data Source=DESKTOP-DD2QERU;Initial Catalog=HotelDatabase;Integrated Security=True;Pooling=False");

        //AFFICHER TOUTES LES CHAMBRES DISPONIBLE UNIQUEMENT
        private void GetRoomsAvailable()
        {
            try
            {
                //OUVERTURE DE CONNEXION
                Con.Open();

                //SQL REQUETE
                SqlCommand sql = new SqlCommand("select * from Room where RStatus = @RS", Con);

                //BINDING DES VALUES
                sql.Parameters.AddWithValue("@RS", "Available");

                //POUR LIRE LES donnees RECUPEREs
                SqlDataReader rdr;
                rdr = sql.ExecuteReader();

                //MISE DES DONNEES DANS UNE TABLE
                DataTable dt = new DataTable();
                dt.Columns.Add("RNum", typeof(int));
                dt.Load(rdr);

                //RECUPPERER LES VALEURS QUI SERONT DES ENTIERS AFIN DE FACILITER LA CLE ETRANGERER
                BroomCb.ValueMember = "RNum";

                //ALIMENTE LES DONNEES VISIBLES PAR LE USER QUI PROVIENNENT DE LA TABLE TYPE :: ON POUVAIT LE FAIRE DEPUIS LE COMBOWBOX
                BroomCb.DataSource = dt;

                //FERMETURE DE CONNEXION A LA BASE DE DONNEES
                Con.Close();
            }
            catch (Exception e)
            {
                //ON LEVE L'EXCEPTION ON AFFICHANT LE MESSAGE D'ERREUR
                MessageBox.Show(e.Message);
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
                BcustumerCb.ValueMember = "CustNum";

                //ALIMENTE LES DONNEES VISIBLES PAR LE USER QUI PROVIENNENT DE LA TABLE TYPE :: ON POUVAIT LE FAIRE DEPUIS LE COMBOWBOX
                BcustumerCb.DataSource = dt;

                //FERMETURE DE CONNEXION A LA BASE DE DONNEES
                Con.Close();
            }
            catch (Exception e)
            {
                //ON LEVE L'EXCEPTION ON AFFICHANT LE MESSAGE D'ERREUR
                MessageBox.Show(e.Message);
            }

        }

        //LA TABLE TYPE REPRESENTE LA CATEGORIE, LORS DE LA CREATION DES CATEGORIES DES CHAMBRE ON MET LA CHAMBRE ET SON PRIX
        //C EST POUR QUOI NOUS AVONS FAIT LA JOINTURE POUR RECUPERER LE PRIX EN FONCTION DE LA CHAMBRE SELECTIONNEE PAR LE USER
        //AFFICHER LE prix de la chambre

        int price = 1;
        private void FetchCost()
        {
            Con.Open();

            //REQUETE D4EXTRATION DU CLIENT SELECTIONNE CAR GETCUSTUMERS RENVOIE LES IDS DES CLIENTS
            //ET PAR RAPPORT A L ID SELECTION ON VA AFFICHER SON NOM

            string Query = "select TypeCost from Room join Type on Type.TypeNum = Room.RType where RNum = " + BroomCb.SelectedValue.ToString() + " ";

            SqlCommand sql = new SqlCommand(Query, Con);

            DataTable dt = new DataTable();

            SqlDataAdapter sda = new SqlDataAdapter(sql);

            //remplissage 
            sda.Fill(dt);

            foreach (DataRow dr in dt.Rows)
            {
                price = Convert.ToInt32(dr["TypeCost"].ToString());
            }

            Con.Close();
        }

        private void InsertBooking()
        {

            //VERIFICATION QUE UNE INFORMATION EST FOURNIE POUR TOUS LES CHAMPS 
            if (BroomCb.SelectedIndex == -1 || BcustumerCb.SelectedIndex == -1 ||
                    BdatePicker.Value.Date == null || BdurationTb.Text == "" || BamountTb.Text == ""
               )
            {
                MessageBox.Show("Missing Information", "ALL Fiels Are Required", MessageBoxButtons.OK);
            }
            else
            {

                try
                {
                    //OUVERTURE DE CONNEXION
                    Con.Open();

                    //SQL REQUETE
                    SqlCommand sql = new SqlCommand("insert into Booking (Room,Custumer,BookDate,Duration,Cost) values(@BR,@BCU,@BDA,@BDU,@BCO) ", Con);
                    //BINDING DES VALUES
                    sql.Parameters.AddWithValue("@BR", BroomCb.SelectedValue.ToString());
                    sql.Parameters.AddWithValue("@BCU", BcustumerCb.SelectedValue.ToString());
                    sql.Parameters.AddWithValue("@BDA", BdatePicker.Value.Date);
                    sql.Parameters.AddWithValue("@BDU", BdurationTb.Text);
                    sql.Parameters.AddWithValue("@BCO", BamountTb.Text);

                    //EXECUTION DE LA REQUETE
                    int nbreLigne = sql.ExecuteNonQuery();

                    //FERMETURE DE CONNEXION A LA BASE DE DONNEES
                    Con.Close();

                    if (nbreLigne > 0)
                    {

                        MessageBox.Show("Room Booked successfully", "Successfully", MessageBoxButtons.OK);

                        //ON AFFICHE L'ELEMENT INSERE
                        popullate();

                        //pour mettre a jour la chambre en booked (reservée)
                        SetRoomBooked(Convert.ToInt32(BroomCb.SelectedValue.ToString()));

                        GetRoomsAvailable();

                        //on vide les champs
                        //ON VID ELES CHAMPS
                        BdurationTb.Text = "";
                       // BamountTb.Text = "";
                        BroomCb.SelectedIndex = -1;
                        BcustumerCb.SelectedIndex = -1;

                        //FERMETURE DE CONNEXION A LA BASE DE DONNEES
                        Con.Close();
                    }

                }
                catch (Exception e)
                {
                    //ON LEVE L'EXCEPTION ON AFFICHANT LE MESSAGE D'ERREUR
                    MessageBox.Show(e.Message);
                }
            }

        }
        //METTRE A JOUR LE STATUS DE LA CHAMBRE QUI VIENT D ETRE RESERVE A BOOKED, CAR ELLE N EST PLUS DISPONIBLE
        private void SetRoomBooked(int id)
        {
            try
            {
                //OUVERTURE DE CONNEXION
                Con.Open();

                //SQL REQUETE
                SqlCommand sql = new SqlCommand("update  Room set RStatus = @RS where RNum = @RNum", Con);

                //BINDING DES VALUES
                sql.Parameters.AddWithValue("@RS", "Booked");
                sql.Parameters.AddWithValue("@RNum", id);

                int nbreLigne = sql.ExecuteNonQuery();

                if (nbreLigne > 0)
                {
                    MessageBox.Show("Room State have Change", "Successfully", MessageBoxButtons.OK);

                }

                //FERMETURE DE CONNEXION A LA BASE DE DONNEES
                Con.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

        }

        //METTRE A JOUR LE STATUS DE LA CHAMBRE QUI VIENT D ETRE RENDU DISPONIBLE (AVAILABLE), CAR ELLE EST MAINTENANT DISPONIBLE
        private void SetRoomAvailable()
        {
            try
            {
                //OUVERTURE DE CONNEXION
                Con.Open();

                //SQL REQUETE
                SqlCommand sql = new SqlCommand("update  Room set RStatus = @RS where RNum = @RNum", Con);

                //BINDING DES VALUES
                sql.Parameters.AddWithValue("@RS", "Available");
                sql.Parameters.AddWithValue("@RNum", BroomCb.Text);

                int nbreLigne = sql.ExecuteNonQuery();

                if (nbreLigne > 0)
                {
                    MessageBox.Show("Room State have Change", "Successfully", MessageBoxButtons.OK);

                }

                //FERMETURE DE CONNEXION A LA BASE DE DONNEES
                Con.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

        }

        //DELETE DATA
        private void DeleteBooking()
        {
            //VERIFICATION QUE UN CHAMP DE LA GRILLE EST SELECTIONNEE 
            if (this.key == 0)
            {
                MessageBox.Show("You Should Select A Booking", "Select A Field To Delete", MessageBoxButtons.OK);
            }
            else
            {
                try
                {
                    //OUVERTURE DE CONNEXION
                    Con.Open();

                    //SQL REQUETE
                    SqlCommand sql = new SqlCommand("delete from Booking where BookingNum = @BKEY", Con);

                    //BINDING DES VALUES
                    sql.Parameters.AddWithValue("@BKEY", key);

                    //AVERTISSEMENT DE SUPPRESSION
                    DialogResult dr = MessageBox.Show("Are you Sure To Deleted this Booking ", "Attention !!!", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);

                    if (dr == DialogResult.OK)
                    {
                        //EXECUTION DE LA REQUETE
                        int nbreLigne = sql.ExecuteNonQuery();

                        //FERMETURE DE CONNEXION A LA BASE DE DONNEES
                        Con.Close();

                        if (nbreLigne > 0)
                        {
                            MessageBox.Show("Booking  Deleted", "Operation Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            //ON MET A JOUR LA CHAMBRE EN AVAILABLE
                            SetRoomAvailable();
                            //ON VIDE LES CHAMPS
                            BdurationTb.Text = "";
                            BamountTb.Text = "";
                            BroomCb.SelectedIndex = -1;
                            BcustumerCb.SelectedIndex = -1;

                            //ON AFFICHE L'ELEMENT INSERE
                            popullate();
                        }
                    }
                    //FERMETURE DE CONNEXION A LA BASE DE DONNEES
                    Con.Close();
                }
                catch (Exception e)
                {
                    //ON LEVE L'EXCEPTION ON AFFICHANT LE MESSAGE D'ERREUR
                    MessageBox.Show(e.Message);
                }
            }
        }

        //AFFICHER LES DONNEES  DE LA TABLE
        private void popullate()
        {
            try
            {
                //OUVERTURE DE CONNEXION
                Con.Open();

                //SQL REQUETE
                string Query = "select * from Booking";
                SqlDataAdapter sda = new SqlDataAdapter(Query, Con);
                SqlCommandBuilder Builder = new SqlCommandBuilder(sda);

                //CREATION DU CACHE DES DONNEES EN MEMOIRE
                var ds = new DataSet();

                //REMPLISSAGE DES LIGNES DANS LE DATASET
                sda.Fill(ds);

                //AFFICHAGE DES CATEGORIES DANS NOTRE TYPESDGVIEW, pour plus d'info mettre le cursuer sur le nom des elements utilises
                BookingsDGView.DataSource = ds.Tables[0];

                //FERMETURE DE CONNEXION A LA BASE DE DONNEES
                Con.Close();
            }
            catch (Exception e)
            {
                //ON LEVE L'EXCEPTION ON AFFICHANT LE MESSAGE D'ERREUR
                throw e;
                //MessageBox.Show(e.Message);
            }
        }

        private void label3_Click(object sender, EventArgs e)
        {
            Rooms rooms = new Rooms();
            rooms.Show();
            this.Hide();
        }

        private void label4_Click(object sender, EventArgs e)
        {
            Types types = new Types();
            types.Show();
            this.Hide();
        }

        private void label5_Click(object sender, EventArgs e)
        {
            Users users = new Users();
            users.Show();
            this.Hide();
        }

        private void label6_Click(object sender, EventArgs e)
        {
            Customers customers = new Customers();
            customers.Show();
            this.Hide();
        }

        private void BroomCb_SelectionChangeCommitted(object sender, EventArgs e)
        {
            FetchCost();
        }

        private void BdurationTb_TextChange(object sender, EventArgs e)
        {


            if (BamountTb.Text == "")
            {
                BamountTb.Text = "Rs 0";
            }
            else
            {
                try
                {
                    int totalAmount = price * Convert.ToInt32(BdurationTb.Text);
                    BamountTb.Text = "" + totalAmount;
                }
                catch (Exception ex)
                {
                    //MessageBox.Show(ex.Message);
                }
            }
        }

        private void BookBtn_Click(object sender, EventArgs e)
        {
            InsertBooking();
        }

        int key = 0;
        private void BookingsDGView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            BroomCb.Text = BookingsDGView.SelectedRows[0].Cells[1].Value.ToString();
            BcustumerCb.Text = BookingsDGView.SelectedRows[0].Cells[2].Value.ToString();
            BdatePicker.Text = BookingsDGView.SelectedRows[0].Cells[3].Value.ToString();

            BdurationTb.Text = BookingsDGView.SelectedRows[0].Cells[4].Value.ToString();

            BamountTb.Text = BookingsDGView.SelectedRows[0].Cells[5].Value.ToString();

            if (BamountTb.Text == "" || BdurationTb.Text == "")
            {
                key = 0;
            }
            else
            {
                key = Convert.ToInt32(BookingsDGView.SelectedRows[0].Cells[0].Value.ToString());
            }
        }

        private void CancelBtn_Click(object sender, EventArgs e)
        {
            DeleteBooking();
            //MessageBox.Show(BroomCb.Text);
            GetRoomsAvailable();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            //ON REDIRIGE 0 LA FENETRE DE CONNEXION
            Login login = new Login();
            login.Show();
            this.Hide();
        }

        private void label8_Click(object sender, EventArgs e)
        {
            Dashboard dashboard = new Dashboard();
            dashboard.Show();
            this.Hide();
        }
    }
}
