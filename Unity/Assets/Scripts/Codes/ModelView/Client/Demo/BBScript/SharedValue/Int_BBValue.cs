using System;

namespace ET.Client
{
    public class Int_BBValue : BBValueBase<int>
    {
        public override Type ValueType
        {
            get
            {
                return typeof(int);
            }
        }
        
        public static Int_BBValue operator +(Int_BBValue bbValue, int add)
        {
            int oldValue = bbValue.GetValue();
            bbValue.SetValue(oldValue + add);
            
            return bbValue;
        }

        public static Int_BBValue operator -(Int_BBValue bbValue, int sub)
        {
            int oldValue = bbValue.GetValue();
            bbValue.SetValue(oldValue - sub);

            return bbValue;
        }

        public static Int_BBValue operator *(Int_BBValue bbValue, int mul)
        {
            int oldValue = bbValue.GetValue();
            bbValue.SetValue(oldValue * mul);
            return bbValue;
        }

        public static Int_BBValue operator /(Int_BBValue bbValue, int div)
        {
            int oldValue = bbValue.GetValue();
            bbValue.SetValue(oldValue / div);
            return bbValue;
        }

        public static Int_BBValue operator %(Int_BBValue bbValue, int mod)
        {
            int oldValue = bbValue.GetValue();
            bbValue.SetValue(oldValue % mod);
            return bbValue;
        }
    }
}