using CommunityToolkit.Maui;
using Microsoft.Maui.LifecycleEvents;
#if WINDOWS
using Microsoft.UI;
using Microsoft.UI.Windowing;
using Windows.Graphics;
using CommunityToolkit.Mvvm.Messaging;
using Code;
#endif


namespace MauiApp4;

public static class MauiProgram
{
#if WINDOWS
    static void OnSizeChanged(object sender, Microsoft.UI.Xaml.WindowSizeChangedEventArgs args)
    {
        ILifecycleEventService service =
MauiWinUIApplication.Current.Services.GetRequiredService<ILifecycleEventService>();
        service.InvokeEvents(nameof(Microsoft.UI.Xaml.Window.SizeChanged));

    }
#endif




    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {

                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });
        builder.UseMauiCommunityToolkit(options =>
        {

            options.SetShouldEnableSnackbarOnWindows(true);
        });

        builder.ConfigureLifecycleEvents(events =>
         {
#if WINDOWS
events.AddWindows(windows => windows
.OnWindowCreated(window =>
{
window.SizeChanged += OnSizeChanged;



   IntPtr handle = WinRT.Interop.WindowNative.GetWindowHandle(window);
 
                        var id = Microsoft.UI.Win32Interop.GetWindowIdFromWindow(handle);
 
                        var appWindow = Microsoft.UI.Windowing.AppWindow.GetFromWindowId(id);                       
 
                        var p = appWindow.Presenter as OverlappedPresenter;
 
                        var titleBar = appWindow.TitleBar;
 
                         //detailpage events
 
                          appWindow.Changed += (sender, args) => {
 
                    if(args.DidSizeChange)
 
                    {
 
                        OverlappedPresenterState res = p.State;
 
                        if (res == OverlappedPresenterState.Maximized)
 
                        {
// this will be invoked when maxmized button clicked
 
WeakReferenceMessenger.Default.Send(new DetailPageStatus("Maximized"));
 
 
                        }
 
                        else if (res == OverlappedPresenterState.Minimized)
 
                        {
// this will be invoked when Minimized button clicked
 
WeakReferenceMessenger.Default.Send(new DetailPageStatus("Minimized"));
 
 
                        }
 
                        else if(res == OverlappedPresenterState.Restored) {
 
// this will be invoked when maxmized button clicked second time, the state is restored.

WeakReferenceMessenger.Default.Send(new DetailPageStatus("Restored"));
 
                                  }
 
                              }

};









}));






events.AddEvent(nameof(Microsoft.UI.Xaml.Window.SizeChanged), () =>
LogEvent("Window SizeChanged"));





#endif
             static bool LogEvent(string eventName, string type = null)
             {
                 System.Diagnostics.Debug.WriteLine($"Lifecycle event: {eventName}{(type ==
                 null ? string.Empty : $" ({type})")}");
                 return true;
             }
         });



        return builder.Build();
    }
}
