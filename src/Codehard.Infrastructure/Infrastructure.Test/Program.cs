using Infrastructure.Test.Entities;
using LanguageExt;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Test;

public class Program
{
    public static void Main(string[] args)
    {
        var hostBuilder = Host.CreateDefaultBuilder(args);

        hostBuilder.ConfigureServices((ctx, sp) =>
        {
            sp.AddLogging(options => options.AddConsole());

            sp.AddDbContext<TestDbContext>(options =>
            {
                options
                    .UseNpgsql(
                        "Server=127.0.0.1;Port=5438;Database=TestDatabase;User Id=postgres;Password=postgres;IncludeErrorDetail=true;")
                    .AddOptionalTranslator();
            });
        });

        var app = hostBuilder.Build();

        var dbContext = app.Services.GetRequiredService<TestDbContext>();

        var some = Some(6);
        var none = Option<int>.None;
        var nullable = (int?)7;

        var q =
            dbContext.Models.Where(m =>
                    m.OwnedTestEntity.Value2 == none &&
                    m.Number == some ||
                    m.Number == none ||
                    m.Number != Optional(nullable) ||
                    // m.Number.IsSome &&
                    m.Number.IsNone ||
                    // m.Text.IsSome &&
                    m.Text.IsNone ||
                    m.Number == 1 &&
                    m.Number > 2 ||
                    m.Number < 3 &&
                    m.Number >= 4 ||
                    m.Number <= 5 &&
                    m.Text == "test" ||
                    EF.Functions.Contains(m.Text, "test") ||
                    EF.Functions.StartsWith(m.Text, "test") &&
                    EF.Functions.EndsWith(m.Text, "test") ||
                    EF.Functions.ToLower(m.Text) == "test" &&
                    EF.Functions.ToUpper(m.Text) == "test")
                .ToList();

        var model = MyModel.Create();
        model.AddChild("123456");

        dbContext.Models.Add(model);
        dbContext.SaveChanges();

        var loadedModel =
            dbContext.Models.FirstOrDefault(m => m.Number.IsSome || EF.Functions.Contains(m.Text, "test"));

        loadedModel.Number = Option<int>.None;
        loadedModel.Text = null;

        dbContext.Models.Update(loadedModel);
        dbContext.SaveChanges();

        dbContext.Models.Remove(loadedModel);
        dbContext.SaveChanges();
    }
}