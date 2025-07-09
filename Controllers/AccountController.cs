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
          var data = _signInManager.SaveRegistrationData(registrationCLS);
          if (data != null)
          {
            response.code = 200;
            response.Data = data;
            response.message = "User registered successfully";
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

    [HttpPost("AdminLogIn")]
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
    public Response<List<ResponseUserModel>> LogInWithMobileAndDeviceId(string Mobile, string DeviceId)
    {
      Response<List<ResponseUserModel>> response = new Response<List<ResponseUserModel>>();
      List<ResponseUserModel> usersDetails = new List<ResponseUserModel>();

      try
      {
        var data = _signInManager.LogInWithMobileAndDeviceId(Mobile, DeviceId);

        if (data != null && data.Any()) 
        {
          foreach (var user in data)
          {
            var details = new ResponseUserModel
            {
              UserID = Convert.ToInt32(user.UserID),
              MobileNumber = user.MobileNumber,
              EmailID = user.EmailID,
              DateofCreation = user.DateofCreation,
              DeviceID = user.DeviceID,
              SubscriptionExpiryDate = user.SubscriptionExpiryDate,
              ExpiryDateApp = user.ExpiryDateApp,
              Platform = user.Platform,
              AppVersion = user.AppVersion,
              LastAPICallDate = user.LastAPICallDate,
              AdminNotes = user.AdminNotes,
              AppCode = user.AppCode,
              SubscriptionStatus = user.SubscriptionStatus
            };
            usersDetails.Add(details); 
          }

          response.code = 200;
          response.Data = usersDetails;
          response.message = "User already exist.";
        }
        else
        {
          response.code = 404;
          response.Data = null;
          response.message = "User not found.";
        }
      }
      catch (Exception ex)
      {
        response.code = 500;
        response.Data = null;
        response.message = "An internal server error occurred: " + ex.Message;
      }
      return response;
    }


    [HttpPost("UpdateAppVersion")]
    public Response<ResponseUpdateUserModel> UpdateAppVersion(UpdateAppVersionModel updateAppVersionModel)
    {
      Response<ResponseUpdateUserModel> response = new Response<ResponseUpdateUserModel>();
      ResponseUpdateUserModel details = new ResponseUpdateUserModel();
      try
      {
        details = _signInManager.UpdateAppVersion(updateAppVersionModel);
        if (details == null)
        {
          response.Data = null;
          response.message = "Data not updated, please try again";
          response.code = 0;
        }
        else
        {
          response.code = 200;
          response.Data = details;
          response.message = "Data updated successfully";
        }
      }
      catch (Exception ex)
      {
        response.code = 500;
        response.Data = null;
        response.message = ex.Message;
      }
      return response;
    }

  }
}
