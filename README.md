# bytecode4cosmosdb

This is a small library to support bytecode-based requests with Azure Cosmos DB

A word of caution before proceeding: this is very much a work in progress.


At the moment Cosmos DB only supports script-based requests and the result is returned in GraphSON 1.0 format (see https://tinkerpop.apache.org/docs/3.4.1/dev/io/). 
Cosmos DB is only compatible with Gremlin.NET 3.4, not with the newer version 3.5 and 3.6 (see https://docs.microsoft.com/en-us/azure/cosmos-db/graph/gremlin-support)

This library converts the bytecode-based request into an Groovy string representation using the GroovyTranslator class taken from the Gremlin.NET 3.5 library (https://github.com/apache/tinkerpop/tree/master/gremlin-dotnet).
The returned result is serialized into expected result types of the bytecode based request.

Example: create an edge between to nodes:

```C#
Edge newEdge = 
         g.V().Has("person", "first_name", fromName)
          .AddE("friends").To(__.V().Has("person", "first_name", toName))
          .Next();
```


To use the library you simply have to 
