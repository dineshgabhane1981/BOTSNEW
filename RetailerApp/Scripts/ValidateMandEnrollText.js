function ValidateMandEnrollText() {
    var D = Object.keys(object.objJsonCustData.JsonListCust).length;// count of values in JsonListCust

    var Dynamicname = [], MandFieldText = [], status;
    status = true;
    for (k = 0; k < D; k++) {
        count = k + 1;

        var ctrlId1 = "txtCustomeEnroll" + k;
        var innerCount = Object.keys(object.objJsonCustData.JsonListCust[k]).length;
        if (innerCount == 1) {

            var H = 1;
            $.each(object.objJsonCustData.JsonListCust[k], function (i, item) {
                if (H == 1) {

                    var Temp1 = $("#" + ctrlId1).val();
                   
                    Dynamicname[k] = item.FieldName;
                    MandFieldText[k] = item.FieldTypeId

                    if ((MandFieldText[k] == 1) && (Temp1 == "")) {
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


