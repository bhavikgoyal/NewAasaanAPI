﻿using Aasaan_API.Models;

namespace Aasaan_API.IServices
{
  public interface IAdmin
  {
    public ResponseDeleteUserModel getUsersDetialsByUserID(int UserID, string Appcode);
    public ResponseDeleteUserModel DeleteUsersRecord(int UserID, string Appcode);
    public ResponseDeleteUserModel saveUsersDataInHistoryTable(ResponseDeleteUserModel UserID);
    public List<ResponseRegistrationCLS> GetAllUsersDetails(int PageIndex, int PageSize);
    public List<ResponseRegistrationCLS> SearchUsersAsync(string? mobileNumber, int pageSize, int pageIndex);
    public ResponseRegistrationCLS UpdateUserAsync(UpdateUsersDetilsAdmin userToUpdate);
    public ResponseRegistrationCLS ApplicationGroupeUpdateAsync(ApplicationGroupeUpdateModel applicationGroupeUpdateModel);
    public ResponseChangePassword ChangeAdminPasswords(ChangeAdminPassword changeAdminPassword);
    public List<ResponseUsersGroupDetails> GetAllGroupsApplicationsDetails(int PageIndex, int PageSize, string MobileNUmber);
    public ResponseUpdatedGroupName UpdateGroupeNameInGroupOfApplications(RequestUpdateGroupeNameInGroupOfApplication userToUpdate);

  }    
}
