using Godot;
using System;

public partial class Grappling : PlayerState
{
	private GrappleHook grappleHook;

	public override void Enter(string prevState) 
	{ 
		grappleHook = player.grapplingHook;

		// The player will get a slight boost in the direction they are holding when it attaches
		// (Only works for forward/left/right and diagonals)
		Vector2 inputDir = Input.GetVector("left", "right", "forward", "back");
		if (inputDir.Y <= 0)
		{
			// CHANGE THIS: W should accelerate TOWARDS grapple point, D to the right relative to grapple point
			// (I think, not too sure tbh)
			Vector3 globalInputDir = (player.collision.Transform.Basis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized();
			player.Velocity += globalInputDir * grappleHook.initialBoost;
		}
		
	}

	public override void HandleInput(InputEvent @event) 
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

	public override void Update(double delta) { }

	public override void PhysicsUpdate(double delta) 
	{ 
		// Apply an acceleration towards the hook
		Vector3 directionToHook = (grappleHook.Position - player.Position).Normalized();
		
		// If the hook point is beneath the player, they will stay on the ground
		// They can jump and move only towards the hook
		if (player.IsOnFloor() && grappleHook.Position.Y <= player.Position.Y)
		{
			Vector3 inputDir = GetGlobalInputDirection();

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
			player.Velocity += directionToHook * (grappleHook.hookAcceleration * (float)delta);

			// Additonally, apply a small accelleration in the player's look direction
			player.Velocity += -player.head.Transform.Basis.Z * (grappleHook.lookDirectionAccel * (float)delta);
		}

		player.MoveAndSlide();
	}

	public override void Exit() { }
}
