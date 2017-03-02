define(['modules/coreModule', 'jquery', 'promise', 'fineuploader'], function (Coremodule, $, ExtendedPromise, FineUploader) {

    function FineUploaderModule() {
    }

    var fineUploader = new FineUploader();

    FineUploaderModule.prototype.InitUploader = function (uploaderElemId, templateId, endPointUrl, maxFileSize, allowedExtensions, itemLimit, refreshImagesElementClass) {
        // prepare extensions => remove "." and trim them
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
                    waitingPath: '/scripts/addons/fineuploader/placeholders/waiting-generic.png',
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
                        var date = new Date();
                        // refresh all images in certain wrapper
                        $("." + refreshImagesElementClass + " img").each(function () {
                            var src = this.src;
                            $(this).attr("src", src + "?" + date.getTime());
                        });
                    }
                    // reset uploader if only 1 item was allowed
                    if (itemLimit === 1) {
                        $("#" + uploaderElemId).fineUploader('reset');
                    }
                }
            }
        });
    }

    return FineUploaderModule;
});