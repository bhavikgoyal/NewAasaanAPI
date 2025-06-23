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

        public ResponseDeleteUserModel getUsersDetialsByUserID(int UserID)
        {
            try
            {
                _connectionCls.clearParameter();
                _connectionCls.addParameter("@UserID", UserID);
                DataTable dt = ConvertDatareadertoDataTable(_connectionCls.ExecuteReader("P2_sp_GetUsersRegistrationByUserID", CommandType.StoredProcedure));
                return ConvertToUsersDetialsByUserID(dt);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
       


        public Task<PaginatedResult<AdminUserCLS>> SearchUsersAsync(string? MobileNumber, int PageSize, int PageIndex)
        {
            try
            {
                _connectionCls.clearParameter();
                _connectionCls.addParameter("@MobileNumber", string.IsNullOrWhiteSpace(MobileNumber) ? (object)DBNull.Value : MobileNumber);
                _connectionCls.addParameter("@PageSize", PageSize);
                _connectionCls.addParameter("@PageIndex", PageIndex);


                DataTable dt = ConvertDatareadertoDataTable(_connectionCls.ExecuteReader("P2_sp_SearchUsersByMobile", CommandType.StoredProcedure));


                PaginatedResult<AdminUserCLS> result = ConvertToSearchUserResult(dt, PageSize, PageIndex);


                return Task.FromResult(result);
            }
            catch (Exception ex)
            {
                throw new Exception("Error executing P2_sp_SearchUsersByMobile.", ex);
            }
        }


        private PaginatedResult<AdminUserCLS> ConvertToSearchUserResult(DataTable dt, int pageIndex, int pageSize)
        {
            var userList = new List<AdminUserCLS>();
            int totalCount = 0;


            if (dt == null || dt.Rows.Count == 0)
            {
                return new PaginatedResult<AdminUserCLS>
                {
                    Items = userList,
                    TotalCount = 0,
                    PageIndex = pageIndex,
                    TotalPages = 0
                };
            }


            totalCount = Convert.ToInt32(dt.Rows[0]["TotalCount"]);


            foreach (DataRow row in dt.Rows)
            {
                var user = new AdminUserCLS
                {
                    UserID = Convert.ToInt32(row["UserID"]),
                    EmailID = row["EmailID"]?.ToString(),
                    MobileNumber = row["MobileNumber"]?.ToString(),
                    DeviceID = row["DeviceID"]?.ToString(),
                    DateOfCreation = Convert.ToDateTime(row["DateofCreation"]),
                    SubscriptionStatus = Convert.ToBoolean(row["SubscriptionStatus"]),
                    SubscriptionExpiryDate = row["SubscriptionExpiryDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(row["SubscriptionExpiryDate"]),
                   
                };
                userList.Add(user);
            }


            var result = new PaginatedResult<AdminUserCLS>
            {
                Items = userList,
                TotalCount = totalCount,
                PageIndex = pageIndex,
                TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
            };

            return result;
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
            users.DateofCreation = Convert.ToDateTime(row["DateofCreation"]?.ToString() ?? string.Empty);
            users.DeviceID = row["DeviceID"]?.ToString() ?? string.Empty;
            users.SubscriptionExpiryDate = Convert.ToDateTime(row["SubscriptionExpiryDate"]?.ToString() ?? string.Empty);
            users.Platform = row["Platform"]?.ToString() ?? string.Empty;
            users.AppVersion = row["AppVersion"]?.ToString() ?? string.Empty;
            users.LastAPICallDate = Convert.ToDateTime(row["LastAPICallDate"]?.ToString() ?? string.Empty);
            users.AdminNotes = row["AdminNotes"]?.ToString() ?? string.Empty;
            users.AppCode = row["AppCode"]?.ToString() ?? string.Empty;

            return users;
        }


        public ResponseDeleteUserModel DeleteUsersRecord(int UserID)
        {
            try
            {
                _connectionCls.clearParameter();
                _connectionCls.addParameter("@UserID", UserID);
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

        public List<CheckMembershipStatusModel> CheckMembershipStatus(string Mobilenumber, string DeviceId, Int32 UserId)
        {
            try
            {
                _connectionCls.clearParameter();
                _connectionCls.addParameter("@MobileNumber", Mobilenumber);
                _connectionCls.addParameter("@DeviceId", DeviceId);
                _connectionCls.addParameter("@UserID", UserId);
                DataTable dt = ConvertDatareadertoDataTable(_connectionCls.ExecuteReader("P2_CheckMembershipStatus", CommandType.StoredProcedure));
                return ConvertToCheckMembershipStatus(dt);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public List<CheckMembershipStatusModel> ConvertToCheckMembershipStatus(DataTable dt)
        {
            List<CheckMembershipStatusModel> SelectedUser = new List<CheckMembershipStatusModel>();
            if (dt == null || dt.Rows.Count == 0)
            {
                return null;
            }

            foreach (DataRow row in dt.Rows)
            {
                CheckMembershipStatusModel users = new CheckMembershipStatusModel();
                DateTime? lastapiDate = GetNullableDate(row["LastAPICallDate"]);
                string isfacebookuser = row["isfacebookuser"]?.ToString() ?? string.Empty;
                string isgoogoleuser = row["isgoogleuser"]?.ToString() ?? string.Empty;
                DateTime lastapidata = Convert.ToDateTime(lastapiDate);
                string lastdate = lastapidata.ToString();
                if (lastdate == "01-01-0001 00:00:00")
                {
                    lastdate = null;
                }
                if (isfacebookuser == null || isfacebookuser == "")
                {
                    isfacebookuser = null;
                }
                if (isgoogoleuser == null)
                {
                    isgoogoleuser = null;
                }
                users.id = Convert.ToInt32(row["UserID"]?.ToString() ?? string.Empty);
                users.email = row["email"]?.ToString() ?? string.Empty;
                users.password = row["Password"]?.ToString() ?? string.Empty;
                users.createdata = Convert.ToDateTime(row["createdata"]?.ToString() ?? string.Empty);
                users.facebook_firstname = row["facebook_firstname"]?.ToString() ?? string.Empty;
                users.facebook_lastname = row["facebook_lastname"]?.ToString() ?? string.Empty;
                users.isfacebookuser = Convert.ToBoolean(isfacebookuser);
                users.isgoogleuser = Convert.ToBoolean(isgoogoleuser);
                users.FacebookID = row["FacebookID"]?.ToString() ?? string.Empty;
                users.GoogleID = row["GoogleID"]?.ToString() ?? string.Empty;
                users.google_firstname = row["google_firstname"]?.ToString() ?? string.Empty;
                users.google_lastname = row["google_lastname"]?.ToString() ?? string.Empty;
                users.DeviceId = row["DeviceId"]?.ToString() ?? string.Empty;
                users.IsActive = Convert.ToBoolean(row["IsActive"]?.ToString() ?? string.Empty);
                users.MobileNumber = row["MobileNumber"]?.ToString() ?? string.Empty;
                users.states = row["states"]?.ToString() ?? string.Empty;
                users.Platforms = row["Platforms"]?.ToString() ?? string.Empty;
                users.AppVersion = row["AppVersion"]?.ToString() ?? string.Empty;
                users.LastAPICallDate = lastdate;
                users.UserRegistrationId = Convert.ToInt32(row["UserRegistrationId"]?.ToString() ?? string.Empty);
                users.Userid = Convert.ToInt32(row["Userid"]?.ToString() ?? string.Empty);
                users.RegistaredDate = Convert.ToDateTime(row["RegistaredDate"]?.ToString() ?? string.Empty);
                users.ExtendDays = Convert.ToInt32(row["ExtendDays"]?.ToString() ?? string.Empty);
                users.ExpiryDate = Convert.ToDateTime(row["ExpiryDate"]?.ToString() ?? string.Empty);
                users.PaidAmount = Convert.ToInt32(row["PaidAmount"]?.ToString() ?? string.Empty);
                users.Notes = row["Notes"]?.ToString() ?? string.Empty;
                users.AppCode = row["AppCode"]?.ToString() ?? string.Empty;
                users.UserStatus = row["UserStatus"]?.ToString() ?? string.Empty;
                SelectedUser.Add(users);
            }

            return SelectedUser;
        }

        public List<ResponseRegistrationCLS> GetAllUsersDetails()
        {
            try
            {
                _connectionCls.clearParameter();
                DataTable dt = ConvertDatareadertoDataTable(_connectionCls.ExecuteReader("P2_sp_GetAllUsersDetails", CommandType.StoredProcedure));
                return ConvertToGetAllUsersDetails(dt);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public List<ResponseRegistrationCLS> ConvertToGetAllUsersDetails(DataTable dt)
        {
            List<ResponseRegistrationCLS> SelectedUser = new List<ResponseRegistrationCLS>();
            if (dt == null || dt.Rows.Count == 0)
            {
                return null;
            }

            foreach (DataRow row in dt.Rows)
            {
                ResponseRegistrationCLS users = new ResponseRegistrationCLS();
                users.UserID = Convert.ToInt32(row["UserID"]?.ToString() ?? string.Empty);
                users.MobileNumber = row["MobileNumber"]?.ToString() ?? string.Empty;
                users.EmailID = row["EmailID"]?.ToString() ?? string.Empty;
                users.DateofCreation = Convert.ToDateTime(row["DateofCreation"]?.ToString() ?? string.Empty);
                users.DeviceID = row["DeviceID"]?.ToString() ?? string.Empty;
                users.SubscriptionExpiryDate = Convert.ToDateTime(row["SubscriptionExpiryDate"]?.ToString() ?? string.Empty);
                users.Platform = row["Platform"]?.ToString() ?? string.Empty;
                users.AppVersion = row["AppVersion"]?.ToString() ?? string.Empty;
                users.LastAPICallDate = Convert.ToDateTime(row["LastAPICallDate"]?.ToString() ?? string.Empty);
                users.SubscriptionStatus = row["SubscriptionStatus"]?.ToString() ?? string.Empty;
                users.AdminNotes = row["AdminNotes"]?.ToString() ?? string.Empty;
                users.AppCode = row["AppCode"]?.ToString() ?? string.Empty;
                SelectedUser.Add(users);
            }

            return SelectedUser;
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

        public UpdateAppVersionModel UpdateAppVersion(UpdateAppVersionModel updateAppVersionModel)
        {
            try
            {
                _connectionCls.clearParameter();
                _connectionCls.addParameter("@UserID", updateAppVersionModel.UserID);
                _connectionCls.addParameter("@MobileNumber", updateAppVersionModel.MobileNumber);
                _connectionCls.addParameter("@DeviceId", updateAppVersionModel.DeviceId);
                _connectionCls.addParameter("@AppVersion", updateAppVersionModel.AppVersion);
                _connectionCls.addParameter("@Platforms", updateAppVersionModel.Platforms);
                _connectionCls.BeginTransaction();
                object result = _connectionCls.ExecuteScalar("P2_UpdateAppVersion", CommandType.StoredProcedure);
                _connectionCls.CommitTransaction();
                return updateAppVersionModel = updateAppVersionModel;
            }
            catch (Exception ex)
            {
                throw new Exception("P2_UpdateAppVersion : " + ex.Message);
            }
        }

        public ResponseUserModel UpdateUserInactiveToTrial(int UserID, DateTime SubscriptionExpiryDate)
        {
            try
            {
                _connectionCls.clearParameter();
                _connectionCls.addParameter("@UserID", UserID);
                _connectionCls.addParameter("@SubscriptionExpiryDate", SubscriptionExpiryDate);
                _connectionCls.BeginTransaction();
                //object result = _connectionCls.ExecuteScalar("SP_Update_UserInActiveToTrial", CommandType.StoredProcedure);
                //_connectionCls.CommitTransaction();

                DataTable dt = ConvertDatareadertoDataTable(_connectionCls.ExecuteReader("SP_Update_UserInActiveToTrial", CommandType.StoredProcedure));
                return ConvertUpdateuser(dt);
            }
            catch (Exception ex)
            {
                throw new Exception("SP_Update_UserInActiveToTrial : " + ex.Message);
            }
        }
        public ResponseUserModel ConvertUpdateuser(DataTable dt)
        {
            if (dt == null || dt.Rows.Count == 0)
            {
                return null;
            }

            DataRow row = dt.Rows[0];
            ResponseUserModel users = new ResponseUserModel();

            users.UserID = Convert.ToInt32(row["UserID"]?.ToString() ?? string.Empty);
            users.MobileNumber = row["MobileNumber"]?.ToString() ?? string.Empty;
            users.EmailID = row["EmailID"]?.ToString() ?? string.Empty;
            users.DateofCreation = Convert.ToDateTime(row["DateofCreation"]?.ToString() ?? string.Empty);
            users.DeviceID = row["DeviceID"]?.ToString() ?? string.Empty;
            users.SubscriptionExpiryDate = Convert.ToDateTime(row["SubscriptionExpiryDate"]?.ToString() ?? string.Empty);
            users.Platform = row["Platform"]?.ToString() ?? string.Empty;
            users.AppVersion = row["AppVersion"]?.ToString() ?? string.Empty;
            users.LastAPICallDate = Convert.ToDateTime(row["LastAPICallDate"]?.ToString() ?? string.Empty);
            users.AdminNotes = row["AdminNotes"]?.ToString() ?? string.Empty;
            users.AppCode = row["AppCode"]?.ToString() ?? string.Empty;

            return users;
        }
    }
}
