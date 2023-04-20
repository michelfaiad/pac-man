using UnityEngine;

public class PacmanView : MonoBehaviour
{
	public CharacterMotor Motor;
	public Animator Animator;

	private void Start()
	{
		Motor.OnDirectionChanged += Motor_OnDirectionChanged;
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

	void Update()
	{

	}
}
