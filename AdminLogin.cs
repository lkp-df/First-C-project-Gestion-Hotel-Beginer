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
    public partial class AdminLogin : Form
    {
        public AdminLogin()
        {
            InitializeComponent();
        }

        //methode d'action sur la base de données
        /**** ETABLISSEMNT VARIABLE DE CONNEXION A LA BASE DE DONNES ********/
        SqlConnection Con = new SqlConnection("Data Source=DESKTOP-DD2QERU;Initial Catalog=HotelDatabase;Integrated Security=True;Pooling=False");

        private void AdminConnection()
        {
            try
            {
                if (UpasswdTb.Text == "")
                {
                    MessageBox.Show("You Should Enter A Admin Pasword", "Password Fiels Is Require", MessageBoxButtons.OK);
                }
                else
                {

                    if (UpasswdTb.Text == "Admin")
                    {
                        //ON A PAS DE TABLE ADMIN, DONC ON SUPPOSE QUE LE USER NE CONNAIT PAS CELA ET QUE NOUS SOMMES LE SEUL, ON A PRIS Admin COMME MOT DE PASSE
                        Users users = new Users();
                        users.Show();
                        this.Hide();
                    }
                    else
                    {
                        MessageBox.Show("Password Invalid", "Incorrect Data", MessageBoxButtons.OK);
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private void CancelLbl_Click(object sender, EventArgs e)
        {
            Login login = new Login();
            login.Show();
            this.Hide();
        }

        private void ClosePic_Click(object sender, EventArgs e)
        {
            //ON FERME L'APPLICATION
            Application.Exit();
        }

        private void LoginBtn_Click(object sender, EventArgs e)
        {
            AdminConnection();
        }
    }
}
