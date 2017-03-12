require(['jquery', 'datepicker', 'ckeditor', 'semantic', ], function ($, Datepicker) {
    // DOM ready
    $(function () {
        // initialization
        $('._YearFoundedDropdown, ._CompanySizeDropdown, ._StateDropdown, ._CompanyCategoryDropdown').dropdown();
    });
});