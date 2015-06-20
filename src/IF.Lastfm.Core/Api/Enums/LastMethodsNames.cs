using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IF.Lastfm.Core.Enums
{
    /// <summary>
    /// Auto-generated class containing all the Last.Fm Api methods names.
    /// </summary>
    /// <remarks>
    /// Generated using the following code:
    /// var allMethods = ProgressReport.GetApiMethods().SelectMany(x => x.Value).OrderBy(x => x);
    /// var getValidName = new Func<string, string>(x => x.Replace('.', '_'));
    /// var getConstDefinition = new Func<string, string>(x => string.Format("internal const string {0} = \"{1}\";", getValidName(x), x));
    /// var allMethodsAsConstDefinitions = "internal static class LastMethodsNames\n{" + string.Join("\n", allMethods.Select(x => getConstDefinition(x))) + "}";
    /// </remarks>
    internal static class LastMethodsNames
    {
        // Warning: not listed on the Api documentation page!
        internal const string album_shout = "album.shout";
        internal const string track_shout = "track.shout";

        internal const string album_addTags = "album.addTags";
        internal const string album_getBuylinks = "album.getBuylinks";
        internal const string album_getInfo = "album.getInfo";
        internal const string album_getShouts = "album.getShouts";
        internal const string album_getTags = "album.getTags";
        internal const string album_getTopTags = "album.getTopTags";
        internal const string album_removeTag = "album.removeTag";
        internal const string album_search = "album.search";
        internal const string album_share = "album.share";
        internal const string artist_addTags = "artist.addTags";
        internal const string artist_getCorrection = "artist.getCorrection";
        internal const string artist_getEvents = "artist.getEvents";
        internal const string artist_getInfo = "artist.getInfo";
        internal const string artist_getPastEvents = "artist.getPastEvents";
        internal const string artist_getPodcast = "artist.getPodcast";
        internal const string artist_getShouts = "artist.getShouts";
        internal const string artist_getSimilar = "artist.getSimilar";
        internal const string artist_getTags = "artist.getTags";
        internal const string artist_getTopAlbums = "artist.getTopAlbums";
        internal const string artist_getTopFans = "artist.getTopFans";
        internal const string artist_getTopTags = "artist.getTopTags";
        internal const string artist_getTopTracks = "artist.getTopTracks";
        internal const string artist_removeTag = "artist.removeTag";
        internal const string artist_search = "artist.search";
        internal const string artist_share = "artist.share";
        internal const string artist_shout = "artist.shout";
        internal const string auth_getMobileSession = "auth.getMobileSession";
        internal const string auth_getSession = "auth.getSession";
        internal const string auth_getToken = "auth.getToken";
        internal const string chart_getHypedArtists = "chart.getHypedArtists";
        internal const string chart_getHypedTracks = "chart.getHypedTracks";
        internal const string chart_getLovedTracks = "chart.getLovedTracks";
        internal const string chart_getTopArtists = "chart.getTopArtists";
        internal const string chart_getTopTags = "chart.getTopTags";
        internal const string chart_getTopTracks = "chart.getTopTracks";
        internal const string event_attend = "event.attend";
        internal const string event_getAttendees = "event.getAttendees";
        internal const string event_getInfo = "event.getInfo";
        internal const string event_getShouts = "event.getShouts";
        internal const string event_share = "event.share";
        internal const string event_shout = "event.shout";
        internal const string geo_getEvents = "geo.getEvents";
        internal const string geo_getMetroArtistChart = "geo.getMetroArtistChart";
        internal const string geo_getMetroHypeArtistChart = "geo.getMetroHypeArtistChart";
        internal const string geo_getMetroHypeTrackChart = "geo.getMetroHypeTrackChart";
        internal const string geo_getMetros = "geo.getMetros";
        internal const string geo_getMetroTrackChart = "geo.getMetroTrackChart";
        internal const string geo_getMetroUniqueArtistChart = "geo.getMetroUniqueArtistChart";
        internal const string geo_getMetroUniqueTrackChart = "geo.getMetroUniqueTrackChart";
        internal const string geo_getMetroWeeklyChartlist = "geo.getMetroWeeklyChartlist";
        internal const string geo_getTopArtists = "geo.getTopArtists";
        internal const string geo_getTopTracks = "geo.getTopTracks";
        internal const string group_getHype = "group.getHype";
        internal const string group_getMembers = "group.getMembers";
        internal const string group_getWeeklyAlbumChart = "group.getWeeklyAlbumChart";
        internal const string group_getWeeklyArtistChart = "group.getWeeklyArtistChart";
        internal const string group_getWeeklyChartList = "group.getWeeklyChartList";
        internal const string group_getWeeklyTrackChart = "group.getWeeklyTrackChart";
        internal const string library_addAlbum = "library.addAlbum";
        internal const string library_addArtist = "library.addArtist";
        internal const string library_addTrack = "library.addTrack";
        internal const string library_getAlbums = "library.getAlbums";
        internal const string library_getArtists = "library.getArtists";
        internal const string library_getTracks = "library.getTracks";
        internal const string library_removeAlbum = "library.removeAlbum";
        internal const string library_removeArtist = "library.removeArtist";
        internal const string library_removeScrobble = "library.removeScrobble";
        internal const string library_removeTrack = "library.removeTrack";
        internal const string playlist_addTrack = "playlist.addTrack";
        internal const string playlist_create = "playlist.create";
        internal const string radio_getPlaylist = "radio.getPlaylist";
        internal const string radio_search = "radio.search";
        internal const string radio_tune = "radio.tune";
        internal const string tag_getInfo = "tag.getInfo";
        internal const string tag_getSimilar = "tag.getSimilar";
        internal const string tag_getTopAlbums = "tag.getTopAlbums";
        internal const string tag_getTopArtists = "tag.getTopArtists";
        internal const string tag_getTopTags = "tag.getTopTags";
        internal const string tag_getTopTracks = "tag.getTopTracks";
        internal const string tag_getWeeklyArtistChart = "tag.getWeeklyArtistChart";
        internal const string tag_getWeeklyChartList = "tag.getWeeklyChartList";
        internal const string tag_search = "tag.search";
        internal const string tasteometer_compare = "tasteometer.compare";
        internal const string tasteometer_compareGroup = "tasteometer.compareGroup";
        internal const string track_addTags = "track.addTags";
        internal const string track_ban = "track.ban";
        internal const string track_getBuylinks = "track.getBuylinks";
        internal const string track_getCorrection = "track.getCorrection";
        internal const string track_getFingerprintMetadata = "track.getFingerprintMetadata";
        internal const string track_getInfo = "track.getInfo";
        internal const string track_getShouts = "track.getShouts";
        internal const string track_getSimilar = "track.getSimilar";
        internal const string track_getTags = "track.getTags";
        internal const string track_getTopFans = "track.getTopFans";
        internal const string track_getTopTags = "track.getTopTags";
        internal const string track_love = "track.love";
        internal const string track_removeTag = "track.removeTag";
        internal const string track_scrobble = "track.scrobble";
        internal const string track_search = "track.search";
        internal const string track_share = "track.share";
        internal const string track_unban = "track.unban";
        internal const string track_unlove = "track.unlove";
        internal const string track_updateNowPlaying = "track.updateNowPlaying";
        internal const string user_getArtistTracks = "user.getArtistTracks";
        internal const string user_getBannedTracks = "user.getBannedTracks";
        internal const string user_getEvents = "user.getEvents";
        internal const string user_getFriends = "user.getFriends";
        internal const string user_getInfo = "user.getInfo";
        internal const string user_getLovedTracks = "user.getLovedTracks";
        internal const string user_getNeighbours = "user.getNeighbours";
        internal const string user_getNewReleases = "user.getNewReleases";
        internal const string user_getPastEvents = "user.getPastEvents";
        internal const string user_getPersonalTags = "user.getPersonalTags";
        internal const string user_getPlaylists = "user.getPlaylists";
        internal const string user_getRecentStations = "user.getRecentStations";
        internal const string user_getRecentTracks = "user.getRecentTracks";
        internal const string user_getRecommendedArtists = "user.getRecommendedArtists";
        internal const string user_getRecommendedEvents = "user.getRecommendedEvents";
        internal const string user_getShouts = "user.getShouts";
        internal const string user_getTopAlbums = "user.getTopAlbums";
        internal const string user_getTopArtists = "user.getTopArtists";
        internal const string user_getTopTags = "user.getTopTags";
        internal const string user_getTopTracks = "user.getTopTracks";
        internal const string user_getWeeklyAlbumChart = "user.getWeeklyAlbumChart";
        internal const string user_getWeeklyArtistChart = "user.getWeeklyArtistChart";
        internal const string user_getWeeklyChartList = "user.getWeeklyChartList";
        internal const string user_getWeeklyTrackChart = "user.getWeeklyTrackChart";
        internal const string user_shout = "user.shout";
        internal const string user_signUp = "user.signUp";
        internal const string user_terms = "user.terms";
        internal const string venue_getEvents = "venue.getEvents";
        internal const string venue_getPastEvents = "venue.getPastEvents";
        internal const string venue_search = "venue.search";
    }
}
