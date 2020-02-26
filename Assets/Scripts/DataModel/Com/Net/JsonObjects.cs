using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gululu
{
    public class MissionData
    {
        public List<Star> Stars { get; set; }
    }

    public class Star
    {
        public int ID { get; set; }
        public string Unlock { get; set; }
        public List<Scene> Scenes { get; set; }
    }

    public class Scene
    {
        public int Index { get; set; }
        public string Unlock { get; set; }
    }
}
