using Abp.Auditing;
using Abp.Domain.Entities;
using CruisePMS.CruiseMasterAmenities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CruisePMS.Clients
{
	[Table("AppClients")]
	[Audited]
	public class Client : Entity<long>, IMayHaveTenant
	{
		public int? TenantId { get; set; }


		public virtual int ReservationHolderID { get; set; }

		[Required]
		public virtual string ClientFirstName { get; set; }

		[Required]
		public virtual string ClientLastName { get; set; }

		public virtual DateTime ClientDOB { get; set; }

		public virtual DateTime WeddingAniversary { get; set; }

		public virtual string ClientCountry { get; set; }

		public virtual string ClientGoogleAddress { get; set; }

		[Required]
		public virtual string ClientEmail { get; set; }

		public virtual string ContactCellPhone { get; set; }

		public virtual DateTime Issued { get; set; }

		public virtual DateTime Expiration { get; set; }

		public virtual string CountryResidence { get; set; }

		public virtual string MedicalIssues { get; set; }

		public virtual string FoodIssues { get; set; }

		public virtual string ClientPassword { get; set; }

		public virtual short IsClientLoginWith { get; set; }

		public virtual int LoginFailAttempt { get; set; }

		public virtual bool IsLocked { get; set; }

		public virtual Nullable<Guid> ProfilePictureId { get; set; }

		public virtual int? ClientGender { get; set; }

		[ForeignKey("ClientGender")]
		public MasterAmenities ClientGendFk { get; set; }

		public virtual int? ClientDocumentID { get; set; }

		[ForeignKey("ClientDocumentID")]
		public MasterAmenities ClientDocumentFk { get; set; }

		public virtual string ClientDocumentNo { get; set; }

		public virtual string ClientZipCode { get; set; }
		public virtual string ClientTitle { get; set; }
		public virtual string ClientCity { get; set; }

		public virtual string ClientCountryRegion { get; set; }

		public virtual string ClientResidenceCountry { get; set; }

		//      [ForeignKey("ClientDocumentNo")]
		//public CruiseMasterAmenities ClientDocumentNoFk { get; set; }

	}
}
