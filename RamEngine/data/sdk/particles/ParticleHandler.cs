using System.Collections.Generic;
using System.Drawing;

public class ParticleHandler
{
    /// <summary>
    /// Creates a particle
    /// </summary>
    public SolidObject CreateParticle(float size, float speed, string texture, float dura)
    {
        SolidObject particle = new SolidObject(
            new Point(0, 0),
            new Size((int)size,
            (int)size),
            texture,
            new List<string>() { "Particle" }
        );

        particle.SetSpeed(speed);
        particle.SetDuration(dura);
        
        particle.Anchored = false;

        return particle;
    }

    /// <summary>
    /// Ticks a particle
    /// </summary>
    public void TickParticle(SolidObject particle)
    {
        if (particle == null) return;

        particle.SetDuration(particle.Duration - 1);

    }
}