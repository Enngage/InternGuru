require(['jquery','modules/tableModule', 'modules/searchModule', 'semantic'], function ($, TableModule, SearchModule) {
    // DOM ready
    $(function () {
        var tableModule = new TableModule();
        var searchModule = new SearchModule();

        // initialize table
        tableModule.initializeListingTable("_ThesesListingTable");

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