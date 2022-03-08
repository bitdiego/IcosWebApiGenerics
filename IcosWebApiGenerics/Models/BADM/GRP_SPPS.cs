using IcosWebApiGenerics.ControllerFactory;
using IcosWebApiGenerics.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace IcosWebApiGenerics.Models.BADM
{
    [GeneratedController("api/spps")]
	public class GRP_SPPS : BaseClass
	{
		public GRP_SPPS() { GroupId = (int)Globals.Groups.GRP_SPPS; }

		public string SPPS_PLOT { get; set; }

		public int? SPPS_LOCATION { get; set; }

		[Column(TypeName = "decimal(18, 8)")]
		public decimal? SPPS_LOCATION_LAT { get; set; }

		[Column(TypeName = "decimal(18, 8)")]
		public decimal? SPPS_LOCATION_LON { get; set; }

		[Column(TypeName = "decimal(18, 8)")]
		public decimal? SPPS_LOCATION_DIST { get; set; }

		[Column(TypeName = "decimal(18, 8)")]
		public decimal? SPPS_LOCATION_ANG { get; set; }

		public string SPPS { get; set; }

		[Column(TypeName = "decimal(18, 8)")]
		public decimal? SPPS_PERC_COVER { get; set; }

		public string SPPS_TWSP_PCT { get; set; }

		public string SPPS_COMMENT { get; set; }

		public string SPPS_DATE { get; set; }

		public string SPPS_PTYPE { get; set; }

	}
}
