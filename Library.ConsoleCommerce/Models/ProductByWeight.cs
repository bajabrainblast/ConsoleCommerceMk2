namespace Library.ConsoleCommerce.Models
{
    internal class ProductByWeight : Product
    {
        //Weight - the weight of the product being purchased
        public double Weight { get; set; }
        //TotalPrice - the total price of the product being purchased.
        public override double TotalPrice
        {
            get
            {
                // if sale, and buying enough to have an effect
                if (BoGo == true && Weight >= 2) {
                    double total = 0, 
                        whole = (int)Weight, 
                        deci = Weight % 1;

                    if (whole % 2 == 0)
                        total += (whole / 2) * Price;
                    else
                        total += (int)(whole / 2) * Price + Price;

                    total += deci * Price;

                    return total;
                }
                // no sale
                else
                    return Weight * Price;
            }
        }

        // default constr
        public ProductByWeight()
        {
            Name = "Name";
            Description = "Description";
            Price = 10;
            Weight = 1;
        }
        // parameterized constr
        public ProductByWeight(string name, string description, double price, double weight)
        {
            Name = name;
            Description = description;
            Price = price;
            Weight = weight;
        }

        public ProductByWeight(string name, string description, double price, double weight, int id)
        {
            Name = name;
            Description = description;
            Price = price;
            Weight = weight;
            Id = id;
        }
        public ProductByWeight(Product prod, double weight)
        {
            Name = prod.Name;
            Description = prod.Description;
            Price = prod.Price;
            Id = prod.Id;
            BoGo = prod.BoGo;
            Weight = weight;
        }
        public override string Display()
        {
            string output = $"ID: {Id} - {Name}: {Description} \n\t${Price} x {Weight}lbs | Total ${TotalPrice}";
            if (BoGo == true)
                output += "\t with Buy one Get one free!!!";
            return output;
        }
        public override string ToString()
        {
            string output = $"ID: {Id} - {Name}: {Description} \n\t${Price} x {Weight}lbs | Total ${TotalPrice}";
            if (BoGo == true)
                output += "\t with Buy one Get one free!!!";
            return output;
        }
    }
}
