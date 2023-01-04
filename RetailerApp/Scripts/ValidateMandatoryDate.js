function ValidateMandatoryDate() {
    var C = Object.keys(object.objJsonData.JsonList1).length;// count of values in JsonList1

    var Dynamicname = [], MandFieldDate = [], status;
    status = 1;
    for (j = 0; j < C; j++) {
        count = j + 1;

        var innerCount = Object.keys(object.objJsonData.JsonList1[j]).length;
        if (innerCount == 1) {

            $.each(object.objJsonData.JsonList1[j], function (i, item) {

                Dynamicname[j] = item.FieldName
                if (item.FieldOptionId == "1") {
                    var ctrlId2 = "txtDate" + j;
                    var Temp2 = $("#" + ctrlId2).val();
                    MandFieldDate[j] = item.FieldTypeId

                    if ((MandFieldDate[j] == "1") && (Temp2 == "")) {
                        status = Dynamicname[j];
                        return status;
                    }
                }
            });
        }
    }
    return status;
}
