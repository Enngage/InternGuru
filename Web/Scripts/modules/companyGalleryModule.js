define(['modules/coreModule', 'jquery', 'promise'], function (Coremodule, $, ExtendedPromise) {

    function CompanyGalleryModule() {
    }

    var Promise = ExtendedPromise.Promise;

    CompanyGalleryModule.prototype.deleteImage = function (companyGuid, fileName) {
        return new Promise(function (resolve, reject) {
            var postUrl = "/api/companygallery/DeleteImage";
            var data = "{ 'CompanyGuid':'" + companyGuid + "', 'FileName':'" + fileName + "' }";

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
                    reject(Error(xhr.responseText))
                }
            });
        });
    }

    return CompanyGalleryModule;
});