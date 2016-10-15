require(['jquery', 'modules/internshipModule', 'modules/tableModule', 'modules/searchModule', 'semantic'], function ($, InternshipModule, TableModule, SearchModule) {
    // DOM ready
    $(function () {
        var internshipModule = new InternshipModule();
        var tableModule = new TableModule();
        var searchModule = new SearchModule();

        // initialize dropdown
        $('._BrowseDropdown').dropdown();

        // initialize table
        tableModule.initializeListingTable("_InternshipsListingTable");

        // initialize autocomplete search for cities
        $('._CitiesAutocomplete')
            .search({
                apiSettings: {
                    url: searchModule.getSearchCitiesUrl()
                },
                fields: {
                    results: 'Items',
                    title: 'Title',
                    description: 'Description'
                },
                error: {
                    noResults: "Město nenalezeno"
                },
                minCharacters: 1
            });

        // initialize autocomplete for internship queries
        $('._InternshipTitleAutocomplete')
            .search({
                apiSettings: {
                    url: searchModule.getInternshipKeywordsUrl()
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