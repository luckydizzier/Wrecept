# ⌨️ KeyboardFlow.md

---
**title:** Keyboard Navigation Flow
**purpose:** Formal description of keyboard behavior in Wrecept
**author:** logic_agent
**date:** 2025-07-02
---

## 🧭 Navigation Principles

A Wrecept minden felületén a billentyűzet az elsődleges vezérlő eszköz. A `KeyboardManager.Handle` globálisan figyeli a `KeyDown` eseményeket és mozgatja a fókuszt. A viselkedés profil alapján szabható, nézetenként külön kezelők már nincsenek.
Az alkalmazás indításakor a `KeyboardManager` betölti a `KeyboardProfile` beállításait a `wrecept.json` fájl `Keyboard` szekciójából, így a felhasználó tetszés szerint módosíthatja a `Next`, `Previous`, `Confirm` és `Cancel` billentyűket.

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
- A fejmezőkben a `KeyboardManager` lépteti a fókuszt.
- `Escape` visszaviszi a főablak listájához.
- Az `InvoiceItemsGrid`-en `Enter` az aktuális tétel szerkesztését indítja.

### BaseMasterView
- `Enter`: kijelölt sor szerkesztése.
- `Delete`: kijelölt sor törlése.
- `Escape`: részletes nézetből vissza a listához.
Az összes mesteradat ViewModel az `EditableMasterDataViewModel` leszármazottja, így ezek a billentyűk minden listában azonos módon viselkednek.

### StageView
- `Escape`: visszateszi a fókuszt az utoljára aktivált menüpontra, ha az elérhető.
- Ha nincs korábbi menüpont, a `KeyboardManager` globális fókusz-visszaállítása lép életbe.
- A billentyűk csak akkor élnek, ha az `AppStateService` böngészés vagy szerkesztés állapotot jelez; mentés vagy hiba közben a bemenetet a StageView elnyeli.

## 📦 Modal Prompt Behavior

Az `ArchivePromptView`, `SaveLinePromptView` és `InvoiceCreatePromptView` egyaránt követi:
- `Enter` a megerősítő parancsot futtatja.
- `Escape` a mégse parancsot hívja.
- Többsoros `TextBox` (`AcceptsReturn=true`) esetén a `KeyboardManager` sem az `Enter`, sem az `Escape` billentyűt nem kezeli, így az új sor bevitele és a vezérlő saját művelete zavartalan.
A fókusz a prompt bezárása után visszatér a megnyitó nézethez.
Ezt a `FocusManager` végzi, amely nézetenként rögzíti az utoljára fókuszba került vezérlőt.

## 📋 Focus Reset Rules

Az `Escape` billentyűt a `StageView` kezeli, és a legutóbb aktivált menüelemre
állítja a fókuszt. Más nézeteknél a billentyű az adott nézet logikájára van
bízva.
Az `Enter` alapértelmezésben a következő vezérlőre ugrik, ha az aktuális kezelő nem nyeli el.

### Fókuszkövető szolgáltatás

A `FocusManager` a nézetekhez rendelt kulcs alapján megjegyzi az utoljára fókuszba került vezérlőt. A promptok vagy nézetek bezárásakor ezen keresztül állítjuk vissza a fókuszt az eredeti elemre. A szolgáltatás singletonként regisztrált, így minden View és ViewModel DI-n keresztül éri el.
Minden programozott fókuszváltáshoz **kötelező** a `FocusManager.RequestFocus` metódust használni; közvetlen `MoveFocus` vagy `Keyboard.Focus` hívás nem megengedett.
Sikertelen `.Focus()` esetén a `RequestFocus` a `System.Windows.Input.FocusManager.SetFocusedElement` hívásával próbálkozik, így a fókusz a legközelebbi fókusz-scope-ban is rögzíthető.

## 💡 Design Philosophy

A billentyűzetes navigációt a sebesség és az időtálló megszokhatóság jegyében terveztük. Minden akció egzaktul megismételhető, vizuális visszajelzéssel kombinálva.
Általánosan `KeyDown` eseményeket használunk. `PreviewKeyDown` csak azoknál a vezérlőknél szükséges, ahol a beviteli mezők saját eseményei elnyelnék a parancsokat:
1. A mesteradat listákat megjelenítő `DataGrid` (BaseMasterView) a sor műveletekhez.
2. A dialógusok (`DialogHelper`) `Enter`/`Escape` kezeléséhez.
3. Az `EditLookup` és `SmartLookup` szövegmezőinél a legördülő lista navigációjához.
- A `ListBox`, `DataGrid`, `ComboBox`, `TreeView`, valamint a `Menu` és `MenuItem` saját nyílkezelése elsőbbséget élvez; a `KeyboardManager` ilyen őst találva nem mozdít fókuszt.
- Lista vezérlőknél `KeyboardNavigation.DirectionalNavigation` értéke `None`, így a nyilak nem ugranak ki a listából.

## 🔧 Future Enhancements

- [x] AccessKey-k hozzárendelése a címkékhez
- [x] Testreszabható billentyűzetprofil `wrecept.json`-on keresztül
- [ ] `Ctrl+Z` visszavonás a sor szerkesztésben
- [ ] Tesztesetek bővítése a `TEST_MATRIX.md`-ben

---

> “Keyboards don't lie. If the app isn't predictable, the flow isn't finished.” — *root_agent*
