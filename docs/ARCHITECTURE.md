---
title: "Architecture Blueprint"
purpose: "Overall module and data flow overview"
author: "docs_agent"
date: "2025-06-27"
---

# 🏗️ Architecture Blueprint

Az alkalmazás rétegei tisztán el vannak választva, hogy a karbantarthatóság és a bővíthetőség hosszú távon biztosított legyen.

## Rétegzett felépítés

1. **UI (Views / Themes)** – XAML nézetek és stílusfájlok. Csak vizuális elemeket tartalmaz, logikát nem.
2. **ViewModel** – A CommunityToolkit.Mvvm segítségével kezeli a felhasználói interakciókat és az adatkötéseket.
3. **Core** – Domain modellek, szolgáltatás interfészek és belső számítások.
4. **Storage** – SQLite + Entity Framework Core konténer, migrációk, repositoryk.

Minden réteg csak az alatta lévőt éri el, közvetlen átjárás nem megengedett.

## Adatáramlás

```
UI → ViewModel → Core → Storage
```

A felhasználói események a ViewModelen keresztül jutnak el a Core-hoz, amely szükség esetén meghívja a Storage réteg szolgáltatásait.

### Adatáramlás részletesen

1. **View** – A felhasználói műveletek (billentyűleütések) eseményeket generálnak.
2. **ViewModel** – A `RelayCommand` hívásain keresztül validálja az adatot, majd meghívja a Core szolgáltatásait.
3. **Core** – Feldolgozza a kérést, számításokat végez, majd repositorykat hív.
4. **Storage** – Az adatbázisműveletek végrehajtása után visszatér a Core réteghez, amely a ViewModelen keresztül értesíti a View-t.

## EF Core kezelése

Az `DbContext` példányai a Storage rétegben élnek. A migrációk és a sémafrissítések parancssori eszközzel, CI környezetben futnak. A ViewModel soha nem fér közvetlenül az adatbázishoz.

---
