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
                DisplayName = "Nejnovější",
                Filter = InternshipOrderFilterEnum.Newest
            },
            new InternshipOrderFilterModel()
            {
                DisplayName = "Nejstarší",
                Filter = InternshipOrderFilterEnum.Oldest
            },
            new InternshipOrderFilterModel()
            {
                DisplayName = "Nejdelší",
                Filter = InternshipOrderFilterEnum.Longest
            },
            new InternshipOrderFilterModel()
            {
                DisplayName = "Nejkratší",
                Filter = InternshipOrderFilterEnum.Shortest
            },
            new InternshipOrderFilterModel()
            {
                DisplayName = "Nejbližší nástup",
                Filter = InternshipOrderFilterEnum.ClosestStart
            },
            new InternshipOrderFilterModel()
            {
                DisplayName = "Nejvzdálenější nástup",
                Filter = InternshipOrderFilterEnum.OutermostStart
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
