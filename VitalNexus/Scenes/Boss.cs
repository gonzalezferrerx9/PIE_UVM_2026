using Godot;

public partial class Boss : StaticBody2D
{
	[Export]
	public float MaxHealth = 500.0f;

	[Export]
	public float ShootInterval = 2.0f;

	[Export]
	public PackedScene BulletScene;

	private float currentHealth;
	private double shootTimer = 0.0;
	private Node2D player;

	public override void _Ready()
	{
		currentHealth = MaxHealth;
		AddToGroup("enemies"); // Se une al grupo para que tu UI/GameManager lo tome en cuenta

		var players = GetTree().GetNodesInGroup("player");
		if (players.Count > 0)
		{
			player = players[0] as Node2D;
		}
	}

	public override void _Process(double delta)
	{
		if (player == null)
		{
			var players = GetTree().GetNodesInGroup("player");
			if (players.Count > 0) player = players[0] as Node2D;
			return;
		}

		shootTimer += delta;
		if (shootTimer >= ShootInterval)
		{
			shootTimer = 0.0;
			ShootAtPlayer();
		}
	}

	private void ShootAtPlayer()
	{
		if (player == null || BulletScene == null) return;

		var bullet = BulletScene.Instantiate() as Node2D;
		if (bullet == null) return;

		bullet.GlobalPosition = GlobalPosition;

		Vector2 direction = (player.GlobalPosition - GlobalPosition).Normalized();
		bullet.Set("Direction", direction);

		GetTree().CurrentScene.AddChild(bullet);
	}

	public void TakeDamage(float damage)
	{
		currentHealth -= damage;
		GD.Print("BOSS DAÑO! VIDA RESTANTE: ", currentHealth);

		if (currentHealth <= 0)
		{
			// Se destruye exactamente igual que las bacterias para dar paso a la UI
			QueueFree();
		}
	}
}
