define(['modules/coreModule', 'jquery', 'promise', 'fineuploader', 'modules/userModule'], function (Coremodule, $, ExtendedPromise, FineUploader, UserModule) {

    function FineUploaderModule() {
    }

    var fineUploader = new FineUploader();
    var userModule = new UserModule();

    FineUploaderModule.prototype.InitUploader = function (uploaderElemId, templateId, endPointUrl, maxFileSize, allowedExtensions, itemLimit, refreshImagesElementClass) {
        // prepare extensions => remove "." and trim them
        var totalItemLimit = itemLimit;
        allowedExtensions.forEach(function (extension, i) {
            var fixedExtension = extension.replace(".", "");
            fixedExtension = fixedExtension.trim();

            allowedExtensions[i] = fixedExtension;
        });

        $("#" + uploaderElemId).fineUploader({
            template: templateId,
            request: {
                endpoint: endPointUrl // POST action that saves files
            },
            thumbnails: {
                placeholders: {
                    //waitingPath: '/scripts/addons/fineuploader/placeholders/waiting-generic.png',
                    notAvailablePath: '/scripts/addons/fineuploader/placeholders/not_available-generic.png'
                }
            },
            validation: {
                allowedExtensions: allowedExtensions, // array
                itemLimit: itemLimit,
                sizeLimit: maxFileSize // bytes
            },
            callbacks: {
                onSubmit: function () {
                },
                onAllComplete: function (id) {
                    if (refreshImagesElementClass) {
                        // update avatar images of current user
                        if (refreshImagesElementClass === "_AvatarOfCurrentUser") {
                            var date = new Date();

                            // get path of user's avatar
                            userModule.getAvatarOfCurrentUser().then(function (userAvatar) {
                                // refresh avatars
                                $("." + refreshImagesElementClass + " img").each(function () {
                                    $(this).attr("src", userAvatar + "?" + date.getTime());
                                });
                            }, function (error) {
                                alert("Nastala chyba");
                                console.error("Failed!", error);
                            });
                        }
                    }
                    // increase max item limit
                    totalItemLimit += itemLimit;
                    $("#" + uploaderElemId).fineUploader('setItemLimit', totalItemLimit);
                }
            }
        });
    }

    return FineUploaderModule;
});