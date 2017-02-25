using System.Web.UI.WebControls.WebParts;
using UI.Builders.Master.Enums;

namespace UI.Builders.Master.Models
{
    public class Layout
    {
        private const string BodyView = "~/Views/Shared/Body/BodyWrapper.cshtml";

        public string Path
        {
            get
            {
                if (!string.IsNullOrEmpty(ParentView))
                {
                    return ParentView;
                }
                return BodyView;
            }
        }

        public LayoutTypeEnum Type { get; set; }
        public string ParentView { get; set; }
    }
}
