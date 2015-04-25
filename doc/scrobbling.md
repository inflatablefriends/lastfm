# Scrobbling

## Features

- Easy scrobbling to Last.fm
- Scrobble multiple tracks at a time
- Automatic caching for scrobbles in the event of no network connection (if caching is enabled)
- Database independent design - easy to integrate the scrobble cache with your preferred database

## Quickstart

```IScrobbler``` provides an interface for scrobbler classes. The ```Inflatable.Lastfm``` package comes with a basic scrobbler that does not support caching; to get caching you can use the SQLiteScrobbler provided in ```Inflatable.Lastfm.SQLite``` package, or you can [integrate your own database](#extensibility).

Create the scrobbler class:
```c#
IScrobbler _scrobbler;

var auth = new LastAuth(key, secret, httpClient);
_scrobbler = new Scrobbler(auth, httpClient);
```

then

```c#
List<Scrobble> scrobbles;

var response = await _scrobbler.ScrobbleAsync(scrobbles); // scrobbles will be sent in batches of 50
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

If used in a music app, this code should be used immediately after a track has finished playing.

## Things to know

### Scrobbles are sent in batches of 50

The Last.fm API supports up to 50 scrobbles being sent at a time. This is handled for you when using an IScrobbler class from the SDK.

### There is a limit on how many scrobbles may be sent in each day

There must be at least 30 seconds in between each scrobble. This means you may scrobble at most ```(24 * 60 * 60) / 30 = 2880``` tracks in one day. If this limit is reached then the scrobbles will be cached if it's enabled.

### Scrobbling is opportunistic

As currently implemented, Scrobblers will only scrobble when the ScrobbleAsync() method is called. Any cached scrobbles will be sent then, and only then. As an illustration, this is a scenario involving our friendly test-subject, User:

- User is commuting to their employment at Business Corporation. They are listening to a playlist of track1, track2 and track3.
- The application is set up to call scrobbler.ScrobbleAsync() when a track is finished playing.
- User's train enters a tunnel. 
- When track1 finishes, User has no network connection. If caching is enabled, then the scrobble is cached.
- User's train exits the tunnel.
- When track2 finishes, User now has a network connection. The call to scrobbler.ScrobbleAsync() inspects its cache, and ends up sending both track1 and track2.
- User is content in the presence of accurate statistics.
- User's train enters another tunnel.
- Track3 finishes playing, but User has no network connection. The scrobble for track3 is cached for later.
- Since the User's playlist has ended, they duly unplug their headphones and exit the train.

At this point, track1 and track2 have been scrobbled - but track3 has not! Track3 is in the scrobble cache. It is not until User's commute, back to their residence in Townsville, when they listen to track4. Only when track4 is finished playing will track3 be scrobbled.

Solving this problem requires some sort of background task to empty the cache. You can write your own that calls ```scrobbler.SendCachedScrobblesAsync()```.

### Scrobbles have an age limit

Last.fm do not accept scrobbles which are older than two weeks (UTC). There's no point sending them, so Scrobbles passed to ScrobbleAsync() which are older than two weeks will be silently dropped.

Right now there is no way to know when this happens. At some point this information will be made available in the reponse object. (#issue number tba)

## Extensibility

The ScrobblerBase class may be extended in your own project to integrate with the database that suits you best. The derived class needs to implement ```GetCachedScrobblesAsync()```, which should return an IEnumerable<Scrobble>, and ```CacheAsync(Scrobble s)``` which should return LastResponseStatus.Cached if successful or throw an exception if not. Exceptions thrown in CacheAsync() will be caught and returned to the user as error codes (see: #5).

### SQLite

SQLite is the first supported database for scrobble caching.

```c#
IScrobbler _scrobbler;

var databasePath = "scrobbles.db"; // if you are already using SQLite, you may be able to use the same database file.
File.Create(databasePath); // there *must* be a file present at this location before calling ScrobbleAsync().

_scrobbler = new SQLiteScrobbler(auth, databasePath, httpClient);
```

To use the SQLiteScrobbler, install [Inflatable.Lastfm.SQLite]() from NuGet. This package will be updated according to major versions of sqlite-net-pcl.

```
Install-Package Inflatable.Lastfm.SQLite
```

Dependencies:

- Newtonsoft.JSON
- Inflatable.Lastfm
- sqlite-net-pcl

