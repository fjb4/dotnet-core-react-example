# .NET Core React Application

Deployment instructions for pushing a React application (one built with the .NET Core React template) to Cloud Foundry.

This has been tested with .NET Core 2.2 and Node.js 10.

### Cloud Foundry Deployment Steps
- Build both the .NET and Node.js portions of the application by running `dotnet publish -o publish`
- Deploy the application artifacts from the ./publish directory to Cloud Foundry `cf push react-example --random-route -p publish`
