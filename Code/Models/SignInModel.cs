namespace Web.Models;

public class SignInModel
{
    public string Password { get; set; }

    public string Username { get; set; }

    public SignInModel()
    {
        Password = Username = string.Empty;
    }
}