---
title: "Testing Strategy"
purpose: "Unit, integration and UI testing principles"
author: "docs_agent"
date: "2025-06-27"
---

# 🧪 Testing Strategy

A Wrecept stabilitását több szinten biztosítjuk.

## Tesztszintek

1. **Unit tesztek** – A Core és ViewModel rétegek logikáját izoláltan ellenőrizzük.
2. **Integration tesztek** – Adatbázis műveletek és szolgáltatások együttműködését vizsgáljuk SQLite in-memory módban.
   * Egy külön teszt a fizikai `app.db` fájlon fut, hogy ellenőrizzük a tényleges mentést és betöltést.
3. **UI tesztek** – WinAppDriverrel automatizáltan teszteljük a billentyűzettel vezérelt nézeteket. Részletek: [developer_guide_hu.md](manuals/developer_guide_hu.md#ui-tesztek-winappdriverrel).

## Hülyebiztos validáció

* Null-check minden publikus belépési ponton.
* Alapértelmezett értékek biztosítása, ahol a modell megengedi.
* Adatintegritás ellenőrzése a Storage rétegben tranzakciókkal.

## Coverage és CI

* Minimum 80% kódfedettségre törekszünk. A Core és ViewModel rétegek kritikus útvonalait teljesen lefedjük.
* A tesztek minden commit után futnak GitHub Actions alatt (`dotnet test`). Ha bármely teszt megbukik, a build elutasításra kerül.

*Megjegyzés: a `wrecept.db` néven szereplő adatbázis csak a migrációk tervezési szakaszában használatos.*

---
