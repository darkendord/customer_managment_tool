namespace Initial_API.Models
{
    public partial class Escalation
    {
        public DateTime CreationDate { get; set; }
        public string Departament { get; set; }
        public int IdEscalation { get; set; }
        public string Notes { get; set; }
        public int IdCustomer { get; set; }
        public int EmployeeNumber { get; set; }
    }
}
