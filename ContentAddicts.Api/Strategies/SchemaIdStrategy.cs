namespace ContentAddicts.Api.Strategies;

public static class SchemaIdStrategy
{
    private static string RemovePrefix(string text, string prefix)
    {
        return text.StartsWith(prefix)
                ? text.Remove(text.IndexOf(prefix), prefix.Length)
                : text;
    }

    private static string RemoveSuffix(string text, string suffix)
    {
        return text.EndsWith(suffix)
                ? text.Remove(text.LastIndexOf(suffix), suffix.Length)
                : text;
    }

    public static string CleanStrategy(Type currentClass)
    {
        string className = currentClass.Name;

        className = RemoveSuffix(className, "Dto");
        className = RemoveSuffix(className, "Command");

        className = RemovePrefix(className, "GetAll");
        className = RemovePrefix(className, "Get");

        return className;
    }
}
