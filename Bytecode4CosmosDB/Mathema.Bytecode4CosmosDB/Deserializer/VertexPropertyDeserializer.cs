using System;
using Gremlin.Net.Structure.IO.GraphSON;
using Gremlin.Net.Structure;
using Newtonsoft.Json.Linq;

namespace Mathema.Bytecode4CosmosDB.Deserializer
{
	public class VertexPropertyDeserializer
	{

		/// <summary>
        /// CosmosDB Strcuture:
        /// {
				  //  "id": "a7111ba7-0ea1-43c9-b6b2-efc5e3aea4c0",
				  //  "label": "person",
				  //  "type": "vertex",
				  //  "outE": {
				  //    "knows": [
				  //      {
				  //        "id": "3ee53a60-c561-4c5e-9a9f-9c7924bc9aef",
				  //        "inV": "04779300-1c8e-489d-9493-50fd1325a658"
				  //      },
				  //      {
				  //        "id": "21984248-ee9e-43a8-a7f6-30642bc14609",
				  //        "inV": "a8e3e741-2ef7-4c01-b7c8-199f8e43e3bc"
				  //      }
				  //    ]
				  //  },
				  //  "properties" : {
					//"name" : [ {
					//  "id" : 0,
					//  "value" : "marko"


					//} ],
					//"location" : [ {
					//  "id" : 6,
					//  "value" : "san diego",
					//  "properties" : {
					//    "startTime" : 1997,
					//    "endTime" : 2001


					//  }
					//}, {
					//}
    /// </summary>
    ///

    public VertexPropertyDeserializer(Vertex vertex)
		{
			_vertex = vertex;
		}

		private Vertex _vertex;

		public dynamic Objectify(KeyValuePair<string, object> property)
		{
			var name = property.Key;
			var details = property.Value;
			
			return new VertexProperty("id", name, "value", _vertex);
		}
	}
}

