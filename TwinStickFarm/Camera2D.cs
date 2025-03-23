using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwinStickFarm
{
    public class Camera2D
    {
        // Position of the camera in the world space
        public Vector2 Position { get; set; }

        // Zoom level of the camera (1 = normal, > 1 = zoom in, <1 = zoom out)
        public float Zoom { get; set; }

        // Store viewport size to center the camera
        public Rectangle Viewport { get; set; }

        // View transformation matrix used when drawing
        public Matrix Transform
        {
            get
            {
                // Translation matrix to move the camera to the correct position
                Matrix translation = Matrix.CreateTranslation(new Vector3(-Position, 0));

                // Create a scale matrix to zoom in/out
                Matrix scale = Matrix.CreateScale(Zoom);

                // Add offset so that camera's position is in the center of the viewport
                Matrix offset = Matrix.CreateTranslation(new Vector3(Viewport.Width * 0.5f, Viewport.Height * 0.5f, 0));

                // Combine the matrices to create the view transformation matrix
                return translation * scale * offset;
            }
        }

        // Constructor: Sets default position to (0, 0) and zoom to 1
        public Camera2D()
        {
            Position = Vector2.Zero;
            Zoom = 1f;
        }
    }
}
