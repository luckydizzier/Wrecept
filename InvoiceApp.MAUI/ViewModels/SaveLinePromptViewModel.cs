using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using InvoiceApp.MAUI.Views;
using Microsoft.Extensions.DependencyInjection;

namespace InvoiceApp.MAUI.ViewModels;

public partial class SaveLinePromptViewModel : ObservableObject
{
    private readonly InvoiceEditorViewModel _parent;
    private readonly bool _finalize;

    public SaveLinePromptViewModel(InvoiceEditorViewModel parent,
        string message = "Mentsem ezt a sort? (Enter=Igen, Esc=Nem)",
        bool finalize = false)
    {
        _parent = parent;
        Message = message;
        _finalize = finalize;
    }

    public string Message { get; }

    [RelayCommand]
    private async Task ConfirmAsync()
    {
        if (_finalize)
            await _parent.FinalizeInvoiceCommand.ExecuteAsync(null);
        else
            await _parent.ItemsEditor.AddLineItemCommand.ExecuteAsync(null);
        _parent.SavePrompt = null;
        _parent.IsInLineFinalizationPrompt = false;
    }

    [RelayCommand]
    private void Cancel()
    {
        _parent.SavePrompt = null;
        _parent.IsInLineFinalizationPrompt = false;
    }
}

