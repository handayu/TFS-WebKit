using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace USeFuturesSpirit
{
    class USeFuturesSpiritUtility
    {
        private static string ms_messageBoxTitle = "USe 期货套利";
        public static void ShowInformationMessageBox(IWin32Window owner,string message)
        {
            MessageBox.Show(owner, message, ms_messageBoxTitle, MessageBoxButtons.OK,MessageBoxIcon.Information);
        }

        public static void ShowWarningMessageBox(IWin32Window owner, string message)
        {
            MessageBox.Show(owner, message, ms_messageBoxTitle, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        public static void ShowErrrorMessageBox(IWin32Window owner, string message)
        {
            MessageBox.Show(owner, message, ms_messageBoxTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public static DialogResult ShowYesNoMessageBox(IWin32Window owner,string message)
        {
            return MessageBox.Show(owner, message, ms_messageBoxTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Information);
        }
    }
}
