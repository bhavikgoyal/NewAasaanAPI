using Aasaan_API.IServices;
using Aasaan_API.Models;
using Aasaan_API.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.Runtime.InteropServices;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Aasaan_API.Controllers

{
    
  [Authorize]
  [Route("api/[controller]")]
  [ApiController]
  public class AdminController : ControllerBase
  {
    public IAdmin _adminDetails;
    private readonly IAdmin _adminService;

    public AdminController(IAdmin adminDetails, IAdmin adminService)
    {
      _adminDetails = adminDetails;
      _adminService = adminService;
    }

        [HttpGet("GetAllUsersDetails")]
    public List<ResponseRegistrationCLS> GetAllUsersDetails()
    {
      try
      {
        return _adminDetails.GetAllUsersDetails();
      }
      catch (Exception ex)
      {
        return new List<ResponseRegistrationCLS>();
      }
    }

        [HttpGet("SearchUsers")]
        public async Task<IActionResult> SearchUsers(
            [FromQuery] string? MobileNumber,
            [FromQuery] int PageSize = 50,
            [FromQuery] int PageIndex = 1)
        {
            try
            {
                var result = await _adminService.SearchUsersAsync(MobileNumber, PageSize, PageIndex);
                return Ok(result);
            }
            catch (Exception ex)
            {
                // Log the exception (ex)
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpPost("DeleteUsersRegisteredRecord")]
    public string DeleteUsersRecord(int UserID)
    {
      Response<ResponseDeleteUserModel> response = new Response<ResponseDeleteUserModel>();
      ResponseDeleteUserModel usersDetails = new ResponseDeleteUserModel();
      try
      {
        var details = _adminDetails.getUsersDetialsByUserID(UserID);
        if (details == null)
        {
          return "Record not found";
        }
        var data = _adminDetails.saveUsersDataInHistoryTable(details);
        usersDetails = _adminDetails.DeleteUsersRecord(UserID);
        if (usersDetails == null)
        {
          response.Data = null;
          response.message = "Record deleted successfully";
          response.code = 200;
        }
        else
        {
         
        }
      }
      catch(Exception ex)
      {
        response.code = 500;
        response.Data = null;
        response.message = ex.Message;
      }
      return "Record delete successfully";
    }

    [HttpGet("CheckMembershipStatus")]
    public List<CheckMembershipStatusModel> CheckMembershipStatus(string Mobilenumber, string DeviceId, Int32 UserId)
    {
      try
      {
        return _adminDetails.CheckMembershipStatus(Mobilenumber, DeviceId, UserId);
      }
      catch (Exception ex)
      {
        return new List<CheckMembershipStatusModel>();
      }
    }

    [HttpPost("UpdateAppVersion")]
    public Response<UpdateAppVersionModel> UpdateAppVersion(UpdateAppVersionModel updateAppVersionModel)
    {
      Response<UpdateAppVersionModel> response = new Response<UpdateAppVersionModel>();
      UpdateAppVersionModel details = new UpdateAppVersionModel();
      try
      {
        details = _adminDetails.UpdateAppVersion(updateAppVersionModel);
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
      catch(Exception ex)  
      {
        response.code = 500;
        response.Data = null;
        response.message = ex.Message;
      }
      return response;
    }


    [HttpPost("UpdateUserInactiveToTrialPeriod")]
    public Response<ResponseUserModel> UpdateUserInactiveToTrial(int UserID, DateTime SubscriptionExpiryDate)
    {
      Response<ResponseUserModel> response = new Response<ResponseUserModel>();
      ResponseUserModel usersDetails = new ResponseUserModel();
      try
      {
        var data = _adminDetails.getUsersDetialsByUserID(UserID);
        if (data == null)
        {
          response.code = 200;
          response.Data = null;
          response.message = "PLease enter valid UserID";
        }
        if (SubscriptionExpiryDate <= DateTime.Now)
        {
          response.code = 200;
          response.Data = null;
          response.message = "Please enter future date";
        }
        usersDetails = _adminDetails.UpdateUserInactiveToTrial(UserID, SubscriptionExpiryDate);
        if (usersDetails == null)
        {
          response.code = 200;
          response.Data = null;
          response.message = "No data found";
        }
        else
        {
          response.code = 200;
          response.Data = usersDetails;
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
