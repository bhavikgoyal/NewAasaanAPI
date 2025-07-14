namespace Aasaan_API.Models
{
  public class ResponseUsersGroupDetails
  {
    public int UserID { get; set; }
    public string MobileNumber { get; set; }
    public string EmailID { get; set; }
    public string DateofCreation { get; set; }
    public string DeviceID { get; set; }
    public string? ExpiryDateApp { get; set; }
    public string Platform { get; set; }
    public string AppVersion { get; set; }
    public string? LastAPICallDate { get; set; }
    public string AdminNotes { get; set; }
    public string AppCode { get; set; }
    public string GroupName { get; set; }
    public string SubscriptionStatus { get; set; }
    public int TotalRecords { get; set; }
  }

  public class RequestUpdateGroupeNameInGroupOfApplication
  {
    public int UserID { get; set; }
    public string MobileNumber { get; set; }
    public List<string> AppCodes { get; set; }
  }

  public class ResponseUpdatedGroupName
  {
    public string Status { get; set; }
    public string Message { get; set; }
  }

}
