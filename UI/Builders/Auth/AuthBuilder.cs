namespace UI.Builders.Auth
{
    public class AuthBuilder
    {
        public AuthMasterBuilder AuthMasterBuilder { get; }
        public AuthCompanyBuilder AuthCompanyBuilder { get; }
        public AuthSubscriptionBuilder AuthSubscriptionBuilder { get; }
        public AuthInternshipBuilder AuthInternshipBuilder { get; }
        public AuthMessageBuilder AuthMessageBuilder { get; }
        public AuthProfileBuilder AuthProfileBuilder { get; }
        public AuthQuestionnaireBuilder AuthQuestionnaireBuilder { get; }
        public AuthThesisBuilder AuthThesisBuilder { get; }

        public AuthBuilder(
            AuthMasterBuilder authMasterBuilder,
            AuthCompanyBuilder authCompanyBuilder,
            AuthSubscriptionBuilder authuthSubscriptionBuilder,
            AuthInternshipBuilder authInternshipBuilder,
            AuthMessageBuilder authMessageBuilder,
            AuthProfileBuilder authProfileBuilder,
            AuthQuestionnaireBuilder authQuestionnaireBuilder,
            AuthThesisBuilder authThesisBuilder)
        {
            AuthMasterBuilder = authMasterBuilder;
            AuthCompanyBuilder = authCompanyBuilder;
            AuthSubscriptionBuilder = authuthSubscriptionBuilder;
            AuthInternshipBuilder = authInternshipBuilder;
            AuthMessageBuilder = authMessageBuilder;
            AuthProfileBuilder = authProfileBuilder;
            AuthQuestionnaireBuilder = authQuestionnaireBuilder;
            AuthThesisBuilder = authThesisBuilder;
        }
    }
}
