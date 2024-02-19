using System;
using System.Collections.Generic;

namespace ET.Client
{
    [ComponentOf]
    public class TODObjectWait: Entity, IAwake, IDestroy, ILoad
    {
        public Dictionary<Type, object> tcss = new();
    }

    public interface TODIDestroyRun
    {
        void SetResult();
    }

    public class ResultCallback<K>: TODIDestroyRun where K : struct, IWaitType
    {
        private ETTask<K> tcs;

        public ResultCallback()
        {
            this.tcs = ETTask<K>.Create(true);
        }

        public bool IsDisposed
        {
            get
            {
                return this.tcs == null;
            }
        }

        public ETTask<K> Task => this.tcs;

        public void SetResult(K k)
        {
            var t = this.tcs;
            this.tcs = null;
            t.SetResult(k);
        }

        public void SetResult()
        {
            var t = this.tcs;
            this.tcs = null;
            t.SetResult(new K() { Error = WaitTypeError.Destroy });
        }
    }
}