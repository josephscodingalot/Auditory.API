using MediatR;
using Stream = Auditory.Domain.Entities.Stream;

namespace Auditory.Application.Queries;

public record GetStreamsByUserQuery(string userName) : IRequest<IEnumerable<Stream>>;