using Query.API.DependencyInjection.Options;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace Query.API.DependencyInjection.Extensions;

public static class SwaggerExtensions
{
    public static void AddSwaggerAPI(this IServiceCollection services)
    {
        services.AddSwaggerGen(); // Đăng ký dịch vụ Swagger để tự động sinh tài liệu API.
        services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>(); // Đăng ký cấu hình Swagger
    }

    public static void UseSwaggerAPI(this WebApplication app)
    {
        app.UseSwagger(); // Kích hoạt middleware để tạo Swagger JSON(swagger.json)
        app.UseSwaggerUI(options =>
        {
            // Duyệt qua danh sách phiên bản API và tạo Swagger UI cho từng phiên bản
            foreach (var version in app.DescribeApiVersions().Select(version => version.GroupName))
            {
                options.SwaggerEndpoint($"/swagger/{version}/swagger.json", version);
            }

            options.DisplayRequestDuration();
            options.EnableTryItOutByDefault();
            options.DocExpansion(DocExpansion.None);
        });
    }
}
