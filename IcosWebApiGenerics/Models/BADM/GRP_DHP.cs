using IcosWebApiGenerics.ControllerFactory;
using IcosWebApiGenerics.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IcosWebApiGenerics.Models.BADM
{
	[GeneratedController("api/dhp")]
	public class GRP_DHP : BaseClass
	{
		public GRP_DHP() { GroupId = (int)Globals.Groups.GRP_DHP; }

		public int DHP_ID { get; set; }

		public string DHP_CAMERA { get; set; }

		public string DHP_CAMERA_SN { get; set; }

		public string DHP_LENS { get; set; }

		public string DHP_LENS_SN { get; set; }

		public int DHP_OC_ROW { get; set; }

		public int DHP_OC_COL { get; set; }

		public int DHP_RADIUS { get; set; }

		public string DHP_COMMENT { get; set; }

		public string DHP_DATE { get; set; }

	}
}
