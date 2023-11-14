using Codehard.Common.AspNetCore.Extensions;
using Microsoft.AspNetCore.Http;
using Moq;

namespace Codehard.Common.AspNetCore.Test;

public class ReadFileExtensionTest
{
    [Fact]
    public async Task ReadFileAsync_ShouldReturnByteArray()
    {
        // Arrange
        var formFile = new Mock<IFormFile>();
        var cancellationToken = new CancellationToken();
        var content = "Hello World from a Fake File";
        var ms = new MemoryStream();
        var writer = new StreamWriter(ms);
        await writer.WriteAsync(content);
        await writer.FlushAsync();
        ms.Position = 0;
        formFile.Setup(file => file.CopyToAsync(It.IsAny<Stream>(), cancellationToken))
                .Callback<Stream, CancellationToken>((stream, token) => ms.CopyTo(stream))
                .Returns(Task.CompletedTask);

        // Act
        var result = await formFile.Object.ReadFileAsync(cancellationToken);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(ms.Length, result.Length);
        Assert.IsType<byte[]>(result);
    }
}