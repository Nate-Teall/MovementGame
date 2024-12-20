using Godot;
using Godot.Collections;
using System;

/*
 * StateMachine.cs
 *
 * Handles tracking the current state, and switching between states when necessary
 * All states should be children of the StateMachine in the Godot editor.
 */
public partial class StateMachine : Node
{
	// The initial state of the machine. If not set in the editor, the first child node is used
	[Export]
	State initialState = null;

	// The current state of the machine
	State currentState;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// Set the current state
		currentState = initialState != null ? initialState : GetChild<State>(0); // Children MUST be States!

		// Connect to every state's Finished signal
		foreach (State state in FindChildren("*", "State"))
		{
			state.Finished += _TransitionToNextState; 
		}

		// NOTE: The state machine may rely on data from the entire scene, so it might be
		// 	necessary to wait for the root node to be ready before setting the initial state
		
		// Set the initial state
		currentState.Enter("");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		currentState.Update(delta);
	}

    public override void _PhysicsProcess(double delta)
    {
        currentState.PhysicsUpdate(delta);
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        currentState.HandleInput(@event);
    }

    private void _TransitionToNextState(string nextState, Dictionary data = null)
	{
		// If the state doesn't exist, stop and print an error
		if (!HasNode(nextState))
		{
			GD.PushError("Attempted to transition to state with path: " + nextState + " but it does not exist!");
			return;
		}
		
		string prevState = currentState.Name;
		currentState.Exit();

		currentState = GetNode<State>(nextState);
		currentState.Enter(prevState, data);
	}
}
