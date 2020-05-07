using System;
using System.Collections.Generic;
using System.Text;

namespace CruisePMS.CruiseMasterAmenities.Dtos
{
    public class RowReorder
    {
        public int DragIndex { get; set; }

        public int DropIndex { get; set; }

        public int DragIndexId { get; set; }

        public int DropIndexId { get; set; }
    }


    public class ReorderRowsByIcon
    {

        public int PresentRowNewId { get; set; }
        public int NewRowOrderValue { get; set; }
        public int ParentId { get; set; }
    }
}
