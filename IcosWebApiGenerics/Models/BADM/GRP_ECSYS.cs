using IcosWebApiGenerics.ControllerFactory;
using IcosWebApiGenerics.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace IcosWebApiGenerics.Models.BADM
{
    [GeneratedController("api/ecsys")]
    public class GRP_ECSYS : BaseClass
    {
		public GRP_ECSYS() { GroupId = (int)Globals.Groups.GRP_ECSYS; }

		public string ECSYS_GA_MODEL { get; set; }

		public string ECSYS_GA_SN { get; set; }

		public string ECSYS_SA_MODEL { get; set; }

		public string ECSYS_SA_SN { get; set; }

		[Column(TypeName = "decimal(18, 8)")]
		public decimal ECSYS_SEP_VERT { get; set; }

		[Column(TypeName = "decimal(18, 8)")]
		public decimal ECSYS_SEP_EASTWARD { get; set; }

		[Column(TypeName = "decimal(18, 8)")]
		public decimal ECSYS_SEP_NORTHWARD { get; set; }

		public string ECSYS_DATE { get; set; }

		public string ECSYS_COMMENT { get; set; }
	}
}
