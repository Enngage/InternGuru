
using Common.Helpers.Internship;
using Entity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;

namespace Core.Context
{
    public class Initializer : System.Data.Entity.CreateDatabaseIfNotExists<AppContext>
    {
        protected override void Seed(AppContext context)
        {
            RunSeed(context);
        }

        private void RunSeed(AppContext context)
        {
            // create admin role
            var roleManager = new RoleManager<Microsoft.AspNet.Identity.EntityFramework.IdentityRole>(new RoleStore<IdentityRole>(context));
            var adminRole = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
            adminRole.Name = "Admin";
            roleManager.Create(adminRole);

            // create admin user
            var userStore = new UserStore<ApplicationUser>(context);
            var userManager = new UserManager<ApplicationUser>(userStore);
            var newUser = new ApplicationUser()
            {
                UserName = "Enn",
                Email = "admin@email.com",

            };

            userManager.Create(newUser, "101154");

            // get this user
            var user = userManager.FindByName("Enn");

            // assign admin role to admin user
            userManager.AddToRole(user.Id, adminRole.Name);

            // company categories
            var companyCategories = new List<CompanyCategory>()
            {
                new CompanyCategory()
                {
                    CodeName = "Technology",
                    Name= "Technologická"
                },
                 new CompanyCategory()
                {
                    CodeName = "IT",
                    Name= "IT"
                },
            };

            foreach (var category in companyCategories)
            {
                context.CompanyCategories.Add(category);
            }

            // internship categories
            var internshipCategories = new List<InternshipCategory>()
            {
                new InternshipCategory()
                {
                    CodeName = "Technology2",
                    Name= "Technologická2"
                },
                 new InternshipCategory()
                {
                    CodeName = "IT2",
                    Name= "IT2"
                },
            };

            foreach (var category in internshipCategories)
            {
                context.InternshipCategories.Add(category);
            }

            // create countries
            var countries = new List<Country>()
            {
                new Country()
                {
                    CountryName = "Česká republika",
                    CountryCode = "cz",
                    CodeName = "cz",
                    Icon = "cz",
                },
                 new Country()
                {
                    CountryName = "Slovensko",
                    CountryCode = "sk",
                    CodeName = "sk",
                    Icon = "sk",
                },
                  new Country()
                {
                    CountryName = "Germany",
                    CountryCode = "de",
                    CodeName = "de",
                    Icon = "de",
                },
                   new Country()
                {
                    CountryName = "United Kingdom",
                    CountryCode = "gb",
                    CodeName = "gb",
                    Icon = "gb",
                },
            };

            // add to context
            foreach (var country in countries)
            {
                context.Countries.Add(country);
            }

            var durationTypes = new List<InternshipDurationType>()
            {
                new InternshipDurationType()
                {
                    CodeName = InternshipDurationTypeEnum.Days.ToString(),
                    DurationName = "Dnů",
                    ID = 1
                },
                 new InternshipDurationType()
                {
                    CodeName = InternshipDurationTypeEnum.Weeks.ToString(),
                    DurationName = "Týdnů",
                     ID = 2
                },
                  new InternshipDurationType()
                {
                    CodeName = InternshipDurationTypeEnum.Months.ToString(),
                    DurationName = "Měsíců",
                    ID = 3
                },
            };

            // add to context
            foreach (var durationType in durationTypes)
            {
                context.InternshipDurationTypes.Add(durationType);
            }

            var internshipAmountTypes = new List<InternshipAmountType>()
            {
                new InternshipAmountType()
                {
                    AmountTypeName = "Celkem",
                    CodeName = "Overall",
                },
                new InternshipAmountType()
                {
                    AmountTypeName = "Měsíc",
                    CodeName = "Month",
                },
                new InternshipAmountType()
                {
                    AmountTypeName = "Týden",
                    CodeName = "Week",
                },
            };

            // save data
            context.SaveChanges();

            // add to context
            foreach (var amountType in internshipAmountTypes)
            {
               context.InternshipAmountTypes.Add(amountType);
            }


            var currencies = new List<Currency>()
            {
                new Currency()
                {
                    CurrencyName = "Kč",
                    CodeName = "CZK",
                    ShowSignOnLeft = false
                },
                  new Currency()
                {
                    CurrencyName = "$",
                    CodeName = "USD",
                    ShowSignOnLeft = true
                },
                    new Currency()
                {
                    CurrencyName = "€",
                    CodeName = "EUR",
                    ShowSignOnLeft = false
                },
            };

            // add to context
            foreach (var currency in currencies)
            {
              context.Currencies.Add(currency);
            }


            var companySizes = new List<CompanySize>()
            {
                new CompanySize()
                {
                    CompanySizeName = "Do 10",
                    CodeName = "9",
                },
                new CompanySize()
                {
                    CompanySizeName = "10-19",
                    CodeName = "19",
                },
                new CompanySize()
                {
                    CompanySizeName = "20-49",
                    CodeName = "49",
                },
                new CompanySize()
                {
                    CompanySizeName = "50-100",
                    CodeName = "99",
                },
                new CompanySize()
                {
                    CompanySizeName = "Více než 100",
                    CodeName = "100+",
                },
            };


            // add to context
            foreach (var companySize in companySizes)
            {
                context.CompanySizes.Add(companySize);
            }

            var thesisTypes = new List<ThesisType>()
            {
                new ThesisType()
                {
                    CodeName = "bp",
                    Name = "Bakalářská práce"
                },
                new ThesisType()
                {
                    CodeName = "dp",
                    Name = "Diplomová práce"
                },
                 new ThesisType()
                {
                    CodeName = "all",
                    Name = "Nezáleží"
                }
            };

            foreach (var thesisType in thesisTypes)
            {
                context.ThesisTypes.Add(thesisType);
            }

            // save data
            context.SaveChanges();

        }
    }
}