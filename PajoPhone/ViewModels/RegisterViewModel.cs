using System.ComponentModel.DataAnnotations;

namespace PajoPhone.Models;
using Microsoft.Build.Framework;

public class RegisterViewModel
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = "";

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; } = "";

    [Required]
    public string Role { get; set; } = "Customer";
}
