namespace Initial_API.Models
{
    public partial class Card
    {
        public DateTime OpenDate { get; set; }
        public DateTime ExprirationDate { get; set; }
        public string NumberCard { get; set; }
        public int IdCard { get; set; }
        public string BillingCycle { get; set; }
        public decimal Balance { get; set; }
        public bool IsCardActive { get; set; }
        public int IdCustomer { get; set; }
    }
}
