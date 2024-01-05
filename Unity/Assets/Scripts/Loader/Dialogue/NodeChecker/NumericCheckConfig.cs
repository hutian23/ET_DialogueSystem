using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ET.Client
{
    public enum NumericCheckerType
    {
        LessThan,
        MoreThan,
        InRange,
        Equal
    }

    public class NumericCheckConfig: NodeCheckConfig
    {
        [Space(5)]
        [InfoBox("对应NumericType")]
        [LabelText("数值类型")]
        public int NumericType;

        [Space(5)]
        [LabelText("检查类型")]
        public NumericCheckerType CheckType;

        private bool LessOrMore => CheckType is NumericCheckerType.MoreThan or NumericCheckerType.LessThan;
        
        [Space(5)]
        [ShowIf("CheckType", NumericCheckerType.InRange)]
        public int minValue;

        [ShowIf("CheckType", NumericCheckerType.InRange)]
        public int maxValue;

        [Space(5)]
        [ShowIf("CheckType", NumericCheckerType.Equal)]
        public int EqualValue;

        [Space(5)]
        [ShowIf("LessOrMore",true)]
        public int CompareValue;
    }
}