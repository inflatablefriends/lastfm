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

Choose one of [the issues marked "up for grabs"](https://github.com/inflatablefriends/lastfm/labels/up%20for%20grabs), or one of the missing API methods [listed in the progress report](https://github.com/inflatablefriends/lastfm/blob/master/PROGRESS.md).

## Branching

In order to keep [the commit history](https://github.com/inflatablefriends/lastfm/commits/master) tidy, I might `git squash` commits from a PR before merging them in. This can cause problems with maintaining a fork: **if you send a PR from the master branch of your fork, then you will need to `git pull --force` after I merge in your changes**. To prevent this extra effort, you can either:

1. Create a branch especially for your work, then send the PR from this branch. This means that any changes I make can never conflict with the master branch of your fork.
2. Squash your own commits before the PR is merged in.
  1. Multiple commits per PR is totally fine, as long as each single commit explains what it does. "Adds tests" is not as good a commit message as "Adds tests and JSON responses for GetAlbumShoutsCommand", for example.

## Writing a command

The API is structured according to [the command pattern](http://en.wikipedia.org/wiki/Command_pattern). This is to reduce duplication of code and make testing easier.

Once you have chosen an API method to work on, you need to do five things before submitting it for inclusion in the API:

1. Create the command in the right folder
2. Implement the command, deriving from `GetAsyncCommandBase<T>` or `PostAsyncCommandBase<T>` (depending on what [the documentation](http://www.last.fm/api) says), and add an `ApiMethodNameAttribute` on the class, corresponding to the API method you are implementing. (for example, `[ApiMethodName("album.shout")]`)
3. Create a method on the relevant *Api class
4. Collect sample responses from the API for this method
5. Create a unit test class for the command using the sample responses

So if I wanted to work on the method `track.getSimilar` I would:

1. Create the [GetSimilarCommand](/src/IF.Lastfm.Core/Api/Commands/Track/GetSimilarCommand.cs) class in /src/IF.Lastfm.Core/Api/Commands/Track
2. Subclass `GetAsyncCommandBase<T>` because the documentation says the method does not need authentication. T will be a `PageResponse<LastTrack>` because the method returns a list of tracks.
3. Add a `GetSimilar` method on the `TrackApi` class
4. Use the Syro dev tool to collect responses for: (page 0, limit 1), (page 0, limit 20) and a failure case, i.e. when a track which doesn't have similar responses. The failure case might be hard to find for some methods, when this is the case the test doesn't matter too much so you may skip it.
5. Create a class in the unit test project, and test the command execution with each of the sample responses.

Once all your tests pass and the project builds, commit your work to your repo, then send a pull request.

## Tests

### Running tests

You will need the [.NET Core SDK](https://dotnet.microsoft.com/download) installed.

Visual Studio 201* should automatically discover the tests in the solution. For Visual Studio Code, you'll need to find a plugin compatible with NUnit tests.

You can also run tests from the command line. In the root folder, run `./run-tests.ps1`. This runs `dotnet test` in each test project folder (e.g. `/src/IF.Tests.Lastfm.Tests/`).

### Writing tests

Every command should have a corresponding unit test file when it makes sense; you can use the type of command as an indicator for which tests are necessary. In addition to the below, any edge cases (such as the timestamps for an album being represented in ms or s depending on which endpoint is called) should be covered.

#### `LastResponse`

- `ParametersCorrect()` - test that `command.SetParameters()` generates the expected output. If the command has multiple constructors or optional parameters these should all be covered in the test.

#### `LastResponse<T>`

As `LastResponse`, plus

- `HandleSuccess()` - test that `command.ExecuteAsync()` deserialises a typical successful response for this API response into the correct type.
- `HandleError()` - test that `command.ExecuteAsync()` deserialises a typical error response for this API correctly, including method code and message.

#### `PageResponse<T>`

As `LastResponse`, plus

- `HandleSuccessSingle()` - test that `command.ExecuteAsync()` deserialises a typical successful response with a single item for this API response correctly: the API doesn't use JSON array notation for single items.
- `HandleSuccessMultiple()` - test that `command.ExecuteAsync()` deserialises a typical successful response with multiple items for this API response correctly. Set the limit parameter to 2 for easier test writing.
- `HandleError()` - test that `command.ExecuteAsync()` deserialises a typical error response for this API correctly, including method code and message.

## Syro

`IF.Lastfm.Syro` is a tool to make building requests to the Last.fm JSON API easier, using the mechanisms in the core library for stuff like generating method signatures and authenticating.

Build the solution and then run the IF.Lastfm.Syro.exe in src/IF.Lastfm.Syro/bin/Debug.

[![Syro app](https://github.com/inflatablefriends/lastfm/blob/master/res/syro.png)](https://github.com/inflatablefriends/lastfm/blob/master/res/syro.png)

The three dropdown menus correspond to the type of the command you want to build - so track.getSimilar corresponds to `DummyGetAsyncCommand`, `PageResponse` and `LastTrack`. The rest of the page corresponds to parameters for that method - check the Last.fm API documentation for what needs to be sent and fill in the data grid.

Clicking the execute button will build and execute the command, and then save the JSON response to a file in the /tmp directory of the solution. 
