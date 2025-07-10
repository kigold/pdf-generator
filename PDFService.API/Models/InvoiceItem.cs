namespace PDFService.API.Models
{
    public class InvoiceItem
    {
        public string Description { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Amount => Quantity * UnitPrice;

    }

    public class InvoiceDetail
    {
        public InvoiceItem[] Items { get; set; } = new InvoiceItem[0];
        public decimal TotalAmount => Items.Sum(x => x.Amount);

    }
}
