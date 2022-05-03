using IcosWebApiGenerics.ControllerFactory;
using IcosWebApiGenerics.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace IcosWebApiGenerics.Models.BADM
{
    [GeneratedController("api/bm")]
    public class GRP_BM : BaseClass
    {
		public GRP_BM() { GroupId = (int)Globals.Groups.GRP_BM; }

		public string BM_MODEL { get; set; }

		public string BM_SN { get; set; }

		public string BM_TYPE { get; set; }

		public string BM_VARIABLE_H_V_R { get; set; }

		[Column(TypeName = "decimal(18, 8)")]
		public decimal? BM_HEIGHT { get; set; }

		[Column(TypeName = "decimal(18, 8)")]
		public decimal? BM_EASTWARD_DIST { get; set; }

		[Column(TypeName = "decimal(18, 8)")]
		public decimal? BM_NORTHWARD_DIST { get; set; }

		[Column(TypeName = "decimal(18, 8)")]
		public decimal? BM_SAMPLING_INT { get; set; }

		public string BM_INST_HEAT { get; set; }

		public string BM_INST_SHIELDING { get; set; }

		public string BM_INST_ASPIRATION { get; set; }

		public int? BM_LOGGER { get; set; }

		public int? BM_FILE { get; set; }

		public string BM_DATE { get; set; }

		public string BM_DATE_START { get; set; }

		public string BM_DATE_END { get; set; }

		[Column(TypeName = "decimal(18, 8)")]
		public decimal? BM_DATE_UNC { get; set; }

		public string BM_COMMENT { get; set; }
		public string BM_NORTHREF { get; set; }
	}
}
