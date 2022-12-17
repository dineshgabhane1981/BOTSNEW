function ConcatData(text) {
    var T1,T2,T3,T4,result,D,D1;
    T1 = "{";
    T2 = "}";

    var c = text.length;
    var T
    result = "{";
    var N = c - 1;
    for (j = 0; j < c; j++) {
        D = text[j][0];
        D1 = text[j][1];
        if (D1 == "") {
            D1 = " ";
        }

        result = result.concat("", "\"");
        result = result.concat("", D);
        result = result.concat("", "\"");
        result = result.concat("", ":");
        result = result.concat("", "\"");
        result = result.concat("", D1);
        result = result.concat("", "\"");
        if (N != j) {
            result = result.concat("", ",");
        }    
    }
    result = result.concat("", "}");
     
    return result;
  }