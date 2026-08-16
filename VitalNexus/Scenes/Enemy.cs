using Godot;

public partial class Enemy : Area2D
{
	[Export]
	public float MaxHealth = 100.0f;

	[Export]
	public float Speed = 50.0f; // Velocidad de movimiento

	[Export]
	public bool FollowPlayer = true; // Si es true, persigue al jugador; si es false, se mueve flotando

	[Export]
	public Texture2D[] TextureVariants; // Lista de texturas/sprites para variaciones

	private float currentHealth;
	private Node2D player;
	private Sprite2D sprite;
	private Vector2 randomDirection;

	public override void _Ready()
	{
		currentHealth = MaxHealth;
		AddToGroup("enemies");

		// Buscar al Sprite2D hijo para cambiar su textura
		sprite = GetNodeOrNull<Sprite2D>("Sprite2D");

		// Asignar textura aleatoria si configuraste alguna en el Inspector
		if (sprite != null && TextureVariants != null && TextureVariants.Length > 0)
		{
			int randomIndex = GD.RandRange(0, TextureVariants.Length - 1);
			sprite.Texture = TextureVariants[randomIndex];
		}

		// Buscar al jugador en el grupo "player" (asegúrate que el Player esté en ese grupo)
		var players = GetTree().GetNodesInGroup("player");
		if (players.Count > 0)
		{
			player = players[0] as Node2D;
		}

		// Dirección aleatoria por si no persigue al jugador
		randomDirection = new Vector2(
			(float)GD.RandRange(-1.0, 1.0),
			(float)GD.RandRange(-1.0, 1.0)
		).Normalized();
	}

	public override void _Process(double delta)
	{
		Move((float)delta);
	}

	private void Move(float delta)
	{
		if (FollowPlayer && player != null)
		{
			// Moverse hacia el jugador
			Vector2 direction = (player.GlobalPosition - GlobalPosition).Normalized();
			GlobalPosition += direction * Speed * delta;

			// Voltear el sprite según la dirección
			if (sprite != null && direction.X != 0)
			{
				sprite.FlipH = direction.X < 0;
			}
		}
		else
		{
			// Flotar en una dirección aleatoria
			GlobalPosition += randomDirection * Speed * delta;
		}
	}

	public void TakeDamage(float damage)
	{
		currentHealth -= damage;

		GD.Print("ENEMIGO RECIBE DAÑO: ", damage);
		GD.Print("VIDA RESTANTE: ", currentHealth);

		if (currentHealth <= 0)
		{
			Die();
		}
	}

	private void Die()
	{
		GD.Print("ENEMIGO DESTRUIDO");
		QueueFree();
	}
}
