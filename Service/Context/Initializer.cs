using Core.Helpers.Internship;
using Entity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using Service.Services.Thesis.Enums;

namespace Service.Context
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
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var adminRole = new IdentityRole {Name = "Admin"};
            roleManager.Create(adminRole);

            // create admin user
            const string adminUserName = "admin@admin.com";
            const string adminDefaultPassword = "admin";

            var userStore = new UserStore<ApplicationUser>(context);
            var userManager = new UserManager<ApplicationUser>(userStore);
            var newUser = new ApplicationUser()
            {
                UserName = adminUserName,
                Email = adminUserName,
                EmailConfirmed = true,
            };

            userManager.Create(newUser, adminDefaultPassword);

            // get this user
            var user = userManager.FindByName(adminUserName);

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
                    CodeName = ThesisTypeEnum.Bp.ToString(),
                    Name = "Bakalářská práce"
                },
                new ThesisType()
                {
                    CodeName = ThesisTypeEnum.Dp.ToString(),
                    Name = "Diplomová práce"
                },
                 new ThesisType()
                {
                    CodeName = ThesisTypeEnum.All.ToString(),
                    Name = "Nezáleží"
                }
            };

            foreach (var thesisType in thesisTypes)
            {
                context.ThesisTypes.Add(thesisType);
            }

            // create languages
            var languages = new List<Language>()
            {
                new Language()
                {
                    CodeName = "cz",
                    LanguageName = "Čeština",
                    IconClass = "cz",
                },
                 new Language()
                {
                    CodeName = "sk",
                    LanguageName = "Slovenština",
                    IconClass = "sk",
                },
                 new Language()
                {
                    CodeName = "de",
                    LanguageName = "Němčina",
                    IconClass = "de",
                },
                new Language()
                {
                    CodeName = "en",
                    LanguageName = "Angličtina",
                    IconClass = "gb",
                },
                new Language()
                {
                    CodeName = "es",
                    LanguageName = "Španělština",
                    IconClass = "es",
                },
                new Language()
                {
                    CodeName = "fr",
                    LanguageName = "Francoužština",
                    IconClass = "fr",
                },
                new Language()
                {
                    CodeName = "cn",
                    LanguageName = "Čínština",
                    IconClass = "cn",
                },
            };

            // add to context
            foreach (var language in languages)
            {
                context.Languages.Add(language);
            }

            // create home office options
            var homeOfficeOptions = new List<HomeOfficeOption>()
            {
                new HomeOfficeOption()
                {
                    HomeOfficeName = "Pouze home office",
                    CodeName = "OnlyHomeOffice",
                    IconClass = "green checkmark"
                },
               new HomeOfficeOption()
                {
                    HomeOfficeName = "Nutná docházka do firmy",
                    CodeName = "OnlyInCompany",
                    IconClass = "red remove"
                },
               new HomeOfficeOption()
                {
                    HomeOfficeName = "Home office možný po zaučení",
                    CodeName = "HomeOfficeAfterTraining",
                    IconClass = ""
                },
            };

            // add to context
            foreach (var homeOfficeOption in homeOfficeOptions)
            {
                context.HomeOfficeOptions.Add(homeOfficeOption);
            }


            // add to context
            foreach (var language in languages)
            {
                context.Languages.Add(language);
            }

            // create student status options
            var studentStatusOptions = new List<StudentStatusOption>()
            {
                new StudentStatusOption()
                {
                    StatusName = "Ano",
                    CodeName = "Yes",
                    IconClass = "green checkmark"
                },
                new StudentStatusOption()
                {
                    StatusName = "Ne",
                    CodeName = "No",
                    IconClass = "red remove"
                },
                new StudentStatusOption()
                {
                    StatusName = "Nezáleží",
                    CodeName = "Any",
                    IconClass = "hand peace"
                },
            };

            // add to context
            foreach (var studentStatusOption in studentStatusOptions)
            {
                context.StudentStatusOptions.Add(studentStatusOption);
            }

            // save data
            context.SaveChanges();
        }
    }
}