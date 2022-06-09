using Library.ConsoleCommerce.Models;
using Library.ConsoleCommerce.Utility;
using Library.ConsoleCommerce.Imports;

namespace Library.ConsoleCommerce.Services
{
    public class Commerce
    {
        private Inventory inv;
        private Cart cart;
        // create inventory and cart to be used 
        public Commerce() {
            inv = new Inventory();
            cart = new Cart(inv);
        }

        // displays options / takes input for employee
        private int EmployeeMenu()
        {
            // list options, take input, return choice
            Console.WriteLine("\nWould you like to:\n\t" +
                "(1) Add a product\n\t" +
                "(2) Add stock\n\t" +
                "(3) Display inventory\n\t" +
                "(4) Edit a product\n\t" +
                "(5) Search\n\t" +
                "(6) Load inventory\n\t" +
                "(7) Save inventory\n\t" +
                "(0) Exit");
            var input = Utilities.TakeInput(0, 7);

            return input;
        }

        // displays options / takes input for customers
        private int CustomerMenu()
        {
            // list options, take input, return choice
            Console.WriteLine("\nWould you like to:\n\t" +
                "(1) Add a product to cart\n\t" +
                "(2) Remove a product from cart\n\t" +
                "(3) Display cart\n\t" +
                "(4) Search\n\t" +
                "(5) Save cart\n\t" +
                "(6) Checkout\n\t" +
                "(7) Load saved cart\n\t" +
                "(0) Exit");    // just exit should return items to inv
            var input = Utilities.TakeInput(0, 7);

            return input;
        }

        // employee decisionmaking
        public void EmployeeLogic()
        {
            int chosenID;
            double amount;

            int action = 1;
            while (action != 0)
            {
                // display menu / take input
                action = EmployeeMenu();

                switch (action)
                {

                    // add a product
                    case 1:
                        inv.CreateProduct();
                        break;

                    // add stock
                    case 2:
                        Console.WriteLine("Which product would you like to add stock to? (Please type the ID)");
                        inv.Display();
                        chosenID = Utilities.TakeInput(0, inv.Products.Count());
                        Console.WriteLine("How much stock would you like to add?");
                        amount = double.Parse(Console.ReadLine() ?? "0");

                        inv.AddStock(chosenID, amount);
                        break;

                    // display
                    case 3:
                        inv.Display();
                        break;

                    // edit product
                    case 4:
                        Console.WriteLine("Which product would you like to edit? (Please type the ID)");
                        inv.Display();
                        chosenID = Utilities.TakeInput(0, inv.Products.Count());
                        
                        Product? editProduct;
                        editProduct = inv.Products.FirstOrDefault(p => p.Id == chosenID);
                        
                        // if it doesnt exist
                        if (editProduct == null)
                            return;

                        Console.WriteLine("Would you like to edit:\n\t(1) Name\n\t(2) Description\n\t(3) Sale status");
                        switch (Utilities.TakeInput(1, 3))
                        {
                            case 1:
                                Console.WriteLine("Please enter the new name -> ");
                                editProduct.Name = Console.ReadLine() ?? "Default Name";
                                break;
                            case 2:
                                Console.WriteLine("Please enter the new description -> ");
                                editProduct.Description = Console.ReadLine() ?? "Default Description";
                                break;
                            case 3:
                                Console.WriteLine("Buy one Get one free:\n\t(1) Yes\n\t(2) No");
                                switch (Utilities.TakeInput(1, 2)) {
                                    case 1:
                                        editProduct.BoGo = true;
                                        break;
                                    case 2:
                                        editProduct.BoGo = false;
                                        break;
                                }
                                break;
                        }
                        break;

                    // search
                    case 5:
                        Console.WriteLine("Please enter the name or description to search by. -> ");
                        string input = Console.ReadLine() ?? "";

                        inv.Search(input);
                        break;
                    // load
                    case 6:
                        Console.WriteLine("Loading...");
                        inv.Load();
                        Console.WriteLine("Load successful!");
                        break;

                    // save
                    case 7:
                        Console.WriteLine("Saving...");
                        inv.Save();
                        Console.WriteLine("Save successful!");
                        break;

                    // exit
                    case 0:
                        Console.WriteLine("Exiting Employee control panel...");
                        break;
                }
                
            }
        }

        // customer decisionmaking 
        public void CustomerLogic()
        {
            int chosenID, action = 1;
            double amount; 
            while (action != 0 && action != 5 && action != 6)
            {
                action = CustomerMenu();

                switch (action)
                {
                    // add product to cart
                    case 1:
                        Console.WriteLine("Please select an item:");
                        inv.Display();

                        Console.WriteLine("Enter the Id of the item -> ");
                        chosenID = int.Parse(Console.ReadLine() ?? "0");
                        Console.WriteLine("Enter the quantity of units -> ");
                        amount = double.Parse(Console.ReadLine() ?? "0");

                        cart.AddItem(chosenID, amount);
                        break;

                    // remove product
                    case 2:
                        Console.WriteLine("Please select an item:");
                        cart.Display();
                        Console.WriteLine("Enter the Id of the item -> ");
                        chosenID = int.Parse(Console.ReadLine() ?? "0");

                        cart.RemoveItem(chosenID);
                        break;

                    // display
                    case 3:
                        cart.Display();
                        break;

                    // search
                    case 4:
                        Console.WriteLine("Please enter the name or description to search by. -> ");
                        string input = Console.ReadLine() ?? "";

                        cart.Search(input);

                        Console.WriteLine("Would you like to search the store inventory:\n\t" +
                            "(1) Yes\n\t" +
                            "(2) No");
                        switch (Utilities.TakeInput(1, 2))
                        {
                            case 1:
                                inv.Search(input);
                                break;
                            default:
                                break; // do nothing
                        }
                        break;

                    // save
                    case 5:
                        Console.WriteLine("Saving...");
                        cart.Save();
                        Console.WriteLine("Save successful. Thanks for shopping!");
                        break;

                    // checkout
                    case 6:
                        Console.WriteLine("Checking out...");
                        cart.CheckOut();
                        break;

                    // load
                    case 7:
                        Console.WriteLine("Loading...");
                        cart.Load();
                        Console.WriteLine("Load successful.");
                        break;

                    // exit
                    case 0:
                        Console.WriteLine("Exiting Customer control panel...");
                        break;
                }
                
            }
        }

        // testing, comment out when done.
        public void Test()
        {

        }

    }
}
