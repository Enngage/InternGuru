require(['modules/coreModule', 'jquery', 'semantic'], function (CoreModule, $) {
    $(function () {
        var coreModule = new CoreModule();
        // type dropdown
        $("#_TypeDropdown").dropdown();

        $("#_AddQuestion").click(function() {
            $('#_QuestionareModal')
                 .modal({
                     closable: true,
                     onDeny: function () {
                         return true;
                     },
                     onApprove: function () {
                         var result = addQuestion();
                         if (result) {
                             return true;
                         } else {
                             return false;
                         }
                     },
                     onShow: function() {
                         setDefaultState();
                     }
                 })
                .modal('show');
        });

        // set initial type question
        setDefaultState();

        // load question types on change
        $("#_TypeDropdown").change(function() {
            var typeId = $(this).val();

            changeQuestionType(typeId);
        });

        function setDefaultState() {
            changeQuestionType("RadioButton");

            // clear question
            $("#_QuestionText").val("");

            // set default dropdown set
            $("#_TypeDropdown").dropdown("set selected", "RadioButton");
        }

        function changeQuestionType(typeId) {
            var insertTemplate = loadInsertQuestionTemplate(typeId);

            if (insertTemplate == null) {
                console.log("Invalid question type '" + typeId + "'");
                alert("Neplatný výběr typu otázky");
                return;
            }

            $("#_QuestionInsertTemplatePlaceholder").html(insertTemplate);

            // refresh modal to center it on screen
            $('#_QuestionareModal').modal('refresh');

            // init types
            initTypes();
        }

        function addQuestion() {
            var questionText = $("#_QuestionText").val();

            if (!questionText) {
                alert("Zadejte text otázky");
                return false;
            }

            var questionType = $("#_TypeDropdown").val();

            if (!questionType) {
                alert("Vyberte typ otázky");
                return false;
            }

            var questionGuid = coreModule.guid();
            var question = {};

            question.guid = questionGuid;
            question.questionText = questionText;
            question.questionType = questionType;

            // get question's text data (in input[text] )
            $("#_QuestionInsertTemplatePlaceholder ._TextData").each(function (i) {
                var dataName = $(this).data("name");
                var value = $(this).val();

                question[dataName] = value;
            });

            // get question's data in radio buttons
            $("#_QuestionInsertTemplatePlaceholder ._RadioButtonData").each(function (i) {
                var dataName = $(this).data("name");
                var isChecked = $(this).parent().checkbox("is checked");

                question[dataName] = isChecked;
            });

            var questionHtml = getDesignerTemplate(question);

            // append question to questionare
            $("#_Questionare").append(questionHtml);

            return true;
        }

        function initTypes() {
            // radio buttons
            $('._RadioButtons').checkbox();
        }

        function getDesignerTemplate(questionData) {
            // get base template
            var baseTemplate = $("#_QuestionPreviewBaseTemplate");

            // set question's text
            baseTemplate.find("._QuestionText").text(questionData.questionText);

            // get page type's template
            var previewTemplate = $(loadDesignerTemplate(questionData.questionType));

            // set input text data fields
            previewTemplate.find("._TextData").each(function (i) {
                // get data name
                var dataName = $(this).data("name");

                // get value
                var value = questionData[dataName];

                // hide field if empty
                var hideField = $(this).data("hideempty") === 1;
                if (hideField && !value) {
                    $(this).closest(".field").hide();
                    return;
                }

                // set text value
                $(this).text(value);
            });

            // set radio data fields
            previewTemplate.find("._RadioButtonData").each(function (i) {
                // get data name
                var dataName = $(this).data("name");

                // check radio button
                var isChecked = questionData[dataName] === true;
                if (isChecked) {
                    $(this).parent().addClass("checked");
                    $(this).attr("checked", "");
                }
            });

            // set field guid
            previewTemplate.find("._FieldGuid").val(questionData.guid);

            // set all hidden input fields
            previewTemplate.find("._InputField").each(function (i) {
                var target = $(this).data("target");

                // get value of target and set field
                $(this).val(questionData[target]);

                // set unique field name identifying the field
                var fieldPrefix = "Data";
                var uniqueFieldName = fieldPrefix + "_" + questionData.guid + "_" + target;
                $(this).attr("name", uniqueFieldName);

            });

            // set generic question type
            var questionTypeElem = previewTemplate.find("._QuestionType");

            var questionTypeName = "QuestionType_" + questionData.guid;
            questionTypeElem.attr("name", questionTypeName);
            questionTypeElem.val(questionData.questionType);

            // set generic question text
            var questionTextElem = previewTemplate.find("._QuestionText");

            var questionTextName = "QuestionText_" + questionData.guid;
            questionTextElem.attr("name", questionTextName);
            questionTextElem.val(questionData.questionText);

            // insert preview template into base template
            baseTemplate.find("._QuestionTypeTemplate").html(previewTemplate.html());

            return baseTemplate.html();
        }

        function loadBaseTemplate() {
            var templateId = "_QuestionPreviewBaseTemplate";
            var template = $("#_" + templateId).html();

            if (typeof template === "undefined" || template == null) {
                return null;
            }
            else {
                return template;
            }
        }

        function loadDesignerTemplate(typeId) {
            var templateClass = "_DesignerTemplate";
            var template = $("#_" + typeId + " ." + templateClass).html();

            if (typeof template === "undefined" || template == null) {
                return null;
            }
            else {
                return template;
            }
        }

        function loadInsertQuestionTemplate(typeId) {
            // get insert template of question type
            var templateClass = "_InsertTemplate";
            var template = $("#_" + typeId + " ." + templateClass).html();

            if (typeof template === "undefined" || template == null)
            {
                return null;
            }
            else
            {
                return template;
            }
        }
    });
});