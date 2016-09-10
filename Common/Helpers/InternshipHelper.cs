using Common.Helpers.Internship;
using System.Collections.Generic;

namespace Common.Helpers
{
    public static class InternshipHelper
    {

        /// <summary>
        /// Gets collection of allowed amount types (e.g. "Total", "Per month" ..)
        /// </summary>
        /// <returns>Collection of duration types</returns>
        public static IEnumerable<string> GetAmountTypes()
        {
            return new List<string>()
            {
                "Celkem",
                "Měsíc"
            };
        }

        /// <summary>
        /// Gets collection of allowed duration types (e.g. "Days", "Month" ..)
        /// </summary>
        /// <returns>Collection of duration types</returns>
        public static IEnumerable<InternshipDurationTypeModel> GetInternshipDurations()
        {
            return new List<InternshipDurationTypeModel>()
            {
                new InternshipDurationTypeModel()
                {
                    DurationName = "Měsíců",
                    DurationValue = "Months",
                },
                new InternshipDurationTypeModel()
                {
                    DurationName = "Týdnů",
                    DurationValue = "Weeks",
                },
                new InternshipDurationTypeModel()
                {
                    DurationName = "Dnů",
                    DurationValue = "Days",
                }
            };
        }

        /// <summary>
        /// Gets collection of possible employee count states
        /// </summary>
        /// <returns>Collection of count states</returns>
        public static IEnumerable<InternshipCompanySizeModel> GetAllowedCompanySizes()
        {
            return new List<InternshipCompanySizeModel>()
            {
              new InternshipCompanySizeModel()
              {
                  Name = "< 10",
                  Value = "9"
              },
               new InternshipCompanySizeModel()
              {
                  Name = "10 - 19",
                  Value = "19"
              },
                new InternshipCompanySizeModel()
              {
                  Name = "20 - 49",
                  Value = "49"
              },
                 new InternshipCompanySizeModel()
              {
                  Name = "50 - 99",
                  Value = "99"
              },
                  new InternshipCompanySizeModel()
              {
                  Name = "100+",
                  Value = "100"
              },
           };
        }
    }
}
