cd ../src/Intro.Web

dotnet publish

cd ../../build

mkdir -p Dockerfile_webapi/bin/

rm -rf Dockerfile_webapi/bin/*

cp -r ../src/Intro.Web/bin/Debug/netcoreapp1.0/publish/*  Dockerfile_webapi/bin/

cd Dockerfile_webapi/

docker build -t webapi:v1.0 .

