using System.Collections.Generic;

namespace ITMO.SoftwareTesting.Dates.Contracts.Models
{
    public class GroupDetails
    {
        public List<PersonListItem> Members { get; set; }
        public List<PersonListItem> Invites { get; set; }
        public List<int> Events { get; set; }
    }
}