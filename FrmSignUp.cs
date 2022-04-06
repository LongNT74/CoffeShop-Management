using CoffeeShopManagement.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CoffeeShopManagement
{
    public partial class FrmSignUp : Form
    {
        public FrmSignUp()
        {
            InitializeComponent();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Hide();
            FrmLogin frmLogin = new FrmLogin();
            frmLogin.ShowDialog();
            this.Close();
        }
        CoffeeShopManagementContext context = new CoffeeShopManagementContext();
        private void btnSignUp_Click(object sender, EventArgs e)
        {
            if (validation())
            {
                try
                {
                    Account acc = new Account();
                    acc.UserName = tbUser.Text;
                    acc.FullName = tbFullName.Text;
                    acc.DateOfBirth = mtbDob.Text;
                    acc.PhoneNumber = mtbPhone.Text;
                    acc.Gender = rbMale.Checked ? true : false;
                    acc.Address = tbAddress.Text;
                    acc.PassWord = tbPass.Text;
                    context.Accounts.Add(acc);
                    context.SaveChanges();
                    MessageBox.Show("add account success!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

            }
            else
            {
                MessageBox.Show("signup failed");
            }
        }
        bool validation()
        {
            bool flag = true;
            string error = "";
            Regex regName = new Regex(@"^[a-zA-Z\s]+$");
            //validate username
            if (tbUser.Text == "")
            {
                flag = false;
                error += "User name is not empty!\n";
                tbUser.Focus();
            }
            //valid fullname

            if (!regName.IsMatch(tbFullName.Text))
            {
                flag = false;
                error += "fullname invalid!\n";
                tbFullName.Focus();
            }
            //validate dateofbirth
            Regex regDate = new Regex(@"(((0|1)[0-9]|2[0-9]|3[0-1])\/(0[1-9]|1[0-2])\/((19|20)\d\d))$");
            if (!regDate.IsMatch(mtbDob.Text))
            {
                flag = false;
                error += "date of birth is not empty!\n";
                mtbDob.Focus();
            }
            //validate gender
            if (!rbMale.Checked && !rbFemale.Checked)
            {
                flag = false;
                error += "Gender is not empty!\n";

            }
            Regex regPhone = new Regex(@"^(0?)(3[2-9]|5[6|8|9]|7[0|6-9]|8[0-6|8|9]|9[0-4|6-9])[0-9]{7}$");
            //validate phone
            if (!regPhone.IsMatch(mtbPhone.Text))
            {
                flag = false;
                error += "Phone invalid (format 09xxxxxxxx)!\n";
                mtbPhone.Focus();
            }
            //validate address
            if (tbAddress.Text == "")
            {
                flag = false;
                error += "Address is not empty!\n";
                tbAddress.Focus();
            }
            //validate password
            if (tbPass.Text == "")
            {
                flag = false;
                error += "Password is not empty!\n";
                tbPass.Focus();
            }
            //validate repass
            if (tbRePass.Text == "")
            {
                flag = false;
                error += "Re-Password is not empty!\n";
                tbRePass.Focus();
            }
            //if pass not equal repass
            if (!tbPass.Text.Equals(tbRePass.Text))
            {
                flag = false;
                error += "Password and Re-Password is not match!\n";
            }

            if (flag == false)
            {
                MessageBox.Show(error);
            }

            return flag;
        }
    }
}
