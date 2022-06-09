namespace Library.ConsoleCommerce.Models
{
    internal class ProductByQuantity : Product
    {
        //Quantity - the amount of the product being purchased
        public int Quantity { get; set; }
        //TotalPrice - the total price of the product being purchased.
        public override double TotalPrice
        {
            get
            {
                // if its buy one get one free, and they bought enough to have effect
                if (BoGo == true && Quantity >= 2)
                {
                    // if they bought an even number of products
                    if (Quantity % 2 == 0)
                        return (Quantity / 2) * Price;
                    else
                        return (int)(Quantity / 2) * Price + Price;
                }
                // no sales
                else
                    return Quantity * Price;
            }
        }

        // default constr
        public ProductByQuantity()
        {
            Name = "Name";
            Description = "Description";
            Price = 10;
            Quantity = 1;
        }
        // parameterized constr
        public ProductByQuantity(string name, string description, double price, int quantity)
        {
            Name = name;
            Description = description;
            Price = price;
            Quantity = quantity;
        }

        public ProductByQuantity(string name, string description, double price, int quantity, int id)
        {
            Name = name;
            Description = description;
            Price = price;
            Quantity = quantity;
            Id = id;
        }
        public ProductByQuantity(Product prod, int quantity)
        {
            Name = prod.Name;
            Description = prod.Description;
            Price = prod.Price;
            Id = prod.Id;
            BoGo = prod.BoGo;
            Quantity = quantity;
        }

        public override string Display()
        {
            string output = $"ID: {Id} - {Name}: {Description} \n\t${Price} x {Quantity} units | Total ${TotalPrice}";
            if (BoGo == true)
                output += "\t with Buy one Get one free!!!";
            return output;
        }
        public override string ToString()
        {
            string output = $"ID: {Id} - {Name}: {Description} \n\t${Price} x {Quantity} units | Total ${TotalPrice}";
            if (BoGo == true)
                output += "\t with Buy one Get one free!!!";
            return output;
        }
    }
}
