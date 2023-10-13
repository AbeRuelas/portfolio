using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Models.Domain.Organizations
{
    public class BaseOrganization
    {
        public string Name { get; set; }
        public string Logo { get; set; }
        public string SiteUrl { get; set; }
    }
}
