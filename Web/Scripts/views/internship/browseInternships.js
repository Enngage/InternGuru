require(['jquery', 'modules/internshipModule', 'modules/tableModule', 'modules/searchModule', 'semantic'], function ($, InternshipModule, TableModule, SearchModule) {
    // DOM ready
    $(function () {
        var tableModule = new TableModule();
        var searchModule = new SearchModule();

        // show filter on click
        $('._ShowFilterButton').click(function (e) {
            e.preventDefault();

            var filterWrapperId = "_FilterWrapper";
            $("#" + filterWrapperId).toggleClass("w-body-hide");
            $(this).toggleClass("blue");
        });

        // initialize dropdown
        $('._BrowseDropdown, ._LengthDropdown, ._PaidDropdown').dropdown();

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