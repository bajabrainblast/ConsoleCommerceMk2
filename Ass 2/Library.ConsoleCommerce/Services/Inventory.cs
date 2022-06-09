using Library.ConsoleCommerce.Models;
using Library.ConsoleCommerce.Utility;
using Library.ConsoleCommerce.Imports;
using Newtonsoft.Json;

namespace Library.ConsoleCommerce.Services
{
    internal class Inventory
    {
        private List<Product> productsList;

        public List<Product> Products { 
            get 
            { 
                return productsList; 
            } 
        }
        public Inventory() {
            productsList = new List<Product>();
        }
        // sets up id
        private int NextId
        {
            get
            {
                if (!productsList.Any())
                {
                    return 1;
                }

                return productsList.Select(t => t.Id).Max() + 1;
            }
        }
        
        // add a product passed in
        public void AddProduct(Product _prod) {
            _prod.Id = NextId;
            productsList.Add(_prod);
        }

        // add stock to a product
        public void AddStock(int _id, double _amount) {
            var ProductOfInterest = productsList.FirstOrDefault(t => t.Id == _id);

            if (ProductOfInterest == null)
                return;

            if (ProductOfInterest is ProductByQuantity)
                (ProductOfInterest as ProductByQuantity).Quantity += (int)_amount;
            else if (ProductOfInterest is ProductByWeight)
                (ProductOfInterest as ProductByWeight).Weight += _amount;
        }

        // create a product and add it
        public void CreateProduct()
        {
            Console.Write("Is this product priced by (1) weight, or (2) quantity? ");
            int choice = Utilities.TakeInput(1, 2);

            Console.Write("Enter the product name -> ");
            var _name = Console.ReadLine() ?? "def name";

            Console.Write("Enter the product description -> ");
            var _description = Console.ReadLine() ?? "def descr";

            Console.Write("Enter the product price -> ");
            var _price = double.Parse(Console.ReadLine() ?? "0");

            // if by weight
            if (choice == 1)
            {
                Console.Write("Enter the product weight -> ");
                var _weight = double.Parse(Console.ReadLine() ?? "0");
                AddProduct(new ProductByWeight(_name, _description, _price, _weight));
            }
            // if by quantity
            else
            {
                Console.Write("Enter the product quantity -> ");
                var _quantity = double.Parse(Console.ReadLine() ?? "0");
                AddProduct(new ProductByQuantity(_name, _description, _price, (int)_quantity));
            }
        }

        // edit a product
        public void Edit(int _id, string _name, string _description)
        {
            var editProduct = productsList.FirstOrDefault(p => p.Id == _id);
            
            if (editProduct == null)
                return;

            editProduct.Name = _name;
            editProduct.Description = _description;

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

        public void Save()
        {
            var json = JsonConvert.SerializeObject(productsList,
                new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All });
            File.WriteAllText("InvData.json", json);
        }
        public void Load()
        {
            var json = File.ReadAllText("InvData.json");
            productsList = JsonConvert.DeserializeObject<List<Product>>(json,
                new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All })
                ?? new List<Product>();
        }

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
                "\n\t(2) Price" +
                "\n\t(3) Id" +
                "\n\t(0) Cancel sort");
            switch (Utilities.TakeInput(0, 3))
            {
                case 1: // sort by name
                    return productsList.OrderBy(x => x.Name);
                    
                case 2: // sort by price
                    return productsList.OrderBy(x => x.Price);
                    
                case 3: // sort by id
                    return productsList.OrderBy(x => x.Id);
                    
                default:
                    return productsList;
            }
        }
    }
}