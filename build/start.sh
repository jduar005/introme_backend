# Build local Docker image of application
cd Dockerfile_webapi/
docker build -t webapi:v1.0 .
cd ../

./compose.sh
