using Abp.Application.Services.Dto;
namespace CruisePMS.CruisePrices.Dtos
{
    public class CreateOrEditCruisePricesDto : EntityDto<int?>
    {
        public decimal UnitPrice { get; set; }
        public string CurrencyId { get; set; }
        public int CruiseFaresId { get; set; }
        public int CruiseDeparturesId { get; set; }
        public int CruiseShipCabinsId { get; set; }
        public int CruiseServicesId { get; set; }
        public int CruiseShipsId { get; set; }
    }
}
