using Abp.Application.Services.Dto;
using System;
namespace CruisePMS.AccountPayments.Dtos
{
	public class CreateOrEditAccountPaymentsDto : EntityDto<int?>
	{

		public long? ReservationId { get; set; }


		public int? TenantIdPayer { get; set; }


		public int? ClientId { get; set; }


		public decimal? Paid { get; set; }


		public string CurrencyId { get; set; }


		public DateTime? PaymentDate { get; set; }


		public string BankDocumentNo { get; set; }


		public string PaymentInvoiceId { get; set; }


		public long? SupplierInvoiceId { get; set; }


		public long? SystemInvoiceId { get; set; }

		public bool ConfirmReservation { get; set; }

	}
}
