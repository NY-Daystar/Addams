// See https://aka.ms/new-console-template for more information

options.DefaultChallengeScheme = SpotifyAuthenticationDefaults.AuthenticationScheme;
}).AddSpotify(options =>
   {
       options.ClientId = Configuration["Spotify:ClientId"];
       options.ClientSecret = Configuration["Spotify:ClientSecret"];
       options.CallbackPath = "/signin-spotify";
   });

app.UseAuthentication();
app.UseAuthorization();

new AuthenticationHttpClient();
var token = await client.RequestToken(code, "http://localhost:5000/signin-spotify", clientId, clientSecret);
