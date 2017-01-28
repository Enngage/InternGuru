require(['jquery', 'semantic'], function ($) {
    // DOM ready
    $(function () {
        $("._ShowLogModal").click(function (e) {
            var logID = $(this).data("logid");
            var modalID = "_Modal_" + logID;

            $('#' + modalID).modal('show');
        });
    });
});