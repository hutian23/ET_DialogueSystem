using System.Collections.Generic;
public class AOTGenericReferences : UnityEngine.MonoBehaviour
{

	// {{ AOT assemblies
	public static readonly IReadOnlyList<string> PatchedAOTAssemblyList = new List<string>
	{
		"Cinemachine.dll",
		"MongoDB.Bson.dll",
		"System.Core.dll",
		"System.dll",
		"Unity.Core.dll",
		"Unity.InputSystem.dll",
		"Unity.ThirdParty.dll",
		"UnityEngine.CoreModule.dll",
		"mscorlib.dll",
	};
	// }}

	// {{ constraint implement type
	// }} 

	// {{ AOT generic types
	// ET.AEvent<ET.Client.NetClientComponentOnRead>
	// ET.AEvent<ET.EventType.AfterCreateClientScene>
	// ET.AEvent<ET.EventType.AfterCreateCurrentScene>
	// ET.AEvent<ET.EventType.AfterUnitCreate>
	// ET.AEvent<ET.EventType.AppStartInitFinish>
	// ET.AEvent<ET.EventType.BattleSceneInit>
	// ET.AEvent<ET.EventType.ChangePosition>
	// ET.AEvent<ET.EventType.ChangeRotation>
	// ET.AEvent<ET.EventType.EnterMapFinish>
	// ET.AEvent<ET.EventType.EntryEvent1>
	// ET.AEvent<ET.EventType.EntryEvent3>
	// ET.AEvent<ET.EventType.LoginFinish>
	// ET.AEvent<ET.EventType.MoveStart>
	// ET.AEvent<ET.EventType.MoveStop>
	// ET.AEvent<ET.EventType.NumbericChange>
	// ET.AEvent<ET.EventType.SceneChangeFinish>
	// ET.AEvent<ET.EventType.SceneChangeStart>
	// ET.AInvokeHandler<ET.Client.B2FilterCallback,byte>
	// ET.AInvokeHandler<ET.Client.BBActionCallback>
	// ET.AInvokeHandler<ET.Client.BBNumericChangedCallback>
	// ET.AInvokeHandler<ET.Client.BBTimerCallback>
	// ET.AInvokeHandler<ET.Client.BehaviorReloadCallback>
	// ET.AInvokeHandler<ET.Client.CreateB2BoxCallback,object>
	// ET.AInvokeHandler<ET.Client.GroundDashChangeCallback>
	// ET.AInvokeHandler<ET.Client.HPChangeCallback>
	// ET.AInvokeHandler<ET.Client.HertzChangeCallback>
	// ET.AInvokeHandler<ET.Client.HotReloadCallBack>
	// ET.AInvokeHandler<ET.Client.HotReloadInitCallback>
	// ET.AInvokeHandler<ET.Client.LandCallback>
	// ET.AInvokeHandler<ET.Client.MoveTypeCallback>
	// ET.AInvokeHandler<ET.Client.ProcessBBScriptCallback>
	// ET.AInvokeHandler<ET.Client.SPChangeCallback>
	// ET.AInvokeHandler<ET.ConfigComponent.GetAllConfigBytes,object>
	// ET.AInvokeHandler<ET.ConfigComponent.GetOneConfigBytes,object>
	// ET.AInvokeHandler<ET.Event.BeginContactCallback>
	// ET.AInvokeHandler<ET.Event.CollisionEnterCallback>
	// ET.AInvokeHandler<ET.Event.CollisionExitCallback>
	// ET.AInvokeHandler<ET.Event.CollisionStayCallback>
	// ET.AInvokeHandler<ET.Event.ContactFilterCallback,byte>
	// ET.AInvokeHandler<ET.Event.EndContactCallback>
	// ET.AInvokeHandler<ET.Event.PostStepCallback>
	// ET.AInvokeHandler<ET.Event.PreSolveCallback>
	// ET.AInvokeHandler<ET.Event.PreStepCallback>
	// ET.AInvokeHandler<ET.Event.TriggerEnterCallback>
	// ET.AInvokeHandler<ET.Event.TriggerExitCallback>
	// ET.AInvokeHandler<ET.Event.TriggerStayCallback>
	// ET.AInvokeHandler<ET.Event.UpdateFlipCallback>
	// ET.AInvokeHandler<ET.InjectFunctionCallback>
	// ET.AInvokeHandler<ET.NavmeshComponent.RecastFileLoader,object>
	// ET.AInvokeHandler<ET.PausedCallback>
	// ET.AInvokeHandler<ET.ShowGUICallback>
	// ET.AInvokeHandler<ET.SingleStepCallback>
	// ET.AInvokeHandler<ET.TimerCallback>
	// ET.AInvokeHandler<ET.UpdateHertzCallback>
	// ET.AInvokeHandler<ET.UpdateUnitProfileCallback>
	// ET.AInvokeHandler<Timeline.UpdateEventTrackCallback>
	// ET.AInvokeHandler<Timeline.UpdateHitboxCallback>
	// ET.AInvokeHandler<Timeline.UpdateRootMotionCallback>
	// ET.AInvokeHandler<Timeline.UpdateRotationCallback>
	// ET.AwakeSystem<object,float,float,int>
	// ET.AwakeSystem<object,float,float>
	// ET.AwakeSystem<object,float>
	// ET.AwakeSystem<object,int,byte>
	// ET.AwakeSystem<object,int,float,float>
	// ET.AwakeSystem<object,int,float>
	// ET.AwakeSystem<object,int,int,int>
	// ET.AwakeSystem<object,int,int,long>
	// ET.AwakeSystem<object,int,int>
	// ET.AwakeSystem<object,int,long>
	// ET.AwakeSystem<object,int,object>
	// ET.AwakeSystem<object,int>
	// ET.AwakeSystem<object,long,int>
	// ET.AwakeSystem<object,long>
	// ET.AwakeSystem<object,object,int>
	// ET.AwakeSystem<object,object,object>
	// ET.AwakeSystem<object,object>
	// ET.AwakeSystem<object>
	// ET.ConfigSingleton<object>
	// ET.DestroySystem<object>
	// ET.ETAsyncTaskMethodBuilder<ET.Client.Wait_CreateMyUnit>
	// ET.ETAsyncTaskMethodBuilder<ET.Client.Wait_SceneChangeFinish>
	// ET.ETAsyncTaskMethodBuilder<ET.Client.Wait_UnitStop>
	// ET.ETAsyncTaskMethodBuilder<System.ValueTuple<uint,object>>
	// ET.ETAsyncTaskMethodBuilder<byte>
	// ET.ETAsyncTaskMethodBuilder<int>
	// ET.ETAsyncTaskMethodBuilder<object>
	// ET.ETAsyncTaskMethodBuilder<uint>
	// ET.ETTask<ET.Client.Wait_CreateMyUnit>
	// ET.ETTask<ET.Client.Wait_SceneChangeFinish>
	// ET.ETTask<ET.Client.Wait_UnitStop>
	// ET.ETTask<System.ValueTuple<uint,object>>
	// ET.ETTask<byte>
	// ET.ETTask<int>
	// ET.ETTask<object>
	// ET.ETTask<uint>
	// ET.FrameLateUpdateSystem<object>
	// ET.FrameUpdateSystem<object>
	// ET.GizmosUpdateSystem<object>
	// ET.IAwake<System.Numerics.Vector2,System.Numerics.Vector2>
	// ET.IAwake<float,float,int>
	// ET.IAwake<float,float>
	// ET.IAwake<float>
	// ET.IAwake<int,byte>
	// ET.IAwake<int,float,float>
	// ET.IAwake<int,float>
	// ET.IAwake<int,int,int>
	// ET.IAwake<int,int,long>
	// ET.IAwake<int,int>
	// ET.IAwake<int,long>
	// ET.IAwake<int,object>
	// ET.IAwake<int>
	// ET.IAwake<long,int>
	// ET.IAwake<long>
	// ET.IAwake<object,int>
	// ET.IAwake<object,object>
	// ET.IAwake<object>
	// ET.IAwakeSystem<System.Numerics.Vector2,System.Numerics.Vector2>
	// ET.IAwakeSystem<float,float,int>
	// ET.IAwakeSystem<float,float>
	// ET.IAwakeSystem<float>
	// ET.IAwakeSystem<int,float,float>
	// ET.IAwakeSystem<int,float>
	// ET.IAwakeSystem<int,int,long>
	// ET.IAwakeSystem<int,int>
	// ET.IAwakeSystem<int,long>
	// ET.IAwakeSystem<int,object>
	// ET.IAwakeSystem<int>
	// ET.IAwakeSystem<long,int>
	// ET.IAwakeSystem<long>
	// ET.IAwakeSystem<object,int>
	// ET.IAwakeSystem<object,object>
	// ET.LateUpdateSystem<object>
	// ET.ListComponent<Unity.Mathematics.float3>
	// ET.ListComponent<long>
	// ET.ListComponent<object>
	// ET.LoadSystem<object>
	// ET.MultiMap<long,long>
	// ET.PostStepSystem<object>
	// ET.PreStepSystem<object>
	// ET.Singleton<object>
	// ET.UpdateSystem<object>
	// MongoDB.Bson.Serialization.IBsonSerializer<object>
	// System.Action<System.Collections.Generic.KeyValuePair<int,object>>
	// System.Action<System.Collections.Generic.KeyValuePair<long,object>>
	// System.Action<System.Collections.Generic.KeyValuePair<object,int>>
	// System.Action<Unity.Mathematics.float3>
	// System.Action<int>
	// System.Action<long,int>
	// System.Action<long,long,object>
	// System.Action<long>
	// System.Action<object,int>
	// System.Action<object,object>
	// System.Action<object>
	// System.ArraySegment.Enumerator<byte>
	// System.ArraySegment<byte>
	// System.ByReference<byte>
	// System.Collections.Generic.ArraySortHelper<System.Collections.Generic.KeyValuePair<int,object>>
	// System.Collections.Generic.ArraySortHelper<System.Collections.Generic.KeyValuePair<long,object>>
	// System.Collections.Generic.ArraySortHelper<System.Collections.Generic.KeyValuePair<object,int>>
	// System.Collections.Generic.ArraySortHelper<Unity.Mathematics.float3>
	// System.Collections.Generic.ArraySortHelper<int>
	// System.Collections.Generic.ArraySortHelper<long>
	// System.Collections.Generic.ArraySortHelper<object>
	// System.Collections.Generic.Comparer<System.Collections.Generic.KeyValuePair<int,object>>
	// System.Collections.Generic.Comparer<System.Collections.Generic.KeyValuePair<long,object>>
	// System.Collections.Generic.Comparer<System.Collections.Generic.KeyValuePair<object,int>>
	// System.Collections.Generic.Comparer<Unity.Mathematics.float3>
	// System.Collections.Generic.Comparer<int>
	// System.Collections.Generic.Comparer<long>
	// System.Collections.Generic.Comparer<object>
	// System.Collections.Generic.Comparer<uint>
	// System.Collections.Generic.ComparisonComparer<System.Collections.Generic.KeyValuePair<int,object>>
	// System.Collections.Generic.ComparisonComparer<System.Collections.Generic.KeyValuePair<long,object>>
	// System.Collections.Generic.ComparisonComparer<System.Collections.Generic.KeyValuePair<object,int>>
	// System.Collections.Generic.ComparisonComparer<Unity.Mathematics.float3>
	// System.Collections.Generic.ComparisonComparer<int>
	// System.Collections.Generic.ComparisonComparer<long>
	// System.Collections.Generic.ComparisonComparer<object>
	// System.Collections.Generic.ComparisonComparer<uint>
	// System.Collections.Generic.Dictionary.Enumerator<int,ET.RpcInfo>
	// System.Collections.Generic.Dictionary.Enumerator<int,long>
	// System.Collections.Generic.Dictionary.Enumerator<int,object>
	// System.Collections.Generic.Dictionary.Enumerator<long,byte>
	// System.Collections.Generic.Dictionary.Enumerator<long,int>
	// System.Collections.Generic.Dictionary.Enumerator<long,long>
	// System.Collections.Generic.Dictionary.Enumerator<long,object>
	// System.Collections.Generic.Dictionary.Enumerator<object,int>
	// System.Collections.Generic.Dictionary.Enumerator<object,long>
	// System.Collections.Generic.Dictionary.Enumerator<object,object>
	// System.Collections.Generic.Dictionary.Enumerator<ushort,object>
	// System.Collections.Generic.Dictionary.KeyCollection.Enumerator<int,ET.RpcInfo>
	// System.Collections.Generic.Dictionary.KeyCollection.Enumerator<int,long>
	// System.Collections.Generic.Dictionary.KeyCollection.Enumerator<int,object>
	// System.Collections.Generic.Dictionary.KeyCollection.Enumerator<long,byte>
	// System.Collections.Generic.Dictionary.KeyCollection.Enumerator<long,int>
	// System.Collections.Generic.Dictionary.KeyCollection.Enumerator<long,long>
	// System.Collections.Generic.Dictionary.KeyCollection.Enumerator<long,object>
	// System.Collections.Generic.Dictionary.KeyCollection.Enumerator<object,int>
	// System.Collections.Generic.Dictionary.KeyCollection.Enumerator<object,long>
	// System.Collections.Generic.Dictionary.KeyCollection.Enumerator<object,object>
	// System.Collections.Generic.Dictionary.KeyCollection.Enumerator<ushort,object>
	// System.Collections.Generic.Dictionary.KeyCollection<int,ET.RpcInfo>
	// System.Collections.Generic.Dictionary.KeyCollection<int,long>
	// System.Collections.Generic.Dictionary.KeyCollection<int,object>
	// System.Collections.Generic.Dictionary.KeyCollection<long,byte>
	// System.Collections.Generic.Dictionary.KeyCollection<long,int>
	// System.Collections.Generic.Dictionary.KeyCollection<long,long>
	// System.Collections.Generic.Dictionary.KeyCollection<long,object>
	// System.Collections.Generic.Dictionary.KeyCollection<object,int>
	// System.Collections.Generic.Dictionary.KeyCollection<object,long>
	// System.Collections.Generic.Dictionary.KeyCollection<object,object>
	// System.Collections.Generic.Dictionary.KeyCollection<ushort,object>
	// System.Collections.Generic.Dictionary.ValueCollection.Enumerator<int,ET.RpcInfo>
	// System.Collections.Generic.Dictionary.ValueCollection.Enumerator<int,long>
	// System.Collections.Generic.Dictionary.ValueCollection.Enumerator<int,object>
	// System.Collections.Generic.Dictionary.ValueCollection.Enumerator<long,byte>
	// System.Collections.Generic.Dictionary.ValueCollection.Enumerator<long,int>
	// System.Collections.Generic.Dictionary.ValueCollection.Enumerator<long,long>
	// System.Collections.Generic.Dictionary.ValueCollection.Enumerator<long,object>
	// System.Collections.Generic.Dictionary.ValueCollection.Enumerator<object,int>
	// System.Collections.Generic.Dictionary.ValueCollection.Enumerator<object,long>
	// System.Collections.Generic.Dictionary.ValueCollection.Enumerator<object,object>
	// System.Collections.Generic.Dictionary.ValueCollection.Enumerator<ushort,object>
	// System.Collections.Generic.Dictionary.ValueCollection<int,ET.RpcInfo>
	// System.Collections.Generic.Dictionary.ValueCollection<int,long>
	// System.Collections.Generic.Dictionary.ValueCollection<int,object>
	// System.Collections.Generic.Dictionary.ValueCollection<long,byte>
	// System.Collections.Generic.Dictionary.ValueCollection<long,int>
	// System.Collections.Generic.Dictionary.ValueCollection<long,long>
	// System.Collections.Generic.Dictionary.ValueCollection<long,object>
	// System.Collections.Generic.Dictionary.ValueCollection<object,int>
	// System.Collections.Generic.Dictionary.ValueCollection<object,long>
	// System.Collections.Generic.Dictionary.ValueCollection<object,object>
	// System.Collections.Generic.Dictionary.ValueCollection<ushort,object>
	// System.Collections.Generic.Dictionary<int,ET.RpcInfo>
	// System.Collections.Generic.Dictionary<int,long>
	// System.Collections.Generic.Dictionary<int,object>
	// System.Collections.Generic.Dictionary<long,byte>
	// System.Collections.Generic.Dictionary<long,int>
	// System.Collections.Generic.Dictionary<long,long>
	// System.Collections.Generic.Dictionary<long,object>
	// System.Collections.Generic.Dictionary<object,int>
	// System.Collections.Generic.Dictionary<object,long>
	// System.Collections.Generic.Dictionary<object,object>
	// System.Collections.Generic.Dictionary<ushort,object>
	// System.Collections.Generic.EqualityComparer<ET.RpcInfo>
	// System.Collections.Generic.EqualityComparer<System.Collections.Generic.KeyValuePair<int,object>>
	// System.Collections.Generic.EqualityComparer<System.Collections.Generic.KeyValuePair<long,object>>
	// System.Collections.Generic.EqualityComparer<byte>
	// System.Collections.Generic.EqualityComparer<int>
	// System.Collections.Generic.EqualityComparer<long>
	// System.Collections.Generic.EqualityComparer<object>
	// System.Collections.Generic.EqualityComparer<uint>
	// System.Collections.Generic.EqualityComparer<ushort>
	// System.Collections.Generic.HashSet.Enumerator<int>
	// System.Collections.Generic.HashSet.Enumerator<long>
	// System.Collections.Generic.HashSet.Enumerator<object>
	// System.Collections.Generic.HashSet.Enumerator<ushort>
	// System.Collections.Generic.HashSet<int>
	// System.Collections.Generic.HashSet<long>
	// System.Collections.Generic.HashSet<object>
	// System.Collections.Generic.HashSet<ushort>
	// System.Collections.Generic.HashSetEqualityComparer<int>
	// System.Collections.Generic.HashSetEqualityComparer<long>
	// System.Collections.Generic.HashSetEqualityComparer<object>
	// System.Collections.Generic.HashSetEqualityComparer<ushort>
	// System.Collections.Generic.ICollection<ET.RpcInfo>
	// System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<int,ET.RpcInfo>>
	// System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<int,long>>
	// System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<int,object>>
	// System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<long,byte>>
	// System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<long,int>>
	// System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<long,long>>
	// System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<long,object>>
	// System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<object,int>>
	// System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<object,long>>
	// System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<object,object>>
	// System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<ushort,object>>
	// System.Collections.Generic.ICollection<Unity.Mathematics.float3>
	// System.Collections.Generic.ICollection<int>
	// System.Collections.Generic.ICollection<long>
	// System.Collections.Generic.ICollection<object>
	// System.Collections.Generic.ICollection<ushort>
	// System.Collections.Generic.IComparer<System.Collections.Generic.KeyValuePair<int,object>>
	// System.Collections.Generic.IComparer<System.Collections.Generic.KeyValuePair<long,object>>
	// System.Collections.Generic.IComparer<System.Collections.Generic.KeyValuePair<object,int>>
	// System.Collections.Generic.IComparer<Unity.Mathematics.float3>
	// System.Collections.Generic.IComparer<int>
	// System.Collections.Generic.IComparer<long>
	// System.Collections.Generic.IComparer<object>
	// System.Collections.Generic.IEnumerable<ET.RpcInfo>
	// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<int,ET.RpcInfo>>
	// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<int,long>>
	// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<int,object>>
	// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<long,byte>>
	// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<long,int>>
	// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<long,long>>
	// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<long,object>>
	// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<object,int>>
	// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<object,long>>
	// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<object,object>>
	// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<ushort,object>>
	// System.Collections.Generic.IEnumerable<Unity.Mathematics.float3>
	// System.Collections.Generic.IEnumerable<int>
	// System.Collections.Generic.IEnumerable<long>
	// System.Collections.Generic.IEnumerable<object>
	// System.Collections.Generic.IEnumerable<ushort>
	// System.Collections.Generic.IEnumerator<ET.RpcInfo>
	// System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<int,ET.RpcInfo>>
	// System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<int,long>>
	// System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<int,object>>
	// System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<long,byte>>
	// System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<long,int>>
	// System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<long,long>>
	// System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<long,object>>
	// System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<object,int>>
	// System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<object,long>>
	// System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<object,object>>
	// System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<ushort,object>>
	// System.Collections.Generic.IEnumerator<Unity.Mathematics.float3>
	// System.Collections.Generic.IEnumerator<int>
	// System.Collections.Generic.IEnumerator<long>
	// System.Collections.Generic.IEnumerator<object>
	// System.Collections.Generic.IEnumerator<ushort>
	// System.Collections.Generic.IEqualityComparer<System.Collections.Generic.KeyValuePair<int,object>>
	// System.Collections.Generic.IEqualityComparer<System.Collections.Generic.KeyValuePair<long,object>>
	// System.Collections.Generic.IEqualityComparer<int>
	// System.Collections.Generic.IEqualityComparer<long>
	// System.Collections.Generic.IEqualityComparer<object>
	// System.Collections.Generic.IEqualityComparer<ushort>
	// System.Collections.Generic.IList<System.Collections.Generic.KeyValuePair<int,object>>
	// System.Collections.Generic.IList<System.Collections.Generic.KeyValuePair<long,object>>
	// System.Collections.Generic.IList<System.Collections.Generic.KeyValuePair<object,int>>
	// System.Collections.Generic.IList<Unity.Mathematics.float3>
	// System.Collections.Generic.IList<int>
	// System.Collections.Generic.IList<long>
	// System.Collections.Generic.IList<object>
	// System.Collections.Generic.KeyValuePair<int,ET.RpcInfo>
	// System.Collections.Generic.KeyValuePair<int,long>
	// System.Collections.Generic.KeyValuePair<int,object>
	// System.Collections.Generic.KeyValuePair<long,byte>
	// System.Collections.Generic.KeyValuePair<long,int>
	// System.Collections.Generic.KeyValuePair<long,long>
	// System.Collections.Generic.KeyValuePair<long,object>
	// System.Collections.Generic.KeyValuePair<object,int>
	// System.Collections.Generic.KeyValuePair<object,long>
	// System.Collections.Generic.KeyValuePair<object,object>
	// System.Collections.Generic.KeyValuePair<ushort,object>
	// System.Collections.Generic.List.Enumerator<System.Collections.Generic.KeyValuePair<int,object>>
	// System.Collections.Generic.List.Enumerator<System.Collections.Generic.KeyValuePair<long,object>>
	// System.Collections.Generic.List.Enumerator<System.Collections.Generic.KeyValuePair<object,int>>
	// System.Collections.Generic.List.Enumerator<Unity.Mathematics.float3>
	// System.Collections.Generic.List.Enumerator<int>
	// System.Collections.Generic.List.Enumerator<long>
	// System.Collections.Generic.List.Enumerator<object>
	// System.Collections.Generic.List<System.Collections.Generic.KeyValuePair<int,object>>
	// System.Collections.Generic.List<System.Collections.Generic.KeyValuePair<long,object>>
	// System.Collections.Generic.List<System.Collections.Generic.KeyValuePair<object,int>>
	// System.Collections.Generic.List<Unity.Mathematics.float3>
	// System.Collections.Generic.List<int>
	// System.Collections.Generic.List<long>
	// System.Collections.Generic.List<object>
	// System.Collections.Generic.ObjectComparer<System.Collections.Generic.KeyValuePair<int,object>>
	// System.Collections.Generic.ObjectComparer<System.Collections.Generic.KeyValuePair<long,object>>
	// System.Collections.Generic.ObjectComparer<System.Collections.Generic.KeyValuePair<object,int>>
	// System.Collections.Generic.ObjectComparer<Unity.Mathematics.float3>
	// System.Collections.Generic.ObjectComparer<int>
	// System.Collections.Generic.ObjectComparer<long>
	// System.Collections.Generic.ObjectComparer<object>
	// System.Collections.Generic.ObjectComparer<uint>
	// System.Collections.Generic.ObjectEqualityComparer<ET.RpcInfo>
	// System.Collections.Generic.ObjectEqualityComparer<System.Collections.Generic.KeyValuePair<int,object>>
	// System.Collections.Generic.ObjectEqualityComparer<System.Collections.Generic.KeyValuePair<long,object>>
	// System.Collections.Generic.ObjectEqualityComparer<byte>
	// System.Collections.Generic.ObjectEqualityComparer<int>
	// System.Collections.Generic.ObjectEqualityComparer<long>
	// System.Collections.Generic.ObjectEqualityComparer<object>
	// System.Collections.Generic.ObjectEqualityComparer<uint>
	// System.Collections.Generic.ObjectEqualityComparer<ushort>
	// System.Collections.Generic.Queue.Enumerator<ET.Client.ComboOffsetBuffer>
	// System.Collections.Generic.Queue.Enumerator<ET.Client.InputInfo>
	// System.Collections.Generic.Queue.Enumerator<ET.Client.LoopSpriteDef>
	// System.Collections.Generic.Queue.Enumerator<ET.Client.OpInfo>
	// System.Collections.Generic.Queue.Enumerator<ET.Event.CollisionBuffer>
	// System.Collections.Generic.Queue.Enumerator<int>
	// System.Collections.Generic.Queue.Enumerator<long>
	// System.Collections.Generic.Queue.Enumerator<object>
	// System.Collections.Generic.Queue<ET.Client.ComboOffsetBuffer>
	// System.Collections.Generic.Queue<ET.Client.InputInfo>
	// System.Collections.Generic.Queue<ET.Client.LoopSpriteDef>
	// System.Collections.Generic.Queue<ET.Client.OpInfo>
	// System.Collections.Generic.Queue<ET.Event.CollisionBuffer>
	// System.Collections.Generic.Queue<int>
	// System.Collections.Generic.Queue<long>
	// System.Collections.Generic.Queue<object>
	// System.Collections.Generic.SortedDictionary.<>c__DisplayClass34_0<int,object>
	// System.Collections.Generic.SortedDictionary.<>c__DisplayClass34_0<long,object>
	// System.Collections.Generic.SortedDictionary.<>c__DisplayClass34_1<int,object>
	// System.Collections.Generic.SortedDictionary.<>c__DisplayClass34_1<long,object>
	// System.Collections.Generic.SortedDictionary.Enumerator<int,object>
	// System.Collections.Generic.SortedDictionary.Enumerator<long,object>
	// System.Collections.Generic.SortedDictionary.KeyCollection.<>c__DisplayClass5_0<int,object>
	// System.Collections.Generic.SortedDictionary.KeyCollection.<>c__DisplayClass5_0<long,object>
	// System.Collections.Generic.SortedDictionary.KeyCollection.<>c__DisplayClass6_0<int,object>
	// System.Collections.Generic.SortedDictionary.KeyCollection.<>c__DisplayClass6_0<long,object>
	// System.Collections.Generic.SortedDictionary.KeyCollection.Enumerator<int,object>
	// System.Collections.Generic.SortedDictionary.KeyCollection.Enumerator<long,object>
	// System.Collections.Generic.SortedDictionary.KeyCollection<int,object>
	// System.Collections.Generic.SortedDictionary.KeyCollection<long,object>
	// System.Collections.Generic.SortedDictionary.KeyValuePairComparer<int,object>
	// System.Collections.Generic.SortedDictionary.KeyValuePairComparer<long,object>
	// System.Collections.Generic.SortedDictionary.ValueCollection.<>c__DisplayClass5_0<int,object>
	// System.Collections.Generic.SortedDictionary.ValueCollection.<>c__DisplayClass5_0<long,object>
	// System.Collections.Generic.SortedDictionary.ValueCollection.<>c__DisplayClass6_0<int,object>
	// System.Collections.Generic.SortedDictionary.ValueCollection.<>c__DisplayClass6_0<long,object>
	// System.Collections.Generic.SortedDictionary.ValueCollection.Enumerator<int,object>
	// System.Collections.Generic.SortedDictionary.ValueCollection.Enumerator<long,object>
	// System.Collections.Generic.SortedDictionary.ValueCollection<int,object>
	// System.Collections.Generic.SortedDictionary.ValueCollection<long,object>
	// System.Collections.Generic.SortedDictionary<int,object>
	// System.Collections.Generic.SortedDictionary<long,object>
	// System.Collections.Generic.SortedSet.<>c__DisplayClass52_0<System.Collections.Generic.KeyValuePair<int,object>>
	// System.Collections.Generic.SortedSet.<>c__DisplayClass52_0<System.Collections.Generic.KeyValuePair<long,object>>
	// System.Collections.Generic.SortedSet.<>c__DisplayClass53_0<System.Collections.Generic.KeyValuePair<int,object>>
	// System.Collections.Generic.SortedSet.<>c__DisplayClass53_0<System.Collections.Generic.KeyValuePair<long,object>>
	// System.Collections.Generic.SortedSet.<>c__DisplayClass85_0<System.Collections.Generic.KeyValuePair<int,object>>
	// System.Collections.Generic.SortedSet.<>c__DisplayClass85_0<System.Collections.Generic.KeyValuePair<long,object>>
	// System.Collections.Generic.SortedSet.<Reverse>d__94<System.Collections.Generic.KeyValuePair<int,object>>
	// System.Collections.Generic.SortedSet.<Reverse>d__94<System.Collections.Generic.KeyValuePair<long,object>>
	// System.Collections.Generic.SortedSet.Enumerator<System.Collections.Generic.KeyValuePair<int,object>>
	// System.Collections.Generic.SortedSet.Enumerator<System.Collections.Generic.KeyValuePair<long,object>>
	// System.Collections.Generic.SortedSet.Node<System.Collections.Generic.KeyValuePair<int,object>>
	// System.Collections.Generic.SortedSet.Node<System.Collections.Generic.KeyValuePair<long,object>>
	// System.Collections.Generic.SortedSet.TreeSubSet.<>c__DisplayClass9_0<System.Collections.Generic.KeyValuePair<int,object>>
	// System.Collections.Generic.SortedSet.TreeSubSet.<>c__DisplayClass9_0<System.Collections.Generic.KeyValuePair<long,object>>
	// System.Collections.Generic.SortedSet.TreeSubSet<System.Collections.Generic.KeyValuePair<int,object>>
	// System.Collections.Generic.SortedSet.TreeSubSet<System.Collections.Generic.KeyValuePair<long,object>>
	// System.Collections.Generic.SortedSet<System.Collections.Generic.KeyValuePair<int,object>>
	// System.Collections.Generic.SortedSet<System.Collections.Generic.KeyValuePair<long,object>>
	// System.Collections.Generic.SortedSetEqualityComparer<System.Collections.Generic.KeyValuePair<int,object>>
	// System.Collections.Generic.SortedSetEqualityComparer<System.Collections.Generic.KeyValuePair<long,object>>
	// System.Collections.Generic.Stack.Enumerator<object>
	// System.Collections.Generic.Stack<object>
	// System.Collections.Generic.TreeSet<System.Collections.Generic.KeyValuePair<int,object>>
	// System.Collections.Generic.TreeSet<System.Collections.Generic.KeyValuePair<long,object>>
	// System.Collections.Generic.TreeWalkPredicate<System.Collections.Generic.KeyValuePair<int,object>>
	// System.Collections.Generic.TreeWalkPredicate<System.Collections.Generic.KeyValuePair<long,object>>
	// System.Collections.ObjectModel.ReadOnlyCollection<System.Collections.Generic.KeyValuePair<int,object>>
	// System.Collections.ObjectModel.ReadOnlyCollection<System.Collections.Generic.KeyValuePair<long,object>>
	// System.Collections.ObjectModel.ReadOnlyCollection<System.Collections.Generic.KeyValuePair<object,int>>
	// System.Collections.ObjectModel.ReadOnlyCollection<Unity.Mathematics.float3>
	// System.Collections.ObjectModel.ReadOnlyCollection<int>
	// System.Collections.ObjectModel.ReadOnlyCollection<long>
	// System.Collections.ObjectModel.ReadOnlyCollection<object>
	// System.Comparison<System.Collections.Generic.KeyValuePair<int,object>>
	// System.Comparison<System.Collections.Generic.KeyValuePair<long,object>>
	// System.Comparison<System.Collections.Generic.KeyValuePair<object,int>>
	// System.Comparison<Unity.Mathematics.float3>
	// System.Comparison<int>
	// System.Comparison<long>
	// System.Comparison<object>
	// System.Comparison<uint>
	// System.Func<System.Collections.Generic.KeyValuePair<long,object>,byte>
	// System.Func<System.Collections.Generic.KeyValuePair<long,object>,long>
	// System.Func<System.Collections.Generic.KeyValuePair<object,int>,byte>
	// System.Func<System.Collections.Generic.KeyValuePair<object,int>,int>
	// System.Func<System.Collections.Generic.KeyValuePair<object,int>,object>
	// System.Func<System.ValueTuple<uint,uint>>
	// System.Func<int,object>
	// System.Func<long,byte>
	// System.Func<object,System.ValueTuple<uint,uint>>
	// System.Func<object,byte>
	// System.Func<object,object,System.ValueTuple<uint,uint>>
	// System.Func<object,object,object>
	// System.Func<object,object>
	// System.Func<object>
	// System.Linq.Buffer<ET.RpcInfo>
	// System.Linq.Buffer<System.Collections.Generic.KeyValuePair<object,int>>
	// System.Linq.Buffer<object>
	// System.Linq.Enumerable.Iterator<System.Collections.Generic.KeyValuePair<long,object>>
	// System.Linq.Enumerable.Iterator<System.Collections.Generic.KeyValuePair<object,int>>
	// System.Linq.Enumerable.Iterator<long>
	// System.Linq.Enumerable.Iterator<object>
	// System.Linq.Enumerable.WhereArrayIterator<object>
	// System.Linq.Enumerable.WhereEnumerableIterator<long>
	// System.Linq.Enumerable.WhereEnumerableIterator<object>
	// System.Linq.Enumerable.WhereListIterator<object>
	// System.Linq.Enumerable.WhereSelectArrayIterator<System.Collections.Generic.KeyValuePair<long,object>,long>
	// System.Linq.Enumerable.WhereSelectArrayIterator<System.Collections.Generic.KeyValuePair<object,int>,object>
	// System.Linq.Enumerable.WhereSelectArrayIterator<object,object>
	// System.Linq.Enumerable.WhereSelectEnumerableIterator<System.Collections.Generic.KeyValuePair<long,object>,long>
	// System.Linq.Enumerable.WhereSelectEnumerableIterator<System.Collections.Generic.KeyValuePair<object,int>,object>
	// System.Linq.Enumerable.WhereSelectEnumerableIterator<object,object>
	// System.Linq.Enumerable.WhereSelectListIterator<System.Collections.Generic.KeyValuePair<long,object>,long>
	// System.Linq.Enumerable.WhereSelectListIterator<System.Collections.Generic.KeyValuePair<object,int>,object>
	// System.Linq.Enumerable.WhereSelectListIterator<object,object>
	// System.Linq.EnumerableSorter<System.Collections.Generic.KeyValuePair<object,int>,int>
	// System.Linq.EnumerableSorter<System.Collections.Generic.KeyValuePair<object,int>>
	// System.Linq.OrderedEnumerable.<GetEnumerator>d__1<System.Collections.Generic.KeyValuePair<object,int>>
	// System.Linq.OrderedEnumerable<System.Collections.Generic.KeyValuePair<object,int>,int>
	// System.Linq.OrderedEnumerable<System.Collections.Generic.KeyValuePair<object,int>>
	// System.Predicate<System.Collections.Generic.KeyValuePair<int,object>>
	// System.Predicate<System.Collections.Generic.KeyValuePair<long,object>>
	// System.Predicate<System.Collections.Generic.KeyValuePair<object,int>>
	// System.Predicate<Unity.Mathematics.float3>
	// System.Predicate<int>
	// System.Predicate<long>
	// System.Predicate<object>
	// System.Predicate<ushort>
	// System.ReadOnlySpan.Enumerator<byte>
	// System.ReadOnlySpan<byte>
	// System.Runtime.CompilerServices.ConfiguredTaskAwaitable.ConfiguredTaskAwaiter<System.ValueTuple<uint,uint>>
	// System.Runtime.CompilerServices.ConfiguredTaskAwaitable.ConfiguredTaskAwaiter<object>
	// System.Runtime.CompilerServices.ConfiguredTaskAwaitable<System.ValueTuple<uint,uint>>
	// System.Runtime.CompilerServices.ConfiguredTaskAwaitable<object>
	// System.Runtime.CompilerServices.TaskAwaiter<System.ValueTuple<uint,uint>>
	// System.Runtime.CompilerServices.TaskAwaiter<object>
	// System.Span.Enumerator<byte>
	// System.Span<byte>
	// System.Threading.Tasks.ContinuationTaskFromResultTask<System.ValueTuple<uint,uint>>
	// System.Threading.Tasks.ContinuationTaskFromResultTask<object>
	// System.Threading.Tasks.Task<System.ValueTuple<uint,uint>>
	// System.Threading.Tasks.Task<object>
	// System.Threading.Tasks.TaskFactory.<>c<System.ValueTuple<uint,uint>>
	// System.Threading.Tasks.TaskFactory.<>c<object>
	// System.Threading.Tasks.TaskFactory.<>c__DisplayClass32_0<System.ValueTuple<uint,uint>>
	// System.Threading.Tasks.TaskFactory.<>c__DisplayClass32_0<object>
	// System.Threading.Tasks.TaskFactory.<>c__DisplayClass35_0<System.ValueTuple<uint,uint>>
	// System.Threading.Tasks.TaskFactory.<>c__DisplayClass35_0<object>
	// System.Threading.Tasks.TaskFactory<System.ValueTuple<uint,uint>>
	// System.Threading.Tasks.TaskFactory<object>
	// System.ValueTuple<int,object>
	// System.ValueTuple<uint,object>
	// System.ValueTuple<uint,uint>
	// UnityEngine.Events.InvokableCall<byte>
	// UnityEngine.Events.InvokableCall<object>
	// UnityEngine.Events.UnityAction<byte>
	// UnityEngine.Events.UnityAction<int>
	// UnityEngine.Events.UnityAction<object>
	// UnityEngine.Events.UnityEvent<byte>
	// UnityEngine.Events.UnityEvent<object>
	// UnityEngine.InputSystem.InputControl<UnityEngine.Vector2>
	// UnityEngine.InputSystem.InputProcessor<UnityEngine.Vector2>
	// UnityEngine.InputSystem.Utilities.InlinedArray<object>
	// }}

