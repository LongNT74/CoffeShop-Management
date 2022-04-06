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
    public partial class FrmMain : Form
    {
        CoffeeShopManagementContext context = new CoffeeShopManagementContext();
        static string abc="";
        public FrmMain(string text)
        {
            abc = text;
            InitializeComponent();
            LoadTable();
            LoadCategory();
        }

        private void logOutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void accountInformationToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            FrmProfile frmProfile = new FrmProfile(abc);
            frmProfile.ShowDialog();
        }

        private void shopManagementToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAdmin frmAdmin = new frmAdmin();
            frmAdmin.ShowDialog();
        }

        void ShowBill(int id)
        {
            lvBill.Clear();
            lvBill.Columns.Add("Food", 150);
            lvBill.Columns.Add("Quantity", 80);
            lvBill.Columns.Add("Unit Price", 90);
            lvBill.Columns.Add("Total Price", 100);
            lvBill.View = View.Details;
            double totalPrice = 0;
            var bill = (from c in context.Bills
                        join p in context.BillInfos on c.BillId equals p.IdBill
                        join e in context.Foods on p.IdFood equals e.FoodId
                        where c.IdTable == id 
                        where c.Status == 0
                        select new { e.FoodName, p.Quantity, e.Price, Total = p.Quantity*e.Price }).ToList();
            foreach (var item in bill)
            {
                String[] row = { item.FoodName, item.Quantity.ToString(), item.Price.ToString(), item.Total.ToString() };
                totalPrice += item.Price*item.Quantity;
                lvBill.Items.Add(new ListViewItem(row));

            }
            tbPrice.Text = totalPrice.ToString();
        }
        void LoadTable()
        {
            var table = (from c in context.TableFoods
                         select new { c.TableId, c.TableName, c.TableStatus }).ToList();
            foreach (var item in table)
            {
                Button btn = new Button() { Width = 120, Height = 120 };
                btn.Text = item.TableName +"\n"+ item.TableStatus;
                btn.Click += btn_Click;
                btn.Tag = item.TableId;
                flpTable.Controls.Add(btn);
                if (item.TableStatus == "Empty")
                {
                    btn.BackColor = Color.LightGreen;
                }
                else
                {
                    btn.BackColor = Color.Red;
                }
            }
        }
        int tableid = 0;
        private void btn_Click(object sender, EventArgs e)
        {
            int tableId = (int)(sender as Button).Tag;
            //lvBill.Tag = (sender as Button).Tag;
            tableid = tableId;
            ShowBill(tableId);
        }
        private void FrmMain_Load(object sender, EventArgs e)
        {
            using(var context = new CoffeeShopManagementContext())
            {
                Account acc = context.Accounts.Where(x => x.UserName == abc).FirstOrDefault();
                if(acc.Type ==0)
                {
                    adminToolStripMenuItem.Enabled = false;
                }
            }
        }
        void LoadCategory()
        {

            var category = context.FoodCategories.Select(p => new { p.CategoryId, p.CategoryName }).ToList();
            foreach (var item in category)
            {

                cbCategory.Items.Add(item.CategoryName);
            }

        }
        private void cbCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            //load product
            lvProductList.Clear();
            lvProductList.Columns.Add("ID", 70);
            lvProductList.Columns.Add("Product Name", 150);
            lvProductList.Columns.Add("Price", 70);
            lvProductList.View = View.Details;
            var index = cbCategory.SelectedIndex;

            var productList = (from a in context.Foods
                               where a.IdCategory == index+1
                               select new { a.FoodId, a.FoodName, a.Price }).ToList();

            foreach (var item in productList)
            {
                String[] row = { item.FoodId.ToString(), item.FoodName, item.Price.ToString() };
                lvProductList.Items.Add(new ListViewItem(row));
            }
        }
        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void btnAddFood_Click(object sender, EventArgs e)
        {
            Bill bill = new Bill();
            if (bill.Status == 1)
            {
                using (var context = new CoffeeShopManagementContext())
                {
                    bill.DateCheckIn = DateTime.Now;
                    bill.IdTable = tableid;
                    bill.Status = 0;
                    context.Bills.Add(bill);
                    context.SaveChanges();
                }
                using (var context = new CoffeeShopManagementContext())
                {
                    BillInfo billInfo = new BillInfo();
                    billInfo.IdBill = bill.BillId;
                    billInfo.IdFood = Convert.ToInt32(lvProductList.SelectedItems[0].Text); ;
                    billInfo.Quantity = Convert.ToInt32(Math.Round(numericUpDown1.Value, 0));
                    context.BillInfos.Add(billInfo);
                    context.SaveChanges();
                }
                using (var context = new CoffeeShopManagementContext())
                {
                    TableFood table = context.TableFoods.FirstOrDefault(x => x.TableId == tableid);
                    table.TableStatus = "Full";
                    context.SaveChanges();
                }
                
            }
            else if (bill.Status == 0)
            {
                using (var context = new CoffeeShopManagementContext())
                {
                    bill.DateCheckIn = DateTime.Now;
                    bill.IdTable = tableid;
                    bill.Status = 0;
                    context.Bills.Add(bill);
                    context.SaveChanges();
                }
                using (var context = new CoffeeShopManagementContext())
                {
                    BillInfo billInfo = new BillInfo();
                    billInfo.IdBill = bill.BillId;
                    billInfo.IdFood = Convert.ToInt32(lvProductList.SelectedItems[0].Text); ;
                    billInfo.Quantity = Convert.ToInt32(Math.Round(numericUpDown1.Value, 0));
                    context.BillInfos.Add(billInfo);
                    context.SaveChanges();
                }
                using (var context = new CoffeeShopManagementContext())
                {
                    TableFood table = context.TableFoods.FirstOrDefault(x => x.TableId == tableid);
                    table.TableStatus = "Full"; 
                    context.SaveChanges();
                }
                
            }
            ShowBill(tableid);
            //LoadTable();
        }

    }
}
