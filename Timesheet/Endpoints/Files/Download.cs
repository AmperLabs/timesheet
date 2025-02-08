using Timesheet.Services;

namespace Timesheet.Endpoints.Files
{
    public class Download : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("api/files/{filename}", async (string filename, BlobService blobService) =>
            {
                var file = await blobService.DownloadFile(filename);

                return Results.File(file.Stream, file.ContentType);
            });
        }
    }
}
