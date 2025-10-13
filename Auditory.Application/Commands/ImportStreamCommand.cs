using MediatR;
using Stream = Auditory.Domain.Entities;

namespace Auditory.Application.Commands;

public record ImportStreamCommand(List<Stream.Stream> spotifyData) : IRequest<List<Stream.Stream>>;
