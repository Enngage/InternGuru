using System.Collections.Generic;
namespace Core.Misc
{
    public static class Cities
    {
        /// <summary>
        /// Gets list of default cities
        /// </summary>
        /// <returns></returns>
        public static List<string> GetDefaultCities()
        {
            return new List<string>()
            {
                "Praha", 
                "Brno",
                "Plzeň",
                "Zlín",
                "Pardubice",
                "Ostrava",
                "Opava",
                "Hradec Králové",
                "České Budějovice",
                "Olomouc",
                "Liberec",
                "Ústí nad Labem"
            }; 
        }
    }
}
