using Godot;

public partial class PlayerController : CharacterBody2D
{
	[Export]
	public float Speed = 300.0f;

	[Export]
	public PackedScene ProjectileScene;

	[Export]
	public float FireRate = 0.15f;

	private Marker2D muzzle;
	private double fireCooldown = 0.0;

	public override void _Ready()
	{
		muzzle = GetNode<Marker2D>("Muzzle");

		if (ProjectileScene == null)
		{
			GD.PrintErr("ERROR: ProjectileScene no está asignado.");
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		Vector2 direction = Vector2.Zero;

		if (Input.IsKeyPressed(Key.W))
			direction.Y -= 1;

		if (Input.IsKeyPressed(Key.S))
			direction.Y += 1;

		if (Input.IsKeyPressed(Key.A))
			direction.X -= 1;

		if (Input.IsKeyPressed(Key.D))
			direction.X += 1;

		direction = direction.Normalized();

		Velocity = direction * Speed;

		MoveAndSlide();

		// Apuntar al mouse
		Vector2 mousePosition = GetGlobalMousePosition();

		LookAt(mousePosition);

		// Disparo
		fireCooldown -= delta;

		if (Input.IsMouseButtonPressed(MouseButton.Left))
		{
			if (fireCooldown <= 0)
			{
				Shoot();

				fireCooldown = FireRate;
			}
		}
	}

	private void Shoot()
	{
		if (ProjectileScene == null)
			return;

		Projectile projectile =
			ProjectileScene.Instantiate<Projectile>();

		Vector2 shootDirection =
			muzzle.GlobalTransform.X.Normalized();

		projectile.GlobalPosition =
			muzzle.GlobalPosition;

		projectile.Direction =
			shootDirection;

		GetTree().CurrentScene.AddChild(projectile);
	}
}
