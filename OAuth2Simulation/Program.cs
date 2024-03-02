using Oauth2Simulation;

// Server parameters
int port = 8888;
string hostname = $"http://localhost:{port}/";

// Oauth2 parameters
var authorityUri = "https://accounts.spotify.com";
var redirectUri = $"http://localhost:{port}/callback";      // In your spotify APP send this redirection
var userId = "gravityx3";                                   // Spotify account username
var clientId = "7fdad4e680a444358b930bcdfe4ac410";          // In your spotify APP 
var clientSecret = "17614f11830840b18104dda81e5477ba";      // In your spotify APP
var scope = "playlist-read-private user-library-read";
var responseType = "code";


int choice = SelectOption();

Method method = new Method(hostname, port, authorityUri, redirectUri, userId, clientId, clientSecret, scope, responseType);

if (choice == 1)
{

    Console.WriteLine("You choose Manual Method\n");
    await new ManualMethod(method).Execute();
}
else if (choice == 2)
{
    Console.WriteLine("You choose Automatic Method\n");
    await new AutomaticMethod(method).Execute();
}
else
{
    Console.WriteLine("Please choose between 1 or 2");
}

int SelectOption()
{
    int choice = 0;
    try
    {
        Console.Write("Which method do you want to simulate for Oauth2 [1-2]\n\t- 1 : Manual\n\t- 2 : Automatic\nYour choice : ");
        choice = Convert.ToInt32(Console.ReadLine());
    }
    catch (FormatException)
    {
        Console.WriteLine("Please choose between 1 or 2");
        return 0;
    }
    return choice;
}


//// Step 1 : Redirect the user to spotify to authorize application
//var authorizationRequestUrl = $"{authority}/authorize?client_id={clientId}&response_type={responseType}&redirect_uri={Uri.EscapeDataString(redirectUri)}&scope={Uri.EscapeDataString(scope)}";
//Console.WriteLine("Authorize application, visiting this URL :");
//Console.WriteLine(authorizationRequestUrl);

//var token = await HttpListen(hostname, authority, clientId, clientSecret, redirectUri);

//if (string.IsNullOrEmpty(token))
//{
//    Console.WriteLine($"Error when exchange authorizing code for a token");
//    return;
//}

//Console.WriteLine($"\n\nToken d'accès : {token}\n\n");

//// Now, you can use token to get access to Spotify API, for exemple get liked song for user
//var spotifyApiUrl = $"https://api.spotify.com/v1/users/{userId}/tracks";
//var httpClient = new HttpClient();
//httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
//var apiResponse = await httpClient.GetAsync(spotifyApiUrl);
//var content = await apiResponse.Content.ReadAsStringAsync();
//Console.WriteLine($"Réponse de l'API Spotify : {content}");


//async static Task<string> HttpListen(string hostname, string authority, string clientId, string clientSecret, string redirectUri)
//{
//    // Créer et démarrer le serveur HTTP
//    var listener = new HttpListener();
//    listener.Prefixes.Add(hostname);
//    listener.Start();

//    Console.WriteLine("Serveur en écoute sur http://localhost:8888/");

//    while (true)
//    {
//        var context = await listener.GetContextAsync();
//        var request = context.Request;
//        if (request.Url?.AbsolutePath == "/callback")
//        {
//            // Get authorizing code
//            var authorizationCode = request.QueryString.Get("code");

//            // Step 3 : Change authorizing code for a token
//            var httpClient = new HttpClient();
//            var url = $"{authority}/api/token";
//            var tokenResponse = await httpClient.RequestAuthorizationCodeTokenAsync(new AuthorizationCodeTokenRequest
//            {
//                Address = url,
//                ClientId = clientId,
//                ClientSecret = clientSecret,
//                Code = authorizationCode ?? String.Empty,
//                RedirectUri = redirectUri
//            });

//            // Afficher le token d'accès récupéré
//            var accessToken = tokenResponse.AccessToken ?? string.Empty;
//            return accessToken;
//        }
//}