using System;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
	private int _currentScore;

	private int _highScore;

	public int CurrentScore { get => _currentScore; }
	public int HighScore { get => _highScore; }

	public event Action<int> OnScoreChanged;
	public event Action<int> OnHighScoreChanged;

	private void Awake()
	{
		_highScore = PlayerPrefs.GetInt("high-score", 0);
	}

	void Start()
	{
		var allCollectables = FindObjectsOfType<Collectable>();

		foreach (var collectable in allCollectables)
		{
			collectable.OnCollected += Collectable_OnCollected;
		}
	}

	private void Collectable_OnCollected(int score, Collectable collectable)
	{
		_currentScore += score;
		OnScoreChanged?.Invoke(_currentScore);

		if (_currentScore >= _highScore)
		{
			_highScore = _currentScore;
			OnHighScoreChanged?.Invoke(_highScore);
		}

		collectable.OnCollected -= Collectable_OnCollected;
	}

	private void OnDestroy()
	{
		PlayerPrefs.SetInt("high-score", _highScore);
	}
}
