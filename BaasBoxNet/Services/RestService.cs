﻿using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using BaasBoxNet.Exceptions;
using BaasBoxNet.Models;
using Newtonsoft.Json;

namespace BaasBoxNet.Services
{
    internal class RestService
    {
        private readonly BaasBox _box;

        internal RestService(BaasBox box)
        {
            _box = box;
        }

        public async Task<T> GetAsync<T>(string url, CancellationToken cancellationToken)
        {
            using (var client = GetHttpClient())
            {
                using (var response = await client.GetAsync(url, cancellationToken).ConfigureAwait(false))
                {
                    return await ProcessResponse<T>(response);
                }
            }
        }

        public async Task<T> PostAsync<T>(string url, HttpContent data, CancellationToken cancellationToken)
        {
            using (var client = GetHttpClient())
            {
                using (
                    var response =
                        await
                            client.PostAsync(CreateRequestUrl(url), data, cancellationToken)
                                .ConfigureAwait(false))
                {
                    return await ProcessResponse<T>(response).ConfigureAwait(false);
                }
            }
        }

        public async Task<T> PutAsync<T>(string url, HttpContent data, CancellationToken cancellationToken)
        {
            using (var client = GetHttpClient())
            {
                using (
                    var response =
                        await
                            client.PutAsync(CreateRequestUrl(url), data, cancellationToken).ConfigureAwait(false)
                    )
                {
                    return await ProcessResponse<T>(response).ConfigureAwait(false);
                }
            }
        }

        public async Task DeleteAsync(string url, CancellationToken cancellationToken)
        {
            using (var client = GetHttpClient())
            {
                using (
                    var response =
                        await
                            client.DeleteAsync(CreateRequestUrl(url), cancellationToken).ConfigureAwait(false)
                    )
                {
                    await ProcessResponse<object>(response).ConfigureAwait(false);
                }
            }
        }

        private async Task<T> ProcessResponse<T>(HttpResponseMessage response)
        {
            var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
            {
                var parsedResponse = JsonConvert.DeserializeObject<BaasBoxResponse<T>>(content);
                return parsedResponse.Data;
            }
            var serverError = JsonConvert.DeserializeObject<BaasServerError>(content);
            throw new BaasApiException((int) response.StatusCode, serverError.Resource, serverError.Method,
                serverError.ApiVersion, serverError.Message);
        }

        private string CreateRequestUrl(string url)
        {
            return string.Format("http://{0}:{1}/{2}", _box.Config.ApiDomain, _box.Config.HttpPort, url);
        }

        private HttpClient GetHttpClient()
        {
            var httpClient = new HttpClient
            {
                BaseAddress = new Uri(string.Format("http://{0}:{1}", _box.Config.ApiDomain, _box.Config.HttpPort))
            };
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.Add("X-BAASBOX-APPCODE", _box.Config.AppCode);
            if (_box.UserManagement.IsAuthenticated)
                httpClient.DefaultRequestHeaders.Add("X-BB-SESSION", _box.User.Session);
            return httpClient;
        }

        public async Task<byte[]> GetFile(string fileID, CancellationToken cancellation)
        {
            var client = GetHttpClient();
            var response = await client.GetByteArrayAsync(CreateRequestUrl("file/") + fileID);
            return response;
        }
    }
}