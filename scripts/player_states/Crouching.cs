using Godot;
using System;

public partial class Crouching : PlayerState
{

	private const float crouchHeightScale = 0.5f;

	public override void Enter(string prevState)
	{
		Vector3 newHeadPos = player.head.Position;
		newHeadPos.Y *= crouchHeightScale;
		player.head.Position = newHeadPos;
	}

	public override void HandleInput(InputEvent @event)
	{
		if (player.IsOnFloor() && Input.IsActionJustReleased("crouch"))
		{
			EmitSignal(SignalName.Finished, IDLE);
		}
		else if (player.IsOnFloor() && Input.IsActionPressed("sprint"))
		{
			EmitSignal(SignalName.Finished, SPRINTING);
		}
	}

	public override void Update(double delta) { }

	public override void PhysicsUpdate(double delta)
	{
		Vector3 newVelocity = player.Velocity;

		Vector2 inputDir = Input.GetVector("left", "right", "forward", "back");
		Vector3 direction = (player.head.Transform.Basis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized();

		if (direction != Vector3.Zero)
		{
			newVelocity.X = direction.X * player.crouchSpeed;
			newVelocity.Z = direction.Z * player.crouchSpeed;
		}
		else
		{
			newVelocity.X = 0;
			newVelocity.Z = 0;
		}

		player.Velocity = newVelocity;
		player.MoveAndSlide();

		if (!player.IsOnFloor())
		{
			EmitSignal(SignalName.Finished, FALLING);
		}
	}

	public override void Exit()
	{
		Vector3 newHeadPos = player.head.Position;
		newHeadPos.Y *= 1 / crouchHeightScale;
		player.head.Position = newHeadPos;
	}
}
