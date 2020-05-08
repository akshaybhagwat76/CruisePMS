using Abp.Domain.Entities;
using System.ComponentModel.DataAnnotations.Schema;
    namespace CruisePMS.MainServices
{
    [Table("MainServices")]
    public class MainService : Entity
    {
        public virtual bool IsMainService { get; set; }
        public virtual string ServiceName { get; set; }
    }
}
