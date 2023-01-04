function ValidateMandEnrollDate() {
    var D = Object.keys(object.objJsonCustData.JsonListCust).length;// count of values in JsonListCust

    var Dynamicname = [], MandFieldDate = [], status;
    status = true;
    for (k = 0; k < D; k++) {
        count = k + 1;

        var ctrlId2 = "txtDateEnroll" + k;
        var innerCount = Object.keys(object.objJsonCustData.JsonListCust[k]).length;
        if (innerCount == 1) {

            var H = 1;
            $.each(object.objJsonCustData.JsonListCust[k], function (i, item) {
                if (H == 1) {

                    var Temp1 = $("#" + ctrlId2).val();

                    Dynamicname[k] = item.FieldName;
                    MandFieldDate[k] = item.FieldTypeId

                    if ((MandFieldDate[k] == 1) && (Temp1 == "")) {
                        status = Dynamicname[k];
                        return status;
                    }
                }
                H++;
            });
        }
    }
    return status;
}


