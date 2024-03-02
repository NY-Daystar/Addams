using IdentityModel.Client; // Make sur NugetPackage IdentityModel is installed
using System.Net;

namespace Oauth2Simulation;

/// <summary>
/// Generate automatically token extracting by uri
/// </summary>
public class AutomaticMethod : Method
{
    public AutomaticMethod(Method method) : base(method.Hostname, method.Port, method.AuthorityUri, method.RedirectUri,
        method.UserId, method.ClientId, method.ClientSecret, method.Scope, method.ResponseType)
    { }

    /// <summary>
    /// Execute Oauth2 Method
    /// </summary>
    async public Task Execute()
    {
        // Step 1 : Redirect the user to spotify to authorize application
        var authorizationRequestUrl = $"{AuthorityUri}/authorize?client_id={ClientId}&response_type={ResponseType}&redirect_uri={Uri.EscapeDataString(RedirectUri)}&scope={Uri.EscapeDataString(Scope)}";
        Console.WriteLine("Authorize application, visiting this URL :");
        Console.WriteLine(authorizationRequestUrl);

        var token = await HttpListen();

        if (string.IsNullOrEmpty(token))
        {
            Console.WriteLine($"Error when exchange authorizing code for a token");
            return;
        }

        Console.WriteLine($"\n\nToken d'accès : {token}\n\n");

        // Now, you can use token to get access to Spotify API, for exemple get liked song for user
        var spotifyApiUrl = $"https://api.spotify.com/v1/users/{UserId}/tracks";
        var httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        var apiResponse = await httpClient.GetAsync(spotifyApiUrl);
        var content = await apiResponse.Content.ReadAsStringAsync();
        Console.WriteLine($"Réponse de l'API Spotify : {content}");

    }

    /// <summary>
    /// Création d'un serveur pour récuper le code d'authorization
    /// </summary>
    /// <returns>Token Oauth2</returns>
    async Task<string> HttpListen()
    {
        // Créer et démarrer le serveur HTTP
        var listener = new HttpListener();
        listener.Prefixes.Add(Hostname);
        listener.Start();

        Console.WriteLine("Serveur en écoute sur http://localhost:8888/");

        while (true)
        {
            var context = await listener.GetContextAsync();
            var request = context.Request;
            if (request.Url?.AbsolutePath == "/callback")
            {
                // Get authorizing code
                var authorizationCode = request.QueryString.Get("code");

                // Step 3 : Change authorizing code for a token
                var httpClient = new HttpClient();
                var url = $"{AuthorityUri}/api/token";
                var tokenResponse = await httpClient.RequestAuthorizationCodeTokenAsync(new AuthorizationCodeTokenRequest
                {
                    Address = url,
                    ClientId = ClientId,
                    ClientSecret = ClientSecret,
                    Code = authorizationCode ?? String.Empty,
                    RedirectUri = RedirectUri
                });

                // Afficher le token d'accès récupéré
                return tokenResponse.AccessToken ?? string.Empty;
            }
        }
    }
}