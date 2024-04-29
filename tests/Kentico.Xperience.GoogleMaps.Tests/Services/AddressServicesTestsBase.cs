using System.Net;
using System.Text.Json;
using CMS.Base.Internal;
using CMS.Core;
using CMS.Tests;
using Microsoft.Extensions.Options;
using NSubstitute;
using NUnit.Framework;

namespace Kentico.Xperience.GoogleMaps.Tests
{
    /// <summary>
    /// Base class for <see cref="AddressValidatorTests"/> tests.
    /// </summary>
    public class AddressServicesTestsBase : UnitTests
    {
        protected const string API_KEY = "API_KEY";
        protected const string DOMAIN = "http://test.com/";

        private protected AddressValidator addressValidator;
        private protected AddressGeocoder addressGeocoder;
        private protected IHttpClientFactory httpClientFactory;
        private protected GoogleMapsOptions options;
        private protected IEventLogService eventLogService;
        private IHttpContextRetriever httpContextRetriever;


        public static int NumberOfRequests { get; private set; }


        protected override void RegisterTestServices()
        {
            base.RegisterTestServices();

            httpContextRetriever = Substitute.For<IHttpContextRetriever>();
            Service.Use<IHttpContextRetriever>(httpContextRetriever);
        }


        [SetUp]
        public void SetUp()
        {
            var iOptions = Substitute.For<IOptions<GoogleMapsOptions>>();
            options = new GoogleMapsOptions
            {
                APIKey = API_KEY,
            };

            iOptions.Value.Returns(options);

            eventLogService = Substitute.For<IEventLogService>();

            httpClientFactory = Substitute.For<IHttpClientFactory>();

            MockHttpClient(new List<HttpResponseMessage>());


            var httpContext = Substitute.For<IHttpContext>();
            var httpRequest = Substitute.For<IRequest>();
            httpRequest.Url.Returns(new Uri(DOMAIN));

            httpContext.Request.Returns(httpRequest);
            httpContextRetriever.GetContext().Returns(httpContext);

            NumberOfRequests = 0;

            addressValidator = new AddressValidator(httpClientFactory, eventLogService, iOptions);
            addressGeocoder = new AddressGeocoder(httpClientFactory, eventLogService, iOptions);
        }


        protected static HttpResponseMessage GetMessage<T>(T content = null, HttpStatusCode statusCode = HttpStatusCode.OK)
            where T : class
        {
            return new HttpResponseMessage
            {
                StatusCode = statusCode,
                Content = new StringContent(JsonSerializer.Serialize(content))
            };
        }


        /// <summary>
        /// Used for mocking responses of httpClient.
        /// </summary>
        /// <param name="responseMessages">Responses to use for requests, returned by the given order.</param>
        protected void MockHttpClient(IList<HttpResponseMessage> responseMessages)
        {
            var httpClient = new HttpClient(new MockHttpMessageHandler(responseMessages));
            httpClientFactory.CreateClient(GoogleMapsConstants.CLIENT_NAME).Returns(httpClient);
        }


        private class MockHttpMessageHandler : HttpMessageHandler
        {
            private readonly Queue<HttpResponseMessage> responseMessages;


            public MockHttpMessageHandler(IEnumerable<HttpResponseMessage> responseMessages)
            {
                this.responseMessages = new Queue<HttpResponseMessage>(responseMessages);
            }


            protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                return Task.FromResult(Send(request, cancellationToken));
            }


            protected override HttpResponseMessage Send(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                NumberOfRequests++;
                return responseMessages.Dequeue();
            }
        }
    }
}
