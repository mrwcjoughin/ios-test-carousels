using System;
using System.Net.Http;
using ModernHttpClient;

namespace test_carousels.Core
{
	public class ModernHttpClient : IModernHttpClient
	{
		public HttpClient Get()
		{
			return new HttpClient(GetNativeHandler());
		}

		public HttpClient Get(HttpMessageHandler handler)
		{
			return new HttpClient(handler);
		}

		public HttpMessageHandler GetNativeHandler(bool throwOnCaptiveNetwork = false, bool useCustomSslCertification = false)
		{
			return new NativeMessageHandler(throwOnCaptiveNetwork, useCustomSslCertification);
		}
	}
}