require(['jquery', 'semantic'], function ($) {
    // DOM ready
    $(function () {
        var conversationDiv = "_ConversationItem";
        $("." + conversationDiv).click(function () {
            var dataUrl = $(this).data("link");

            window.location.replace(dataUrl);
        });
    });
});