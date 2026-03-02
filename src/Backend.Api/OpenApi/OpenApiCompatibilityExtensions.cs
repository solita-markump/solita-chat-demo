using System.Text;
using Microsoft.AspNetCore.Http.Metadata;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi;
using Microsoft.OpenApi.Models;
using Microsoft.OpenApi.Writers;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class OpenApiServiceCollectionExtensions
    {
        public static IServiceCollection AddOpenApi(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            return services;
        }
    }
}

namespace Microsoft.AspNetCore.Builder
{
    public static class OpenApiEndpointRouteBuilderExtensions
    {
        public static IEndpointConventionBuilder MapOpenApi(
            this IEndpointRouteBuilder endpoints,
            string pattern = "/openapi/{documentName}.json")
        {
            return endpoints.MapGet(pattern, (string documentName, EndpointDataSource endpointDataSource) =>
            {
                if (!string.Equals(documentName, "v1", StringComparison.OrdinalIgnoreCase))
                {
                    return Results.NotFound();
                }

                var document = CreateDocument(endpointDataSource);
                return Results.Text(document, "application/json");
            }).ExcludeFromDescription();
        }

        private static string CreateDocument(EndpointDataSource endpointDataSource)
        {
            var document = new OpenApiDocument
            {
                Info = new OpenApiInfo
                {
                    Title = "solita-chat-demo API",
                    Version = "v1"
                },
                Paths = new OpenApiPaths()
            };

            foreach (var endpoint in endpointDataSource.Endpoints.OfType<RouteEndpoint>())
            {
                var methods = endpoint.Metadata.GetMetadata<HttpMethodMetadata>()?.HttpMethods;
                if (methods is null || methods.Count == 0)
                {
                    continue;
                }

                var path = endpoint.RoutePattern.RawText;
                if (string.IsNullOrWhiteSpace(path) ||
                    path.StartsWith("/openapi", StringComparison.OrdinalIgnoreCase) ||
                    path.StartsWith("/scalar", StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                if (!path.StartsWith('/'))
                {
                    path = $"/{path}";
                }

                if (!document.Paths.TryGetValue(path, out var pathItem))
                {
                    pathItem = new OpenApiPathItem();
                    document.Paths[path] = pathItem;
                }

                foreach (var method in methods)
                {
                    var operationType = method.ToUpperInvariant() switch
                    {
                        "GET" => OperationType.Get,
                        "POST" => OperationType.Post,
                        "PUT" => OperationType.Put,
                        "PATCH" => OperationType.Patch,
                        "DELETE" => OperationType.Delete,
                        "HEAD" => OperationType.Head,
                        "OPTIONS" => OperationType.Options,
                        "TRACE" => OperationType.Trace,
                        _ => (OperationType?)null
                    };

                    if (operationType is null)
                    {
                        continue;
                    }

                    var operation = new OpenApiOperation
                    {
                        Summary = endpoint.Metadata.GetMetadata<IEndpointSummaryMetadata>()?.Summary,
                        Description = endpoint.Metadata.GetMetadata<IEndpointDescriptionMetadata>()?.Description,
                        Responses = CreateResponses(endpoint.Metadata)
                    };

                    var tagsMetadata = endpoint.Metadata.GetMetadata<ITagsMetadata>();
                    if (tagsMetadata is not null && tagsMetadata.Tags.Count > 0)
                    {
                        operation.Tags = tagsMetadata.Tags
                            .Select(tag => new OpenApiTag { Name = tag })
                            .ToList();
                    }

                    var acceptsMetadata = endpoint.Metadata.GetMetadata<IAcceptsMetadata>();
                    if (acceptsMetadata is not null)
                    {
                        var contentTypes = acceptsMetadata.ContentTypes.Count > 0
                            ? acceptsMetadata.ContentTypes
                            : ["application/json"];

                        operation.RequestBody = new OpenApiRequestBody { Required = true };
                        foreach (var contentType in contentTypes)
                        {
                            operation.RequestBody.Content[contentType] = new OpenApiMediaType
                            {
                                Schema = new OpenApiSchema { Type = "object" }
                            };
                        }
                    }

                    pathItem.Operations[operationType.Value] = operation;
                }
            }

            using var stream = new MemoryStream();
            using var writer = new StreamWriter(stream, new UTF8Encoding(false), leaveOpen: true);
            var openApiWriter = new OpenApiJsonWriter(writer);
            document.SerializeAsV3(openApiWriter);
            writer.Flush();
            stream.Position = 0;
            using var reader = new StreamReader(stream, Encoding.UTF8);
            return reader.ReadToEnd();
        }

        private static OpenApiResponses CreateResponses(EndpointMetadataCollection metadata)
        {
            var responses = new OpenApiResponses();
            var producesMetadata = metadata.OfType<IProducesResponseTypeMetadata>().ToList();

            if (producesMetadata.Count == 0)
            {
                responses["200"] = new OpenApiResponse { Description = "HTTP 200 response." };
                return responses;
            }

            foreach (var produces in producesMetadata)
            {
                var statusCode = produces.StatusCode.ToString();
                responses[statusCode] = new OpenApiResponse
                {
                    Description = $"HTTP {statusCode} response."
                };
            }

            return responses;
        }
    }
}
