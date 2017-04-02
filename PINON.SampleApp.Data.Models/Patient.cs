namespace PINON.SampleApp.Data.Models
{
    public class Patient : Common
    {
        public string Weight { get; set; }

        public string Height { get; set; }

        public string HairColor { get; set; }

        public string EyeColor { get; set; }

        public string UserAccountId { get; set; }

        public int HospitalId { get; set; }
        public virtual Hospital Hospital { get; set; }
    }
}