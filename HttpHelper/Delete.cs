using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace HttpHelper
{
    public static class Delete
    {
        /// <summary> Отправляет на сервер асинхронный запрос типа DELETE, добавляя к нему параметры </summary>
        /// <param name="url"> URL сервера </param>
        /// <param name="action"> Идентификатор действия на сервере </param>
        /// <param name="headers"> Заголовки </param>
        /// <param name="handler"> Для прокси</param>
        /// <exception cref="HttpRequestException">
        ///     Возникает при невозможности соединения с сервером или когда сервер возвращает
        ///     HTTP-статус, отличный от 200
        /// </exception>
        /// <exception cref="System.Net.WebException"> Возникает при отсутствии сети </exception>
        /// <returns> Возвращает ответ сервера в виде строки JSON </returns>
        public static async Task<string> Async(string url, string action = null,
            IEnumerable<KeyValuePair<string, string>> headers = null,
            HttpClientHandler handler = null)
        {
            using (HttpResponseMessage responseMessage =
                await MakeRequest(DeleteQueryMethodEnum.DELETEAsync, url, action, headers, handler))
            {
                return await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
            }
        }

        /// <summary> Отправляет на сервер синхронный запрос типа DELETE, добавляя к нему параметры </summary>
        /// <param name="url"> URL сервера </param>
        /// <param name="action"> Идентификатор действия на сервере </param>
        /// <param name="headers"> Заголовки </param>
        /// <param name="handler"> Для прокси</param>
        /// <exception cref="HttpRequestException">
        ///     Возникает при невозможности соединения с сервером или когда сервер возвращает
        ///     HTTP-статус, отличный от 200
        /// </exception>
        /// <exception cref="System.Net.WebException"> Возникает при отсутствии сети </exception>
        /// <returns> Возвращает ответ сервера в виде строки JSON </returns>
        public static string Sync(string url, string action = null,
            IEnumerable<KeyValuePair<string, string>> headers = null,
            HttpClientHandler handler = null)
        {
            using (HttpResponseMessage responseMessage =
                MakeRequest(DeleteQueryMethodEnum.DELETE, url, action, headers, handler).Result)
            {
                return responseMessage.Content.ReadAsStringAsync().Result;
            }
        }

        private static async Task<HttpResponseMessage> MakeRequest(DeleteQueryMethodEnum postQueryMethod, string url,
            string action, IEnumerable<KeyValuePair<string, string>> headers = null,
            HttpClientHandler handler = null)
        {
            using (HttpClient client = handler != null ? new HttpClient(handler, true) : new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();

                if (headers != null)
                    foreach (KeyValuePair<string, string> header in headers)
                        client.DefaultRequestHeaders.Add(header.Key, header.Value);

                return postQueryMethod == DeleteQueryMethodEnum.DELETE
                    ? client.DeleteAsync(new Uri(url).AppendUri(action)).Result
                    : await client.DeleteAsync(new Uri(url).AppendUri(action)).ConfigureAwait(false);
            }
        }
    }
}