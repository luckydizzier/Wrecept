---
title: "Tesztelési stratégia"
purpose: "Egység-, integráció- és UI-tesztelési elvek"
author: "docs_agent"
date: "2025-07-08"
---

# 🧪 Tesztelési stratégia

A Wrecept stabilitását több szinten biztosítjuk.

## Tesztszintek

1. **Unit tesztek** – A Core és ViewModel rétegek logikáját izoláltan ellenőrizzük.
2. **Integration tesztek** – Adatbázis műveletek és szolgáltatások együttműködését vizsgáljuk SQLite in-memory módban.
   * Egy külön teszt a fizikai `app.db` fájlon fut, hogy ellenőrizzük a tényleges mentést és betöltést.
3. **UI tesztek** – A MAUI alkalmazást a cross-platform `MauiUITest` keretrendszerrel futtatjuk emulátorokon és fizikai eszközökön.
   * A tesztek a `tests/InvoiceApp.MAUI.Tests` projektben találhatók.

## Hülyebiztos validáció

* Null-check minden publikus belépési ponton.
* Alapértelmezett értékek biztosítása, ahol a modell megengedi.
* Adatintegritás ellenőrzése a Storage rétegben tranzakciókkal.

## Coverage és CI

* Minimum 100% kódfedettségre törekszünk. A Core és ViewModel rétegek kritikus útvonalait teljesen lefedjük.
* A tesztek minden commit után futnak GitHub Actions alatt (`dotnet test`). Ha bármely teszt megbukik, a build elutasításra kerül.
* Kódfedettséget a `dotnet test --collect:"XPlat Code Coverage"` paranccsal mérünk.
  A CI szintén ezt használja, és a projektfájlokban szereplő `<Threshold>100</Threshold>`
  beállítás miatt bármilyen visszaesés hibát eredményez.
  A MAUI projektekhez szükséges környezetet a CI a `dotnet workload install maui` paranccsal készíti elő,
  majd `dotnet build` és `dotnet test` lépéseket futtat minden támogatott platformra.
* A kódfedettségi statisztikából kizárjuk az `InvoiceApp.Data/Migrations` mappa osztályait
  az `<ExcludeByFile>` projektbeállítással.

  *Megjegyzés: a `wrecept.db` néven szereplő adatbázis csak a migrációk tervezési szakaszában használatos.*

*2025-07-08:* Utolsó teljes lefedettségi mérés `dotnet test --collect:"XPlat Code Coverage"` parancs futtatásával 100%-ot jelzett mindhárom tesztprojektre.

---
