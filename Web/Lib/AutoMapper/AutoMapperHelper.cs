using AutoMapper;
using Entity;

namespace Web.Lib.AutoMapper
{
    public class AutoMapperHelper
    {
        public static IMapper GetMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMissingTypeMaps = true;

                // create maps here

                // Map all entities to themselves to enable deep cloning
                cfg.CreateMap<ApplicationUser, ApplicationUser>();
                cfg.CreateMap<Company, Company>();
                cfg.CreateMap<CompanySize, CompanySize>();
                cfg.CreateMap<CompanyCategory, CompanyCategory>();
                cfg.CreateMap<Country, Country>();
                cfg.CreateMap<Currency, Currency>();
                cfg.CreateMap<Email, Email>();
                cfg.CreateMap<HomeOfficeOption, HomeOfficeOption>();
                cfg.CreateMap<Internship, Internship>();
                cfg.CreateMap<Internship, Internship>();
                cfg.CreateMap<InternshipAmountType, InternshipAmountType>();
                cfg.CreateMap<InternshipCategory, InternshipCategory>();
                cfg.CreateMap<InternshipDurationType, InternshipDurationType>();
                cfg.CreateMap<Language, Language>();
                cfg.CreateMap<Log, Log>();
                cfg.CreateMap<Message, Message>();
                cfg.CreateMap<StudentStatusOption, StudentStatusOption>();
                cfg.CreateMap<Thesis, Thesis>();
                cfg.CreateMap<ThesisType, ThesisType>();

            });

            return new Mapper(config);
        }
    }
}