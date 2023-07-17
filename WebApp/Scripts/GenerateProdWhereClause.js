function GenerateProdWhereClause(Input) {
    var L, Temp = "";
    const Str = (JSON.stringify(Input)).split(',');
    //alert("Length" + Str.length);
    var c = Str.length - 1;
    try {
        if (Str.length == 1) {
            for (i = 0; i <= c; i++) {
               
                /*L = "ProductCode = '" + (Str[i].replace('[', '')) + "'";*/
                    //L = "'" + (Str[i].replace('[', '')) + "'";
                L = (Str[i].replace('[', ''));
                    L = L.replace('"', '')
                    L = L.replace('"', '')
                    L = L.replace(']', '')
                    Temp = Temp.concat(L);            
            }
        }
        else if (Str.length == 2) {
            for (i = 0; i <= c; i++) {
                //alert(Str[i]);
                if (i == 0) {
                    /*L = "ProductCode = '" + (Str[i].replace('[', '')) + "' and ";*/
                    /*L = "'" + (Str[i].replace('[', '')) + "' and ";*/
                    L = (Str[i].replace('[', '')) + "' and ";
                    //alert(L);
                    L = L.replace('"', '')
                    L = L.replace('"', '')
                    Temp = Temp.concat(L);
                }
                else {
                    L = (Str[i].replace(']', ''));
                    L = L.replace('"', '')
                    L = L.replace('"', '')
                   // Temp = Temp.concat("ProductCode ='" + L + "'");
                    Temp = Temp.concat("ProductCode ='" + L);
                }
            }
        }
        else {
            for (i = 0; i <= c; i++) {
                //alert(Str[i]);
                if (i == 0) {
                    //L = "ProductCode = '" + (Str[i].replace('[', '')) + "' and ";
                    /*L = "'" + (Str[i].replace('[', '')) + "' and ";*/
                    L = (Str[i].replace('[', '')) + "' and ";
                    //alert(L);
                    L = L.replace('"', '')
                    L = L.replace('"', '')
                    Temp = Temp.concat(L);
                }
                else if (i == c) {
                    L = (Str[i].replace(']', ''));
                    L = L.replace('"', '')
                    L = L.replace('"', '')
                    /*Temp = Temp.concat("ProductCode = '" + L + "'");*/
                    Temp = Temp.concat("ProductCode = '" + L);
                }
                else {
                    L = (Str[i].replace('"', ''));
                    L = L.replace('"', '')
                    Temp = Temp.concat("ProductCode = '" + L + "' and ");
                }

            }
        }
    }
    catch (err) {
        document.getElementById("demo").innerHTML = err.message;
    }

    return Temp;
}