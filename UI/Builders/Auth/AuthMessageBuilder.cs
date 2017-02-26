using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entity;
using Entity.Base;
using PagedList;
using PagedList.EntityFramework;
using Service.Exceptions;
using UI.Builders.Auth.Forms;
using UI.Builders.Auth.Models;
using UI.Builders.Auth.Views;
using UI.Builders.Services;
using UI.Builders.Shared.Models;
using UI.Exceptions;

namespace UI.Builders.Auth
{
    public class AuthMessageBuilder : AuthMasterBuilder
    {
        #region Constructor

        public AuthMessageBuilder(ISystemContext systemContext, IServicesLoader servicesLoader) : base(systemContext, servicesLoader)
        {
        }

        #endregion

        #region Actions

        public async Task<AuthConversationsView> BuildConversationsViewAsync(int? page)
        {
            var authMaster = await GetAuthMasterModelAsync();

            if (authMaster == null)
            {
                return null;
            }

            return new AuthConversationsView()
            {
                AuthMaster = authMaster,
                ConversationsPaged = await GetConversationsAsync(page)
            };
        }

        public async Task<AuthConversationView> BuildConversationViewAsync(string otherUserId, int? page, AuthMessageForm messageForm = null)
        {

            var authMaster = await GetAuthMasterModelAsync();

            if (authMaster == null)
            {
                return null;
            }

            var defaultMessageForm = new AuthMessageForm()
            {
                RecipientApplicationUserId = otherUserId
            };

            // don't show anything if recipient == current user. It should always be the other user
            if (otherUserId.Equals(CurrentUser.Id, StringComparison.OrdinalIgnoreCase))
            {
                // to do maybe
            }

            // get other user and check if he exists
            var otherUser = await GetMessageUserAsync(otherUserId);

            if (otherUser == null)
            {
                return null;
            }

            // get messages 
            var messages = await GetConversationMessagesAsync(otherUserId, page);

            // mark all received messaged for current user as read
            await Services.MessageService.MarkMessagesAsRead(CurrentUser.Id, otherUserId);

            return new AuthConversationView()
            {
                AuthMaster = authMaster,
                Messages = messages,
                ConversationUser = otherUser,
                Me = new AuthMessageUserModel()
                {
                    FirstName = CurrentUser.FirstName,
                    LastName = CurrentUser.LastName,
                    UserID = CurrentUser.Id,
                    UserName = CurrentUser.UserName,
                    Nickname = CurrentUser.Nickname,
                },
                MessageForm = messageForm ?? defaultMessageForm
            };
        }

        #endregion

        #region Methods

        public async Task<int> CreateMessage(AuthMessageForm form)
        {
            try
            {
                if (!CurrentUser.IsAuthenticated)
                {
                    // only authenticated users can send messages
                    throw new ValidationException("Pro odeslání zprávy se musíš prřihlásit");
                }

                var message = new Message()
                {
                    SenderApplicationUserId = CurrentUser.Id,
                    RecipientApplicationUserId = form.RecipientApplicationUserId,
                    MessageText = form.Message,
                    Subject = null, // no subject needed
                    IsRead = false,
                };

                var result = await Services.MessageService.InsertAsync(message);

                return result.ObjectID;
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

        #region Helper methods

        /// <summary>
        /// Gets conversations for current user
        /// </summary>
        /// <param name="otherUserId">ID of the other user (NEVER current user)</param>
        /// <param name="page">Page number</param>
        /// <returns>Collection of messages of current user with other user</returns>
        private async Task<IPagedList<AuthMessageModel>> GetConversationMessagesAsync(string otherUserId, int? page)
        {
            var pageSize = 10;
            var pageNumber = (page ?? 1);

            var messagesQuery = Services.MessageService.GetAll()
                .Where(m =>
                    (m.RecipientApplicationUserId == CurrentUser.Id || m.SenderApplicationUserId == CurrentUser.Id)
                    &&
                    (m.RecipientApplicationUserId == otherUserId || m.SenderApplicationUserId == otherUserId))
                .OrderByDescending(m => m.Created)
                .Select(m => new AuthMessageModel()
                {
                    ID = m.ID,
                    MessageCreated = m.Created,
                    SenderApllicationUserId = m.SenderApplicationUserId,
                    SenderApplicationUserName = m.SenderApplicationUser.UserName,
                    RecipientApplicationUserId = m.RecipientApplicationUserId,
                    RecipientApplicationUserName = m.RecipientApplicationUser.UserName,
                    MessageText = m.MessageText,
                    CurrentUserId = CurrentUser.Id,
                    IsRead = m.IsRead,
                    SenderFirstName = m.SenderApplicationUser.FirstName,
                    SenderNickname = m.SenderApplicationUser.Nickname,
                    RecipientNickname = m.RecipientApplicationUser.Nickname,
                    SenderLastName = m.SenderApplicationUser.LastName,
                    RecipientFirstName = m.RecipientApplicationUser.FirstName,
                    RecipientLastName = m.RecipientApplicationUser.LastName,
                    Subject = m.Subject,
                });

            var cacheSetup = Services.CacheService.GetSetup<AuthMessageModel>(GetSource());
            cacheSetup.Dependencies = new List<string>()
            {
                EntityKeys.KeyUpdateAny<Message>(),
                EntityKeys.KeyDeleteAny<Message>(),
                EntityKeys.KeyCreateAny<Message>(),
            };
            cacheSetup.ObjectStringID = CurrentUser.Id + "_" + otherUserId;
            cacheSetup.PageNumber = pageNumber;
            cacheSetup.PageSize = pageSize;

            return await Services.CacheService.GetOrSet(async () => await messagesQuery.ToPagedListAsync(pageNumber, pageSize), cacheSetup);
        }

        #endregion

    }
}
