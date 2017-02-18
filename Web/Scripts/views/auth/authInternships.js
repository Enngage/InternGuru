require(['jquery', 'modules/internshipModule', 'modules/modalModule', 'semantic'], function ($, InternshipModule, ModalModule) {
    $(function () {
        var internshipModule = new InternshipModule();
        var modalModule = new ModalModule();

        modalModule.initDeleteModal("._DeleteModal", "._DeleteInternshipButton", internshipModule.deleteInternship, "tr");
    });
});