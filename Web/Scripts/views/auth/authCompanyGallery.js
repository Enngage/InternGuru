require(['jquery', 'modules/companyGalleryModule', 'modules/fineUploaderModule', 'semantic'], function ($, CompanyGalleryModule, FineUploaderModule) {
    // DOM ready
    $(function () {
        var companyGalleryModule = new CompanyGalleryModule();
        var fineUploaderModule = new FineUploaderModule();

        var configElem = $("#_CompanyGalleryConfig");

        var companyGuid = configElem.data("guid");
        var endPointUrl = configElem.data("endpointurl");
        var maxFileSizeBytes = configElem.data("masfilesizebytes");
        var allowedExtensions = configElem.data("allowedextensions").split(",");
        var itemLimit = 10;

        // init drop uploader
        fineUploaderModule.InitUploader("fine-uploader-validation", "qq-template-validation", endPointUrl, maxFileSizeBytes, allowedExtensions, itemLimit);

        // initialize semantic ui cards
        $('._CompanyGalleryCards .image').dimmer({
            on: 'hover'
        });

        $('._CompanyGalleryCards ._DeleteGallery').click(function () {
            var fileName = $(this).data("filename");
            var cardColumn = HideCard($(this).closest('.column'));

            companyGalleryModule.deleteImage(companyGuid, fileName).then(cardColumn, function () {

                HideCard(cardColumn);

            }, function (error) {
                alert("Nastala chyba");
                console.error("Failed!", error);
            });
        });

        function HideCard(target) {
            $(target).remove();
        }
    });
});