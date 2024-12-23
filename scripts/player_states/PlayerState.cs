using Godot;
using System;

/*
 * PlayerState.cs 
 *
 * Base class for states for the player. Defines all state names
 * 	and asserts the parent node is a player.
 */
public abstract partial class PlayerState : State
{
	protected const string IDLE = "Idle";
	protected const string WALKING = "Walking";
	protected const string SPRINTING = "Sprinting";
	protected const string JUMPING = "Jumping";
	protected const string FALLING = "Falling";
	protected const string CROUCHING = "Crouching";
	protected const string SLIDING = "Sliding";

	protected Player player;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		player = Owner as Player;
		if (player == null)
		{
			GD.PushError("The PlayerState class must only be used within the Player scene! It's owner must be a Player node.");
		}
	}

	// Returns the player's input direction (8 ways)
	protected Vector3 GetInputDirection()
	{
		Vector2 inputDir = Input.GetVector("left", "right", "forward", "back");
		return (player.collision.Transform.Basis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized();
	}

	protected Vector3 MoveInDirection(float moveSpeed)
	{
		Vector3 newVelocity = player.Velocity;

		Vector3 direction = GetInputDirection();

		if (direction != Vector3.Zero)
		{
			newVelocity.X = direction.X * moveSpeed;
			newVelocity.Z = direction.Z * moveSpeed;
		}
		else
		{
			newVelocity.X = 0;
			newVelocity.Z = 0;
		}

		return newVelocity;
	}

	public override abstract void Enter(string prevState);

	public override abstract void HandleInput(InputEvent @event);

	public override abstract void Update(double delta);

	public override abstract void PhysicsUpdate(double delta);

	public override abstract void Exit();
}
