using System.Linq.Expressions;
using Codehard.Functional.EntityFramework;
using Infrastructure.Test.Entities;
using LanguageExt;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Test;

public class Program
{
    public class DelegateDecompilerQueryPreprocessor : IQueryExpressionInterceptor
    {
        Expression IQueryExpressionInterceptor.QueryCompilationStarting(
            Expression queryExpression,
            QueryExpressionEventData eventData)
        {
            var exprVisitor = new OptionExpressionVisitor();

            var expression = exprVisitor.Visit(queryExpression);

            return expression;
        }
    }

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
                options.AddInterceptors(new DelegateDecompilerQueryPreprocessor());
            });
        });

        var app = hostBuilder.Build();

        var dbContext = app.Services.GetRequiredService<TestDbContext>();

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