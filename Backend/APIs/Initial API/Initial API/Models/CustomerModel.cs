namespace Initial_API.Models
{
    public partial class CustomerModel
    {
        public string Name { get; set; }
        public string LastNamer { get; set; }
        public int IdCustomer { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Adress { get; set; }
        public string TypeOfCustomer { get; set; }
        public bool IsCustomerActicve { get; set; }
        public int IdentificationNumber { get; set; }
    }
}
