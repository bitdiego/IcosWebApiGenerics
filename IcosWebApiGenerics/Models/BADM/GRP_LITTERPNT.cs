using IcosWebApiGenerics.ControllerFactory;
using IcosWebApiGenerics.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace IcosWebApiGenerics.Models.BADM
{
	[GeneratedController("api/litterpnt")]
	public class GRP_LITTERPNT : BaseClass
	{
		public GRP_LITTERPNT() { GroupId = (int)Globals.Groups.GRP_LITTERPNT; }

		public string LITTERPNT_PLOT { get; set; }

		public int LITTERPNT_ID { get; set; }

		[Column(TypeName = "decimal(18, 8)")]
		public decimal? LITTERPNT_AREA { get; set; }

		[Column(TypeName = "decimal(18, 8)")]
		public decimal? LITTERPNT_EASTWARD_DIST { get; set; }

		[Column(TypeName = "decimal(18, 8)")]
		public decimal? LITTERPNT_NORTHWARD_DIST { get; set; }

		[Column(TypeName = "decimal(18, 8)")]
		public decimal? LITTERPNT_DISTANCE_POLAR { get; set; }

		[Column(TypeName = "decimal(18, 8)")]
		public decimal? LITTERPNT_ANGLE_POLAR { get; set; }

		public string LITTERPNT_TYPE { get; set; }

		public string LITTERPNT_FRACTION { get; set; }

		public string LITTERPNT_SPP { get; set; }

		[Column(TypeName = "decimal(18, 8)")]
		public decimal? LITTERPNT { get; set; }

		[Column(TypeName = "decimal(18, 8)")]
		public decimal? LITTERPNT_LEAVESAREA { get; set; }

		[Column(TypeName = "decimal(18, 8)")]
		public decimal? LITTERPNT_COARSE_DIAM { get; set; }

		[Column(TypeName = "decimal(18, 8)")]
		public decimal? LITTERPNT_COARSE_LENGTH { get; set; }

		[Column(TypeName = "decimal(18, 8)")]
		public decimal? LITTERPNT_COARSE_ANGLE { get; set; }

		public int LITTERPNT_COARSE_DECAY { get; set; }

		public string LITTERPNT_COMMENT { get; set; }

		public string LITTERPNT_DATE { get; set; }

	}
}
