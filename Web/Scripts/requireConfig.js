// file: js/require-setup.js
//
// Declare this variable before loading RequireJS JavaScript library
// To config RequireJS after it’s loaded, pass the below object into require.config();

var require = {  
    urlArgs: "v=" + GetVersion(), // appends url to each required module - used for cache clearing
    baseUrl: "/scripts",
    shim: {
        "highcharts": { "deps": ['jquery'] }, // highcharts needs to be loaded after jQuery
        "semantic": { "deps": ['jquery'] }, // semantic needs to be loaded after jQuery
        "datepicker": { "deps": ['jquery'] }, // datepicker needs to be loaded after jQuery
        "unitegallery": { "deps": ['jquery'] }, // unitegallery needs to be loaded after jQuery 
        "unitegallery_theme": { "deps": ['jquery', 'unitegallery'] }, // unitegallery needs to be loaded after jQuery AND unitegallery 
        "fineuploader": { "deps": ['jquery'] }, // finei uploader needs to be loaded after jQuery
        "jquery_cookie": { "deps": ['jquery'] }, // finei uploader needs to be loaded after jQuery
    },
    paths: {
        "jquery": "addons/jquery-3.1.1.min",
        "highcharts": "addons/highcharts", // http://www.highcharts.com/docs/getting-started/installation
        "ckeditor": "addons/ckeditor/ckeditor", // http://docs.ckeditor.com/#!/guide/dev_installation
        "promise": "addons/es6-promise.min", // https://github.com/jakearchibald/es6-promise#readme
        "semantic": "../Semantic/dist/semantic.min", // https://github.com/jakearchibald/es6-promise#readme
        "datepicker": "addons/datepicker-master/dist/datepicker.min", // https://github.com/fengyuanchen/datepicker
        "unitegallery": "addons/unitegallery-master/dist/js/unitegallery.min", // http://unitegallery.net/index.php?page=tiles-columns
        "unitegallery_theme": "addons/unitegallery-master/dist/themes/tiles/ug-theme-tiles", // http://unitegallery.net/index.php?page=tiles-columns
        "sortable": "addons/sortable", // https://github.com/RubaXa/Sortable
        "fineuploader": "addons/fineuploader/jquery.fine-uploader.min", // https://fineuploader.com/demos.html
        "jquery_cookie": "addons/jquery.cookie",
    },
    packages: [{
        name: "codemirror",
        location: "/scripts/addons/CodeMirror-master",
        main: "lib/codemirror"
    }],
};

// --------------- Version needs to be changed manually along with VersionInfo.cs ------------- //
function GetVersion() {
    return "0.97";
}
