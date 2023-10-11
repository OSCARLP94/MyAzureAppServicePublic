using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyAzureAppService.Entities.DTOS;
using MyAzureAppService.Services;

namespace MyAzureAppService.Controllers
{
    [Produces("application/json")]
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class PostController : ControllerBase
	{
		private readonly IPostService _postService;

		public PostController(IPostService postService)
		{
			_postService = postService;
		}

        [Route(nameof(PostController.GetAll)), HttpGet]
        public async Task<dynamic> GetAll()
		{
			return await _postService.GetAll();
		}

		[Route(nameof(PostController.Register)), HttpPost]
		public async Task<dynamic> Register(PostDTO post)
		{
			return await _postService.Register(post);
		}
	}
}

