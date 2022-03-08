using IcosWebApiGenerics.ControllerFactory;
using IcosWebApiGenerics.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace IcosWebApiGenerics.Models.BADM
{
	[GeneratedController("api/sosm")]
	public class GRP_SOSM : BaseClass
	{
		public GRP_SOSM() { GroupId = (int)Globals.Groups.GRP_SOSM; }

		public string SOSM_PLOT_ID { get; set; }

		public string SOSM_SAMPLE_ID { get; set; }

		public string SOSM_SAMPLE_MAT { get; set; }

		[Column(TypeName = "decimal(18, 8)")]
		public decimal? SOSM_AREA { get; set; }

		[Column(TypeName = "decimal(18, 8)")]
		public decimal? SOSM_VOLUME { get; set; }


		[Column(TypeName = "decimal(18, 8)")]
		public decimal? SOSM_UD { get; set; }

		[Column(TypeName = "decimal(18, 8)")]
		public decimal? SOSM_LD { get; set; }

		[Column(TypeName = "decimal(18, 8)")]
		public decimal? SOSM_THICKNESS { get; set; }

		[Column(TypeName = "decimal(18, 8)")]
		public decimal? SOSM_W0 { get; set; }

		[Column(TypeName = "decimal(18, 8)")]
		public decimal? SOSM_W30 { get; set; }

		[Column(TypeName = "decimal(18, 8)")]
		public decimal? SOSM_W30_E { get; set; }

		[Column(TypeName = "decimal(18, 8)")]
		public decimal? SOSM_W105_S { get; set; }

		[Column(TypeName = "decimal(18, 8)")]
		public decimal? SOSM_W70_R { get; set; }

		[Column(TypeName = "decimal(18, 8)")]
		public decimal? SOSM_WX30_E { get; set; }

		[Column(TypeName = "decimal(18, 8)")]
		public decimal? SOSM_WX105_E { get; set; }

		public string SOSM_COMMENT { get; set; }

		public string SOSM_DATE { get; set; }

		[Column(TypeName = "decimal(18, 8)")]
		public decimal? SOSM_W0_OH { get; set; }

		[Column(TypeName = "decimal(18, 8)")]
		public decimal? SOSM_W0_OS { get; set; }

		[Column(TypeName = "decimal(18, 8)")]
		public decimal? SOSM_W0_OR { get; set; }

		[Column(TypeName = "decimal(18, 8)")]
		public decimal? SOSM_CONC_C { get; set; }

		public string SOSM_CONC_C_METHOD { get; set; }

		[Column(TypeName = "decimal(18, 8)")]
		public decimal? SOSM_CONC_N { get; set; }

		public string SOSM_CONC_N_METHOD { get; set; }

		public string SOSM_SAMPLE_CEES_ID { get; set; }
		public decimal? SOSM_BD { get; set; }
		public decimal? SOSM_TEX_ROCK { get; set; }
		public decimal? SOSM_CN_RATIO { get; set; }
		public decimal? SOSM_STOCK_C { get; set; }
		public decimal? SOSM_STOCK_N { get; set; }

	}
}
