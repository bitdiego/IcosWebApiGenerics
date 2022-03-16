using IcosWebApiGenerics.ControllerFactory;
using IcosWebApiGenerics.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace IcosWebApiGenerics.Models.BADM
{
    [GeneratedController("api/cept")]
	public class GRP_CEPT : BaseClass
	{
		public GRP_CEPT() { GroupId = (int)Globals.Groups.GRP_CEPT; }

		[Column(TypeName = "decimal(18, 8)")]
		public decimal? CEPT_ELADP { get; set; }

		[Column(TypeName = "decimal(18, 8)")]
		public decimal? CEPT_ABSORP { get; set; }

		public int? CEPT_FIRST { get; set; }

		public int? CEPT_LAST { get; set; }

		public string CEPT_COMMENT { get; set; }

		public string CEPT_DATE { get; set; }

	}
}
