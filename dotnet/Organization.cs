using System;
using WePair.Models.Domain.Locations;
using WePair.Models.Domain.Users;

namespace WePair.Models.Domain.Organizations
{
    public class Organization : BaseOrganization
    {
        public int Id { get; set; }
        public LookUp OrganizationType { get; set; }
        public string Headline { get; set; }
        public string Description { get; set; }
        public Location Location { get; set; }
        public string Phone { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public BaseUser CreatedBy { get; set; }
    }
}
