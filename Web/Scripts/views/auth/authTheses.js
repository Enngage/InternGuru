require(['jquery', 'modules/thesisModule', 'modules/modalModule', 'semantic'], function ($, ThesisModule, ModalModule) {
    $(function () {
        var thesisModule = new ThesisModule();
        var modalModule = new ModalModule();

        modalModule.initDeleteModal("._DeleteModal", "._DeleteThesisButton", thesisModule.deleteThesis, "tr");
    });
});