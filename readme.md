# Docker Build Overview

Docker Build implements a client-server architecture, where:

- Client: Buildx is the client and the user interface for running and managing builds.
- Server: BuildKit is the server, or builder, that handles the build execution.

When you invoke a build, the Buildx client sends a build request to the BuildKit backend. BuildKit resolves the build instructions and executes the build steps. The build output is either sent back to the client or uploaded to a registry, such as Docker Hub.

Buildx and BuildKit are both installed with Docker Desktop and Docker Engine out-of-the-box. When you invoke the docker build command, you're using Buildx to run a build using the default BuildKit bundled with Docker.

## Buildx
Buildx is a Docker CLI plugin that extends the docker command with the full support of the features provided by BuildKit. 

Buildx is the CLI tool that you use to run builds. The docker build command is a wrapper around Buildx. When you invoke docker build, Buildx interprets the build options and sends a build request to the BuildKit backend.

## BuildKit
BuildKit is the backend that executes the build instructions. It is responsible for resolving the build steps, managing the build cache, and producing the final build output.
A build execution starts with the invocation of a docker build command. Buildx interprets your build command and sends a build request to the BuildKit backend. The build request includes:

- The Dockerfile
- Build arguments
- Export options
- Caching options

BuildKit resolves the build instructions and executes the build steps. While BuildKit is executing the build, Buildx monitors the build status and prints the progress to the terminal.

If the build requires resources from the client, such as local files or build secrets, BuildKit requests the resources that it needs from Buildx.

```
docker build -t myapp:latest .

```

- Docker CLI sends a build request to the Docker engine.
- You typically get a single-platform image loaded into your local Docker image store.

Simple mental model: “Docker CLI asks the local Docker engine to build; engine uses BuildKit (kitchen) if enabled

## Multi-platform build (the big reason people use Buildx)

```
docker buildx build --platform linux/amd64,linux/arm64 -t myapp:latest --push .
```
What happens:

- Buildx tells BuildKit: “Build for amd64 and arm64.”
- BuildKit builds separate images and produces a multi-arch manifest.
- --push is required because a multi-arch result usually can’t be “loaded” into the classic local image store as one thing.

That command builds two separate images, one for linux/amd64 and one for linux/arm64, then publishes them under one tag (myrepo/myapp:1.0) using a multi-arch manifest.

### What actually happens step-by-step

Buildx tells BuildKit to build for two platforms
  - linux/amd64 (x86_64 Intel/AMD)
  - linux/arm64 (ARM64, e.g., Apple Silicon, AWS Graviton, Raspberry Pi 64-bit)

BuildKit produces two outputs
   - Image A: contains amd64 binaries + layers
   - Image B: contains arm64 binaries + layers
   - --push uploads both images AND a “manifest list”

The manifest is like an index: “this tag has two variants; pick the one matching the client machine.”
At runtime, the correct one is auto-selected

### When you do:

docker pull myrepo/myapp:1.0

Docker checks your host CPU/OS and pulls the matching variant automatically.
 - If your machine is amd64 → it pulls the amd64 image.
 - If your machine is arm64 → it pulls the arm64 image.
It creates two images under one tag. The tag points to a manifest that chooses the right image for the platform.

# Refernces
- [Docker Buildx documentation](https://docs.docker.com/build/concepts/overview/)
- [Multistage docker files](https://tecadmin.net/dotnet-core-multi-stage-dockerfile/)