FROM mcr.microsoft.com/dotnet/sdk:7.0-bookworm-slim as build
WORKDIR /app
EXPOSE 80
COPY Shared/TelephoneDirectory.Shared/*.csproj Shared/TelephoneDirectory.Shared/
COPY Services/PersonContact/TelephoneDirectory.PersonContact.API/*.csproj Services/PersonContact/TelephoneDirectory.PersonContact.API/
RUN dotnet restore Services/PersonContact/TelephoneDirectory.PersonContact.API/*.csproj
COPY . .
RUN dotnet publish Services/PersonContact/TelephoneDirectory.PersonContact.API/*.csproj -c Release -o /out
FROM mcr.microsoft.com/dotnet/aspnet:7.0-bookworm-slim as runtime
WORKDIR /app
COPY --from=build /out .
ENTRYPOINT [ "dotnet","TelephoneDirectory.PersonContact.API.dll" ]