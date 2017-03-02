define(['modules/coreModule', 'jquery', 'promise', 'fineuploader'], function (Coremodule, $, ExtendedPromise, FineUploader) {

    function FineUploaderModule() {
    }

    var fineUploader = new FineUploader();
    
    FineUploaderModule.prototype.InitUploader = function (uploaderElemId, templateId, endPointUrl, maxFileSize, allowedExtensions, itemLimit) {
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
            }
        });
    }

    return FineUploaderModule;
});