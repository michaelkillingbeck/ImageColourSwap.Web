using Amazon.CognitoIdentityProvider;
using Amazon.Extensions.CognitoAuthentication;
using Web.Models;

namespace Web.Bootstrapping;

internal class IdentityBootstrapping
{
    internal static void AddIdentityProvider(WebApplicationBuilder builder)
    {
        CognitoPoolOptionsModel cognitoSection = new CognitoPoolOptionsModel();
        builder.Configuration.GetSection("Cognito").Bind(cognitoSection);

        AmazonCognitoIdentityProviderClient cognitoProvider = new AmazonCognitoIdentityProviderClient();
        CognitoUserPool cognitoUserPool = new CognitoUserPool(
            cognitoSection.UserPoolId,
            cognitoSection.UserPoolClientid,
            cognitoProvider,
            cognitoSection.UserPoolClientSecret);

        builder.Services.AddSingleton<IAmazonCognitoIdentityProvider>(cognitoProvider);
        builder.Services.AddSingleton(cognitoUserPool);
        builder.Services.AddCognitoIdentity();
    }
}