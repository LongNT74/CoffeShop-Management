using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CoffeeShopManagement.Models;

namespace CoffeeShopManagement
{
    public partial class FrmLogin : Form
    {
        public FrmLogin()
        {
            InitializeComponent();
        }

        private void btnSignUp_Click(object sender, EventArgs e)
        {
            this.Hide();
            FrmSignUp frmSignUp = new FrmSignUp();
            frmSignUp.ShowDialog();
            this.Close();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (context.Accounts.Where(c => c.UserName == tbUserName.Text
            && c.PassWord == tbPass.Text).Count() > 0)
            {
                this.Hide();
                FrmMain frmMain = new FrmMain(tbUserName.Text);
                frmMain.ShowDialog();
                this.Close();
            }
            else
            {
                MessageBox.Show("Wrong username or password!");
            }
        }

        private void FrmLogin_Load(object sender, EventArgs e)
        {

        }
        CoffeeShopManagementContext context = new CoffeeShopManagementContext();
    }
}
