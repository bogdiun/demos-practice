namespace NotesService.API.Authentication;

public class JwtSettings
{
    internal const string Key = nameof(JwtSettings);

    public string Secret { get; set; }
}
