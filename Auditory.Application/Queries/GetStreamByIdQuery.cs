using MediatR;

namespace Auditory.Application.Queries;

public record GetStreamByIdQuery(Guid StreamId) : IRequest<Stream>, IRequest<Domain.Entities.Stream>;