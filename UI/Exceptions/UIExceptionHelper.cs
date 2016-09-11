﻿
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
                case UIExceptionEnum.Unknown:
                default:
                    return "Nastala neznáma chyba";
            }
        }
    }
}
