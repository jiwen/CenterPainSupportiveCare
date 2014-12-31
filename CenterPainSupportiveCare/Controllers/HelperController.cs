using CenterPainSupportiveCareModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CenterPainSupportiveCare.Controllers
{
    public class HelperController
    {

        private static Entities db = new Entities();

        public static List<CenterPainSupportiveCare.Models.Error> ErrorMessage(Exception ex, string methodName)
        {
            var error = new CenterPainSupportiveCareModels.Error()
            {
                GUID = Guid.NewGuid().ToString(),
                Message = string.Format("Error in {0}. {1}", methodName, ex.Message),
                Stacktrace = ex.StackTrace,
                DateCreated = DateTime.UtcNow
            };
            db.Errors.Add(error);
            db.SaveChanges();

            var errors = new List<CenterPainSupportiveCare.Models.Error>();
            errors.Add(new CenterPainSupportiveCare.Models.Error(error));
            return errors;
        }
    }
}