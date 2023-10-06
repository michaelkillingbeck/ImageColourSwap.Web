namespace Web.Models;

public class CognitoPoolOptionsModel
{
    public string UserPoolClientid { get; set; }
    public string UserPoolClientSecret { get; set; }
    public string UserPoolId { get; set; }

    public CognitoPoolOptionsModel()
    {
        UserPoolClientid =
            UserPoolClientSecret =
                UserPoolId = string.Empty;
    }
}