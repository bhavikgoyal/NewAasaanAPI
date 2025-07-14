using Aasaan_API.IServices;
using Aasaan_API.Models;
using DB_con;
using System.Data;
using System.Data.SqlClient;

namespace Aasaan_API.DbService
{
  public class AccountService : IAccount
  {
    private readonly ConnectionCls _connectionCls;
    private readonly IConfiguration _configuration;

    public AccountService(IConfiguration configuration)
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
    public ResponseRegistrationCLS SaveRegistrationData(RequestRegistrationCLS registrationCL)
    {
      try
      {
        //_connectionCls.BeginTransaction();
        _connectionCls.clearParameter();
        RegistrationData(registrationCL, DBTrans.Insert);
        DataTable dt = ConvertDatareadertoDataTable(_connectionCls.ExecuteReader("sp_Insert_UsersRegistration", CommandType.StoredProcedure));
        //_connectionCls.CommitTransaction();
        return ConvertToRegistrationData(dt);
      }
      catch (Exception ex)
      {
        throw new Exception("sp_Insert_UsersRegistration : " + ex.Message);
      }
      finally 
      {
        _connectionCls.clearParameter();
      }
    }

    
    private void RegistrationData(RequestRegistrationCLS obj, DB_con.DBTrans trans)
    {
      try
      {
        _connectionCls.addParameter("@MobileNumber", obj.MobileNumber);
        _connectionCls.addParameter("@EmailID", obj.EmailID);  
        _connectionCls.addParameter("@DeviceID", obj.DeviceID);
        _connectionCls.addParameter("@SubscriptionExpiryDate", obj.SubscriptionExpiryDate);
        _connectionCls.addParameter("@Platform", obj.Platform);
        _connectionCls.addParameter("@AppVersion", obj.AppVersion);
        _connectionCls.addParameter("@AdminNotes", obj.AdminNotes);
        _connectionCls.addParameter("@AppCode", obj.AppCode);
      }
      catch (Exception ex)
      {
        throw;
      }
    }

    public ResponseRegistrationCLS ConvertToRegistrationData(DataTable dt)
    {
      try
      {
        if (dt == null || dt.Rows.Count == 0)
        {
          return null;
        }

        DataRow row = dt.Rows[0];
        ResponseRegistrationCLS users = new ResponseRegistrationCLS();

        users.UserID = Convert.ToInt32(row["UserID"]?.ToString() ?? string.Empty);
        users.MobileNumber = row["MobileNumber"]?.ToString() ?? string.Empty;
        users.EmailID = row["EmailID"]?.ToString() ?? string.Empty;
        if (row["DateofCreation"] != DBNull.Value)
        {
          users.DateofCreation = Convert.ToDateTime(row["DateofCreation"]).ToString("dd/MM/yyyy");
        }
        else
        {
          users.DateofCreation = null; 
        }
        users.DeviceID = row["DeviceID"]?.ToString() ?? string.Empty;
        if (row["SubscriptionExpiryDate"] != DBNull.Value)
        {
          users.SubscriptionExpiryDate = Convert.ToDateTime(row["SubscriptionExpiryDate"]).ToString("dd/MM/yyyy");
        }
        else
        {
          users.SubscriptionExpiryDate = null; 
        }
        if (row["ExpiryDateApp"] != DBNull.Value)
        {
          users.ExpiryDateApp = Convert.ToDateTime(row["ExpiryDateApp"]).ToString("dd/MM/yyyy");
        }
        else
        {
          users.ExpiryDateApp = null;
        }
        users.Platform = row["Platform"]?.ToString() ?? string.Empty;
        users.AppVersion = row["AppVersion"]?.ToString() ?? string.Empty;
        if (row["LastAPICallDate"] != DBNull.Value)
        {
          users.LastAPICallDate = Convert.ToDateTime(row["LastAPICallDate"]).ToString("dd/MM/yyyy");
        }
        else
        {
          users.LastAPICallDate = null; 
        }
        users.AdminNotes = row["AdminNotes"]?.ToString() ?? string.Empty;
        users.AppCode = row["AppCode"]?.ToString() ?? string.Empty;
        users.SubscriptionStatus = row["SubscriptionStatus"]?.ToString() ?? string.Empty;
        return users;
      }
      catch (Exception ex)
      {
        throw ex;
      }      
    }

    public LoginModel Login(string username, string password)
    {
      try
      {
        _connectionCls.clearParameter();
        _connectionCls.addParameter("@Email", username);
        _connectionCls.addParameter("@Password", password);
        DataTable dt = ConvertDatareadertoDataTable(_connectionCls.ExecuteReader("sp_AdminLogin", CommandType.StoredProcedure));
        return ConvertToLoginList(dt);
      }
      catch (Exception ex)
      {
        throw;
      }                      
    }

    public LoginModel ConvertToLoginList(DataTable dt)
    {
      if (dt == null || dt.Rows.Count == 0)
      {
        return null;
      }

      DataRow row = dt.Rows[0];
      LoginModel users = new LoginModel();

      users.UserID = Convert.ToInt32(row["UserID"]?.ToString() ?? string.Empty);
      users.Email = row["Email"]?.ToString() ?? string.Empty;
      users.Password = row["Password"]?.ToString() ?? string.Empty;
      return users;
    }

