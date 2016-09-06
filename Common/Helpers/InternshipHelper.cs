using Common.Helpers.Internship;
using System.Collections.Generic;

namespace Common.Helpers
{
    public static class InternshipHelper
    {
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
