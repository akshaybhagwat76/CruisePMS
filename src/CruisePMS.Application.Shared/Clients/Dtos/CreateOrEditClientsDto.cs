using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CruisePMS.Clients.Dtos
{
	public class CreateOrEditClientsDto : EntityDto<long?>
	{

		public int ReservationHolderID { get; set; }


		[Required]
		public string ClientFirstName { get; set; }


		[Required]
		public string ClientLastName { get; set; }


		public DateTime ClientDOB { get; set; }


		public DateTime WeddingAniversary { get; set; }


		public string ClientCountry { get; set; }


		public string ClientGoogleAddress { get; set; }


		[Required]
		public string ClientEmail { get; set; }


		public string ContactCellPhone { get; set; }


		public DateTime Issued { get; set; }


		public DateTime Expiration { get; set; }


		public string CountryResidence { get; set; }


		public string MedicalIssues { get; set; }


		public string FoodIssues { get; set; }


		public string ClientPassword { get; set; }


		public int IsClientLoginWith { get; set; }


		public int LoginFailAttempt { get; set; }


		public bool IsLocked { get; set; }


		public int ProfilePictureId { get; set; }

		public string ClientZipCode { get; set; }
		public string ClientTitle { get; set; }
		public string ClientCity { get; set; }
		public string ClientCountryRegion { get; set; }

		public string ClientResidenceCountry { get; set; }

		public int? ClientGender { get; set; }

		public int? ClientDocumentID { get; set; }

		public string ClientDocumentNo { get; set; }
	}
}
