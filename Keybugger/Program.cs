using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LedCSharp;

namespace Keybugger
{
    class Program
    {
        private static bool _ledInitialized = false;
        public bool _vsDebuggerIsRunning = false;
        static void Main(string[] args)
        {
            while (true)
            {
                DoWorkAsyncInfiniteLoop();

            }
        }

        static async Task DoWorkAsyncInfiniteLoop()
        {
            while (true)
            {
                if (System.Diagnostics.Debugger.IsAttached)
                {
                    // Initialize the LED SDK

                    if (!_ledInitialized)
                    {
                        _ledInitialized = LogitechGSDK.LogiLedInit();
                        if (!_ledInitialized)
                        {
                            Console.WriteLine("LogitechGSDK.LogiLedInit() failed.");
                            return;
                        }
                        // Set all devices to Black
                        LogitechGSDK.LogiLedSetLighting(0, 0, 0);
                    }

                    LogitechGSDK.LogiLedSetTargetDevice(LogitechGSDK.LOGI_DEVICETYPE_ALL);

                    // Set some keys on keyboard
                    LogitechGSDK.LogiLedFlashSingleKey(keyboardNames.L, 0, 100, 100, 500,1000);
                    LogitechGSDK.LogiLedFlashLighting(0, 100, 100, 100, 1000);
                    LogitechGSDK.LogiLedSetLightingForKeyWithKeyName(keyboardNames.O, 0, 100, 100);
                    //LogitechGSDK.LogiLedFlashSingleKey(keyboardNames.G, 0, 100, 100);
                    //LogitechGSDK.LogiLedSetLightingForKeyWithKeyName(keyboardNames.I, 0, 100, 100);
                    //LogitechGSDK.LogiLedPulseLighting(0, 100, 100, 100, 500);

                }
                else
                {
                    //if not running debugger, if sdk is up, shut it down
                    if (_ledInitialized)
                        LogitechGSDK.LogiLedShutdown();
                }
                // don't run again for at least 200 milliseconds
                await Task.Delay(5000);
            }
        }
    }
}
