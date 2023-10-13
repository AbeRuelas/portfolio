using WePair.Models;
using WePair.Models.Domain.Organizations;
using WePair.Models.Requests.Organizations;

namespace WePair.Services.Interfaces
{
    public interface IOrganizationService
    {
        public int Add(OrganizationAddRequest model, int currentUser);
        Organization SelectById(int id);
        Paged<Organization> SelectAll_Paginated(int pageIndex, int pageSize);
        Paged<Organization> SelectByCreatedBy_Paginated(int pageIndex, int pageSize, int createdBy);
        Paged<Organization> SelectByCategory(int pageIndex, int pageSize, int id);
        Paged<Organization> SearchPaginated(int pageIndex, int pageSize, string query);
        void Update(OrganizationUpdateRequest model, int currentUser);
        void Delete(int id);
        public int AddOrgMember(OrgMemberAddRequest model, int inviteId);
        public void UpdateOM(OrgMemberUpdateRequest model);
        public void DeleteOM(int id);
        public OrgMember OMSelectById(int id);
        public Paged<OrgMember> OMSelectByOrgId_Paginated(int orgId, int pageIndex, int pageSize);
        public Paged<OrgMember> OMSearchByPagination(int pageIndex, int pageSize, string query);
        public int InviteOrgMember(OrgInviteMemberAddRequest model, int userId, string token);
        public void DeleteInviteMem(int id);
        public OrgInviteMembers ConfirmInvite(string token, int userId);
    }
}
