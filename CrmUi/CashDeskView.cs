using CrmBL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CrmUi {
    class CashDeskView {
        CashDesk cashDesk;
        public Label CashDeskName { get; set; }
        public NumericUpDown Price { get; set; }
        public ProgressBar QueueLength { get; set; }
        public Label LeaveCustomersCount { get; set; }
        public CashDeskView(CashDesk cashDesk, int number, int x, int y) {
            this.cashDesk = cashDesk;
            CashDeskName = new Label();
            Price = new NumericUpDown();
            QueueLength = new ProgressBar();
            LeaveCustomersCount = new Label();
            //
            // CashDeskName
            //
            CashDeskName.AutoSize = true;
            CashDeskName.Location = new System.Drawing.Point(x, y);
            CashDeskName.Name = "label1" + number;
            CashDeskName.Size = new System.Drawing.Size(35, 13);
            CashDeskName.TabIndex = number;
            CashDeskName.Text = cashDesk.ToString();
            //
            // LeaveCustomersCount
            //
            LeaveCustomersCount.AutoSize = true;
            LeaveCustomersCount.Location = new System.Drawing.Point(x + 400, y);
            LeaveCustomersCount.Name = "label2" + number;
            LeaveCustomersCount.Size = new System.Drawing.Size(35, 13);
            LeaveCustomersCount.TabIndex = number;
            LeaveCustomersCount.Text = "";
            // 
            // Price
            // 
            Price.Location = new System.Drawing.Point(x + 70, y);
            Price.Name = "numericUpDown1" + number;
            Price.Size = new System.Drawing.Size(120, 20);
            Price.TabIndex = number;
            Price.Maximum = 1000000000000;
            //
            // QueueLength
            //
            QueueLength.Location = new System.Drawing.Point(x + 250, y);
            QueueLength.Maximum = cashDesk.MaxQueueLength;
            QueueLength.Name = "progressBar1" + number;
            QueueLength.Size = new System.Drawing.Size(100, 23);
            QueueLength.TabIndex = number;
            QueueLength.Value = 0;

            cashDesk.CheckClosed += CashDesk_CheckClosed;
        }

        private void CashDesk_CheckClosed(object sender, Check e) {
            if(!Price.IsDisposed) {
                Price.Invoke((Action)delegate {
                    Price.Value += e.Price;
                    QueueLength.Value = cashDesk.Count;
                    LeaveCustomersCount.Text = cashDesk.ExitCustomer.ToString();
                });
            }
        }
    }
}
