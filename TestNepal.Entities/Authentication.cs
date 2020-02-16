namespace TestNepal.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Authentication")]
    public class Authentication
    {
        public int ID { get; set; }

        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(250)]
        public string DisplayText { get; set; }

        public bool? IsActive { get; set; }
    }
}
