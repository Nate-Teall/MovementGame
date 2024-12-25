using Godot;
using System;

public partial class Grappling : PlayerState
{
	private GrappleHook grappleHook;
	public override void Enter(string prevState) 
	{ 
		grappleHook = player.grapplingHook;
	}

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
			else if (Input.IsActionPressed("crouch"))
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
		Vector3 directionToHook = (grappleHook.Position - player.Position).Normalized();
		// Apply a slight acceleration in the player's look direction
		//Vector3 lookDirection = -player.head.Transform.Basis.Z.Normalized();
		
		// If the hook point is beneath the player, they will stay on the ground
		// They can jump and move only towards the hook
		if (player.IsOnFloor() && grappleHook.Position.Y <= player.Position.Y)
		{
			Vector3 inputDir = GetInputDirection();
			
			if ( inputDir.AngleTo(directionToHook) <= (Mathf.Pi / 2) )
				player.Velocity = MoveInDirection(player.walkSpeed);
			else
				player.Velocity = Vector3.Zero;

			if (Input.IsActionJustPressed("jump"))
			{
				player.Velocity += new Vector3(0, player.jumpVelocity, 0);
			}
		}
		else
		{
			player.Velocity += directionToHook * (player.grapplingHookAcceleration * (float)delta);
		}

		player.MoveAndSlide();
	}

	public override void Exit() { }
}
