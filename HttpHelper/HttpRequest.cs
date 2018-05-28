using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace HttpHelper
{
    public class HttpRequest
    {
        private HttpMethod _method;
        private Uri _url;
        private IEnumerable<KeyValuePair<string, string>> _headers;
        private IEnumerable<KeyValuePair<string, string>> _parametrs;
        private HttpClientHandler _clientHandler;
        private double _timeout = 3;


        public HttpRequest MethodAdd(HttpMethod method)
        {
            _method = method;
            return this;
        }

        public HttpRequest UrlAdd(string url)
        {
            _url = !(Uri.TryCreate(url, UriKind.Absolute, out Uri uriResult) && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps))
                ? throw new UriFormatException("Неверный формат url")
                : uriResult;
            return this;
        }

        public HttpRequest ActionAdd(string action)
        {
            _url = _url?.AppendUri(action);
            return this;
        }

        public HttpRequest ActionAdd(IEnumerable<string> actions)
        {
            foreach (string action in actions)
            {
                _url = _url?.AppendUri(action);
            }
            return this;
        }

        public HttpRequest HeaderAdd(KeyValuePair<string, string> header)
        {
            _headers = _headers.Append(header);
            return this;
        }

        public HttpRequest HeaderAdd(IEnumerable<KeyValuePair<string, string>> headers)
        {
            _headers = _headers != null ? _headers.Concat(headers) : headers;
            return this;
        }

        public HttpRequest ParametrAdd(KeyValuePair<string, string> parametr)
        {
            _parametrs = _parametrs.Append(parametr);
            return this;
        }

        public HttpRequest ParametrAdd(IEnumerable<KeyValuePair<string, string>> parametrs)
        {
            _parametrs = _parametrs != null ? _parametrs.Concat(_parametrs) : parametrs;
            return this;
        }

        public HttpRequest ClientHandlerAdd(HttpClientHandler clientHandler)
        {
            _clientHandler = clientHandler;
            return this;
        }

        public HttpRequest TimeOut(double timeout)
        {
            _timeout = timeout <= 0 ? 3 : timeout;
            return this;
        }

        public async Task<string> SendAsync()
        {
            using (HttpResponseMessage responseMessage = await ResponseMessage().ConfigureAwait(false))
            {
                return await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
            }
        }

        private async Task<HttpResponseMessage> ResponseMessage()
        {
            using (HttpContent content = new FormUrlEncodedContent(_parametrs ?? new KeyValuePair<string, string>[] { }))
            using (HttpClient client = _clientHandler != null ? new HttpClient(_clientHandler, true) : new HttpClient())
            {
                client.Timeout = TimeSpan.FromSeconds(_timeout);

                if (_headers != null)
                    foreach (KeyValuePair<string, string> header in _headers)
                        client.DefaultRequestHeaders.Add(header.Key, header.Value);
                return await client.SendAsync(new HttpRequestMessage {RequestUri = _url, Method = _method, Content = content}).ConfigureAwait(false);
            }
        }
    }
}