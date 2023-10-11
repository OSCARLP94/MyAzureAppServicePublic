using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace MyAzureAppService.Entities.DTOS
{
	[DataContract]
	public class PostDTO
	{
		[Required]
		public string User { get; set; }

		[Required]
		public string Title { get; set; }

		public string Description { get; set; }
	}
}

