function countChar(val, div) {
    var len = val.value.length;
    if (len >= 500) {
        val.value = val.value.substring(0, 500);
    } else {
        $('#' + div).text(500 - len);
    }
};