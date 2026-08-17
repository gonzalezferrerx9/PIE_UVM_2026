using Godot;

public partial class Cinematic : Control
{
	private TextureRect _textureRect;
	private bool _waitingForInput = false;

	private string[] _images =
	{
		"res://Cinematics/Intro-1.jpg",
		"res://Cinematics/Intro-2.jpg",
		"res://Cinematics/Intro-3.jpg",
		"res://Cinematics/Intro-4.jpg",
		"res://Cinematics/Intro-5.jpg",
		"res://Cinematics/Intro-6.jpg",
		"res://Cinematics/Intro-7.jpg",
        "res://Cinematics/Intro-8.jpg"
	};

	public override async void _Ready()
{
	_textureRect = GetNode<TextureRect>("TextureRect");

	// Mostrar Intro-1 hasta Intro-7.
	for (int i = 0; i < _images.Length - 1; i++)
	{
		_textureRect.Texture = GD.Load<Texture2D>(_images[i]);

		await ToSignal(
			GetTree().CreateTimer(5.0),
			SceneTreeTimer.SignalName.Timeout
		);
	}

	// Mostrar Intro-8.
	_textureRect.Texture =
		GD.Load<Texture2D>(_images[_images.Length - 1]);

	// Desde este momento aceptamos una tecla inmediatamente.
	_waitingForInput = true;
}

	public override void _Input(InputEvent @event)
	{
		if (!_waitingForInput)
			return;

		if (@event is InputEventKey keyEvent &&
			keyEvent.Pressed &&
			!keyEvent.Echo)
		{
			FinishCinematic();
		}
	}

	private void FinishCinematic()
{
	GetTree().ChangeSceneToFile("res://Scenes/Main.tscn");
}

}
