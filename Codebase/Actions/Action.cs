using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace GGJ_DisasterMode.Codebase.Actions
{
    enum ActionState
    {
        Inactive,
        Active,
    }

    enum ActionType
    {
        ListenAction,
        DirectAction,
    }

    class GameAction : Draggable
    {
        public Point? ActionPosition
        {
            get;
            private set;
        }

        public Point? ActionDirection
        {
            get;
            private set;
        }

        public ActionType ActionType
        {
            get;
            private set;
        }

        public ActionState ActionState
        {
            get;
            private set;
        }

        Rectangle uiLocation;

        public GameAction(ActionType actionType, Rectangle uiLocation)
            :base(uiLocation)
        {
            ActionType = actionType;
            ActionPosition = null;
            ActionDirection = null;

            ActionState = Actions.ActionState.Inactive;

            this.uiLocation = uiLocation;

            //once placed, we don't allow the player to mvoe them this go
            Redraggable = false;
        }

        public void LoadContent(ContentManager content)
        {
            //Load textures for different types of actions 
            Texture2D uiTexture = content.Load<Texture2D>("Graphics/Dropoffs/Dropoff_FirstAid_Basic");
            base.SetContent(uiTexture, null);
        }



        public void PlaceAction(Point mapLocation)
        {
            if (ActionType == Actions.ActionType.DirectAction)
            {
                throw new Exception("Tried to place direct action without a direction");
            }

            ActionPosition = mapLocation;

            ActionState = Actions.ActionState.Active;
        }

        public void PlaceAction(Point mapLocation, Point targetLocation)
        {
            if (ActionType == Actions.ActionType.ListenAction)
            {
                throw new Exception("Tried to place listen action with too many parameters");
            }

            ActionPosition = mapLocation;
            ActionDirection = targetLocation;

            ActionState = Actions.ActionState.Active;
        }

        public void Update(GameTime gameTime)
        {
        }

        public void ProcessDay()
        {
        }

        public static GameAction CreateNewActionFromAction(GameAction oldAction, Rectangle uiLocation)
        {
            GameAction newAction = new GameAction(oldAction.ActionType, uiLocation);
            newAction.SetContent(oldAction.staticTexture, oldAction.draggingTexture);

            return newAction;
        }
    }
}
