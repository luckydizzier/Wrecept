using System;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Threading;
using Microsoft.Extensions.DependencyInjection;
using Wrecept.Wpf.ViewModels;
using Wrecept.Core.Services;
using Wrecept.Core.Utilities;
using Wrecept.Wpf;
using Wrecept.Wpf.Services;

namespace Wrecept.Wpf.Views;

public partial class InvoiceEditorView : UserControl
{

    public InvoiceEditorView()
    {
        if (App.Provider is null)
        {
            InitializeComponent();
            return;
        }
        Initialize(App.Provider.GetRequiredService<InvoiceEditorViewModel>());
    }

    public InvoiceEditorView(InvoiceEditorViewModel viewModel)
    {
        Initialize(viewModel);
    }

    private void Initialize(InvoiceEditorViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
        Loaded += async (_, _) =>
        {
            var progressVm = new ProgressViewModel();
            var progressWindow = new StartupWindow { DataContext = progressVm };
            progressWindow.Show();
            var progress = new Progress<ProgressReport>(r =>
            {
                progressVm.GlobalProgress = r.SubtaskPercent;
                progressVm.SubProgress = r.SubtaskPercent;
                progressVm.StatusMessage = r.Message;
            });
            await viewModel.LoadAsync(progress);
            progressWindow.Close();
        };
    }


}
