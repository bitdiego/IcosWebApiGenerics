using IcosWebApiGenerics.ControllerFactory;
using IcosWebApiGenerics.Utils;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace IcosWebApiGenerics.Models.BADM
{
    [GeneratedController("api/location")]
    public class GRP_LOCATION : BaseClass
    {
        public GRP_LOCATION() 
        { 
            GroupId = (int)Globals.Groups.GRP_LOCATION; 
        }

        [Column(TypeName = "decimal(18, 8)")]

        public decimal LOCATION_LAT { get; set; }
        [Column(TypeName = "decimal(18, 8)")]
        public decimal LOCATION_LONG { get; set; }

        [Column(TypeName = "decimal(18, 8)")]
        public decimal? LOCATION_ELEV { get; set; }

        public string LOCATION_DATE { get; set; }

        public string LOCATION_COMMENT { get; set; }

    }
}
