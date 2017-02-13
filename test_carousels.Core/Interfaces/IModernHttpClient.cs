using System;
using System.Net.Http;

namespace test_carousels.Core
{
	public interface IModernHttpClient
	{
		HttpClient Get();

		HttpClient Get(HttpMessageHandler handler);

		HttpMessageHandler GetNativeHandler(bool throwOnCaptiveNetwork = false, bool useCustomSslCertification = false);
	}
}