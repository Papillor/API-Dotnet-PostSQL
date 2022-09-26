namespace API_Dotnet_PostSQL.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Branch { get; set; }
        public int Seniority { get; set; }
    }
}
