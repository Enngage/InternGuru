define(['modules/coreModule', 'jquery', 'promise'], function (Coremodule, $, ExtendedPromise) {

    function ThesisMdoule() {
    }

    var Promise = ExtendedPromise.Promise;

    ThesisMdoule.prototype.deleteThesis = function (thesisID) {
        return new Promise(function (resolve, reject) {
            var postUrl = "/api/thesis/DeleteThesis";
            var data = "{ 'ThesisID':'" + thesisID + "' }";

            $.ajax({
                url: postUrl,
                data: data,
                type: "POST",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    resolve(response);
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    reject(Error(xhr.responseText));
                }
            });
        });
    }

    return ThesisMdoule;
});