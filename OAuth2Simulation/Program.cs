using Oauth2Simulation;

// Server parameters
int port = 8888;
string hostname = $"http://localhost:{port}/";

// Oauth2 parameters
var authorityUri = "https://accounts.spotify.com";
var redirectUri = $"${hostname}/callback";      // In your spotify APP send this redirection
var scope = "playlist-read-private user-library-read";
var responseType = "code";

// TODO to COMPLETE
var userId = "XXXXXXXXXXXXXXXXXXXXXX";                      // Spotify account username
var clientId = "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX";          // In your spotify APP 
var clientSecret = "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX";      // In your spotify APP

int choice = SelectOption();

Method method = new(hostname, port, authorityUri, redirectUri, userId, clientId, clientSecret, scope, responseType);

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

static int SelectOption()
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