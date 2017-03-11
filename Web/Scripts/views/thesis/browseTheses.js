require(['jquery', 'modules/searchModule', 'semantic'], function ($, SearchModule) {
    // DOM ready
    $(function () {
        var searchModule = new SearchModule();

        // initialize autocomplete search for cities
        $('._ThesesNameAutocomplete')
            .search({
                apiSettings: {
                    url: searchModule.getThesisKeywordsUrl()
                },
                fields: {
                    results: 'Items',
                    title: 'Title',
                    description: 'Description'
                },
                error: {
                    noResults: "Nic nenalezeno"
                },
                minCharacters: 1
            });
    });
});