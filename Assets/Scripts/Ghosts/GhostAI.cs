using System;
using UnityEngine;
using UnityEngine.UIElements;

public enum GhostState
{
	Active,
	Vulnerable,
	VulnerabilityEnding,
	Defeated
}

[RequireComponent(typeof(GhostMove))]
public class GhostAI : MonoBehaviour
{
	public float VulnerabilityEndingTime;

	private GhostMove _ghostMove;

	private Transform _pacman;
	
	private GhostState _ghostState;

	private float _vulnerabilityTimer;

	public event Action<GhostState> OnGhostStateChanged;

	public void Reset()
	{
		_ghostMove.CharacterMotor.ResetPosition();
		_ghostState = GhostState.Active;
		OnGhostStateChanged?.Invoke(_ghostState);
	}

	public void StopMoving()
	{
		_ghostMove.CharacterMotor.enabled = false;
	}

	public void StartMoving()
	{
		_ghostMove.CharacterMotor.enabled = true;
	}

	public void SetVulnerable(float duration)
	{
		_vulnerabilityTimer = duration;
		_ghostState = GhostState.Vulnerable;
		OnGhostStateChanged?.Invoke(_ghostState);
	}

	void Start()
	{
		_ghostMove = GetComponent<GhostMove>();
		_ghostMove.OnUpdateMoveTarget += GhostMove_OnUpdateMoveTarget;

		_pacman = GameObject.FindWithTag("Player").transform;

		_ghostState = GhostState.Active;
	}

	private void GhostMove_OnUpdateMoveTarget()
	{
		switch (_ghostState)
		{
			case GhostState.Active:
				_ghostMove.SetTargetMoveLocation(_pacman.position);
				break;
			case GhostState.Vulnerable:
			case GhostState.VulnerabilityEnding:
				break;

		}		
	}

	private void Update()
	{
		switch (_ghostState)
		{
			case GhostState.Active:

				break;

			case GhostState.Vulnerable:
				_vulnerabilityTimer -= Time.deltaTime;

				if (_vulnerabilityTimer <= VulnerabilityEndingTime)
				{
					_ghostState = GhostState.VulnerabilityEnding;
					OnGhostStateChanged?.Invoke(_ghostState);
				}

				break;

			case GhostState.VulnerabilityEnding:
				_vulnerabilityTimer -= Time.deltaTime;

				if(_vulnerabilityTimer <= 0)
				{
					_ghostState = GhostState.Active;
					OnGhostStateChanged?.Invoke(_ghostState);
				}

				break;



		}
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		switch (_ghostState)
		{
			case GhostState.Active:
				if (other.CompareTag("Player"))
				{
					other.GetComponent<Life>().RemoveLife();
				}
				break;
			
			case GhostState.Vulnerable:
			case GhostState.VulnerabilityEnding:
				if (other.CompareTag("Player"))
				{
					_ghostState = GhostState.Defeated;
					OnGhostStateChanged?.Invoke(_ghostState);
				}
				break;



		}

		
	}
}
