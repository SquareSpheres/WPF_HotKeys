# HotKeysLib

HotKeysLib is a micro library for setting global hotkeys in a wpf(Windows Presentation Foundation) application. Save yourself the trouble of calling unmanaged code, and bothering with the windows API. 


### Installation

just download the code and compile into a dll, use diretctly in your code.

### Example Usage

Register CTRL-ALt-8 as a hotkey. When the hotkey is pressed, it will simplt print out some information.

```cs
 public partial class MainWindow : Window
    {

        IHotKeyManager manager = HotKeyManager.GetInstance();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void register_button_Click(object sender, RoutedEventArgs e)
        {
        try
            {
                // if it fails to register the hotkey it will throw a HotKeyException
                manager.RegisterNewHotkey(VirtualKeys.N8, Modifiers.MOD_CONTROL | Modifiers.MOD_ALT, doSomething);
            }
            catch (HotKeyException exception)
            {
                Console.WriteLine(exception.Message);
            }
        }

        private void unregister_button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // if it fails to unregister the hotkey it will throw a HotKeyException
                manager.UnregisterHotKey(VirtualKeys.N8, Modifiers.MOD_CONTROL | Modifiers.MOD_ALT);
            }
            catch (HotKeyException exception)
            {
                Console.WriteLine(exception.Message);
            }
        }

        public void doSomething(Object sender, HotKeyPressedEventArgs args)
        {
            Console.WriteLine("{" + args.Modifiers + "} - " + args.Key + " pressed at " + args.Time);
            Console.WriteLine("Hotkey ID = " + args.HotKeyId);
            Console.WriteLine("Mouse Position = " + args.PtX + "," + args.PtY);
        }
    }
```

```
#Output example : 
{MOD_ALT, MOD_CONTROL} - N8 pressed at 771137625
Hotkey ID = 0
Mouse Position = 781,572
```

### Todos

 - Write tests

License
----

Unlicense
