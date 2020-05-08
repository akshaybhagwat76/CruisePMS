using System;
using System.Collections.Generic;
using System.Text;

namespace CruisePMS.AmenityStorages.Dtos
{
    public class CruiseAmenitiesStorageCruiseMasterAmenitiesLookupTableDto
    {
        public int Id { get; set; }

        public string DisplayName { get; set; }
    }

    public class MasterAmenitiesForListBoxLookupTableDto
    {
        public int Id { get; set; }
        public long SourceId { get; set; }
        public string DisplayName { get; set; }
        public long SectionId { get; set; }
        public int MasterAmenitiesId { get; set; }

        public long CruiseAmenitiesStorageId { get; set; }
    }
}
