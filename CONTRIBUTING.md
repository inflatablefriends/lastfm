# Contributing

The best way to contribute is to use the SDK in your application! Your feedback and testing will make it better for everyone.

This document explains the process of writing code and submitting it for inclusion in the SDK. You can ask any questions in our [Gitter chat](https://gitter.im/inflatablefriends/lastfm)

## Setup

First off, you will need 

- a version of Visual Studio that can compile portable class libraries - unfortunately this means you can't use an Express edition of Visual Studio
- a working install of Git - the easiest way is to install [Github for Windows](https://windows.github.com/)
- Fiddler or another web request inspector - set this up so you can inspect HTTPS requests (see step 4 of [this guide](http://rikk.it/blog/capture-windows-phone-8-network-traffic-with-fiddler/))

Once you have those installed, you can fork [the repo](https://github.com/inflatablefriends/lastfm), clone it to your machine and start work.

## Choosing something to work on

Right now we want to get all the API commands finished! So choose one [from this list](https://github.com/inflatablefriends/lastfm/blob/master/PROGRESS.md).

### Writing a command

The API is structured according to [the command pattern](http://en.wikipedia.org/wiki/Command_pattern). This is to reduce duplication of code and make testing easier.

Once you have chosen an API method to work on, you need to do five things before submitting it for inclusion in the API:

1. Create the command in the right folder
2. Implement the command, deriving from GetAsyncCommandBase<T> or PostAsyncCommandBase<T> (depending on what [the documentation](http://www.last.fm/api) says)
3. Create a method on the relevent *Api class
4. Collect sample responses from the API for this method
5. Create a unit test class for the command using the sample responses

So if I wanted to work on the method ```track.getSimilar``` I would:

1. Create the [GetSimilarTracksCommand](https://github.com/inflatablefriends/lastfm/blob/master/src/IF.Lastfm.Core/Api/Commands/TrackApi/GetSimilarTracksCommand.cs) class in /src/IF.Lastfm.Core/Api/Commands/TrackApi
2. Subclass GetAsyncCommandBase<T> because the documentation says the method does not need authentication. T will be a PageResponse<LastTrack> because the method returns a list of tracks.
3. Add a GetSimilarTracks method on the TrackApi class
4. Use the Syro dev tool to collect responses for: (page 0, limit 1), (page 0, limit 20) and a track which doesn't have similar responses
5. Create a class in the unit test project, and test the command execution with each of the sample responses.

Once all your tests pass and the project builds, commit your work to your repo, then send a pull request.

### Syro

IF.Lastfm.Syro is a tool to make building requests to the Last.fm JSON API easier. The main benefit is that it generates method signatures for authenticated commands, meaning you can get responses from an API call without having to build up the command first.

Using it is simple - build the solution and then run the IF.Lastfm.Syro.exe in src/IF.Lastfm.Syro/bin/Debug.

[![Syro app](https://github.com/inflatablefriends/lastfm/blob/master/res/syro.png)](https://github.com/inflatablefriends/lastfm/blob/master/res/syro.png)

The three dropdown menus correspond to the type of the command you want to build - so track.getSimilar corresponds to DummyGetAsyncCommand, PageResponse and LastTrack. The rest of the page corresponds to parameters on the request - check the documentation for what needs to be sent and fill in the data grid.

Clicking the execute button will build and execute the command and then save the JSON response to a file in the /tmp directory of the solution. 
