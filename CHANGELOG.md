# CHANGELOG

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](http://keepachangelog.com/)
and this project adheres to [Semantic Versioning](http://semver.org/).

### Project releases

## v1.1.0 - December XXth, 2025 - GUI

## Added

- User interface

### Changed

- New project in .NET 10
- Adapt to new [spotify rules in its api](https://developer.spotify.com/documentation/web-api/references/changes/february-2026)

### Removed

- Console application

## v1.0.4 - December 22th, 2025 - Authentication PKCE

## Added

- Using refresh token to get access token
- Option to show playlist folder location

### Changed

- Documentation structure in README
- Screen to use application in README

### Removed

- Oauth2 Simulator

### Fixed

- Improve French translation
- Improve English translation
- Authorization to renewal access token with new redirect uri - [https://developer.spotify.com/documentation/web-api/concepts/redirect_uri]

## v1.0.3 - November 12th, 2025 - Last features

## Added

- Multi-languages
- Show and modify configuration
- Show logs

### Fixed

- Adjust color displayed when warnings or errors
- Tracks not in Spotify

## v1.0.2 - August 1st, 2025 - Small fixes

## Added

- Open folder explorer after saving playlists

### Fixed

- Playlist selection
- Number of track for Liked Songs

## v1.0.1 - October 4th, 2024 - Endpoint tracks

### Added

- git hook controls

### Fixed

- Endpoint for songs liked

## v1.0.0 - March 2nd, 2024 - Oauth2

### Added

- Automatisation of generation of OAuth2
- Add Simulation of OAuth2
- Update `README.md`

### Changed

- Entity with JsonProperty

## v0.0.1 - January 29th, 2023 - Core

### Added

- Config, Api and Service to export Spotify's playlist users
- Can export liked songs
- `README.md` and`LICENSE.md`
