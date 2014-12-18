using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CenterPainSupportiveCare.Models
{
    public class Status
    {
        public enum Statuses { Active = 1, Deleted = 2}

        public Status()
        {

        }

        public Status(CenterPainSupportiveCareModels.Status status)
        {
            Id = status.StatusId.ToString();
            Description = status.Description;
        }

        public string Id { get; set; }
        public string Description { get; set; }
    }
}