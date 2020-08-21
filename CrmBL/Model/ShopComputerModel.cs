using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CrmBL.Model {
    public class ShopComputerModel {
        Generator generator = new Generator();
        Random rnd = new Random();
        bool isWorking = false;
        List<Task> tasks = new List<Task>();
        CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
        CancellationToken token;

        public List<CashDesk> CashDesks { get; set; } = new List<CashDesk>();
        public List<Cart> Carts { get; set; } = new List<Cart>();
        public List<Check> Checks { get; set; } = new List<Check>();
        public List<Sell> Sells { get; set; } = new List<Sell>();
        public Queue<Seller> Sellers { get; set; } = new Queue<Seller>();
        public int CustomerSpeed { get; set; } = 100;
        public int CashDeskSpeed { get; set; } = 100;

        public ShopComputerModel() {

            var sellers = generator.GetNewSellers(20);
            generator.GetNewProducts(1000);
            generator.GetNewCustomers(100);
            token = cancelTokenSource.Token;

            foreach (var seller in sellers) {
                Sellers.Enqueue(seller);
            }

            for (int i = 0; i < 3; i++) {
                CashDesks.Add(new CashDesk(CashDesks.Count, Sellers.Dequeue(), null));
            }
        }

        public void Start() {            
            tasks.Add(Task.Run(() => CreateCarts(10, token)));
            tasks.AddRange(CashDesks.Select(c => new Task(() => CashDeskWork(c, token))));
            foreach(var task in tasks) {
                task.Start();
            }
        }

        public void Stop() {
            cancelTokenSource.Cancel();
        }

        private void CashDeskWork(CashDesk cashDesk, CancellationToken token) {
            while (isWorking) {
                if(cashDesk.Count > 0) {
                    cashDesk.Dequeue();
                    Thread.Sleep(CashDeskSpeed);
                }
            }
        }

        private void CreateCarts(int customerCounts, CancellationToken token) {
            while (!token.IsCancellationRequested) {
                var customers = generator.GetNewCustomers(customerCounts);
                var carts = new Queue<Cart>();

                foreach(var customer in customers) {
                    var cart = new Cart(customer);

                    foreach(var product in generator.GetRandomProducts(10, 30)) {
                        cart.Add(product);
                    }

                    var cash = CashDesks[rnd.Next(CashDesks.Count)];
                    cash.Enqueue(cart);
                }

                Thread.Sleep(CustomerSpeed);
            }
        }
    }
}
