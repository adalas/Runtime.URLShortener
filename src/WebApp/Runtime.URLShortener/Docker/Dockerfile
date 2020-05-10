FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
RUN apt-get update -yq \
    && apt-get install curl gnupg -yq \
    && curl -sL https://deb.nodesource.com/setup_10.x | bash \
    && apt-get install nodejs -yq

WORKDIR /app
EXPOSE 80
EXPOSE 443

COPY src .
WORKDIR "/app/WebApp/Runtime.URLShortener"
RUN dotnet restore "Runtime.URLShortener.csproj"
RUN dotnet build "Runtime.URLShortener.csproj" -c Release -o /app/build
WORKDIR /app/build/ClientApp 
COPY  src/WebApp/Runtime.URLShortener/ClientApp .
run npm install
run npm run-script build
WORKDIR "/app/WebApp/Runtime.URLShortener"


RUN dotnet publish "Runtime.URLShortener.csproj" -c Release -o /app/build


FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS runtime
RUN apt-get update -yq \
    && apt-get install curl gnupg -yq \
    && curl -sL https://deb.nodesource.com/setup_10.x | bash \
    && apt-get install nodejs -yq
WORKDIR /app
COPY --from=build /app/build ./



ENTRYPOINT ["dotnet", "Runtime.URLShortener.dll"]