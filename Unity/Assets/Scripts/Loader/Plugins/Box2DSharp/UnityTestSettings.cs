using System.Runtime.Serialization;
using Testbed.Abstractions;
using UnityEngine;

[DataContract]
public class UnityTestSettings : TestSettings
{
    [DataMember]
    public FullScreenMode FullScreenMode;
}