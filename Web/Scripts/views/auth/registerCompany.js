require(['jquery', 'datepicker', 'ckeditor', 'semantic', ], function ($, Datepicker) {
    // DOM ready
    $(function () {
        // initialization
        $('._YearFoundedDropdown, ._CompanySizeDropdown, ._StateDropdown').dropdown();
        FormValidation();
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
            CKEDITOR.replace('_LongDescription', {
                customConfig: '/scripts/addons/ckeditor/simpleEditorConfig.js'
            });
        }

        function FormValidation() {
            $('#_RegisterCompanyForm').form({
                fields: {
                    Title: {
                        identifier: 'CompanyName',
                        rules: [
                          {
                              type: 'empty',
                              prompt: 'Vyplň název společnosti'
                          }
                        ]
                    },
                    PublicEmail: {
                        identifier: 'PublicEmail',
                        rules: [
                          {
                              type: 'empty',
                              prompt: 'Vyplň e-mail společnosti'
                          }
                        ]
                    },
                    ShortDescription: {
                        identifier: 'ShortDescription',
                        rules: [
                          {
                              type: 'empty',
                              prompt: 'Popiš svou firmu'
                          }
                        ]
                    },
                    YearFounded: {
                        identifier: 'YearFounded',
                        rules: [
                          {
                              type: 'empty',
                              prompt: 'Zvol rok založení'
                          },
                             {
                                 type: 'integer[1..100000]',
                                 prompt: 'Rok musí být celé číslo'
                             }
                        ]
                    },
                    Address: {
                        identifier: 'Address',
                        rules: [
                          {
                              type: 'empty',
                              prompt: 'Zadej adresu společnosti'
                          }
                        ]
                    },
                    State: {
                        identifier: 'State',
                        rules: [
                          {
                              type: 'empty',
                              prompt: 'Zvol stát'
                          }
                        ]
                    },
                    City: {
                        identifier: 'City',
                        rules: [
                          {
                              type: 'empty',
                              prompt: 'Zvol město'
                          }
                        ]
                    },
                    Web: {
                        identifier: 'Web',
                        rules: [
                          {
                              type: 'empty',
                              prompt: 'Zadej web společnosti'
                          },
                           {
                               type: 'url',
                               prompt: 'Zadej validní url webu'
                           }
                        ]
                    },
                }
            });
        }

    });
});