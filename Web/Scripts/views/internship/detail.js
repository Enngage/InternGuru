require(['jquery', 'semantic'], function ($) {
    // DOM ready
    $(function () {
        // fixed top scrolling menu  
        $('#_InternshipMenuWrapper')
            .visibility({
                type: 'fixed',
                //offset: 54 // fixed by CSS 
            });
    });
});