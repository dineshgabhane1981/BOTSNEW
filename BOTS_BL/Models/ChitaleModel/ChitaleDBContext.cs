using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace BOTS_BL.Models.ChitaleModel
{
    public partial class ChitaleDBContext : DbContext
    {
        public ChitaleDBContext()
            : base("name=ChitaleDBContext")
        {
        }

        public virtual DbSet<CancelOrderDetail> CancelOrderDetails { get; set; }
        public virtual DbSet<ChequeBounceConfig> ChequeBounceConfigs { get; set; }
        public virtual DbSet<ChequeReturnMaster> ChequeReturnMasters { get; set; }
        public virtual DbSet<CityMaster> CityMasters { get; set; }
        public virtual DbSet<ClusterMaster> ClusterMasters { get; set; }
        public virtual DbSet<CustomerDetail> CustomerDetails { get; set; }
        public virtual DbSet<CustomerMapping> CustomerMappings { get; set; }
        public virtual DbSet<ErrorServerLog> ErrorServerLogs { get; set; }
        public virtual DbSet<InvoiceDetail> InvoiceDetails { get; set; }
        public virtual DbSet<InvoiceMaster> InvoiceMasters { get; set; }
        public virtual DbSet<InvoiceOrderMapping> InvoiceOrderMappings { get; set; }
        public virtual DbSet<LogDetail> LogDetails { get; set; }
        public virtual DbSet<LoseDetail> LoseDetails { get; set; }
        public virtual DbSet<OrderDetail> OrderDetails { get; set; }
        public virtual DbSet<OrderMaster> OrderMasters { get; set; }
        public virtual DbSet<OrderPointsConfig> OrderPointsConfigs { get; set; }
        public virtual DbSet<OTPMaintenance> OTPMaintenances { get; set; }
        public virtual DbSet<PaymentMaster> PaymentMasters { get; set; }
        public virtual DbSet<PaymentPointsConfig> PaymentPointsConfigs { get; set; }
        public virtual DbSet<PointsExpiry> PointsExpiries { get; set; }
        public virtual DbSet<ProductReturnDetail> ProductReturnDetails { get; set; }
        public virtual DbSet<ProductReturnMaster> ProductReturnMasters { get; set; }
        public virtual DbSet<SalesmanMapping> SalesmanMappings { get; set; }
        public virtual DbSet<SubClusterMaster> SubClusterMasters { get; set; }
        public virtual DbSet<TargetMaster> TargetMasters { get; set; }
        public virtual DbSet<TempBalance> TempBalances { get; set; }
        public virtual DbSet<TempPoint> TempPoints { get; set; }
        public virtual DbSet<TempReceiptNo> TempReceiptNoes { get; set; }
        public virtual DbSet<TransactionMaster> TransactionMasters { get; set; }
        public virtual DbSet<tblRedemptionRequest> tblRedemptionRequests { get; set; }
        public virtual DbSet<TgtvsAchMaster> TgtvsAchMasters { get; set; }
        public virtual DbSet<RedemptionValue> RedemptionValues { get; set; }
        public virtual DbSet<OrderVsRavanaDay> OrderVsRavanaDays { get; set; }
        public virtual DbSet<InvoiceToOrder> InvoiceToOrders { get; set; }
        public virtual DbSet<tblErrorLog> tblErrorLogs { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CancelOrderDetail>()
                .Property(e => e.CustomerId)
                .IsUnicode(false);

            modelBuilder.Entity<CancelOrderDetail>()
                .Property(e => e.CustomerType)
                .IsUnicode(false);

            modelBuilder.Entity<CancelOrderDetail>()
                .Property(e => e.OrderNo)
                .IsUnicode(false);

            modelBuilder.Entity<CancelOrderDetail>()
                .Property(e => e.ProductCode)
                .IsUnicode(false);

            modelBuilder.Entity<CancelOrderDetail>()
                .Property(e => e.ProductName)
                .IsUnicode(false);

            modelBuilder.Entity<ChequeBounceConfig>()
                .Property(e => e.ConditionText)
                .IsUnicode(false);

            modelBuilder.Entity<ChequeReturnMaster>()
                .Property(e => e.CustomerId)
                .IsUnicode(false);

            modelBuilder.Entity<ChequeReturnMaster>()
                .Property(e => e.CustomerType)
                .IsUnicode(false);

            modelBuilder.Entity<ChequeReturnMaster>()
                .Property(e => e.ReceiverCustomerType)
                .IsUnicode(false);

            modelBuilder.Entity<ChequeReturnMaster>()
                .Property(e => e.ReceiverCustomerId)
                .IsUnicode(false);

            modelBuilder.Entity<ChequeReturnMaster>()
                .Property(e => e.ChequeNo)
                .IsUnicode(false);

            modelBuilder.Entity<ChequeReturnMaster>()
                .Property(e => e.ReceiptNo)
                .IsUnicode(false);

            modelBuilder.Entity<CityMaster>()
                .Property(e => e.City)
                .IsUnicode(false);

            modelBuilder.Entity<ClusterMaster>()
                .Property(e => e.Cluster)
                .IsUnicode(false);

            modelBuilder.Entity<CustomerDetail>()
                .Property(e => e.CustomerId)
                .IsUnicode(false);

            modelBuilder.Entity<CustomerDetail>()
                .Property(e => e.CustomerName)
                .IsUnicode(false);

            modelBuilder.Entity<CustomerDetail>()
                .Property(e => e.CustomerType)
                .IsUnicode(false);

            modelBuilder.Entity<CustomerDetail>()
                .Property(e => e.BOCustomerId)
                .IsUnicode(false);

            modelBuilder.Entity<CustomerDetail>()
                .Property(e => e.Status)
                .IsUnicode(false);

            modelBuilder.Entity<CustomerDetail>()
                .Property(e => e.City)
                .IsUnicode(false);

            modelBuilder.Entity<CustomerDetail>()
                .Property(e => e.Cluster)
                .IsUnicode(false);

            modelBuilder.Entity<CustomerDetail>()
                .Property(e => e.SubCluster)
                .IsUnicode(false);

            modelBuilder.Entity<CustomerDetail>()
                .Property(e => e.MobileNo)
                .IsUnicode(false);

            modelBuilder.Entity<CustomerDetail>()
                .Property(e => e.Address)
                .IsUnicode(false);

            modelBuilder.Entity<CustomerDetail>()
                .Property(e => e.Town)
                .IsUnicode(false);

            modelBuilder.Entity<CustomerDetail>()
                .Property(e => e.Taluka)
                .IsUnicode(false);

            modelBuilder.Entity<CustomerDetail>()
                .Property(e => e.District)
                .IsUnicode(false);

            modelBuilder.Entity<CustomerDetail>()
                .Property(e => e.State)
                .IsUnicode(false);

            modelBuilder.Entity<CustomerDetail>()
                .Property(e => e.Pincode)
                .IsUnicode(false);

            modelBuilder.Entity<CustomerDetail>()
                .Property(e => e.PanNo)
                .IsUnicode(false);

            modelBuilder.Entity<CustomerDetail>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<CustomerDetail>()
                .Property(e => e.GSTNo)
                .IsUnicode(false);

            modelBuilder.Entity<CustomerMapping>()
                .Property(e => e.CustomerId)
                .IsUnicode(false);

            modelBuilder.Entity<CustomerMapping>()
                .Property(e => e.CustomerType)
                .IsUnicode(false);

            modelBuilder.Entity<CustomerMapping>()
                .Property(e => e.MappedCustomerId)
                .IsUnicode(false);

            modelBuilder.Entity<CustomerMapping>()
                .Property(e => e.MappedCustomerType)
                .IsUnicode(false);

            modelBuilder.Entity<CustomerMapping>()
                .Property(e => e.CustomerName)
                .IsUnicode(false);

            modelBuilder.Entity<CustomerMapping>()
                .Property(e => e.MappedCustomerName)
                .IsUnicode(false);

            modelBuilder.Entity<CustomerMapping>()
                .Property(e => e.Status)
                .IsUnicode(false);

            modelBuilder.Entity<ErrorServerLog>()
                .Property(e => e.ErrorNumber)
                .IsUnicode(false);

            modelBuilder.Entity<ErrorServerLog>()
                .Property(e => e.ErrorProcedure)
                .IsUnicode(false);

            modelBuilder.Entity<ErrorServerLog>()
                .Property(e => e.ErrorSeverity)
                .IsUnicode(false);

            modelBuilder.Entity<ErrorServerLog>()
                .Property(e => e.ErrorState)
                .IsUnicode(false);

            modelBuilder.Entity<ErrorServerLog>()
                .Property(e => e.ErrorLine)
                .IsUnicode(false);

            modelBuilder.Entity<ErrorServerLog>()
                .Property(e => e.ErrorMessage)
                .IsUnicode(false);

            modelBuilder.Entity<InvoiceDetail>()
                .Property(e => e.InvoiceNo)
                .IsUnicode(false);

            modelBuilder.Entity<InvoiceDetail>()
                .Property(e => e.CustomerId)
                .IsUnicode(false);

            modelBuilder.Entity<InvoiceDetail>()
                .Property(e => e.CustomerType)
                .IsUnicode(false);

            modelBuilder.Entity<InvoiceDetail>()
                .Property(e => e.ProductCode)
                .IsUnicode(false);

            modelBuilder.Entity<InvoiceDetail>()
                .Property(e => e.ProductName)
                .IsUnicode(false);

            modelBuilder.Entity<InvoiceDetail>()
                .Property(e => e.Qty)
                .HasPrecision(18, 3);

            modelBuilder.Entity<InvoiceDetail>()
                .Property(e => e.ServiceProviderId)
                .IsUnicode(false);

            modelBuilder.Entity<InvoiceDetail>()
                .Property(e => e.ServiceProviderType)
                .IsUnicode(false);

            modelBuilder.Entity<InvoiceDetail>()
                .Property(e => e.Status)
                .IsUnicode(false);

            modelBuilder.Entity<InvoiceMaster>()
                .Property(e => e.InvoiceNo)
                .IsUnicode(false);

            modelBuilder.Entity<InvoiceMaster>()
                .Property(e => e.CustomerId)
                .IsUnicode(false);

            modelBuilder.Entity<InvoiceMaster>()
                .Property(e => e.CustomerType)
                .IsUnicode(false);

            modelBuilder.Entity<InvoiceMaster>()
                .Property(e => e.ServiceProviderId)
                .IsUnicode(false);

            modelBuilder.Entity<InvoiceMaster>()
                .Property(e => e.ServiceProviderType)
                .IsUnicode(false);

            modelBuilder.Entity<InvoiceMaster>()
                .Property(e => e.Status)
                .IsUnicode(false);

            modelBuilder.Entity<InvoiceOrderMapping>()
                .Property(e => e.OrderNo)
                .IsUnicode(false);

            modelBuilder.Entity<InvoiceOrderMapping>()
                .Property(e => e.InvoiceNo)
                .IsUnicode(false);

            modelBuilder.Entity<InvoiceOrderMapping>()
                .Property(e => e.CustomerId)
                .IsUnicode(false);

            modelBuilder.Entity<InvoiceOrderMapping>()
                .Property(e => e.CustomerType)
                .IsUnicode(false);

            modelBuilder.Entity<InvoiceOrderMapping>()
                .Property(e => e.ServiceProviderId)
                .IsUnicode(false);

            modelBuilder.Entity<InvoiceOrderMapping>()
                .Property(e => e.ServiceProviderType)
                .IsUnicode(false);

            modelBuilder.Entity<LogDetail>()
                .Property(e => e.ReceivedData)
                .IsUnicode(false);

            modelBuilder.Entity<LoseDetail>()
                .Property(e => e.OrderNo)
                .IsUnicode(false);

            modelBuilder.Entity<LoseDetail>()
                .Property(e => e.InvoiceNo)
                .IsUnicode(false);

            modelBuilder.Entity<LoseDetail>()
                .Property(e => e.ServiceProviderId)
                .IsUnicode(false);

            modelBuilder.Entity<LoseDetail>()
                .Property(e => e.ServiceProviderType)
                .IsUnicode(false);

            modelBuilder.Entity<LoseDetail>()
                .Property(e => e.ProductCode)
                .IsUnicode(false);

            modelBuilder.Entity<LoseDetail>()
                .Property(e => e.ProductName)
                .IsUnicode(false);

            modelBuilder.Entity<LoseDetail>()
                .Property(e => e.CustomerId)
                .IsUnicode(false);

            modelBuilder.Entity<LoseDetail>()
                .Property(e => e.CustomerType)
                .IsUnicode(false);

            modelBuilder.Entity<OrderDetail>()
                .Property(e => e.OrderNo)
                .IsUnicode(false);

            modelBuilder.Entity<OrderDetail>()
                .Property(e => e.CustomerId)
                .IsUnicode(false);

            modelBuilder.Entity<OrderDetail>()
                .Property(e => e.RefOrderNo)
                .IsUnicode(false);

            modelBuilder.Entity<OrderDetail>()
                .Property(e => e.ProductCode)
                .IsUnicode(false);

            modelBuilder.Entity<OrderDetail>()
                .Property(e => e.ProductName)
                .IsUnicode(false);

            modelBuilder.Entity<OrderDetail>()
                .Property(e => e.Qty)
                .IsUnicode(false);

            modelBuilder.Entity<OrderDetail>()
                .Property(e => e.CustomerType)
                .IsUnicode(false);

            modelBuilder.Entity<OrderDetail>()
                .Property(e => e.Status)
                .IsUnicode(false);

            modelBuilder.Entity<OrderMaster>()
                .Property(e => e.OrderNo)
                .IsUnicode(false);

            modelBuilder.Entity<OrderMaster>()
                .Property(e => e.CustomerId)
                .IsUnicode(false);

            modelBuilder.Entity<OrderMaster>()
                .Property(e => e.CustomerType)
                .IsUnicode(false);

            modelBuilder.Entity<OrderMaster>()
                .Property(e => e.PONumber)
                .IsUnicode(false);

            modelBuilder.Entity<OrderMaster>()
                .Property(e => e.OrderStatus)
                .IsUnicode(false);

            modelBuilder.Entity<OrderMaster>()
                .Property(e => e.RefOrderNo)
                .IsUnicode(false);

            modelBuilder.Entity<OrderMaster>()
                .Property(e => e.ServiceProviderId)
                .IsUnicode(false);

            modelBuilder.Entity<OrderMaster>()
                .Property(e => e.ServiceProviderType)
                .IsUnicode(false);

            modelBuilder.Entity<OrderMaster>()
                .Property(e => e.Status)
                .IsUnicode(false);

            modelBuilder.Entity<OrderPointsConfig>()
                .Property(e => e.ConditionText)
                .IsUnicode(false);

            modelBuilder.Entity<OTPMaintenance>()
                .Property(e => e.MobileNo)
                .IsUnicode(false);

            modelBuilder.Entity<OTPMaintenance>()
                .Property(e => e.CounterId)
                .IsUnicode(false);

            modelBuilder.Entity<OTPMaintenance>()
                .Property(e => e.OTP)
                .IsUnicode(false);

            modelBuilder.Entity<PaymentMaster>()
                .Property(e => e.PaymentType)
                .IsUnicode(false);

            modelBuilder.Entity<PaymentMaster>()
                .Property(e => e.CustomerId)
                .IsUnicode(false);

            modelBuilder.Entity<PaymentMaster>()
                .Property(e => e.CustomerType)
                .IsUnicode(false);

            modelBuilder.Entity<PaymentMaster>()
                .Property(e => e.PaymentStatus)
                .IsUnicode(false);

            modelBuilder.Entity<PaymentMaster>()
                .Property(e => e.InvoiceNo)
                .IsUnicode(false);

            modelBuilder.Entity<PaymentMaster>()
                .Property(e => e.ChequeNo)
                .IsUnicode(false);

            modelBuilder.Entity<PaymentMaster>()
                .Property(e => e.ServiceProviderId)
                .IsUnicode(false);

            modelBuilder.Entity<PaymentMaster>()
                .Property(e => e.ServiceProviderType)
                .IsUnicode(false);

            modelBuilder.Entity<PaymentMaster>()
                .Property(e => e.Status)
                .IsUnicode(false);

            modelBuilder.Entity<PaymentMaster>()
                .Property(e => e.ReceiptNo)
                .IsUnicode(false);

            modelBuilder.Entity<PaymentPointsConfig>()
                .Property(e => e.ConditionText)
                .IsUnicode(false);

            modelBuilder.Entity<PointsExpiry>()
                .Property(e => e.CustomerId)
                .IsUnicode(false);

            modelBuilder.Entity<PointsExpiry>()
                .Property(e => e.CustomerType)
                .IsUnicode(false);

            modelBuilder.Entity<PointsExpiry>()
                .Property(e => e.Status)
                .IsUnicode(false);

            modelBuilder.Entity<ProductReturnDetail>()
                .Property(e => e.CustomerId)
                .IsUnicode(false);

            modelBuilder.Entity<ProductReturnDetail>()
                .Property(e => e.CustomerType)
                .IsUnicode(false);

            modelBuilder.Entity<ProductReturnDetail>()
                .Property(e => e.ProductCode)
                .IsUnicode(false);

            modelBuilder.Entity<ProductReturnDetail>()
                .Property(e => e.ProductName)
                .IsUnicode(false);

            modelBuilder.Entity<ProductReturnDetail>()
                .Property(e => e.Qty)
                .IsUnicode(false);

            modelBuilder.Entity<ProductReturnDetail>()
                .Property(e => e.ProductReturnReason)
                .IsUnicode(false);

            modelBuilder.Entity<ProductReturnDetail>()
                .Property(e => e.MFCCode)
                .IsUnicode(false);

            modelBuilder.Entity<ProductReturnDetail>()
                .Property(e => e.ProductReturnRefNo)
                .IsUnicode(false);

            modelBuilder.Entity<ProductReturnDetail>()
                .Property(e => e.ReceiverCustomerId)
                .IsUnicode(false);

            modelBuilder.Entity<ProductReturnDetail>()
                .Property(e => e.ReceiverCustomerType)
                .IsUnicode(false);

            modelBuilder.Entity<ProductReturnDetail>()
                .Property(e => e.ProductVolume)
                .HasPrecision(18, 3);

            modelBuilder.Entity<ProductReturnMaster>()
                .Property(e => e.OrderNo)
                .IsUnicode(false);

            modelBuilder.Entity<ProductReturnMaster>()
                .Property(e => e.InvoiceNo)
                .IsUnicode(false);

            modelBuilder.Entity<ProductReturnMaster>()
                .Property(e => e.CustomerId)
                .IsUnicode(false);

            modelBuilder.Entity<ProductReturnMaster>()
                .Property(e => e.CustomerType)
                .IsUnicode(false);

            modelBuilder.Entity<SalesmanMapping>()
                .Property(e => e.NationalHead)
                .IsUnicode(false);

            modelBuilder.Entity<SalesmanMapping>()
                .Property(e => e.ZonalHead)
                .IsUnicode(false);

            modelBuilder.Entity<SalesmanMapping>()
                .Property(e => e.StateHead)
                .IsUnicode(false);

            modelBuilder.Entity<SalesmanMapping>()
                .Property(e => e.AreaSalesManager)
                .IsUnicode(false);

            modelBuilder.Entity<SalesmanMapping>()
                .Property(e => e.SalesExecutive)
                .IsUnicode(false);

            modelBuilder.Entity<SalesmanMapping>()
                .Property(e => e.SalesOfficer)
                .IsUnicode(false);

            modelBuilder.Entity<SalesmanMapping>()
                .Property(e => e.SalesRepresentative)
                .IsUnicode(false);

            modelBuilder.Entity<SalesmanMapping>()
                .Property(e => e.SalesmanId)
                .IsUnicode(false);

            modelBuilder.Entity<SalesmanMapping>()
                .Property(e => e.SalesmanName)
                .IsUnicode(false);

            modelBuilder.Entity<SalesmanMapping>()
                .Property(e => e.ParticipantId)
                .IsUnicode(false);

            modelBuilder.Entity<SalesmanMapping>()
                .Property(e => e.ParticipantType)
                .IsUnicode(false);

            modelBuilder.Entity<SalesmanMapping>()
                .Property(e => e.Status)
                .IsUnicode(false);

            modelBuilder.Entity<SalesmanMapping>()
                .Property(e => e.ParticipantName)
                .IsUnicode(false);

            modelBuilder.Entity<SubClusterMaster>()
                .Property(e => e.SubCluster)
                .IsUnicode(false);

            modelBuilder.Entity<TargetMaster>()
                .Property(e => e.CustomerId)
                .IsUnicode(false);

            modelBuilder.Entity<TargetMaster>()
                .Property(e => e.CustomerType)
                .IsUnicode(false);

            modelBuilder.Entity<TargetMaster>()
                .Property(e => e.ProductCode)
                .IsUnicode(false);

            modelBuilder.Entity<TargetMaster>()
                .Property(e => e.ProductName)
                .IsUnicode(false);

            modelBuilder.Entity<TargetMaster>()
                .Property(e => e.TargetProductVolume)
                .HasPrecision(18, 3);

            modelBuilder.Entity<TargetMaster>()
                .Property(e => e.MFCCode)
                .IsUnicode(false);

            modelBuilder.Entity<TargetMaster>()
                .Property(e => e.Status)
                .IsUnicode(false);

            modelBuilder.Entity<TempBalance>()
                .Property(e => e.CustomerId)
                .IsUnicode(false);

            modelBuilder.Entity<TempBalance>()
                .Property(e => e.CustomerType)
                .IsUnicode(false);

            modelBuilder.Entity<TempBalance>()
                .Property(e => e.Status)
                .IsUnicode(false);

            modelBuilder.Entity<TempPoint>()
                .Property(e => e.CustomerId)
                .IsUnicode(false);

            modelBuilder.Entity<TempPoint>()
                .Property(e => e.CustomerType)
                .IsUnicode(false);

            modelBuilder.Entity<TempPoint>()
                .Property(e => e.TxnType)
                .IsUnicode(false);

            modelBuilder.Entity<TempPoint>()
                .Property(e => e.Status)
                .IsUnicode(false);

            modelBuilder.Entity<TempReceiptNo>()
                .Property(e => e.ReceiptNo)
                .IsUnicode(false);

            modelBuilder.Entity<TempReceiptNo>()
                .Property(e => e.Status)
                .IsUnicode(false);

            modelBuilder.Entity<TransactionMaster>()
                .Property(e => e.CustomerId)
                .IsUnicode(false);

            modelBuilder.Entity<TransactionMaster>()
                .Property(e => e.CustomerType)
                .IsUnicode(false);

            modelBuilder.Entity<TransactionMaster>()
                .Property(e => e.TxnType)
                .IsUnicode(false);

            modelBuilder.Entity<TransactionMaster>()
                .Property(e => e.Status)
                .IsUnicode(false);

            modelBuilder.Entity<TransactionMaster>()
                .Property(e => e.OrderNo)
                .IsUnicode(false);

            modelBuilder.Entity<TransactionMaster>()
                .Property(e => e.PONumber)
                .IsUnicode(false);

            modelBuilder.Entity<TransactionMaster>()
                .Property(e => e.InvoiceNo)
                .IsUnicode(false);

            modelBuilder.Entity<TransactionMaster>()
                .Property(e => e.ReceiptNo)
                .IsUnicode(false);

            modelBuilder.Entity<TransactionMaster>()
                .Property(e => e.TxnElement)
                .IsUnicode(false);

            modelBuilder.Entity<TransactionMaster>()
                .Property(e => e.AppID)
                .IsUnicode(false);

            modelBuilder.Entity<TransactionMaster>()
                .Property(e => e.RefOrderNo)
                .IsUnicode(false);
        }

        
    }
}
