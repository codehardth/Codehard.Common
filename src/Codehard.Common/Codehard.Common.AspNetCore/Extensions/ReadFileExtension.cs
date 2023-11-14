using Microsoft.AspNetCore.Http;

namespace Codehard.Common.AspNetCore.Extensions;

/// <summary>
/// Read file extension.
/// </summary>
public static class ReadFileExtension
{
    /// <summary>
    /// Read file as byte array.
    /// </summary>
    /// <param name="formFile"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public static async Task<byte[]> ReadFileAsync(this IFormFile formFile, CancellationToken cancellationToken)
    {
        await using var memoryStream = new MemoryStream();
        await formFile.CopyToAsync(memoryStream, cancellationToken);
        return memoryStream.ToArray();
    }
}