require(['jquery'], function ($) {
    // DOM ready
    $(function () {
        // scroll to the bottom of header
        var headerSelector = "#_UIHeaderWrapper";
        var moveButtonSelector = "._UIHeaderMove";
        $(headerSelector + " " + moveButtonSelector).click(function (e) {
            $('html, body').animate({
                scrollTop: $(headerSelector).next("div").offset().top // scroll to next div (first div available after header)
            }, 400);
        });
    });
});