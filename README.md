# Inflatable Last.fm .NET SDK

![Project logo](./res/if-lastfm-logo-300.png)

[![Code licence](https://img.shields.io/badge/licence-MIT-blue.svg?style=flat)](LICENCE.md) [![Build status](https://ci.appveyor.com/api/projects/status/c8gg2cw4jibbsg3u)](https://ci.appveyor.com/project/rikkit/lastfm) [![Gitter](https://badges.gitter.im/Join Chat.svg)](https://gitter.im/inflatablefriends/lastfm?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge)


## Project Goals

- To provide complete .NET bindings for the Last.fm REST API
- To build useful components for Last.fm applications
- To be the very best, like no-one ever was

## Contributing

Input is always welcome! [Raise an issue on GitHub](https://github.com/inflatablefriends/lastfm/issues), or send a message to [the Gitter chatroom](https://gitter.im/inflatablefriends/lastfm) if you need help with the library. 

If you're interested in contributing code or documentation, [this short introduction to the library](doc/contributing.md) will help you get started.

## Quickstart

### Installing the SDK

#### NuGet - production code

Install [the NuGet package](
https://www.nuget.org/packages/Inflatable.Lastfm/). Search for ```Inflatable.Lastfm``` in the NuGet package browser.

#### NuGet - prerelease code

There is a NuGet package built for every commit to master. Add ```https://ci.appveyor.com/nuget/lastfm``` to your NuGet package sources, and install the ```IF.Lastfm.Core``` prerelease package.

#### From source

Clone this repo and reference ```IF.Lastfm.Core``` in your application. Your IDE needs to support C# 6 and portable libraries - Visual Studio 2015 Community or better.

### Using the SDK

First, [sign up for Last.fm API](http://last.fm/api) access if you haven't already.

Create a LastfmClient:

```c#
var client = new LastfmClient("apikey", "apisecret");
```

Get information about an album:

```c#
var response = await client.Album.GetInfoAsync("Grimes", "Visions");

LastAlbum visions = response.Content;
```

For methods that return several items, you can iterate over the response:

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
- [Contributing](doc/contributing.md)
- [Scrobbling](doc/scrobbling.md)
- [Dependency Injection](doc/dependency-injection.md)
- [Example Windows Phone app](https://github.com/inflatablefriends/lastfm-samples)

## Platform Compatibility

The main package targets ```netstandard1.1```. Development is on the ```master``` branch.

### Dependencies

- Newtonsoft.Json 9.0.1 =<
- System.Net.Http 4.3.0 =<

### Supported platforms

Check [this table](https://docs.microsoft.com/en-us/dotnet/articles/standard/library#net-platforms-support) for supported platforms.

### Other platforms

If you need support for a .NET platform that doesn't support .Net Standard 1.1, first see if the feature you need is available in v0.3 or earlier - that version targeted PCL profile 259, and so is compatible with e.g. Windows 8.0 and Windows Phone 7.

If you need a feature for these older platforms, please raise an issue.

## Credits

Maintained by [@rikkilt](http://twitter.com/rikkilt).
Thanks to [all contributors](https://github.com/inflatablefriends/lastfm/graphs/contributors)!
