using Godot;
using Godot.Collections;
using System;

/*
 * PlayerState.cs 
 *
 * Base class for states for the player. Defines all state names
 * 	and asserts the parent node is a player.
 */
public abstract partial class PlayerState : State
{
	public const string IDLE = "Idle";
	public const string WALKING = "Walking";
	public const string RUNNING = "Running";
	public const string JUMPING = "Jumping";

	public Player player { get; private set; }

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// We might need to wait for Owner.Ready before doing this..?

		player = Owner as Player;
		if (player == null)
		{
			GD.PushError("The PlayerState class must only be used within the Player scene! It's owner must be a Player node.");
		}
	}

	public override abstract void HandleInput(InputEvent @event);

	public override abstract void Update(double delta);

	public override abstract void PhysicsUpdate(double delta);

	public override abstract void Enter(string prevState, Dictionary data = null);

	public override abstract void Exit();
}
