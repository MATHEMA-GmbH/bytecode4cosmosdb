
using Mathema.Bytecode4CosmosDB.Deserializer;
using Gremlin.Net.Driver;
using Gremlin.Net.Process.Remote;
using Gremlin.Net.Process.Traversal;
using Gremlin.Net.Process.Traversal.Translator;
using Gremlin.Net.Structure;
using Gremlin.Net.Structure.IO.GraphSON;

namespace Mathema.Bytecode4CosmosDB
{
	public class CosmosDbRemoteConnection : IRemoteConnection, IDisposable
	{
        private const string TYPE_KEY = "type";
        private readonly IGremlinClient _client;
        private readonly string _traversalSource;

        /// <summary>
        /// Filter on these keys provided to OptionsStrategy and apply them to the request. Note that
        /// "scriptEvaluationTimeout" was deprecated in 3.3.9 but still supported in server implementations and will
        /// be removed in later versions. 
        /// </summary>
        private readonly List<String> _allowedKeys = new List<string>
                    {Tokens.ArgsEvalTimeout, "scriptEvaluationTimeout", Tokens.ArgsBatchSize,
                     Tokens.RequestId, Tokens.ArgsUserAgent};

        private readonly string _sessionId;
        private string Processor => IsSessionBound ? Tokens.ProcessorSession : Tokens.ProcessorTraversal;

        /// <inheritdoc />
        public bool IsSessionBound => _sessionId != null;

        

        /// <summary>
        ///     Initializes a new <see cref="IRemoteConnection" /> using "g" as the default remote TraversalSource name.
        /// </summary>
        /// <param name="client">The <see cref="IGremlinClient" /> that will be used for the connection.</param>
        /// <exception cref="ArgumentNullException">Thrown when client is null.</exception>
        public CosmosDbRemoteConnection(IGremlinClient client) : this(client, "g")
        {
        }

        /// <summary>
        ///     Initializes a new <see cref="IRemoteConnection" />.
        /// </summary>
        /// <param name="client">The <see cref="IGremlinClient" /> that will be used for the connection.</param>
        /// <param name="traversalSource">The name of the traversal source on the server to bind to.</param>
        /// <exception cref="ArgumentNullException">Thrown when client is null.</exception>
        public CosmosDbRemoteConnection(IGremlinClient client, string traversalSource)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _traversalSource = traversalSource ?? throw new ArgumentNullException(nameof(traversalSource));
        }

        private CosmosDbRemoteConnection(IGremlinClient client, string traversalSource, Guid sessionId)
            : this(client, traversalSource)
        {
            _sessionId = sessionId.ToString();
        }

        /// <summary>
        ///     Submits <see cref="Bytecode" /> for evaluation to a remote Gremlin Server.
        /// </summary>
        /// <param name="bytecode">The <see cref="Bytecode" /> to submit.</param>
        /// <returns>A <see cref="ITraversal" /> allowing to access the results and side-effects.</returns>
        public async Task<ITraversal<S, E>> SubmitAsync<S, E>(Bytecode bytecode)
        {
            var requestId = Guid.NewGuid();
            var resultSet = await SubmitBytecodeAsync(requestId, bytecode).ConfigureAwait(false);
            return new DriverRemoteTraversal<S, E>(_client, requestId, resultSet);
        }

        private async Task<IEnumerable<Traverser>> SubmitBytecodeAsync(Guid requestid, Bytecode bytecode)
        {
            IList<Traverser> resultList = new List<Traverser>();

            String queryString = GroovyTranslator.Of("g").Translate(bytecode);

            Traverser traverser = null;
            var resultSet = await _client.SubmitAsync<dynamic>(queryString).ConfigureAwait(false);

            if (resultSet.Count > 0)
            {
                foreach (var result in resultSet)
                {
                    dynamic objectifiedResult = ResultToTypeConverter.Convert(result);

                    traverser = new Traverser(objectifiedResult);
                    resultList.Add(traverser);
                }
            }

            return resultList.AsEnumerable<Traverser>();
        }

        /// <inheritdoc />
        public void Dispose()
        {
            _client?.Dispose();
        }
    }
}

