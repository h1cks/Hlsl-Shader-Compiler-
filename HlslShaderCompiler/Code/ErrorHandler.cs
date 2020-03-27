using System;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
#if !WINDOWS_UWP
using System.Windows.Forms;
#endif
using D3D11 = SharpDX.Direct3D11;

namespace Insane3D
{
    static class ErrorHandler
    {
#if DEBUG
        //   public static D3D11.Device s_device;
        static D3D11.InfoQueue s_infoQueue;

        static D3D11.Device s_parentDevice = null;
#endif

        [Conditional("DEBUG"), MethodImpl(MethodImplOptions.AggressiveInlining)]
        static public void WriteConsoleLine(string consoleLine)
        {
            Console.WriteLine(consoleLine);
        }

        static public string GetCurrentMethod(Exception ex)
        {
#if !WINDOWS_UWP
            StackTrace st = new StackTrace(ex, false);
            StackFrame sf = st.GetFrames()[0];

            return sf.GetMethod().Name;
#else
            return ex.Message;
#endif
        }

        public static void SetErrorHandling(D3D11.Device d3dDevice)
        {
#if DEBUG
            s_parentDevice = d3dDevice;

            if (d3dDevice == null)
                return;


            if (s_infoQueue == null)
            {
                if ((s_parentDevice.CreationFlags & SharpDX.Direct3D11.DeviceCreationFlags.Debug) == SharpDX.Direct3D11.DeviceCreationFlags.Debug)
                {
                    s_infoQueue = s_parentDevice.QueryInterface<D3D11.InfoQueue>();

                    s_infoQueue.SetBreakOnSeverity(D3D11.MessageSeverity.Error, true);
                    s_infoQueue.SetBreakOnSeverity(D3D11.MessageSeverity.Warning, true);
                    s_infoQueue.SetBreakOnSeverity(D3D11.MessageSeverity.Corruption, true);
                    s_infoQueue.SetBreakOnSeverity(D3D11.MessageSeverity.Information, true);
                }
            }
#endif
        }
#if DEBUG
        internal static void Dispose()
        {
            if (s_infoQueue != null)
            {
                s_infoQueue.Dispose();
            }
        }
#endif
        //====================================================================================================================//

        public static void DoErrorHandling(Exception ex, string errorDetails, bool canIgnore = false)
        {
#if DEBUG
            var result = SharpDX.Result.GetResultFromException(ex);

            string Errors = "Directx: " + result.Code + " - " + ex.Message + "\n\n" + ex.StackTrace;

            if (s_infoQueue != null)
            {
                for (int j = 0; j < s_infoQueue.NumStoredMessages; j++)
                {
                    D3D11.Message a = s_infoQueue.GetMessage(j);

                    Errors = a.Description.ToString().Trim('\0') + '\n' + Errors;

                    Console.WriteLine(a.Description.ToString());
                }

                Console.WriteLine(Errors);
            }

#if !WINDOWS_UWP
            Strafe.PopupErrorBox.Show("Object Type: " + ex.TargetSite.DeclaringType.FullName, "Stack Trace: " + ex.StackTrace, "Error Message:" + ex.Message + '\n' + Errors);

            if (!canIgnore)
                Application.Exit();
#endif
#endif
        }

        //====================================================================================================================//

        public static void ShowMessage(string title, string details)
        {
#if !WINDOWS_UWP
            while (MessageBox.Show(title, details, MessageBoxButtons.OK) != DialogResult.OK)
            {

            }
#else
            Debug.Assert(false, a_details);
#endif
        }

