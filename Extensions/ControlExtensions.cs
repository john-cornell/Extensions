using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;


public static class ControlExtensions
{
    [DllImport("user32.dll")]
    public static extern int SendMessage(IntPtr hWnd, Int32 wMsg, bool wParam, Int32 lParam);

    private const int WM_SETREDRAW = 11;

    public static void SuspendDrawing(this Control parent)
    {
        if (!parent.IsDisposed) SendMessage(parent.Handle, WM_SETREDRAW, false, 0);
    }

    public static void ResumeDrawing(this Control parent)
    {
        if (!parent.IsDisposed)
        {
            SendMessage(parent.Handle, WM_SETREDRAW, true, 0);
            parent.Refresh();
        }
    }

    public static T GetChildControl<T>(this Control me, string controlName) where T : class
    {
        //Query syntax! how long ago did I write this??!
        return (from Control control in me.Controls
                where control.Name == controlName
                select control).FirstOrDefault().As<T>();
    }

    public static IEnumerable<Control> EnumerateControls(this Control me)
    {
        return EnumerateControls(me, (c) => true);
    }

    public static IEnumerable<Control> EnumerateControls(this Control me, Predicate<Control> predicate)
    {
        foreach (Control control in me.Controls)
        {
            yield return control;
        }
    }

    public static void InvokeIfRequired(this ISynchronizeInvoke control, Action<ISynchronizeInvoke> action)
    {        
        if (control.InvokeRequired)
        {
            if (control is Control && control.As<Control>().IsDisposed) return;
            try
            {
                control.Invoke(new Action(() => action(control)), null);
            }
            catch (ObjectDisposedException)
            {
                return;
            }
        }
        else
        {
            action(control);
        }
    }
}
