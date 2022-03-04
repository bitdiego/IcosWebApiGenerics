using IcosWebApiGenerics.ControllerFactory;
using IcosWebApiGenerics.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace IcosWebApiGenerics.Models.BADM
{
	[GeneratedController("api/climateavg")]
	public class GRP_CLIM_AVG : BaseClass
	{
		public GRP_CLIM_AVG() { GroupId = (int)Globals.Groups.GRP_CLIM_AVG; }

		[Column(TypeName = "decimal(18, 8)")]
		public decimal? MAT { get; set; }

		[Column(TypeName = "decimal(18, 8)")]
		public decimal? MAP { get; set; }

		[Column(TypeName = "decimal(18, 8)")]
		public decimal? MAR { get; set; }

		[Column(TypeName = "decimal(18, 8)")]
		public decimal? MAC_YEARS { get; set; }

		public string MAC_DATE { get; set; }

		public string MAC_COMMENTS { get; set; }

		[Column(TypeName = "decimal(18, 8)")]
		public decimal? MAS { get; set; }

		public string CLIMATE_KOEPPEN { get; set; }

	}
}
