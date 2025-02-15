# Docker - Redis
docker pull redis/redis-stack-server:latest
docker run -p 6379:6379 --name redis -d redis/redis-stack-server
docker ps
docker exec -it redis redis-cli