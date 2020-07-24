using System;
using System.Collections.Generic;
using System.Text;

namespace albion_avalon
{
    public class albionzonedefinition
    {
        public string zonename { get; set; }//name of the zone
        public List<albionportaldefinition> connectedzones { get; set; }//list containing one item for each portal in the zone
    }
    public class albionportaldefinition
    {
        public string connectedzone { get; set; }//the name of the zone the portal leads to
        public double minutestilldecay { get; set; }//how many more minutes the portal is going to exist before it despawns/gets replaced
    }
}
