using Amazon.CognitoIdentityProvider;
using Amazon.Extensions.CognitoAuthentication;
using Web.Models;

namespace Web.Bootstrapping;

internal class IdentityBootstrapping
{
    internal static void AddIdentityProvider(WebApplicationBuilder builder)
    {
        var cognitoSection = builder.Configuration.GetSection("Cognito");
        var cognitoProvider = new AmazonCognitoIdentityProviderClient();
        var cognitoUserPool = new CognitoUserPool(
            cognitoSection["UserPoolId"],
            cognitoSection["UserPoolClientid"],
            cognitoProvider,
            cognitoSection["UserPoolClientSecret"]);

        builder.Services.Configure<SettingsModel>(
            builder.Configuration.GetSection("Settings")
        );

        builder.Services.AddSingleton<IAmazonCognitoIdentityProvider>(cognitoProvider);
        builder.Services.AddSingleton<CognitoUserPool>(cognitoUserPool);
        builder.Services.AddCognitoIdentity();
    }
}