namespace Backend.Api.Tests.Infrastructure;

public sealed class DockerFactAttribute : FactAttribute
{
    public DockerFactAttribute()
    {
        if (!IsDockerSocketAvailable())
        {
            Skip = "Docker is required for PostgreSQL integration tests.";
        }
    }

    private static bool IsDockerSocketAvailable()
    {
        if (OperatingSystem.IsWindows())
        {
            return File.Exists(@"\\.\pipe\docker_engine");
        }

        return File.Exists("/var/run/docker.sock");
    }
}
