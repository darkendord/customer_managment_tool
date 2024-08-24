namespace Initial_API.Models
{
    public partial class Notes
    {
        public DateTime CreationDate { get; set; }
        public string CreatedBy { get; set; }
        public string Detail { get; set; }
        public int IdNote { get; set; }
        public int IdCustomer { get; set; }
        public int EmployeeNumber { get; set; }
    }
}
