define(['modules/coreModule', 'jquery', 'promise'], function (Coremodule, $, ExtendedPromise) {

    function InternshipModule() {
    }

    var Promise = ExtendedPromise.Promise;

    InternshipModule.prototype.something = function () {
        return new Promise(function (resolve, reject) {
            var postUrl = "/api/company/GetMoreCompanies";
            $.ajax({
                url: postUrl,
                type: "POST",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    resolve(response);
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    reject(Error(xhr.responseText))
                }
            });
        });
    }

    return InternshipModule;
});