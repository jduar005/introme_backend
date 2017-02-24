FROM microsoft/dotnet:latest

# Author
MAINTAINER J.P Duarte <jpjuanduarte@gmail.com>

# create working folder 
RUN mkdir /app

# copy the dll to docker 
COPY bin/ /app/

# Environment Setup
WORKDIR /app

#RUN ["dotnet", "restore"]
#RUN ["dotnet", "build"]

# expose and make sure .NET binds port
EXPOSE 5000
ENV ASPNETCORE_URLS http://*:5000

# run the application
ENTRYPOINT ["dotnet", "Intro.Web.dll"]