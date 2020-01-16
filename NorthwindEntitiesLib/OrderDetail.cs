namespace Packt.Shared
{
    public class OrderDetail 
    {
        public int OrderID { get; set; }
        public int ProductID { get; set; }
        public decimal UnitPrice { get; set; }
        public short Quantity { get; set; }
        public double Discount { get; set; }
        public Order Order { get; set; }
        public Product Product { get; set; }
    }
}