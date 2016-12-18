
namespace UI.Exceptions
{
    public static class UIExceptionHelper
    {

        /// <summary>
        /// Gets message based on given exception type
        /// </summary>
        /// <param name="exceptionType"></param>
        public static string GetExceptionMessage(UIExceptionEnum exceptionType)
        {
            switch (exceptionType)
            {
                case UIExceptionEnum.SaveFailure:
                    return "Ukládání se nezdařilo";
                case UIExceptionEnum.NotAuthenticated:
                    return "Pro tuto akci musíte být přihlášení";
                case UIExceptionEnum.NoPermission:
                    return "Pro tuto akci nemáte dostatek oprávnění";
                case UIExceptionEnum.Unknown:
                default:
                    return "Nastala neznáma chyba";
            }
        }
    }
}
