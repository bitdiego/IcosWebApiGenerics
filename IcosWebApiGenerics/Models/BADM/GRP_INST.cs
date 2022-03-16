using IcosWebApiGenerics.ControllerFactory;
using IcosWebApiGenerics.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IcosWebApiGenerics.Models.BADM
{
	[GeneratedController("api/instruments")]
	public class GRP_INST : BaseClass
	{
		public GRP_INST()
		{
			GroupId = (int)Globals.Groups.GRP_INST;
		}
		public string INST_MODEL { get; set; }

		public string INST_SN { get; set; }

		public string INST_FIRMWARE { get; set; }

		public string INST_FACTORY { get; set; }

		public string INST_CALIB_FUNC { get; set; }

		public string INST_COMMENT { get; set; }

		public string INST_DATE { get; set; }

	}
}
