using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Appeon.SnapDevelop.SvnServices
{
    internal static class SvnMessageView
    {
       
        /// <summary>
        /// Appends a line to the svn message view.
        /// </summary>
        public static void AppendLine(string text)
        {
            //Category.AppendLine(text);
        }

        public static void HandleNotifications(SvnClientWrapper client)
        {
            client.Notify += delegate (object sender, NotificationEventArgs e) {
                AppendLine(e.Kind + e.Action + " " + e.Path);
            };
            //AsynchronousWaitDialog waitDialog = null;
            client.OperationStarted += delegate (object sender, SubversionOperationEventArgs e) {
                //if (waitDialog == null)
                {
                    //waitDialog = AsynchronousWaitDialog.ShowWaitDialog("svn " + e.Operation);
                    //					waitDialog.Cancelled += delegate {
                    //						client.Cancel();
                    //					};
                }
            };
            client.OperationFinished += delegate {
                //if (waitDialog != null)
                //{
                //    waitDialog.Dispose();
                //    waitDialog = null;
                //}
            };
        }
    }
}
