using IcosWebApiGenerics.ControllerFactory;
using IcosWebApiGenerics.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace IcosWebApiGenerics.Models.BADM
{
    [GeneratedController("api/gai")]
	public class GRP_GAI : BaseClass
	{
		public GRP_GAI() { GroupId = (int)Globals.Groups.GRP_GAI; }

		public string GAI_PLOT { get; set; }

		public string GAI_PLOT_TYPE { get; set; }

		public int? GAI_LOCATION { get; set; }

		[Column(TypeName = "decimal(18, 8)")]
		public decimal? GAI_AREA { get; set; }

		public string GAI_METHOD { get; set; }

		[Column(TypeName = "decimal(18, 8)")]
		public decimal? GAI { get; set; }

		[Column(TypeName = "decimal(18, 8)")]
		public decimal? GAI_HEIGHTC { get; set; }

		public string GAI_PHEN { get; set; }

		public string GAI_SPP { get; set; }

		public string GAI_PTYPE { get; set; }

		public int? GAI_DHP_ID { get; set; }

		[Column(TypeName = "decimal(18, 8)")]
		public decimal? GAI_DHP_SLOPE { get; set; }

		[Column(TypeName = "decimal(18, 8)")]
		public decimal? GAI_DHP_ASPECT { get; set; }

		public string GAI_COMMENT { get; set; }

		public string GAI_DATE { get; set; }

		[Column(TypeName = "decimal(18, 8)")]
		public decimal? GAI_DHP_EASTWARD_DIST { get; set; }

		[Column(TypeName = "decimal(18, 8)")]
		public decimal? GAI_DHP_NORTHWARD_DIST { get; set; }

		[Column(TypeName = "decimal(18, 8)")]
		public decimal? GAI_DHP_DISTANCE_POLAR { get; set; }

		[Column(TypeName = "decimal(18, 8)")]
		public decimal? GAI_DHP_ANGLE_POLAR { get; set; }

		public int? GAI_CEPT_ID { get; set; }

		[Column(TypeName = "decimal(18, 8)")]
		public decimal? GAI_CEPT_EASTWARD_DIST { get; set; }

		[Column(TypeName = "decimal(18, 8)")]
		public decimal? GAI_CEPT_NORTHWARD_DIST { get; set; }

		[Column(TypeName = "decimal(18, 8)")]
		public decimal? GAI_CEPT_DISTANCE_POLAR { get; set; }

		[Column(TypeName = "decimal(18, 8)")]
		public decimal? GAI_CEPT_ANGLE_POLAR { get; set; }

		[Column(TypeName = "decimal(18, 8)")]
		public decimal? GAI_LOCATION_HEIGHT { get; set; }



		/// <summary>
		/// Not for Web UI!!!!
		/// </summary>
		public int? GAI_DHP_SETUP_ID { get; set; }

		public string GAI_DHP_QC { get; set; }

		public string GAI_DHP_QC_EXT { get; set; }

		[Column(TypeName = "decimal(18, 8)")]
		public decimal? GAI_DHP_EFF { get; set; }

		[Column(TypeName = "decimal(18, 8)")]
		public decimal? GAI_DHP_THRESH { get; set; }

		[Column(TypeName = "decimal(18, 8)")]
		public decimal? GAI_DHP_CLUMP { get; set; }

		[Column(TypeName = "decimal(18, 8)")]
		public decimal? GAI_DHP_OVEREXP { get; set; }

		[Column(TypeName = "decimal(18, 8)")]
		public decimal? GAI_CEPT_LOCATION_MEAS { get; set; }

		public string GAI_CAMPAIGN { get; set; }

	}
}
