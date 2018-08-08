using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.ComponentModel;
using System.Activities;

public class ScrollingScreenShot : CodeActivity
{
    [Category("Input")]
    [RequiredArgument]
    public InArgument<string> URL { get; set; }

    [Category("Output")]
    [RequiredArgument]
    public OutArgument<Bitmap> Output { get; set; }

    protected override void Execute(CodeActivityContext context)
    {
        var url = URL.Get(context);
        ScreenShot s = new ScreenShot();
        var output = s.Create(url);
    //  ResultNumber.Set(context, result);
        Output.Set(context,output);
        //throw new NotImplementedException();
    }
}
public class ScreenShot
{
    [ComImport]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("0000010d-0000-0000-C000-000000000046")]
    private interface IViewObject
    {
        [PreserveSig]
        int Draw([In] [MarshalAs(UnmanagedType.U4)] int dwDrawAspect, int lindex, IntPtr pvAspect,
                  [In] /*tagDVTARGETDEVICE*/ IntPtr ptd, IntPtr hdcTargetDev, IntPtr hdcDraw,
                  [In] /*COMRECT*/ Rectangle lprcBounds, [In] /*COMRECT*/ IntPtr lprcWBounds, IntPtr pfnContinue,
                  [In] int dwContinue);

        [PreserveSig]
        int GetColorSet([In] [MarshalAs(UnmanagedType.U4)] int dwDrawAspect, int lindex, IntPtr pvAspect,
                         [In] /*tagDVTARGETDEVICE*/ IntPtr ptd, IntPtr hicTargetDev, [Out] /*tagLOGPALETTE*/ IntPtr ppColorSet);

        [PreserveSig]
        int Freeze([In] [MarshalAs(UnmanagedType.U4)] int dwDrawAspect, int lindex, IntPtr pvAspect, [Out] IntPtr pdwFreeze);

        [PreserveSig]
        int Unfreeze([In] [MarshalAs(UnmanagedType.U4)] int dwFreeze);

        void SetAdvise([In] [MarshalAs(UnmanagedType.U4)] int aspects, [In] [MarshalAs(UnmanagedType.U4)] int advf,
                        [In] [MarshalAs(UnmanagedType.Interface)] /*IAdviseSink*/ IntPtr pAdvSink);

        void GetAdvise([In] [Out] [MarshalAs(UnmanagedType.LPArray)] int[] paspects,
                        [In] [Out] [MarshalAs(UnmanagedType.LPArray)] int[] advf,
                        [In] [Out] [MarshalAs(UnmanagedType.LPArray)] /*IAdviseSink[]*/ IntPtr[] pAdvSink);
    }

    public Bitmap Create(string url)
    {
        using (var webBrowser = new WebBrowser())
        {
            webBrowser.ScrollBarsEnabled = true;
            webBrowser.ScriptErrorsSuppressed = true;
            webBrowser.Navigate(url);

            while (webBrowser.ReadyState != WebBrowserReadyState.Complete)
            {
                Application.DoEvents();
            }

            webBrowser.Width = webBrowser.Document.Body.ScrollRectangle.Width;
            webBrowser.Height = webBrowser.Document.Body.ScrollRectangle.Height;

            var bitmap = new Bitmap(webBrowser.Width, webBrowser.Height);
            var graphics = Graphics.FromImage(bitmap);
            var hdc = graphics.GetHdc();

            var rect = new Rectangle(0, 0, webBrowser.Width, webBrowser.Height);

            var viewObject = (IViewObject)webBrowser.Document.DomDocument;
            viewObject.Draw(1, -1, (IntPtr)0, (IntPtr)0, (IntPtr)0, hdc, rect, (IntPtr)0, (IntPtr)0, 0);

            graphics.ReleaseHdc(hdc);

            return bitmap;
        }
    }
}

