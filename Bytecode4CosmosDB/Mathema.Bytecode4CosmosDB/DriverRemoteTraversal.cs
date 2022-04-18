using Gremlin.Net.Driver;
using Gremlin.Net.Process.Traversal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mathema.Bytecode4CosmosDB
{
    internal class DriverRemoteTraversal<S, E> : DefaultTraversal<S, E>
    {
        public DriverRemoteTraversal(IGremlinClient gremlinClient, Guid requestId,
            IEnumerable<Traverser> traversers)
        {
            Traversers = traversers;
        }
    }
}