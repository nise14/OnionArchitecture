namespace Application.Dtos;

public class ClientDto
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? LastName { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? Address { get; set; }
    public int Age { get; set; }
}