require(['jquery', 'modules/tableModule', 'semantic'], function ($, TableModule) {
    // DOM ready
    $(function () {
        var tableModule = new TableModule();

        $('.menu .item').tab();

        tableModule.initializeListingTable("_ConversationListingTable");
    });
});