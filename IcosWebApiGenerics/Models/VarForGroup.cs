using IcosWebApiGenerics.ControllerFactory;
using IcosWebApiGenerics.Models.BADM;
using System.Collections.Generic;

namespace IcosWebApiGenerics.Models
{
    [GeneratedController("api/varforgroup")]
    public class VarForGroup : BaseClass
    {
        public VarForGroup()
        {
            if (VList == null)
            {
                VList = new List<Variable>();
            }
        }
        //public int ID { get; set; }
        //public int GroupId { get; set; }
        //public int DataStatus { get; set; }
        public string Name { get; set; }
        public bool CanDuplicate { get; set; }
        public List<Variable> VList { get; set; }
    }
}
