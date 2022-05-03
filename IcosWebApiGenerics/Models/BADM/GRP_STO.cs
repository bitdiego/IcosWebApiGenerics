using IcosWebApiGenerics.ControllerFactory;
using IcosWebApiGenerics.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace IcosWebApiGenerics.Models.BADM
{
    [GeneratedController("api/sto")]
    public class GRP_STO : BaseClass
    {
		public GRP_STO() { GroupId = (int)Globals.Groups.GRP_STO; }

		public string STO_CONFIG { get; set; }

		public string STO_TYPE { get; set; }

		public int? STO_PROF_ID { get; set; }

		public int? STO_PROF_LEVEL { get; set; }

		[Column(TypeName = "decimal(18, 8)")]
		public decimal? STO_PROF_HEIGHT { get; set; }

		[Column(TypeName = "decimal(18, 8)")]
		public decimal? STO_PROF_EASTWARD_DIST { get; set; }

		[Column(TypeName = "decimal(18, 8)")]
		public decimal? STO_PROF_NORTHWARD_DIST { get; set; }

		public int? STO_PROF_HORIZ_S_POINTS { get; set; }

		[Column(TypeName = "decimal(18, 8)")]
		public decimal? STO_PROF_BUFFER_VOL { get; set; }

		[Column(TypeName = "decimal(18, 8)")]
		public decimal? STO_PROF_BUFFER_FLOWRATE { get; set; }

		[Column(TypeName = "decimal(18, 8)")]
		public decimal? STO_PROF_TUBE_LENGTH { get; set; }

		[Column(TypeName = "decimal(18, 8)")]
		public decimal? STO_PROF_TUBE_DIAM { get; set; }
		public string STO_PROF_TUBE_MAT { get; set; }

		public string STO_PROF_TUBE_THERM { get; set; }

		[Column(TypeName = "decimal(18, 8)")]
		public decimal? STO_PROF_SAMPLING_TIME { get; set; }

		public string STO_GA_MODEL { get; set; }

		public string STO_GA_SN { get; set; }

		public string STO_GA_VARIABLE { get; set; }

		public int? STO_GA_PROF_ID { get; set; }

		[Column(TypeName = "decimal(18, 8)")]
		public decimal? STO_GA_FLOW_RATE { get; set; }

		[Column(TypeName = "decimal(18, 8)")]
		public decimal? STO_GA_SAMPLING_INT { get; set; }

		[Column(TypeName = "decimal(18, 8)")]
		public decimal? STO_GA_AZIM_MOUNT { get; set; }

		[Column(TypeName = "decimal(18, 8)")]
		public decimal? STO_GA_VERT_MOUNT { get; set; }

		[Column(TypeName = "decimal(18, 8)")]
		public decimal? STO_GA_TUBE_LENGTH { get; set; }

		[Column(TypeName = "decimal(18, 8)")]
		public decimal? STO_GA_TUBE_DIAM { get; set; }

		public string STO_GA_TUBE_MAT { get; set; }

		public string STO_GA_TUBE_THERM { get; set; }

		public string STO_GA_CAL_VARIABLE { get; set; }

		[Column(TypeName = "decimal(18, 8)")]
		public decimal? STO_GA_CAL_VALUE { get; set; }

		[Column(TypeName = "decimal(18, 8)")]
		public decimal? STO_GA_CAL_REF { get; set; }

		[Column(TypeName = "decimal(18, 8)")]
		public decimal? STO_GA_CAL_TA { get; set; }

		public int? STO_LOGGER { get; set; }

		public int? STO_FILE { get; set; }

		public string STO_DATE { get; set; }

		public string STO_DATE_START { get; set; }

		public string STO_DATE_END { get; set; }

		[Column(TypeName = "decimal(18, 8)")]
		public decimal? STO_DATE_UNC { get; set; }

		public string STO_COMMENT { get; set; }
	}
}
