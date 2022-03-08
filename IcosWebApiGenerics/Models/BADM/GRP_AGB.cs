using IcosWebApiGenerics.ControllerFactory;
using IcosWebApiGenerics.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace IcosWebApiGenerics.Models.BADM
{
	[GeneratedController("api/agb")]
	public class GRP_AGB : BaseClass
	{
		public GRP_AGB()
		{
			GroupId = (int)Globals.Groups.GRP_AGB;
		}

		public string AGB_PLOT { get; set; }

		public string AGB_PLOT_TYPE { get; set; }

		public int? AGB_LOCATION { get; set; }

		[Column(TypeName = "decimal(18, 8)")]
		public decimal? AGB_LOCATION_HEIGHT { get; set; }

		[Column(TypeName = "decimal(18, 8)")]
		public decimal? AGB_AREA { get; set; }

		[Column(TypeName = "decimal(18, 8)")]
		public decimal? AGB { get; set; }

		[Column(TypeName = "decimal(18, 8)")]
		public decimal? AGB_HEIGHTC { get; set; }

		[Column(TypeName = "decimal(18, 8)")]
		public decimal? AGB_NPP_MOSS { get; set; }

		public string AGB_PHEN { get; set; }

		public string AGB_SPP { get; set; }

		public string AGB_PTYPE { get; set; }

		public string AGB_ORGAN { get; set; }

		public string AGB_COMMENT { get; set; }

		public string AGB_DATE { get; set; }


	}
}
