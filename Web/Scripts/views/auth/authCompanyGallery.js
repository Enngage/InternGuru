require(['jquery', 'dropzone', 'modules/companyGalleryModule', 'semantic'], function ($, DropZone, CompanyGalleryModule) {
    // DOM ready
    $(function () {
        var companyGalleryModule = new CompanyGalleryModule();
        var companyGuid = $('#_CompanyGalleryGuid').data("guid");

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