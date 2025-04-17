using System;

namespace ET.Client
{
    public class Struct_BBValue<T> : BBValueBase<T> where T : struct
    {
        public override Type ValueType
        {
            get
            {
                return typeof(T);
            }
        }
    }
}