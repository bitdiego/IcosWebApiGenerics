using IcosWebApiGenerics.ControllerFactory;
using IcosWebApiGenerics.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace IcosWebApiGenerics.Models.BADM
{
    [GeneratedController("api/bulkh")]
	public class GRP_BULKH : BaseClass
	{
		public GRP_BULKH() { GroupId = (int)Globals.Groups.GRP_BULKH; }

		public string BULKH_PLOT { get; set; }

		public string BULKH_PLOT_TYPE { get; set; }

		[Column(TypeName = "decimal(18, 8)")]
		public decimal BULKH { get; set; }

		public string BULKH_COMMENT { get; set; }

		public string BULKH_DATE { get; set; }

	}
}
