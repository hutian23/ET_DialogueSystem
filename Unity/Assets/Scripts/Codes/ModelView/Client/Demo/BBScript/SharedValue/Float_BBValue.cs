using System;

namespace ET.Client
{
    public class Float_BBValue : BBValueBase<float>
    {
        public override Type ValueType
        {
            get
            {
                return typeof (float);
            }
        }

        public static Float_BBValue operator +(Float_BBValue bbValue, float add)
        {
            float oldValue = bbValue.GetValue();
            bbValue.SetValue(oldValue + add);

            return bbValue;
        }

        public static Float_BBValue operator -(Float_BBValue bbValue, float sub)
        {
            float oldValue = bbValue.GetValue();
            bbValue.SetValue(oldValue - sub);

            return bbValue;
        }

        public static Float_BBValue operator *(Float_BBValue bbValue, float mul)
        {
            float oldValue = bbValue.GetValue();
            bbValue.SetValue(oldValue * mul);
            
            return bbValue;
        }

        public static Float_BBValue operator /(Float_BBValue bbValue, float div)
        {
            float oldValue = bbValue.GetValue();
            bbValue.SetValue(oldValue / div);
            
            return bbValue;
        }

        public static Float_BBValue operator %(Float_BBValue bbValue, float mod)
        {
            float oldValue = bbValue.GetValue();
            bbValue.SetValue(oldValue % mod);
            return bbValue;
        }
    }
}