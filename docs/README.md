---
title: "Wrecept README"
purpose: "Project overview"
author: "root_agent"
date: "2025-06-27"
---

# 🎛️ Wrecept

[![CI](https://github.com/luckydizzier/wrecept/actions/workflows/ci.yml/badge.svg)](https://github.com/luckydizzier/wrecept/actions/workflows/ci.yml)

**A retro-modern invoice recording desktop application inspired by DOS-era logistics systems.**

---

## 📦 Project Purpose

Wrecept originally started as a Windows-only WPF application. The project has since moved to **.NET MAUI** to enable cross-platform use while keeping the retro look and predictable behavior.

---

## ✨ Features (Planned)

| Feature                          | Status                  |
| -------------------------------- | ----------------------- |
| Retro-style UI                   | ✅ Stage design complete |
| Structured top menu              | ✅ StageView in place    |
| Invoice recording (header/items) | ⏳ UI skeleton ready     |
| Product master data              | ⏳ List view available   |
| Supplier selection               | ⏳ List view available   |
| SQLite-powered persistence       | 🔒 Deferred             |
| Audio-visual feedback            | 🔒 Deferred             |
| Backup & recovery after outage   | 🔒 Deferred             |

---

## 🎹 Interface Philosophy

* **No mouse. No clutter.**
* All screens mimic DOS layouts — with color-coded panels and full-screen efficiency.
* Most of the menus are still placeholders, but product management already works.

---

## 📁 Folder Structure

```
InvoiceApp.Core/       # Domain models and service interfaces
InvoiceApp.Data/       # EF Core data access and repositories
InvoiceApp.MAUI/       # MAUI UI project
docs/                  # Documentation
tools/                 # Helper scripts
CHANGELOG.md
Wrecept.sln
```

---

## 🛠 Technologies

* Language: **C#** (.NET 8)
* UI: **.NET MAUI**
* Platform: **Windows/Linux/macOS**

---

## 🎯 Next Steps

1. Build out invoice editor UI (inspired by screenshot #3)
2. Integrate fake data into product and supplier modules
3. Discuss minimal database model based on real .dbf structure
4. Mandatory startup tasks can be found in the "Kick OFF" section of [DEV_SPECS.md](DEV_SPECS.md)

## ✅ Kick OFF

The MAUI project lives in `InvoiceApp.MAUI` and contains the following basics:

* `App.xaml` and `App.xaml.cs` – application configuration
* `MainWindow.xaml` – main window
* `App.xaml.cs` holds DI and startup logic
* `MainWindow` loads the `StageView` layout

These ensure the program runs immediately in a Windows environment.

---

## ✅ Running Tests

Tests can be run with the following command:

```bash
dotnet test tests/Wrecept.Tests/Wrecept.Tests.csproj
```

---

## 🧾 Credits

Original layout, logic and color schema: \[Egon’s legacy Clipper app]
Reconstruction by: \[ChatGPT-Dev Agent – 2025 Edition]

> "The stage is set, the mixer is ready. Time to wire things up."

---

*Work in Progress – Not intended for production use (yet).*

---

## 📚 Documentation

- [ARCHITECTURE.md](ARCHITECTURE.md) – Layers and data flow
- [AGENTS.md](AGENTS.md) – Agent responsibilities
- [CODE_STANDARDS.md](CODE_STANDARDS.md) – Coding guidelines
- [DEV_SPECS.md](DEV_SPECS.md) – Development specification
- [ERROR_HANDLING.md](ERROR_HANDLING.md) – Fault tolerance
- [FAULT_PLAN.md](FAULT_PLAN.md) – Fault injection plan
- [TEST_STRATEGY.md](TEST_STRATEGY.md) – Test strategy
- [PROJECT_STRUCTURE.md](PROJECT_STRUCTURE.md) – File-level overview
- [MAUI_PORT_PREP.md](MAUI_PORT_PREP.md) – WPF audit and MAUI migration notes
- [../.git-branch-policy.md](../.git-branch-policy.md) – Git branch policy
- [about.md](about.md) – Program information (EN)
- [release_notes_0_0_1.md](release_notes_0_0_1.md) – Initial MVP release notes
- [manuals/developer_guide_hu.md](manuals/developer_guide_hu.md) – Developer guide (HU)
- [manuals/user_manual_hu.md](manuals/user_manual_hu.md) – User manual (HU)
