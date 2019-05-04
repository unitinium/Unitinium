using System.Collections.Generic;

namespace Unitinium
{
    public class SceneNode
    {
        public string name { get; set; }
        public object[] components {get;set;}
        public List<SceneNode> childs { get; set; }

        public SceneNode()
        {
            childs = new List<SceneNode>();
        }
    }
}