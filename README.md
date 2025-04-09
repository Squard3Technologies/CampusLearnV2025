# CampusLearnV2025




#Posgress Setup Docker 
(https://www.dbvis.com/thetable/how-to-set-up-postgres-using-docker/)
======================================================================

##Pull Posgress Image
- docker pull postgres


##Step 2 - Create a Docker volume
- docker volume create postgres_data


##Step 3 - Run the Postgres Docker container 
- docker run --name postgres_container -e POSTGRES_PASSWORD=sen371 -d -p 5432:5432 -v postgres_data:/var/lib/postgresql/data postgres


##Step 4 - Verify the Container is running
- docker ps


##Step 5 - Connect to the Postgres database
- use any tool:
	- DataGrip
	- DBBeaver	
	
##Step 6 - Starting or stopping or removing the postgres container
- Start		: 	docker start postgres_container
- Stop		: 	docker stop postgres_container
- Remove	:	docker rm postgres_container
	
