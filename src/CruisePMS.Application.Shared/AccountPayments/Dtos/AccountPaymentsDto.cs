using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace CruisePMS.AccountPayments.Dtos
{
	public class AccountPaymentsDto : EntityDto
	{
		public decimal? Paid { get; set; }

		public string CurrencyId { get; set; }

		public DateTime? PaymentDate { get; set; }

		public string BankDocumentNo { get; set; }

		public string PaymentInvoiceId { get; set; }

		public long? SupplierInvoiceId { get; set; }

		public long? SystemInvoiceId { get; set; }

		public string Agent { get; set; }
		public string Client { get; set; }

	}
}
