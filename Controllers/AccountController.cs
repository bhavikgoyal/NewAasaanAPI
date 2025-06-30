using Aasaan_API.IServices;
using Aasaan_API.Models;
using Aasaan_API.Security;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Aasaan_API.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class AccountController : ControllerBase
  {
    private readonly IAccount _signInManager;

    public AccountController(IAccount signInManager)
    {
      _signInManager = signInManager;
    }

    [HttpPost("SaveRegistrationData")]
    public Response<ResponseRegistrationCLS> SaveRegistrationData(RequestRegistrationCLS registrationCLS)
    {
      Response<ResponseRegistrationCLS> response = new Response<ResponseRegistrationCLS>();
      try
      {
        if (registrationCLS == null)
        {
          response.code = 400;
          response.Data = null;
          response.message = "All field is required";
          return response;
        }
        else
        {
          var create = _signInManager.LogInWithMobileAndDeviceId(registrationCLS.MobileNumber, registrationCLS.DeviceID);
         
          if (create != null)
          {
            var newcreate = new ResponseRegistrationCLS
            {
              UserID = create.UserID,
              MobileNumber = create.MobileNumber,   
              EmailID = create.EmailID,
              DateofCreation = create.DateofCreation,
              DeviceID = create.DeviceID,
              SubscriptionExpiryDate = create.SubscriptionExpiryDate,
              Platform = create.Platform,
              AppVersion = create.AppVersion,
              LastAPICallDate = create.LastAPICallDate,
              AdminNotes = create.AdminNotes,
              AppCode = create.AppCode,
              SubscriptionStatus = create.SubscriptionStatus
            };

            response.code = 200;
            response.Data = null;
            response.message = "User already registered";
            return response;
          }
          var data = _signInManager.SaveRegistrationData(registrationCLS);
          if (data != null)
          {
            response.code = 200;
            response.Data = data;
            response.message = "User registered successfully";
            return response;
          }
        }
      }
      catch(Exception ex) 
      {
        response.code = 500;
        response.Data = null;
        response.message = ex.Message;
      }
      return response;
    }

    [HttpPost("LogIn")]
    public Response<LoginModel> Login(string Email, string Password) 
    {
      Response<LoginModel> response = new Response<LoginModel>();
      LoginModel usersDetails = new LoginModel();
      try
      {
        usersDetails = _signInManager.Login(Email, Password);
        if (usersDetails == null)
        {
          response.Data = null;
          response.message = "Invalid Login Credential";
          response.code = 200;
        }
        else
        {
          var token = TokenManager.GenerateToken(usersDetails.Email, usersDetails.Password, 1, Convert.ToInt32(usersDetails.UserID));
          usersDetails.Token = token;
          response.code = 200;
          response.Data = usersDetails;
          response.message = "Data Listing";
        }
      }
      catch(Exception ex)
      {
        response.code = 500;
        response.Data = null;
        response.message = ex.Message;
      }
      return response;
    }

    [HttpPost("LogInWithMobileAndDeviceId")]
    public Response<ResponseUserModel> LogInWithMobileAndDeviceId(string Mobile, string DeviceId)
    {
      Response<ResponseUserModel> response = new Response<ResponseUserModel>();
      ResponseUserModel usersDetails = new ResponseUserModel();
      try
      {
        usersDetails = _signInManager.LogInWithMobileAndDeviceId(Mobile, DeviceId);
        if (usersDetails == null)
        {
          response.code = 200;
          response.Data = null;
          response.message = "Invalid Login Credential";          
        }
        else
        {
          var token = TokenManager.GenerateTokens(usersDetails.MobileNumber, usersDetails.DeviceID, 1, Convert.ToInt32(usersDetails.UserID));
          usersDetails.Token = token;
          response.code = 200;
          response.Data = usersDetails;
          response.message = "Data fetched successfully";
        }
      }
      catch(Exception ex) 
      {
        response.code = 500;
        response.Data = null;
        response.message = ex.Message;
      }
      return response;
    }

  }
}
