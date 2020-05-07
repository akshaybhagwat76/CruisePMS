
using System;
using Abp.Application.Services.Dto;

namespace CruisePMS.CruiseMasterAmenities.Dtos
{
    public class MasterAmenitiesDto : EntityDto
    {

		public virtual int NewId { get; set; }

		public virtual string Code { get; set; }

		public virtual string DisplayName { get; set; }

		public virtual string Lang { get; set; }

		public virtual int ParentId { get; set; }

		public virtual string DisplayName2 { get; set; }

		public virtual int? SourceId { get; set; }

		public virtual int? OrderColumn { get; set; }

		public virtual short? Original { get; set; }

		public virtual string SourceTable { get; set; }

	}
}