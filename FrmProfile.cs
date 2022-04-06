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
    public partial class FrmProfile : Form
    {
        static string abc="";
        public FrmProfile(string text)
        {
            abc = text;
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void FrmProfile_Load(object sender, EventArgs e)
        {
            tbUser.Enabled=false;
            using (var context = new CoffeeShopManagementContext())
            {
                Account account = context.Accounts.Where(x => x.UserName == abc).FirstOrDefault();
                tbUser.Text = account.UserName;
                tbFullName.Text = account.FullName;
                tbDob.Text = account.DateOfBirth;
                if (account.Gender ==true)
                {
                    rbMale.Checked=true;
                }
                else
                {
                    rbFemale.Checked =true;
                }

                tbPhone.Text = account.PhoneNumber;
                tbAddress.Text = account.Address;
            }    
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            string account = tbUser.Text;
            string name = tbFullName.Text;
            string dob = tbDob.Text;
            bool check;
            if (rbMale.Checked == true)
            {
                check = true;
            }
            else
            {
                check = false;
            }
            string phone = tbPhone.Text;
            string address = tbAddress.Text;
            string pass = tbPass.Text;
            string rePass = tbRePass.Text;

            if(pass==rePass)
            {
                using (var context = new CoffeeShopManagementContext())
                {
                    Account account1 = context.Accounts.Where(x => x.UserName == tbUser.Text).FirstOrDefault();
                    account1.FullName = name;
                    account1.DateOfBirth = dob;
                    account1.Gender = check;
                    account1.PhoneNumber = phone;
                    account1.Address = address;
                    account1.PassWord = rePass;
                    context.SaveChanges();
                }
                MessageBox.Show("success");
                this.Hide();
            }
            else
            {
                MessageBox.Show("Password not equal");
            }
          
        }
       
    }
}
