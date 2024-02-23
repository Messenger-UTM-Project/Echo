PROJECT_NAME := Echo

.PHONY: run updatedb build clear launch mkcert

run:
	dotnet run --project $(PROJECT_NAME)

updatedb:
	dotnet ef database update --project $(PROJECT_NAME)

build:
	dotnet build --project $(PROJECT_NAME)

clear:
	rm -rf $(PROJECT_NAME)/bin $(PROJECT_NAME)/obj

mkcert:
	mkcert -install && mkcert -cert-file $(PROJECT_NAME)/cert.pem -key-file $(PROJECT_NAME)/key.pem localhost 127.0.0.1 0.0.0.0

launch: updatedb run
