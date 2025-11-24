# Redis
docker pull redis/redis-stack-server:latest
docker run -p 6379:6379 --name redis -d redis/redis-stack-server
docker ps
docker exec -it redis redis-cli

# RabitMQ
docker compose -f docker-compose.Dev.infrastructure.yaml up

<img width="671" height="601" alt="DistributeSystemArt" src="https://github.com/user-attachments/assets/d572d943-1c42-49d3-b049-e3ae9b98f18b" />
