using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IcosWebApiGenerics.Models.BADM
{
    public class Group
    {
        [Key]
        public int id_group { get; set; }
        public string GroupName { get; set; }
        public string Description { get; set; }
    }
}
