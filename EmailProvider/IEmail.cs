namespace EmailProvider
{
    public interface IEmail
    {
        string To { get; set; }
        string From { get; set; }
        string Subject { get; set; }
        string HtmlBody { get; set; }
        bool IsHtml { get; set; }
    }
}
