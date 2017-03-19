using System;
using Core.Helpers.Internship;
using Entity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.Linq;
using Core.Extensions;
using Core.Helpers;
using NLipsum.Core;
using Service.Services.Languages;
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
            // basic setup
            const string adminRoleName = "Admin";
            const string adminUserName = "yazoo@email.cz";
            const string adminDefaultPassword = "101154";

            // create admin role
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var adminRole = new IdentityRole { Name = adminRoleName };
            roleManager.Create(adminRole);

            // create admin user
            var userStore = new UserStore<ApplicationUser>(context);
            var userManager = new UserManager<ApplicationUser>(userStore);
            var newUser = new ApplicationUser()
            {
                UserName = adminUserName,
                Email = adminUserName,
                EmailConfirmed = true
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
                    CodeName = "Ecommerce",
                    Name= "E-commerce",
                },
                new CompanyCategory()
                {
                    CodeName = "Startup",
                    Name= "Startup"
                },
                 new CompanyCategory()
                {
                    CodeName = "Technology",
                    Name= "Technologie"
                },
                new CompanyCategory()
                {
                    CodeName = "Travel",
                    Name= "Cestování"
                },
                new CompanyCategory()
                {
                    CodeName = "Finance",
                    Name= "Finance"
                },
                new CompanyCategory()
                {
                    CodeName = "Media",
                    Name= "Media"
                },
                new CompanyCategory()
                {
                    CodeName = "Food",
                    Name= "Jídlo"
                },
                new CompanyCategory()
                {
                    CodeName = "SocialNetworks",
                    Name= "Sociální sítě"
                },
               new CompanyCategory()
                {
                    CodeName = "Others",
                    Name= "Jiné"
                },
                new CompanyCategory()
                {
                    CodeName = "Games",
                    Name= "Hry a zábava"
                },
                  new CompanyCategory()
                {
                    CodeName = "Services",
                    Name= "Služby"
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
                    CodeName = "Backend programátor",
                    Name= "BackendProgrammer"
                },
                new InternshipCategory()
                {
                    CodeName = "Designer",
                    Name= "Designer"
                },
                new InternshipCategory()
                {
                    CodeName = "Front-end programátor",
                    Name= "FrontendProgrammer"
                },
                new InternshipCategory()
                {
                    CodeName = "Manager",
                    Name= "Manager"
                },
                new InternshipCategory()
                {
                    CodeName = "Project manager",
                    Name= "ProjectManager"
                },
                new InternshipCategory()
                {
                    CodeName = "Analytik",
                    Name= "Analyst"
                },
                new InternshipCategory()
                {
                    CodeName = "Databázový specialista",
                    Name= "DatabaseAnalyst"
                },
                new InternshipCategory()
                {
                    CodeName = "Technical writer",
                    Name= "TechnicalWriter"
                },
                    new InternshipCategory()
                {
                    CodeName = "IT",
                    Name= "IT"
                },
                new InternshipCategory()
                {
                    CodeName = "Ostatní",
                    Name= "Others"
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
                new Country()
                {
                    CountryName = "United States",
                    CountryCode = "us",
                    CodeName = "us",
                    Icon = "us",
                },
                new Country()
                {
                    CountryName = "Australia",
                    CountryCode = "au",
                    CodeName = "au",
                    Icon = "au",
                },
                new Country()
                {
                    CountryName = "Canada",
                    CountryCode = "ca",
                    CodeName = "ca",
                    Icon = "ca",
                },
                new Country()
                {
                    CountryName = "France",
                    CountryCode = "fr",
                    CodeName = "fr",
                    Icon = "fr",
                },
                new Country()
                {
                    CountryName = "Spain",
                    CountryCode = "es",
                    CodeName = "es",
                    Icon = "es",
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
                    AmountTypeName = "Hodina",
                    CodeName = "Hour",
                },
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
                //new Currency()
                //{
                //    CurrencyName = "£",
                //    CodeName = "GBP",
                //    ShowSignOnLeft = false
                //},
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
                new Language()
                {
                    CodeName = "ru",
                    LanguageName = "Ruština",
                    IconClass = "ru",
                },
                new Language()
                {
                    CodeName = "Other",
                    LanguageName = "Jiný",
                    IconClass = "",
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

            // education types
            var educationTypes = new List<EducationType>()
            {
                new EducationType()
                {
                    CodeName = "Any",
                    Name = "Jakékoliv",
                    ID = 1
                },
                 new EducationType()
                {
                     CodeName = "HighSchool",
                     Name = "Středoškolské",
                     ID = 2
                },
                  new EducationType()
                {
                    CodeName = "Bachelor",
                    Name = "Bakalářské",
                    ID = 3
                },
                    new EducationType()
                {
                    CodeName = "University",
                    Name = "Magisterské",
                    ID = 4
                },
            };

            // add to context
            foreach (var educationType in educationTypes)
            {
                context.EducationTypes.Add(educationType);
            }

            // fill test data 
            FillTestData(context, userManager);

            // save data
            context.SaveChanges();
        }

        private void FillTestData(AppContext context, UserManager<ApplicationUser> userManager)
        {
            // define genereated data 
            const int internshipsCount = 250;
            const int companyCount = 50;
            const int thesesCount = 100;
            const string userPassword = "adminadmin";

            // generate companies
            for (var i = 0; i < companyCount; i++)
            {
                var userName = $"{GetLoremIpsumName(true)}@{GetLoremIpsumName(true)}.com";
                // create user for each company first
                var newUser = new ApplicationUser()
                {
                    Nickname = GetLoremIpsumName(),
                    UserName = userName,
                    Email = userName,
                };

                userManager.Create(newUser, userPassword);

                // get just created user
                var user = userManager.FindByEmail(newUser.Email);

                var companyName = GetLoremIpsumName();

                var company = new Company()
                {
                    CreatedByApplicationUserId = user.Id,
                    UpdatedByApplicationUserId = user.Id,
                    CompanyCategory = context.CompanyCategories.RandomItem(),
                    City = GetLoremIpsumName(),
                    CompanyName = companyName,
                    CompanySize = context.CompanySizes.RandomItem(),
                    Country = context.Countries.RandomItem(),
                    Web = $"http://www.{GetLoremIpsumName(true)}.com",
                    Address = GetLoremIpsumName(),
                    Lat = GetRandomLattitude(),
                    Lng = GetRandomLongtitude(),
                    LongDescription = GetLoremIpsumText(),
                    CodeName = StringHelper.GetCodeName(companyName),
                    Created = DateTime.Now,
                    Updated = DateTime.Now,
                    Guid = Guid.NewGuid()
                };

                context.Companies.Add(company);
            }

            // genereate internships
            for (var i = 0; i < internshipsCount; i++)
            {
                // get random company for this internship
                var company = context.Companies.RandomItem();

                // duration
                var minDuration = context.InternshipDurationTypes.RandomItem();
                var maxDuration = context.InternshipDurationTypes.RandomItem();

                var minDurationValue = GetRandomDurationValue();
                var maxDurationValue = GetRandomDurationValue(minDurationValue);

                var amount = GetRandomInternshipAmout();

                var internship = new Internship()
                {
                    CreatedByApplicationUser = company.CreatedByApplicationUser,
                    UpdatedByApplicationUser = company.CreatedByApplicationUser,
                    Created = DateTime.Now,
                    Updated = DateTime.Now,
                    Company = company,
                    AmountType = context.InternshipAmountTypes.RandomItem(),
                    City = GetLoremIpsumName(),
                    Country = context.Countries.RandomItem(),
                    Currency = GetCurrency(context.Currencies.ToList()),
                    Amount = amount,
                    Description = GetInternshipDescription(),
                    HasFlexibleHours = GetRandomBool(),
                    WorkingHours = GetRandomWorkingHours(),
                    EducationType = context.EducationTypes.RandomItem(),
                    InternshipCategory = context.InternshipCategories.RandomItem(),
                    IsActive = true,
                    ActiveSince = DateTime.Now,
                    IsPaid = amount > 0,
                    Languages = GetRandomLanguages(context),
                    MaxDurationType = maxDuration,
                    MinDurationType = minDuration,
                    MaxDurationInDays = GetDurationInDays(maxDuration.DurationTypeEnum, maxDurationValue),
                    MaxDurationInWeeks = GetDurationInWeeks(maxDuration.DurationTypeEnum, maxDurationValue),
                    MaxDurationInMonths = GetDurationInMonths(maxDuration.DurationTypeEnum, maxDurationValue),
                    MinDurationInDays = GetDurationInDays(minDuration.DurationTypeEnum, minDurationValue),
                    MinDurationInWeeks = GetDurationInWeeks(minDuration.DurationTypeEnum, minDurationValue),
                    MinDurationInMonths = GetDurationInMonths(minDuration.DurationTypeEnum, minDurationValue),
                    Requirements = GetLoremIpsumText(),
                    Title = GetLoremIpsumName(),
                    StudentStatusOption = context.StudentStatusOptions.RandomItem(),
                    StartDate = GetRandomDate(),
                };

                internship.CodeName = StringHelper.GetCodeName(internship.Title);

                context.Internships.Add(internship);
            }

            for (var i = 0; i < thesesCount; i++)
            {
                var company = context.Companies.RandomItem();

                var amount = GetRandomInternshipAmout();

                var thesis = new Thesis()
                {
                    Amount = amount,
                    CreatedByApplicationUser = company.CreatedByApplicationUser,
                    UpdatedByApplicationUser = company.CreatedByApplicationUser,
                    Company = company,
                    Currency = GetCurrency(context.Currencies.ToList()),
                    Description = GetLoremIpsumText(),
                    InternshipCategory = context.InternshipCategories.RandomItem(),
                    IsActive = true,
                    ActiveSince = DateTime.Now,
                    IsPaid = amount > 0,
                    ThesisName = GetLoremIpsumName(),
                    ThesisType = context.ThesisTypes.RandomItem(),
                    Created = DateTime.Now,
                    Updated = DateTime.Now,
                };

                thesis.CodeName = StringHelper.GetCodeName(thesis.ThesisName);

                context.Theses.Add(thesis);
            }
        }

        #region Helper methods

        private Currency GetCurrency(IList<Currency> currencies)
        {
            var czkCurrency = currencies.FirstOrDefault(m => m.CodeName == "CZK");
            return czkCurrency ?? currencies.First();
        }

        #endregion

        #region Random methods

        private DateTime GetRandomDate()
        {
            var gen = new Random();
            var start = DateTime.Now;
            var range = (DateTime.Now.AddMonths(12) - start).Days;
            return start.AddDays(gen.Next(range));
        }

        private int GetRandomDurationValue(int minValue)
        {
            var random = new Random();
            return random.Next(minValue, 15);
        }

        private int GetRandomDurationValue()
        {
            var random = new Random();
            return random.Next(1, 10);
        }

        private string GetRandomLanguages(AppContext context)
        {
            var random = new Random();
            return string.Join(LanguageService.InternshipLanguageStringSeparator.ToString(), context.Languages.Take(random.Next(0, context.Languages.Count())));
        }

        private string GetRandomWorkingHours()
        {
            var random = new Random();

            return $"{random.Next(4, 12)}:00 - {random.Next(12, 24)}:00";
        }

        private bool GetRandomBool()
        {
            var random = new Random();
            return random.Next(1, 3) == 2;
        }


        private int GetRandomInternshipAmout()
        {
            var random = new Random();
            return random.Next(1, 100000);
        }

        private float GetRandomLongtitude()
        {
            var random = new Random();
            return random.Next(-180, 180);
        }

        private float GetRandomLattitude()
        {
            var random = new Random();
            return random.Next(-90, 90);
        }

        private string GetLoremIpsumName(bool oneWordOnly = false)
        {
            var lipsumGenerator = new LipsumGenerator();
            var random = new Random();

            return oneWordOnly ? string.Join(" ", lipsumGenerator.GenerateWords(1)) :string.Join(" ", lipsumGenerator.GenerateWords(random.Next(1, 3)));
        }

        private string GetLoremIpsumText()
        {
            var lipsumGenerator = new LipsumGenerator();
            var random = new Random();

            return string.Join(" ", lipsumGenerator.GenerateParagraphs(random.Next(1, 9)));
        }

        #endregion

        #region Duration helper - Copy from InternshipDurationService

        /// <summary>
        /// Gets duration in weeks from given duration type
        /// </summary>
        /// <param name="durationType">Duration type (month, day, week..)</param>
        /// <param name="duration">Duration</param>
        /// <returns>Duration in weeks</returns>
        private int GetDurationInWeeks(InternshipDurationTypeEnum durationType, int duration)
        {
            switch (durationType)
            {
                case InternshipDurationTypeEnum.Weeks:
                    // no need to convert
                    return duration;
                case InternshipDurationTypeEnum.Days:
                    // convert Days to Weeks
                    return (int)duration / 7;
                case InternshipDurationTypeEnum.Months:
                    // convert Months to Weeks
                    return (int)duration * 4;
                default:
                    throw new ArgumentException("Invalid duration type");
            }
        }

        /// <summary>
        /// Gets duration in days from given duration type
        /// </summary>
        /// <param name="durationType">Duration type (month, day, week..)</param>
        /// <param name="duration">Duration</param>
        /// <returns>Duration in days</returns>
        private int GetDurationInDays(InternshipDurationTypeEnum durationType, int duration)
        {
            switch (durationType)
            {
                case InternshipDurationTypeEnum.Weeks:
                    // convert weeks to days
                    return duration * 7;
                case InternshipDurationTypeEnum.Days:
                    // no need to convert
                    return duration;
                case InternshipDurationTypeEnum.Months:
                    // convert Months to Days
                    return duration * 30;
                default:
                    throw new ArgumentException("Invalid duration type");
            }
        }

        /// <summary>
        /// Gets duration in months from given duration type
        /// </summary>
        /// <param name="durationType">Duration type (month, day, week..)</param>
        /// <param name="duration">Duration</param>
        /// <returns>Duration in months</returns>
        private int GetDurationInMonths(InternshipDurationTypeEnum durationType, int duration)
        {
            switch (durationType)
            {
                case InternshipDurationTypeEnum.Weeks:
                    // convert weeks to monts
                    return (int)duration / 4;
                case InternshipDurationTypeEnum.Days:
                    // convert Days to Months
                    return (int)duration / 30;
                case InternshipDurationTypeEnum.Months:
                    // no need to convert
                    return duration;
                default:
                    throw new ArgumentException("Invalid duration type");
            }
        }

        private string GetInternshipDescription()
        {
            return @"
<strong>Job Description</strong>

Develop and lead a strong software team to create world class NPI capability through innovative development and support of operations systems and engineering tools. You will define and implement the development methodologies and technologies that will enable the team to consistently deliver high quality systems and tools on time and within budget. In addition, you will ensure the ongoing development of an outstanding team.Software Engineers conduct or participate in multidisciplinary software development and collaborate with other engineers in the team. Determines computer user needs, advises users on best practices and seeks use cases for defining accurate requirements even when the users are not sure of what they need. Responds to customer/client requests or events as they occur. Develops solutions to problems utilizing formal education, judgement and formal software process.

<strong>Qualifications</strong>

Minimum Qualifications:
BS in Computer Science and equivalent with a minimum of 6+ years of relevant experience in software development and management on Microsoft development platform.

Preferred Qualifications
Strong software system architecture experience with proficient skills on Microsoft development platform - .NET, C#, SQL, ASP, C/C++, WPF, LINQ, IIS, AJAX, Office automation.
Solid SW product life cycle management capabilities.
Execution focus - proven track record of on-time & in-budget delivery of high quality SW and process based solutions.
Ability to work through ambiguity and drive cross organization teams to achieve results.
Experience in working with off-shore teams.
Excellent organizational, communication verbal and written, mentoring, and conflict management skills

You're able to make the complex simple and have demonstrated an ability to understand and explain issues from both a technical and a business functional point of view.You effortlessly switch between big picture thinking and nit-picking detail.You solve problems with the right mix of creativity and deep analysis.You're passionate about communication, group dynamics and coaching.You have an insatiable appetite for learning new things and improving existing ones. You cultivate that same appetite in your developers and teams.You work at speed and inspire with your can-do attitude. You pay attention to details and take great pride in your work.

<strong>Inside this Business Group</strong>

The Programmable Solutions Group (PSG) was formed from the acquisition of Altera. As part of Intel, PSG will create market-leading programmable logic devices that deliver a wider range of capabilities than customers experience today. Combining Altera's industry-leading FPGA technology and customer support with Intel's world-class semiconductor manufacturing capabilities will enable customers to create the next generation of electronic systems with unmatched performance and power efficiency. PSG takes pride in creating an energetic and dynamic work environment that is driven by ingenuity and innovation. We believe the growth and success of our group is directly linked to the growth and satisfaction of our employees. That is why PSG is committed to a work environment that is flexible and collaborative, and allows our employees to reach their full potential.


Posting Statement. Intel prohibits discrimination based on race, color, religion, gender, national origin, age, disability, veteran status, marital status, pregnancy, gender expression or identity, sexual orientation or any other legally protected status.
";
        }

        #endregion
    }
}