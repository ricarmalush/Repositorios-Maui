namespace Transference.Models
{
    public class UserModel
    {
        public int UserId { get; set; }
        public int Id { get; set; }
        public double HoursWorked { get; set; }
        public string Name { get; set; }
        public string LastName1 { get; set; }
        public string LastName2 { get; set; }
        public string Province { get; set; }
        public string Community { get; set; }
    }

    public class OvertimeModel
    {
        public int UserId { get; set; }
        public DateTime AuditCreateDate { get; set; }
        public double Hours { get; set; }
        public string Reason { get; set; }
        public string FullName { get; set; }
    }

}
