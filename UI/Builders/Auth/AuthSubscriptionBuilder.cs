using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Service.Exceptions;
using UI.Builders.Services;
using UI.Builders.Shared.Models;
using UI.Exceptions;

namespace UI.Builders.Auth
{
    public class AuthSubscriptionBuilder : AuthMasterBuilder
    {
        #region Constructor

        public AuthSubscriptionBuilder(ISystemContext systemContext, IServicesLoader servicesLoader) : base(systemContext, servicesLoader)
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// Adds given cities to current user subscription
        /// </summary>
        /// <param name="cities">Cities</param>
        public async Task AddSubscribedCitiesToCurrentUserAsync(IList<string> cities)
        {
            try
            {
                if (!CurrentUser.IsAuthenticated)
                {
                    throw new ValidationException("Pro tuto akci musíš být přihlášen");
                }

                var applicationUser = await Services.IdentityService.GetAsync(CurrentUser.Id);

                if (applicationUser == null)
                {
                    throw new ValidationException($"Uživatel s ID {CurrentUser.Id} nebyl nalezen");
                }

                // set object properties
                if (cities == null)
                {
                    applicationUser.SubscribedCities = string.Empty;
                }
                else
                {
                    applicationUser.SubscribedCities = string.Join(",", cities);
                }

                await Services.IdentityService.UpdateAsync(applicationUser);
            }
            catch (ValidationException ex)
            {
                // log error
                Services.LogService.LogException(ex);

                // re-throw
                throw new UiException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                // log error
                Services.LogService.LogException(ex);

                // re-throw
                throw new UiException(UiExceptionEnum.SaveFailure, ex);
            }
        }      

        #endregion

    }
}
