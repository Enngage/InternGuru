using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using UI.Base;
using UI.Builders.Email.Models;
using UI.Builders.Email.Views;
using UI.Builders.Services;
using UI.Builders.Shared.Models;

namespace UI.Builders.Email
{
    public  class EmailBuilder : BaseBuilder
    {

        #region Constructor

        public EmailBuilder(ISystemContext systemContext, IServicesLoader servicesLoader) : base(systemContext, servicesLoader) { }

        #endregion

        #region Actions

        public async Task<EmailPreviewView> BuildEmailPreviewAsync(Guid emailGuid)
        {
            var email = await Services.EmailService.GetAll()
                .Where(m => m.Guid == emailGuid)
                .Select(m => new EmailPreviewModel()
                {
                    Subject = m.Subject,
                    EmailBody = m.HtmlBody
                })
                .FirstOrDefaultAsync();

            if (email == null)
            {
                return null;
            }

            return new EmailPreviewView()
            {
                Email = email
            };
        }

        #endregion
    }
}
