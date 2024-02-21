PROJECT_NAME := Echo

.PHONY: run migrate updatedb build clean

run:
	dotnet run --project $(PROJECT_NAME)

migrate:
	dotnet ef migrations add MigrationName --project $(PROJECT_NAME)

updatedb:
	dotnet ef database update --project $(PROJECT_NAME)

build:
	dotnet build --project $(PROJECT_NAME)

clean:
	rm -rf $(PROJECT_NAME)/bin $(PROJECT_NAME)/obj
