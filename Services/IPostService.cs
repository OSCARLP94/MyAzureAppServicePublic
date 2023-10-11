using System;
using MyAzureAppService.Entities.DTOS;

namespace MyAzureAppService.Services
{
	public interface IPostService
	{
		Task<dynamic> GetAll();

		Task<dynamic> Register(PostDTO post);
	}
}

