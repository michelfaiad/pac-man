using System;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
	private int _currentScore;

	private int _highScore;

	private int _ghostScore;

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
		_ghostScore = 200;

		var allCollectables = FindObjectsOfType<Collectable>();

		foreach (var collectable in allCollectables)
		{
			collectable.OnCollected += Collectable_OnCollected;
		}

		var allGhosts = FindObjectsOfType<GhostAI>();
		foreach (var ghost in allGhosts)
		{
			ghost.OnGhostStateChanged += GhostAI_OnGhostStateChanged;
		}
	}

	private void GhostAI_OnGhostStateChanged(GhostState state)
	{
		if (state == GhostState.Defeated)
		{
			Debug.Log($"_ghostScore before:{_ghostScore}");
			_currentScore += _ghostScore;
			_ghostScore += _ghostScore<800?200:0;
			Debug.Log($"_ghostScore after:{_ghostScore}");
			ScoreEvents();
		}
	}

	private void Collectable_OnCollected(int score, Collectable collectable)
	{
		_currentScore += score;
		ScoreEvents();

		collectable.OnCollected -= Collectable_OnCollected;
	}

	private void ScoreEvents()
	{
		OnScoreChanged?.Invoke(_currentScore);

		if (_currentScore >= _highScore)
		{
			_highScore = _currentScore;
			OnHighScoreChanged?.Invoke(_highScore);
		}
	}

	private void OnDestroy()
	{
		PlayerPrefs.SetInt("high-score", _highScore);
	}
}
