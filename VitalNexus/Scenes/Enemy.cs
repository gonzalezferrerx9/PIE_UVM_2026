using Godot;

public partial class Enemy : Area2D
{
	[Export]
	public float MaxHealth = 100.0f;

	private float currentHealth;

	public override void _Ready()
	{
		currentHealth = MaxHealth;

		AddToGroup("enemies");
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
