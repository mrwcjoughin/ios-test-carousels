using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using MvvmCross.Platform;
using MvvmCross.Platform.Platform;
using Newtonsoft.Json;

namespace test_carousels.Core.Services
{
	public class JsonRestService : IRestService
	{
		private const int TimeOutSeconds = 30;

		private const string JsonContentType = "application/json";

		private readonly IModernHttpClient _modernHttpClient;

		public JsonRestService(IModernHttpClient modernHttpClient)
		{
			_modernHttpClient = modernHttpClient;
		}

		#region IRestService implementation

		public async Task<T> GetAsync<T>(string url, IDictionary<string, object> parameters = null, IDictionary<string, string> headerValues = null) where T : new()
		{
			using (var httpClient = CreateHttpClient())
			{
				if (parameters != null && parameters.Any())
				{
					url = AddUrlParams(url, parameters);
				}

				if (headerValues != null)
				{
					SetHeaderValues(httpClient, headerValues);
				}

				try
				{
					Mvx.Trace(MvxTraceLevel.Diagnostic, "Request URL: {0}", url);

					var response = await httpClient.GetAsync(url).ConfigureAwait(false);

					if (response.StatusCode == HttpStatusCode.Unauthorized)
					{
						throw new Exception("Unauthorized");
					}

					var responseString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

					Mvx.Trace(MvxTraceLevel.Diagnostic, "Response: {0}", responseString);

					if (response.IsSuccessStatusCode)
					{
						#if DEBUG
						if (responseString.Length > 85000)
						{
							Mvx.Trace(MvxTraceLevel.Warning, "{0}", "Large JSON response will use the large object heap resulting in extra garbage collection. Stream the response instead.");
						}
						#endif

						//var result = await Task<T>.Factory.StartNew(() => JsonConvert.DeserializeObject<T>(responseString, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore })).ConfigureAwait(false);
						var result = await JsonConvert.DeserializeObjectAsync<T>(responseString, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

						return result;
					}

					throw new Exception($"Unexpected API error ({(int)response.StatusCode}): {responseString}");
				}
				catch (TaskCanceledException tcex)
				{
					// if cancellation wasn't explicitly requested, it was probably a Timeout
					if (!tcex.CancellationToken.IsCancellationRequested)
					{
						throw new TimeoutException("The connection timed out; please check your internet connection and try again.", tcex);
					}

					throw;
				}
				catch (WebException wex)
				{
					var msg = $"* WebException while making JSON call. Message: {wex.Message} StackTrace: {wex.StackTrace}, StatusCode: {wex.Status}, URL: {url}";
					Mvx.Trace(MvxTraceLevel.Diagnostic, msg);

					throw;
				}
				catch (Exception e)
				{
					Mvx.Trace(MvxTraceLevel.Diagnostic, e.Message);
					throw;
				}
			}
		}

		//public async Task<object> GetAsyncAnon(string url, IDictionary<string, object> parameters = null, IDictionary<string, string> headerValues = null)
		//{
		//	using (var httpClient = CreateHttpClient())
		//	{
		//		if (parameters != null && parameters.Any())
		//		{
		//			url = AddUrlParams(url, parameters);
		//		}

		//		if (headerValues != null)
		//		{
		//			SetHeaderValues(httpClient, headerValues);
		//		}

		//		try
		//		{
		//			Mvx.Trace(MvxTraceLevel.Diagnostic, "Request URL: {0}", url);

		//			var response = await httpClient.GetAsync(url).ConfigureAwait(false);

		//			if (response.StatusCode == HttpStatusCode.Unauthorized)
		//			{
		//				throw new Exception("Unauthorized");
		//			}

		//			var responseString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

		//			Mvx.Trace(MvxTraceLevel.Diagnostic, "Response: {0}", responseString);

		//			if (response.IsSuccessStatusCode)
		//			{
		//				#if DEBUG
		//				if (responseString.Length > 85000)
		//				{
		//					Mvx.Trace(MvxTraceLevel.Warning, "{0}", "Large JSON response will use the large object heap resulting in extra garbage collection. Stream the response instead.");
		//				}
		//				#endif

		//				var result = await Task.Factory.StartNew(() => JsonConvert.DeserializeObject(responseString)).ConfigureAwait(false);

		//				return result;
		//			}

		//			throw new Exception($"Unexpected API error ({(int)response.StatusCode}): {responseString}");
		//		}
		//		catch (TaskCanceledException tcex)
		//		{
		//			// if cancellation wasn't explicitly requested, it was probably a Timeout
		//			if (!tcex.CancellationToken.IsCancellationRequested)
		//			{
		//				throw new TimeoutException("The connection timed out; please check your internet connection and try again.", tcex);
		//			}

		//			throw;
		//		}
		//		catch (WebException wex)
		//		{
		//			var msg = $"* WebException while making JSON call. Message: {wex.Message} StackTrace: {wex.StackTrace}, StatusCode: {wex.Status}, URL: {url}";
		//			Mvx.Trace(MvxTraceLevel.Diagnostic, msg);

		//			throw;
		//		}
		//		catch (Exception e)
		//		{
		//			Mvx.Trace(MvxTraceLevel.Diagnostic, e.Message);
		//			throw;
		//		}
		//	}
		//}

		public async Task<T> PostParametersAsync<T>(string url, IDictionary<string, object> parameters, IDictionary<string, string> headerValues = null) where T : new()
		{
			using (var httpClient = CreateHttpClient())
			{
				if (headerValues != null)
				{
					SetHeaderValues(httpClient, headerValues);
				}

				if (parameters != null && parameters.Any())
				{
					url = AddUrlParams(url, parameters);
				}

				try
				{
					var response = await httpClient.PostAsync(url, null).ConfigureAwait(false);

					if (response.StatusCode == HttpStatusCode.Unauthorized)
					{
						throw new Exception("Unauthorized");
					}

					var responseString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

					#if DEBUG
					Mvx.Trace(MvxTraceLevel.Diagnostic, "Response: {0}", responseString);

					if (responseString.Length > 85000)
					{
						throw new Exception("Large JSON response will use the large object heap resulting in extra garbage collection. Stream the response instead.");
					}
					#endif

					// Mvx.Trace(MvxTraceLevel.Diagnostic, "Received response for URL {0} - {1}", url.Substring(0, Math.Min(url.Length, 80)), responseString);

					var result = await Task<T>.Factory.StartNew(() => JsonConvert.DeserializeObject<T>(responseString)).ConfigureAwait(false);
					// Mvx.Trace(MvxTraceLevel.Diagnostic, "Received object response for URL {0} - {1}", url.Substring(0, Math.Min(url.Length, 80)), result);

					return result;
				}
				catch (TaskCanceledException tcex)
				{
					// if cancellation wasn't explicitly requested, it was probably a Timeout
					if (!tcex.CancellationToken.IsCancellationRequested)
					{
						throw new TimeoutException("The connection timed out; please check your internet connection and try again.", tcex);
					}

					throw;
				}
				catch (WebException wex)
				{
					var msg = $"* WebException while making JSON call. Message: {wex.Message} StackTrace: {wex.StackTrace}, StatusCode: {wex.Status}, URL: {url}";
					Mvx.Trace(MvxTraceLevel.Diagnostic, msg);

					/*if (wex.Status == WebExceptionStatus.ConnectFailure || wex.Status == WebExceptionStatus.SendFailure)
                      {
                        throw new SilentNoInternetException();
                      }
                    }*/

					throw;
				}
			}
		}

		public async Task<T> PostFormAsync<T>(string url, Dictionary<string, object> postData = null, IDictionary<string, string> headerValues = null) where T : new()
		{
			using (var httpClient = CreateHttpClient())
			{
				if (headerValues != null)
				{
					SetHeaderValues(httpClient, headerValues);
				}

				FormUrlEncodedContent content = null;

				if (postData != null)
				{
					var paramList = new List<KeyValuePair<string, string>>();
					foreach (var item in postData)
					{
						paramList.Add(new KeyValuePair<string, string>(item.Key, item.Value == null ? string.Empty : item.Value.ToString()));
					}

					content = new FormUrlEncodedContent(paramList.ToArray());
				}

				try
				{
					var response = await httpClient.PostAsync(url, content).ConfigureAwait(false);

					if (response.StatusCode == HttpStatusCode.Unauthorized)
					{
						throw new Exception("Unauthorized");
					}

					var responseString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

					#if DEBUG
					Mvx.Trace(MvxTraceLevel.Diagnostic, "Response: {0}", responseString);

					if (responseString.Length > 85000)
					{
						throw new Exception("Large JSON response will use the large object heap resulting in extra garbage collection. Stream the response instead.");
					}
					#endif

					// Mvx.Trace(MvxTraceLevel.Diagnostic, "Received response for URL {0} - {1}", url.Substring(0, Math.Min(url.Length, 80)), responseString);

					var result = await Task<T>.Factory.StartNew(() => JsonConvert.DeserializeObject<T>(responseString)).ConfigureAwait(false);
					// Mvx.Trace(MvxTraceLevel.Diagnostic, "Received object response for URL {0} - {1}", url.Substring(0, Math.Min(url.Length, 80)), result);

					return result;
				}
				catch (TaskCanceledException tcex)
				{
					// if cancellation wasn't explicitly requested, it was probably a Timeout
					if (!tcex.CancellationToken.IsCancellationRequested)
					{
						throw new TimeoutException("The connection timed out; please check your internet connection and try again.", tcex);
					}

					throw;
				}
				catch (WebException wex)
				{
					var msg = $"* WebException while making JSON call. Message: {wex.Message} StackTrace: {wex.StackTrace}, StatusCode: {wex.Status}, URL: {url}";
					Mvx.Trace(MvxTraceLevel.Diagnostic, msg);

					/*if (wex.Status == WebExceptionStatus.ConnectFailure || wex.Status == WebExceptionStatus.SendFailure)
                      {
                        throw new SilentNoInternetException();
                      }
                    }*/

					throw;
				}
			}
		}

		public async Task<TResponse> PostJsonAsync<TResponse, TRequest>(string url, TRequest request, IDictionary<string, string> headerValues)
			where TResponse : new()
			where TRequest : class
		{
			using (var httpClient = CreateHttpClient())
			{
				if (headerValues != null)
				{
					SetJsonHeaderValues(httpClient, headerValues);
				}

				var requestString = JsonConvert.SerializeObject(request);
				Mvx.Trace(MvxTraceLevel.Diagnostic, "Request URL: {0}", url);
				Mvx.Trace(MvxTraceLevel.Diagnostic, "Request: {0}", requestString);
				var content = new StringContent(requestString, Encoding.UTF8, JsonContentType);

				try
				{
					var response = await httpClient.PostAsync(url, content).ConfigureAwait(false);

					Mvx.Trace(MvxTraceLevel.Diagnostic, "StatusCode: {0}", response.StatusCode);
					if (response.StatusCode == HttpStatusCode.Unauthorized)
					{
						throw new Exception("Unauthorized");
					}

					var responseString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

					#if DEBUG
					Mvx.Trace(MvxTraceLevel.Diagnostic, "Response: {0}", responseString);

					if (responseString.Length > 85000)
					{
						throw new Exception("Large JSON response will use the large object heap resulting in extra garbage collection. Stream the response instead.");
					}
					#endif

					// Mvx.Trace(MvxTraceLevel.Diagnostic, "Received response for URL {0} - {1}", url.Substring(0, Math.Min(url.Length, 80)), responseString);

					var result = await Task<TResponse>.Factory.StartNew(() => JsonConvert.DeserializeObject<TResponse>(responseString)).ConfigureAwait(false);
					// Mvx.Trace(MvxTraceLevel.Diagnostic, "Received object response for URL {0} - {1}", url.Substring(0, Math.Min(url.Length, 80)), result);

					return result;
				}
				catch (TaskCanceledException tcex)
				{
					// if cancellation wasn't explicitly requested, it was probably a Timeout
					if (!tcex.CancellationToken.IsCancellationRequested)
					{
						throw new TimeoutException("The connection timed out; please check your internet connection and try again.", tcex);
					}

					throw;
				}
				catch (WebException wex)
				{
					var msg = $"* WebException while making JSON call. Message: {wex.Message} StackTrace: {wex.StackTrace}, StatusCode: {wex.Status}, URL: {url}";
					Mvx.Trace(MvxTraceLevel.Diagnostic, msg);

					throw;
				}
				catch (Exception e)
				{
					var msg = $"* Exception while making JSON call. Message: {e.Message} StackTrace: {e.StackTrace}, URL: {url}";
					Mvx.Trace(MvxTraceLevel.Diagnostic, msg);

					throw;
				}
			}
		}

		public async Task<TResponse> PutJsonAsync<TResponse, TRequest>(string url, TRequest request, IDictionary<string, string> headerValues)
			where TResponse : new()
			where TRequest : class
		{
			using (var httpClient = CreateHttpClient())
			{
				if (headerValues != null)
				{
					SetJsonHeaderValues(httpClient, headerValues);
				}

				var requestString = JsonConvert.SerializeObject(request);
				Mvx.Trace(MvxTraceLevel.Diagnostic, "Request: {0}", requestString);
				var content = new StringContent(requestString, Encoding.UTF8, JsonContentType);

				try
				{
					var response = await httpClient.PutAsync(url, content).ConfigureAwait(false);

					Mvx.Trace(MvxTraceLevel.Diagnostic, "StatusCode: {0}", response.StatusCode);
					if (response.StatusCode == HttpStatusCode.Unauthorized)
					{
						throw new Exception("Unauthorized");
					}

					var responseString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

					#if DEBUG
					Mvx.Trace(MvxTraceLevel.Diagnostic, "Response: {0}", responseString);

					if (responseString.Length > 85000)
					{
						throw new Exception("Large JSON response will use the large object heap resulting in extra garbage collection. Stream the response instead.");
					}
					#endif

					// Mvx.Trace(MvxTraceLevel.Diagnostic, "Received response for URL {0} - {1}", url.Substring(0, Math.Min(url.Length, 80)), responseString);

					var result = await Task<TResponse>.Factory.StartNew(() => JsonConvert.DeserializeObject<TResponse>(responseString)).ConfigureAwait(false);
					// Mvx.Trace(MvxTraceLevel.Diagnostic, "Received object response for URL {0} - {1}", url.Substring(0, Math.Min(url.Length, 80)), result);

					return result;
				}
				catch (TaskCanceledException tcex)
				{
					// if cancellation wasn't explicitly requested, it was probably a Timeout
					if (!tcex.CancellationToken.IsCancellationRequested)
					{
						throw new TimeoutException("The connection timed out; please check your internet connection and try again.", tcex);
					}

					throw;
				}
				catch (WebException wex)
				{
					var msg = $"* WebException while making JSON call. Message: {wex.Message} StackTrace: {wex.StackTrace}, StatusCode: {wex.Status}, URL: {url}";
					Mvx.Trace(MvxTraceLevel.Diagnostic, msg);

					throw;
				}
				catch (Exception e)
				{
					var msg = $"* Exception while making JSON call. Message: {e.Message} StackTrace: {e.StackTrace}, URL: {url}";
					Mvx.Trace(MvxTraceLevel.Diagnostic, msg);

					throw;
				}
			}
		}

		#endregion

		private static void SetHeaderValues(HttpClient httpClient, IDictionary<string, string> headerValues)
		{
			httpClient.DefaultRequestHeaders.Clear();

			foreach (var keyValuePair in headerValues)
			{
				httpClient.DefaultRequestHeaders.Add(keyValuePair.Key, keyValuePair.Value);
			}
		}

		private static void SetJsonHeaderValues(HttpClient httpClient, IDictionary<string, string> headerValues)
		{
			httpClient.DefaultRequestHeaders.Clear();
			httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(JsonContentType));

			foreach (var keyValuePair in headerValues)
			{
				httpClient.DefaultRequestHeaders.Add(keyValuePair.Key, keyValuePair.Value);
			}
		}

		private static string AddUrlParams(string baseUrl, IDictionary<string, object> parameters)
		{
			var stringBuilder = new StringBuilder(baseUrl);
			var hasFirstParam = baseUrl.Contains("?");

			foreach (var parameter in parameters)
			{
				var format = hasFirstParam ? "&{0}={1}" : "?{0}={1}";
				stringBuilder.AppendFormat(
					CultureInfo.InvariantCulture,
					format,
					Uri.EscapeDataString(parameter.Key),
					parameter.Value == null ? string.Empty : Uri.EscapeDataString(parameter.Value.ToString()));

				hasFirstParam = true;
			}

			return stringBuilder.ToString();
		}

		private HttpClient CreateHttpClient()
		{
			var handler = _modernHttpClient.GetNativeHandler();

			var httpClient = _modernHttpClient.Get(handler);
			httpClient.Timeout = TimeSpan.FromSeconds(TimeOutSeconds);

			return httpClient;
		}
	}
}