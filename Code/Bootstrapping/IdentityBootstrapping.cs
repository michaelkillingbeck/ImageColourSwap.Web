using Amazon.CognitoIdentityProvider;
using Amazon.Extensions.CognitoAuthentication;
using Web.Models;

namespace Web.Bootstrapping;

internal static class IdentityBootstrapping
{
    internal static void AddIdentityProvider(WebApplicationBuilder builder)
    {
        CognitoPoolOptionsModel cognitoSection = new();
        builder.Configuration.GetSection("Cognito").Bind(cognitoSection);

        AmazonCognitoIdentityProviderClient cognitoProvider = new();
        CognitoUserPool cognitoUserPool = new(
            cognitoSection.UserPoolId,
            cognitoSection.UserPoolClientid,
            cognitoProvider,
            cognitoSection.UserPoolClientSecret);

        _ = builder.Services.AddSingleton<IAmazonCognitoIdentityProvider>(cognitoProvider);
        _ = builder.Services.AddSingleton(cognitoUserPool);
        _ = builder.Services.AddCognitoIdentity();
    }
}