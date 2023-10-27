
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
    }
}
