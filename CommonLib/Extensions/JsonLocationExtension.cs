using System;
using CommonLib.StringLocalizer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;

namespace CommonLib.Extensions
{
    public static class JsonLocationExtension
    {
        public static IServiceCollection AddJsonLocation(this IServiceCollection services)
        {
            services.AddLocalization();
            services.AddSingleton<LocalizationMiddleware>();
            services.AddSingleton<IStringLocalizerFactory, JsonStringLocalizerFactory>();

            return services;
        }
    }
}

