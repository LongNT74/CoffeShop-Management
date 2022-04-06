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
    public partial class frmAdmin : Form
    {
        CoffeeShopManagementContext db;
        public frmAdmin()
        {
            InitializeComponent();
            db= new CoffeeShopManagementContext();
            loadcbCategory();
            load(null);
        }
        void load(string key)
        {
            if (key == null)
            {
                dgvListFoods.DataSource = (from f in db.Foods
                                           join fc in db.FoodCategories on f.IdCategory equals fc.CategoryId
                                           select new { f.FoodId, f.FoodName, fc.CategoryName, f.Price }).ToList();
            }
            else
            {
                dgvListFoods.DataSource = (from f in db.Foods
                                           join fc in db.FoodCategories on f.IdCategory equals fc.CategoryId
                                           where f.FoodName.Contains(key)
                                           select new { f.FoodId, f.FoodName, fc.CategoryName, f.Price }).ToList();
            }
            dgvListFoods.ClearSelection();

        }
        private void dgvListFoods_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                var selected = dgvListFoods.Rows[e.RowIndex];

                tbId.Text = selected.Cells[0].Value.ToString();
                tbName.Text = selected.Cells[1].Value.ToString();
                nbPrice.Value = Convert.ToDecimal(selected.Cells[3].Value.ToString());
                cbCategory.SelectedIndex = cbCategory.FindString(selected.Cells[2].Value.ToString());
            }
            catch (Exception ex)
            {

            }
        }
        void loadcbCategory()
        {
            cbCategory.Items.Clear();
            cbCategory.DataSource = db.FoodCategories.ToList();
            cbCategory.DisplayMember = "categoryName";
            cbCategory.ValueMember = "categoryId";
        }
        private void btnSearch_Click(object sender, EventArgs e)
        {
            load(tbSearch.Text);
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            Food food = new Food();
            food.IdCategory = (int)cbCategory.SelectedValue;
            food.FoodName = tbName.Text;
            food.Price = (double)nbPrice.Value;
            db.Add(food);
            db.SaveChanges();
            MessageBox.Show("add food succesffully !");
            load(tbSearch.Text);
        }
        private void btnEdit_Click(object sender, EventArgs e)
        {
            Food food = db.Foods.Find(Convert.ToInt32(tbId.Text));
            food.IdCategory = (int)cbCategory.SelectedValue;
            food.FoodName = tbName.Text;
            food.Price = (double)nbPrice.Value;
            db.Update(food);
            db.SaveChanges();
            MessageBox.Show("update food succesffully !");
            load(tbSearch.Text);
        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show($"Do you want to remove {tbName.Text}?", "Confirmation", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                try
                {
                    db.Foods.Remove(db.Foods.Find(Convert.ToInt32(tbId.Text)));
                    db.SaveChanges();
                    load(tbSearch.Text);
                }
                catch
                {
                    MessageBox.Show("Cannot remove this food !");
                }
            }
        }
        private void btnView_Click(object sender, EventArgs e)
        {
            //where b.IdBillNavigation.DateCheckIn >= dtpFromDate.Value && b.IdBillNavigation.DateCheckOut<=dtpToDate.Value select new {b.IdFoodNavigation.}
            dgvSales.DataSource = (from bi in db.BillInfos
                                   join b in db.Bills on bi.IdBill equals b.BillId
                                   join tf in db.TableFoods on b.IdTable equals tf.TableId
                                   join f in db.Foods on bi.IdFood equals f.FoodId
                                   where b.DateCheckIn >= dtpFromDate.Value && b.DateCheckOut <= dtpToDate.Value
                                   select new { tf.TableName, b.DateCheckIn, b.DateCheckOut, Total = f.Price*bi.Quantity }
                                   ).ToList();
        }
        private void tabPage3_Click(object sender, EventArgs e)
        {
            if(tabControl1.SelectedIndex==2)
            {
                loadDataCategory();
            }    
            else if(tabControl1.SelectedIndex==3)
            {
                loadDataAccount();
            }    
        }
        public void loadDataCategory()
        {
            dgvCategory.Rows.Clear();
            using (var context = new CoffeeShopManagementContext())
            {
                List<FoodCategory> list = context.FoodCategories.ToList();
                foreach (FoodCategory item in list)
                {
                    dgvCategory.Rows.Add(item.CategoryId, item.CategoryName);
                }
            }
        }
        public void loadDataAccount()
        {
            dgvAccount.Rows.Clear();
            using (var context = new CoffeeShopManagementContext())
            {
                List<Account> list = context.Accounts.ToList();
                foreach (Account item in list)
                {
                    dgvAccount.Rows.Add(item.UserName, item.FullName,item.DateOfBirth,item.Gender,item.PhoneNumber,item.Address);
                }
            }
        }

        private void dgvCategory_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            tbCategoryId.Text = dgvCategory.Rows[e.RowIndex].Cells["Column1"].Value.ToString();
            tbCategoryName.Text = dgvCategory.Rows[e.RowIndex].Cells["Column2"].Value.ToString();
        }

        private void dgvAccount_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        private void dgvAccount_CellClick(object sender, DataGridViewCellEventArgs e)
        {          
            tbAccount.Text = dgvAccount.Rows[e.RowIndex].Cells["Column3"].Value.ToString();
            tbUsername.Text = dgvAccount.Rows[e.RowIndex].Cells["Column4"].Value.ToString();
            tbDob.Text = dgvAccount.Rows[e.RowIndex].Cells["Column5"].Value.ToString();
            bool gender = bool.Parse(dgvAccount.Rows[e.RowIndex].Cells["Column6"].Value.ToString());
            if (gender == true)
            {
                tbGender.Text ="male";
            }
            else
            {
                tbGender.Text = "female";
            }
            tbPhone.Text = dgvAccount.Rows[e.RowIndex].Cells["Column7"].Value.ToString();
            tbAddress.Text = dgvAccount.Rows[e.RowIndex].Cells["Column8"].Value.ToString();

        }
        private void btnAddCategory_Click(object sender, EventArgs e)
        {
            string nameCate = tbCategoryName.Text;
            using (var context = new CoffeeShopManagementContext())
            {
                FoodCategory category = new FoodCategory();
                category.CategoryName = nameCate;
                context.FoodCategories.Add(category);
                context.SaveChanges();
            }
            loadDataCategory();

        }
        private void btnDeleteUser_Click(object sender, EventArgs e)
        {
            using (var context = new CoffeeShopManagementContext())
            {
                Account account = context.Accounts.Where(x => x.UserName == tbAccount.Text).FirstOrDefault();
                context.Accounts.Remove(account);
                context.SaveChanges();
            }
            loadDataAccount();
        }

        private void tbCategoryId_TextChanged(object sender, EventArgs e)
        {
        }

        private void btnEditCategory_Click(object sender, EventArgs e)
        {
            string idCate = tbCategoryId.Text;
            string nameCate = tbCategoryName.Text;
            using (var context = new CoffeeShopManagementContext())
            {
                FoodCategory category = context.FoodCategories.Where(x => x.CategoryId == int.Parse(tbCategoryId.Text)).FirstOrDefault();
                category.CategoryName = nameCate;
                context.SaveChanges();
            }
            loadDataCategory();
        }

        private void dgvListFoods_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                var selected = dgvListFoods.Rows[e.RowIndex];

                tbId.Text = selected.Cells[0].Value.ToString();
                tbName.Text = selected.Cells[1].Value.ToString();
                nbPrice.Value = Convert.ToDecimal(selected.Cells[3].Value.ToString());
                cbCategory.SelectedIndex = cbCategory.FindString(selected.Cells[2].Value.ToString());
            }
            catch (Exception ex)
            {

            }
        }
    }
}
