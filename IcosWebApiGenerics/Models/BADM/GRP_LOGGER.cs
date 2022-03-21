using IcosWebApiGenerics.ControllerFactory;
using IcosWebApiGenerics.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IcosWebApiGenerics.Models.BADM
{
	[GeneratedController("api/logger")]
	public class GRP_LOGGER : BaseClass
	{
		public GRP_LOGGER() 
		{ 
			GroupId = (int)Globals.Groups.GRP_LOGGER; 
		}

		public string LOGGER_MODEL { get; set; }

		public string LOGGER_SN { get; set; }

		public int LOGGER_ID { get; set; }

		public string LOGGER_DATE { get; set; }

		public string LOGGER_COMMENTS { get; set; }

	}
}
