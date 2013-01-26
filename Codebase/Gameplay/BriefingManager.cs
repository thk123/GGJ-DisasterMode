using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using GGJ_DisasterMode.Codebase.Screens;

namespace GGJ_DisasterMode.Codebase.Gameplay
{
    public class BriefingManager
    {
        private ContentManager content;
        private Texture2D target;
        private Texture2D backgroundTexture;
        private Texture2D intelTexture;
        private Texture2D opsTexture;
        private Texture2D targetTexture;

        private SpriteFont font;

        private Texture2D overlayTexture;

        private bool missionRunning;

        public BriefingManager(ContentManager content)
        {
            this.content = new ContentManager(content.ServiceProvider);
            this.missionRunning = true;
            
            this.font = content.Load<SpriteFont>("Fonts//title");
            this.backgroundTexture = content.Load<Texture2D>("graphics//backgrounds//brief_back");
            this.overlayTexture = content.Load<Texture2D>("graphics//blank");

            //this.placeholder = content.Load<Model>("Models//car");

            //this.actor = new Actor3();
            //actor.Rotation = new Vector3(0.0f, 0.0f, MathHelper.ToRadians(90.0f));
            //actor.AngularVelocity = new Vector3(0.0f, 0.0f, 0.015f);
        }

        public void Update(GameTime gameTime, out bool missionRunning)
        {
           // actor.Update();

            //camera.TargetPosition = actor.Position;

            //camera.Update(gameTime);

            missionRunning = this.missionRunning;
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            spriteBatch.Draw(backgroundTexture, new Rectangle(0, 0, 850, 480), Color.White);
            spriteBatch.Draw(overlayTexture, new Rectangle(0, 0, 850, 480), Color.White);

            spriteBatch.DrawString(font,

                "Commander\n\n" +
                "Experimental technology has been stolen from a desert base\n" +
                "Retrieve it. Use of lethal force has been authorised\n" +
                "Our generals have identified several suspects entering the city\n" +
                "Make sure you get the right man\n" +
                "Our generals will be providing you with intelligence on each truck \ndirectly through the W.I.N.K network\n" +
                "You must whittle down the suspects and retrieve the technology\n" +
                "Be careful, the three generals in this area are currently under investigation.\n" +
                "Photos clearly show two ranking officers meeting with local gangs, \nthough their identities are unclear\n" +
                "What we do know is that one of our men is definitely honourable \nthough exactly which is not known\n" +
                "The city radar system will track the progress of each truck\n" +
                "Contain the suspects, placing road blocks and traffic jams using \nyour command console to hinder them\n" +
                "If the target escapes the city the technology will be lost\n" +
                "A sniper is prepped in a helicopter above the city to act on your orders.",
                new Vector2(100.0f, 100.0f),
                Color.Red,0.0f, Vector2.Zero, 0.75f, SpriteEffects.None, 1.0f);
   
            spriteBatch.End();

            //ModelArtist.DrawModel(ref placeholder, actor.WorldMatrix, camera.View, camera.Projection);
        }

        /// <summary>
        /// Lets the game respond to player input. Unlike the Update method,
        /// this will only be called when the gameplay screen is active.
        /// </summary>
        public void HandleInput(GamePadState gamePadState)
        {
            if (gamePadState == null)
                throw new ArgumentNullException("input");

            if ( (gamePadState.Buttons.A == ButtonState.Pressed) || Keyboard.GetState().IsKeyDown(Keys.Space))
                this.missionRunning = false;

        }
    }
}
