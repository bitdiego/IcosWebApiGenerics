using IcosWebApiGenerics.ControllerFactory;
using IcosWebApiGenerics.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IcosWebApiGenerics.Models.BADM
{
	[GeneratedController("api/landownership")]
	public class GRP_LAND_OWNERSHIP : BaseClass
	{
		public GRP_LAND_OWNERSHIP() { GroupId = (int)Globals.Groups.GRP_LAND_OWNERSHIP; }

		public string LAND_OWNERSHIP { get; set; }

		public string LAND_OWNER { get; set; }

		public string LAND_DATE { get; set; }

		public string LAND_COMMENT { get; set; }

	}
}
