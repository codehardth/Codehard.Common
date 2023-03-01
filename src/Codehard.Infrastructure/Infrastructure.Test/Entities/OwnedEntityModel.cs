using LanguageExt;

namespace Infrastructure.Test.Entities;

public class OwnedEntity
{
    private int? value2;

    public string? Value1 { get; set; }

    public Option<int> Value2 { get; set; }
}