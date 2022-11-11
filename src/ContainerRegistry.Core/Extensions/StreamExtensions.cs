using System.Security.Cryptography;
using System.Text;

namespace ContainerRegistry.Core.Extensions;

public static class StreamExtensions
{
    private const int DefaultCopyBufferSize = 81920;

    public static async Task<FileStream> AsTemporaryFileStreamAsync(
        this Stream original,
        CancellationToken cancellationToken = default)
    {
        var result = new FileStream(
            Path.GetTempFileName(),
            FileMode.Create,
            FileAccess.ReadWrite,
            FileShare.None,
            DefaultCopyBufferSize,
            FileOptions.DeleteOnClose);

        try
        {
            await original.CopyToAsync(result, DefaultCopyBufferSize, cancellationToken);
            result.Position = 0;
        }
        catch (Exception)
        {
            await result.DisposeAsync();
        }

        return result;
    }
    public static async Task<string> AsHashAsync(this Stream stream)
    {
        var sb = new StringBuilder();

        await using (var ms = await stream.AsTemporaryFileStreamAsync())
        {
            using (var hash = SHA256.Create())
            {
                var result = await hash.ComputeHashAsync(ms);

                foreach (var b in result)
                    sb.Append(b.ToString("x2"));
            }
        }

        return sb.ToString();
    }
}