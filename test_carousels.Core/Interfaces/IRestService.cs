using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace test_carousels.Core
{
	public interface IRestService
	{
		Task<T> GetAsync<T>(string url, IDictionary<string, object> parameters = null, IDictionary<string, string> headerValues = null) where T : new();

		//Task<object> GetAsyncAnon(string url, IDictionary<string, object> parameters = null, IDictionary<string, string> headerValues = null);

		Task<T> PostParametersAsync<T>(string url, IDictionary<string, object> parameters, IDictionary<string, string> headerValues = null) where T : new();

		Task<T> PostFormAsync<T>(string url, Dictionary<string, object> postData = null, IDictionary<string, string> headerValues = null) where T : new();

		Task<TResponse> PostJsonAsync<TResponse, TRequest>(string url, TRequest request, IDictionary<string, string> headerValues)
			where TResponse : new()
			where TRequest : class;

		Task<TResponse> PutJsonAsync<TResponse, TRequest>(string url, TRequest request, IDictionary<string, string> headerValues)
			where TResponse : new()
			where TRequest : class;
	}
}