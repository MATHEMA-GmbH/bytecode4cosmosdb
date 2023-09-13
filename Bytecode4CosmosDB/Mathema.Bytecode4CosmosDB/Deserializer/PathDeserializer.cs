
using Gremlin.Net.Structure;
using Gremlin.Net.Structure.IO.GraphSON;
using System.Collections;

namespace Mathema.Bytecode4CosmosDB.Deserializer
{
	public class PathDeserializer
	{
		public PathDeserializer()
		{
			//Gremlin.Net.Structure.Path path = new Gremlin.Net.Structure.Path();
		}

        public static dynamic Objectify(Dictionary<string, object> dataset)
        {
            var labels = dataset["labels"];
            IList< ISet<string>> labelList = new List<ISet<string>>();

            var returnedObjects = dataset["objects"] as IEnumerable;

            var objects = new List<object>();


            foreach (object? item in returnedObjects)
            {
                    objects.Add(ResultToTypeConverter.Convert(item));
            }

            var path = new Gremlin.Net.Structure.Path(labelList, objects);

            return path;
        }
    }
}

