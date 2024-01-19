using UnityEngine;

public class PacmanView : MonoBehaviour
{
	public CharacterMotor Motor;

	public Life CharacterLife;

	public Animator Animator;

	public AudioSource AudioSource;

	public AudioClip LifeLostSound;

	private void Start()
	{
		Motor.OnDirectionChanged += Motor_OnDirectionChanged;
		Motor.OnResetPosition += Motor_OnResetPosition;
		Motor.OnDisabled += Motor_OnDisabled;
		CharacterLife.OnLifeRemoved += CharacterLife_OnLifeRemoved;

		Animator.SetBool("Moving", false);
		Animator.SetBool("Dead", false);
	}

	private void Motor_OnDisabled()
	{
		Animator.speed = 0;
	}

	private void Motor_OnResetPosition()
	{
		Animator.SetBool("Moving", false);
		Animator.SetBool("Dead", false);
	}

	private void CharacterLife_OnLifeRemoved(int _)
	{
		transform.Rotate(0, 0, -90);
		AudioSource.PlayOneShot(LifeLostSound);
		Animator.speed = 1;
		Animator.SetBool("Moving", false);
		Animator.SetBool("Dead", true);
	}

	private void Motor_OnDirectionChanged(Direction direction)
	{
		switch (direction)
		{
			case Direction.None:
				Animator.SetBool("Moving", false);
				break;

			case Direction.Up:
				transform.rotation = Quaternion.Euler(0, 0, 90);
				Animator.SetBool("Moving", true);

				break;

			case Direction.Left:
				transform.rotation = Quaternion.Euler(0, 0, 180);
				Animator.SetBool("Moving", true);
				break;

			case Direction.Down:
				transform.rotation = Quaternion.Euler(0, 0, 270);
				Animator.SetBool("Moving", true);
				break;

			case Direction.Right:
				transform.rotation = Quaternion.Euler(0, 0, 0);
				Animator.SetBool("Moving", true);
				break;
		}
	}
}
