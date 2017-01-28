require(['jquery', 'modules/tableModule'], function ($, TableModule) {
    // DOM ready
    $(function () {
        var tableModule = new TableModule();

        // initialize table
        tableModule.initializeListingTable("_InternshipsListingTable");
    });
});