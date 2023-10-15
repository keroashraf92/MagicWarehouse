using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MagicWarehouse.Models
{

    public class validationModel
    {
        [Key]
        public int ID { get; set; }

        [RegularExpression(@"^[0-9]*$", ErrorMessage = "ID must contain only digits.")]


        public string IMEI { get; set; }

        [Required(ErrorMessage = "IMEI is required")]
        [StringLength(15, MinimumLength = 15, ErrorMessage = "IMEI must be exactly 15 characters")]

        [RegularExpression(@"^[0-9]*$", ErrorMessage = "DeviceTypeID must contain only digits.")]
        public int DeviceTypeID { get; set; }

        [RegularExpression(@"^[0-9]*$", ErrorMessage = "Status must contain only digits.")]
        public int Status { get; set; }

        
        public int ReceivedDataProvider { get; set; }

        // Other properties
    }
}