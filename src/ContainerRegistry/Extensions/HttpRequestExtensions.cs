using ContainerRegistry.Core.Extensions;

namespace ContainerRegistry.Extensions;

public static class HttpRequestExtensions
{
    public static async Task<Stream> GetUploadStreamOrNullAsync(this HttpRequest request,
        CancellationToken cancellationToken)
    {
        Stream rawUploadStream = null;
        try
        {
            if (request.HasFormContentType && request.Form.Files.Count > 0)
            {
                rawUploadStream = request.Form.Files[0].OpenReadStream();
            }
            else
            {
                rawUploadStream = request.Body;
            }

            return await rawUploadStream?.AsTemporaryFileStreamAsync(cancellationToken);
        }
        finally
        {
            rawUploadStream?.Dispose();
        }
    }
}