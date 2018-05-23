namespace BotSalaryPackageCommittee.Dialogs
{
    public class EmployeeInformation
    {
        public int EmployeeId { get; set; }
        public string Name { get; set; }
        public string Function { get; set; }
        public string Governance { get; set; }
        public decimal Salary { get; set; }
        public string Currency { get; internal set; }
    }
    

}