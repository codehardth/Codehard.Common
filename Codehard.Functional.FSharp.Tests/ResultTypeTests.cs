using CodeHard.Function.FSharp.Tests.Types;
using LanguageExt.Common;

namespace Codehard.Functional.FSharp.Tests
{
    public class ResultTypeTests
    {
        [Fact]
        public async Task WhenConvertOkResultToAff_ShouldRunToSuccess()
        {
            // Arrange
            var fSharpResult = ResultType.getOkResult();

            // Act
            var aff = fSharpResult.ToAff(ResultType.mapError);
            var result = (await aff.Run()).ThrowIfFail();

            // Arrange
            Assert.Equal(0, result);
        }

        [Fact]
        public async Task WhenConvertErrorResultToAff_ShouldRunToFail()
        {
            // Arrange
            var fSharpResult = ResultType.getErrorResult();

            // Act
            var aff = fSharpResult.ToAff(ResultType.mapError);
            var result = await aff.Run();

            // Arrange
            Assert.Throws<ErrorException>(
                () => result.ThrowIfFail());
        }
    }
}