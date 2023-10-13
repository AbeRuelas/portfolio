using WePair.Data;
using WePair.Data.Providers;
using WePair.Models;
using WePair.Models.Domain.Users;
using WePair.Models.Domain.Organizations;
using WePair.Services.Interfaces;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using WePair.Models.Requests.Organizations;
using System.Linq;
using WePair.Models.Domain;
using WePair.Models.Enums;

namespace WePair.Services
{
    public class OrganizationService : IOrganizationService
    {
        IDataProvider _data = null;
        ILookUpService _lookUpService = null;
        ILocationService _locationService = null;
        public OrganizationService(IDataProvider data, ILookUpService lookUpService, ILocationService locationService)
        {
            _data = data;
            _lookUpService = lookUpService;
            _locationService = locationService;
        }

        public int Add(OrganizationAddRequest model, int currentUser)
        {
            int id = 0;
            _data.ExecuteNonQuery("[dbo].[Organizations_Insert]", inputParamMapper: delegate (SqlParameterCollection col)
            {
                AddCommonParams(model, col, currentUser);
                SqlParameter idOut = new SqlParameter("@Id", SqlDbType.Int);
                idOut.Direction = ParameterDirection.Output;
                col.Add(idOut);
            },
            returnParameters: delegate (SqlParameterCollection returnCollection)
            {
                object anId = returnCollection["@Id"].Value;
                int.TryParse(anId.ToString(), out id);
            });
            return id;
        }

        public Organization SelectById(int id)
        {
            Organization organization = null;
            _data.ExecuteCmd("[dbo].[Organizations_Select_ById]", delegate (SqlParameterCollection col)
            {
                col.AddWithValue("@Id", id);
            },
            delegate (IDataReader reader, short set)
            {
                organization = MapSingleOrganization(reader);
            });
            return organization;
        }

        public Paged<Organization> SelectAll_Paginated(int pageIndex, int pageSize)
        {
            Paged<Organization> pagedList = null;
            List<Organization> list = null;
            int totalCount = 0;
            _data.ExecuteCmd("[dbo].[Organizations_SelectAll_Paginated]", delegate (SqlParameterCollection col)
            {
                col.AddWithValue("@PageIndex", pageIndex);
                col.AddWithValue("@PageSize", pageSize);
            },
            singleRecordMapper: delegate (IDataReader reader, short set)
            {
                Organization organization = MapSingleOrganization(reader);
                if (totalCount == 0)
                {
                    totalCount = reader.GetSafeInt32(reader.FieldCount - 1);
                }
                if (list == null)
                {
                    list = new List<Organization>();
                }
                list.Add(organization);
            });
            if (list != null)
            {
                pagedList = new Paged<Organization>(list, pageIndex, pageSize, totalCount);
            }
            return pagedList;
        }

        public Paged<Organization> SelectByCreatedBy_Paginated(int pageIndex, int pageSize, int currentUser)
        {
            Paged<Organization> pagedList = null;
            List<Organization> list = null;
            int totalCount = 0;

            _data.ExecuteCmd("[dbo].[Organizations_Select_ByCreatedBy_Paginated]", delegate (SqlParameterCollection col)
            {
                col.AddWithValue("@PageIndex", pageIndex);
                col.AddWithValue("@PageSize", pageSize);
                col.AddWithValue("@CreatedBy", currentUser);
            },
            (reader, recordSetIndex) =>
            {
                Organization organization = MapSingleOrganization(reader);
                if (totalCount == 0) { reader.GetInt32(reader.FieldCount - 1); }

                if (list == null)
                {
                    list = new List<Organization>();
                }
                list.Add(organization);
            });
            if (list != null)
            {
                pagedList = new Paged<Organization>(list, pageIndex, pageSize, totalCount);
            }
            return pagedList;
        }

