using System.Collections.Generic;

namespace UI.Builders.Search.Autocomplete
{
    public class SearchAutocompleteResultModel
    {
        public int Count { get; set; }
        public IEnumerable<SearchAutocompleteItemModel> Items { get; set; }
    }
}
