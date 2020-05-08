using Abp.Auditing;
using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CruisePMS.AccountPayments
{
	[Table("AppAccountPayment")]
	[Audited]
	public class AccountPayment : Entity, IMustHaveTenant
	{
		public int TenantId { get; set; }
		public virtual long? ReservationId { get; set; }

		public virtual int? TenantIdPayer { get; set; }

		public virtual int? ClientId { get; set; }

		public virtual decimal? Paid { get; set; }

		public virtual string CurrencyId { get; set; }

		public virtual DateTime? PaymentDate { get; set; }

		public virtual string BankDocumentNo { get; set; }

		public virtual string PaymentInvoiceId { get; set; }

		public virtual long? SupplierInvoiceId { get; set; }

		public virtual long? SystemInvoiceId { get; set; }
	}
}
