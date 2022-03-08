using IcosWebApiGenerics.ControllerFactory;
using IcosWebApiGenerics.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace IcosWebApiGenerics.Models.BADM
{
	[GeneratedController("api/tree")]
	public class GRP_TREE : BaseClass
	{
		public GRP_TREE() { GroupId = (int)Globals.Groups.GRP_TREE; }

		public string TREE_PLOT { get; set; }

		public int? TREE_ID { get; set; }

		[Column(TypeName = "decimal(18, 8)")]
		public decimal? TREE_EASTWARD_DIST { get; set; }

		[Column(TypeName = "decimal(18, 8)")]
		public decimal? TREE_NORTHWARD_DIST { get; set; }

		[Column(TypeName = "decimal(18, 8)")]
		public decimal? TREE_DISTANCE_POLAR { get; set; }

		[Column(TypeName = "decimal(18, 8)")]
		public decimal? TREE_ANGLE_POLAR { get; set; }

		public string TREE_VARMAP { get; set; }

		[Column(TypeName = "decimal(18, 8)")]
		public decimal? TREE_DBH { get; set; }

		public int? TREE_STUMP { get; set; }

		[Column(TypeName = "decimal(18, 8)")]
		public decimal? TREE_HEIGHT { get; set; }

		public string TREE_SPP { get; set; }

		public string TREE_STATUS { get; set; }

		public string TREE_COMMENT { get; set; }

		public string TREE_DATE { get; set; }

	}
}
