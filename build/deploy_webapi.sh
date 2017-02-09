docker stop webapi 
docker rm webapi 

docker run -d -p 5000:5000 --name=webapi webapi:v1.0 