docker-compose.exe stop
docker-compose.exe rm -f

DOCKER_IP=$(echo $DOCKER_HOST | cut -d':' -f2 | sed 's/\/\///')

../forward.sh -D

../forward.sh 27017

DOCKER_IP=$DOCKER_IP docker-compose up