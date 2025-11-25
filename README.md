# Redis
docker pull redis/redis-stack-server:latest
docker run -p 6379:6379 --name redis -d redis/redis-stack-server
docker ps
docker exec -it redis redis-cli

# RabitMQ
docker compose -f docker-compose.Dev.infrastructure.yaml up

<img width="671" height="601" alt="JK drawio" src="https://github.com/user-attachments/assets/8c7ff2ae-d6be-40c3-a555-515786c0a74c" />
