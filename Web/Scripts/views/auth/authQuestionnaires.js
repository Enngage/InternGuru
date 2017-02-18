require(['jquery', 'modules/questionnaireModule', 'modules/modalModule', 'semantic'], function ($, QuestionnaireModule, ModalModule) {
    $(function () {
        var questionnaireModule = new QuestionnaireModule();
        var modalModule = new ModalModule();

        modalModule.initDeleteModal("._DeleteModal", "._DeleteQuestionnaireButton", questionnaireModule.deleteQuestionnaire, "tr");
    });
});