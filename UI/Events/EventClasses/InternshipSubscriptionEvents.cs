using System;
using System.Linq;
using System.Web;
using Core.Helpers;
using Entity;
using UI.Builders.Services;
using UI.Helpers;

namespace UI.Events.EventClasses
{
    public class InternshipSubscriptionEvents
    {

        private IServicesLoader Services { get; }
        private readonly System.Web.Mvc.UrlHelper _urlHelper = new System.Web.Mvc.UrlHelper(HttpContext.Current.Request.RequestContext);

        /// <summary>
        /// Url to the main site without actions/params
        /// </summary>
        private string SiteUrl => _urlHelper.RequestContext.HttpContext.Request.Url?.Scheme + "://" + _urlHelper.RequestContext.HttpContext.Request.Url?.Authority;


        public InternshipSubscriptionEvents(IServicesLoader services)
        {
            Services = services;
        }

        /// <summary>
        /// Sends notification to all subscribers if internship is in their city
        /// </summary>
        /// <param name="newInternship"></param>
        /// <param name="existingInternship"></param>
        public void ProcessNewInternshipBasedOnCitySubscriptions(Internship newInternship, Internship existingInternship)
        {
            if (newInternship == null)
            {
                var exception = new ArgumentNullException($"Cannot process '{nameof(ProcessNewInternshipBasedOnCitySubscriptions)}' event because internship is null");

                Services.LogService.LogException(exception);

                return;
            }

            // determin whether to send notification message
            bool sendNotification = false;

            if (newInternship.IsActive && existingInternship == null)
            {
                sendNotification = true;
            }
            else if(existingInternship != null)
            {
                // new internship is active, but it wasnt before = send notification
                if (!existingInternship.IsActive && newInternship.IsActive)
                {
                    sendNotification = true;
                }

                // city of the old internship has changed
                if (!existingInternship.City.Trim().Equals(newInternship.City.Trim()))
                {
                    sendNotification = true;
                }
            }

            if (!sendNotification)
            {
                return;
            }

            // get all users who subscribed to the city
            var subscribers = Services.IdentityService.GetAll()
                .Where(m => m.SubscribedCities.Contains(newInternship.City))
                .ToList();

            if (subscribers.Any())
            {
                var internshipCategory = Services.InternshipCategoryService.GetSingle(newInternship.InternshipCategoryID).FirstOrDefault();
                var internshipCategoryName = internshipCategory == null ? "Neznámá kategorie" : internshipCategory.Name;

                var company = Services.CompanyService.GetSingle(newInternship.CompanyID).FirstOrDefault();
                var companyName = company == null ? "Neznámá firma" : company.CompanyName;

                var currency = Services.CurrencyService.GetSingle(newInternship.CurrencyID).FirstOrDefault();
                var currencyName = currency == null ? "N/A" : currency.CurrencyName;
                var currencyShowOnLeft = currency?.ShowSignOnLeft ?? true;

                var amountType = Services.InternshipAmountTypeService.GetSingle(newInternship.AmountTypeID).FirstOrDefault();
                var amountTypeName = amountType == null ? "N/A" : amountType.AmountTypeName;

                foreach (var subscriber in subscribers)
                {
                    var subject = $"Nová stáž v městě '{newInternship.City}'";
                    var title = subject;
                    var message = $"Stáž <strong>'{newInternship.Title}'</strong> u firmy '<strong>{companyName}</strong>' v měste <strong>'{newInternship.City}'</strong> byla právě přidána." +
                                  $"<p><strong>Kategorie:</strong> {internshipCategoryName}</p>" +
                                  $"<p><strong>Odměna/mzda:</strong> {TextHelper.DisplayCurrencyValueStatic(newInternship.Amount, currencyName, currencyShowOnLeft)} / {amountTypeName}</p>" +
                                  $"<p><strong>Přidáno: </strong> {DateHelper.FormatDateAndTime(newInternship.Created)}</p>" +
                                  $"<p><strong>Nástup od: </strong> {DateHelper.FormatDate(newInternship.StartDate)}</p>" +
                                  $"<p>Více informací spolu s popisem najdeš na našem webu po kliknutí odkazí níže. Podívej se a třeba Tě stáž zaujme!</p>" +
                                  $"<p style=\"font-size: 11px\">PS: Tento e-mail je generován na základě Tvého nastavení odběru stáží a kdykoliv se můžeš odhlásit.</p>";
                    var emailHtml = Services.EmailTemplateService.GetBasicTemplate(subscriber.Email, title, message, message, GetInternshipUrl(newInternship.ID, newInternship.CodeName), "Zobrazit stáž");

                    try
                    {
                        // send e-mail
                        this.Services.EmailService.SendEmail(subscriber.Email, subject, emailHtml);
                    }
                    catch (Exception ex)
                    {
                        // e-mail could not be send because the template was not found
                        this.Services.EmailService.LogFailedEmail(subscriber.Email, subject, message, ex.Message);

                        throw;
                    }
                }
            }
        }

        /// <summary>
        /// Gets url to internship
        /// </summary>
        /// <param name="internshipID">InternshipID</param>
        /// <param name="internshipCodeName">IntersnhipCodeName</param>
        /// <returns>URL of the internship</returns>
        private string GetInternshipUrl(int internshipID, string internshipCodeName)
        {
            return SiteUrl + "/" + _urlHelper.Action("Index", "Internship", new { id = internshipID, codeName = internshipCodeName });
        }
    }
}
