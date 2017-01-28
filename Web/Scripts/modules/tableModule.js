define(['modules/coreModule', 'jquery', 'promise'], function (Coremodule, $, ExtendedPromise) {

    function TableModule() {
    }

    var Promise = ExtendedPromise.Promise;

    TableModule.prototype.initializeListingTable = function initializeListingTable(tableClassName) {
        // redirect on row click
        $("." + tableClassName + "> tbody > tr").click(function () {
            var dataUrl = $(this).data("link");

            window.location.replace(dataUrl);
        });
    }

    return TableModule;
});