using System.ComponentModel;
using System.Windows.Forms;

namespace Avance
{
    public static class ControlExtensionMethods
    {
        //source
        // http://stackoverflow.com/questions/271398/what-are-your-favorite-extension-methods-for-c-codeplex-com-extensionoverflow
        /// <summary>
        /// Mimics VB With statement
        /// 
        /// Returns whether the function is being executed during design time in Visual Studio.
        /// </summary>
        public static bool IsDesignTime(this Control control)
        {
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
            {
                return true;
            }

            if (control.Site != null && control.Site.DesignMode)
            {
                return true;
            }

            var parent = control.Parent;
            while (parent != null)
            {
                if (parent.Site != null && parent.Site.DesignMode)
                {
                    return true;
                }
                parent = parent.Parent;
            }
            return false;
        }

        public static bool IsNullOrDisposed(this Form me)
        {
            return me == null || me.IsDisposed;
        }
    }
}
