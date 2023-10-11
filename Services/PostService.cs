using System;
using Microsoft.ApplicationInsights;
using MyAzureAppService.DataAccess.Repositories;
using MyAzureAppService.Entities;
using MyAzureAppService.Entities.DTOS;

namespace MyAzureAppService.Services
{
    public class PostService : IPostService
    {
        private readonly ICosmoRepository<Post> _postRepository;

        public PostService(ICosmoRepository<Post> postRepository)
        {
            _postRepository = postRepository;
        }

        public async Task<dynamic> GetAll()
        {
            try
            {
                var data= await _postRepository.ListData();

                return ExceptionLib.Response.Successful(data);
            }
            catch(Exception ex)
            {
                // Registrar la excepción en Application Insights
                TelemetryClient telemetry = new TelemetryClient();
                telemetry.TrackException(ex);

                return ExceptionLib.Response.WithError(ex.Message);
            }
        }

        public async Task<dynamic> Register(PostDTO post)
        {
            try
            {
                Post item = new();
                item.IdUser = post.User;
                item.Title = post.Title;
                item.Description = post.Description;

                var response = await _postRepository.Register(item, item.IdUser);
                return ExceptionLib.Response.Successful(response);
            }
            catch (Exception ex)
            {
                // Registrar la excepción en Application Insights
                TelemetryClient telemetry = new TelemetryClient();
                telemetry.TrackException(ex);

                return ExceptionLib.Response.WithError(ex.Message);
            }
        }
    }
}

