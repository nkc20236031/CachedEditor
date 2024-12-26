using UnityEngine;
using RizeLibrary.Attribute;

public class Test : MonoBehaviour
{
	// CachedEditor属性
	// エディタ内のコンポーネントをキャッシュし描画させる属性。
	// UnityEngine.Object型のフィールド(Transform, Rigidbody, ScriptableObject等)に対してのみ使用することができる。
	
	[CachedEditor]
	public Transform Transform;
	
	public Data Data;
}

[System.Serializable]
public class Data
{
	[SerializeField] private string Name;
	
	[CachedEditor]
	public Transform Transform;
}
