namespace Auditory.Domain.Entities;

public class Stream
{
    public Guid Id { get; set; } = Guid.NewGuid();

    // Required
    public string UserName { get; set; }
    public string Platform { get; set; }
    public int MsPlayed { get; set; }
    public string ConnCountry { get; set; }
    public string TrackName { get; set; }
    public string ArtistName { get; set; }
    public string AlbumName { get; set; }
    public string SpotifyTrackUri { get; set; }
    public DateTime Timestamp { get; set; }

    // Optional metadata
    public string? IpAddress { get; private set; }
    public string? UserAgent { get; private set; }
    public string? EpisodeName { get; private set; }
    public string? EpisodeShowName { get; private set; }
    public string? SpotifyEpisodeUri { get; private set; }
    public string? ReasonStart { get; private set; }
    public string? ReasonEnd { get; private set; }

    // Flags
    public bool Shuffle { get; private set; }
    public bool Skipped { get; private set; }
    public bool Offline { get; private set; }
    public long OfflineTimestamp { get; private set; }
    public bool IncognitoMode { get; private set; }

    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

    public Stream()
    {
        
    }

    // Constructors
    public Stream(
        string userName,
        string platform,
        int msPlayed,
        string connCountry,
        string trackName,
        string artistName,
        string albumName,
        string spotifyTrackUri,
        DateTime timestamp
    )
    {
        if (string.IsNullOrWhiteSpace(userName))
            throw new ArgumentException("UserName cannot be empty");
        if (msPlayed < 0)
            throw new ArgumentException("MsPlayed must be >= 0");
        if (timestamp == default)
            throw new ArgumentException("Timestamp is required");

        UserName = userName;
        Platform = platform;
        MsPlayed = msPlayed;
        ConnCountry = connCountry;
        TrackName = trackName;
        ArtistName = artistName;
        AlbumName = albumName;
        SpotifyTrackUri = spotifyTrackUri;
        Timestamp = timestamp;
    }
    
    // Example behaviors
    public void MarkAsSkipped() => Skipped = true;
    public bool IsValidStream() => MsPlayed > 30000; // only count >30s as "valid play"
} 