
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
    }
}
