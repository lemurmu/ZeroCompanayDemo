using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LD.Platform.Function
{
    public class STAThreadCaller
    {
        Control ctl;
        object[] emptyObjectArray = new object[0];

        public STAThreadCaller(Control ctl)
        {
            if (ctl == null)
                throw new ArgumentNullException("ctl");
            this.ctl = ctl;
        }

        public object Call(Delegate method, object[] arguments)
        {
            if (method == null)
            {
                throw new ArgumentNullException("method");
            }
            return ctl.Invoke(method, arguments);
        }

        public void BeginCall(Delegate method, object[] arguments)
        {
            if (method == null)
            {
                throw new ArgumentNullException("method");
            }
            try
            {
                ctl.BeginInvoke(method, arguments);
            }
            catch (InvalidOperationException ex)
            {
                //LoggingService.Warn("Error in SafeThreadAsyncCall", ex);
            }
        }

        /// <summary>
        /// Makes a call GUI threadsafe. WARNING: This method waits for the result of the
        /// operation, which can result in a dead-lock when the main thread waits for a lock
        /// held by this thread!
        /// </summary>
        public R SafeThreadFunction<R>(Func<R> method)
        {
            return (R)this.Call(method, emptyObjectArray);
        }

        /// <summary>
        /// Makes a call GUI threadsafe. WARNING: This method waits for the result of the
        /// operation, which can result in a dead-lock when the main thread waits for a lock
        /// held by this thread!
        /// </summary>
        public R SafeThreadFunction<A, R>(Func<A, R> method, A arg1)
        {
            return (R)this.Call(method, new object[] { arg1 });
        }

        /// <summary>
        /// Makes a call GUI threadsafe. WARNING: This method waits for the result of the
        /// operation, which can result in a dead-lock when the main thread waits for a lock
        /// held by this thread!
        /// </summary>
        public void SafeThreadCall(Action method)
        {
            this.Call(method, emptyObjectArray);
        }

        /// <summary>
        /// Makes a call GUI threadsafe. WARNING: This method waits for the result of the
        /// operation, which can result in a dead-lock when the main thread waits for a lock
        /// held by this thread!
        /// </summary>
        public void SafeThreadCall<A>(Action<A> method, A arg1)
        {
            this.Call(method, new object[] { arg1 });
        }

        /// <summary>
        /// Makes a call GUI threadsafe. WARNING: This method waits for the result of the
        /// operation, which can result in a dead-lock when the main thread waits for a lock
        /// held by this thread!
        /// </summary>
        public void SafeThreadCall<A, B>(Action<A, B> method, A arg1, B arg2)
        {
            this.Call(method, new object[] { arg1, arg2 });
        }

        /// <summary>
        /// Makes a call GUI threadsafe. WARNING: This method waits for the result of the
        /// operation, which can result in a dead-lock when the main thread waits for a lock
        /// held by this thread!
        /// </summary>
        public void SafeThreadCall<A, B, C>(Action<A, B, C> method, A arg1, B arg2, C arg3)
        {
            this.Call(method, new object[] { arg1, arg2, arg3 });
        }

        /// <summary>
        /// Makes a call GUI threadsafe without waiting for the returned value.
        /// </summary>
        public void SafeThreadAsyncCall(Action method)
        {
            this.BeginCall(method, new object[0]);
        }

        /// <summary>
        /// Makes a call GUI threadsafe without waiting for the returned value.
        /// </summary>
        public void SafeThreadAsyncCall<A>(Action<A> method, A arg1)
        {
            this.BeginCall(method, new object[] { arg1 });
        }

        /// <summary>
        /// Makes a call GUI threadsafe without waiting for the returned value.
        /// </summary>
        public void SafeThreadAsyncCall<A, B>(Action<A, B> method, A arg1, B arg2)
        {
            this.BeginCall(method, new object[] { arg1, arg2 });
        }

        /// <summary>
        /// Makes a call GUI threadsafe without waiting for the returned value.
        /// </summary>
        public void SafeThreadAsyncCall<A, B, C>(Action<A, B, C> method, A arg1, B arg2, C arg3)
        {
            this.BeginCall(method, new object[] { arg1, arg2, arg3 });
        }

        /// <summary>
        /// Calls a method on the GUI thread, but delays the call a bit.
        /// </summary>
        public void CallLater(int delayMilliseconds, Action method)
        {
            if (delayMilliseconds <= 0)
                throw new ArgumentOutOfRangeException("delayMilliseconds", delayMilliseconds, "Value must be positive");
            if (method == null)
                throw new ArgumentNullException("method");
            SafeThreadAsyncCall(
                delegate
                {
                    Timer t = new Timer();
                    t.Interval = delayMilliseconds;
                    t.Tick += delegate
                    {
                        t.Stop();
                        t.Dispose();
                        method();
                    };
                    t.Start();
                });
        }

    }
}
