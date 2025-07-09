using Aasaan_API.IServices;
using Aasaan_API.Models;
using DB_con;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;

namespace Aasaan_API.DbService
{
  public class AdminDetailsService : IAdmin
  {
    private readonly ConnectionCls _connectionCls;
    private readonly IConfiguration _configuration;
    public AdminDetailsService(IConfiguration configuration)
    {
      _configuration = configuration;
      _connectionCls = new ConnectionCls(_configuration);
    }

    public DataTable ConvertDatareadertoDataTable(IDataReader dr)
    {
      DataTable dt = new DataTable();
      dt.Load(dr);
      return dt;
    }

    public List<ResponseRegistrationCLS> GetAllUsersDetails(int PageIndex, int PageSize)
    {
      try
      {
        _connectionCls.clearParameter();
        _connectionCls.addParameter("PageSize", PageSize);
        _connectionCls.addParameter("PageIndex", PageIndex);

        DataTable dt = ConvertDatareadertoDataTable(_connectionCls.ExecuteReader("P2_sp_GetAllUsersDetails", CommandType.StoredProcedure));
        return ConvertToGetAllUsersDetails(dt);
      }
      catch (Exception ex)
      {
        throw;
      }
    }

    // Find your existing converter for GetAllUsersDetails
    public List<ResponseRegistrationCLS> ConvertToGetAllUsersDetails(DataTable dt)
    {
      List<ResponseRegistrationCLS> selectedUser = new List<ResponseRegistrationCLS>();
      if (dt == null || dt.Rows.Count == 0)
      {
        return selectedUser; // Return an empty list instead of null
      }

      foreach (DataRow row in dt.Rows)
      {
        var users = new ResponseRegistrationCLS();
        users.UserID = Convert.ToInt32(row["UserID"]);
        users.MobileNumber = row["MobileNumber"]?.ToString();
        users.EmailID = row["EmailID"]?.ToString();
        if (row["DateofCreation"] != DBNull.Value)
        {
          users.DateofCreation = Convert.ToDateTime(row["DateofCreation"]).ToString("dd/MM/yyyy");
        }
        else
        {
          users.DateofCreation = null;
        }
        users.DeviceID = row["DeviceID"]?.ToString();
        if (row["ExpiryDateApp"] != DBNull.Value)
        {
          users.SubscriptionExpiryDate = Convert.ToDateTime(row["ExpiryDateApp"]).ToString("dd/MM/yyyy");
        }
        else
        {
          users.SubscriptionExpiryDate = null;
        }
        users.Platform = row["Platform"]?.ToString();
        users.AppVersion = row["AppVersion"]?.ToString();
        if (row["LastAPICallDate"] != DBNull.Value)
        {
          users.LastAPICallDate = Convert.ToDateTime(row["LastAPICallDate"]).ToString("dd/MM/yyyy");
        }
        else
        {
          users.LastAPICallDate = null;
        }
        users.AdminNotes = row["AdminNotes"]?.ToString();
        users.AppCode = row["AppCode"]?.ToString();
        users.SubscriptionStatus = row["SubscriptionStatus"]?.ToString();
        users.TotalRecords = Convert.ToInt32(row["TotalRecords"]);
        selectedUser.Add(users);
      }

      return selectedUser;
    }

