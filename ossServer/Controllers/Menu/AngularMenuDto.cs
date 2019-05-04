using System.Collections.Generic;

namespace ossServer.Controllers.Menu
{
    public class AngularMenuDto
    {
        public string Title { get; set; }
        public string RouterLink { get; set; }
        public bool Enabled { get; set; }

        public List<AngularMenuDto> Sub { get; set; }
    }
}
