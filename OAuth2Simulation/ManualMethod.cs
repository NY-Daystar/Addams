using IdentityModel.Client; // Make sur NugetPackage IdentityModel is installed

namespace Oauth2Simulation;

/// <summary>
/// Generate Oauth2 Token copying authorizing code from url manually
/// </summary>
public class ManualMethod : Method
{
    public ManualMethod(Method method) : base(method.Hostname, method.Port, method.AuthorityUri, method.RedirectUri,
        method.UserId, method.ClientId, method.ClientSecret, method.Scope, method.ResponseType)
    { }

    /// <summary>
    /// Execute Oauth2 Method
    /// </summary>
    async public Task Execute()
    {     
        // Étape 1 : Rediriger l'utilisateur vers Spotify pour autoriser l'application
        var authorizationRequestUrl = $"{AuthorityUri}/authorize?client_id={ClientId}&response_type={ResponseType}&redirect_uri={Uri.EscapeDataString(RedirectUri)}&scope={Uri.EscapeDataString(Scope)}";
        Console.WriteLine("Veuillez autoriser l'application en visitant cette URL :");
        Console.WriteLine(authorizationRequestUrl);

        // Étape 2 : Récupérer le code d'autorisation de Spotify
        Console.WriteLine("Entrez le code d'autorisation obtenu après avoir autorisé l'application :");
        var authorizationCode = Console.ReadLine();

        // Étape 3 : Échanger le code contre un jeton d'accès
        var httpClient = new HttpClient();
        var tokenResponse = await httpClient.RequestAuthorizationCodeTokenAsync(new AuthorizationCodeTokenRequest
        {
            Address = $"{AuthorityUri}/api/token",
            ClientId = ClientId,
            ClientSecret = ClientSecret,
            Code = authorizationCode,
            RedirectUri = RedirectUri
        });

        if (tokenResponse.IsError)
        {
            Console.WriteLine($"Erreur lors de l'échange du code d'autorisation contre un jeton : {tokenResponse.Error}");
            return;
        }

        // Maintenant, vous pouvez utiliser tokenResponse.AccessToken pour accéder à l'API Spotify
        // Par exemple, vous pouvez utiliser un autre appel HTTP pour accéder à l'API Spotify avec ce token d'accès.
        Console.WriteLine($"\n\nToken d'accès : {tokenResponse.AccessToken}\n\n");

        //Ensuite, vous pouvez utiliser le token d'accès pour accéder aux ressources Spotify
        // Comme par exemple :
        var spotifyApiUrl = $"https://api.spotify.com/v1/users/{UserId}/tracks";
        httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", tokenResponse.AccessToken);
        var apiResponse = await httpClient.GetAsync(spotifyApiUrl);
        var content = await apiResponse.Content.ReadAsStringAsync();
        Console.WriteLine($"Réponse de l'API Spotify : {content}");
    }
}