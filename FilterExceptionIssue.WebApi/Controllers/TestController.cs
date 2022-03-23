using FilterExceptionIssue.WebApi.Common.Models;
using FilterExceptionIssue.WebApi.GeochronologyFeature.Commands.SaveGeochronology;
using MassTransit;
using Microsoft.AspNetCore.Mvc;

namespace FilterExceptionIssue.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        private readonly ILogger<TestController> _logger;

        public TestController(ILogger<TestController> logger)
        {
            _logger = logger;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> SaveGeochronology([FromServices] IRequestClient<SaveGeochronology> requestClient)
        {
            try
            {
                var e = await requestClient.GetResponse<Geochronology>(new
                {
                    Geochronology = new Geochronology { Id = 1, Title = "" }
                });

                return Ok(e);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}