using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IcosWebApiGenerics.Models.BADM
{
    public class BADMList
    {
        [Key]
        public int id_badmlist { get; set; }

        public string vocabulary { get; set; }

        public string shortname { get; set; }

        public string description { get; set; }

        public int cv_index { get; set; }
    }
}