    public ResponseDeleteUserModel getUsersDetialsByUserID(int UserID, string Appcode)
    {
      try
      {
        _connectionCls.clearParameter();
        _connectionCls.addParameter("@UserID", UserID);
        _connectionCls.addParameter("@AppCode", Appcode);
        DataTable dt = ConvertDatareadertoDataTable(_connectionCls.ExecuteReader("P2_sp_GetUsersRegistrationByUserID", CommandType.StoredProcedure));
        return ConvertToUsersDetialsByUserID(dt);
      }
      catch (Exception ex)
      {
        throw;
      }
    }
    public ResponseDeleteUserModel ConvertToUsersDetialsByUserID(DataTable dt)
    {
      if (dt == null || dt.Rows.Count == 0)
      {
        return null;
      }

      DataRow row = dt.Rows[0];
      ResponseDeleteUserModel users = new ResponseDeleteUserModel();

      users.UserID = Convert.ToInt32(row["UserID"]?.ToString() ?? string.Empty);
      users.MobileNumber = row["MobileNumber"]?.ToString() ?? string.Empty;
      users.EmailID = row["EmailID"]?.ToString() ?? string.Empty;
      users.DateofCreation = Convert.ToDateTime(row["DateofCreation"]);
      users.DeviceID = row["DeviceID"]?.ToString() ?? string.Empty;
      users.SubscriptionExpiryDate = Convert.ToDateTime(row["ExpiryDateApp"]);
      users.Platform = row["Platform"]?.ToString() ?? string.Empty;
      users.AppVersion = row["AppVersion"]?.ToString() ?? string.Empty;
      users.LastAPICallDate = Convert.ToDateTime(row["LastAPICallDate"]);
      users.AdminNotes = row["AdminNotes"]?.ToString() ?? string.Empty;
      users.AppCode = row["AppCode"]?.ToString() ?? string.Empty;

      return users;
    }


    public List<ResponseRegistrationCLS> SearchUsersAsync(string? MobileNumber ,int PageIndex, int PageSize)
    {
      try
      {
        _connectionCls.clearParameter();
        _connectionCls.addParameter("@MobileNumber", MobileNumber);
        _connectionCls.addParameter("@PageIndex", PageIndex);
        _connectionCls.addParameter("@PageSize", PageSize);


        DataTable dt = ConvertDatareadertoDataTable(_connectionCls.ExecuteReader("P2_sp_SearchUsersByMobile", CommandType.StoredProcedure));
        return ConvertToSearchUserResult(dt);


        //return Task.FromResult(result);
      }
      catch (Exception ex)
      {
        throw new Exception("Error executing P2_sp_SearchUsersByMobile.", ex);
      }
    }
    private List<ResponseRegistrationCLS> ConvertToSearchUserResult(DataTable dt)
    {
      List<ResponseRegistrationCLS> selectedUser = new List<ResponseRegistrationCLS>();
      
      if (dt == null || dt.Rows.Count == 0)
      {
        return selectedUser;
      }
      foreach (DataRow row in dt.Rows)
      {
        var user = new ResponseRegistrationCLS
        {
          UserID = Convert.ToInt32(row["UserID"]),
          EmailID = row["EmailID"]?.ToString(),
          MobileNumber = row["MobileNumber"]?.ToString(),
          DeviceID = row["DeviceID"]?.ToString(),
          DateofCreation = Convert.ToDateTime(row["DateofCreation"]).ToString("dd/MM/yyyy"),
          SubscriptionStatus = row["SubscriptionStatus"].ToString(),
          ExpiryDateApp = Convert.ToDateTime(row["ExpiryDateApp"]).ToString("dd/MM/yyyy"),
          Platform = row["Platform"]?.ToString(),
          AppVersion = row["AppVersion"]?.ToString(),
          AdminNotes = row["AdminNotes"]?.ToString(),
          AppCode = row["AppCode"]?.ToString(),
          LastAPICallDate = Convert.ToDateTime(row["LastAPICallDate"]).ToString("dd/MM/yyyy"),
          TotalRecords = Convert.ToInt32(row["TotalRecords"]),
        };
        selectedUser.Add(user);
      }
      return selectedUser;
    }

