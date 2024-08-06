namespace NotesService.API.Auth;

using System.Threading.Tasks;

public interface IAuthenticationService
{
    Task<AuthenticationResult> RegisterAsync(string email, string password);

    Task<AuthenticationResult> LoginAsync(string email, string password);
}
