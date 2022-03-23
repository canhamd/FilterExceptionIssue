using MassTransit;

namespace FilterExceptionIssue.WebApi.GeochronologyFeature.Commands.SaveGeochronology
{
    public class SaveGeochronologyConsumer : IConsumer<SaveGeochronology>
    {
        public SaveGeochronologyConsumer()
        {

        }

        public async Task Consume(ConsumeContext<SaveGeochronology> context)
        {
            await context.RespondAsync(context.Message.Geochronology);
        }
    }
}