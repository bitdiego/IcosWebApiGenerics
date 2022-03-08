using IcosWebApiGenerics.ControllerFactory;
using IcosWebApiGenerics.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace IcosWebApiGenerics.Models.BADM
{
	[GeneratedController("api/dsnow")]
	public class GRP_D_SNOW : BaseClass
	{
		public GRP_D_SNOW() { GroupId = (int)Globals.Groups.GRP_D_SNOW; }

		public string D_SNOW_PLOT { get; set; }

		[Column(TypeName = "decimal(18, 8)")]
		public decimal? D_SNOW_EASTWARD_DIST { get; set; }

		[Column(TypeName = "decimal(18, 8)")]
		public decimal? D_SNOW_NORTHWARD_DIST { get; set; }

		[Column(TypeName = "decimal(18, 8)")]
		public decimal? D_SNOW_DISTANCE_POLAR { get; set; }

		[Column(TypeName = "decimal(18, 8)")]
		public decimal? D_SNOW_ANGLE_POLAR { get; set; }

		public string D_SNOW_VARMAP { get; set; }

		[Column(TypeName = "decimal(18, 8)")]
		public decimal D_SNOW { get; set; }

		public string D_SNOW_COMMENT { get; set; }

		public string D_SNOW_DATE { get; set; }

	}
}
