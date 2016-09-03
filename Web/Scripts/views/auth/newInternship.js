require(['jquery', 'datepicker', 'ckeditor', 'semantic' ], function ($, Datepicker) {
    // DOM ready
    $(function () {
        // initialization
        $('._DropdownSearch, ._CurrencyDropdown, ._DurationDropdown').dropdown();
        InitializeDatePicker();
        FormValidation();
        SetInitialState();
        InitializeCkEditor();

        // checkbox events
        $('#_IsPaidCheckbox').checkbox({
            onChecked: function () {
                // show field
                ShowAmountField();
            },
            onUnchecked: function () {
                // hide field
                HideAmountField();
            }
        });

        function InitializeCkEditor() {
            CKEDITOR.replace('_Description', {
                customConfig: '/scripts/addons/ckeditor/simpleEditorConfig.js'
            });
        }

        function ShowAmountField() {
            var amountFieldID = "_InternshipAmountField";
            $("#" + amountFieldID).removeClass("w-body-hide");
        }

        function HideAmountField() {
            var amountFieldID = "_InternshipAmountField";
            $("#" + amountFieldID).addClass("w-body-hide");
        }

        function InitializeDatePicker() {
            $('._Datepicker').datepicker();
        }

        function SetInitialState() {
            var paidChecked = $('#_IsPaidCheckbox').checkbox("is checked");
            if (paidChecked) {
                ShowAmountField();
            }
            else {
                HideAmountField();
            }
        }

        function FormValidation() {
            $('#_InternshipForm').form({
                fields: {
                    Title: {
                        identifier: 'Title',
                        rules: [
                          {
                              type: 'empty',
                              prompt: 'Vyplň název pozice'
                          }
                        ]
                    },
                    Description: {
                        identifier: 'Description',
                        rules: [
                          {
                              type: 'empty',
                              prompt: 'Vyplň popis pozice'
                          }
                        ]
                    },
                    City: {
                        identifier: 'City',
                        rules: [
                          {
                              type: 'empty',
                              prompt: 'Zadej město'
                          }
                        ]
                    },
                    State: {
                        identifier: 'State',
                        rules: [
                          {
                              type: 'empty',
                              prompt: 'Zadej stát'
                          }
                        ]
                    },
                    StartDate: {
                        identifier: 'StartDate',
                        rules: [
                          {
                              type: 'empty',
                              prompt: 'Zadej požadovaný začátek stáže'
                          }
                        ]
                    },
                    Duration: {
                        identifier: 'Duration',
                        rules: [
                          {
                              type: 'empty',
                              prompt: 'Zadej délku stáže'
                          },
                          {
                              type: 'integer[1..100000]',
                              prompt: 'Zadej délku stáže číslem'
                          }
                        ]
                    },
                }
            });
        }

    });
});