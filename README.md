# Messenger 

<div align="center"><p>
    <a href="https://github.com/Messenger-UTM-Project/Echo/pulse">
      <img alt="Last commit" src="https://img.shields.io/github/last-commit/Messenger-UTM-Project/Echo?style=for-the-badge&logo=starship&color=8bd5ca&logoColor=D9E0EE&labelColor=302D41"/>
    </a>
    <a href="https://github.com/Messenger-UTM-Project/Echo/blob/main/LICENSE">
      <img alt="License" src="https://img.shields.io/github/license/Messenger-UTM-Project/Echo?style=for-the-badge&logo=starship&color=ee999f&logoColor=D9E0EE&labelColor=302D41" />
    </a>
    <a href="https://github.com/Messenger-UTM-Project/Echo">
      <img alt="Repo Size" src="https://img.shields.io/github/repo-size/Messenger-UTM-Project/Echo?color=%23DDB6F2&label=SIZE&logo=codesandbox&style=for-the-badge&logoColor=D9E0EE&labelColor=302D41" />
    </a>
</p></div>

### Table of contents
- [Usage](#usage)
- [Docs](https://github.com/Messenger-UTM-Project/Docs)

## Usage
1. **Docker**
    - `docker compose up`
2. **Manual**
    - Set up postgreSQL DB and create/edit `db.json`
    - `dotnet tool install dotnet-ef --version 8.0.1 && 
        dotnet dotnet-ef migrations add InitialCreate || true && 
        dotnet dotnet-ef database update && 
        make launch`
