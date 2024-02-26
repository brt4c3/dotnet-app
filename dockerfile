FROM mcr.microsoft.com/dotnet/aspnet:8.0
ARG source
WORKDIR /src/app
EXPOSE 80
#COPY ${source:-obj/Docker/publish} .
ENTRYPOINT ["dotnet", " DeleteAssets.dll "]
