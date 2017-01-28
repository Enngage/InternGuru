require(['jquery', 'modules/infoMessageModule'], function ($, InfoMessageModule) {
    // DOM ready
    $(function () {
        var infoMessageModule = new InfoMessageModule();

        // closable messages
        var closableMessageClass = "._InfoMessageClosable";
        var uiMessageClass = ".message";

        $(closableMessageClass + " .close").on('click',
            function () {
                // fade it
                $(this).closest(uiMessageClass).transition('fade');

                // make sure that closable message is not show again if necessary
                var messageID = $(this).closest(uiMessageClass).data("messageid");
                var ClosedForDaysCount = $(this).closest(uiMessageClass).data("closedfordayscount");
                var rememberClosed = $(this).closest(uiMessageClass).data("rememberclosed");

                // process request
                processClosableMessage(messageID, ClosedForDaysCount, rememberClosed);
            });

        function processClosableMessage(messageID, ClosedForDaysCount, rememberClosed) {
            infoMessageModule.processClosableMessage(messageID, ClosedForDaysCount, rememberClosed).then(function () {
            console.log("Message " + messageID + " closed");
        }, function (error) {
            console.error("Failed!", error);
        });
    }

    });
}); 