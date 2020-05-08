using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;

namespace CruisePMS.Clients.Dtos
{
    public class ClientsDto: EntityDto<long?>
	{
		public int ReservationHolderID { get; set; }

		public string ClientFirstName { get; set; }

		public string PIN { get; set; }
		public string ClientLastName { get; set; }

		public DateTime ClientDOB { get; set; }

		public DateTime WeddingAniversary { get; set; }

		public string ClientCountry { get; set; }

		public string ClientGoogleAddress { get; set; }

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

		public Nullable<Guid> ProfilePictureId { get; set; }


		public int? ClientGender { get; set; }

		public int? ClientDocumentID { get; set; }

		public int? ClientDocumentNo { get; set; }


	}
}
