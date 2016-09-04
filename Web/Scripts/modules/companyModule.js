define(['modules/coreModule', 'jquery', 'promise'], function (Coremodule, $, ExtendedPromise) {

    function CompanyModule() {
    }

    var Promise = ExtendedPromise.Promise;

    CompanyModule.prototype.getMoreCompanies = function () {
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

    return CompanyModule;
});