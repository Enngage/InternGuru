using System;
using System.Data.Entity;
using System.Threading.Tasks;
using Service.Exceptions;
using UI.Builders.Auth.Forms;
using UI.Builders.Auth.Views;
using UI.Builders.Services;
using UI.Builders.Shared.Models;
using UI.Exceptions;

namespace UI.Builders.Auth
{
    public class AuthProfileBuilder : AuthMasterBuilder
    {
        #region Constructor

        public AuthProfileBuilder(ISystemContext systemContext, IServicesLoader servicesLoader) : base(systemContext, servicesLoader)
        {
        }

        #endregion

        #region Actions

        public async Task<AuthAvatarView> BuildAvatarViewAsync()
        {
            var authMaster = await GetAuthMasterModelAsync();

            if (authMaster == null)
            {
                return null;
            }

            var avatarForm = new AuthAvatarUploadForm();

            return new AuthAvatarView()
            {
                AuthMaster = authMaster,
                AvatarForm = avatarForm
            };
        }

        public async Task<AuthEditProfileView> BuildEditProfileViewAsync()
        {
            var authMaster = await GetAuthMasterModelAsync();

            if (authMaster == null)
            {
                return null;
            }

            var currentApplicationUser = await Services.IdentityService.GetSingle(CurrentUser.Id).FirstOrDefaultAsync();

            if (currentApplicationUser == null)
            {
                throw new UiException($"Uživatel s ID {CurrentUser.Id} nebyl nalezen");
            }

            var form = new AuthEditProfileForm()
            {
                FirstName = currentApplicationUser.FirstName,
                LastName = currentApplicationUser.LastName,
                Nickname = currentApplicationUser.Nickname
            };

            return new AuthEditProfileView()
            {
                AuthMaster = authMaster,
                ProfileForm = form
            };
        }

        public async Task<AuthEditProfileView> BuildEditProfileViewAsync(AuthEditProfileForm form)
        {
            var authMaster = await GetAuthMasterModelAsync();

            if (authMaster == null)
            {
                return null;
            }

            return new AuthEditProfileView()
            {
                AuthMaster = authMaster,
                ProfileForm = form
            };
        }

        #endregion

        #region Methods

        /// <summary>
        /// Edits profile
        /// </summary>
        /// <param name="form">form</param>
        public async Task EditProfile(AuthEditProfileForm form)
        {
            try
            {
                if (!CurrentUser.IsAuthenticated)
                {
                    throw new ValidationException("Pro změnu údajů se musíš přihlásit");
                }

                var applicationUser = await Services.IdentityService.GetAsync(CurrentUser.Id);

                if (applicationUser == null)
                {
                    throw new ValidationException($"Uživatel s ID {CurrentUser.Id} nebyl nalezen");
                }

                // set object properties
                applicationUser.FirstName = form.FirstName;
                applicationUser.LastName = form.LastName;
                applicationUser.Nickname = form.Nickname;

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

        /// <summary>
        /// Sets type of current user
        /// </summary>
        /// <param name="isCandidate">Indicates if user should be set as candidate</param>
        ///  <param name="isCompany">Indicates if user should be set as company</param>
        /// <returns></returns>
        public async Task SetUserTypeAsync(bool isCompany, bool isCandidate)
        {
            try
            {
                if (!CurrentUser.IsAuthenticated)
                {
                    // only authenticated users can send messages
                    throw new ValidationException("Pro vytvoření stáže se musíš přihlásit");
                }

                var applicationUser = await Services.IdentityService.GetAsync(CurrentUser.Id);

                if (applicationUser == null)
                {
                    throw new ValidationException($"Uživatel s ID {CurrentUser.Id} nebyl nalezen");
                }


                applicationUser.IsCandidate = isCandidate;
                applicationUser.IsCompany = isCompany;

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
