using System;
using Models.Domain.Locations;
using Models.Domain.Users;

namespace Models.Domain.Organizations
{
    public class Organization
    {
        public int Id { get; set; }
        public LookUp OrganizationType { get; set; }
        public string Name { get; set; }
        public string Headline { get; set; }
        public string Description { get; set; }
        public string Logo { get; set; }
        public Location Location { get; set; }
        public string Phone { get; set; }
        public string SiteUrl { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public BaseUser CreatedBy { get; set; }
    }
}
