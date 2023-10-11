using System;
using Newtonsoft.Json;

namespace MyAzureAppService.Entities
{
	public class Post
	{
		public Post()
		{
			Id = Guid.NewGuid().ToString();
			RegisterDate = DateTime.UtcNow;
		}

        [JsonProperty("id")]
        public string Id { get; set; }

		public string IdUser { get; set; }

		public string Title { get; set; }

		public string Description { get; set; }

		public DateTime RegisterDate { get; set; }
	}
}

