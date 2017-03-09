using PagedList;
using System.Collections.Generic;
using UI.Builders.Internship.Enums;
using UI.Builders.Internship.Models;
using UI.Builders.Master.Views;

namespace UI.Builders.Internship.Views
{
    public class InternshipBrowseView : MasterView
    {
        public IPagedList<InternshipBrowseModel> Internships { get; set; }
        public IEnumerable<InternshipCategoryModel> InternshipCategories { get; set; }

        public IList<InternshipOrderFilterModel> OrderFilters => new List<InternshipOrderFilterModel>()
        {
            new InternshipOrderFilterModel()
            {
                DisplayName = "Od nejnovějších",
                Filter = InternshipOrderFilterEnum.Newest
            },
            new InternshipOrderFilterModel()
            {
                DisplayName = "Od nejstarších",
                Filter = InternshipOrderFilterEnum.Oldest
            },
            new InternshipOrderFilterModel()
            {
                DisplayName = "Od nejdelších",
                Filter = InternshipOrderFilterEnum.Longest
            },
            new InternshipOrderFilterModel()
            {
                DisplayName = "Od nejkratších",
                Filter = InternshipOrderFilterEnum.Shortest
            },
        };

        public IList<InternshiPaidFilterModel> PaidFilters => new List<InternshiPaidFilterModel>()
        {
            new InternshiPaidFilterModel()
            {
                DisplayName = "Nezáleží",
                Filter = InternshipPaidFilterEnum.Any
            },
               new InternshiPaidFilterModel()
            {
                DisplayName = "Pouze placené",
                Filter = InternshipPaidFilterEnum.Paid
            },
                  new InternshiPaidFilterModel()
            {
                DisplayName = "Pouze neplacené",
                Filter = InternshipPaidFilterEnum.NotPaid
            }
        };

        public IList<InternshipLengthFilterModel> LengthFilters => new List<InternshipLengthFilterModel>()
        {
            new InternshipLengthFilterModel()
            {
                DisplayName = "Nezáleží",
                Filter = InternshipLengthFilterEnum.Any
            },
            new InternshipLengthFilterModel()
            {
                DisplayName = "Do měsíce",
                Filter = InternshipLengthFilterEnum.UpToAMonth
            },
            new InternshipLengthFilterModel()
            {
                DisplayName = "1 - 2 měsíce",
                Filter = InternshipLengthFilterEnum.OneToTwoMonths
            },
            new InternshipLengthFilterModel()
            {
                DisplayName = "3 - 6 měsíců",
                Filter = InternshipLengthFilterEnum.ThreeToSixMonths
            },
            new InternshipLengthFilterModel()
            {
                DisplayName = "Více než 6 měsíců",
                Filter = InternshipLengthFilterEnum.MoreThenSixMonths
            }
        };
    }
}
