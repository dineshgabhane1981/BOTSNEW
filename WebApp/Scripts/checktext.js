
function CheckText(text) {
    var val = text,status;
    var english = /^[A-Za-z][A-Za-z0-9!$%&*()_+\-=\[\]{};':"\\|,.<>\/?\#\ \n\'']*@$/;
    if (val != '') {
        if (english.test(val) == false) {

            status = 1;
        }
    }
    return status;
  }