        public Paged<Organization> SelectByCategory(int id, int pageIndex, int pageSize)
        {
            Paged<Organization> pagedList = null;
            List<Organization> list = null;
            int totalCount = 0;

            _data.ExecuteCmd("[dbo].[Organizations_Select_ByOrganizationCategory]", delegate (SqlParameterCollection col)
            {
                col.AddWithValue("@PageIndex", pageIndex);
                col.AddWithValue("@PageSize", pageSize);
                col.AddWithValue("@Id", id);
            },
            (reader, recordSetIndex) =>
            {
                Organization organization = MapSingleOrganization(reader);
                if (totalCount == 0) { reader.GetInt32(reader.FieldCount - 1); }

                if (list == null)
                {
                    list = new List<Organization>();
                }
                list.Add(organization);
            });
            if (list != null)
            {
                pagedList = new Paged<Organization>(list, pageIndex, pageSize, totalCount);
            }
            return pagedList;
        }

        public Paged<Organization> SearchPaginated(int pageIndex, int pageSize, string query)
        {
            Paged<Organization> pagedList = null;
            List<Organization> list = null;
            int totalCount = 0;

            _data.ExecuteCmd("[dbo].[Organizations_Search_Paginated]", delegate (SqlParameterCollection col)
            {
                col.AddWithValue("@PageIndex", pageIndex);
                col.AddWithValue("@PageSize", pageSize);
                col.AddWithValue("@Query", query);
            },
            (reader, recordSetIndex) =>
            {
                Organization organization = MapSingleOrganization(reader);
                if (totalCount == 0) { reader.GetInt32(reader.FieldCount - 1); }

                if (list == null)
                {
                    list = new List<Organization>();
                }
                list.Add(organization);
            });
            if (list != null)
            {
                pagedList = new Paged<Organization>(list, pageIndex, pageSize, totalCount);
            }
            return pagedList;
        }

        public void Update(OrganizationUpdateRequest model, int currentUser)
        {
            _data.ExecuteNonQuery("[dbo].[Organizations_Update]", inputParamMapper: delegate (SqlParameterCollection col)
            {
                AddCommonParams(model, col, currentUser);
                col.AddWithValue("@Id", model.Id);
            },
            returnParameters: null);
        }

