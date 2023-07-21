function GenerateWhereClause(Input) {
    var L, Temp = "";
    const Str = (JSON.stringify(Input)).split(',');
    //alert("Length" + Str.length);
    var c = Str.length - 1;
    try {
        if (Str.length == 1) {
            for (i = 0; i <= c; i++) {

                L = (Str[i].replace('[', ''));
                //alert(L);
                L = L.replace('"', '')
                L = L.replace('"', '')
                L = L.replace(']', '')
                Temp = Temp.concat(L);
            }
        }
        else {
            for (i = 0; i <= c; i++) {
                
                   
                L = (Str[i].replace('[', ''));
                L = L.replace('"', '')
                L = L.replace('"', '')
                L = L.replace(']', '')
                    
                if (i == 0)
                {
                    Temp = Temp.concat(L);
                }
                else {
                    Temp = Temp.concat("," + L);
                }
            }

        }
    }
    catch (err) {
        document.getElementById("demo").innerHTML = err.message;
    }

    return Temp;
}