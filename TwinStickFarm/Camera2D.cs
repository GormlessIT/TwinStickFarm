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

        // Dictionary of zoom levels
        public Dictionary<string, float> ZoomLevels { get; set; }

        // Current zoom level
        public string CurrentZoomLevel { get; set; }

        // Store viewport size for offset calculations
        public Rectangle Viewport { get; set; }

        // Dead zone around camera center where player moves without camera following
        public Rectangle DeadZone { get; set; }

        // Bounds of the world that the camera can move in
        public Rectangle WorldBounds { get; set; }

        // Smoothing factor: higher values make camera catch up to player faster
        public float Smoothing { get; set; } = 5f;

        // View transformation matrix used when drawing
        public Matrix Transform
        {
            get
            {
                // Translation matrix to move the camera to the correct position
                Matrix translation = Matrix.CreateTranslation(new Vector3(-Position, 0));

                // Create a scale matrix to zoom in/out
                Matrix scale = Matrix.CreateScale(GetZoomFactor());

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

            // Initialize zoom levels dictionary
            ZoomLevels = new Dictionary<string, float>
            {
                { "FAR", 0.65f },
                { "DEFAULT", 0.9f },
                { "CLOSE", 1.25f }
            };

            CurrentZoomLevel = "DEFAULT";

            // Initialize default DeadZone size 
            DeadZone = new Rectangle(-100, -100, 100, 100); // 100x100px centered on camera
        }

        // Get zoom factor based on current zoom level
        private float GetZoomFactor()
        {
            if(ZoomLevels.ContainsKey(CurrentZoomLevel))
            {
                return ZoomLevels[CurrentZoomLevel];
            }
            else
            {
                return 1f;
            }
        }

        // Update camera position based on player position and dead zone
        public void Update(Vector2 playerPosition, float deltaTime)
        {
            // Calculate camera's center in world coordinates
            Vector2 cameraCenter = Position;

            // Calculate offset between playerPosition and cameraCenter
            Vector2 offset = playerPosition - cameraCenter;

            // Check if playerPosition is outside of the dead zone
            if (offset.X < DeadZone.Left || offset.X > DeadZone.Right ||
                offset.Y < DeadZone.Top  || offset.Y > DeadZone.Bottom)
            {
                // Calculate desired position for the camera so that player is inside the dead zone when camera moves
                Vector2 desiredPosition = Position;

                if (offset.X < DeadZone.Left)
                {
                    desiredPosition.X = playerPosition.X - DeadZone.Left;
                }
                else if (offset.X > DeadZone.Right)
                {
                    desiredPosition.X = playerPosition.X - DeadZone.Right;
                }

                if (offset.Y < DeadZone.Top)
                {
                    desiredPosition.Y = playerPosition.Y - DeadZone.Top;
                }
                else if (offset.Y > DeadZone.Bottom)
                {
                    desiredPosition.Y = playerPosition.Y - DeadZone.Bottom;
                }

                // Smoothly interpolate camera position towards desired position
                Position = Vector2.Lerp(Position, desiredPosition, Smoothing * deltaTime);
            }

            // Clamp camera position to world bounds
            // Calculate the half-width and half-height of the viewport (taking zoom into account)
            float halfWidth = (Viewport.Width / GetZoomFactor()) / 2f;
            float halfHeight = (Viewport.Height / GetZoomFactor()) / 2f;

            // Clamp X and Y so camera doesn't reveal space outside of the world bounds
            float clampedX = MathHelper.Clamp(Position.X, WorldBounds.Left + halfWidth, WorldBounds.Right - halfWidth);
            float clampedY = MathHelper.Clamp(Position.Y, WorldBounds.Top + halfHeight, WorldBounds.Bottom - halfHeight);

            // Update camera position with clamped values
            Position = new Vector2(clampedX, clampedY);
        }
    }
}
