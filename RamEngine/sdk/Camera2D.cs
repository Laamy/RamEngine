using OpenTK;

public class Camera2D
{
    public Vector2 Position { get; private set; }
    public float Zoom { get; private set; }
    public Matrix4 ViewMatrix { get; private set; }

    public Camera2D(Vector2 position, float zoom)
    {
        Position = position;
        Zoom = zoom;
        UpdateViewMatrix();
    }

    public void SetPosition(Vector2 position)
    {
        Position = position;
        UpdateViewMatrix();
    }

    public void SetZoom(float zoom)
    {
        Zoom = zoom;
        UpdateViewMatrix();
    }

    public void Move(Vector2 delta)
    {
        Position += delta;
        UpdateViewMatrix();
    }

    public void ZoomIn(float factor)
    {
        Zoom *= factor;
        UpdateViewMatrix();
    }

    public void ZoomOut(float factor)
    {
        Zoom /= factor;
        UpdateViewMatrix();
    }

    private void UpdateViewMatrix()
    {
        // Calculate the view matrix based on the camera's position and zoom
        ViewMatrix = Matrix4.CreateTranslation(new Vector3(-Position.X, -Position.Y, 0.0f)) *
                     Matrix4.CreateScale(Zoom, Zoom, 1.0f);
    }
}