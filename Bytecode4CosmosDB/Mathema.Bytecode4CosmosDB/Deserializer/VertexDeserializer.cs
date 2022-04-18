using Gremlin.Net.Structure;
using Gremlin.Net.Structure.IO.GraphSON;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mathema.Bytecode4CosmosDB.Deserializer
{
    internal class VertexDeserializer
    {
        public static dynamic Objectify(Dictionary<string, object> dataset)
        {
            var id = dataset["id"];
            var label = (string)dataset["label"] ?? Vertex.DefaultLabel;

            var vertex = new Vertex(id, label);

            if (dataset["properties"] != null)
            {
                Dictionary<string, object> properties = (Dictionary<string, object>)dataset["properties"];

                foreach (var item in properties)
                {
                    VertexPropertyDeserializer propertiesDeserializer = new VertexPropertyDeserializer(vertex);
                    propertiesDeserializer.Objectify(item);
                }
            }

            return vertex;
        }
    }
}
