namespace Geko.HttpClientService.Tests.Helpers;

public class DelegatingHandlerStub : DelegatingHandler
{
    private readonly Func<System.Net.Http.HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> _handlerFunc;
    public DelegatingHandlerStub(HttpStatusCode httpStatusCode)
    {
        _handlerFunc = (request, cancellationToken) => Task.FromResult(new HttpResponseMessage(httpStatusCode));
    }

    public DelegatingHandlerStub(HttpStatusCode httpStatusCode, HttpContent httpContent)
    {
        var httpResponseMessage = new HttpResponseMessage(httpStatusCode)
        {
            Content = httpContent
        };
        _handlerFunc = (request, cancellationToken) => Task.FromResult(httpResponseMessage);
    }

    public DelegatingHandlerStub(Func<System.Net.Http.HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> handlerFunc)
    {
        _handlerFunc = handlerFunc;
    }

    protected override Task<HttpResponseMessage> SendAsync(System.Net.Http.HttpRequestMessage request, CancellationToken cancellationToken)
    {
        return _handlerFunc(request, cancellationToken);
    }
}
