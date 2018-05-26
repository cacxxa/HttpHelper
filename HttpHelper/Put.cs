using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace HttpHelper
{
    public static class Put
    {
        /// <summary> Отправляет на сервер асинхронный запрос типа PUT, добавляя к нему параметры </summary>
        /// <param name="url"> URL сервера </param>
        /// <param name="action"> Идентификатор действия на сервере </param>
        /// <param name="parameters">
        ///     Массив параметров для передачи на сервер в формате <see cref="KeyValuePair{TKey,TValue}" />
        /// </param>
        /// <param name="headers"> Заголовки </param>
        /// <param name="handler"> Для прокси</param>
        /// <exception cref="HttpRequestException">
        ///     Возникает при невозможности соединения с сервером или когда сервер возвращает
        ///     HTTP-статус, отличный от 200
        /// </exception>
        /// <exception cref="System.Net.WebException"> Возникает при отсутствии сети </exception>
        /// <returns> Возвращает ответ сервера в виде строки JSON </returns>
        public static async Task<string> Async(string url, string action = null,
            IEnumerable<KeyValuePair<string, string>> parameters = null,
            IEnumerable<KeyValuePair<string, string>> headers = null,
            HttpClientHandler handler = null)
        {
            using (HttpContent content =
                new FormUrlEncodedContent(parameters ?? new KeyValuePair<string, string>[] { }))
            using (HttpResponseMessage responseMessage =
                await MakeRequest(PutQueryMethodEnum.PUTAsync, url, action, content, headers, handler))
            {
                return await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
            }
        }

        /// <summary> Отправляет на сервер синхронный запрос типа PUT, добавляя к нему параметры </summary>
        /// <param name="url"> URL сервера </param>
        /// <param name="action"> Идентификатор действия на сервере </param>
        /// <param name="parameters">
        ///     Массив параметров для передачи на сервер в формате <see cref="KeyValuePair{TKey,TValue}" />
        /// </param>
        /// <param name="headers"> Заголовки </param>
        /// <param name="handler"> Для прокси</param>
        /// <exception cref="HttpRequestException">
        ///     Возникает при невозможности соединения с сервером или когда сервер возвращает
        ///     HTTP-статус, отличный от 200
        /// </exception>
        /// <exception cref="System.Net.WebException"> Возникает при отсутствии сети </exception>
        /// <returns> Возвращает ответ сервера в виде строки JSON </returns>
        public static string Sync(string url, string action = null,
            IEnumerable<KeyValuePair<string, string>> parameters = null,
            IEnumerable<KeyValuePair<string, string>> headers = null,
            HttpClientHandler handler = null)
        {
            using (HttpContent content =
                new FormUrlEncodedContent(parameters ?? new KeyValuePair<string, string>[] { }))
            using (HttpResponseMessage responseMessage =
                MakeRequest(PutQueryMethodEnum.PUT, url, action, content, headers, handler).Result)
            {
                return responseMessage.Content.ReadAsStringAsync().Result;
            }
        }

        private static async Task<HttpResponseMessage> MakeRequest(PutQueryMethodEnum postQueryMethod, string url,
            string action, HttpContent content, IEnumerable<KeyValuePair<string, string>> headers = null,
            HttpClientHandler handler = null)
        {
            using (HttpClient client = handler != null ? new HttpClient(handler, true) : new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();

                if (headers != null)
                    foreach (KeyValuePair<string, string> header in headers)
                        client.DefaultRequestHeaders.Add(header.Key, header.Value);

                return postQueryMethod == PutQueryMethodEnum.PUT
                    ? client.PutAsync(new Uri(url).AppendUri(action), content).Result
                    : await client.PutAsync(new Uri(url).AppendUri(action), content).ConfigureAwait(false);
            }
        }
    }
}