# ⌨️ KeyboardFlow.md

---
**title:** Keyboard Navigation Flow
**purpose:** Formal description of keyboard behavior in Wrecept
**author:** logic_agent
**date:** 2025-07-02
---

## 🧭 Navigation Principles

A Wrecept minden felületén a billentyűzet az elsődleges vezérlő eszköz. A `NavigationHelper.Handle` segít az általános, fokozatmentes fókuszmozgatásban, míg az egyes nézetek saját `KeyDown` kezelőkkel finomítják a viselkedést.

## 🔑 Key Bindings Overview

| Key      | Globális hatás                     | Lokális környezet                        | Megjegyzés |
|---------|-----------------------------------|------------------------------------------|------------|
| `Enter` | Fókusz a következő vezérlőre       | Sor megerősítés, inline létrehozó indítása |            |
| `Down`  | Fókusz a következő vezérlőre       | Lista vagy rács navigációja              | „Enter” tükre |
| `Up`    | Fókusz az előző vezérlőre          | Előző szerkeszthető mezőre lép            |            |
| `Escape`| Fókusz vissza a főablakra          | Prompt vagy modal bezárása               |            |
| `Delete`| –                                 | Mesterlistákban tétel törlése            | `BaseMasterView` köti |
| `Tab`   | Alapértelmezett tabuláció         | Csak modális vagy inline szerkesztőben módosított | |

## 🧾 View-Specific Flow

### InvoiceEditorView
- A fejmezőkben `Enter` vagy `Down` továbbítja a fókuszt.
- `Escape` visszaviszi a főablak listájához.
- Az „Inline Item Entry” sor a `OnEntryKeyDown` eseményt használja:
  - `Enter` az utolsó mezőben (jelölés: `Tag="LastEntry"`) meghívja az `AddLineItemCommand`-et.
  - Egyébként a `NavigationHelper` lép közbe.
- Az `InvoiceItemsGrid`-en `Enter` az aktuális tétel szerkesztését indítja.

### BaseMasterView
- `Enter`: kijelölt sor szerkesztése.
- `Delete`: kijelölt sor törlése.
- `Escape`: részletes nézetből vissza a listához.
Az összes mesteradat ViewModel az `EditableMasterDataViewModel` leszármazottja, így ezek a billentyűk minden listában azonos módon viselkednek.
Az InputBindingek helyett a rács `PreviewKeyDown` eseménye futtatja a parancsokat,
így szövegmezők szerkesztésekor az `Enter` nem zárja le véletlenül a részleteteket.

### StageView
- `Escape`: visszateszi a fókuszt az utoljára aktivált menüpontra, ha az elérhető.
- Ha nincs korábbi menüpont, a `NavigationHelper` globális fókusz-visszaállítása lép életbe.

## 📦 Modal Prompt Behavior

Az `ArchivePromptView`, `SaveLinePromptView` és `InvoiceCreatePromptView` egyaránt követi:
- `Enter` a megerősítő parancsot futtatja.
- `Escape` a mégse parancsot hívja.
- Többsoros `TextBox` (`AcceptsReturn=true`) esetén a `NavigationHelper` sem az `Enter`, sem az `Escape` billentyűt nem kezeli, így az új sor bevitele és a vezérlő saját művelete zavartalan.
A fókusz a prompt bezárása után visszatér a megnyitó nézethez.
Ezt a `FocusTrackerService` végzi, amely nézetenként rögzíti az utoljára fókuszba került vezérlőt.

## 📋 Focus Reset Rules

Globálisan az `Escape` csak a főablakból indítva helyezi a fókuszt vissza a főablakra:
```csharp
Application.Current.MainWindow?.Focus();
```
Más ablakokban a billentyűt a saját logikájuk kezeli.
Az `Enter` alapértelmezésben a következő vezérlőre ugrik, ha az aktuális kezelő nem nyeli el.

### Fókuszkövető szolgáltatás

Az `IFocusTrackerService` a nézetekhez rendelt kulcs alapján megjegyzi az utoljára fókuszba került vezérlőt. A promptok vagy nézetek bezárásakor a `FormNavigator` ennek segítségével állítja vissza a fókuszt az eredeti elemre. A szolgáltatás singletonként regisztrált, így minden View és ViewModel DI-n keresztül éri el.

## 💡 Design Philosophy

A billentyűzetes navigációt a sebesség és az időtálló megszokhatóság jegyében terveztük. Minden akció egzaktul megismételhető, vizuális visszajelzéssel kombinálva.
Általánosan `KeyDown` eseményeket használunk. `PreviewKeyDown` csak azoknál a vezérlőknél szükséges, ahol a beviteli mezők saját eseményei elnyelnék a parancsokat:
1. A mesteradat listákat megjelenítő `DataGrid` (BaseMasterView) a sor műveletekhez.
2. A dialógusok (`DialogHelper`) `Enter`/`Escape` kezeléséhez.
3. Az `EditLookup` és `SmartLookup` szövegmezőinél a legördülő lista navigációjához.
- A `ListBox`, `DataGrid`, `ComboBox`, `TreeView`, valamint a `Menu` és `MenuItem` saját nyílkezelése elsőbbséget élvez; a `NavigationHelper` ilyen őst találva nem mozdít fókuszt.

## 🔧 Future Enhancements

- [ ] AccessKey-k hozzárendelése a címkékhez
- [ ] Testreszabható billentyűzetprofil `wrecept.json`-on keresztül
- [ ] `Ctrl+Z` visszavonás a sor szerkesztésben
- [ ] Tesztesetek bővítése a `TEST_MATRIX.md`-ben

---

> “Keyboards don't lie. If the app isn't predictable, the flow isn't finished.” — *root_agent*
