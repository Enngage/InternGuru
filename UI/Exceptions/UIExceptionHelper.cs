﻿
namespace UI.Exceptions
{
    public static class UiExceptionHelper
    {

        /// <summary>
        /// Gets message based on given exception type
        /// </summary>
        /// <param name="exceptionType"></param>
        public static string GetExceptionMessage(UiExceptionEnum exceptionType)
        {
            switch (exceptionType)
            {
                case UiExceptionEnum.SaveFailure:
                    return "Ukládání se nezdařilo";
                case UiExceptionEnum.NotAuthenticated:
                    return "Pro tuto akci musíte být přihlášení";
                case UiExceptionEnum.NoPermission:
                    return "Pro tuto akci nemáte oprávnění";
                case UiExceptionEnum.DeleteFailure:
                    return "Smazání objektu se nezdařilo";
                case UiExceptionEnum.Unknown:
                default:
                    return "Nastala neznámá chyba";
            }
        }
    }
}
