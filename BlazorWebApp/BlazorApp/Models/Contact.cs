namespace BlazorApp.Models;

public class Contact
{
    public int ContactId { get; set; }

    public required string FullName { get; set; }

    public string? Address { get; set; }

    public string? City { get; set; }

    public string? State { get; set; }

    public string? Zip { get; set; }

    public required string Email { get; set; }
}
