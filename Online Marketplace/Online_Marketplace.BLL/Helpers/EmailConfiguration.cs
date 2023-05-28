using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Online_Marketplace.BLL.Helpers
{

    /*    public class EmailConfiguration
        {
            public string From { get; set; }
            public string SmtpServer { get; set; }
            public int Port { get; set; }
            public string UserName { get; set; }
            public string Password { get; set; }
        }*/
    public class EmailSettings
    {
        public string ApiKey { get; set; }
        public string SenderEmail { get; set; }
        public string SenderName { get; set; }

      
    }




}
