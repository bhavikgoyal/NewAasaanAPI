using Microsoft.AspNetCore.Mvc;

namespace Aasaan_API.Security
{
  public class AuthenticationFailureResult : IActionResult
  {
    private readonly string _reasonPhrase;

    public AuthenticationFailureResult(string reasonPhrase)
    {
      _reasonPhrase = reasonPhrase;
    }

    public Task ExecuteResultAsync(ActionContext context)
    {
      var result = new ContentResult
      {
        StatusCode = 401,
        Content = _reasonPhrase
      };

      return result.ExecuteResultAsync(context);
    }
  }
}
