using IcosWebApiGenerics.ControllerFactory;
using IcosWebApiGenerics.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace IcosWebApiGenerics.Models.BADM
{
	[GeneratedController("api/allom")]
	public class GRP_ALLOM : BaseClass
	{
		public GRP_ALLOM() { GroupId = (int)Globals.Groups.GRP_ALLOM; }

		[Column(TypeName = "decimal(18, 8)")]
		public decimal ALLOM_DBH { get; set; } = -9999;

		[Column(TypeName = "decimal(18, 8)")]
		public decimal ALLOM_HEIGHT { get; set; } = -9999;

		public string ALLOM_SPP { get; set; }

		[Column(TypeName = "decimal(18, 8)")]
		public decimal ALLOM_STEM_BIOM { get; set; } = -9999;

		[Column(TypeName = "decimal(18, 8)")]
		public decimal ALLOM_BRANCHES_BIOM { get; set; } = -9999;

		[Column(TypeName = "decimal(18, 8)")]
		public decimal ALLOM_LEAVES_BIOM { get; set; }

		[Column(TypeName = "decimal(18, 8)")]
		public decimal ALLOM_NDLE_CONUM { get; set; }

		public string ALLOM_COMMENT { get; set; }

		public string ALLOM_DATE { get; set; }

	}
}
