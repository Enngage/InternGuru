define(['modules/coreModule', 'jquery', 'promise'], function (Coremodule, $, ExtendedPromise) {

    function QuestionnaireModule() {
    }

    var Promise = ExtendedPromise.Promise;

    QuestionnaireModule.prototype.deleteQuestionnaire = function (questionnaireID) {
        return new Promise(function (resolve, reject) {
            var postUrl = "/api/questionnaire/DeleteQuestionnaire";
            var data = "{ 'QuestionnaireID':'" + questionnaireID + "' }";

            $.ajax({
                url: postUrl,
                data: data,
                type: "POST",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    resolve(response);
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    reject(Error(xhr.responseText));
                }
            });
        });
    }

    return QuestionnaireModule;
});