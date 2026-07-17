using System.Windows;
using System.Windows.Input;

namespace TruStretch
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);


            bool shiftHeld =
                Keyboard.IsKeyDown(Key.LeftShift) ||
                Keyboard.IsKeyDown(Key.RightShift);



            // SHIFT CLICK = GUI ONLY
            if (shiftHeld)
            {
                MainWindow window = new MainWindow();
                window.Show();

                return;
            }



            // NORMAL CLICK = TOGGLE RESOLUTION
            ResolutionManager.ApplySmartToggle();


            Shutdown();
        }
    }
}