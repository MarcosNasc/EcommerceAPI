namespace Ecommerce.API.DTOs;

public class LoginResponseDTO
{
    public string AccessToken { get; set; }
    public double ExpiresIn { get; set; }
    public UserTokenDTO UserToken { get; set; }
}