using Aasaan_API.Models;

namespace Aasaan_API.IServices
{
  public interface IAdmin
  {
    public ResponseDeleteUserModel getUsersDetialsByUserID(int UserID);
    public ResponseDeleteUserModel DeleteUsersRecord(int UserID);
    public ResponseDeleteUserModel saveUsersDataInHistoryTable(ResponseDeleteUserModel UserID);
    public List<CheckMembershipStatusModel> CheckMembershipStatus(string Mobilenumber, string DeviceId, Int32 UserId);
    public List<ResponseRegistrationCLS> GetAllUsersDetails();
    public UpdateAppVersionModel UpdateAppVersion(UpdateAppVersionModel updateAppVersionModel);
    public ResponseUserModel UpdateUserInactiveToTrial(int UserID, DateTime SubscriptionExpiryDate);
    Task<PaginatedResult<AdminUserCLS>> SearchUsersAsync(string? mobileNumber, int pageIndex, int pageSize);
    Task<AdminUserCLS> UpdateUserAsync(AdminUserCLS userToUpdate);
    }

    
}