    public Task<ResponseRegistrationCLS> UpdateUserAsync(UpdateUsersDetilsAdmin userToUpdate)
    {
      try
      {
        _connectionCls.clearParameter();

        _connectionCls.addParameter("@UserID", userToUpdate.UserID);
        _connectionCls.addParameter("@MobileNumber", userToUpdate.MobileNumber);
        _connectionCls.addParameter("@AppCode", userToUpdate.AppCode);
        _connectionCls.addParameter("@SubscriptionExpiryDate", userToUpdate.SubscriptionExpiryDate);


        DataTable dt = ConvertDatareadertoDataTable(_connectionCls.ExecuteReader("P2_sp_UpdateUserDetails", CommandType.StoredProcedure));

        ResponseRegistrationCLS updatedUser = ConvertToAdminUser(dt);

        return Task.FromResult(updatedUser);
      }
      catch (Exception ex)
      {
        if (ex is System.Data.SqlTypes.SqlTypeException)
        {
          throw new Exception("A date value was out of the acceptable SQL Server range.", ex);
        }
        throw new Exception("Error executing P2_sp_UpdateUserDetails.", ex);
      }
    }
    private ResponseRegistrationCLS ConvertToAdminUser(DataTable dt)
    {
      if (dt == null || dt.Rows.Count == 0)
      {
        return null;
      }

      DataRow row = dt.Rows[0];

      var user = new ResponseRegistrationCLS
      {
        UserID = Convert.ToInt32(row["UserID"]),
        MobileNumber = row["MobileNumber"]?.ToString(),
        EmailID = row["EmailID"]?.ToString(),
        DateofCreation = Convert.ToDateTime(row["DateofCreation"]).ToString("dd/MM/yyyy"),
        DeviceID = row["DeviceID"]?.ToString(),
        ExpiryDateApp = Convert.ToDateTime(row["ExpiryDateApp"]).ToString("dd/MM/yyyy"),
        Platform = row["Platform"]?.ToString(),
        AppVersion = row["AppVersion"]?.ToString(),
        LastAPICallDate = Convert.ToDateTime(row["LastAPICallDate"]).ToString("dd/MM/yyyy"),
        AdminNotes = row["AdminNotes"]?.ToString(),
        AppCode = row["AppCode"]?.ToString(),
        SubscriptionStatus = row["SubscriptionStatus"].ToString(),
      };

      return user;
    }

    public ResponseDeleteUserModel DeleteUsersRecord(int UserID, string Appcode)
    {
      try
      {
        _connectionCls.clearParameter();
        _connectionCls.addParameter("@UserID", UserID);
        _connectionCls.addParameter("@AppCode", Appcode);
        DataTable dt = ConvertDatareadertoDataTable(_connectionCls.ExecuteReader("P2_sp_DeleteUsersRegistration", CommandType.StoredProcedure));
        return ConvertToLoginList(dt);
      }
      catch (Exception ex)
      {
        throw;
      }
    }
    public ResponseDeleteUserModel ConvertToLoginList(DataTable dt)
    {
      if (dt == null || dt.Rows.Count == 0)
      {
        return null;
      }

      DataRow row = dt.Rows[0];
      ResponseDeleteUserModel users = new ResponseDeleteUserModel();

      users.UserID = Convert.ToInt32(row["UserID"]?.ToString() ?? string.Empty);
      users.EmailID = row["email"]?.ToString() ?? string.Empty;
      return users;
    }

    public ResponseDeleteUserModel saveUsersDataInHistoryTable(ResponseDeleteUserModel responseDeleteUser)
    {
      try
      {
        _connectionCls.clearParameter();
        usershistorydata(responseDeleteUser, DBTrans.Insert);
        _connectionCls.BeginTransaction();
        object result = _connectionCls.ExecuteScalar("P2_sp_InsertUsersHistory", CommandType.StoredProcedure);
        _connectionCls.CommitTransaction();
        return responseDeleteUser = responseDeleteUser;
      }
      catch (Exception ex)
      {
        throw new Exception("P2_sp_InsertUsersHistory : " + ex.Message);
      }
    }
    private void usershistorydata(ResponseDeleteUserModel obj, DB_con.DBTrans trans)
    {
      try
      {
        _connectionCls.clearParameter();
        _connectionCls.addParameter("@UserId", obj.UserID);
        _connectionCls.addParameter("@MobileNumber", obj.MobileNumber);
        _connectionCls.addParameter("@EmailID", obj.EmailID);
        _connectionCls.addParameter("@DateofCreation", obj.DateofCreation);
        _connectionCls.addParameter("@DeviceID", obj.DeviceID);
        _connectionCls.addParameter("@SubscriptionExpiryDate", obj.SubscriptionExpiryDate);
        _connectionCls.addParameter("@Platform", obj.Platform);
        _connectionCls.addParameter("@AppVersion", obj.AppVersion);
        _connectionCls.addParameter("@LastAPICallDate", obj.LastAPICallDate);
        _connectionCls.addParameter("@AdminNotes", obj.AdminNotes);
        _connectionCls.addParameter("@AppCode", obj.AppCode);
      }
      catch (Exception ex)
      {
        throw;
      }
    }


