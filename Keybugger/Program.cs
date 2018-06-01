using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using LedCSharp;

namespace Keybugger
{
    class KeybuggerService : ServiceBase
    {
        private static bool _ledInitialized = false;
        public bool _vsDebuggerIsRunning = false;

        public KeybuggerService()
        {
            this.ServiceName = "Keybugger Background Service";
            this.EventLog.Log = "Application";

            // These Flags set whether or not to handle that specific
            //  type of event. Set to true if you need it, false otherwise.
            this.CanHandlePowerEvent = true;
            this.CanHandleSessionChangeEvent = true;
            this.CanPauseAndContinue = true;
            this.CanShutdown = true;
            this.CanStop = true;
        }
        static void Main(string[] args)
        {
            ServiceBase.Run(new KeybuggerService());
        }

        /// <summary>
        /// OnStart(): Put startup code here
        ///  - Start threads, get inital data, etc.
        /// </summary>
        /// <param name="args"></param>
        protected override void OnStart(string[] args)
        {
            base.OnStart(args);
        }

        /// <summary>
        /// Dispose of objects that need it here.
        /// </summary>
        /// <param name="disposing">Whether
        ///    or not disposing is going on.</param>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        /// <summary>
        /// OnShutdown(): Called when the System is shutting down
        /// - Put code here when you need special handling
        ///   of code that deals with a system shutdown, such
        ///   as saving special data before shutdown.
        /// </summary>
        protected override void OnShutdown()
        {
            base.OnShutdown();
        }

        /// <summary>
        /// OnPowerEvent(): Useful for detecting power status changes,
        ///   such as going into Suspend mode or Low Battery for laptops.
        /// </summary>
        /// <param name="powerStatus">The Power Broadcast Status
        /// (BatteryLow, Suspend, etc.)</param>
        protected override bool OnPowerEvent(PowerBroadcastStatus powerStatus)
        {
            return base.OnPowerEvent(powerStatus);
        }

        /// <summary>
        /// OnStop(): Put your stop code here
        /// - Stop threads, set final data, etc.
        /// </summary>
        protected override void OnStop()
        {
            base.OnStop();
        }

        static Task DoWorkAsyncInfiniteLoop()
        {
            while (true)
            {
                //System.Diagnostics.Process.
                if (System.Diagnostics.Debugger.IsAttached)
                {
                    // Initialize the LED SDK

                    if (!_ledInitialized)
                    {
                        _ledInitialized = LogitechGSDK.LogiLedInit();
                        if (!_ledInitialized)
                        {
                            Console.WriteLine("LogitechGSDK.LogiLedInit() failed.");
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
                System.Threading.Thread.Sleep(500);
            }
        }
    }
}
