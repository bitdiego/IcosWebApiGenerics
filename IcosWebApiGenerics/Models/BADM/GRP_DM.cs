using IcosWebApiGenerics.Utils;
using System.ComponentModel.DataAnnotations.Schema;

namespace IcosWebApiGenerics.Models.BADM
{
	public class GRP_DM : BaseClass
	{
		public GRP_DM()
		{
			GroupId = (int)Globals.Groups.GRP_DM;
		}

		public string DM_AGRICULTURE { get; set; }

		public string DM_ENCROACH { get; set; }

		public string DM_EXT_WEATHER { get; set; }

		public string DM_FERT_M { get; set; }

		public string DM_FERT_O { get; set; }

		public string DM_FIRE { get; set; }

		public string DM_FORESTRY { get; set; }

		public string DM_GRAZE { get; set; }

		public string DM_INS_PATH { get; set; }

		public string DM_PESTICIDE { get; set; }

		public string DM_PLANTING { get; set; }

		public string DM_TILL { get; set; }

		public string DM_WATER { get; set; }

		public string DM_GENERAL { get; set; }

		[Column(TypeName = "decimal(18, 8)")]
		public decimal? DM_SURF { get; set; }

		[Column(TypeName = "decimal(18, 8)")]
		public decimal? DM_SURF_MEAS_PRECISION { get; set; }

		public string DM_DATE { get; set; }

		public string DM_DATE_START { get; set; }

		public string DM_DATE_END { get; set; }

		[Column(TypeName = "decimal(18, 8)")]
		public decimal? DM_DATE_UNC { get; set; }

		public string DM_COMMENT { get; set; }

	}
}
