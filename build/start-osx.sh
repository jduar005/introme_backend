#!/bin/sh
# Retrieve images
docker pull salgat/elasticsearch-head
docker pull salgat/elk-cors

docker-compose down

# Build local Docker image of latest DynamoDB
cd dynamodb
docker build -t dynamodb-latest .
cd ..

dockerIp=$(ifconfig | grep 'inet 192' | cut -d ' ' -f 2)
echo Exporting DOCKER_IP=$dockerIp
export DOCKER_IP=$dockerIp
docker-compose up -d 

echo "\n\nLet's cook... $dockerIp \n"
