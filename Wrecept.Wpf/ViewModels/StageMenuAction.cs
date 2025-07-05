namespace Wrecept.Wpf.ViewModels
{
    public enum StageMenuAction
    {
        // Sz�ml�k menu
        InboundDeliveryNotes,
        UpdateInboundInvoices,
        
        // T�rzsek menu
        EditProducts,
        EditProductGroups,
        EditSuppliers,
        EditVatKeys,
        EditPaymentMethods,
        EditUnits,
        
        // List�k menu
        ListProducts,
        ListSuppliers,
        ListInvoices,
        InventoryCard,
        
        // Szerviz menu
        CheckFiles,
        AfterPowerOutage,
        ScreenSettings,
        PrinterSettings,
        EditUserInfo,
        
        // N�vjegy menu
        UserInfo,
        
        // V�ge menu
        ExitApplication
    }
}