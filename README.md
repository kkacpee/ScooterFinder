# ScooterFinder
Przed uruchomieniem aplikacji należy w pliku ScooterFinderApi/ServerApi/appsettings.json zmienić ConnectionString do bazy danych.

Do aplikacji ScooterFinder przygotowane zostały pliki Dockerfile oraz docker-compose.yml pozwalające na uruchomienie aplikacji w kontenrze.
W celu zbudowania obrazu należy użyc komendy:
```
docker build {nazwa_obrazu} .
```
znajdując się w folderze zawierajacym plik dockerfile.

W celu wykorzystania pliku docker-compose.yml należy zmienić źródło kontenera webapi na nazwę utworzonego przez siebie obrazu.
Po tej zmianie serwer wraz z bazę można uruchomić komendą:
```
docker compose up
```
znajdując się w folderze zawierajacym plik docker-compose.yml.


Aplikacja serwera tworzy REST API wystawiające poniższe endpointy:
![alt text](https://github.com/kkacpee/ScooterFinder/blob/main/swgr.png?raw=true)
