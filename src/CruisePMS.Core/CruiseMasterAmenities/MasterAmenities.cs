using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;

namespace CruisePMS.CruiseMasterAmenities
{
	[Table("AppMasterAmenities")]
    [Audited]
    public class MasterAmenities : AuditedEntity 
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