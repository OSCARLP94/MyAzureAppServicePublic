using System;
namespace MyAzureAppService.Entities.DTOS
{
	public class BlobEventDTO
	{
        public string Id { get; set; }
        public string Topic { get; set; }
        public string Subject { get; set; }
        public string EventType { get; set; }
        public DateTime EventTime { get; set; }
        public object Data { get; set; }
    }

    public class BlobCreatedEventData
    {
        public string Api { get; set; }
        public string ClientRequestId { get; set; }
        public string RequestId { get; set; }
        public BlobCreatedEventDataDetails Details { get; set; }
    }

    public class BlobCreatedEventDataDetails
    {
        public string ContainerName { get; set; }
        public string BlobType { get; set; }
        public string Url { get; set; }
    }

    public class SubscriptionValidationEventData
    {
        public string ValidationCode { get; set; }
        public string ValidationUrl { get; set; }
    }
}

