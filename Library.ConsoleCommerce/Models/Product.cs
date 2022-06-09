namespace Library.ConsoleCommerce.Models
{
    internal class Product
    {
        //Name - the name of the product
        public string Name { get; set; }
        //Description - the description of the product
        public string Description { get; set; }
        //Price - the unit price of the product
        public double Price { get; set; }
        //id number
        public int Id { get; set; }
        // buy one get one free
        public bool BoGo { get; set; }
        // needs to be overwritten
        public virtual double TotalPrice { get; }

        public Product()
        {
            Name = "";
            Description = "";
        }

        public virtual string Display() 
        {
            return "UNDEFINED PRODUCT TYPE";
        }
        public override string ToString()
        {
            return "UNDEFINED PRODUCT TYPE";

        }
    }
}
