define(['modules/coreModule', 'jquery', 'promise'], function (Coremodule, $, ExtendedPromise) {

    function UserModule() {
    }

    var Promise = ExtendedPromise.Promise;

    UserModule.prototype.getAvatarOfCurrentUser = function () {
        return new Promise(function (resolve, reject) {
            var postUrl = "/api/user/GetAvatarOfCurrentUser";

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

    return UserModule;
});