define(['modules/coreModule', 'jquery', 'promise'], function (Coremodule, $, ExtendedPromise) {

    function SearchModule() {
    }

    var Promise = ExtendedPromise.Promise;

    SearchModule.prototype.getSearchCitiesUrl = function (pageNumber, search) {
        return '/api/search/GetSearchCities?q={query}';
    }

    return SearchModule;
});