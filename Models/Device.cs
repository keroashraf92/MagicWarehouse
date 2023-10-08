using MagicWarehouse.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MagicWarehouse.Models
{
    public  class Device
    {
       
        public string IMEI { get; set; }
        public int DeviceTypeID { get; set; }
        public System.DateTime ReceivedDateProvider { get; set; }
        public string Status { get; set; }

    }

}