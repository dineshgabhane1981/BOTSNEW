function ValidateMandatorySelect(){
  var C = Object.keys(object.objJsonData.JsonList1).length;// count of values in JsonList1
   
  var Dynamicname = [], MandFieldSelect = [],status;
  status = true;
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
                        MandFieldSelect[j] = item.FieldTypeId
                       
                        if ((MandFieldSelect[j] == 1) && (Temp1 == 0))
                       {
                            status = Dynamicname[j];
                            return status;
                       }
                    }
                    K++;
                });
            }
        }
       return status;
}


        