using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CenterPainSupportiveCare.Models
{
    public class Error
    {
        public Error() { }

        public Error(CenterPainSupportiveCareModels.Error error)
        {
            Id = error.ErrorId.ToString();
            Guid = error.GUID;
            Message = error.Message;
            Stacktrace = error.Stacktrace;
            DateCreated = error.DateCreated;
        }

        public string Id { get; set; }
        public string Guid { get; set; }
        public string Message { get; set; }
        public string Stacktrace { get; set; }
        public DateTime? DateCreated { get; set; }
    }
}