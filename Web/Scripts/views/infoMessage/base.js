require(['jquery', 'modules/infoMessageModule'], function ($, InfoMessageModule) {
    // DOM ready
    $(function () {
        var infoMessageModule = new InfoMessageModule();
        // close closable messages
        var closableMessageClass = "._InfoMessageClosable";
        $(closableMessageClass + " .close").on('click',
            function () {
                $(this).closest('.message').transition('fade');
                var messageID = $(this).data("messageid");
                ProcessClosableMessage(messageID);
            });

    function ProcessClosableMessage(messageID) {
        infoMessageModule.processClosableMessage(messageID).then(function () {
            console.log("Message " + messageID + " closed");
        }, function (error) {
            console.error("Failed!", error);
        });
    }

    });
}); 