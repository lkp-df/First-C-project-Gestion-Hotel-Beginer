using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyHotel
{
    public partial class Types : Form
    {
        //LE CONSTRUCTEUR
        public Types()
        {
            InitializeComponent();
            //A L'APPEL DE CETTE FENETRE, ON AFFICHE LES DONNEES PRESENTE
            popullate();
        }

        //methode d'action sur la base de données
        /**** ETABLISSEMNT VARIABLE DE CONNEXION A LA BASE DE DONNES ********/
        SqlConnection Con = new SqlConnection(@"Data Source=DESKTOP-DD2QERU;Initial Catalog=HotelDatabase;Integrated Security=True;Pooling=False");

        private void InsertCategories()
        {
            //VERIFICATION QUE UNE INFORMATION EST FOURNIE POUR TOUS LES CHAMPS 
            if (TypeNameTb.Text == "" || TypeCostTb.Text == "")
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
                    SqlCommand sql = new SqlCommand("insert into Type(TypeName,TypeCost) " +
                                                     "values(@TN,@TC) ", Con);
                    //BINDING DES VALUES
                    sql.Parameters.AddWithValue("@TN", TypeNameTb.Text);
                    sql.Parameters.AddWithValue("@TC", TypeCostTb.Text);

                    //EXECUTION DE LA REQUETE
                    int nbreLigne = sql.ExecuteNonQuery();

                    //FERMETURE DE CONNEXION A LA BASE DE DONNEES
                    Con.Close();

                    if (nbreLigne > 0)
                    {
                        MessageBox.Show("Rooms Type Are Added", "Successfully", MessageBoxButtons.OK);

                        //ON AFFICHE L'ELEMENT INSERE
                        popullate();
                    }

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
                string Query = "select * from  Type ";
                SqlDataAdapter sda = new SqlDataAdapter(Query, Con);
                SqlCommandBuilder Builder = new SqlCommandBuilder(sda);

                //CREATION DU CACHE DES DONNEES EN MEMOIRE
                var ds = new DataSet();

                //REMPLISSAGE DES LIGNES DANS LE DATASET
                sda.Fill(ds);

                //AFFICHAGE DES CATEGORIES DANS NOTRE TYPESDGVIEW, pour plus d'info mettre le cursuer sur le nom des elements utilises
                TypesDGView.DataSource = ds.Tables[0];

                //FERMETURE DE CONNEXION A LA BASE DE DONNEES
                Con.Close();
            }
            catch (Exception e)
            {
                //ON LEVE L'EXCEPTION ON AFFICHANT LE MESSAGE D'ERREUR
                MessageBox.Show(e.Message);
            }
        }

        //MODIFIER LA DONNES DE LA TABLE
        private void EditCategorie()
        {
            //VERIFICATION QUE UNE INFORMATION EST FOURNIE POUR TOUS LES CHAMPS 
            if (TypeNameTb.Text == "" || TypeCostTb.Text == "")
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
                    SqlCommand sql = new SqlCommand("update Type set  TypeName = @TN, TypeCost = @TC where TypeNum = @RKEY", Con);

                    //BINDING DES VALUES
                    sql.Parameters.AddWithValue("@TN", TypeNameTb.Text);
                    sql.Parameters.AddWithValue("@TC", TypeCostTb.Text);
                    sql.Parameters.AddWithValue("@RKEY", key);

                    //EXECUTION DE LA REQUETE
                    int nbreLigne = sql.ExecuteNonQuery();

                    //FERMETURE DE CONNEXION A LA BASE DE DONNEES
                    Con.Close();

                    if (nbreLigne > 0)
                    {
                        MessageBox.Show("Type  Updated", "Successfully", MessageBoxButtons.OK);

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
        private void DeleteTypes()
        {
            //VERIFICATION QUE UN CHAMP DE LA GRILLE EST SELECTIONNEE 
            if (this.key == 0)
            {
                MessageBox.Show("You Should Select A Categorie", "Select A Field To Delete", MessageBoxButtons.OK);
            }
            else
            {
                try
                {
                    //OUVERTURE DE CONNEXION
                    Con.Open();

                    //SQL REQUETE
                    SqlCommand sql = new SqlCommand("delete from Type where TypeNum = @RKEY", Con);

                    //BINDING DES VALUES
                    sql.Parameters.AddWithValue("@RKEY", key);

                    //AVERTISSEMENT DE SUPPRESSION
                    DialogResult dr = MessageBox.Show("Are you Sure To Deleted this Categorie ", "Attention !!!", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);

                    if (dr == DialogResult.OK)
                    {
                        //EXECUTION DE LA REQUETE
                        int nbreLigne = sql.ExecuteNonQuery();

                        //FERMETURE DE CONNEXION A LA BASE DE DONNEES
                        Con.Close();

                        if (nbreLigne > 0)
                        {
                            MessageBox.Show("Type  Deleted", "Operation Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            //ON VIDE LES CHAMPS
                            TypeNameTb.Text = "";
                            TypeCostTb.Text = "";

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
            this.InsertCategories();
        }

        //OUVRIR LA FENETRE DES ROOMS (CATEGRORE) SUR UN CLICK : ROOMS EST UN OBJET
        private void label3_Click(object sender, EventArgs e)
        {
            Rooms rooms = new Rooms();
            rooms.Show();
            //ON CACHE LA FENETRE COURANTE
            this.Hide();
        }

        int key = 0;
        private void TypesDGView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //ON RECUPERE LE CONTENU  DES COLONNES DE NOTRE GRILLE
            //CE SERA POUR L EDITION DES DONNEES, ON LES REAFFICHE DANS LES CHAMPS  POUR LA MODIIFCATION
            TypeNameTb.Text = TypesDGView.SelectedRows[0].Cells[1].Value.ToString();
            TypeCostTb.Text = TypesDGView.SelectedRows[0].Cells[2].Value.ToString();

            if (TypeNameTb.Text == "")
            {
                this.key = 0;
            }
            else
            {
                //CE SERA POUR LA RECUPERATION DE L'ID DE L'ELEMENT 0A MODIFIER : la colonne 0 comprend l'id c'est pour quoi
                this.key = Convert.ToInt32(TypesDGView.SelectedRows[0].Cells[0].Value.ToString());
            }
        }

        private void EditBtn_Click(object sender, EventArgs e)
        {
            this.EditCategorie();
        }

        private void DeleteBtn_Click(object sender, EventArgs e)
        {
            DeleteTypes();
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
