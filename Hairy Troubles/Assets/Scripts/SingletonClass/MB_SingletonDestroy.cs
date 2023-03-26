using UnityEngine;

public class MB_SingletonDestroy<T> : MonoBehaviour where T : Component
{
	private static T instance;

	public static T Get()
	{
		return instance;
	}

	public virtual void Awake()
	{
		if (instance == null)
		{
			instance = this as T;
		}
		else
		{
			Destroy(gameObject);
		}
	}

	public virtual void OnDestroy()
	{
		if (instance == this as T) instance = null;
	}
}
