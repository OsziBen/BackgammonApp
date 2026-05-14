# Szakdolgozat – Backgammon közösség- és online versenyszervező webalkalmazás

Ez a repository tartalmazza a **Széchenyi István Egyetem** **Mérnökinformatikus BSc** szakos hallgatójaként készített szakdolgozatomhoz tartozó kódokat, dokumentumokat és jegyzeteket.

## Téma

**Cím:** Backgammon közösség- és online versenyszervező webalkalmazás
**Témavezető:** Dr. Galli Richárd, egyetemi adjunktus, szakmentor
Máté Richárd, sportszervező
**Hallgató:** Olaszi Bence
**Év:** 2024/25/02 - 2025/26/02

## Futtatás és telepítés

### Backend futtatása Dockerrel

A backend alkalmazás Docker image-ből is futtatható.

#### Image buildelése:

```bash
docker build -t backgammon-app .

```

#### Container futtatása:

```
docker run -p 8080:8080 backgammon-app
```

#### Connection string beállítása

A backend konfigurációja az appsettings.json fájlban található.

Példa konfiguráció:

```
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=backgammon_db;Username=postgres;Password=your_password"
  }
}
```

#### Backend futtatása Docker nélkül

Ha lokálisan futtatod:

```
dotnet restore
dotnet build
dotnet run
```

#### Frontend futtatása (Angular)

Függőségek telepítése:

```
npm install
```

Fejlesztői szerver indítása:

```
ng serve -o
```

Az alkalmazás elérhető:

```
http://localhost:4200
```

#### Alapértelmezett portok

```
Backend: 8080
Frontend: 4200
PostgreSQL: 5432
```

#### Megjegyzések

Győződj meg róla, hogy a PostgreSQL fut a backend indulása előtt
Docker esetén a localhost a host gépre mutat
Ha konténerben fut a backend és DB együtt, akkor külön hálózat vagy service name szükséges (jelen setupban nem használva)

# Repository klónozása

git clone https://github.com/OsziBen/Szakdolgozat.git
cd Szakdolgozat
...

## Kapcsolat

**Email:** olaszibence@gmail.com
