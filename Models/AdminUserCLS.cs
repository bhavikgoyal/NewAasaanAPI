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

        public DateTime DateOfCreation { get; set; }
 
        public bool SubscriptionStatus { get; set; }

        public DateTime? SubscriptionExpiryDate { get; set; }

        public string Status { get; set; }

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

  public class ResponseRegistrationCLS
  {
    public int UserID { get; set; }
    public string MobileNumber { get; set; }
    public string EmailID { get; set; }
    public DateTime DateofCreation { get; set; }
    public string DeviceID { get; set; }
    public DateTime SubscriptionExpiryDate { get; set; }
    public string Platform { get; set; }
    public string AppVersion { get; set; }
    public DateTime LastAPICallDate { get; set; }
    public string AdminNotes { get; set; }
    public string AppCode { get; set; }
    public string SubscriptionStatus { get; set; }
  }

  public class CheckMembershipStatusModel
  {
    public int id { get; set; }
    public string email { get; set; }
    public string password { get; set; }
    public DateTime createdata { get; set; }
    public string facebook_firstname { get; set; }
    public string facebook_lastname { get; set; }
    public bool isfacebookuser { get; set; }
    public bool isgoogleuser { get; set; }
    public string FacebookID { get; set; }
    public string GoogleID { get; set; }
    public string google_firstname { get; set; }
    public string google_lastname { get; set; }
    public string DeviceId { get; set; }
    public bool IsActive { get; set; }
    public string MobileNumber { get; set; }
    public string states { get; set; }
    public string Platforms { get; set; }
    public string AppVersion { get; set; }
    public string LastAPICallDate { get; set; }
    public int UserRegistrationId { get; set; }
    public int Userid { get; set;}
    public DateTime RegistaredDate { get; set; }
    public int ExtendDays { get; set; }
    public DateTime ExpiryDate { get; set; }
    public int PaidAmount { get; set; }
    public string Notes { get; set; }
    public string AppCode { get; set; }
    public string UserStatus { get; set; }

  }

  public class UpdateAppVersionModel
  {
    public int UserID { get; set; }
    public string MobileNumber { get; set; }
    public string DeviceId { get; set; }
    public string AppVersion { get; set; }
    public string Platforms { get; set; }
  }

}
