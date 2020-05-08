using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace CruisePMS.CruiseDepartures.Dtos
{
    public class CreateOrEditCruiseDeparturesDto : EntityDto<int?>
    {

        public int DepartureYear { get; set; }
        public string SeasonGroup { get; set; }
        public DateTime DepartureDate { get; set; }
        public int? CruisesId { get; set; }


    }

    public class CreateDeparturesDto
    {
        public int Id { get; set; }

        public int DepartureYear { get; set; }

        public string SeasonGroup { get; set; }

        public DateTime DepartureDate { get; set; }
        public string strDepartureDate { get; set; }

        public bool IsDeleted { get; set; }

        public bool IsChecked { get; set; }

        public int CruiseId { get; set; }

    }

    public class InsertedRecordsDto
    {
        public int Id { get; set; }
        public DateTime DepartureDate { get; set; }
        public int DepartureYear { get; set; }
        public int CruiseId { get; set; }
        public string SeasonGroup { get; set; }
    }

    public class InsertdRecordsByCruiseId
    {
        public List<InsertedRecordsDto> insertedRecordsDtos { get; set; } = new List<InsertedRecordsDto>();
    }
}