    public List<ResponseUserModel> LogInWithMobileAndDeviceId(string Mobile, string DeviceId)
    {
      try
      {
        _connectionCls.clearParameter();
        _connectionCls.addParameter("@MobileNumber", Mobile);
        _connectionCls.addParameter("@DeviceId", DeviceId);
        DataTable dt = ConvertDatareadertoDataTable(_connectionCls.ExecuteReader("sp_CheckUserMobilenoAndDeviceIdexist", CommandType.StoredProcedure));
        return ConvertToUserLoginList(dt);
      }
      catch (Exception ex)
      {
        throw;
      }
      finally { _connectionCls.clearParameter(); }
    }

    public List<ResponseUserModel> ConvertToUserLoginList(DataTable dt)
    {
      if (dt == null || dt.Rows.Count == 0)
      {
        return null;
      }

      List<ResponseUserModel> selectedUser = new List<ResponseUserModel>();
      foreach (DataRow row in dt.Rows)
      {
        var user = new ResponseUserModel
        {
          UserID = Convert.ToInt32(row["UserID"]?.ToString() ?? string.Empty),
          MobileNumber = row["MobileNumber"]?.ToString() ?? string.Empty,
          EmailID = row["EmailID"]?.ToString() ?? string.Empty,
          DateofCreation = Convert.ToDateTime(row["DateofCreation"]).ToString("dd/MM/yyyy"),
          DeviceID = row["DeviceID"]?.ToString() ?? string.Empty,
          SubscriptionExpiryDate = Convert.ToDateTime(row["SubscriptionExpiryDate"]).ToString("dd/MM/yyyy"),
          ExpiryDateApp = Convert.ToDateTime(row["ExpiryDateApp"]).ToString("dd/MM/yyyy"),
          Platform = row["Platform"]?.ToString() ?? string.Empty,
          AppVersion = row["AppVersion"]?.ToString() ?? string.Empty,
          LastAPICallDate = Convert.ToDateTime(row["LastAPICallDate"]).ToString("dd/MM/yyyy"),
          AdminNotes = row["AdminNotes"]?.ToString() ?? string.Empty,
          AppCode = row["AppCode"]?.ToString() ?? string.Empty,
          SubscriptionStatus = row["SubscriptionStatus"]?.ToString() ?? string.Empty,
        };
        selectedUser.Add(user);
      }
      return selectedUser;
    }
    public DateTime? GetNullableDate(object obj)
    {
      if (obj == null || obj == DBNull.Value)
      {
        return null;
      }

      if (DateTime.TryParse(obj.ToString(), out DateTime date))
      {
        return date;
      }

      return null;
    }

    public ResponseUpdateUserModel UpdateAppVersion(UpdateAppVersionModel updateAppVersionModel)
    {
      try
      {
        _connectionCls.clearParameter();
        _connectionCls.addParameter("@MobileNumber", updateAppVersionModel.MobileNumber);
        _connectionCls.addParameter("@DeviceId", updateAppVersionModel.DeviceId);
        _connectionCls.addParameter("@UserID", updateAppVersionModel.UserID);
        _connectionCls.addParameter("@AppVersion", updateAppVersionModel.AppVersion);
        _connectionCls.addParameter("@Platform", updateAppVersionModel.Platforms);
        _connectionCls.BeginTransaction();
        DataTable dt = ConvertDatareadertoDataTable(_connectionCls.ExecuteReader("sp_UpdateAppVersion", CommandType.StoredProcedure));
        return ConvertToUpdateUsers(dt);
      }
      catch (Exception ex)
      {
        throw new Exception("sp_UpdateAppVersion : " + ex.Message);
      }
    }
    public ResponseUpdateUserModel ConvertToUpdateUsers(DataTable dt)
    {
      if (dt == null || dt.Rows.Count == 0)
      {
        return null;
      }

      DataRow row = dt.Rows[0];
      ResponseUpdateUserModel users = new ResponseUpdateUserModel();

      users.UserID = Convert.ToInt32(row["UserID"]?.ToString() ?? string.Empty);
      users.MobileNumber = row["MobileNumber"]?.ToString() ?? string.Empty;
      users.EmailID = row["EmailID"]?.ToString() ?? string.Empty;
      if (row["DateofCreation"] != DBNull.Value)
      {
        users.DateofCreation = Convert.ToDateTime(row["DateofCreation"]).ToString("dd/MM/yyyy");
      }
      else
      {
        users.DateofCreation = null;
      }
      users.DeviceID = row["DeviceID"]?.ToString() ?? string.Empty;
      if (row["SubscriptionExpiryDate"] != DBNull.Value)
      {
        users.SubscriptionExpiryDate = Convert.ToDateTime(row["SubscriptionExpiryDate"]).ToString("dd/MM/yyyy");
      }
      else
      {
        users.SubscriptionExpiryDate = null;
      }
      users.Platform = row["Platform"]?.ToString() ?? string.Empty;
      users.AppVersion = row["AppVersion"]?.ToString() ?? string.Empty;
      if (row["LastAPICallDate"] != DBNull.Value)
      {
        users.LastAPICallDate = Convert.ToDateTime(row["LastAPICallDate"]).ToString("dd/MM/yyyy");
      }
      else
      {
        users.LastAPICallDate = null;
      }
      users.AdminNotes = row["AdminNotes"]?.ToString() ?? string.Empty;
      users.AppCode = row["AppCode"]?.ToString() ?? string.Empty;
      users.SubscriptionStatus = row["SubscriptionStatus"]?.ToString() ?? string.Empty;

      return users;
    }
  }
}
