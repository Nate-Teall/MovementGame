using Godot;
using Godot.Collections;
using System;

public partial class Idle : PlayerState
{
	public override void Enter(string prevState)
	{
		Vector3 newVelocity = player.Velocity;
		newVelocity.X = 0;
		newVelocity.Y = 0;
		newVelocity.Z = 0;
		player.Velocity = newVelocity;
	}

    public override void HandleInput(InputEvent @event) { }

    public override void Update(double delta)
    {
        // Not sure why the player would be in the air and not already in falling state.
		// Unless default state is IDLE and player spawns off the ground
		if (!player.IsOnFloor())
		{
			EmitSignal(SignalName.Finished, FALLING);
		}
		else
		{
			Vector2 inputDir = Input.GetVector("left", "right", "forward", "back");
			if (Input.IsActionJustPressed("jump"))
			{
				EmitSignal(SignalName.Finished, JUMPING);
			}
			else if (Input.IsActionJustPressed("crouch"))
			{
				EmitSignal(SignalName.Finished, CROUCHING);
			}
			else if (inputDir != Vector2.Zero)
			{
				// Can only sprint forward/sideways
				if (Input.IsActionPressed("sprint") && inputDir.Y <= 0)
				{
					EmitSignal(SignalName.Finished, SPRINTING);
				}
				else
				{
					EmitSignal(SignalName.Finished, WALKING);
				}
			}
		}
    }

    public override void PhysicsUpdate(double delta)
    {
        player.MoveAndSlide();
    }

    public override void Exit() { }
}
