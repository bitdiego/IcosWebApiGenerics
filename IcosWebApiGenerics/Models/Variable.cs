namespace IcosWebApi.Models
{
    public class Variable
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public bool Required { get; set; }
        public string VarIndex { get; set; }
        public string Value { get; set; }
        public int CvIndex { get; set; }
        public bool HasMultiple { get; set; }
        public int Unit { get; set; }
        public string UnitOfMeasure { get; set; }
        public int GroupId { get; set; }
        public int DataStatus { get; set; }
        public int Row { get; set; }
        public string Cell { get; set; }
    }
}
