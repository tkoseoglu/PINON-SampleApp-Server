using System;
using System.ComponentModel.DataAnnotations;

namespace PINON.SampleApp.Data.Models
{
    public abstract class Common
    {
        [Key]
        public int Id { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public string ModifiedBy { get; set; }

        public DateTime ModifiedOn { get; set; }

        public bool IsDeleted { get; set; }
    }
}