        public void Delete(int id)
        {
            _data.ExecuteNonQuery("[dbo].[Organizations_Delete_ById]", delegate (SqlParameterCollection col)
            {
                col.AddWithValue("@Id", id);
            },
            returnParameters: null);
        }
        public int AddOrgMember(OrgMemberAddRequest model, int inviteId)
        {
            int id = 0;
            _data.ExecuteNonQuery("[dbo].[OrganizationMembers_Insert]", inputParamMapper: delegate (SqlParameterCollection col)
            {  
                col.AddWithValue("@UserId", model.UserId);
                AddCommonParamsOM(model, col);
                SqlParameter idOut = new SqlParameter("@Id", SqlDbType.Int);
                idOut.Direction = ParameterDirection.Output;
                col.Add(idOut);
            },
            returnParameters: delegate (SqlParameterCollection returnCollection)
            {
                object anId = returnCollection["@Id"].Value;
                int.TryParse(anId.ToString(), out id);
            });
            if (id > 0)
            {
                DeleteInviteMem(inviteId);
            }

            return id;
        }
        public void UpdateOM(OrgMemberUpdateRequest model)
        {
            _data.ExecuteNonQuery("[dbo].[OrganizationMembers_Update]", inputParamMapper: delegate (SqlParameterCollection col)
            {
                col.AddWithValue("@UserId", model.UserId);
                AddCommonParamsOM(model, col);
                col.AddWithValue("@Id", model.Id);
            },
            returnParameters: null);
        }
        public void DeleteOM(int id)
        {
            _data.ExecuteNonQuery("[dbo].[OrganizationMembers_Delete_ById]", delegate (SqlParameterCollection col)
            {
                col.AddWithValue("@Id", id);
            },
            returnParameters: null);
        }
        public OrgMember OMSelectById(int id)
        {
            OrgMember orgMember = null;
            _data.ExecuteCmd("[dbo].[OrganizationMembers_Select_ById]", delegate (SqlParameterCollection col)
            {
                col.AddWithValue("@Id", id);
            },
            delegate (IDataReader reader, short set)
            {
                int startingIndex = 0;
                orgMember = MapSingleOrgMember(reader, ref startingIndex);
            });
            return orgMember;
        }
        public Paged<OrgMember> OMSelectByOrgId_Paginated(int orgId,int pageIndex, int pageSize )
        {
            Paged<OrgMember> pagedList = null;
            List<OrgMember> list = null;
            int totalCount = 0;

            _data.ExecuteCmd("[dbo].[OrganizationMembers_Select_ByOrgId]", delegate (SqlParameterCollection col)
            {
                col.AddWithValue("@OrgId", orgId);
                col.AddWithValue("@PageIndex", pageIndex);
                col.AddWithValue("@PageSize", pageSize);
            },
            (reader, recordSetIndex) =>
            {
                int startingIndex = 0;
                OrgMember orgMember = MapSingleOrgMember(reader, ref startingIndex);
                if (totalCount == 0) 
                {
                    totalCount = reader.GetSafeInt32(startingIndex++);
                }
                if (list == null)
                {
                    list = new List<OrgMember>();
                }
                list.Add(orgMember);
            });
            if (list != null)
            {
                pagedList = new Paged<OrgMember>(list, pageIndex, pageSize, totalCount);
            }
            return pagedList;
        }
        public Paged<OrgMember> OMSearchByPagination(int pageIndex, int pageSize, string query)
        {
            Paged<OrgMember> pagedList = null;
            List<OrgMember> list = null;
            int totalCount = 0;

            _data.ExecuteCmd("[dbo].[OrganizationMembers_Search_ByOrgByEmailByName_Paginated]", delegate (SqlParameterCollection col)
            {
                col.AddWithValue("@PageIndex", pageIndex);
                col.AddWithValue("@PageSize", pageSize);
                col.AddWithValue("@Query", query);
            },
            delegate (IDataReader reader, short set)
            {
                int startingIndex = 0;
                OrgMember orgMember = MapSingleOrgMember(reader, ref startingIndex);
                if (totalCount == 0)
                {
                    totalCount = reader.GetSafeInt32(startingIndex++);
                }
                if (list == null)
                {
                    list = new List<OrgMember>();
                }
                list.Add(orgMember);
            });
            if (totalCount > 0 && list != null)
            {
                pagedList = new Paged<OrgMember>(list, pageIndex, pageSize, totalCount);
            }
            return pagedList;
        }
        public int InviteOrgMember(OrgInviteMemberAddRequest model, int userId, string token)
        {
            int id = 0;
            _data.ExecuteNonQuery("[dbo].[InviteMembersOrg_Insert]", inputParamMapper: delegate (SqlParameterCollection col)
            {
                AddCommonParamsInvite(model, col, token, userId);
                SqlParameter idOut = new SqlParameter("@Id", SqlDbType.Int);
                idOut.Direction = ParameterDirection.Output;
                col.Add(idOut);
            },
            returnParameters: delegate (SqlParameterCollection returnCollection)
            {
                object anId = returnCollection["@Id"].Value;
                int.TryParse(anId.ToString(), out id);
            });
            return id;
        }
        public void DeleteInviteMem(int id)
        {
            _data.ExecuteNonQuery("[dbo].[InviteMembersOrg_Delete_ById]", delegate (SqlParameterCollection col)
            {
                col.AddWithValue("@Id", id);
            },
            returnParameters: null);
        }
        public OrgInviteMembers ConfirmInvite(string token, int userId)
        {
            OrgInviteMembers inviteMember = null;
            _data.ExecuteCmd("[dbo].[InviteMembersOrg_Select_ByToken]", delegate (SqlParameterCollection col)
            {
                col.AddWithValue("@Token", token);
            },
            delegate (IDataReader reader, short set)
            {
                inviteMember = MapSingleOrgInviteMember(reader);
                if (inviteMember != null)
                {
                    OrgMemberAddRequest model = new OrgMemberAddRequest
                    {
                        OrganizationId = inviteMember.OrganizationId,
                        UserId = userId, 
                        UserRoleId = inviteMember.UserRoleId,
                        PositionType = (int)PositionTypes.Support,
                        UserOrgEmail = inviteMember.Email
                    };
                    int addedMemberId = AddOrgMember(model, inviteMember.Id);
                }

            });
            return inviteMember;
        }

