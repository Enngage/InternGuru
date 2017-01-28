namespace EmailProvider
{
    public interface IEmailMessage
    {
        string To { get; set; }
        string From { get; set; }
        string Subject { get; set; }
        string HtmlBody { get; set; }
        bool IsHtml { get; set; }
    }
}
