
// ReSharper disable InconsistentNaming
namespace Web.Lib.RouteLocalization
{
    public sealed class RouteCultures
    {

        private readonly string _culture;

        public static readonly string en = "en";
        public static readonly string cs = "cs";

        private RouteCultures(string culture)
        {
            _culture = culture;
        }

        public override string ToString()
        {
            return _culture;
        }

    }
}