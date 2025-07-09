using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Text.Json.Serialization;

namespace Aasaan_API.Models
{
  public class AdminUserCLS
  {
    public int UserID { get; set; }
    public string EmailID { get; set; }
    public string MobileNumber { get; set; }
    public string DeviceID { get; set; }
    public string? DateOfCreation { get; set; }
    public string SubscriptionStatus { get; set; }
    public string? SubscriptionExpiryDate { get; set; }
    public string Platform { get; set; }
    public string AppVersion { get; set; }
    public string? LastAPICallDate { get; set; }
    public string AdminNotes { get; set; }
    public string AppCode { get; set; }
    public int TotalCount { get; set; }
  }

  public class RequestRegistrationCLS
  {
    public string MobileNumber { get; set; }
    public string EmailID { get; set; }
    public string DeviceID { get; set; }
    public DateTime SubscriptionExpiryDate { get; set; }
    public string Platform { get; set; }
    public string AppVersion { get; set; }
    public string AdminNotes { get; set; }
    public string AppCode { get; set; }


  }

  public class GroupeOfapplicationsModel
  {
    public int UserID { get; set; }
    public string AppCode { get; set; }
    public string MobileNumber { get; set; }
    public string DeviceID { get; set; }
    public string AppVersion { get; set; }
    public string Platform { get; set; }
    public DateTime ExpiryDateApp { get; set; }
  }

  public class ResponseRegistrationCLS
  {
    public int UserID { get; set; }
    public string MobileNumber { get; set; }
    public string EmailID { get; set; }
    public string DateofCreation { get; set; }
    public string DeviceID { get; set; }
    public string? SubscriptionExpiryDate { get; set; }
    public string? ExpiryDateApp { get; set; }
    public string Platform { get; set; }
    public string AppVersion { get; set; }
    public string? LastAPICallDate { get; set; }
    public string AdminNotes { get; set; }
    public string AppCode { get; set; }
    public string SubscriptionStatus { get; set; }
    public int TotalRecords { get; set; }
  }

  public class UpdateAppVersionModel
  {
    public int UserID { get; set; }
    public string MobileNumber { get; set; }
    public string DeviceId { get; set; }
    public string AppVersion { get; set; }
    public string Platforms { get; set; }
  }


  public class UpdateUsersDetilsAdmin
  {
    public int UserID { get; set; }
    public string MobileNumber { get; set; }
    public string AppCode { get; set; }
    public string? SubscriptionExpiryDate { get; set; }
  }

  public class ApplicationGroupeUpdateModel
  {
    public int UserID { get; set; }
    public string MobileNumber { get; set; }
    public string AppCode { get; set; }
    public string? SubscriptionExpiryDate { get; set; }
  }

  public class ChangeAdminPassword
  {
    public int UserID { get; set; }
    public string OldPassword { get; set; }
    public string NewPassword { get; set; }
  }
  public class ResponseChangePassword
  {
    public string Message { get; set; }
  }
}
