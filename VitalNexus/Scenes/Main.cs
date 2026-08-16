using Godot;

public partial class Main : Node2D
{
	private bool levelCompleted = false;
	private bool levelStarted = false;

	public override void _Ready()
	{
		GD.Print("VITAL NEXUS INICIADO");

		CallDeferred(nameof(StartLevel));
	}

	private void StartLevel()
	{
		var enemies = GetTree().GetNodesInGroup("enemies");

		GD.Print("ENEMIGOS DETECTADOS AL INICIAR: ", enemies.Count);

		if (enemies.Count > 0)
		{
			levelStarted = true;
		}
		else
		{
			GD.PrintErr("ERROR: No se encontraron enemigos en el grupo 'enemies'.");
		}
	}

	public override void _Process(double delta)
	{
		if (!levelStarted)
			return;

		if (levelCompleted)
			return;

		var enemies = GetTree().GetNodesInGroup("enemies");

		if (enemies.Count == 0)
		{
			Victory();
		}
	}

	private void Victory()
	{
		levelCompleted = true;

		GD.Print("NIVEL COMPLETADO");

		GetTree().ChangeSceneToFile(
            "res://Scenes/Victory.tscn"
		);
	}
}
