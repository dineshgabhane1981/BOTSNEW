function InvoiceDynData() {
    var Dynamicname = [];
    var DynamicResult = [];
    var DynamicData = [];
    var Temp = [];

    var C = Object.keys(object.objJsonData.JsonList1).length;// count of values in JsonList1
 
    for (j = 0; j < C; j++) {
        count = j + 1;

        var ctrlId = "ddlCustomCtrl" + j;
        var innerCount = Object.keys(object.objJsonData.JsonList1[j]).length;
        if (innerCount > 1) {

            var K = 1;
            $.each(object.objJsonData.JsonList1[j], function (i, item) {
                if (K == 1) {

                    var Temp1 = $("#" + ctrlId).val();
                    Dynamicname[j] = item.FieldName;
                    DynamicData.push([Dynamicname[j], Temp1]);
                }
                K++;
            });

        }
        else {

            $.each(object.objJsonData.JsonList1[j], function (i, item) {

                Dynamicname[j] = item.FieldName
                if (item.FieldOptionId == "1") {
                    var ctrlId1 = "txtCustome" + j;
                    var Temp2 = $("#" + ctrlId1).val();
                    DynamicData.push([Dynamicname[j], Temp2]);
                }
                else if (item.FieldOptionId == "2") {
                    var ctrlId2 = "txtDate" + j;
                    var Temp3 = $("#" + ctrlId2).val();
                    DynamicData.push([Dynamicname[j], Temp3]);
                }
            });
        }
    }
    return DynamicData
}
