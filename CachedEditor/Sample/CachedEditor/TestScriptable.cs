using RizeLibrary.Attribute;
using UnityEngine;

[CreateAssetMenu]
public class TestScriptable : ScriptableObject
{
	public string Name;
	
	[CachedEditor]
	public Transform Transform;
	
	public int ID;
	public float Value;
}
