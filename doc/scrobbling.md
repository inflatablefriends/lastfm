# Scrobbling

## Quickstart

Create the scrobbler class:
```c#
var auth = new LastAuth(key, secret);
var scrobbler = new Scrobbler(auth);
```

Or if using a SQLite database:
```c#
var scrobbler = new SQLiteScrobbler(auth, databasePath);
```

then

```c#
var scrobble = new Scrobble("65daysofstatic", "The Fall of Math", "Hole", DateTimeOffset.UtcNow);

var response = await scrobbler.ScrobbleAsync();

if (response.Success)
{
	// The scrobble was either successfully sent or has been cached to be sent later.
	bool scrobbleCached = response.Status == LastResponseStatus.Cached;
}
else
{
	if (response.Status == LastResponseStatus.RequestFailed)
	{
		// response.Exception contains info on the http request failing
	}
	else if (response.Status == LastResponseStatus.CacheFailed)
	{
		// response.Exception contains info on the caching mechanism failing
	}
}

```

## Extensibility

```IF.Lastfm.Core.Scrobblers.Scrobbler```, the core implementation of IScrobbler doesn't cache failed requests. 

The ```ScrobblerBase``` class provides the mechanism for caching scrobbles. Classes deriving it may implement scrobble caching using an external database - if the scrobble request fails, then it will be saved to a database to be sent later. Currently, any cached scrobbles can either be sent when ```ScrobbleAsync(Scrobble s)``` is next called, or independently by calling ```SendCachedScrobblesAsync()```.

### SQLite

The ```IF.Lastfm.SQLite.SQLiteScrobbler``` class enables scrobble caching using a SQLite database. 


This is in the NuGet package:

```
Install-Package Inflatable.Lastfm.SQLite
```

Dependencies:

- Newtonsoft.JSON
- Inflatable.Lastfm
- sqlite-net-pcl

