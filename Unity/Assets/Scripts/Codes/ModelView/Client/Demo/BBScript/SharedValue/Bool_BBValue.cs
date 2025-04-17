using System;

namespace ET.Client
{
    public class Bool_BBValue : BBValueBase<bool>
    {
        public override Type ValueType
        {
            get
            {
                return typeof(bool);
            }
        }
    }
}