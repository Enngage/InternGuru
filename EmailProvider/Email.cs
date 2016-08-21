namespace EmailProvider
{
    public class Email : IEmail
    {
        public string To { get; set; }
        public string From { get; set; }
        public string Subject { get; set; }
        public string HtmlBody { get; set; }
        public bool IsHtml { get; set; }
    }
}
