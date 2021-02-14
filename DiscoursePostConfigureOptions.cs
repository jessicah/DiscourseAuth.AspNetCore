using DiscourseAuth.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Options;

namespace Microsoft.Extensions.DependencyInjection
{
	class DiscoursePostConfigureOptions<TOptions, THandler> : IPostConfigureOptions<TOptions>
		where TOptions : DiscourseOptions, new()
		where THandler : DiscourseHandler<TOptions>
	{
		private readonly IDataProtectionProvider _dp;

		public DiscoursePostConfigureOptions(IDataProtectionProvider dataProtection)
		{
			_dp = dataProtection;
		}

		public void PostConfigure(string name, TOptions options)
		{
			options.DataProtectionProvider ??= _dp;

			if (options.StateDataFormat == null)
			{
				var dataProtector = options.DataProtectionProvider.CreateProtector(
					typeof(THandler).FullName, name, "v1");
				options.StateDataFormat = new PropertiesDataFormat(dataProtector);
			}
		}
	}
}
