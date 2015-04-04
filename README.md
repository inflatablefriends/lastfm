# Inflatable Last.fm .NET SDK [![Build status](https://ci.appveyor.com/api/projects/status/c8gg2cw4jibbsg3u)](https://ci.appveyor.com/project/rikkit/lastfm)

MIT licensed. Maintained by [@rikkilt](http://twitter.com/rikkilt).

Feature request? Bug? Or just wanna help out? Check out [the issues on GitHub](https://github.com/inflatablefriends/lastfm/issues).

If you have comments or need some help, post to our chat room on [![Gitter](https://badges.gitter.im/Join Chat.svg)](https://gitter.im/inflatablefriends/lastfm?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge).

## Project Goals

- Provide complete, and completely tested, bindings for the Last.fm REST API for use on modern .NET platforms.
- Provide functionality beyond API bindings, to spread the joy of Last.fm to developers everywhere.
- To be the very best, like no-one ever was.

## Quickstart
### Installing the SDK

If you want to work with the Last.fm API, you should install [the NuGet package](
https://www.nuget.org/packages/Inflatable.Lastfm/). Search for ```Inflatable.Lastfm``` in the NuGet package browser or run this commmand in the NuGet package console:

```ps
PM> Install-Package Inflatable.Lastfm
```

You can also clone this repo and build from source, referencing IF.Lastfm.Core in your project. Keep in mind this requires a version of Visual Studio that supports portable class libraries and PCL profile 259 - Visual Studio 2012/3 Pro or higher running on Windows 8 or higher.

### Using the SDK

Once IF.Lastfm.Core is referenced, it's pretty simple to get started. First, sign up for Last.fm API access if you haven't already 

This is how to get album info:

```c#
var auth = new LastAuth("apikey", "apisecret");
var albumApi = new AlbumApi(auth); // this is an unauthenticated call to the API
var response = await albumApi.GetAlbumInfoAsync("Grimes", "Visions");
var visions = response.Content; // visions is a LastAlbum
```

For methods that return several items, you can simply iterate over the response

```c#
var pageResponse = await artistApi.GetTopTracksForArtistAsync("Ben Frost", page: 5, itemsPerPage: 100);

foreach (var wallOfSound in pageResponse)
{
	// wallOfSound is a LastTrack
}
```

Several API methods require user authentication. Once you have your user's Last.fm username and password:

```c#
var auth = new LastAuth("apikey", "apisecret");

// wait for authentication
var response = await auth.GetSessionTokenAsync("username", "pass");

if (response.Success && auth.HasAuthenticated) {
	var trackApi = new TrackApi(auth);
	var loved = await trackApi.LoveTrackAsync("CIRCLONT6A [141.98][Syrobonkus mix]", "Aphex Twin");
}
```

Some documentation is available on the [GitHub wiki](https://github.com/rikkit/lastfm-wp/wiki). You can also check the Windows Phone demo project for some example code. 

Any problems, just ask in [Gitter](https://gitter.im/inflatablefriends/lastfm).

## Documentation

- [Api method progress report](PROGRESS.md)
- [Scrobbling](doc/scrobbling.md)
- [Dependency Injection](doc/dependency-injection.md)

## Planned Features

Everyone working with Last.fm is likely to need similar kinds of features, so it makes sense for us to work on them together. Here are a few things to look for in the future:

- **Fire-and-forget scrobbling**: transparent handling of scrobbles made offline
- **Built-in request cache for API calls**
- **Improved identification of poorly tagged tracks** using audio fingerprints and the MusicBrainz API

If you have any other neat ideas, [post them in our Gitter](https://gitter.im/inflatablefriends/lastfm) so we can talk :smile:

## Platform Compatibility

The current PCL profile is

- .NET Framework 4.5
- Windows 8.0
- Windows Phone 8.1
- Windows Phone Silverlight 8

If you need support for Mono, Windows Phone 7, or another .NET platform, please ask: the current profile was chosen to reduce dependencies for the most common use cases. The SDK should be trivial to port to any platform which supports Async, HttpClient and Json.Net.
