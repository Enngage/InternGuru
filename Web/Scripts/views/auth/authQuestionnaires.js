require(['jquery', 'modules/questionnaireModule', 'semantic'], function ($, QuestionnaireModule) {
    $(function () {
        var questionnaireModule = new QuestionnaireModule();
        var questionnaireID = 0;
        var questionnaireTrElem = null;

        // init modal
        $('._DeleteModal')
            .modal({
                closable: false,
                onDeny: function () {
                    return true;
                },
                onApprove: function () {
                    deleteQuestionnaire(questionnaireID);
                }
            });

        $('._DeleteQuestionnaireButton').click(function (e) {
            e.preventDefault();

            questionnaireID = $(this).data("id");
            questionnaireTrElem = $(this).closest("tr");
            $('._DeleteModal').modal('show');
        });

        function deleteQuestionnaire(questionnaireID) {

            if (!questionnaireID) {
                alert("Invalid questionnaire");
            }

            questionnaireModule.deleteQuestionnaire(questionnaireID).then(function () {
                // hide item
                $(questionnaireTrElem).remove();

            }, function (error) {
                alert("Nastala chyba");
                console.error("Failed!", error);
            });
        }
    });
});