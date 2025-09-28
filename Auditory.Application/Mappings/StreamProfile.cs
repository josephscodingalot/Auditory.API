using AutoMapper;

namespace Auditory.Application.Mappings;

public class StreamProfile : Profile
{
    public StreamProfile()
    {
        CreateMap<DTO.Stream, Domain.Entities.Stream>()
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.username))
            .ForMember(dest => dest.Platform, opt => opt.MapFrom(src => src.platform))
            .ForMember(dest => dest.MsPlayed, opt => opt.MapFrom(src => src.ms_played))
            .ForMember(dest => dest.ConnCountry, opt => opt.MapFrom(src => src.conn_country))
            .ForMember(dest => dest.TrackName, opt => opt.MapFrom(src => src.master_metadata_track_name))
            .ForMember(dest => dest.ArtistName, opt => opt.MapFrom(src => src.master_metadata_album_artist_name))
            .ForMember(dest => dest.AlbumName, opt => opt.MapFrom(src => src.master_metadata_album_album_name))
            .ForMember(dest => dest.SpotifyTrackUri, opt => opt.MapFrom(src => src.spotify_track_uri))
            .ForMember(dest => dest.Timestamp, opt => opt.MapFrom(src => src.ts))
            .ForMember(dest => dest.Shuffle, opt => opt.MapFrom(src => src.shuffle))
            .ForMember(dest => dest.Skipped, opt => opt.MapFrom(src => src.skipped))
            .ForMember(dest => dest.Offline, opt => opt.MapFrom(src => src.offline))
            .ForMember(dest => dest.IncognitoMode, opt => opt.MapFrom(src => src.incognito_mode));
    }
}