using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace IcosWebApiGenerics.Models.BADM
{
    public class BaseClass
    {
        public BaseClass()
        {

        }

        public int Id { get; set; }

        public int DataStatus { get; set; }

        public int InsertUserId { get; set; }

        public DateTime InsertDate { get; set; }

        public int? DeleteUserId { get; set; }

        public DateTime? DeletedDate { get; set; }

        public int SiteId { get; set; }
        public int? DataOrigin { get; set; }

        [NotMapped]
        public int GroupId { get; set; }

        [NotMapped]
        public static List<string> Differences { get; set; }

        public static bool operator ==(BaseClass obj1, BaseClass obj2)
        {
            if (ReferenceEquals(obj1, obj2))
            {
                return true;
            }
            if (ReferenceEquals(obj1, null))
            {
                return false;
            }
            if (ReferenceEquals(obj2, null))
            {
                return false;
            }

            return obj1.Equals(obj2);
        }

        public static bool operator !=(BaseClass obj1, BaseClass obj2)
        {
            return !(obj1 == obj2);
        }

        public bool Equals(BaseClass other)
        {
            bool res = true;
            if (Differences == null) Differences = new List<string>();
            if (ReferenceEquals(other, null))
            {
                return false;
            }
            if (ReferenceEquals(this, other))
            {
                return true;
            }
            foreach (var prop in other.GetType().GetProperties())
            {
                if (prop.Name == "Id" || prop.Name == "DataStatus" || prop.Name == "InsertUserId"
                    || prop.Name == "InsertDate" || prop.Name == "DeletedDate" || prop.Name == "DeleteUserId" || prop.Name == "Differences") continue;
                var val1 = prop.GetValue(other, null);
                var val2 = this.GetType().GetProperty(prop.Name).GetValue(this, null);
                bool bTemp= Object.Equals(val1, val2); ;
                res = res && bTemp;
                if (!bTemp)
                {
                    Differences.Add(prop.Name);
                }
            }
            return res;
        }

        public void CopyVals(BaseClass other)
        {
            foreach (var prop in other.GetType().GetProperties())
            {
                if (prop.Name == "Id" || prop.Name == "DataStatus" || prop.Name == "InsertUserId"
                    || prop.Name == "InsertDate" || prop.Name == "DeletedDate" || prop.Name == "DeleteUserId" || prop.Name == "Differences") continue;
                var val1 = prop.GetValue(other, null);
                var val2 = this.GetType().GetProperty(prop.Name).GetValue(this, null);
                if (val1 != null && val2 == null)
                {
                    prop.SetValue(this, val1);
                }
            }
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as BaseClass);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = Id.GetHashCode();
                hashCode = (hashCode * 397) ^ SiteId.GetHashCode();
                return hashCode;
            }
        }

    }
}
