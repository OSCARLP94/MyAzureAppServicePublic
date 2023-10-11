using System;
namespace MyAzureAppService.Entities
{
	public class User
	{
		public User()
		{
		}

		public string Id { get; set; }
		public string Email { get; set; }
		public string Password { get; set; }
	}
}

