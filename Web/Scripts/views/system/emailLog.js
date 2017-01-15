require(['jquery', 'semantic'], function ($) {
    // DOM ready
    $(function () {
        $("._ShowLogModal").click(function (e) {
            var emailID = $(this).data("emailid");
            var modalID = "_Modal_" + emailID;

            $('#' + modalID).modal('show');
        });
    });
});