define(['modules/coreModule', 'jquery', 'promise'], function (CoreModule, $, ExtendedPromise) {

    function BlogCategoryModule() {
    }

    var Promise = ExtendedPromise.Promise;

    BlogCategoryModule.prototype.DeleteCategory = function (categoryID) {
        return new Promise(function (resolve, reject) {
            var postUrl = "/api/BlogCategory/DeleteCategory/";
            var data = "{ 'CategoryID':'" + categoryID + "'}";

            $.ajax({
                url: postUrl,
                data: data,
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

    return BlogCategoryModule;
});