        private Organization MapSingleOrganization(IDataReader reader)
        {
            Organization organization = new Organization();
            int startingIndex = 0;
            organization.Id = reader.GetSafeInt32(startingIndex++);
            organization.OrganizationType = _lookUpService.MapSingleLookUp(reader, ref startingIndex);
            organization.Name = reader.GetSafeString(startingIndex++);
            organization.Headline = reader.GetSafeString(startingIndex++);
            organization.Description = reader.GetSafeString(startingIndex++);
            organization.Logo = reader.GetSafeString(startingIndex++);
            organization.Location = _locationService.MapSingleLocation(reader, ref startingIndex);
            organization.Phone = reader.GetSafeString(startingIndex++);
            organization.SiteUrl = reader.GetSafeString(startingIndex++);
            organization.DateCreated = reader.GetDateTime(startingIndex++);
            organization.DateModified = reader.GetDateTime(startingIndex++);
            organization.CreatedBy = reader.DeserializeObject<BaseUser>(startingIndex++);
            return organization;
        }

        private OrgMember MapSingleOrgMember(IDataReader reader, ref int startingIndex)
        {
            OrgMember orgMember = new OrgMember();
            
            orgMember.Id = reader.GetSafeInt32(startingIndex++);
            orgMember.Organization = new BaseOrganization();
            orgMember.Organization.Name = reader.GetSafeString(startingIndex++);
            orgMember.Organization.Logo = reader.GetSafeString(startingIndex++);
            orgMember.Organization.SiteUrl = reader.GetSafeString(startingIndex++);
            orgMember.User = reader.DeserializeObject<BaseUser>(startingIndex++);
            List<LookUp> roles = reader.DeserializeObject<List<LookUp>>(startingIndex++);
            orgMember.Roles = roles.Select(x => x.Name).ToList();
            orgMember.Position = reader.GetSafeString(startingIndex++);
            orgMember.UserOrgEmail = reader.GetSafeString(startingIndex++);
            orgMember.DateCreated = reader.GetDateTime(startingIndex++);
            orgMember.DateModified = reader.GetDateTime(startingIndex++);
            
            return orgMember;
        }
        private OrgInviteMembers MapSingleOrgInviteMember(IDataReader reader)
        {
            OrgInviteMembers inMember = new OrgInviteMembers();
            int startingIndex = 0;
            inMember.Id = reader.GetSafeInt32(startingIndex++);
            inMember.FirstName = reader.GetSafeString(startingIndex++);
            inMember.LastName = reader.GetSafeString(startingIndex++);
            inMember.Email = reader.GetSafeString(startingIndex++);
            inMember.UserRoleId = reader.GetSafeInt32(startingIndex++);
            inMember.OrganizationId = reader.GetSafeInt32(startingIndex++);
            return inMember;
        }
        private static void AddCommonParams(OrganizationAddRequest model,
            SqlParameterCollection col, int currentUser)
        {
            col.AddWithValue("@OrganizationTypeId", model.OrganizationTypeId);
            col.AddWithValue("@Name", model.Name);
            col.AddWithValue("@Headline", model.Headline);
            col.AddWithValue("@Description", model.Description);
            col.AddWithValue("@Logo", model.Logo);
            col.AddWithValue("@LocationId", model.LocationId);
            col.AddWithValue("@Phone", model.Phone);
            col.AddWithValue("@SiteUrl", model.SiteUrl);
            col.AddWithValue("@CreatedBy", currentUser);
        }
        private static void AddCommonParamsOM(OrgMemberAddRequest model,
           SqlParameterCollection col)
        {
            col.AddWithValue("@OrgId", model.OrganizationId);
            col.AddWithValue("@UserRoleId", model.UserRoleId);
            col.AddWithValue("@PositionType", model.PositionType);
            col.AddWithValue("@OrgEmail", model.UserOrgEmail);
        }

        private static void AddCommonParamsInvite(OrgInviteMemberAddRequest model,
           SqlParameterCollection col, string token, int userId)
        {
            col.AddWithValue("@FirstName", model.FirstName);
            col.AddWithValue("@LastName", model.LastName);
            col.AddWithValue("@Email", model.Email);
            col.AddWithValue("@UserRoleTypeId", model.UserRoleId);
            col.AddWithValue("@OrganizationId", model.OrganizationId);
            col.AddWithValue("@Token", token);
            col.AddWithValue("@CreatedBy", userId);
        }
        
    }
}
