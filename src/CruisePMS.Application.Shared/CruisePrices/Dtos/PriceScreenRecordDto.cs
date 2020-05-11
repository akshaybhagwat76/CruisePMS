using System.Collections.Generic;
namespace CruisePMS.CruisePrices.Dtos
{
    public class PriceScreenRecordDto
    {
        public int FareId { get; set; }
        public int CruiseId { get; set; }

        public int CruiseShipsId { get; set; }

        public string DefaultCurency { get; set; }
        public string ServiceUnit { get; set; }

        public List<ShipCabinDetails> ShipCabinDetail { get; set; }

        public List<SeasonGroups> SeasonGroup { get; set; }
    }
    public class GetCruisePriceModalsScreen
    {
        public int cruiseId { get; set; }
        public int serviceGroupId { get; set; }
    }
    public class CruisePriceModalsScreen
    {
        public List<SeasonGroups> SeasonGroup { get; set; }
        public List<ServiceList> ServiceList { get; set; }
        public string DefaultCurency { get; set; }
        public string ServiceUnit { get; set; }
        public int CruiseId { get; set; }
        public int CruiseShipsId { get; set; }
    }

    public class ServiceList
    {
        public int ServiceId { get; set; }
        public int CruiseServiceGroupsId { get; set; }
        public string ServiceName { get; set; }
    }

    public class ShipCabinDetails
    {
        public int CruiseShipDecksId { get; set; }
        public int Id { get; set; }
        public int CabinTypeID { get; set; }
        public string DeckName { get; set; }
        public string CabinType { get; set; }
    }
    public class SeasonGroups
    {
        public string SeasonGroupName { get; set; }
    }


    public class SavePriceScreenRecordDto
    {
        public int CabinId { get; set; }
        public string CabinType { get; set; }
        public int CabinTypeID { get; set; }
        public int CruiseId { get; set; }
        public int CruiseShipDecksId { get; set; }
        public int CruiseShipsId { get; set; }
        public string DeckName { get; set; }
        public int FareId { get; set; }
        public string SeasonGroup { get; set; }
        public string UnitPrice1 { get; set; }
        public string UnitPrice2 { get; set; }
        public string UnitPrice3 { get; set; }
        public string UnitPrice4 { get; set; }
        public string UnitPrice5 { get; set; }
        public string UnitPrice6 { get; set; }
        public bool TextBoxValue1 { get; set; }
        public bool TextBoxValue2 { get; set; }
        public bool TextBoxValue3 { get; set; }
        public bool TextBoxValue4 { get; set; }
        public bool TextBoxValue5 { get; set; }
        public bool TextBoxValue6 { get; set; }

        public int PriceYear { get; set; }
        public int CruiseServiceId { get; set; }
        public string DefaultCurency { get; set; }

    }

    public class CheckDuplicate
    {
        public int CruiseShipDecksId { get; set; }
        public int Id { get; set; }
        public int CabinTypeID { get; set; }
    }
}
