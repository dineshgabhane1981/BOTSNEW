function ValidateEmail(text) {
    var val = text, status;
    
    var email = /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
    if (val != '') {
        if (email.test(val) == false) {

            status = 1;
        }
        else {
            status = 0;
        }
    }
    return status;
  }