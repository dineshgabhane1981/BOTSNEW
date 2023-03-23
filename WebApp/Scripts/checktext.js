
function CheckText(text) {
    var val = text,status;
    
    var english = /[A-Za-z0-9!$%&*()_+\-=\[\]{};':"\\|,.<>\/?\#\ \n\''@]/ig;
    
    var  Text = safeRedactName(val, english);
    var Temp = Text.length

    if (Temp > 10) {
        status = 1
    }
    else {
        status = 0
    }

    return status;
}

function safeRedactName(text, name) {
    return text.replaceAll(name, "");
}

