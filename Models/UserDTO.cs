using System.ComponentModel.DataAnnotations;

namespace HotelListing.Models;

public class LoginUserDTO
{
    [Required]
    [StringLength(15)]
    public string Password { get; set; }

    [Required]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; }
}
public class UserDTO:LoginUserDTO
{

    public string FirstName { get; set; }
    public string LastName { get; set; }

    [DataType(DataType.PhoneNumber)]
    public string PhoneNumber { get; set; }

    public ICollection<string> Roles { get; set; }
}
