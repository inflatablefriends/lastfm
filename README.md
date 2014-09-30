#Last.fm Portable .NET API

[![Build status](https://ci.appveyor.com/api/projects/status/c8gg2cw4jibbsg3u)](https://ci.appveyor.com/project/rikkit/lastfm)

MIT licensed. Maintained by [@rikkilt](http://twitter.com/rikkilt).

Feature request? Bug? Raise a [new issue on GitHub](https://github.com/rikkit/lastfm-wp/issues/new).

Wanna help? Check out [the open issues](https://github.com/rikkit/lastfm-wp/issues).

## Quickstart

``` c#
var auth = new Auth("apikey", "apisecret");
var response = await auth.GetSessionTokenAsync("username", "pass");

if (response.Success && auth.HasAuthenticated) {
	var albumApi = new AlbumApi(auth);
	var visions = await albumApi.GetAlbumInfoAsync("Grimes", "Visions");
}
```

Check out the [GitHub wiki](https://github.com/rikkit/lastfm-wp/wiki) for more documentation
