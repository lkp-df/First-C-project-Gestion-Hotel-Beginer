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
    public partial class Rooms : Form
    {
        public Rooms()
        {
            InitializeComponent();
            //A L'APPEL DE CETTE FENETRE, ON AFFICHE LES DONNEES PRESENTE
            popullate();
            GetCategories();
        }

        //methode d'action sur la base de données
        /**** ETABLISSEMNT VARIABLE DE CONNEXION A LA BASE DE DONNES ********/
        SqlConnection Con = new SqlConnection(@"Data Source=DESKTOP-DD2QERU;Initial Catalog=HotelDatabase;Integrated Security=True;Pooling=False");
        private void InsertRooms()
        {
            //VERIFICATION QUE UNE INFORMATION EST FOURNIE POUR TOUS LES CHAMPS 
            if (RnameTb.Text == "" || RTypeCb.SelectedIndex == -1 || RStatusCb.SelectedIndex == -1)
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
                    SqlCommand sql = new SqlCommand("insert into Room(RName,RType,RStatus) " +
                                                     "values(@RN,@RT,@RS) ", Con);
                    //BINDING DES VALUES
                    sql.Parameters.AddWithValue("@RN", RnameTb.Text);

                    //AU NIVEAU DE TYPE ROOM, LES DONNEES PROVIENNENT DIRECTEMENT DE LA BD,
                    //SUR LE COMBOWBOX, FAUT CHOISIR UTILISER LES ELEMENTS LIES AUX DONNEES
                    //vue que le SELECTEDINDEX COMMENCE PAR 0,  ON LE FERA INCREMENTER DE + 1 AFIN DE RESPECTER LES CONTRAINTES DES CLES ETRANGERES
                    var idSelected = (RTypeCb.SelectedIndex) + 1;
                   // MessageBox.Show(RTypeCb.SelectedValue.ToString());

                    //SelectIdex CAR C'EST UN COMBOBOX et ON AURA BESOIN QUE DE RECUPERER L ID
                    sql.Parameters.AddWithValue("@RT", RTypeCb.SelectedValue.ToString());

                    sql.Parameters.AddWithValue("@RS", "Available");

                    //EXECUTION DE LA REQUETE
                    int nbreLigne = sql.ExecuteNonQuery();

                    //FERMETURE DE CONNEXION A LA BASE DE DONNEES
                    Con.Close();

                    if (nbreLigne > 0)
                    {
                        MessageBox.Show("Rooms  Are Added", "Successfully", MessageBoxButtons.OK);

                        //ON AFFICHE L'ELEMENT INSERE
                        popullate();
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
                string Query = "select * from  Room ";
                SqlDataAdapter sda = new SqlDataAdapter(Query, Con);
                SqlCommandBuilder Builder = new SqlCommandBuilder(sda);

                //CREATION DU CACHE DES DONNEES EN MEMOIRE
                var ds = new DataSet();

                //REMPLISSAGE DES LIGNES DANS LE DATASET
                sda.Fill(ds);

                //AFFICHAGE DES CATEGORIES DANS NOTRE TYPESDGVIEW, pour plus d'info mettre le cursuer sur le nom des elements utilises
                RoomsDGView.DataSource = ds.Tables[0];

                //FERMETURE DE CONNEXION A LA BASE DE DONNEES
                Con.Close();
            }
            catch (Exception e)
            {
                //ON LEVE L'EXCEPTION ON AFFICHANT LE MESSAGE D'ERREUR
                MessageBox.Show(e.Message);
            }
        }

        //AFFICHER TOUS LES TYPES DE CHAMBRE EN BASE DANS ROOM
        private void GetCategories()
        {
            try
            {
                //OUVERTURE DE CONNEXION
                Con.Open();

                //SQL REQUETE
                SqlCommand sql = new SqlCommand("select * from Type", Con);

                //POUR LIRE LES TYPES RECUPERER
                SqlDataReader rdr;
                rdr = sql.ExecuteReader();

                //MISE DES DONNEES DANS UNE TABLE
                DataTable dt = new DataTable();
                dt.Columns.Add("TypeNum", typeof(int));
                dt.Load(rdr);

                //RECUPPERER LES VALEURS QUI SERONT DES ENTIERS AFIN DE FACILITER LA CLE ETRANGERER
                RTypeCb.ValueMember = "TypeNum";

                //ALIMENTE LES DONNEES VISIBLES PAR LE USER QUI PROVIENNENT DE LA TABLE TYPE :: ON POUVAIT LE FAIRE DEPUIS LE COMBOWBOX
                RTypeCb.DataSource = dt;

                //FERMETURE DE CONNEXION A LA BASE DE DONNEES
                Con.Close();
            }
            catch (Exception e)
            {
                //ON LEVE L'EXCEPTION ON AFFICHANT LE MESSAGE D'ERREUR
                MessageBox.Show(e.Message);
            }

        }

        //MODIFIER LA DONNES DE LA ATBLE
        private void EditRooms()
        {
            //VERIFICATION QUE UNE INFORMATION EST FOURNIE POUR TOUS LES CHAMPS 
            if (RnameTb.Text == "" || RTypeCb.SelectedIndex == -1 || RStatusCb.SelectedIndex == -1)
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
                    SqlCommand sql = new SqlCommand("update Room set  RName = @RN, RType = @RT, Rstatus = @RS where Rnum = @RKEY", Con);

                    //BINDING DES VALUES
                    sql.Parameters.AddWithValue("@RN", RnameTb.Text);
                    sql.Parameters.AddWithValue("@RKEY", key);

                    //AU NIVEAU DE TYPE ROOM, LES DONNEES PROVIENNENT DIRECTEMENT DE LA BD,
                    //SUR LE COMBOWBOX, FAUT CHOISIR UTILISER LES ELEMENTS LIES AUX DONNEES
                    //vue que le SELECTEDINDEX COMMENCE PAR 0,  ON LE FERA INCREMENTER DE + 1 AFIN DE RESPECTER LES CONTRAINTES DES CLES ETRANGERES
                    var idSelected = (RTypeCb.SelectedIndex) + 1;

                    //SelectIdex CAR C'EST UN COMBOBOX et ON AURA BESOIN QUE DE RECUPERER L ID
                    sql.Parameters.AddWithValue("@RT", idSelected);

                    sql.Parameters.AddWithValue("@RS", RStatusCb.SelectedItem.ToString());

                    //EXECUTION DE LA REQUETE
                    int nbreLigne = sql.ExecuteNonQuery();

                    //FERMETURE DE CONNEXION A LA BASE DE DONNEES
                    Con.Close();

                    if (nbreLigne > 0)
                    {
                        MessageBox.Show("Rooms  Updated", "Successfully", MessageBoxButtons.OK);

                        //ON AFFICHE L'ELEMENT INSERE
                        popullate();
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

        //DELETE DATA
        private void DeleteRooms()
        {
            //VERIFICATION QUE UN CHAMP DE LA GRILLE EST SELECTIONNEE 
            if (this.key == 0)
            {
                MessageBox.Show("You Should Select A Room", "Select A Field To Delete", MessageBoxButtons.OK);
            }
            else
            {
                try
                {
                    //OUVERTURE DE CONNEXION
                    Con.Open();

                    //SQL REQUETE
                    SqlCommand sql = new SqlCommand("delete from Room where RNum = @RKEY", Con);

                    //BINDING DES VALUES
                    sql.Parameters.AddWithValue("@RKEY", key);

                    //AVERTISSEMENT DE SUPPRESSION
                    DialogResult dr = MessageBox.Show("Are you Sure To Deleted this Room ", "Attention !!!", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                    if (dr == DialogResult.OK)
                    {
                        //EXECUTION DE LA REQUETE
                        int nbreLigne = sql.ExecuteNonQuery();
                        //FERMETURE DE CONNEXION A LA BASE DE DONNEES
                        Con.Close();

                        if (nbreLigne > 0)
                        {
                            MessageBox.Show("Room  Deleted", "Successfully", MessageBoxButtons.OK, MessageBoxIcon.Question);
                            //ON VIDE LES CHAMPS
                            RnameTb.Text = "";
                            RTypeCb.SelectedIndex = 0;
                            RStatusCb.Text = "Status";
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

        //METHODES D'EVENEMENTS

        //POUR SAUVEGARDER UNE CHAMBRE
        private void SaveBtn_Click(object sender, EventArgs e)
        {
            //SUR CLIQUE SUR LE BOUTON SAVE ON APPELLE LA FONCTION QUI FAIT L'INSERTION
            InsertRooms();
        }

        private void Rooms_Load(object sender, EventArgs e)
        {
            // TODO: cette ligne de code charge les données dans la table 'hotelDatabaseDataSet.Type'. Vous pouvez la déplacer ou la supprimer selon les besoins.
            this.typeTableAdapter.Fill(this.hotelDatabaseDataSet.Type);

        }

        int key = 0;

        private void RoomsDGView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //ON RECUPERE LE CONTENU  DES COLONNES DE NOTRE GRILLE
            //CE SERA POUR L EDITION DES DONNEES, ON LES REAFFICHE DANS LE RNAMETB DANS LA ZONE DE TEXTE POUR LA MODIIFCATION
            RnameTb.Text = RoomsDGView.SelectedRows[0].Cells[1].Value.ToString();
            RTypeCb.Text = RoomsDGView.SelectedRows[0].Cells[2].Value.ToString();
            RStatusCb.Text = RoomsDGView.SelectedRows[0].Cells[3].Value.ToString();

            if (RnameTb.Text == "")
            {
                key = 0;
            }
            else
            {
                //CE SERA POUR LA RECUPERATION DE L'ID DE L'ELEMENT 0A MODIFIER
                key = Convert.ToInt32(RoomsDGView.SelectedRows[0].Cells[0].Value.ToString());
            }
        }

        private void EditBtn_Click(object sender, EventArgs e)
        {
            this.EditRooms();
        }

        private void DeleteBtn_Click(object sender, EventArgs e)
        {
            DeleteRooms();
        }

        //OUVERTURE DES FENETRES
        private void label4_Click(object sender, EventArgs e)
        {
            Types types = new Types();

            types.Show();

            //ON CACHE LA FENETRE COURANTE
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

        private void label7_Click(object sender, EventArgs e)
        {
            Bookings bookings = new Bookings();
            bookings.Show();
            this.Hide();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            //ON REDIRIGE a LA FENETRE DE CONNEXION
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
