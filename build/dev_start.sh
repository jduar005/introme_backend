docker-compose.exe stop
docker-compose.exe rm -f

# docker pull salgat/elasticsearch-head
# docker pull salgat/elk-cors

# Build local Docker image of application
./build_webapi.sh

DOCKER_IP=$(echo $DOCKER_HOST | cut -d':' -f2 | sed 's/\/\///')

./forward.sh -D 5000
./forward.sh -D 9200
./forward.sh -D 5601
./forward.sh -D 9100

./forward.sh -i 0.0.0.0 9200
./forward.sh 5601
./forward.sh 5000
./forward.sh 9100

DOCKER_IP=$DOCKER_IP docker-compose up
