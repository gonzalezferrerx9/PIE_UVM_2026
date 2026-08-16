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

		// 1. Detectar bacterias (Area2D)
		Godot.Collections.Array<Area2D> areas = GetOverlappingAreas();
		foreach (Area2D area in areas)
		{
			if (TryDamageTarget(area))
			{
				QueueFree();
				return;
			}
		}

		// 2. Detectar al Boss (StaticBody2D / Node2D)
		Godot.Collections.Array<Node2D> bodies = GetOverlappingBodies();
		foreach (Node2D body in bodies)
		{
			if (TryDamageTarget(body))
			{
				QueueFree();
				return;
			}
		}
	}

	private bool TryDamageTarget(Node2D target)
	{
		// Si el objetivo está en el grupo "enemies" y tiene el método TakeDamage
		if (target.IsInGroup("enemies"))
		{
			if (target.HasMethod("TakeDamage"))
			{
				target.Call("TakeDamage", Damage);
				return true;
			}
		}
		return false;
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
