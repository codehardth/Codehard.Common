using System.Text.Json;
using Codehard.Common.DomainModel;
using Infrastructure.Test.Entities;
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
                options.UseNpgsql(
                    "Server=127.0.0.1;Port=5452;Database=TestDatabase;User Id=postgres;Password=Lt&R_6M6dR>=V6yz;IncludeErrorDetail=true;");
            });
        });
        // var optionBuilder = new DbContextOptionsBuilder<TestDbContext>();
        // optionBuilder.UseNpgsql(
        //     "Server=127.0.0.1;Port=5452;Database=TestDatabase;User Id=postgres;Password=Lt&R_6M6dR>=V6yz;IncludeErrorDetail=true;");
        //
        // var dbContext = new TestDbContext(optionBuilder.Options);
        // //
        // // var model = Model.Create();
        // //
        // // dbContext.Models.Add(model);
        // // dbContext.SaveChanges();
        //
        // var models = dbContext.Models.Where(
        //     m => m.Id.Equals(Guid.Parse("cc24ba41-8202-4492-8283-099dd797d828"))).ToList();

        var app = hostBuilder.Build();

        var dbContext = app.Services.GetRequiredService<TestDbContext>();

        // var model = MyModel.Create();
        // model.AddChild("123456");

        // dbContext.Models.Add(model);
        // dbContext.SaveChanges();

        var id = Guid.Parse("7dc343a9-9d3e-4c6a-8c80-aeb09bae6e3f");
        var models =
            dbContext.Models
                .Where(m => m.Id == new GuidKey(id)).ToList();

        var childs = models[0].Childs.ToList();

        var json = JsonSerializer.Serialize(models);

        var deserializedModel = JsonSerializer.Deserialize<MyModel[]>(json);
    }
}