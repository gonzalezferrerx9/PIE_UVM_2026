using Godot;

public partial class BossBullet : Area2D
{
	[Export]
	public float Speed = 200.0f;

	[Export]
	public float Damage = 10.0f;

	[Export]
	public float LifeTime = 4.0f;

	public Vector2 Direction = Vector2.Zero;

	public override void _Ready()
	{
		BodyEntered += OnBodyEntered;
		AreaEntered += OnAreaEntered;

		// Se destruye solo tras unos segundos
		GetTree().CreateTimer(LifeTime).Timeout += () => QueueFree();
	}

	public override void _Process(double delta)
	{
		GlobalPosition += Direction * Speed * (float)delta;
	}

	private void OnBodyEntered(Node2D body)
	{
		if (body.IsInGroup("player"))
		{
			if (body.HasMethod("TakeDamage"))
			{
				body.Call("TakeDamage", Damage);
			}
			QueueFree();
		}
	}

	private void OnAreaEntered(Area2D area)
	{
		if (area.IsInGroup("player"))
		{
			if (area.HasMethod("TakeDamage"))
			{
				area.Call("TakeDamage", Damage);
			}
			QueueFree();
		}
	}
}
