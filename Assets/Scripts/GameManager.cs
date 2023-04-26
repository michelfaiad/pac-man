using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	private enum GameState
	{
		Starting,
		Playing,
		GameOver,
		Victory
	}

	public float StartupTime;

	private GhostAI[] _allGhosts;
	private CharacterMotor _pacmanMotor;

	private GameState _gameState;
	private int _victoryCount;

	void Start()
	{
		var allCollectables = FindObjectsOfType<Collectable>();

		_victoryCount = 0;

		foreach (var collectable in allCollectables)
		{
			_victoryCount++;
			collectable.OnCollected += Collectable_OnCollected;
		}

		var pacman = GameObject.FindWithTag("Player");
		_pacmanMotor = pacman.GetComponent<CharacterMotor>();
		_allGhosts = FindObjectsOfType<GhostAI>();

		StopAllCharacters();

		_gameState = GameState.Starting;
	}

	private void Collectable_OnCollected(int obj)
	{
		_victoryCount--;

		if (_victoryCount <= 0)
		{
			_gameState = GameState.Victory;
			Debug.Log("Victory!!");
		}
	}

	private void Update()
	{
		switch (_gameState)
		{
			case GameState.Starting:

				StartupTime -= Time.deltaTime;

				if (StartupTime <= 0)
				{
					_gameState = GameState.Playing;
					StartAllCharacters();
				}

				break;

			case GameState.Victory:

				if (Input.anyKey)
				{
					SceneManager.LoadScene(0);
				}

				break;
		}
	}

	private void StopAllCharacters()
	{
		_pacmanMotor.enabled = false;

		foreach (var ghost in _allGhosts)
		{
			ghost.StopMoving();
		}
	}

	private void StartAllCharacters()
	{
		_pacmanMotor.enabled = true;

		foreach (var ghost in _allGhosts)
		{
			ghost.StartMoving();
		}
	}
}
