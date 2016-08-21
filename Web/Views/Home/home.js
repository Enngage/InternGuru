
require(['modules/CoreModule', 'jquery', 'semantic'], function (Core, $) {

    // DOM ready
    $(function () {
        // initialize embed video
        $('._Embed').embed();

        // hide/show most visited channels list
        $("._TrendingFilter").click(function (e) {
            // disable all filter options
            $("#_TrendingFilter span").removeClass("secondary");
            $(this).addClass("secondary");
            var showID = $(this).data("showid");

            // hide all trending wrappers
            $("._TrendingWrapper").addClass("w-body-hide");
            $("#" + showID).removeClass("w-body-hide");
        });  

    });
});