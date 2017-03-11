require(['jquery', 'modules/searchModule', 'semantic'], function ($, SearchModule) {
    // DOM ready
    $(function () {
        var searchModule = new SearchModule();

        // initialize dropdown
        $('._LengthDropdown,').dropdown();

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