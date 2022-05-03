using IcosWebApiGenerics.ControllerFactory;
using IcosWebApiGenerics.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace IcosWebApiGenerics.Models.BADM
{
    [GeneratedController("api/ecwexcl")]
    public class GRP_ECWEXCL : BaseClass
    {
        public GRP_ECWEXCL()
        {
            GroupId = (int)Globals.Groups.GRP_ECWEXCL;
        }
        [Column(TypeName = "decimal(18, 8)")]
        public decimal ECWEXCL { get; set; }

        [Column(TypeName = "decimal(18, 8)")]
        public decimal ECWEXCL_RANGE { get; set; }

        public string ECWEXCL_ACTION { get; set; }

        public string ECWEXCL_DATE { get; set; }

        public string ECWEXCL_COMMENT { get; set; }
        public string ECWEXCL_NORTHREF { get; set; }
    }
}
