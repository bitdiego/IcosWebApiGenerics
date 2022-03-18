using IcosWebApiGenerics.ControllerFactory;
using IcosWebApiGenerics.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace IcosWebApiGenerics.Models.BADM
{
    [GeneratedController("api/eddycov")]
    public class GRP_EC : BaseClass
    {
        public GRP_EC() { GroupId = (int)Globals.Groups.GRP_EC; }

        public string EC_MODEL { get; set; }

        public string EC_SN { get; set; }

        public string EC_TYPE { get; set; }

        [Column(TypeName = "decimal(18, 8)")]
        public decimal? EC_HEIGHT { get; set; }

        [Column(TypeName = "decimal(18, 8)")]
        public decimal? EC_EASTWARD_DIST { get; set; }

        [Column(TypeName = "decimal(18, 8)")]
        public decimal? EC_NORTHWARD_DIST { get; set; }

        [Column(TypeName = "decimal(18, 8)")]
        public decimal? EC_SAMPLING_INT { get; set; }

        public string EC_SA_HEAT { get; set; }

        [Column(TypeName = "decimal(18, 8)")]
        public decimal? EC_SA_OFFSET_N { get; set; }

        public string EC_SA_WIND_FORMAT { get; set; }

        public string EC_SA_GILL_ALIGN { get; set; }

        public string EC_SA_GILL_PCI { get; set; }

        [Column(TypeName = "decimal(18, 8)")]
        public decimal? EC_GA_FLOW_RATE { get; set; }

        public string EC_GA_LICOR_FM_SN { get; set; }

        public string EC_GA_LICOR_TP_SN { get; set; }

        public string EC_GA_LICOR_AIU_SN { get; set; }

        [Column(TypeName = "decimal(18, 8)")]
        public decimal? EC_GA_CAL_CO2_ZERO { get; set; }

        [Column(TypeName = "decimal(18, 8)")]
        public decimal? EC_GA_CAL_CO2_OFFSET { get; set; }

        [Column(TypeName = "decimal(18, 8)")]
        public decimal? EC_GA_CAL_CO2_REF { get; set; }

        [Column(TypeName = "decimal(18, 8)")]
        public decimal? EC_GA_CAL_H2O_ZERO { get; set; }

        [Column(TypeName = "decimal(18, 8)")]
        public decimal? EC_GA_CAL_TA { get; set; }

        public int? EC_LOGGER { get; set; }

        public int? EC_FILE { get; set; }

        public string EC_DATE { get; set; }

        public string EC_DATE_START { get; set; }

        public string EC_DATE_END { get; set; }

        [Column(TypeName = "decimal(18, 8)")]
        public decimal? EC_DATE_UNC { get; set; }

        public string EC_COMMENT { get; set; }

        [Column(TypeName = "decimal(18, 8)")]
        public decimal? EC_SA_NORTH_MAGDEC { get; set; }
        public string EC_NORTHREF { get; set; }

    }
}
