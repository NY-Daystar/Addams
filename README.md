﻿[![Addams-CI](https://github.com/NY-Daystar/addams/actions/workflows/dotnet.yml/badge.svg)](https://github.com/NY-Daystar/addams/actions/workflows/dotnet.yml)
[![License: GPL v3](https://img.shields.io/badge/License-GPLv3-blue.svg)](https://www.gnu.org/licenses/gpl-3.0)
[![Version](https://img.shields.io/github/tag/NY-Daystar/addams.svg)](https://github.com/NY-Daystar/addams/releases)

![GitHub watchers](https://img.shields.io/github/watchers/ny-daystar/addams)
![GitHub forks](https://img.shields.io/github/forks/ny-daystar/addams)
![GitHub Repo stars](https://img.shields.io/github/stars/ny-daystar/addams)
![GitHub repo size](https://img.shields.io/github/repo-size/ny-daystar/addams)
![GitHub language count](https://img.shields.io/github/languages/count/ny-daystar/addams)
![GitHub top language](https://img.shields.io/github/languages/top/ny-daystar/addams)
![GitHub commit activity (branch)](https://img.shields.io/github/commit-activity/m/ny-daystar/addams/main)
![GitHub issues](https://img.shields.io/github/issues/ny-daystar/addams)
![GitHub closed issues](https://img.shields.io/github/issues-closed-raw/ny-daystar/addams)
![GitHub](https://img.shields.io/github/license/ny-daystar/addams)
[![All Contributors](https://img.shields.io/badge/all_contributors-1-blue.svg?style=circular)](#contributors)

![C#](https://img.shields.io/badge/c%23-%23239120.svg?style=for-the-badge&logo=csharp&logoColor=white)

# Addams

C# project to export in csv spotify user's playlist  
Source code analysed with [DeepSource](https://deepsource.com/)

**\_Version: v1.0.2**

## Summary

-   [Requirements](#requirements)
-   [How to use](#how-to-use)
-   [Get started](#get-started)
    -   [Setup in spotify](#setup-in-spotify)
    -   [Setup project](#setup-project)
    -   [Unit tests](#tests)
-   [How it works](#how-it-works)
-   [Understand OAuth2](#oauth2-simulation)
-   [Contact](#contact)
-   [Credits](#credits)

To test on spotify there is some link

## Requirements

-   [.NET Framework](https://dotnet.microsoft.com/en-us/download/dotnet/7.0) >= 7.0
-   [VS 2022](https://visualstudio.microsoft.com/fr/vs/) >= 2022

## How to use

First you need to create a spotify application in developper mode
to do this [follow this](#setup-spotify-app)

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
$ git clone git@github.com:NY-Daystar/Addams.git
```

2. Open VS 2022 -> `Open project or solution`
3. Select `Addams.sln`
4. Rebuild solution
5. F5 to launch project in Debug mode

## For developpers

You can activate git hooks with this command

```bash
git config --global core.hooksPath .githooks
```

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

## OAuth2 Simulation

To understand how spotify generate token you can try the projet `Oauth2Simulation`

This project will show you step-by-step the mecanism of Auth2

1. Authorize the application
2. Retrieve Authorization code
3. Exchange Authorization code for an access Token
4. Call Api to test Spotify Token

For more information :

-   [Authentication guide](https://johnnycrazy.github.io/SpotifyAPI-NET/docs/auth_introduction)
-   [Use authorization code](https://johnnycrazy.github.io/SpotifyAPI-NET/docs/authorization_code)

## Tests

you can run unit tests in `Addams.Tests` project

## Contact

-   To make a pull request: https://github.com/NY-Daystar/addams/pulls
-   To summon an issue: https://github.com/NY-Daystar/addams/issues
-   For any specific demand by mail: [luc4snoga@gmail.com](mailto:luc4snoga@gmail.com?subject=[GitHub]%addams%20Project)

## Credits

Made by Lucas Noga.  
Licensed under GPLv3.
