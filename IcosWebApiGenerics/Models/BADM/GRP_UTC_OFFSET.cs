using IcosWebApiGenerics.ControllerFactory;
using IcosWebApiGenerics.Utils;
using System.ComponentModel.DataAnnotations.Schema;

namespace IcosWebApiGenerics.Models.BADM
{
	[GeneratedController("api/utcoffset")]
	public class GRP_UTC_OFFSET : BaseClass
	{
		public GRP_UTC_OFFSET() { GroupId = (int)Globals.Groups.GRP_UTC_OFFSET; }

		[Column(TypeName = "decimal(18, 8)")]
		public decimal UTC_OFFSET { get; set; }

		public string UTC_OFFSET_DATE_START { get; set; }

		public string UTC_OFFSET_COMMENT { get; set; }

	}
}
