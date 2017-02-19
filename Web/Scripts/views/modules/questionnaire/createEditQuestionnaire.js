require(['modules/coreModule', 'jquery', 'sortable', 'semantic'], function (CoreModule, $, Sortable) {
    $(function () {
        var coreModule = new CoreModule();

        // init sortable questions
        Sortable.create(_Questionnaire,
        {
            ghostClass: "w-questionnaire-ghost-class"
        });

        // type dropdown
        $("#_TypeDropdown").dropdown();

        // handle remove question clicks
        $("body").on("click", "._QuestionDeleteButton", function () {
            $(this).closest("._QuestionWrapper").remove();
        });

        // handle edit question clicks
        $("body").on("click", "._QuestionEdit", function () {
            // init the edit question's data
            var questionWrapper = $(this).closest("._QuestionWrapper");

            var questionText = questionWrapper.find("._QuestionText").val();
            var questionType = questionWrapper.find("._QuestionType").val();
            var questionGuid = questionWrapper.find("._FieldGuid").val();

            var question = [];
            question.QuestionText = questionText;
            question.QuestionType = questionType;
            question.Guid = questionGuid;

            // get all input values
            questionWrapper.find("._InputField").each(function (i) {
                var target = $(this).data("target");
                var value = $(this).val();
                question[target] = value;
            });

            openQuestionModal(question, questionWrapper);
        });

        // handle add question clicks
        $("#_AddQuestion").click(function () {
            openQuestionModal(null, null);
        });

        // set initial type question
        setDefaultState();

        // init on load
        initQuestionnaire();

        // load question types on change
        $("#_TypeDropdown").change(function () {
            var typeId = $(this).val();

            changeQuestionType(typeId, false);
        });

        function openQuestionModal(question, editQuestionElem) {
            $('#_QuestionnaireModal')
                .modal({
                    closable: true,
                    onDeny: function () {
                        return true;
                    },
                    onApprove: function () {

                        if (!question) {
                            question = [];
                        }

                        question.QuestionText = $("#_QuestionText").val();
                        question.QuestionType = $("#_TypeDropdown").val();

                        var result = false;

                        if (!question) {
                            // add question
                            alert(question.QuestionText);
                            result = addEditQuestion(question, null, false);
                        } else {
                            // edit question
                            result = addEditQuestion(question, editQuestionElem, false);
                        }

                        if (result) {
                            return true;
                        } else {
                            return false;
                        }
                    },
                    onShow: function () {
                        if (!question) {
                            setDefaultState();
                            setModalAddQuestionContext();
                        } else {
                            setModalEditQuestionContext();
                            $("#_QuestionText").val(question.QuestionText);
                            changeQuestionType(question.QuestionType, question);
                        }
                    }
                })
                .modal('show');
        }

        function setModalEditQuestionContext() {
            $("#_ModalHeaderLabel").text("Upravit otázku");
            $("#_ModalSaveLabel").text("Upravit");
        }

        function setModalAddQuestionContext() {
            $("#_ModalHeaderLabel").text("Vložit otázku");
            $("#_ModalSaveLabel").text("Vložit");
        }

        // initialize with questions from the initial state
        function initQuestionnaire() {
            // get JSON with question from hidden input
            var initialStateJson = coreModule.tryParseJson($("#_QuestionnaireInitialState").val());

            if (!initialStateJson) {
                console.log("Questionnaire has no initial state");
                return;
            }
            console.log(initialStateJson);

            $.each(initialStateJson, function (i, question) {
                // add all questions to 
                addEditQuestion(question, null, true);
            });
        }

        // sets default state of modal window with question selection
        function setDefaultState() {
            changeQuestionType("RadioButton", false);

            // clear question
            $("#_QuestionText").val("");
        }

        // changes question type in the modal window
        function changeQuestionType(typeId, question) {
            // set dropdown first
            $("#_TypeDropdown").dropdown("set selected", typeId);

            var insertTemplate = loadInsertQuestionTemplate(typeId, question);

            if (insertTemplate == null) {
                console.log("Invalid question type '" + typeId + "'");
                return;
            }

            $("#_QuestionInsertTemplatePlaceholder").html(insertTemplate);

            // refresh modal to center it on screen
            $('#_QuestionnaireModal').modal('refresh');

            // init types
            initTypes();
        }

        function addEditQuestion(question, editedQuestionElem, isInitialLoad) {
            if (!question) {
                alert("Nevalidní otázka");
            }

            if (!question.QuestionText) {
                alert("Zadejte text otázky");
                return false;
            }

            if (!question.QuestionType) {
                alert("Vyberte typ otázky");
                return false;
            }

            if (!editedQuestionElem) {
                // set guid for new questions only
                if (!question.Guid) {
                    question.Guid = coreModule.guid();
                }
            }

            // process data of active question
            if (!isInitialLoad) {

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
            }
                // process question data from the initial state (on page load)
            else {
                $.each(question.Data, function (i, data) {
                    // set question data
                    question[data.Name] = data.Value;
                });

            }

            var questionHtml = getDesignerTemplate(question);

            // append question if is new question
            if (!editedQuestionElem) {
                $("#_Questionnaire").append(questionHtml);
            } else {
                // edit question
                console.log(editedQuestionElem);
                editedQuestionElem.replaceWith(questionHtml);
            }

            return true;
        }

        function initTypes() {
            // radio buttons
            $('._RadioButtons').checkbox();
        }

        function getDesignerTemplate(question) {
            // get base template
            var baseTemplate = $("#_QuestionPreviewBaseTemplate");

            // set question's text
            baseTemplate.find("._QuestionText").text(question.QuestionText);

            // get page type's template
            var designerTemplate = $(loadDesignerTemplate(question.QuestionType));

            var correctAnswerElem = baseTemplate.find("._QuestionCorrectAnswer");

            // set input text data fields
            designerTemplate.find("._TextData").each(function (i) {
                // get data name
                var dataName = $(this).data("name");

                // get value
                var value = question[dataName];

                // hide field if empty
                var hideField = $(this).data("hideempty") === 1;
                if (hideField && !value) {
                    $(this).closest(".field").hide();
                    return;
                }

                // set text value
                $(this).text(value);

                // set correct answer
                correctAnswerElem.val(value);
            });

            // set radio data fields
            designerTemplate.find("._RadioButtonData").each(function (i) {
                // get data name
                var dataName = $(this).data("name");

                // check radio button
                var isChecked = question[dataName] === "true" || question[dataName] === true;

                // set questionGuid as the name of the radio inputs = radio sets are separate
                $(this).attr("name", question.Guid);

                if (isChecked) {
                    $(this).parent().addClass("checked");
                    $(this).attr("checked", "");

                    // get value of the correct answer
                    var sourceValue = $(this).data("valuesource");

                    if (!sourceValue) {
                        console.log("Cannot set correct answer for Radio button: '" + dataName + "'");
                        return;
                    }

                    // set question's correct answer
                    correctAnswerElem.val(question[sourceValue]);
                }
            });

            // set all hidden input fields
            designerTemplate.find("._InputField").each(function (i) {
                var target = $(this).data("target");

                // get value of target and set field
                $(this).val(question[target]);

                // set unique field name identifying the field
                var fieldPrefix = "Data";
                var uniqueFieldName = fieldPrefix + "_" + question.Guid + "_" + target;
                $(this).attr("name", uniqueFieldName);

            });

            // set field guid
            baseTemplate.find("._FieldGuid").val(question.Guid);

            // set generic question type
            var questionTypeElem = baseTemplate.find("._QuestionType");

            var questionTypeName = "QuestionType_" + question.Guid;
            questionTypeElem.attr("name", questionTypeName);
            questionTypeElem.val(question.QuestionType);

            // set generic question text
            var questionTextElem = baseTemplate.find("._QuestionText");

            var questionTextName = "QuestionText_" + question.Guid;
            questionTextElem.attr("name", questionTextName);
            questionTextElem.val(question.QuestionText);

            // insert preview template into base template
            baseTemplate.find("._QuestionTypeTemplate").html(designerTemplate.html());

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

        function loadInsertQuestionTemplate(typeId, question) {
            // get insert template of question type
            var templateClass = "_InsertTemplate";
            var templateHtml = $("#_" + typeId + " ." + templateClass).html();

            if (typeof templateHtml === "undefined" || templateHtml == null) {
                return null;
            }
            else {
                // set input values if present
                if (question) {
                    var template = $(templateHtml);

                    // set text data
                    template.find("._TextData").each(function (i) {
                        var dataName = $(this).data("name");
                        $(this).attr("value", question[dataName]);
                    });

                    // set radio data
                    template.find("._RadioButtonData").each(function (i) {
                        var dataName = $(this).data("name");
                        var value = question[dataName];

                        if (value === 'true' || value === true) {
                            // check checkbox
                            $(this).parent().addClass("checked");
                            $(this).attr("checked", "");
                            console.log(dataName + " | " + value);
                        }

                        $(this).val(question[dataName]);
                    });

                    return template.html();

                } else {
                    return templateHtml;
                }
            }
        }
    });
});