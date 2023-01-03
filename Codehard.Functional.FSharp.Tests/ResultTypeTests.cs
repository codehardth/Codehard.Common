using CodeHard.Function.FSharp.Tests.Types;
using LanguageExt.Common;
using static Codehard.Functional.FSharp.Prelude;

namespace Codehard.Functional.FSharp.Tests
{
    public class ResultTypeTests
    {
        [Fact]
        public void WhenConvertOkResultToFin_ShouldContainSuccessValue()
        {
            // Arrange
            var fSharpResult = ResultType.getOkResult();

            // Act
            var fin = fSharpResult.ToFin(ResultType.mapError);
            var result = fin.ThrowIfFail();

            // Assert
            Assert.Equal(0, result);
        }

        [Fact]
        public void WhenConvertErrorResultToFin_ShouldContainFailValue()
        {
            // Arrange
            var fSharpResult = ResultType.getErrorResult();

            // Act
            var fin = fSharpResult.ToFin(ResultType.mapError);

            // Assert
            Assert.Throws<ExpectedException>(
                () => fin.ThrowIfFail());
        }
        
        [Fact]
        public void WhenConvertOkResultToEff_ShouldRunToSuccess()
        {
            // Arrange
            var fSharpResult = ResultType.getOkResult();

            // Act
            var eff = fSharpResult.ToEff(ResultType.mapError);
            var result = eff.Run().ThrowIfFail();

            // Assert
            Assert.Equal(0, result);
        }

        [Fact]
        public async Task WhenConvertErrorResultToEff_ShouldRunToFail()
        {
            // Arrange
            var fSharpResult = ResultType.getErrorResult();

            // Act
            var eff = fSharpResult.ToEff(ResultType.mapError);
            var result = eff.Run();

            // Assert
            Assert.Throws<ExpectedException>(
                () => result.ThrowIfFail());
        }
        
        [Fact]
        public async Task WhenConvertOkResultToAff_ShouldRunToSuccess()
        {
            // Arrange
            var fSharpResult = ResultType.getOkResult();

            // Act
            var aff = fSharpResult.ToAff(ResultType.mapError);
            var result = (await aff.Run()).ThrowIfFail();

            // Assert
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

            // Assert
            Assert.Throws<ExpectedException>(
                () => result.ThrowIfFail());
        }
        
        [Fact]
        public async Task WhenConvertTaskOfOkResultToAff_ShouldRunToSuccess()
        {
            // Arrange
            var fSharpResult = Task.FromResult(ResultType.getOkResult());

            // Act
            var aff = fSharpResult.ToAff(ResultType.mapError);
            var result = (await aff.Run()).ThrowIfFail();

            // Assert
            Assert.Equal(0, result);
        }

        [Fact]
        public async Task WhenConvertTaskOfErrorResultToAff_ShouldRunToFail()
        {
            // Arrange
            var fSharpResult = Task.FromResult(ResultType.getErrorResult());

            // Act
            var aff = fSharpResult.ToAff(ResultType.mapError);
            var result = await aff.Run();

            // Assert
            Assert.Throws<ExpectedException>(
                () => result.ThrowIfFail());
        }
        
        [Fact]
        public void WhenWrapOkResultInEff_ShouldRunToSuccess()
        {
            // Act
            var eff = Eff(
                ResultType.getOkResult,
                ResultType.mapError);
            
            var result = eff.Run().ThrowIfFail();

            // Assert
            Assert.Equal(0, result);
        }
        
        [Fact]
        public void WhenWrapErrorResultInEff_ShouldRunToFail()
        {
            // Act
            var eff = Eff(
                ResultType.getErrorResult,
                ResultType.mapError);
            
            var result = eff.Run();

            // Assert
            Assert.Throws<ExpectedException>(
                () => result.ThrowIfFail());
        }
        
        [Fact]
        public async Task WhenWrapTaskOfOkResultInAff_ShouldRunToSuccess()
        {
            // Act
            var eff = Aff(
                () => Task.FromResult(ResultType.getOkResult()),
                ResultType.mapError);
            
            var result = (await eff.Run()).ThrowIfFail();

            // Assert
            Assert.Equal(0, result);
        }
        
        [Fact]
        public async Task WhenWrapTaskOfErrorResultInAff_ShouldRunToFail()
        {
            // Act
            var eff = Aff(
                () => Task.FromResult(ResultType.getErrorResult()),
                ResultType.mapError);
            
            var result = await eff.Run();

            // Assert
            Assert.Throws<ExpectedException>(
                () => result.ThrowIfFail());
        }
    }
}