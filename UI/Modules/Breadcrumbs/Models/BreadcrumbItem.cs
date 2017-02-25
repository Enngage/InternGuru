namespace UI.Modules.Breadcrumbs.Models
{
    public class BreadcrumbItem
    {
        public BreadcrumbItem(string title)
        {
            Title = title;
        }

        public BreadcrumbItem(string title, string url)
        {
            Url = url;
            Title = title;
        }

        public string Url { get; set; }
        public string Title { get; set; }
        public bool HasUrl => !string.IsNullOrEmpty(Url);
    }
}
