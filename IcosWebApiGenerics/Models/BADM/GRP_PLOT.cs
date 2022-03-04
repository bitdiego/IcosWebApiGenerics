using IcosWebApiGenerics.ControllerFactory;
using IcosWebApiGenerics.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IcosWebApiGenerics.Models.BADM
{
	[GeneratedController("api/samplingscheme")]
	public class GRP_PLOT : BaseClass
	{
		public GRP_PLOT()
		{
			GroupId = (int)Globals.Groups.GRP_PLOT;
		}

		public string PLOT_ID { get; set; }

		public string PLOT_TYPE { get; set; }
		[DisplayFormat(DataFormatString = "{0:0.########}", ApplyFormatInEditMode = true)]
		public decimal? PLOT_EASTWARD_DIST { get; set; }
		[DisplayFormat(DataFormatString = "{0:0.########}", ApplyFormatInEditMode = true)]
		public decimal? PLOT_NORTHWARD_DIST { get; set; }
		[DisplayFormat(DataFormatString = "{0:0.########}", ApplyFormatInEditMode = true)]
		public decimal? PLOT_DISTANCE_POLAR { get; set; }
		[DisplayFormat(DataFormatString = "{0:0.########}", ApplyFormatInEditMode = true)]
		public decimal? PLOT_ANGLE_POLAR { get; set; }

		public string PLOT_REFERENCE_POINT { get; set; }

		[DisplayFormat(DataFormatString = "{0:0.########}", ApplyFormatInEditMode = true)]
		public decimal? PLOT_LOCATION_LAT { get; set; }

		[DisplayFormat(DataFormatString = "{0:0.########}", ApplyFormatInEditMode = true)]
		public decimal? PLOT_LOCATION_LONG { get; set; }

		public string PLOT_COMMENT { get; set; }

		public string PLOT_DATE { get; set; }
		[DisplayFormat(DataFormatString = "{0:0.########}", ApplyFormatInEditMode = true)]
		public decimal? PLOT_RECT_X { get; set; }
		[DisplayFormat(DataFormatString = "{0:0.########}", ApplyFormatInEditMode = true)]
		public decimal? PLOT_RECT_Y { get; set; }
		[DisplayFormat(DataFormatString = "{0:0.########}", ApplyFormatInEditMode = true)]
		public decimal? PLOT_RECT_DIR { get; set; }
		[DisplayFormat(DataFormatString = "{0:0.########}", ApplyFormatInEditMode = true)]
		public decimal? PLOT_HEIGHT { get; set; }

        public string PLOT_NORTHREF { get; set; }

    }
}
