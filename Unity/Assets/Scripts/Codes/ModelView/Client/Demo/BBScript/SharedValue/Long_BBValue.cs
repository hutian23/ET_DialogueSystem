using System;

namespace ET.Client
{
    public class Long_BBValue : BBValueBase<long>
    {
        public override Type ValueType
        {
            get
            {
                return typeof(long);
            }
        }
        
        public static Long_BBValue operator +(Long_BBValue bbValue, long add)
        {
            long oldValue = bbValue.GetValue();
            bbValue.SetValue(oldValue + add);
            
            return bbValue;
        }

        public static Long_BBValue operator -(Long_BBValue bbValue, long sub)
        {
            long oldValue = bbValue.GetValue();
            bbValue.SetValue(oldValue - sub);

            return bbValue;
        }

        public static Long_BBValue operator *(Long_BBValue bbValue, long mul)
        {
            long oldValue = bbValue.GetValue();
            bbValue.SetValue(oldValue * mul);
            return bbValue;
        }

        public static Long_BBValue operator /(Long_BBValue bbValue, long div)
        {
            long oldValue = bbValue.GetValue();
            bbValue.SetValue(oldValue / div);
            return bbValue;
        }

        public static Long_BBValue operator %(Long_BBValue bbValue, long mod)
        {
            long oldValue = bbValue.GetValue();
            bbValue.SetValue(oldValue % mod);
            return bbValue;
        }
    }
}