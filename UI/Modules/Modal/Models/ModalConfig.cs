using System.Collections.Generic;

namespace UI.Modules.Modal.Models
{
    public class ModalConfig
    {
        public string ElementClass { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public IList<ModalButton> Buttons { get; set; }
        public string HeaderIconClass { get; set; }
        public bool CenterText { get; set; } = true;
    }
}
