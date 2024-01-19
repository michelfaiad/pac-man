using System;
using UnityEngine;

public class Collectable : MonoBehaviour
{
	public bool IsVictoryCondition;

	public int Score;

	public event Action<int, Collectable> OnCollected;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		OnCollected?.Invoke(Score, this);
		Destroy(gameObject);
	}
}
