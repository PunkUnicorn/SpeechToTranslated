//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace TranslateWordsGui
//{
//    public class SuspendLayout : IDisposable
//    {
//        private readonly Control suspendMyLayout;

//        public SuspendLayout(Control suspendMyLayout, Action doWhenSuspended)
//        {
//            try
//            {
//                suspendMyLayout.SuspendLayout();
//                doWhenSuspended();
//            }
//            finally
//            {
//                suspendMyLayout.ResumeLayout();
//            }

//        }

//        public SuspendLayout(Control suspendMyLayout)
//        {
//            suspendMyLayout.SuspendLayout();
//            this.suspendMyLayout = suspendMyLayout;
//        }

//        public void Dispose()
//        {
//            if (suspendMyLayout is not null)
//                suspendMyLayout.ResumeLayout();
//        }
//    }
//}
