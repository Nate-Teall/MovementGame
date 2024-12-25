using Godot;
using System;

public partial class Grappling : PlayerState
{
	public override void Enter(string prevState) { }

	public override void HandleInput(InputEvent @event) { }

	public override void Update(double delta) 
	{ 
		// Transition to the correct state when the player releases the grapple button
		// The code for returning the grappling hook is already in Player.cs
		if (Input.IsActionJustReleased("grapple"))
		{
			Vector2 inputDir = Input.GetVector("left", "right", "forward", "back");

			if(!player.IsOnFloor())
			{
				EmitSignal(SignalName.Finished, FALLING);
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
			else
			{
				EmitSignal(SignalName.Finished, IDLE);
			}
		}
	}

	public override void PhysicsUpdate(double delta) 
	{ 
		// Apply an acceleration towards the hook
		Vector3 directionToHook = (player.grapplingHook.Position - player.Position).Normalized();
		// Apply a slight acceleration in the player's look direction
		//Vector3 lookDirection = -player.head.Transform.Basis.Z.Normalized();

		Vector3 newVelocity = player.Velocity;
		
		newVelocity += directionToHook * (player.grapplingHookAcceleration * (float)delta);

		player.Velocity = newVelocity;

		player.MoveAndSlide();
	}

	public override void Exit() { }
}
