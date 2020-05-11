using Abp.Application.Services.Dto;

namespace CruisePMS.CruisePrices.Dtos
{
    public class CruisePricesDto : EntityDto
    {
        public decimal UnitPrice { get; set; }

        public string CurrencyId { get; set; }

        public string SeasonGroup { get; set; }

        public int CruiseFaresId { get; set; }

        public int CruiseDeparturesId { get; set; }

        public int CruiseShipCabinsId { get; set; }

        public int CruiseServicesId { get; set; }

        public int CruiseShipsId { get; set; }

        public string CabintypeName { get; set; }
        public string DeckName { get; set; }

    }
}
