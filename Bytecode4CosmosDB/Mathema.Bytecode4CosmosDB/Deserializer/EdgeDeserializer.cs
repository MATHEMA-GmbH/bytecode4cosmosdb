using System;
using Gremlin.Net.Structure;
using Gremlin.Net.Structure.IO.GraphSON;

namespace Mathema.Bytecode4CosmosDB.Deserializer
{
    public class EdgeDeserializer
    {
        public EdgeDeserializer()
        {
        }

        public static dynamic Objectify(Dictionary<string, object> dataset)
        {
            var id = dataset["id"];
            var label = (string)dataset["label"] ?? "<<Edge>>";

            var inVLabel = (string)dataset["inVLabel"] ?? "";
            var inVId = (string)dataset["inV"];
            var inVertex = new Vertex(inVId, inVLabel);

            var outVLabel = (string)dataset["outVLabel"] ?? "";
            var outVId = (string)dataset["outV"];
            var outVertex = new Vertex(outVId, outVLabel);


            var edge = new Edge(id, outVertex, label, inVertex);

            return edge;
        }
    }
}