    public ResponseRegistrationCLS ApplicationGroupeUpdateAsync(ApplicationGroupeUpdateModel userToUpdate)
    {
      try
      {
        _connectionCls.clearParameter();

        _connectionCls.addParameter("@UserID", userToUpdate.UserID);
        _connectionCls.addParameter("@MobileNumber", userToUpdate.MobileNumber);
        _connectionCls.addParameter("@AppCode", userToUpdate.AppCode);
        _connectionCls.addParameter("@SubscriptionExpiryDate", userToUpdate.SubscriptionExpiryDate);


        DataTable dt = ConvertDatareadertoDataTable(_connectionCls.ExecuteReader("P2_sp_UpdateGroupeApplicationDetails", CommandType.StoredProcedure));

        ResponseRegistrationCLS updatedUser = ConvertToApplicationGroup(dt);

        return updatedUser;
      }
      catch (Exception ex)
      {
        if (ex is System.Data.SqlTypes.SqlTypeException)
        {
          throw new Exception("A date value was out of the acceptable SQL Server range.", ex);
        }
        throw new Exception("Error executing P2_sp_UpdateGroupeApplicationDetails.", ex);
      }
    }
    private ResponseRegistrationCLS ConvertToApplicationGroup(DataTable dt)
    {
      if (dt == null || dt.Rows.Count == 0)
      {
        return null;
      }

      DataRow row = dt.Rows[0];

      var user = new ResponseRegistrationCLS
      {
        UserID = Convert.ToInt32(row["UserID"]),
        MobileNumber = row["MobileNumber"]?.ToString(),
        EmailID = row["EmailID"]?.ToString(),
        DateofCreation = Convert.ToDateTime(row["DateofCreation"]).ToString("dd/MM/yyyy"),
        DeviceID = row["DeviceID"]?.ToString(),
        SubscriptionExpiryDate = Convert.ToDateTime(row["ExpiryDateApp"]).ToString("dd/MM/yyyy"),
        Platform = row["Platform"]?.ToString(),
        AppVersion = row["AppVersion"]?.ToString(),
        LastAPICallDate = Convert.ToDateTime(row["LastAPICallDate"]).ToString("dd/MM/yyyy"),
        AdminNotes = row["AdminNotes"]?.ToString(),
        AppCode = row["AppCode"]?.ToString(),
        SubscriptionStatus = row["SubscriptionStatus"].ToString(),
      };

      return user;
    }


    public ResponseChangePassword ChangeAdminPasswords(ChangeAdminPassword changeAdminPassword)
    {
      try
      {
        _connectionCls.clearParameter();

        _connectionCls.addParameter("@UserID", changeAdminPassword.UserID);
        _connectionCls.addParameter("@OldPassword", changeAdminPassword.OldPassword);
        _connectionCls.addParameter("@NewPassword", changeAdminPassword.NewPassword);


        DataTable dt = ConvertDatareadertoDataTable(_connectionCls.ExecuteReader("P2_sp_ChangeAdminPassword", CommandType.StoredProcedure));

        ResponseChangePassword updatedUser = ConvertTochangepassword(dt);

        return updatedUser;
      }
      catch (Exception ex)
      {
        if (ex is System.Data.SqlTypes.SqlTypeException)
        {
          throw new Exception("A date value was out of the acceptable SQL Server range.", ex);
        }
        throw new Exception("Error executing P2_sp_ChangeAdminPassword.", ex);
      }
    }
    private ResponseChangePassword ConvertTochangepassword(DataTable dt)
    {
      if (dt == null || dt.Rows.Count == 0)
      {
        return null;
      }

      DataRow row = dt.Rows[0];

      var user = new ResponseChangePassword
      {
        Message = row["Message"]?.ToString(),
      };

      return user;
    }     
  }
}
