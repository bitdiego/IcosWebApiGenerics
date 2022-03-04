using IcosWebApiGenerics.ControllerFactory;
using IcosWebApiGenerics.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace IcosWebApiGenerics.Models.BADM
{
	[GeneratedController("api/tower")]
	public class GRP_TOWER : BaseClass
	{
		public GRP_TOWER() { GroupId = (int)Globals.Groups.GRP_TOWER; }

		public string TOWER_TYPE { get; set; }

		[Column(TypeName = "decimal(18, 8)")]
		public decimal? TOWER_HEIGHT { get; set; }

		public string TOWER_ACCESS { get; set; }

		public string TOWER_POWER { get; set; }

		[Column(TypeName = "decimal(18, 8)")]
		public decimal? TOWER_POWER_AVAIL { get; set; }

		public string TOWER_DATATRAN { get; set; }

		[Column(TypeName = "decimal(18, 8)")]
		public decimal? TOWER_DATATRAN_SPEED { get; set; }

		public string TOWER_DATE { get; set; }

		public string TOWER_COMMENT { get; set; }

	}
}
