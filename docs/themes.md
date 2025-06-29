---
title: "WPF Themes"
purpose: "Style guide for RetroTheme"
author: "docs_agent"
date: "2025-06-27"
---

# 🎨 Retro Theme Overview

A Retro UI hangs on warm yellows and oranges reminiscent of classic terminals. The XAML resource dictionary defines global styles for common controls and ensures consistent keyboard focus cues. The main colors are `#FFE187`, `#FFD700`, and `#FFA726` against darker text.

- **StageBackground:** Warm yellow base for all windows and dialogs.
- **HoverBrush:** `#F5A623` on mouse-over states.
- **AccentBrush:** Orange used for pressed actions.
- **HighlightBrush:** Gold for active elements and focus rings.
- **ControlBackgroundBrush:** Pale yellow surface for controls.

Every control style sets `FocusVisualStyle` to display a dashed border so keyboard navigation is obvious.
