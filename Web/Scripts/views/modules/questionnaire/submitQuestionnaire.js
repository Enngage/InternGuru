require(['modules/coreModule', 'jquery', 'semantic'], function (CoreModule, $) {
    $(function () {
        var coreModule = new CoreModule();

        initQuestionnaire();

        // initialize with questions from the initial state
        function initQuestionnaire() {
            // get JSON with question from hidden input
            var initialStateJson = coreModule.tryParseJson($("#_QuestionnaireInitialState").val());

            if (!initialStateJson) {
                console.log("No questionnaire loaded");
                return;
            }
            console.log(initialStateJson);

            $.each(initialStateJson, function (i, question) {
                // add all questions
                addQuestion(question);
            });

            initTypes();
        }

        function addQuestion(question) {
            if (!question) {
                console.log("Invalid question");
                return;
            }

            var baseTemplate = $(loadBaseTemplate());

            // set question's text
            baseTemplate.find("._QuestionText").text(question.QuestionText);

            // set question's Guid
            baseTemplate.find("._FieldGuid").val(question.Guid);

            // get question submit template
            var submitTemplate = $(loadSubmitTemplate(question.QuestionType));

            // set input names of all submit fields
            submitTemplate.find("._SubmitData").each(function (i) {
                var dataName = $(this).data("name");
                var fieldPrefix = "SubmitData";
                var uniqueFieldName = fieldPrefix + "_" + question.Guid + "_" + dataName;

                // set value
                var target = $(this).data("target");
                if (target) {
                    var dataValue = getDataValue(question.Data, target).Value;
                    $(this).attr("value", dataValue);
                }

                $(this).attr("name", uniqueFieldName);
            });

            //hide fields
            submitTemplate.find(".field").each(function (i) {
                var hideIfEmpty = parseInt($(this).data("hideempty"));

                if (hideIfEmpty === 1) {
                    var target = $(this).data("target");
                    var dataValue = getDataValue(question.Data, target).Value;

                    if (!dataValue) {
                        $(this).hide();
                    }
                }
            });

            // set all data fields
            submitTemplate.find("._DataField").each(function (i) {
                var dataName = $(this).data("name");
                var dataValue = getDataValue(question.Data, dataName).Value;
                $(this).text(dataValue);
                $(this).val(dataValue);
            });

            // set input text fields
            submitTemplate.find("._InputField").each(function (i) {
                $(this).attr("value", question.Answer);
            });

            // set radio button fields
            submitTemplate.find("._RadioButtonData").each(function (i) {
                var target = $(this).data("target");
                var questionText = getDataValue(question.Data, target).Value;
                var isChecked = question.Answer === questionText;

                if (isChecked) {
                    $(this).parent().addClass("checked");
                    $(this).attr("checked", "");
                }
            });

            // inject submit template into base template
            baseTemplate.find("._QuestionTypeTemplate").html(submitTemplate.html());

            // append question to DOM
            $("#_QuestionnaireFormWrapper").append(baseTemplate.html());

        }

        function getDataValue(data, dataName) {
            for (var i in data) {
                if (data[i].Name === dataName) {
                    return data[i]; 
                }
            }
        }

        function loadBaseTemplate() {
            var templateId = "QuestionSubmitBaseTemplate";
            var template = $("#_" + templateId).html();

            if (typeof template === "undefined" || template == null) {
                return null;
            }
            else {
                return template;
            }
        }


        function initTypes() {
            // radio buttons
            $('._RadioButtons').checkbox();
        }

        function loadSubmitTemplate(typeId) {
            var templateClass = "_SubmitTemplate";
            var template = $("#_" + typeId + " ." + templateClass).html();

            if (typeof template === "undefined" || template == null) {
                return null;
            }
            else {
                return template;
            }
        }
    });
});