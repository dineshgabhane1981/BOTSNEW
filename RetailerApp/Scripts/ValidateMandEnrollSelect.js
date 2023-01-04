function ValidateMandEnrollSelect() {
    var D = Object.keys(object.objJsonCustData.JsonListCust).length;// count of values in JsonListCust

    var Dynamicname = [], MandFieldSelect = [], status;
    status = true;
    for (k = 0; k < D; k++) {
        count = k + 1;

        var ctrlId = "ddlCustomCtrlEnroll" + k;
        var innerCount = Object.keys(object.objJsonCustData.JsonListCust[k]).length;
        if (innerCount > 1) {

            var H = 1;
            $.each(object.objJsonCustData.JsonListCust[k], function (i, item) {
                if (H == 1) {

                    var Temp1 = $("#" + ctrlId).val();

                    Dynamicname[k] = item.FieldName;
                    MandFieldSelect[k] = item.FieldTypeId

                    if ((MandFieldSelect[k] == 1) && (Temp1 == 0)) {
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


