using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrmBL.Model {
    public class CashDesk {

        CrmContext db;

        public int Number { get; set; }

        public Seller Seller { get; set; }

        public Queue<Cart> Queue { get; set; }

        public int MaxQueueLength { get; set; }

        public int ExitCustomer { get; set; }

        public bool isModel { get; set; }

        public int Count => Queue.Count;

        public event EventHandler<Check> CheckClosed;

        public CashDesk(int number, Seller seller, CrmContext db) {
            Number = number;
            Seller = seller;
            Queue = new Queue<Cart>();
            isModel = true;
            MaxQueueLength = 10;
            this.db = db ?? new CrmContext();
        }

        public void Enqueue(Cart cart) {
            if (Queue.Count < MaxQueueLength) {
                Queue.Enqueue(cart);
            }
            else {
                ExitCustomer++;
            }
        }

        public decimal Dequeue() {
            decimal sum = 0;

            if (Queue.Count == 0) return 0;

            var cart = Queue.Dequeue();
            if (cart != null) {
                var check = new Check() {
                    SellerId = Seller.SellerId,
                    Seller = Seller,
                    CustomerId = cart.Customer.CustomerId,
                    Customer = cart.Customer,
                    Created = DateTime.Now
                };

                if (!isModel) {
                    db.Checks.Add(check);
                    db.SaveChanges();
                }
                else {
                    check.CheckId = 0;
                }

                var sells = new List<Sell>();

                foreach (Product product in cart) {
                    if(product.Count > 0) {
                        var sell = new Sell() {
                            CheckId = check.CheckId,
                            Check = check,
                            ProductId = product.ProductId,
                            Product = product
                        };                    

                        sells.Add(sell);

                        if (!isModel) {
                            db.Sells.Add(sell);
                        }

                        product.Count--;
                        sum += product.Price;
                    }
                }

                check.Price = sum;

                if (!isModel) {
                    db.SaveChanges();
                }

                CheckClosed?.Invoke(this, check);
            }
            return sum;
        }

        public override string ToString() {
            return $"Касса №{Number}";
        }
    }
}
