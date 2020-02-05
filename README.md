# .NET Core React Application

Note that this has been tested with .NET Core 3.1 and Node.js 13.7.

In this example, the .csproj file has been modified from the default React project template behavior, so that the "PublishRunWebpack" target is conditionally executed:

```xml
<Target Name="PublishRunWebpack" AfterTargets="ComputeFilesToPublish" Condition=" '$(Configuration)' != 'Release' ">
```

Conditionally executing the "PublishRunWebpack" target allows you to control when the React artifacts are built. In this case, both the .NET and React artifacts are generated for debug builds, but only the .NET artifacts are generated for release builds. This could be useful for situations where you want to generate the React artifacts separately.

Note that this uses a condition based on whether or not you are creating a release build. This is just an example, and it may make more sense to base the condition on something else.

## Debug Builds

Debug builds are unchanged from their normal behavior in a React project template. In the example below, because debug is specified, both .NET and React artifacts will be generated:

```
dotnet publish -o publish -c Debug

# deploy the contents of the publish folder to Cloud Foundry
cf push react-example --random-route -p publish
```

## Release Builds

When making a release build, only the .NET artifacts will be generated. Because of this, running `dotnet publish` for release builds does not require Node.js or NPM to be installed. However, the React artifacts will need to be built separately.

```
dotnet publish -o publish -c Release

# above step didn't generate React artifacts, so we'll need to do it ourselves
(cd ClientApp && npm install)
(cd ClientApp && npm run build)

# now copy the React artifacts to the publish folder
mkdir -p ./publish/ClientApp/build/ && cp -R ./ClientApp/build/* ./publish/ClientApp/build/

# deploy the contents of the publish folder to Cloud Foundry
cf push react-example --random-route -p publish
```