        //====================================================================================================================//
#if DEBUG
#if MEMORYLEAKTRACKER
        public static void GatherActiveObjects(DirectXManager dxManager, string filename, uint frameCount, bool writeFile, bool detailContent = false)
        {

            string report = SharpDX.Diagnostics.ObjectTracker.ReportActiveObjects();

            var stacktrace_ = SharpDX.Diagnostics.ObjectTracker.GetStackTrace();
            var activeObjectList_ = SharpDX.Diagnostics.ObjectTracker.FindActiveObjects();

            Console.WriteLine(SharpDX.Diagnostics.ObjectTracker.ReportActiveObjects());

            System.IO.StreamWriter file = null;

            if (!writeFile)
            {
                for (int i = 0; i < activeObjectList_.Count; i ++)
                {
                    Console.WriteLine(activeObjectList_[i]);
                }
            }
            else
            {
                file = new System.IO.StreamWriter(filename + " - " + frameCount + ".txt", true);
                file.WriteLine(SharpDX.Diagnostics.ObjectTracker.ReportActiveObjects());
                file.WriteLine("Active objects: " + activeObjectList_.Count);

                if (detailContent)
                {
                    for (int i = 0; i < activeObjectList_.Count; i++)
                    {
                        SharpDX.Diagnostics.ObjectReference item = activeObjectList_[i];
                        string a = item.StackTrace;


                        if (item.Object.IsAlive)
                        {
                            object reference_ = item.Object.Target;

                            file.WriteLine(reference_.ToString());

                            foreach (var prop in reference_.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic))
                            {
                                try
                                {
                                    string aa = prop.Name + " " + prop.GetValue(reference_, null);
                                    file.WriteLine(aa);
                                }
                                catch
                                {
                                    file.WriteLine("Exception");
                                }


                            }

                            file.WriteLine("------------------------------------------------------------------");


                        }

                    }
                }

                file.Close();

                dxManager.CaptureActiveObjects = false;
            }

    }
#endif
#endif
        //====================================================================================================================//

        public static void DisplayError(string stringError)
        {
            Trace.WriteLine(stringError);
#if !WINDOWS_UWP
            MessageBox.Show(stringError);
#endif
        }
    }
}

/*----------------------------------------------------------------------
*
*  BEFORE YOU GO ANY FURTHER:  TO TEST WEIRD DIRECTX Issues, use the INFOQUEUE Interface.
*          
*   create the queue interface whereveryou need it.
*   ->>>>>>   m_infoQueue = device11.QueryInterface<D3D11.InfoQueue>();
* 
*  change  | D3D11.DeviceCreationFlags.NONE to  | D3D11.DeviceCreationFlags.Debug;
*  Wrap the specific DirectX Call in the following code.
  try

        D3D11.InfoQueue m_infoQueue = m_device.QueryInterface<D3D11.InfoQueue>();
        m_infoQueue.SetBreakOnSeverity(D3D11.MessageSeverity.Error, true);
        m_infoQueue.SetBreakOnSeverity(D3D11.MessageSeverity.Warning, true);
        m_infoQueue.SetBreakOnSeverity(D3D11.MessageSeverity.Corruption, true);
        m_infoQueue.SetBreakOnSeverity(D3D11.MessageSeverity.Information, true);

 // to use object tracking, you put this just before where the leak is, you then use the report to pull the data.  
 // Name each COM object if you wish through the "DebugName".  You can then explore through the report each outstanding object and find it via the debug name.
  //      SharpDX.Configuration.EnableObjectTracking = true;

{

}
catch
{               
for (int j = 0; j < m_infoQueue.NumStoredMessages; j++)
{
    D3D11.Message a = m_infoQueue.GetMessage(j);

    Console.WriteLine(a.Description);
}
}

* This will write out the error.
* 
*  Then add these lines to the info queue function also to make DIrect x Shititself.
* 
*       m_infoQueue.SetBreakOnSeverity(D3D11.MessageSeverity.Error, true);
        m_infoQueue.SetBreakOnSeverity(D3D11.MessageSeverity.Warning, true);
* 
* 
*  Sharpdx also has a memory leak function: 
* 
* SharpDX.Configuration.EnableObjectTracking = true;
*   String report = SharpDX.Diagnostics.ObjectTracker.ReportActiveObjects();
*    string stacktrace = SharpDX.Diagnostics.ObjectTracker.GetStackTrace();
* 
* 
*----------------------------------------------------------------------*/
