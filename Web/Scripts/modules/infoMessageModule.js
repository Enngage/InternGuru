define(['modules/coreModule', 'jquery', 'promise'], function (Coremodule, $, ExtendedPromise) {

    function InfoMessageModule() {
    }

    var Promise = ExtendedPromise.Promise;

    InfoMessageModule.prototype.processClosableMessage = function (messageID) {
        return new Promise(function (resolve, reject) {
            var postUrl = "/api/infoMessage/processClosableMessage";
            var data = "{ 'MessageID':'" + messageID + "'}";

            $.ajax({
                url: postUrl,
                data: data,
                type: "POST",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function () {
                    resolve(true);
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    reject(Error(xhr.responseText))
                }
            });
        });
    }

    return InfoMessageModule;
});