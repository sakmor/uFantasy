using UnityEngine;

public partial class GameFlow : MonoBehaviour
{
	public Animator Animator;
	public static Animator Controller;

	private void Awake()
	{
		Object.DontDestroyOnLoad(this);
	}
	private void Start()
	{
		Animator.enabled = true;
		Controller = Animator;
	}
}
