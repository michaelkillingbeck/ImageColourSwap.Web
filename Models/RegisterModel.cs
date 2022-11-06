namespace Web.Models;

public class RegisterModel
{
    public string EmailAddress { get; set; }
    public string Password { get; set; }
    public string Username { get; set; }

    public RegisterModel()
    {
        EmailAddress = Password = Username = String.Empty;
    }
}