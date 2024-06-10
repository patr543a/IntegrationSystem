using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using IntegrationSystem.Infrastructure.Persistence.Interfaces.UnitOfWorks;

namespace IntegrationSystem.Presentation.InternalServices.XML_GRPC.Services;

/// <summary>
/// The gRPC service for XML.
/// </summary>
/// <param name="logger">The logger to use.</param>
/// <param name="unitOfWork">The unit of work to use.</param>
public class XmlService(ILogger<XmlService> logger, IIntegrationSystemUnitOfWork unitOfWork)
	: XML.XMLBase
{
	/// <summary>
	/// The logger.
	/// </summary>
	private readonly ILogger<XmlService> _logger = logger;

	/// <summary>
	/// The unit of work.
	/// </summary>
	private readonly IIntegrationSystemUnitOfWork _unitOfWork = unitOfWork;

	/// <summary>
	/// Gets the last changed date and time of the users.
	/// </summary>
	/// <param name="request">The request.</param>
	/// <param name="context">The request context.</param>
	/// <returns>The last changed date and time of the users.</returns>
	public override Task<XMLLastChangedReply> XMLLastChanged(XMLLastChangedRequest request, ServerCallContext context)
	{
		// Get the last modified time
		var time = _unitOfWork.XmlUserRepository.LastModified;

		// Return the last changed date and time
		return Task.FromResult(new XMLLastChangedReply
		{
			Time = Timestamp.FromDateTime(time),
			Day = time.Day,
			Month = time.Month,
			Year = time.Year
		});
	}

	/// <summary>
	/// Gets the users.
	/// </summary>
	/// <param name="request">The request.</param>
	/// <param name="context">The request context.</param>
	/// <returns>The users.</returns>
	public override async Task<XMLGetUsersReply> XMLGetUsers(XMLGetUsersRequest request, ServerCallContext context)
	{
		// Create the reply
		var reply = new XMLGetUsersReply();

		// Add the users
		reply.Users.AddRange((await _unitOfWork.XmlUserRepository.GetAllAsync()).Select(xu => new XMLUser
		{
			Number = xu.Number,
			Name = xu.Name,
			Title = xu.Title,
			Address = xu.Address,
			City = xu.City,
			Email = xu.Email,
			Mobile = xu.Mobile,
		}));

		return await Task.FromResult(reply);
	}
}
