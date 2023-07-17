using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BOTS_BL.Models;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Data.SqlClient;
using System.Web.Mvc;
using BOTS_BL.Models.Reports;

namespace BOTS_BL.Repository
{
    public class OtherReportsRepository
    {
        CustomerRepository CR = new CustomerRepository();
        Exceptions newexception = new Exceptions();
        public List<SellingProductValue> GetTop5SellingProductValue(string GroupId, string connstr)
        {
            List<SellingProductValue> lstTop5SessingProductValue = new List<SellingProductValue>();
            try
            {
                using (var context = new BOTSDBContext(connstr))
                {
                    lstTop5SessingProductValue = context.Database.SqlQuery<SellingProductValue>("sp_BOTS_Top5_SellingProductValue").ToList<SellingProductValue>();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetTop5SellingProductValue");
            }
            return lstTop5SessingProductValue;
        }

        public List<SellingProductValue> GetBottom5SellingProductValue(string GroupId, string connstr)
        {
            List<SellingProductValue> lstBottom5SessingProductValue = new List<SellingProductValue>();
            try
            {
                using (var context = new BOTSDBContext(connstr))
                {
                    lstBottom5SessingProductValue = context.Database.SqlQuery<SellingProductValue>("sp_BOTS_Bottom5_SellingProductValue").ToList<SellingProductValue>();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetBottom5SellingProductValue");
            }
            return lstBottom5SessingProductValue;
        }
        public List<ReportForDownload> GetReportDownloadData(string groupId)
        {
            List<ReportForDownload> lstReportDownload = new List<ReportForDownload>();
            try
            {
                using (var context = new CommonDBContext())
                {
                    lstReportDownload = context.ReportForDownloads.Where(x=>x.GroupId== groupId).ToList();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetReportDownloadData");
            }

            return lstReportDownload;
        }
        public List<tblFranchiseeEnquiry> GetFranchiseeEnquiryList(string GroupId)
        {
            List<tblFranchiseeEnquiry> objData = new List<tblFranchiseeEnquiry>();
            try
            {
                var connectionString = CR.GetCustomerConnString(GroupId);
                using (var contextNew = new BOTSDBContext(connectionString))
                {
                    objData = contextNew.tblFranchiseeEnquiries.OrderByDescending(x => x.AddedDate).ToList();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetFranchiseeEnquiryList");
            }

            return objData;
        }
        public List<ProductWisePerformance> GetProductWiseReport(string GroupId, string connstr)
        {
            List<ProductWisePerformance> objProdData = new List<ProductWisePerformance>();
            try
            {
                using(var context = new BOTSDBContext(connstr))
                {
                    objProdData = context.Database.SqlQuery<ProductWisePerformance>("select T.Productcode as Productcode,T.CategoryCode as ProductCategoryCode,T.SubCategoryCode as ProductSubCategoryCode,Min(P.ProductName) as ProductName,Min(P.CategoryName) as ProductCategoryName ,Min(P.SubCategoryName) as ProductSubCategoryName,count(distinct T.Mobileno) as UniqueMember,count(T.Mobileno) as TotalTxn,sum(cast(cast(T.ProductQty as numeric(18,2)) as bigint) )as TotalQuantity,sum(T.ProductAmt) as TotalAmount,Max(cast(T.TxnDatetime as date)) as LastPurchasedate from View_tblTxnProDetailsMaster T inner join tblProductMaster P on T.Productcode = P.Productcode Group by T.Productcode,T.CategoryCode,T.SubCategoryCode Order by sum(cast(cast(T.ProductQty as numeric(18,2)) as bigint) ) desc,count(T.Productcode) desc").ToList();
                }
            }
            catch(Exception ex)
            {
                newexception.AddException(ex, "GetProductWisePerformances");
            }
            return objProdData;
        }

        public List<ProductWisePerformance> GetTop15SellingProduct(string GroupId, string connstr)
        {
            List<ProductWisePerformance> ObjTopSold = new List<ProductWisePerformance>();

            try
            {
                using (var context = new BOTSDBContext(connstr))
                {
                    ObjTopSold = context.Database.SqlQuery<ProductWisePerformance>("select top(15) Productcode,ProductCategoryCode,ProductSubCategoryCode,Min(ProductName) as ProductName,Min(ProductCategoryName) as ProductCategoryName ,Min(ProductSubCategoryName) as ProductSubCategoryName,count(Productcode) as TotalTxn,sum(cast(cast(ProductQty as numeric(18,2)) as bigint) )as TotalQuantity,sum(ProductAmt) as TotalAmount,Max(cast(datetime as date)) as LastPurchasedate from TxnProductDetails Group by Productcode,ProductCategoryCode,ProductSubCategoryCode Order by sum(cast(cast(ProductQty as numeric(18,2)) as bigint) ) desc,count(Productcode) desc").ToList();
                }
            }
            catch(Exception ex)
            {
                newexception.AddException(ex, "GetTop15SellingProduct");
            }

            return ObjTopSold;
        }

        public List<ProductWisePerformance> GetBtm15SellingProduct(string GroupId, string connstr)
        {
            List<ProductWisePerformance> ObjBtmSold = new List<ProductWisePerformance>();

            try
            {
                using (var context = new BOTSDBContext(connstr))
                {
                    ObjBtmSold = context.Database.SqlQuery<ProductWisePerformance>("select top(15) Productcode,ProductCategoryCode,ProductSubCategoryCode,Min(ProductName) as ProductName,Min(ProductCategoryName) as ProductCategoryName ,Min(ProductSubCategoryName) as ProductSubCategoryName,count(Productcode) as TotalTxn,sum(cast(cast(ProductQty as numeric(18,2)) as bigint) )as TotalQuantity,sum(ProductAmt) as TotalAmount,Max(cast(datetime as date)) as LastPurchasedate from TxnProductDetails Group by Productcode,ProductCategoryCode,ProductSubCategoryCode Order by sum(cast(cast(ProductQty as numeric(18,2)) as bigint) ) asc,count(Productcode) asc").ToList();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetBtm15SellingProduct");
            }

            return ObjBtmSold;
        }

        public List<ProductWisePerformance> GetProductDetailFilter(string GroupId, string connstr, string FromDate, string ToDate, string OutletId)
        {
            List<ProductWisePerformance> Obj = new List<ProductWisePerformance>();
            List<ProductWisePerformance> Obj1 = new List<ProductWisePerformance>();
            //string StrLastdate = string.Empty;
            //string StrFromdate, StrTodate;
            
            //StrFromdate = FromDate.ToString();
            //StrTodate = ToDate.ToString();
            try
            {
                using (var context = new BOTSDBContext(connstr))
                {
                    if(string.IsNullOrEmpty(FromDate) && string.IsNullOrEmpty(ToDate) && string.IsNullOrEmpty(OutletId))
                    {
                        Obj = context.Database.SqlQuery<ProductWisePerformance>("select T.Productcode as Productcode,T.CategoryCode as ProductCategoryCode,T.SubCategoryCode as ProductSubCategoryCode,Min(P.ProductName) as ProductName,Min(P.CategoryName) as ProductCategoryName ,Min(P.SubCategoryName) as ProductSubCategoryName,count(distinct T.Mobileno) as UniqueMember,count(T.Mobileno) as TotalTxn,sum(cast(cast(T.ProductQty as numeric(18,2)) as bigint) )as TotalQuantity,sum(T.ProductAmt) as TotalAmount,Max(cast(T.TxnDatetime as date)) as LastPurchasedate from View_tblTxnProDetailsMaster T inner join tblProductMaster P on T.Productcode = P.Productcode Group by T.Productcode,T.CategoryCode,T.SubCategoryCode Order by sum(cast(cast(T.ProductQty as numeric(18,2)) as bigint)) desc,count(P.Productcode) desc").ToList();
                    }
                    else if(FromDate != "" && OutletId != "" && ToDate != "")
                    {
                        Obj = context.Database.SqlQuery<ProductWisePerformance>("select T.Productcode as Productcode,T.CategoryCode as ProductCategoryCode,T.SubCategoryCode as ProductSubCategoryCode,Min(P.ProductName) as ProductName,Min(P.CategoryName) as ProductCategoryName ,Min(P.SubCategoryName) as ProductSubCategoryName,count(distinct T.Mobileno) as UniqueMember,count(T.Mobileno) as TotalTxn,sum(cast(cast(T.ProductQty as numeric(18,2)) as bigint) )as TotalQuantity,sum(T.ProductAmt) as TotalAmount,Max(cast(T.TxnDatetime as date)) as LastPurchasedate from View_tblTxnProDetailsMaster T inner join tblProductMaster P on T.Productcode = P.Productcode where (cast(T.TxnDatetime as date) between @FromDate and @ToDate) and T.OutletId = @Outlet Group by T.Productcode,T.CategoryCode,T.SubCategoryCode Order by sum(cast(cast(T.ProductQty as numeric(18,2)) as bigint) ) desc,count(T.Productcode) desc", new SqlParameter("@FromDate", FromDate), new SqlParameter("@ToDate", ToDate), new SqlParameter("@Outlet", OutletId)).ToList();
                    }
                    else if(FromDate != "" && string.IsNullOrEmpty(OutletId) && ToDate != "")
                    {
                        Obj = context.Database.SqlQuery<ProductWisePerformance>("select T.Productcode as Productcode,T.CategoryCode as ProductCategoryCode,T.SubCategoryCode as ProductSubCategoryCode,Min(P.ProductName) as ProductName,Min(P.CategoryName) as ProductCategoryName ,Min(P.SubCategoryName) as ProductSubCategoryName,count(distinct T.Mobileno) as UniqueMember,count(T.Mobileno) as TotalTxn,sum(cast(cast(T.ProductQty as numeric(18,2)) as bigint) )as TotalQuantity,sum(T.ProductAmt) as TotalAmount,Max(cast(T.TxnDatetime as date)) as LastPurchasedate from  View_tblTxnProDetailsMaster T inner join tblProductMaster P on T.Productcode = P.Productcode where (cast(T.TxnDatetime as date) between @FromDate and @ToDate) Group by T.Productcode,T.CategoryCode,T.SubCategoryCode Order by sum(cast(cast(T.ProductQty as numeric(18,2)) as bigint) ) desc,count(T.Productcode) desc", new SqlParameter("@FromDate", FromDate), new SqlParameter("@ToDate", ToDate)).ToList();
                    }
                    else if(string.IsNullOrEmpty(FromDate) && string.IsNullOrEmpty(ToDate) && OutletId != "")
                    {
                        Obj = context.Database.SqlQuery<ProductWisePerformance>("select T.Productcode as Productcode,T.CategoryCode as ProductCategoryCode,T.SubCategoryCode as ProductSubCategoryCode,Min(P.ProductName) as ProductName,Min(P.CategoryName) as ProductCategoryName ,Min(P.SubCategoryName) as ProductSubCategoryName,count(distinct T.Mobileno) as UniqueMember,count(T.Mobileno) as TotalTxn,sum(cast(cast(T.ProductQty as numeric(18,2)) as bigint) )as TotalQuantity,sum(T.ProductAmt) as TotalAmount,Max(cast(T.TxnDatetime as date)) as LastPurchasedate from View_tblTxnProDetailsMaster T inner join tblProductMaster P on T.Productcode = P.Productcode where T.OutletId = @Outlet Group by T.Productcode,T.CategoryCode,T.SubCategoryCode Order by sum(cast(cast(T.ProductQty as numeric(18,2)) as bigint) ) desc,count(T.Productcode) desc", new SqlParameter("@Outlet", OutletId)).ToList();
                    }
                    
                }
            }
            catch(Exception ex)
            {
                newexception.AddException(ex, "GetProductDetailFilter");
            }

            return Obj;
        }
        public List<SelectListItem> GetProductId(string GroupId, string connstr)
        {
            List<SelectListItem> ObjProdList = new List<SelectListItem>();

            try
            {
                using (var context = new BOTSDBContext(connstr))
                {
                    var Proddata = context.tblProductMasters.ToList();

                    foreach (var item in Proddata)
                    {
                        ObjProdList.Add(new SelectListItem {
                            Text = item.ProductName,
                            Value = Convert.ToString(item.ProductCode)
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetProductId");
            }

            return ObjProdList;
        }
        public List<SelectListItem> GetCategoryCode(string GroupId,string connstr)
        {
            List<SelectListItem> ObjCategoryCode = new List<SelectListItem>();
            List <ProductDetailsMaster> objProd = new List<ProductDetailsMaster>();
            try
            {
                using (var context = new BOTSDBContext(connstr))
                {
                    //var Proddata = context.ProductMasters.Select(o => o.CategoryCode && o.CategoryName).Distinct().ToList();

                    objProd = context.Database.SqlQuery<ProductDetailsMaster>("select CategoryCode,CategoryName from tblProductMaster Group by CategoryCode,CategoryName").ToList();

                    foreach (var item in objProd)
                    {
                        ObjCategoryCode.Add(new SelectListItem
                        {
                            Text = item.CategoryName,
                            Value = Convert.ToString(item.CategoryCode)
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetProductId");
            }

            return ObjCategoryCode;
        }
        public List<SelectListItem> GetSubCategoryCodeALL(string GroupId, string connstr)
        {
            List<SelectListItem> ObjSubCategoryCode = new List<SelectListItem>();
            List<ProductDetailsMaster> objProd = new List<ProductDetailsMaster>();
            try
            {
                using (var context = new BOTSDBContext(connstr))
                {
                    //var Proddata = context.ProductMasters.Select(o => o.CategoryCode && o.CategoryName).Distinct().ToList();

                    objProd = context.Database.SqlQuery<ProductDetailsMaster>("select SubCategoryCode,SubCategoryName from tblProductMaster Group by SubCategoryCode,SubCategoryName").ToList();

                    foreach (var item in objProd)
                    {
                        ObjSubCategoryCode.Add(new SelectListItem
                        {
                            Text = Convert.ToString(item.SubCategoryName),
                            Value = Convert.ToString(item.SubCategoryCode)
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetSubCategoryCodeALL");
            }

            return ObjSubCategoryCode;
        }
        public List<SelectListItem> GetSubCategoryCode(string connstr, string CategoryCode)
        {
            List<SelectListItem> ObjSubCategoryCode = new List<SelectListItem>();
            List<ProductDetailsMaster> objProd = new List<ProductDetailsMaster>();
            try
            {
                using (var context = new BOTSDBContext(connstr))
                {
                    //var Proddata = context.ProductMasters.Select(o => o.CategoryCode && o.CategoryName).Distinct().ToList();

                    if(CategoryCode == "0000")
                    {
                        objProd = context.Database.SqlQuery<ProductDetailsMaster>("select SubCategoryCode,SubCategoryName from tblProductMaster Group by SubCategoryCode,SubCategoryName").ToList();
                    }
                    else 
                    {
                        objProd = context.Database.SqlQuery<ProductDetailsMaster>("select SubCategoryCode,SubCategoryName from tblProductMaster where CategoryCode = @CategoryCode Group by SubCategoryCode,SubCategoryName", new SqlParameter("@CategoryCode", CategoryCode)).ToList();
                    }                  

                    foreach (var item in objProd)
                    {
                        ObjSubCategoryCode.Add(new SelectListItem
                        {
                            Text = Convert.ToString(item.SubCategoryName),
                            Value = Convert.ToString(item.SubCategoryCode)
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetSubCategoryCode");
            }

            return ObjSubCategoryCode;
        }
        public List<SelectListItem> GetProductCode(string connstr, string SubCategorycode)
        {
            List<SelectListItem> ObjSubCategoryCode = new List<SelectListItem>();
            List<ProductDetailsMaster> objProd = new List<ProductDetailsMaster>();
            try
            {
                using (var context = new BOTSDBContext(connstr))
                {

                    if(SubCategorycode == "0000")
                    {
                        objProd = context.Database.SqlQuery<ProductDetailsMaster>("select ProductCode,ProductName from tblProductMaster Group by ProductCode,ProductName").ToList();
                    }
                    else
                    {
                        objProd = context.Database.SqlQuery<ProductDetailsMaster>("select ProductCode,ProductName from tblProductMaster where SubCategoryCode = @SubCategoryCode Group by ProductCode,ProductName", new SqlParameter("@SubCategoryCode", SubCategorycode)).ToList();
                    }                    

                    foreach (var item in objProd)
                    {
                        ObjSubCategoryCode.Add(new SelectListItem
                        {
                            Text = Convert.ToString(item.ProductName),
                            Value = Convert.ToString(item.ProductCode)
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetProductCode");
            }

            return ObjSubCategoryCode;
        }
        public List<ProductAnalytics> GetProductAnalyticFilter(string GroupId, string connstr, Int16 PurchaseFiter1, Int16 PurchaseFiter2, string dtFrom3, string Todte3, string CategoryCode1, string SubCategoryCode1, string LstProd1, string CategoryCode2, string SubCategoryCode2, string LstProd2, string NotPurchasedSince, Int32 AmountSpentFrom, Int32 AmountSpentTo, string LstOutlet)
        {
            List<ProductAnalytics> Obj = new List<ProductAnalytics>();
            //List<ProductDetailsMaster> objProd = new List<ProductDetailsMaster>();
            try
            {
                using (var context = new BOTSDBContext(connstr))
                {
                    Obj = context.Database.SqlQuery<ProductAnalytics>("sp_ProductAnalyticsFilter @pi_AmountSpentFrom,@pi_AmountSpentTo,@pi_CategoryCode1,@pi_CategoryCode2,@pi_LstOutlet,@pi_LstProd1,@pi_LstProd2,@pi_NotPurchasedSince,@pi_PurchaseFiter1,@pi_PurchaseFiter2,@pi_SubCategoryCode1,@pi_SubCategoryCode2,@pi_Todte3,@pi_dtFrom3", 
                        new SqlParameter("@pi_AmountSpentFrom", AmountSpentFrom), new SqlParameter("@pi_AmountSpentTo", AmountSpentTo), new SqlParameter("@pi_CategoryCode1", CategoryCode1), new SqlParameter("@pi_CategoryCode2", CategoryCode2), new SqlParameter("@pi_LstOutlet", LstOutlet), new SqlParameter("@pi_LstProd1", LstProd1), new SqlParameter("@pi_LstProd2", LstProd2), new SqlParameter("@pi_NotPurchasedSince", NotPurchasedSince), new SqlParameter("@pi_PurchaseFiter1", PurchaseFiter1), new SqlParameter("@pi_PurchaseFiter2", PurchaseFiter2), new SqlParameter("@pi_SubCategoryCode1", SubCategoryCode1), new SqlParameter("@pi_SubCategoryCode2", SubCategoryCode2), new SqlParameter("@pi_Todte3", Todte3), new SqlParameter("@pi_dtFrom3", dtFrom3)).ToList();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetProductId");
            }

            return Obj;
        }
    }
}
