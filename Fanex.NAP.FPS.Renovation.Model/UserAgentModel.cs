using System.Xml.Serialization;

namespace Fanex.NAP.FPS.Renovation.Model
{    
    public class UserAgentModel
    {  
        public string UserAgent { get; set; }
 
        public int OSId { get; set; }
 
        public int BrowserId { get; set; }
 
        public string OSName { get; set; }
 
        public string BrowserName { get; set; }

        public long UserAgentId { get; set; }
    }
}
