using Godot;

public partial class Victory : Control
{
	private bool _canContinue = false;

	public override async void _Ready()
	{
		GD.Print("VICTORIA");

		// Pequeña espera para evitar que una tecla anterior salte la pantalla.
		await ToSignal(
			GetTree().CreateTimer(1.0),
			SceneTreeTimer.SignalName.Timeout
		);

		_canContinue = true;
	}

	public override void _Input(InputEvent @event)
	{
		if (!_canContinue)
			return;

		if (@event is InputEventKey keyEvent &&
			keyEvent.Pressed &&
			!keyEvent.Echo)
		{
			GetTree().ChangeSceneToFile("res://Scenes/Outro.tscn");
		}
	}
}
