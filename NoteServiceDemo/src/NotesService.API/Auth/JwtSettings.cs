namespace NotesService.API.Auth;

public class JwtSettings
{
    internal const string ConfigurationKey = nameof(JwtSettings);

    // TODO: Use User Secrets for DEV and some secret service/ key vault in prod.
    public string Secret { get; set; }

    public int ExpiryMinutes { get; set; }

    public string Issuer { get; set; }
}
