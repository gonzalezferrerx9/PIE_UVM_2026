using Godot;

public partial class Outro : Control
{
	private TextureRect _textureRect;
	private int _currentImage = 0;
	private bool _waitingForInput = false;

	private string[] _images =
	{
		"res://Cinematics/Outro-1.jpg",
		"res://Cinematics/Outro-2.jpg",
		"res://Cinematics/Outro-3.jpg",
		"res://Cinematics/Outro-4.jpg",
		"res://Cinematics/Outro-5.jpg",
		"res://Cinematics/Outro-6.jpg",
		"res://Cinematics/Outro-7.jpg",
		"res://Cinematics/Outro-8.jpg",
        "res://Cinematics/Outro-9.jpg"
	};

	// Aquí indicamos cuáles imágenes esperan una tecla.
	private int[] _waitForInputImages =
	{
		5
	};

	public override void _Ready()
	{
		_textureRect = GetNode<TextureRect>("TextureRect");
		ShowCurrentImage();
	}

	private async void ShowCurrentImage()
	{
		_textureRect.Texture =
			GD.Load<Texture2D>(_images[_currentImage]);

		// Si esta imagen debe esperar una tecla:
		if (ShouldWaitForInput(_currentImage))
		{
			_waitingForInput = true;
			return;
		}

		// Si no, espera 5 segundos y continúa sola.
		await ToSignal(
			GetTree().CreateTimer(5.0),
			SceneTreeTimer.SignalName.Timeout
		);

		NextImage();
	}

	public override void _Input(InputEvent @event)
	{
		if (!_waitingForInput)
			return;

		if (@event is InputEventKey keyEvent &&
			keyEvent.Pressed &&
			!keyEvent.Echo)
		{
			_waitingForInput = false;
			NextImage();
		}
	}

	private void NextImage()
	{
		if (_currentImage < _images.Length - 1)
		{
			_currentImage++;
			ShowCurrentImage();
		}
		else
		{
			FinishOutro();
		}
	}

	private bool ShouldWaitForInput(int imageIndex)
	{
		foreach (int index in _waitForInputImages)
		{
			if (imageIndex == index)
				return true;
		}

		return false;
	}

	private void FinishOutro()
	{
		GD.Print("Outro terminado");
	}
}
