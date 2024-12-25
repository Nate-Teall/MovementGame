using Godot;
using System;

public partial class Sliding : PlayerState
{
	private float slideSpeed;
	private Vector3 movingDirection;

	public override void Enter(string prevState) 
	{ 
		// Get the player's current speed and input direction
		slideSpeed = player.Velocity.Length();
		movingDirection = GetGlobalInputDirection();  			// slide in direction of input

		if (movingDirection == Vector3.Zero)
			movingDirection = player.Velocity.Normalized(); // slide in direction of velocity

		Vector3 newHeadPos = player.head.Position;
		newHeadPos.Y *= Player.crouchHeightScale;
		player.head.Position = newHeadPos;
	}

	public override void HandleInput(InputEvent @event) 
	{ 
		Vector2 inputDir = Input.GetVector("left", "right", "forward", "back");

		// Sliding can be cancelled by jumping or unpressing crouch
		if (Input.IsActionJustPressed("jump"))
		{
			EmitSignal(SignalName.Finished, JUMPING);
		}
		else if (Input.IsActionJustReleased("crouch"))
		{
			if (inputDir != Vector2.Zero)
			{
				if (Input.IsActionPressed("sprint") && inputDir.Y <= 0)
				{
					EmitSignal(SignalName.Finished, SPRINTING);
				}
					else
				{
					EmitSignal(SignalName.Finished, WALKING);
				}
			}
			else
			{
				EmitSignal(SignalName.Finished, IDLE);
			}
		}
	}

	public override void Update(double delta) { }

	public override void PhysicsUpdate(double delta) 
	{
		Vector3 newVelocity = player.Velocity;

		newVelocity.X = movingDirection.X * slideSpeed;
		newVelocity.Z = movingDirection.Z * slideSpeed;

		// Each frame, decellerate the player until they are stopped
		slideSpeed = Mathf.MoveToward(slideSpeed, 0, player.slideDecel * (float)delta);

		player.Velocity = newVelocity;
		player.MoveAndSlide();

		if (slideSpeed <= 0)
		{
			if (Input.IsActionPressed("crouch"))
			{
				EmitSignal(SignalName.Finished, CROUCHING);
			}
			else if (Input.GetVector("left", "right", "forward", "back") != Vector2.Zero)
			{
				string nextState = Input.IsActionPressed("sprint") ? SPRINTING : WALKING;
				EmitSignal(SignalName.Finished, nextState);
			}
			else
			{
				EmitSignal(SignalName.Finished, IDLE);
			}
		}
	}

	public override void Exit() 
	{ 
		Vector3 newHeadPos = player.head.Position;
		newHeadPos.Y *= 1 / Player.crouchHeightScale;
		player.head.Position = newHeadPos;
	}
}
