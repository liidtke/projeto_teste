using System.ComponentModel.DataAnnotations;
namespace WebApi.Domain;

public class Patient
{
    public int Id { get; set; }
    
    [MaxLength(200)]
    public string? Name { get; set; }
    
    [MaxLength(50)]
    public string? Phone { get; set; }
    
    [MaxLength(100)]
    public string Email { get; set; }
    
    public Gender Gender { get; set; }
}


public enum Gender
{
    Male,
    Female,
}