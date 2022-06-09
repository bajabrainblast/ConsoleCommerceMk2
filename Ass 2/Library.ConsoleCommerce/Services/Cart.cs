using Library.ConsoleCommerce.Models;
using Library.ConsoleCommerce.Utility;
using Library.ConsoleCommerce.Imports;
using Newtonsoft.Json;

namespace Library.ConsoleCommerce.Services
{
    internal class Cart
    {
        const double SALES_TAX = 0.07;
        private Inventory inventory;
        private List<Product> productsList;
        public List<Product> Products {   get { return productsList; }    }

        public Cart(Inventory _inventory)
        {
            productsList = new List<Product>();
            inventory = _inventory;
        }

        //assumes valid id and quantity. adds an item from inv to productslist 
        public void AddItem(int itemId, double amtAdded)
        {
            var item = inventory.Products.FirstOrDefault(p => p.Id == itemId);
            if (item == null)
                return;

            bool doesntExist = true;
            
            foreach (Product prod in productsList)      // see if the item was already added
                if (prod.Id == item.Id)                 // if it was already added, add to the stock
                { 
                    doesntExist = false;

                    // add to stock
                    if (prod is ProductByQuantity)
                        (prod as ProductByQuantity).Quantity += (int)amtAdded;
                    else if (prod is ProductByWeight)
                        (prod as ProductByWeight).Weight += amtAdded;

                    return;     // jobs done
                }

            // if not, add the product to cart
            if (doesntExist)
            {
                if (item is ProductByQuantity)
                {
                    productsList.Add(new ProductByQuantity(item, (int)amtAdded));
                    (item as ProductByQuantity).Quantity -= (int)amtAdded;      
                }
                else if (item is ProductByWeight)
                {
                    productsList.Add(new ProductByWeight(item, amtAdded));
                    (item as ProductByWeight).Weight -= amtAdded;   // and sub from inventory
                }
            } 
        }
        
        // assumes valid id, removes item from productslist, adds back to inv
        public void RemoveItem(int itemId)
        {
            var returnItem = productsList.FirstOrDefault(p => p.Id == itemId);
            var stock = inventory.Products.FirstOrDefault(p => p.Id == itemId);

            // null catching
            if (returnItem == null)
                return;
            if (stock == null) {
                inventory.Products.Add(returnItem);
                return;
            }

            if (stock is ProductByQuantity)
                (stock as ProductByQuantity).Quantity += (returnItem as ProductByQuantity).Quantity;
            else if (stock is ProductByWeight)
                (stock as ProductByWeight).Weight += (returnItem as ProductByWeight).Weight;

            productsList.Remove(returnItem);
        }

        public void Display()
        {
            if (productsList.Count() == 0)
            {
                Console.WriteLine("The inventory is empty...");
                return;
            }

            // loop as long as they want, with the opportunity to sort beforehand
            while (Utilities.NavigateList(SortedList()))
                ; // just let it loop


        }

        public IEnumerable<Product> SortedList()
        {
            Console.WriteLine("Would you like to sort by..." +
                "\n\t(1) Name" +
                "\n\t(2) Total Price" +
                "\n\t(3) Id" +
                "\n\t(0) Cancel sort");
            switch (Utilities.TakeInput(0, 3))
            {
                case 1: // sort by name
                    return productsList.OrderBy(x => x.Name);

                case 2: // sort by price
                    return productsList.OrderBy(x => x.TotalPrice);

                case 3: // sort by id
                    return productsList.OrderBy(x => x.Id);

                default:
                    return productsList;
            }
        }

        public void CheckOut()
        {
            var total = productsList.Sum(p => p.TotalPrice);
            Console.WriteLine($"Subtotal: ${total}");
            var tax = total * SALES_TAX;
            Console.WriteLine($"Tax: ${tax}");

            Console.WriteLine($"Total: ${total + tax}\n\tThank you for shopping with us.");
            
            CollectPayment();
            CollectAddress();

            Console.WriteLine("Your products are on the way!");
        }

        public void Save()
        {
            var json = JsonConvert.SerializeObject(productsList,
                new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All});
            File.WriteAllText("CartData.json", json);
        }
        public void Load()
        {
            var json = File.ReadAllText("CartData.json");
            productsList = JsonConvert.DeserializeObject<List<Product>>(json,
                new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All}) 
                ?? new List<Product>();
        }

        // search by name
        public void SearchName(string search)
        {
            Utilities.NavigateList(productsList.Where(i => i.Name.Contains(search)));
        }

        // search via description
        public void SearchDescription(string search)
        {
            Utilities.NavigateList(productsList.Where(i => i.Description.Contains(search)));
        }

        // search by name or desc
        public void Search(string search)
        {
            Utilities.NavigateList(productsList.Where(i => i.Description.Contains(search) || i.Name.Contains(search)));
        }

        // not actually used lol
        public override string ToString()
        {
            string temp = "";

            foreach (Product prod in productsList)
            {
                temp += prod.ToString();
                temp += '\n';
            }

            return temp;
        }

        internal void CollectPayment()
        {
            string CCNum, Pin, Exp, FullName;

            do
            {
                Console.Write("Please type your credit card number -> ");
                CCNum = Console.ReadLine() ?? "INVALID";
                Console.Write("Please type your pin -> ");
                Pin = Console.ReadLine() ?? "INVALID";
                Console.Write("Please type your expiration date -> ");
                Exp = Console.ReadLine() ?? "INVALID";
                Console.Write("Please type your full name -> ");
                FullName = Console.ReadLine() ?? "INVALID";

                Console.WriteLine($"Summary:\n" +
                    $"\t{FullName}, #{CCNum}" +
                    $"\tExpiration: {Exp}, Pin: {Pin}");

                Console.WriteLine("Is that correct?\n\t(1) Yes\n\t(2) No");
            } while (Utilities.TakeInput(1, 2) != 1);
        }
        internal void CollectAddress()
        {
            string address;

            do {
                Console.WriteLine("Please type the shipping address below:");
                address = Console.ReadLine() ?? "INVALID";

                Console.WriteLine($"Summary: {address}");

                Console.WriteLine("Is that correct?\n\t(1) Yes\n\t(2) No");
            } while (Utilities.TakeInput(1, 2) != 1);
        }
    }
}
