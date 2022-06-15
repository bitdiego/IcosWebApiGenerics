using System.ComponentModel.DataAnnotations.Schema;

namespace IcosWebApiGenerics.Models
{
    public class Variable
    {
        [Column("id_variable", TypeName = "int")]
        public int ID { get; set; }
        public string Name { get; set; }

        [Column("required", TypeName = "bool")]
        public bool Required { get; set; }

        [Column("varIndex", TypeName = "bool")]
        public string VarIndex { get; set; }
        [NotMapped]
        public string Value { get; set; }

        [Column("cv_index", TypeName = "int")]
        public int? CvIndex { get; set; }

        [Column("hasMultiple", TypeName = "bool")]
        public bool HasMultiple { get; set; }

        [Column("unit_type", TypeName = "smallint")]
        public int Unit { get; set; }

        [Column("unit_of_measure", TypeName = "string")]
        public string UnitOfMeasure { get; set; }

        [Column("group_id", TypeName = "int")]
        public int GroupId { get; set; }
        [NotMapped]
        public int DataStatus { get; set; }

        [NotMapped] 
        public int Row { get; set; }

        [NotMapped]
        public string Cell { get; set; }
    }
}
