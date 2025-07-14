---
title: "Hibakezelés"
purpose: "Futásidejű és fordítási védelmi szabályok"
author: "docs_agent"
date: "2025-06-27"
---

# 🚨 Hibakezelési terv

Ez a dokumentum összefoglalja a hibakezelési stratégiát. Cél, hogy az alkalmazás összeomlás nélkül, jól nyomon követhetően működjön.

## Kivételkezelés

* A magas szintű beavatkozási pontokon (ViewModel, Service) használunk `try`-`catch` blokkot.
* Csak a szükséges adatot logoljuk, hogy érzékeny információ ne kerüljön ki.
* Kivétel elnyelése helyett visszajelzést adunk a felhasználónak a `feedback_agent` segítségével.

## Aktív védelem

* **Runtime**: Minden bemenetet validálunk a modellbe kerülés előtt. Default értékeket használunk null helyett.
* **Compile-time**: `Nullable` engedélyezett, figyelmeztetések tiltása nem megengedett.

## Naplózás

* A `ILogService` a `%AppData%/InvoiceApp/logs` könyvtárba ír naplóbejegyzéseket.
* A Storage réteg hibáit a `SerilogLogService` rögzíti gördülő JSON fájlokba
  (5 × 5 MB).
* Kritikus hiba esetén a felhasználó dönthet a folytatásról vagy kilépésről.

## Konkrét példák

1. **Adatbázis fájl hiánya vagy hiányzó elérési út** – Ha az adatbázis helye nincs megadva vagy az `app.db` nem található, a Storage réteg a `%AppData%/InvoiceApp/app.db` fájlt hozza létre, majd figyelmeztető üzenetet jelenítünk meg.
2. **Üres adatbázis** – Ha egyetlen táblában sincs adat, minta rekordokat szúrunk be és figyelmeztetjük a felhasználót.
3. **Sémahibák indításkor** – A `DatabaseInitializer` a `DbInitializer` segítségével futtatja a migrációkat és `PRAGMA journal_mode=WAL` parancsot ad ki. A `DataSeeder` külön kontextust használ, így a DI-ből kapott példány nem marad használatban.
4. **Sérült konfigurációs fájl** – A `settings.json` olvasásakor `JsonException` vagy `IOException` esetén hibaüzenetet írunk a naplóba és alapértelmezett beállításokkal folytatjuk.
5. **Sérült állapotfájl** – A `state.json` nem olvasható vagy hiányos, ekkor figyelmeztetés nélkül alapértelmezett nézetre és számlára állunk vissza.
6. **Sérült import fájl** – Hibás formátumú vagy hiányzó adatfájl betöltésekor megszakítjuk a folyamatot, naplózzuk a fájl nevét és a kiváltó hibát, és lehetőséget adunk új fájl kiválasztására.
7. **Hálózati kimaradás** – Külső frissítések letöltése közben kapcsolatvesztés esetén újrapróbálkozunk, majd offline módra váltunk, miközben a felhasználót tájékoztatjuk.
8. **Sikertelen adatbázis írás** – Ha a fájl zárolt vagy elfogy a tárhely, hibaüzenetet jelenítünk meg, a műveletet naplózzuk, majd biztonsági mentés után újrapróbáljuk.
9. **Indítási hiba** – Ha a `DataSeeder` másodszori próbálkozásra is `SqliteException`-t kap, a részleteket az `ILogService` naplózza a `logs` mappába, majd hibaüzenetet jelenítünk meg.
10. **Egyéb inicializációs hiba** – A `DatabaseInitializer` általános kivételt is naplóz. Ha a migráció sikertelen, a program leáll.
11. **Hiányzó tábla új modell után** – A `DataSeeder` felismeri a `no such table` hibát, ismét migrációt futtat és naplózza az eseményt.
12. **Sérült adatbázis fájl** – A `DatabaseRecoveryService` futtatja a `PRAGMA integrity_check` parancsot. Hiba esetén az `app.db` a `backup/corrupt_YYYYMMDD.db` néven elmentésre kerül, a séma újraépül és a `ChangeLog` alapján állítjuk vissza az adatokat.

*Megjegyzés: a `wrecept.db` fájlt kizárólag fejlesztés közbeni migrációkhoz használjuk.*

---
