using System;
using Microsoft.Maui.Controls;
using CommunityToolkit.Mvvm.Input;
using InvoiceApp.Core.Enums;
using InvoiceApp.MAUI.ViewModels;

namespace InvoiceApp.MAUI.Services;

public class StageMenuKeyboardHandler : IKeyboardHandler
{
    private readonly StageViewModel _stage;
    private readonly StageMenuAction[] _actions = Enum.GetValues<StageMenuAction>();
    private int _index;

    public StageMenuKeyboardHandler(StageViewModel stage)
    {
        _stage = stage;
    }

    public bool HandleKey(KeyEventArgs e)
    {
        switch (e.Key)
        {
            case Key.Up:
                _index = (_index - 1 + _actions.Length) % _actions.Length;
                _stage.StatusBar.ActiveMenu = _actions[_index].ToString();
                return true;
            case Key.Down:
                _index = (_index + 1) % _actions.Length;
                _stage.StatusBar.ActiveMenu = _actions[_index].ToString();
                return true;
            case Key.Insert:
            case Key.Enter or Key.Return:
                var action = _actions[_index];
                _stage.HandleMenuCommand.Execute(action);
                return true;
        }
        return false;
    }
}
