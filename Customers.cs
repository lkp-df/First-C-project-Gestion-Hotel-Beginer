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
    public partial class Customers : Form
    {
        public Customers()
        {
            InitializeComponent();
            this.popullate();
        }

        //methode d'action sur la base de données
        /**** ETABLISSEMNT VARIABLE DE CONNEXION A LA BASE DE DONNES ********/
        SqlConnection Con = new SqlConnection("Data Source=DESKTOP-DD2QERU;Initial Catalog=HotelDatabase;Integrated Security=True;Pooling=False");

        private void InsertCustumers()
        {
            //VERIFICATION QUE UNE INFORMATION EST FOURNIE POUR TOUS LES CHAMPS 
            if (CustnameTb.Text == "" || CustGenderCb.SelectedIndex == -1 ||
                    CustphoneTb.Text == ""
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
                    SqlCommand sql = new SqlCommand("insert into Custumer (CustName,CustPhone,CustGender) values(@CN,@CPH,@CG) ", Con);

                    //BINDING DES VALUES
                    sql.Parameters.AddWithValue("@CN", CustnameTb.Text);
                    sql.Parameters.AddWithValue("@CPH", CustphoneTb.Text);
                    sql.Parameters.AddWithValue("@CG", CustGenderCb.SelectedItem.ToString());

                    //EXECUTION DE LA REQUETE
                    int nbreLigne = sql.ExecuteNonQuery();

                    //FERMETURE DE CONNEXION A LA BASE DE DONNEES
                    Con.Close();

                    if (nbreLigne > 0)
                    {
                        MessageBox.Show("Customer  Are Added", "Successfully", MessageBoxButtons.OK);

                        //on vide les champs
                        CustnameTb.Text = "";
                        CustGenderCb.SelectedIndex = 0;
                        CustphoneTb.Text = "";

                        //FERMETURE DE CONNEXION A LA BASE DE DONNEES
                        Con.Close();

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
                string Query = "select * from Custumer";
                SqlDataAdapter sda = new SqlDataAdapter(Query, Con);
                SqlCommandBuilder Builder = new SqlCommandBuilder(sda);

                //CREATION DU CACHE DES DONNEES EN MEMOIRE
                var ds = new DataSet();

                //REMPLISSAGE DES LIGNES DANS LE DATASET
                sda.Fill(ds);

                //AFFICHAGE DES CATEGORIES DANS NOTRE TYPESDGVIEW, pour plus d'info mettre le cursuer sur le nom des elements utilises
                CustumersDGView.DataSource = ds.Tables[0];

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

        //MODIFIER LA DONNES DE LA TABLE
        private void EditCustumer()
        {
            //VERIFICATION QUE UNE INFORMATION EST FOURNIE POUR TOUS LES CHAMPS 
            if (CustnameTb.Text == "" || CustphoneTb.Text == "" ||
                CustGenderCb.SelectedIndex == -1)
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
                    SqlCommand sql = new SqlCommand("update Custumer set  CustName = @CN, CustPhone = @CPH, CustGender = @CG where CustNum = @CKEY", Con);

                    //BINDING DES VALUES
                    sql.Parameters.AddWithValue("@CN", CustnameTb.Text);
                    sql.Parameters.AddWithValue("@CPH", CustphoneTb.Text);
                    sql.Parameters.AddWithValue("@CG", CustGenderCb.SelectedItem.ToString());
                    sql.Parameters.AddWithValue("@CKEY", key);

                    //EXECUTION DE LA REQUETE
                    int nbreLigne = sql.ExecuteNonQuery();

                    //FERMETURE DE CONNEXION A LA BASE DE DONNEES
                    Con.Close();

                    if (nbreLigne > 0)
                    {
                        MessageBox.Show("Customer  Updated", "Successfully", MessageBoxButtons.OK);

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
        private void DeleteCustumer()
        {
            //VERIFICATION QUE UN CHAMP DE LA GRILLE EST SELECTIONNEE 
            if (this.key == 0)
            {
                MessageBox.Show("You Should Select A User", "Select A Field To Delete", MessageBoxButtons.OK);
            }
            else
            {
                try
                {
                    //OUVERTURE DE CONNEXION
                    Con.Open();

                    //SQL REQUETE
                    SqlCommand sql = new SqlCommand("delete from Custumer where CustNum = @CKEY", Con);

                    //BINDING DES VALUES
                    sql.Parameters.AddWithValue("@CKEY", key);

                    //AVERTISSEMENT DE SUPPRESSION
                    DialogResult dr = MessageBox.Show("Are you Sure To Deleted this Customer ", "Attention !!!", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);

                    if (dr == DialogResult.OK)
                    {
                        //EXECUTION DE LA REQUETE
                        int nbreLigne = sql.ExecuteNonQuery();

                        //FERMETURE DE CONNEXION A LA BASE DE DONNEES
                        Con.Close();

                        if (nbreLigne > 0)
                        {
                            MessageBox.Show("Customer  Deleted", "Operation Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            //ON VIDE LES CHAMPS
                            CustnameTb.Text = "";
                            CustphoneTb.Text = "";


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
        //OUVERTURE DES FENETRES
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

        private void label7_Click(object sender, EventArgs e)
        {
            Bookings bookings = new Bookings();
            bookings.Show();
            this.Hide();
        }

        private void SaveBtn_Click(object sender, EventArgs e)
        {
            InsertCustumers();
        }

        int key = 0;
        private void CustumersDGView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            CustnameTb.Text = CustumersDGView.SelectedRows[0].Cells[1].Value.ToString();
            CustphoneTb.Text = CustumersDGView.SelectedRows[0].Cells[2].Value.ToString();
            CustGenderCb.Text = CustumersDGView.SelectedRows[0].Cells[3].Value.ToString();

            if (CustnameTb.Text == "" || CustphoneTb.Text == "" || CustGenderCb.Text == "")
            {
                key = 0;
            }
            else
            {
                key = Convert.ToInt32(CustumersDGView.SelectedRows[0].Cells[0].Value.ToString());
            }
        }

        private void EditBtn_Click(object sender, EventArgs e)
        {
            EditCustumer();
        }

        private void DeleteBtn_Click(object sender, EventArgs e)
        {
            DeleteCustumer();
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
