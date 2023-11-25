using Model.Graph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphEditor.Model.Loggers
{
    internal interface ILogger
    {
        public List<Tuple<NonOrientedGraphNode, NonOrientedGraphEdge, string>> GetVisisted();
    }
}
