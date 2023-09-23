FROM mcr.microsoft.com/dotnet/sdk:7.0-bookworm-slim as build
WORKDIR /app
EXPOSE 80
COPY Shared/TelephoneDirectory.Shared/*.csproj Shared/TelephoneDirectory.Shared/
COPY Services/ContactReport/TelephoneDirectory.ContactReport.Core/*.csproj Services/ContactReport/TelephoneDirectory.ContactReport.Core/
COPY Services/ContactReport/TelephoneDirectory.ContactReport.Repository/*.csproj Services/ContactReport/TelephoneDirectory.ContactReport.Repository/
COPY Services/ContactReport/TelephoneDirectory.ContactReport.Service/*.csproj Services/ContactReport/TelephoneDirectory.ContactReport.Service/
COPY Services/ContactReport/TelephoneDirectory.ContactReport.API/*.csproj Services/ContactReport/TelephoneDirectory.ContactReport.API/
RUN dotnet restore Services/ContactReport/TelephoneDirectory.ContactReport.API/*.csproj
COPY . .
RUN dotnet publish Services/ContactReport/TelephoneDirectory.ContactReport.API/*.csproj -c Release -o /out
FROM mcr.microsoft.com/dotnet/aspnet:7.0-bookworm-slim as runtime
WORKDIR /app
COPY --from=build /out .
ENTRYPOINT [ "dotnet","TelephoneDirectory.ContactReport.API.dll" ]