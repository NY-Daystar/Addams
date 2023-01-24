# Addams

[![Addams-CI](https://github.com/LucasNoga/addams/actions/workflows/dotnet.yml/badge.svg)](https://github.com/LucasNoga/addams/actions/workflows/dotnet.yml)
[![License: GPL v3](https://img.shields.io/badge/License-GPLv3-blue.svg)](https://www.gnu.org/licenses/gpl-3.0)
[![Version](https://img.shields.io/github/tag/LucasNoga/addams.svg)](https://github.com/LucasNoga/addams/releases)
[![Total views](https://img.shields.io/sourcegraph/rrc/github.com/LucasNoga/addams.svg)](https://sourcegraph.com/github.com/LucasNoga/addams)

C# project to export in csv spotify user's playlist

**_Version: 1.1.0_**

## Summary

-   [Requirements](#requirements)
-   [How to use](#how-to-use)
-   [Get started](#get-started)
    -   [Setup in spotify](#setup-in-spotify)
    -   [Setup project](#setup-project)
    -   [Unit tests](#tests)
-   [How it works](#how-it-works)
-   [Contact](#contact)
-   [Credits](#credits)

To test on spotify there is some link

## Requirements

-   [.NET Framework](https://dotnet.microsoft.com/en-us/download/dotnet/7.0) >= 7.0
-   For developpment: [VS 2022](https://visualstudio.microsoft.com/fr/vs/) >= 2022

## How to use

First you need to create a spotify application in developper mode
to do this [follow this](#setup-spotify-app)

TODO download executable from github Addams.exe

-   TODO mettre la console spotify les liens etc... etc...
//TODO mettre les bons scope de l'OAUTH2' playlist-read-private user-library-read

// TODO specifier le mode debug avec --debug

## Get Started

### Setup spotify app

1. You need to create an application on spotify in this [link](https://developer.spotify.com/dashboard/applications)

2. Click on create an app

    1. Set app name: `Addams`
    2. Set app name: `C# tool to export spotify playlist`

3. Click into your app created then get value of `Client ID` and `Client Secret`

IMPORTANT: You can delete your app in this [link](https://www.spotify.com/fr/account/apps/)

## Setup project

1. Clone repository

```bash
$ git clone git@github.com:LucasNoga/Addams.git
```

2. Open VS 2022 -> `Open project or solution`
3. Select `Addams.sln`
4. Rebuild solution
5. F5 to launch project in Debug mode

## How it works

The project setup an OAUTH2 token with your [spotify app credentials](#setup-in-spotify) to execute spotify api request

-   To get user's playlist : `https://api.spotify.com/v1/me/playlists?limit=50&offset=0`
-   To get playlist tracks : `https://api.spotify.com/v1/playlists/{playlistID}`

Once all data fetched we create a csv for each playlist with track's data:

-   `Track Name` : Name of the track
-   `Artist Name(s)` : List of artist (separated by `|`)
-   `Album Name` : Name of the album
-   `Album Artist Name(s)` : Album's artists (separated by `|`)
-   `Album Release Date` : Release date of the album (`YYYY-MM-DD`)
-   `Disc Number` : If album has multiple disc
-   `Track Duration` : Time duratio of the track (`minutes:secondes`)
-   `Track Number` : Number of the track in the album
-   `Explicit` : If track is explicit or not (`True or False`)
-   `Popularity` : Number in range 0-100 for unpopular to very popular
-   `Added At` : Datetime when you add this track in your playlist
-   `Track Uri` : Spotify url of the track
-   `Artist Url` : Spotify url of the artist
-   `Album Uri` : Spotify url of the album
-   `Album Image Url` : Url image of the album
-   `Track Preview Url` : Url track preview of the album (30sec audio)

These csv are saved in the same path of Addams.exe in a folder name `data`

## Tests

you can run unit tests in `Addams.Tests` project

TODO suite README Dathomir : https://github.com/LucasNoga/Dathomir#get-started
TODO suite README Dathomir : https://raw.githubusercontent.com/LucasNoga/Dathomir/master/README.md

## Contact

-   To make a pull request: https://github.com/LucasNoga/addams/pulls
-   To summon an issue: https://github.com/LucasNoga/addams/issues
-   For any specific demand by mail: [luc4snoga@gmail.com](mailto:luc4snoga@gmail.com?subject=[GitHub]%addams%20Project)

## Credits

Made by Lucas Noga.  
Licensed under GPLv3.
