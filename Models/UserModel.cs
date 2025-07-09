
namespace Aasaan_API.Models
{
  public class UserModel
  {

  }

  public class RequestUserModel
  {
    public string MobileNumber { get; set; }
    public string DeviceId { get; set; }
  }

  public class ResponseUserModel
  {
    public int UserID { get; set; }
    public string MobileNumber { get; set; }
    public string EmailID { get; set; }
    public string? DateofCreation { get; set; }
    public string DeviceID { get; set; }
    public string? SubscriptionExpiryDate { get; set; }
    public string ExpiryDateApp {  get; set; }
    public string Platform { get; set; }
    public string AppVersion { get; set; }
    public string? LastAPICallDate { get; set; }
    public string AdminNotes { get; set; }
    public string SubscriptionStatus { get; set; }
    public string AppCode { get; set; }
    public string Token { get; internal set; }

    public static implicit operator ResponseUserModel(List<ResponseUserModel> v)
    {
      throw new NotImplementedException();
    }
  }

  public class ResponseDeleteUserModel
  {
    public int UserID { get; set; }
    public string MobileNumber { get; set; }
    public string EmailID { get; set; }
    public DateTime? DateofCreation { get; set; }
    public string DeviceID { get; set; }
    public DateTime? SubscriptionExpiryDate { get; set; }
    public string Platform { get; set; }
    public string AppVersion { get; set; }
    public DateTime? LastAPICallDate { get; set; }
    public string AdminNotes { get; set; }
    public string AppCode { get; set; }

  }

  public class ResponseUpdateUserModel
  {
    public int UserID { get; set; }
    public string MobileNumber { get; set; }
    public string EmailID { get; set; }
    public string DateofCreation { get; set; }
    public string DeviceID { get; set; }
    public string SubscriptionExpiryDate { get; set; }
    public string Platform { get; set; }
    public string AppVersion { get; set; }
    public string LastAPICallDate { get; set; }
    public string AdminNotes { get; set; }
    public string SubscriptionStatus { get; set; }
    public string AppCode { get; set; }

  }
}
