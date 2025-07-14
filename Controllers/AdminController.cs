using Aasaan_API.IServices;
using Aasaan_API.Models;
using Aasaan_API.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Specialized;
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
    public List<ResponseRegistrationCLS> GetAllUsersDetails(int PageIndex, int PageSize)
    {
      try
      {
        return _adminDetails.GetAllUsersDetails(PageIndex, PageSize);
      }
      catch (Exception ex)
      {
        return new List<ResponseRegistrationCLS>();
      }
    }

    [HttpGet("SearchUsers")]
    public List<ResponseRegistrationCLS> SearchUsers(string MobileNumber, int PageIndex, int PageSize)
    {
      try
      {
        return _adminService.SearchUsersAsync(MobileNumber, PageIndex, PageSize);

      }
      catch (Exception ex)
      {
        return new List<ResponseRegistrationCLS>();
      }
    }

    [HttpDelete("DeleteUsersRegisteredRecord")]
    public string DeleteUsersRecord(int UserID, string Appcode)
    {
      Response<ResponseDeleteUserModel> response = new Response<ResponseDeleteUserModel>();
      ResponseDeleteUserModel usersDetails = new ResponseDeleteUserModel();
      try
      {
        var details = _adminDetails.getUsersDetialsByUserID(UserID, Appcode);
        if (details == null)
        {
          return "Record not found";
        }
        var data = _adminDetails.saveUsersDataInHistoryTable(details);
        usersDetails = _adminDetails.DeleteUsersRecord(UserID, Appcode);
        if (usersDetails == null)
        {
          response.Data = null;
          response.message = "Record deleted successfully";
          response.code = 200;
          return "Record delete successfully";
        }
        else
        {

        }
      }
      catch (Exception ex)
      {
        response.code = 500;
        response.Data = null;
        response.message = ex.Message;
        return "";
      }
      return "";
    }


    [HttpPost("UpdateUser")]
    public Response<ResponseRegistrationCLS> UpdateUser([FromBody] UpdateUsersDetilsAdmin userToUpdate)
    {
      Response<ResponseRegistrationCLS> response = new Response<ResponseRegistrationCLS>();
      ResponseRegistrationCLS updatedUser = new ResponseRegistrationCLS();

      if (userToUpdate.UserID == 0 || userToUpdate == null)
      {
        response.code = 500;
        response.Data = null;
        response.message = "ID mismatch";
        return response;
      }

      try
      {
        updatedUser = _adminService.UpdateUserAsync(userToUpdate);

        if (updatedUser != null)
        {
          response.code = 200;
          response.Data = updatedUser;
          response.message = "Update user Details successfully";
          return response;
        }
        else
        {
          response.code = 400;
          response.Data = null;
          response.message = "Please try again";
          return response;
        }

      }
      catch (Exception ex)
      {
        response.code = 400;
        response.Data = null;
        response.message = "Please try again";
        return response;
      }
    }


    [HttpPost("ApplicationGroupeUpdate")]
    public Response<ResponseRegistrationCLS> ApplicationGroupeUpdate([FromBody] ApplicationGroupeUpdateModel userToUpdate)
    {
      Response<ResponseRegistrationCLS> response = new Response<ResponseRegistrationCLS>();
      ResponseRegistrationCLS usersDetails = new ResponseRegistrationCLS();

      try
      {
        var updatedUser = _adminService.ApplicationGroupeUpdateAsync(userToUpdate);
        if (updatedUser != null)
        {
          response.code = 200;
          response.Data = updatedUser;
          response.message = "Data updated successfully";
        }
        else
        {
          response.Data = null;
          response.message = "Data not updated, please try again";
          response.code = 0;
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

    [HttpPost("ChangeAdminPassword")]
    public Response<ResponseChangePassword> ChangeAdminPassword([FromBody] ChangeAdminPassword userToUpdate)
    {
      Response<ResponseChangePassword> response = new Response<ResponseChangePassword>();
      ResponseChangePassword usersDetails = new ResponseChangePassword();

      try
      {
        var updatedUser = _adminService.ChangeAdminPasswords(userToUpdate);
        if (updatedUser != null)
        {
          response.code = 200;
          response.Data = null;
          response.message = updatedUser.Message;
        }
        else
        {
          response.Data = null;
          response.message = updatedUser.Message;
          response.code = 400;
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

    [HttpGet("GetAllGroupsApplicationsDetails")]
    public List<ResponseUsersGroupDetails> GetAllGroupsApplicationsDetail(int PageIndex, int PageSize)
    {
      try
      {
        return _adminDetails.GetAllGroupsApplicationsDetails(PageIndex, PageSize);
      }
      catch (Exception ex)
      {
        return new List<ResponseUsersGroupDetails>();
      }
    }


    [HttpPost("UpdateGroupeNameInGroupOfApplication")]
    public Response<ResponseUpdatedGroupName> UpdateGroupeNameInGroupOfApplication([FromBody] RequestUpdateGroupeNameInGroupOfApplication userToUpdate)
    {
      Response<ResponseUpdatedGroupName> response = new Response<ResponseUpdatedGroupName>();
      ResponseUpdatedGroupName updatedUser = new ResponseUpdatedGroupName();

      if (userToUpdate.UserID == 0 || userToUpdate == null)
      {
        response.code = 500;
        response.Data = null;
        response.message = "ID mismatch";
        return response;
      }

      if (userToUpdate.AppCodeOne == userToUpdate.AppCodeTwo)
      {
        response.code = 500;
        response.Data = null;
        response.message = "You can not use same appcode, please chose different appcode";
        return response;
      }
      try
      {
        updatedUser = _adminService.UpdateGroupeNameInGroupOfApplications(userToUpdate);

        if (updatedUser != null)
        {
          response.code = 200;
          response.Data = updatedUser;
          response.message = updatedUser.Message;
          return response;
        }
        else
        {
          response.code = 400;
          response.Data = null;
          response.message = "Please try again";
          return response;
        }

      }
      catch (Exception ex)
      {
        response.code = 400;
        response.Data = null;
        response.message = "Please try again";
        return response;
      }
    }

  }
}
