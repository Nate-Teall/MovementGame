using Godot;
using Godot.Collections;
using System;

/*
 * State.cs
 *
 * An abstract class that declares functions for a single state. 
 * Extend this class to implement a state.
 */
public abstract partial class State : Node // What is "partial" keyword?
{
	// Emitted when the state finishes and wants to transition to another state.
	[Signal]
	public delegate void FinishedEventHandler(string NextState, Dictionary data);

	// Called by the state machine when receiving unhandled input events
	public abstract void HandleInput(InputEvent @event);

	// Called by the state machine on the engine's main loop tick
	public abstract void Update(float delta);

	// Called by the state machine on the engine's physics update tick
	public abstract void PhysicsUpdate(float delta);

	// Called by the state machine upon changing the active state. The `data` parameter 
	// 	is a dictionary with arbitrary data the state can use to initialize itself.
	public abstract void Enter(string PrevState, Dictionary data = null);

	// Called by the state machine before changing the active state. 
	// 	Use this function to clean up the state.
	public abstract void Exit();

}
