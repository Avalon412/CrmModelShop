﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrmBL.Model {
    public class CashDesk {

        CrmContext db = new CrmContext();

        public int Number { get; set; }

        public Seller Seller { get; set; }

        public Queue<Cart> Queue { get; set; }

        public int MaxQueueLength { get; set; }

        public int ExitCustomer { get; set; }

        public bool isModel { get; set; }

        public CashDesk(int number, Seller seller) {
            Number = number;
            Seller = seller;
            Queue = new Queue<Cart>();
            isModel = true;
        }

        public void Enqueue(Cart cart) {
            if (Queue.Count <= MaxQueueLength) {
                Queue.Enqueue(cart);
            }
            else {
                ExitCustomer++;
            }
        }

        public decimal Dequeue() {
            decimal sum = 0;
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

                if (!isModel) {
                    db.SaveChanges();
                }
            }

            return sum;
        }
    }
}