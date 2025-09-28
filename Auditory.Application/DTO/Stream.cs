namespace Auditory.Application.DTO;

public class Stream
{
    public DateTime ts { get; set; }
    public string username { get; set; }
    public string platform { get; set; }
    public int ms_played { get; set; }
    public string conn_country { get; set; }
    public string master_metadata_track_name { get; set; }
    public string master_metadata_album_artist_name { get; set; }
    public string master_metadata_album_album_name { get; set; }
    public string spotify_track_uri { get; set; }
    public bool shuffle { get; set; }
    public bool skipped { get; set; }
    public bool offline { get; set; }
    public bool incognito_mode { get; set; }
}