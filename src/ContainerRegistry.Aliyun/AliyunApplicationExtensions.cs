using Aliyun.OSS;
using ContainerRegistry.Core;
using ContainerRegistry.Core.Configuration;
using ContainerRegistry.Core.Extensions;
using ContainerRegistry.Core.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace ContainerRegistry.Aliyun;

public static class AliyunApplicationExtensions
{
    public static ContainerRegistryBuilder AddAliyunOssStorage(this ContainerRegistryBuilder builder)
    {
        builder.Services.AddContainerRegistryOptions<AliyunStorageOptions>(nameof(ContainerRegistryOptions.Storage));
        builder.Services.AddTransient<AliyunStorageService>();
        builder.Services.TryAddTransient<IStorageService>(provider =>
            provider.GetRequiredService<AliyunStorageService>());

        builder.Services.AddSingleton(provider =>
        {
            var options = provider.GetRequiredService<IOptions<AliyunStorageOptions>>().Value;

            return new OssClient(options.Endpoint, options.AccessKey, options.AccessKeySecret);
        });
        builder.Services.AddProvider<IStorageService>((provider, config) =>
            !config.HasStorageType("AliyunOss") ? null : provider.GetRequiredService<AliyunStorageService>());
        return builder;
    }

    public static ContainerRegistryBuilder AddAliyunOssStorage(this ContainerRegistryBuilder builder,
        Action<AliyunStorageOptions> configure)
    {
        builder.AddAliyunOssStorage();
        builder.Services.Configure(configure);
        return builder;
    }
}