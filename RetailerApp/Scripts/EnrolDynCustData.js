function EnrolDynCustData() {
    var Dynamicname = [];
    var DynamicResult = [];
    var DynamicCustData = [];
    var Temp = [];

    var C = Object.keys(object.objJsonCustData.JsonListCust).length;// count of values in JsonList1
 
    for (k = 0; k < C; k++) {
        count = k + 1;

        var ctrlId = "ddlCustomCtrlEnroll" + k;
        var innerCount = Object.keys(object.objJsonCustData.JsonListCust[k]).length;
        if (innerCount > 1) {

            var g = 1;
            $.each(object.objJsonCustData.JsonListCust[k], function (i, item) {
                if (g == 1) {

                    var Temp1 = $("#" + ctrlId).val();
                    Dynamicname[k] = item.FieldName;
                    DynamicCustData.push([Dynamicname[k], Temp1]);
                }
                g++;
            });

        }
        else {

            $.each(object.objJsonCustData.JsonListCust[k], function (i, item) {

                Dynamicname[k] = item.FieldName
                if (item.FieldOptionId == "1") {
                    var ctrlId1 = "txtCustomeEnroll" + k;
                    var Temp2 = $("#" + ctrlId1).val();
                    DynamicCustData.push([Dynamicname[k], Temp2]);
                }
                else if (item.FieldOptionId == "2") {
                    var ctrlId2 = "txtDateEnroll" + k;
                    var Temp3 = $("#" + ctrlId2).val();
                    DynamicCustData.push([Dynamicname[k], Temp3]);
                }
            });
        }
    }
    return DynamicCustData
}
