using System;
using Gremlin.Net.Driver;
using Gremlin.Net.Structure.IO.GraphSON;

namespace Mathema.Bytecode4CosmosDB
{
    public class CosmosDbGremlinClientBuilder
    {

        private readonly GremlinServer _server;
        private ConnectionPoolSettings _connectionPoolSettings;


        private CosmosDbGremlinClientBuilder(GremlinServer server)
        {
            _server = server;
        }


        public static CosmosDbGremlinClientBuilder BuildClientForServer(string hostname, int port, string cosmosDatabase, string cosmosAuthKey, string cosmosCollection)
        {
            return new CosmosDbGremlinClientBuilder(new GremlinServer(hostname, port, enableSsl: true,
                                                        username: "/dbs/" + cosmosDatabase + "/colls/" + cosmosCollection,
                                                        password: cosmosAuthKey));

        }

        /// <summary>
        ///     Configures the <see cref="ConnectionPoolSettings"/> for the client.
        /// </summary>
        /// <param name="connectionPoolSettings">The connection pool settings to use.</param>
        public CosmosDbGremlinClientBuilder WithConnectionPoolSettings(ConnectionPoolSettings connectionPoolSettings)
        {
            _connectionPoolSettings = connectionPoolSettings;
            return this;
        }

        /// <summary>
        ///     Creates the <see cref="IGremlinClient" /> with the given settings and pre-configured for Cosmos DB.
        /// </summary>
        public IGremlinClient Create()
        {


            return new GremlinClient(gremlinServer: _server,
                                     graphSONReader: new GraphSON2Reader(),
                                     graphSONWriter: new GraphSON2Writer(),
                                     mimeType: "application/vnd.gremlin-v2.0+json",
                                     connectionPoolSettings:  _connectionPoolSettings);
        }
    }
}

