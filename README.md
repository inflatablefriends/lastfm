# Inflatable Last.fm .NET SDK [![Build status](https://ci.appveyor.com/api/projects/status/c8gg2cw4jibbsg3u)](https://ci.appveyor.com/project/rikkit/lastfm)

MIT licensed. Maintained by [@rikkilt](http://twitter.com/rikkilt).

Feature request? Bug? Or just wanna help out? Check out [the issues on GitHub](https://github.com/inflatablefriends/lastfm/issues).

If you have comments or need some help, just post to our chat room on [![Gitter](https://badges.gitter.im/Join Chat.svg)](https://gitter.im/inflatablefriends/lastfm?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge)

## Quickstart

```c#
var auth = new Auth("apikey", "apisecret");
var response = await auth.GetSessionTokenAsync("username", "pass");

if (response.Success && auth.HasAuthenticated) {
	var albumApi = new AlbumApi(auth);
	var visions = await albumApi.GetAlbumInfoAsync("Grimes", "Visions");
}
```

Some documentation is available on the [GitHub wiki](https://github.com/rikkit/lastfm-wp/wiki), but hopefully the source is good enough to document itself!

## Project Goals

- Provide complete, and completely tested, bindings for the Last.fm REST API for use on modern .NET platforms.
- Provide functionality beyond mere API bindings, to spread the joy of Last.fm to developers everywhere.
- To be the very best, like no-one ever was.

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
