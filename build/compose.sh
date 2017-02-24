docker-compose.exe stop
docker-compose.exe rm -f

# docker pull salgat/elasticsearch-head
# docker pull salgat/elk-cors

DOCKER_IP=$(echo $DOCKER_HOST | cut -d':' -f2 | sed 's/\/\///')

./forward.sh -D

./forward.sh -i 0.0.0.0 9200
./forward.sh 5601
./forward.sh 5000
./forward.sh 9100
./forward.sh 27017

DOCKER_IP=$DOCKER_IP docker-compose up