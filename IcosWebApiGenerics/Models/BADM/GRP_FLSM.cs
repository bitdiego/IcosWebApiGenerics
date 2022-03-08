using Castle.MicroKernel.SubSystems.Conversion;
using IcosWebApiGenerics.ControllerFactory;
using IcosWebApiGenerics.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace IcosWebApiGenerics.Models.BADM
{
	[GeneratedController("api/flsm")]
	public class GRP_FLSM : BaseClass
	{
		public GRP_FLSM()
		{
			GroupId = (int)Globals.Groups.GRP_FLSM;
		}

		public string FLSM_PLOT_ID { get; set; }

		public int? FLSM_TREE_ID { get; set; }

		public int FLSM_SAMPLE_ID { get; set; }

		public string FLSM_SPP { get; set; }

		[Column(TypeName = "decimal(18, 8)")]
		public decimal? FLSM_LMA_AREA { get; set; }

		[Column(TypeName = "decimal??(18, 8)")]
		public decimal? FLSM_LMA_DW { get; set; }
		public string FLSM_SAMPLE_TYPE { get; set; }
		public string FLSM_PTYPE { get; set; }

		public string FLSM_COMMENT { get; set; }

		public string FLSM_DATE { get; set; }

		/// <summary>
		/// The following, must NOT be submitted by BADM
		/// </summary>

		[Column(TypeName = "decimal(18, 8)")]
		public decimal? FLSM_CONC_CA { get; set; }

		[Column(TypeName = "decimal(18, 8)")]
		public decimal? FLSM_CONC_CU { get; set; }

		[Column(TypeName = "decimal(18, 8)")]
		public decimal? FLSM_CONC_FE { get; set; }

		[Column(TypeName = "decimal(18, 8)")]
		public decimal? FLSM_CONC_MG { get; set; }

		[Column(TypeName = "decimal(18, 8)")]
		public decimal? FLSM_CONC_MN { get; set; }

		[Column(TypeName = "decimal(18, 8)")]
		public decimal? FLSM_CONC_C { get; set; }

		[Column(TypeName = "decimal(18, 8)")]
		public decimal? FLSM_CONC_N { get; set; }

		[Column(TypeName = "decimal(18, 8)")]
		public decimal? FLSM_CONC_P { get; set; }

		[Column(TypeName = "decimal(18, 8)")]
		public decimal? FLSM_CONC_K { get; set; }

		[Column(TypeName = "decimal(18, 8)")]
		public decimal? FLSM_CONC_ZN { get; set; }

		[Column(TypeName = "decimal(18, 8)")]
		public decimal? FLSM_CONC_DRYRATIO { get; set; }

		public string FLSM_CONC_USRAVE_SN { get; set; }

		[Column(TypeName = "decimal(18, 8)")]
		public decimal? FLSM_CONC_CA_UNC { get; set; }

		[Column(TypeName = "decimal(18, 8)")]
		public decimal? FLSM_CONC_CU_UNC { get; set; }

		[Column(TypeName = "decimal(18, 8)")]
		public decimal? FLSM_CONC_FE_UNC { get; set; }

		[Column(TypeName = "decimal(18, 8)")]
		public decimal? FLSM_CONC_MG_UNC { get; set; }

		[Column(TypeName = "decimal(18, 8)")]
		public decimal? FLSM_CONC_MN_UNC { get; set; }

		[Column(TypeName = "decimal(18, 8)")]
		public decimal? FLSM_CONC_C_UNC { get; set; }

		[Column(TypeName = "decimal(18, 8)")]
		public decimal? FLSM_CONC_N_UNC { get; set; }

		[Column(TypeName = "decimal(18, 8)")]
		public decimal? FLSM_CONC_P_UNC { get; set; }

		[Column(TypeName = "decimal(18, 8)")]
		public decimal? FLSM_CONC_K_UNC { get; set; }

		[Column(TypeName = "decimal(18, 8)")]
		public decimal? FLSM_CONC_ZN_UNC { get; set; }

		[Column(TypeName = "decimal(18, 8)")]
		public decimal? FLSM_DRYRATIO_UNC { get; set; }

	}
}