	public void RefMethods()
	{
		// object Cinemachine.CinemachineVirtualCamera.AddCinemachineComponent<object>()
		// object Cinemachine.CinemachineVirtualCamera.GetCinemachineComponent<object>()
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<ET.ETTaskCompleted,object>(ET.ETTaskCompleted&,object&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter<System.ValueTuple<uint,uint>>,object>(System.Runtime.CompilerServices.TaskAwaiter<System.ValueTuple<uint,uint>>&,object&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter<object>,object>(System.Runtime.CompilerServices.TaskAwaiter<object>&,object&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,object>(object&,object&)
		// System.Void ET.ETAsyncTaskMethodBuilder<System.ValueTuple<uint,object>>.AwaitUnsafeOnCompleted<object,object>(object&,object&)
		// System.Void ET.ETAsyncTaskMethodBuilder<byte>.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.ValueTaskAwaiter,object>(System.Runtime.CompilerServices.ValueTaskAwaiter&,object&)
		// System.Void ET.ETAsyncTaskMethodBuilder<byte>.AwaitUnsafeOnCompleted<object,object>(object&,object&)
		// System.Void ET.ETAsyncTaskMethodBuilder<int>.AwaitUnsafeOnCompleted<ET.ETTaskCompleted,object>(ET.ETTaskCompleted&,object&)
		// System.Void ET.ETAsyncTaskMethodBuilder<int>.AwaitUnsafeOnCompleted<object,object>(object&,object&)
		// System.Void ET.ETAsyncTaskMethodBuilder<object>.AwaitUnsafeOnCompleted<ET.ETTaskCompleted,object>(ET.ETTaskCompleted&,object&)
		// System.Void ET.ETAsyncTaskMethodBuilder<object>.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter<object>,object>(System.Runtime.CompilerServices.TaskAwaiter<object>&,object&)
		// System.Void ET.ETAsyncTaskMethodBuilder<object>.AwaitUnsafeOnCompleted<object,object>(object&,object&)
		// System.Void ET.ETAsyncTaskMethodBuilder<uint>.AwaitUnsafeOnCompleted<object,object>(object&,object&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.EventSystem.<PublishAsync>d__33<ET.Event.AfterB2WorldCreated>>(ET.EventSystem.<PublishAsync>d__33<ET.Event.AfterB2WorldCreated>&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.EventSystem.<PublishAsync>d__33<ET.EventType.AppStartInitFinish>>(ET.EventSystem.<PublishAsync>d__33<ET.EventType.AppStartInitFinish>&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.EventSystem.<PublishAsync>d__33<ET.EventType.BattleSceneInit>>(ET.EventSystem.<PublishAsync>d__33<ET.EventType.BattleSceneInit>&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.EventSystem.<PublishAsync>d__33<ET.EventType.EntryEvent1>>(ET.EventSystem.<PublishAsync>d__33<ET.EventType.EntryEvent1>&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.EventSystem.<PublishAsync>d__33<ET.EventType.EntryEvent2>>(ET.EventSystem.<PublishAsync>d__33<ET.EventType.EntryEvent2>&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.EventSystem.<PublishAsync>d__33<ET.EventType.EntryEvent3>>(ET.EventSystem.<PublishAsync>d__33<ET.EventType.EntryEvent3>&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.EventSystem.<PublishAsync>d__33<ET.EventType.LoginFinish>>(ET.EventSystem.<PublishAsync>d__33<ET.EventType.LoginFinish>&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.EventSystem.<PublishAsync>d__33<ET.EventType.SceneChangeFinish>>(ET.EventSystem.<PublishAsync>d__33<ET.EventType.SceneChangeFinish>&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.EventSystem.<PublishAsync>d__33<ET.EventType.SceneChangeStart>>(ET.EventSystem.<PublishAsync>d__33<ET.EventType.SceneChangeStart>&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<object>(object&)
		// System.Void ET.ETAsyncTaskMethodBuilder<ET.Client.Wait_CreateMyUnit>.Start<object>(object&)
		// System.Void ET.ETAsyncTaskMethodBuilder<ET.Client.Wait_SceneChangeFinish>.Start<object>(object&)
		// System.Void ET.ETAsyncTaskMethodBuilder<ET.Client.Wait_UnitStop>.Start<object>(object&)
		// System.Void ET.ETAsyncTaskMethodBuilder<System.ValueTuple<uint,object>>.Start<object>(object&)
		// System.Void ET.ETAsyncTaskMethodBuilder<byte>.Start<object>(object&)
		// System.Void ET.ETAsyncTaskMethodBuilder<int>.Start<object>(object&)
		// System.Void ET.ETAsyncTaskMethodBuilder<object>.Start<object>(object&)
		// System.Void ET.ETAsyncTaskMethodBuilder<uint>.Start<object>(object&)
		// object ET.Entity.AddChild<object,int>(int,bool)
		// object ET.Entity.AddChild<object,long,int>(long,int,bool)
		// object ET.Entity.AddChild<object,object,int>(object,int,bool)
		// object ET.Entity.AddChild<object,object,object>(object,object,bool)
		// object ET.Entity.AddChild<object>(bool)
		// object ET.Entity.AddChildWithId<object,int>(long,int,bool)
		// object ET.Entity.AddComponent<object,System.Numerics.Vector2,System.Numerics.Vector2>(System.Numerics.Vector2,System.Numerics.Vector2,bool)
		// object ET.Entity.AddComponent<object,float,float,int>(float,float,int,bool)
		// object ET.Entity.AddComponent<object,float,float>(float,float,bool)
		// object ET.Entity.AddComponent<object,float>(float,bool)
		// object ET.Entity.AddComponent<object,int,float,float>(int,float,float,bool)
		// object ET.Entity.AddComponent<object,int,float>(int,float,bool)
		// object ET.Entity.AddComponent<object,int,int,long>(int,int,long,bool)
		// object ET.Entity.AddComponent<object,int,int>(int,int,bool)
		// object ET.Entity.AddComponent<object,int,long>(int,long,bool)
		// object ET.Entity.AddComponent<object,int,object>(int,object,bool)
		// object ET.Entity.AddComponent<object,int>(int,bool)
		// object ET.Entity.AddComponent<object,long,int>(long,int,bool)
		// object ET.Entity.AddComponent<object,long>(long,bool)
		// object ET.Entity.AddComponent<object,object,int>(object,int,bool)
		// object ET.Entity.AddComponent<object,object,object>(object,object,bool)
		// object ET.Entity.AddComponent<object>(bool)
		// object ET.Entity.GetChild<object>(long)
		// object ET.Entity.GetComponent<object>()
		// object ET.Entity.GetParent<object>()
		// System.Void ET.Entity.RemoveComponent<object>()
		// System.Void ET.EventSystem.Awake<System.Numerics.Vector2,System.Numerics.Vector2>(ET.Entity,System.Numerics.Vector2,System.Numerics.Vector2)
		// System.Void ET.EventSystem.Awake<float,float,int>(ET.Entity,float,float,int)
		// System.Void ET.EventSystem.Awake<float,float>(ET.Entity,float,float)
		// System.Void ET.EventSystem.Awake<float>(ET.Entity,float)
		// System.Void ET.EventSystem.Awake<int,float,float>(ET.Entity,int,float,float)
		// System.Void ET.EventSystem.Awake<int,float>(ET.Entity,int,float)
		// System.Void ET.EventSystem.Awake<int,int,long>(ET.Entity,int,int,long)
		// System.Void ET.EventSystem.Awake<int,int>(ET.Entity,int,int)
		// System.Void ET.EventSystem.Awake<int,long>(ET.Entity,int,long)
		// System.Void ET.EventSystem.Awake<int,object>(ET.Entity,int,object)
		// System.Void ET.EventSystem.Awake<int>(ET.Entity,int)
		// System.Void ET.EventSystem.Awake<long,int>(ET.Entity,long,int)
		// System.Void ET.EventSystem.Awake<long>(ET.Entity,long)
		// System.Void ET.EventSystem.Awake<object,int>(ET.Entity,object,int)
		// System.Void ET.EventSystem.Awake<object,object>(ET.Entity,object,object)
		// System.Void ET.EventSystem.Invoke<ET.Client.BBActionCallback>(ET.Client.BBActionCallback)
		// System.Void ET.EventSystem.Invoke<ET.Client.BBActionCallback>(int,ET.Client.BBActionCallback)
		// System.Void ET.EventSystem.Invoke<ET.Client.BBNumericChangedCallback>(ET.Client.BBNumericChangedCallback)
		// System.Void ET.EventSystem.Invoke<ET.Client.BBNumericChangedCallback>(int,ET.Client.BBNumericChangedCallback)
		// System.Void ET.EventSystem.Invoke<ET.Client.BBTimerCallback>(int,ET.Client.BBTimerCallback)
		// System.Void ET.EventSystem.Invoke<ET.Client.BehaviorReloadCallback>(ET.Client.BehaviorReloadCallback)
		// System.Void ET.EventSystem.Invoke<ET.Client.BehaviorReloadCallback>(int,ET.Client.BehaviorReloadCallback)
		// System.Void ET.EventSystem.Invoke<ET.Client.GroundDashChangeCallback>(ET.Client.GroundDashChangeCallback)
		// System.Void ET.EventSystem.Invoke<ET.Client.GroundDashChangeCallback>(int,ET.Client.GroundDashChangeCallback)
		// System.Void ET.EventSystem.Invoke<ET.Client.HertzChangeCallback>(ET.Client.HertzChangeCallback)
		// System.Void ET.EventSystem.Invoke<ET.Client.HertzChangeCallback>(int,ET.Client.HertzChangeCallback)
		// System.Void ET.EventSystem.Invoke<ET.Client.HotReloadCallBack>(ET.Client.HotReloadCallBack)
		// System.Void ET.EventSystem.Invoke<ET.Client.HotReloadCallBack>(int,ET.Client.HotReloadCallBack)
		// System.Void ET.EventSystem.Invoke<ET.Client.HotReloadInitCallback>(ET.Client.HotReloadInitCallback)
		// System.Void ET.EventSystem.Invoke<ET.Client.HotReloadInitCallback>(int,ET.Client.HotReloadInitCallback)
		// System.Void ET.EventSystem.Invoke<ET.Client.LandCallback>(ET.Client.LandCallback)
		// System.Void ET.EventSystem.Invoke<ET.Client.LandCallback>(int,ET.Client.LandCallback)
		// System.Void ET.EventSystem.Invoke<ET.Client.MoveTypeCallback>(ET.Client.MoveTypeCallback)
		// System.Void ET.EventSystem.Invoke<ET.Client.MoveTypeCallback>(int,ET.Client.MoveTypeCallback)
		// System.Void ET.EventSystem.Invoke<ET.Client.ProcessBBScriptCallback>(ET.Client.ProcessBBScriptCallback)
		// System.Void ET.EventSystem.Invoke<ET.Client.ProcessBBScriptCallback>(int,ET.Client.ProcessBBScriptCallback)
		// System.Void ET.EventSystem.Invoke<ET.Client.SPChangeCallback>(ET.Client.SPChangeCallback)
		// System.Void ET.EventSystem.Invoke<ET.Client.SPChangeCallback>(int,ET.Client.SPChangeCallback)
		// System.Void ET.EventSystem.Invoke<ET.Event.CollisionEnterCallback>(int,ET.Event.CollisionEnterCallback)
		// System.Void ET.EventSystem.Invoke<ET.Event.CollisionExitCallback>(int,ET.Event.CollisionExitCallback)
		// System.Void ET.EventSystem.Invoke<ET.Event.CollisionStayCallback>(int,ET.Event.CollisionStayCallback)
		// System.Void ET.EventSystem.Invoke<ET.Event.TriggerEnterCallback>(int,ET.Event.TriggerEnterCallback)
		// System.Void ET.EventSystem.Invoke<ET.Event.TriggerExitCallback>(int,ET.Event.TriggerExitCallback)
		// System.Void ET.EventSystem.Invoke<ET.Event.TriggerStayCallback>(int,ET.Event.TriggerStayCallback)
		// System.Void ET.EventSystem.Invoke<ET.Event.UpdateFlipCallback>(ET.Event.UpdateFlipCallback)
		// System.Void ET.EventSystem.Invoke<ET.Event.UpdateFlipCallback>(int,ET.Event.UpdateFlipCallback)
		// System.Void ET.EventSystem.Invoke<ET.PausedCallback>(ET.PausedCallback)
		// System.Void ET.EventSystem.Invoke<ET.PausedCallback>(int,ET.PausedCallback)
		// System.Void ET.EventSystem.Invoke<ET.ShowGUICallback>(ET.ShowGUICallback)
		// System.Void ET.EventSystem.Invoke<ET.ShowGUICallback>(int,ET.ShowGUICallback)
		// System.Void ET.EventSystem.Invoke<ET.SingleStepCallback>(ET.SingleStepCallback)
		// System.Void ET.EventSystem.Invoke<ET.SingleStepCallback>(int,ET.SingleStepCallback)
		// byte ET.EventSystem.Invoke<ET.Client.B2FilterCallback,byte>(int,ET.Client.B2FilterCallback)
		// object ET.EventSystem.Invoke<ET.Client.CreateB2BoxCallback,object>(ET.Client.CreateB2BoxCallback)
		// object ET.EventSystem.Invoke<ET.Client.CreateB2BoxCallback,object>(int,ET.Client.CreateB2BoxCallback)
		// object ET.EventSystem.Invoke<ET.NavmeshComponent.RecastFileLoader,object>(ET.NavmeshComponent.RecastFileLoader)
		// object ET.EventSystem.Invoke<ET.NavmeshComponent.RecastFileLoader,object>(int,ET.NavmeshComponent.RecastFileLoader)
		// System.Void ET.EventSystem.Publish<ET.Client.NetClientComponentOnRead>(ET.Scene,ET.Client.NetClientComponentOnRead)
		// System.Void ET.EventSystem.Publish<ET.EventType.AfterCreateClientScene>(ET.Scene,ET.EventType.AfterCreateClientScene)
		// System.Void ET.EventSystem.Publish<ET.EventType.AfterCreateCurrentScene>(ET.Scene,ET.EventType.AfterCreateCurrentScene)
		// System.Void ET.EventSystem.Publish<ET.EventType.AfterUnitCreate>(ET.Scene,ET.EventType.AfterUnitCreate)
		// System.Void ET.EventSystem.Publish<ET.EventType.ChangePosition>(ET.Scene,ET.EventType.ChangePosition)
		// System.Void ET.EventSystem.Publish<ET.EventType.ChangeRotation>(ET.Scene,ET.EventType.ChangeRotation)
		// System.Void ET.EventSystem.Publish<ET.EventType.EnterMapFinish>(ET.Scene,ET.EventType.EnterMapFinish)
		// System.Void ET.EventSystem.Publish<ET.EventType.MoveStart>(ET.Scene,ET.EventType.MoveStart)
		// System.Void ET.EventSystem.Publish<ET.EventType.MoveStop>(ET.Scene,ET.EventType.MoveStop)
		// System.Void ET.EventSystem.Publish<ET.EventType.NumbericChange>(ET.Scene,ET.EventType.NumbericChange)
		// ET.ETTask ET.EventSystem.PublishAsync<ET.Event.AfterB2WorldCreated>(ET.Scene,ET.Event.AfterB2WorldCreated)
		// ET.ETTask ET.EventSystem.PublishAsync<ET.EventType.AppStartInitFinish>(ET.Scene,ET.EventType.AppStartInitFinish)
		// ET.ETTask ET.EventSystem.PublishAsync<ET.EventType.BattleSceneInit>(ET.Scene,ET.EventType.BattleSceneInit)
		// ET.ETTask ET.EventSystem.PublishAsync<ET.EventType.EntryEvent1>(ET.Scene,ET.EventType.EntryEvent1)
		// ET.ETTask ET.EventSystem.PublishAsync<ET.EventType.EntryEvent2>(ET.Scene,ET.EventType.EntryEvent2)
		// ET.ETTask ET.EventSystem.PublishAsync<ET.EventType.EntryEvent3>(ET.Scene,ET.EventType.EntryEvent3)
		// ET.ETTask ET.EventSystem.PublishAsync<ET.EventType.LoginFinish>(ET.Scene,ET.EventType.LoginFinish)
		// ET.ETTask ET.EventSystem.PublishAsync<ET.EventType.SceneChangeFinish>(ET.Scene,ET.EventType.SceneChangeFinish)
		// ET.ETTask ET.EventSystem.PublishAsync<ET.EventType.SceneChangeStart>(ET.Scene,ET.EventType.SceneChangeStart)
		// object ET.Game.AddSingleton<object>()
		// object ET.JsonHelper.FromJson<object>(string)
		// object ET.MongoHelper.Deserialize<object>(byte[])
		// object ET.MongoHelper.FromJson<object>(string)
		// System.Void ET.RandomGenerator.BreakRank<object>(System.Collections.Generic.List<object>)
		// string ET.StringHelper.ArrayToString<float>(float[])
		// object MongoDB.Bson.Serialization.BsonSerializer.Deserialize<object>(MongoDB.Bson.IO.IBsonReader,System.Action<MongoDB.Bson.Serialization.BsonDeserializationContext.Builder>)
		// object MongoDB.Bson.Serialization.BsonSerializer.Deserialize<object>(string,System.Action<MongoDB.Bson.Serialization.BsonDeserializationContext.Builder>)
		// MongoDB.Bson.Serialization.IBsonSerializer<object> MongoDB.Bson.Serialization.BsonSerializer.LookupSerializer<object>()
		// object MongoDB.Bson.Serialization.IBsonSerializerExtensions.Deserialize<object>(MongoDB.Bson.Serialization.IBsonSerializer<object>,MongoDB.Bson.Serialization.BsonDeserializationContext)
		// object System.Activator.CreateInstance<object>()
		// object[] System.Array.Empty<object>()
		// bool System.Enum.TryParse<int>(string,bool,int&)
		// bool System.Enum.TryParse<int>(string,int&)
		// object System.Linq.Enumerable.FirstOrDefault<object>(System.Collections.Generic.IEnumerable<object>,System.Func<object,bool>)
		// System.Linq.IOrderedEnumerable<System.Collections.Generic.KeyValuePair<object,int>> System.Linq.Enumerable.OrderBy<System.Collections.Generic.KeyValuePair<object,int>,int>(System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<object,int>>,System.Func<System.Collections.Generic.KeyValuePair<object,int>,int>)
		// System.Collections.Generic.IEnumerable<long> System.Linq.Enumerable.Select<System.Collections.Generic.KeyValuePair<long,object>,long>(System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<long,object>>,System.Func<System.Collections.Generic.KeyValuePair<long,object>,long>)
		// System.Collections.Generic.IEnumerable<object> System.Linq.Enumerable.Select<System.Collections.Generic.KeyValuePair<object,int>,object>(System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<object,int>>,System.Func<System.Collections.Generic.KeyValuePair<object,int>,object>)
		// System.Collections.Generic.IEnumerable<object> System.Linq.Enumerable.Select<object,object>(System.Collections.Generic.IEnumerable<object>,System.Func<object,object>)
		// ET.RpcInfo[] System.Linq.Enumerable.ToArray<ET.RpcInfo>(System.Collections.Generic.IEnumerable<ET.RpcInfo>)
		// object[] System.Linq.Enumerable.ToArray<object>(System.Collections.Generic.IEnumerable<object>)
		// System.Collections.Generic.IEnumerable<object> System.Linq.Enumerable.Where<object>(System.Collections.Generic.IEnumerable<object>,System.Func<object,bool>)
		// System.Collections.Generic.IEnumerable<long> System.Linq.Enumerable.Iterator<System.Collections.Generic.KeyValuePair<long,object>>.Select<long>(System.Func<System.Collections.Generic.KeyValuePair<long,object>,long>)
		// System.Collections.Generic.IEnumerable<object> System.Linq.Enumerable.Iterator<System.Collections.Generic.KeyValuePair<object,int>>.Select<object>(System.Func<System.Collections.Generic.KeyValuePair<object,int>,object>)
		// System.Collections.Generic.IEnumerable<object> System.Linq.Enumerable.Iterator<object>.Select<object>(System.Func<object,object>)
		// System.Threading.Tasks.Task<object> System.Threading.Tasks.TaskFactory.StartNew<object>(System.Func<object>,System.Threading.CancellationToken)
		// object UnityEngine.Component.GetComponent<object>()
		// object[] UnityEngine.Component.GetComponents<object>()
		// object[] UnityEngine.Component.GetComponentsInChildren<object>()
		// object[] UnityEngine.Component.GetComponentsInChildren<object>(bool)
		// object UnityEngine.GameObject.AddComponent<object>()
		// object UnityEngine.GameObject.GetComponent<object>()
		// object[] UnityEngine.GameObject.GetComponents<object>()
		// object[] UnityEngine.GameObject.GetComponentsInChildren<object>()
		// object[] UnityEngine.GameObject.GetComponentsInChildren<object>(bool)
		// object UnityEngine.Object.Instantiate<object>(object)
		// object UnityEngine.Object.Instantiate<object>(object,UnityEngine.Transform,bool)
		// object UnityEngine.Resources.Load<object>(string)
	}
}