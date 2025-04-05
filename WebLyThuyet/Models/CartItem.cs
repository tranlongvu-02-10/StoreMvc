namespace WebLyThuyet.Models
{
    public class CartItem
    {
        //thông tin sản phẩm
        public int ProductId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
