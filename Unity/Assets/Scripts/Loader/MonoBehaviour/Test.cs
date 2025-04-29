using ET;
using MongoDB.Bson;
using Sirenix.OdinInspector;
using UnityEngine;

public class Test : MonoBehaviour {
	
	[Button("Test")]
	public void Test_111()
	{
		TestAAAA test = new(){a = 100, _test = new TestBBBB(){b = 1000}};
		TestAAAA test2 = MongoHelper.Clone(test);
		test2._test = new TestBBBB() { b = 100 };
		
		Debug.LogWarning(test.ToJson());
		Debug.LogWarning(test2.ToJson());
	}
	
	// public float speed = 2f; // 控制动画速度
	// public float amplitude = 3f; // 控制振幅
	
	void Update()
	{
		// 使用 Mathf.Cos 计算物体的垂直移动
		// float y = amplitude * Mathf.Cos(Time.time * speed);
		// transform.position = new Vector3(transform.position.x, y, transform.position.z);
	}
}

public class TestAAAA
{
	public int a = 10;
	public TestBBBB _test;
}

public class TestBBBB
{
	public int b = 10;
}