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
    },
    paths: {
        "jquery": "addons/jquery-2.1.4.min",
        "highcharts": "addons/highcharts", // http://www.highcharts.com/docs/getting-started/installation
        "ckeditor": "addons/ckeditor/ckeditor", // http://docs.ckeditor.com/#!/guide/dev_installation
        "promise": "addons/es6-promise.min", // https://github.com/jakearchibald/es6-promise#readme
        "semantic": "../Semantic/dist/semantic.min", // https://github.com/jakearchibald/es6-promise#readme
        "datepicker": "addons/datepicker-master/dist/datepicker.min", // https://github.com/fengyuanchen/datepicker
    },
    packages: [{
        name: "codemirror",
        location: "/scripts/addons/CodeMirror-master",
        main: "lib/codemirror"
    }],
};

// --------------- Version needs to be changed manually along with VersionInfo.cs ------------- //
function GetVersion() {
    return "1.0";
}
