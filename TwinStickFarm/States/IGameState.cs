using Microsoft.Xna.Framework;

namespace TwinStickFarm.States
{
    public interface IGameState
    {
        void Enter();
        void Exit();
        void Update(GameTime gameTime);
        void Draw(GameTime gameTime);
    }
}