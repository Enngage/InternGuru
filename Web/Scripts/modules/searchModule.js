define(['modules/coreModule', 'jquery', 'promise'], function (Coremodule, $, ExtendedPromise) {

    function SearchModule() {
    }

    var Promise = ExtendedPromise.Promise;

    SearchModule.prototype.getSearchCitiesUrl = function () {
        return '/api/search/GetSearchCities?q={query}';
    }

    SearchModule.prototype.getInternshipKeywordsUrl = function () {
        return '/api/search/GetInternshipKeywords?q={query}';
    }


    return SearchModule;
});