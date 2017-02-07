define(['jquery'], function ($) {

    function General() {}

    /// creates random guid
    General.prototype.guid = function guid() {
        return s4() + s4() + '-' + s4() + '-' + s4() + '-' +
          s4() + '-' + s4() + s4() + s4();
    }

    /// Used to get random char
    function s4() {
        return Math.floor((1 + Math.random()) * 0x10000)
          .toString(16)
          .substring(1);
    }

    General.prototype.numberWithCommas = function numberWithCommas(number) {
        return number.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
    }

    // I know its twice here...
    General.prototype.formatNumber = function numberWithCommas(number) {
        return this.numberWithCommas(number);
    }

    General.prototype.roundNumber = function numberWithCommas(number) {
        return Math.round(number * 100) / 100;
    }

    General.prototype.replaceAll = function (text, search, replacement) {
        return text.replace(new RegExp(search, 'g'), replacement);
    };

    General.prototype.changeQueryStringValue = function (uri, key, value) {
        var re = new RegExp("([?&])" + key + "=.*?(&|$)", "i");
        var separator = uri.indexOf('?') !== -1 ? "&" : "?";
        if (uri.match(re)) {
            return uri.replace(re, '$1' + key + "=" + value + '$2');
        }
        else {
            return uri + separator + key + "=" + value;
        }
    }

    return General;
});