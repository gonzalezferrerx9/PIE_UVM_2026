using Godot;

public partial class Projectile : Area2D
{
	[Export]
	public float Speed = 800.0f;

	[Export]
	public float Damage = 25.0f;

	public Vector2 Direction = Vector2.Right;

	public override void _Ready()
	{
		Monitoring = true;
		Monitorable = true;

		QueueRedraw();
	}

	public override void _PhysicsProcess(double delta)
	{
		GlobalPosition += Direction * Speed * (float)delta;

		Godot.Collections.Array<Area2D> areas = GetOverlappingAreas();

		foreach (Area2D area in areas)
		{
			if (area is Enemy enemy)
			{
				enemy.TakeDamage(Damage);

				QueueFree();

				return;
			}
		}
	}

	public override void _Draw()
	{
		DrawCircle(
			Vector2.Zero,
			6.0f,
			Colors.White
		);
	}
}
