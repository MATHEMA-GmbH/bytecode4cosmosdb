using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mathema.Bytecode4CosmosDB.Deserializer
{
    internal class ResultToTypeConverter
    {
        private const string TYPE_KEY = "type";

        public static dynamic? Convert(dynamic result)
        {
            dynamic objectifiedResult = null;

            if (result is Dictionary<string, object>)
            {
                Dictionary<string, object>? resultDict = result as Dictionary<string, object>;

                // handling Vertex and Egde entries
                if (resultDict != null && resultDict.ContainsKey(TYPE_KEY))
                {
                    string type = resultDict[TYPE_KEY].ToString();

                    switch (type)
                    {
                        case "vertex":
                            objectifiedResult = VertexDeserializer.Objectify(resultDict);

                            break;
                        case "edge":
                            objectifiedResult = EdgeDeserializer.Objectify(resultDict);

                            break;
                        default:
                            throw new Exception($"Unknow type: {type}");
                    }
                }
                // handling Path entries
                else if (resultDict != null && resultDict.ContainsKey("labels")
                               && resultDict.ContainsKey("objects"))
                {
                    objectifiedResult = PathDeserializer.Objectify(resultDict);
                }
                else  // in case of ValueMap() as last traversal
                {

                    objectifiedResult = resultDict;
                }
            }
            else
            {
                throw new Exception($"unknown result type {result.GetType()}" );
            }

            return objectifiedResult;
        }
    }
}
