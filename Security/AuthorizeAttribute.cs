using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace Aasaan_API.Security
{
  [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
  public class AuthorizeAttribute : Attribute, IAuthorizationFilter
  {
    public string Roles { get; set; }
    public void OnAuthorization(AuthorizationFilterContext context)
    {
      var handler = new JwtSecurityTokenHandler();

      var auth = context.HttpContext.Request.Headers.Authorization.ToString();
      if (auth.Contains("Bearer"))
      {
        var arr = auth.Split(' ');
        var jsonToken = handler.ReadToken(arr[arr.Count() - 1]);
        var tokenS = jsonToken as JwtSecurityToken;
        string? UserId = tokenS.Claims?.SingleOrDefault(p => p.Type == "USID")?.Value;
        string? user = tokenS.Claims?.SingleOrDefault(p => p.Type == "Name")?.Value;
        //    string? userRole = tokenS.Claims?.SingleOrDefault(p => p.Type == "RoleId")?.Value;
        //;
        //var user = context.HttpContext.Items["User"];
        //var userRole = context.HttpContext.Items["UserRole"];
        //var UserId = context.HttpContext.Items["Usid"];
        GlobalUtilities.UserId = Convert.ToInt32(UserId);
        //if (Roles != Convert.ToString(userRole))
        //{
        //    context.Result = new JsonResult(new { message = "Not Access Permission" }) { StatusCode = StatusCodes.Status401Unauthorized };
        //}
        //Roles = Convert.ToString(userRole);
        if (user == null)
        {
          // not logged in
          context.Result = new JsonResult(new { message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };
        }
      }
      else
      {
        context.Result = new JsonResult(new { message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };
      }


    }
  }
}
