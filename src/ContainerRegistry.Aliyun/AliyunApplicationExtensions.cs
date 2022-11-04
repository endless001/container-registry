using Aliyun.OSS;
using ContainerRegistry.Core.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace ContainerRegistry.Aliyun;

public static class AliyunApplicationExtensions
{
    public static IServiceCollection AddAliyunOssStorage(this IServiceCollection services)
    {
        services.AddTransient<AliyunStorageService>();
        services.TryAddTransient<IStorageService>(provider => provider.GetRequiredService<AliyunStorageService>());

        services.AddSingleton(provider =>
        {
            var options = provider.GetRequiredService<IOptions<AliyunStorageOptions>>().Value;

            return new OssClient(options.Endpoint, options.AccessKey, options.AccessKeySecret);
        });

        return services;
    }
}