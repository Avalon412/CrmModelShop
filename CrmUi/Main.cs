using CrmBL.Model;
using System;
using System.Windows.Forms;

namespace CrmUi {
    public partial class Main : Form {
        CrmContext db;

        public Main() {
            InitializeComponent();
            db = new CrmContext();
        }

        private void ProductToolStripMenuItem_Click(object sender, EventArgs e) {
            var catalogProduct = new Catalog<Product>(db.Products);
            catalogProduct.Show();
        }

        private void SellerToolStripMenuItem_Click(object sender, EventArgs e) {
            var catalogSeller = new Catalog<Seller>(db.Sellers);
            catalogSeller.Show();
        }

        private void CustomerToolStripMenuItem_Click(object sender, EventArgs e) {
            var catalogCustomer = new Catalog<Customer>(db.Customers);
            catalogCustomer.Show();
        }

        private void CheckToolStripMenuItem_Click(object sender, EventArgs e) {
            var catalogCheck = new Catalog<Check>(db.Checks);
            catalogCheck.Show();
        }

        private void СustomerAddToolStripMenuItem2_Click(object sender, EventArgs e) {
            var form = new CustomerForm();
            if(form.ShowDialog() == DialogResult.OK) {
                db.Customers.Add(form.Customer);
                db.SaveChanges();
            }
        }
    }
}
