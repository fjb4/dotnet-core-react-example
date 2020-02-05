## Sample Concourse Pipeline for Building .NET Core React Applications

This repository contains an example of a Concourse pipeline that builds .NET Core React applications (i.e. those created in Visual Studio using the React project template or by running `dotnet new react` from the command line).

Under normal circumstances, publishing a .NET Core React application (`dotnet publish`) builds everything in one step. This includes both .NET and React artifacts. This is convenient but, in order to do this, both the .NET SDK and Node.js must be installed.

In a Concourse CI pipeline, building this in one step means you'd need to create a custom Docker image that combines both the .NET SDK and Node.js. However, if you were able to build the artifacts separately, the .NET build step could use an official .NET SDK image and the React build step could use an official Node.js image. That is what is demonstrated in this repository.

In order to build the .NET and React artifacts separately, the first step is to modify the project's .csproj (`react-example.csproj`) file and prevent it from building the React artifacts when publishing in a Release configuration. You do this by adding a condition to the `PublishRunWebpack` target:

```
Condition=" '$(Configuration)' != 'Release' "
```
Once that has been done, our Concourse pipeline (`pipeline.yml`) can use two separate tasks to build the project. The first task is `build-dotnet` which is solely responsible for generating the .NET artifacts. To do so, it uses an official .NET Core SDK image from Microsoft:

```yaml
- task: build-dotnet
  config:
      ...
      source:
          repository: mcr.microsoft.com/dotnet/core/sdk
          tag: 3.1
      run:
      path: /bin/bash
      args:
          - -c
          - |
          dotnet restore -r linux-musl-x64
          dotnet publish -c Release -o ../dotnet-artifacts -r linux-musl-x64 --self-contained false --no-restore
```

The second task is `build-node` and it builds the React/JavaScript artifacts for the project. It uses an official Node.js image:

```yaml
- task: build-node
  config:
      ...
      source:
          repository: node
          tag: 13
      run:
      path: /bin/bash
      args:
          - -c
          - |
          npm install
          npm run build
      ...
```

After these tasks execute, the pipeline combines the output of these tasks to produce the final container image, and then the image is pushed to Docker Hub.


### Deploying and Triggering the Pipeline

Note that this pipeline creates a container image and pushes it to Docker Hub. To do this, you must specify credentials for a Docker Hub repository in a file named `creds.yml`:

```yaml
docker_hub_username: <username>
docker_hub_password: <password>
```

Once you've created `creds.yml` you can set the pipeline:

```
fly -t <target> set-pipeline -c pipeline.yml -p dotnet-core-react -l creds.yml -n
```

And then trigger the pipeline to run:

```
fly -t <target> trigger-job --job dotnet-core-react/build -w 
```


### Running the Application

To run the application, execute the following command and then visit `http://localhost:8000` in your web browser:

```
docker container run -it --rm -p 8000:80 <docker-hub-username>/dotnet-core-react-example
```
