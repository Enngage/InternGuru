define(['modules/coreModule', 'jquery', 'promise'], function (Coremodule, $, ExtendedPromise) {

    function AdminModule() {
    }

    var Promise = ExtendedPromise.Promise;

    AdminModule.prototype.clearCache = function () {
        return new Promise(function (resolve, reject) {
            var postUrl = "/api/admin/clearCache/";
            $.ajax({
                url: postUrl,
                type: "POST",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    resolve(true);
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    reject(Error(xhr.responseText))
                }
            });
        });
    }

    AdminModule.prototype.generateChannels = function () {
        return new Promise(function (resolve, reject) {
            var postUrl = "/api/admin/generateChannels/";
            $.ajax({
                url: postUrl,
                type: "POST",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    resolve(true);
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    reject(Error(xhr.responseText))
                }
            });
        });
    }

    return AdminModule;
});