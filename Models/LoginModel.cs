using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Aasaan_API.Models
{
  public class LoginModel
  {   
    public int UserID { get; set; }
    [Required]
    [Display(Name = "Email")]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [DataType(DataType.Password)]
    [Display(Name = "Password")]
    public string Password { get; set; }
    public string Token { get; set; }
  }
}
