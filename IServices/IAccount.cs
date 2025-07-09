using Aasaan_API.Models;

namespace Aasaan_API.IServices
{
  public interface IAccount
  {
    public LoginModel Login(string username, string password);
    public List<ResponseUserModel> LogInWithMobileAndDeviceId (string Mobile, string DeviceId);
    public ResponseRegistrationCLS SaveRegistrationData(RequestRegistrationCLS registrationCLS);
    public ResponseUpdateUserModel UpdateAppVersion(UpdateAppVersionModel updateAppVersionModel);
    
  }
}
