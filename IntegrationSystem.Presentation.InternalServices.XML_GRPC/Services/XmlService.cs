using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using IntegrationSystem.Presentation.InternalServices.XML_GRPC;

namespace IntegrationSystem.Presentation.InternalServices.XML_GRPC.Services;

public class XmlService(ILogger<XmlService> logger)
    : XML.XMLBase
{
    private readonly ILogger<XmlService> _logger = logger;

	public override Task<LastChangedReply> LastChanged(LastChangedRequest request, ServerCallContext context)
	{
		return Task.FromResult(new LastChangedReply
		{
			Time = Timestamp.FromDateTime(DateTime.UtcNow),
			Day = DateTime.UtcNow.Day,
			Month = DateTime.UtcNow.Month,
			Year = DateTime.UtcNow.Year
		});
	}

	public override Task<GetUsersReply> GetUsers(GetUsersRequest request, ServerCallContext context)
	{
		return Task.FromResult(new GetUsersReply
		{
		});
	}
}
