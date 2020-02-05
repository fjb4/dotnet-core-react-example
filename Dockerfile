FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-alpine
WORKDIR /app

COPY /dotnet-artifacts /node-artifacts ./

ENTRYPOINT ["./react-example"]
