using LanguageExt.Common;

namespace Codehard.Functional.Tests
{
    public class AffWithExtTests
    {
        [Fact]
        public async Task WhenUseWith_ShouldMatchFinResultSuccessfully()
        {
            // Arrage
            var aAff = SuccessAff(1);
            var bAff = SuccessAff("1");
            
            // Act
            var abAff = aAff.With(bAff);
            var abFin = await abAff.Run();

            // Assert
            var ab = abFin.ThrowIfFail();
            Assert.Equal(1, ab.Item1);
            Assert.Equal("1", ab.Item2);
        }

        [Fact]
        public async Task WhenUseGuardWithTruePredicate_ShouldMatchFinResultSuccessfully()
        {
            // Arrage
            var aAff = SuccessAff(1);
            var bAff = SuccessAff("1");

            // Act
            var abAff = 
                aAff
                .With(bAff)
                .Guard(
                    ab => ab.Item1 == 1 && ab.Item2 == "1",
                    Error.New(1, "Wrong value!"));

            var abFin = await abAff.Run();

            // Assert
            var ab = abFin.ThrowIfFail();
            Assert.Equal(1, ab.Item1);
            Assert.Equal("1", ab.Item2);
        }

        [Fact]
        public async Task WhenUseGuardMultiArgsWithTruePredicate_ShouldMatchFinResultSuccessfully()
        {
            // Arrage
            var aAff = SuccessAff(1);
            var bAff = SuccessAff("1");

            // Act
            var abAff =
                aAff
                .With(bAff)
                .Guard(
                    (a, b) => a == 1 && b == "1",
                    Error.New(1, "Wrong value!"));

            var abFin = await abAff.Run();

            // Assert
            var ab = abFin.ThrowIfFail();
            Assert.Equal(1, ab.Item1);
            Assert.Equal("1", ab.Item2);
        }

        [Fact]
        public async Task WhenUseGuardWithFalsePredicate_ShouldMatchFinResultFail()
        {
            // Arrage
            var aAff = SuccessAff(1);
            var bAff = SuccessAff("1");

            // Act
            var abAff =
                aAff
                .With(bAff)
                .Guard(
                    ab => ab.Item1 == 2 && ab.Item2 == "2",
                    Error.New(1, "Wrong value!"));

            var abFin = await abAff.Run();

            // Assert
            Assert.True(abFin.IsFail);
        }

        [Fact]
        public async Task WhenUseGuardDeconstructTupleWithFalsePredicate_ShouldMatchFinResultFail()
        {
            // Arrage
            var aAff = SuccessAff(1);
            var bAff = SuccessAff("1");

            // Act
            var abAff =
                aAff
                .With(bAff)
                .Guard(
                    (a, b) => a == 2 && b == "2",
                    Error.New(1, "Wrong value!"));

            var abFin = await abAff.Run();

            // Assert
            Assert.True(abFin.IsFail);
        }
    }
}