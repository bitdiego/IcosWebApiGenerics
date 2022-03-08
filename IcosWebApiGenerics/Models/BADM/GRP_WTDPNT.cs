using IcosWebApiGenerics.ControllerFactory;
using IcosWebApiGenerics.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace IcosWebApiGenerics.Models.BADM
{
	[GeneratedController("api/wtdpnt")]
	public class GRP_WTDPNT : BaseClass
	{
		public GRP_WTDPNT() { GroupId = (int)Globals.Groups.GRP_WTDPNT; }

		public string WTDPNT_PLOT { get; set; }

		[Column(TypeName = "decimal(18, 8)")]
		public decimal? WTDPNT_EASTWARD_DIST { get; set; }

		[Column(TypeName = "decimal(18, 8)")]
		public decimal? WTDPNT_NORTHWARD_DIST { get; set; }

		[Column(TypeName = "decimal(18, 8)")]
		public decimal? WTDPNT_DISTANCE_POLAR { get; set; }

		[Column(TypeName = "decimal(18, 8)")]
		public decimal? WTDPNT_ANGLE_POLAR { get; set; }

		[Column(TypeName = "decimal(18, 8)")]
		public decimal? WTDPNT_WELL_DEPTH { get; set; }

		public string WTDPNT_VARMAP { get; set; }

		[Column(TypeName = "decimal(18, 8)")]
		public decimal WTDPNT { get; set; }

		public string WTDPNT_COMMENT { get; set; }

		public string WTDPNT_DATE { get; set; }

	}
}
