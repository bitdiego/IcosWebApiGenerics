using IcosWebApiGenerics.ControllerFactory;
using IcosWebApiGenerics.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IcosWebApiGenerics.Models.BADM
{
    [GeneratedController("api/dsnow")]
    public class GRP_FILE : BaseClass
    {
		public GRP_FILE()
		{
			GroupId = (int)Globals.Groups.GRP_FILE;
		}

		public int FILE_ID { get; set; }

		public int FILE_LOGGER_ID { get; set; }

		public string FILE_FORMAT { get; set; }

		public string FILE_TYPE { get; set; }

		public int? FILE_HEAD_NUM { get; set; }

		public int? FILE_HEAD_VARS { get; set; }

		public int? FILE_HEAD_TYPE { get; set; }

		public string FILE_EPOCH { get; set; }

		public string FILE_DATE { get; set; }

		public string FILE_COMMENTS { get; set; }

		public string FILE_EXTENSION { get; set; }

		public string FILE_MISSING_VALUE { get; set; }

		public string FILE_TIMESTAMP { get; set; }

		public string FILE_COMPRESS { get; set; }
	}
}
