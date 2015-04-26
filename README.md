# Inflatable Last.fm .NET SDK [![Build status](https://ci.appveyor.com/api/projects/status/c8gg2cw4jibbsg3u)](https://ci.appveyor.com/project/rikkit/lastfm)

MIT licensed. 

Feature request? Bug? Looking to help? Check out [the issues on GitHub](https://github.com/inflatablefriends/lastfm/issues).

If you have comments or need some help, post to our chat room on [![Gitter](https://badges.gitter.im/Join Chat.svg)](https://gitter.im/inflatablefriends/lastfm?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge).

## Project Goals

- Provide complete .NET bindings for the Last.fm REST API
- Provide components useful in any Last.fm app
- To be the very best, like no-one ever was

## Quickstart

### Installing the SDK

#### NuGet - production code

Install [the NuGet package](
https://www.nuget.org/packages/Inflatable.Lastfm/). Search for ```Inflatable.Lastfm``` in the NuGet package browser.____

#### NuGet - prerelease code

There is a NuGet package built for every commit to master. Add ```https://ci.appveyor.com/nuget/lastfm-ql51ic53xoqw``` to your NuGet package sources, and install the ```IF.Lastfm.Core``` prerelease package.

#### From source

Clone this repo and reference ```IF.Lastfm.Core``` in your application. Your IDE needs to support building portable libraries - Visual Studio 2013 Community or better.

### Using the SDK

First, [sign up for Last.fm API](http://last.fm/api) access if you haven't already.

Create a LastfmClient:

```c#
var client = new LastfmClient("apikey", "apisecret");
```

Get information about an album:

```c#
var response = client.Album.GetInfoAsync("Grimes", "Visions");

LastAlbum visions = response.Content;
```

For methods that return several items, you can simply iterate over the response

```c#
var pageResponse = await client.Artist.GetTopTracksAsync("Ben Frost", page: 5, itemsPerPage: 100);

var trackNames = pageResponse.Select(track => track.Name);
```

Several API methods require user authentication. Once you have your user's Last.fm username and password, you can authenticate your instance of LastfmClient:

```c#
var response = await client.Auth.GetSessionTokenAsync("username", "pass");

// or load an existing session
UserSession cachedSession;
var succesful = client.Auth.LoadSession(cachedSession);
```

Authenticated methods then work like any other

```c#
if (client.Auth.HasAuthenticated) {
	var response = await client.Track.LoveAsync("Ibi Dreams of Pavement (A Better Day)", "Broken Social Scene");
}
```

## Documentation

- [Api method progress report](PROGRESS.md)
- [Example Windows Phone app](https://github.com/inflatablefriends/lastfm-samples)
- [Scrobbling](doc/scrobbling.md)
- [Dependency Injection](doc/dependency-injection.md)

## Platform Compatibility

The current PCL profile is

- .NET Framework 4.5
- Windows 8.0
- Windows Phone 8.1
- Windows Phone Silverlight 8

If you need support for Mono, Windows Phone 7, or another .NET platform, please ask: the current profile was chosen to reduce dependencies for the most common use cases. The SDK should be trivial to port to any platform which supports Async, HttpClient and Json.Net.

## Credits

Maintained by [@rikkilt](http://twitter.com/rikkilt).
Thanks to [all contributors](https://github.com/inflatablefriends/lastfm/graphs/contributors)!