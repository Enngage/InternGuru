using Core.Helpers.Internship;

namespace UI.Builders.Auth.Models
{
    public class AuthInternshipDurationType
    {
        public int ID { get; set; }
        public string DurationName { get; set; }
        public string CodeName { get; set; }
        public  InternshipDurationTypeEnum DurationTypeEnum { get; set; }
    }
}
