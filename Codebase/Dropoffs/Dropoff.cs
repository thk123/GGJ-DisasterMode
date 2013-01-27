using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

using GGJ_DisasterMode.Codebase.Characters;

namespace GGJ_DisasterMode.Codebase.Dropoffs
{
    enum DropoffType
    {
        Dropoff_Health_Low,
        Dropoff_Food_Low,
        Dropoff_Water_Low,
        Dropoff_Temperature_Low,

        Dropoff_Health_Medium,
        Dropoff_Food_Medium,
        Dropoff_Water_Medium,
        Dropoff_Temperature_Medium,

        Dropoff_Health_High,
        Dropoff_Food_High,
        Dropoff_Water_High,
        Dropoff_Temperature_High,
    }

    enum DropoffState
    {
        Unordered,
        Ordered,
        Placed,
        Delivered,
        Used,
        Decayed, 
    }

    struct DropoffProperties
    {
        public DropoffType type;
        public int duration;
        public int delay;
        public int useCount;

        public Texture2D shopTexture;
        public Texture2D draggingTexture;
        public Texture2D placedTexture;
        public Texture2D navTexture;
    }


    class Dropoff : Draggable
    {
        public static Dictionary<DropoffType, DropoffProperties> dropoffTypes = new Dictionary<DropoffType, DropoffProperties>();
        public static void InitTypes(ContentManager content)
        {
            dropoffTypes.Add(DropoffType.Dropoff_Food_Low, DropoffPropertiesFile.GetPropertiesFoodLow(content));
            dropoffTypes.Add(DropoffType.Dropoff_Food_Medium, DropoffPropertiesFile.GetPropertiesFoodMedium(content));
            dropoffTypes.Add(DropoffType.Dropoff_Food_High, DropoffPropertiesFile.GetPropertiesFoodHigh(content));
            dropoffTypes.Add(DropoffType.Dropoff_Health_Low, DropoffPropertiesFile.GetPropertiesHealthLow(content));
            dropoffTypes.Add(DropoffType.Dropoff_Health_Medium, DropoffPropertiesFile.GetPropertiesHealthMedium(content));
            dropoffTypes.Add(DropoffType.Dropoff_Health_High, DropoffPropertiesFile.GetPropertiesHealthHigh(content));
            dropoffTypes.Add(DropoffType.Dropoff_Water_Low, DropoffPropertiesFile.GetPropertiesWaterLow(content));
            dropoffTypes.Add(DropoffType.Dropoff_Water_Medium, DropoffPropertiesFile.GetPropertiesWaterMedium(content));
            dropoffTypes.Add(DropoffType.Dropoff_Water_High, DropoffPropertiesFile.GetPropertiesWaterHigh(content));
            dropoffTypes.Add(DropoffType.Dropoff_Temperature_Low, DropoffPropertiesFile.GetPropertiesTempLow(content));
            dropoffTypes.Add(DropoffType.Dropoff_Temperature_Medium, DropoffPropertiesFile.GetPropertiesTempMedium(content));
            dropoffTypes.Add(DropoffType.Dropoff_Temperature_High, DropoffPropertiesFile.GetPropertiesTempHigh(content));
        }

        

        public DropoffType DropoffType
        {
            get
            {
                return dropoffProperties.type;
            }
        }

        public DropoffState CurrentState
        {
            get;
            private set;
        }

        public int DaysToDelivery
        {
            get;
            private set;
        }

        public int DaysToDecay
        {
            get;
            private set;
        }

        private int RemainingUses
        {
            get;
            set;
        }

        public bool IsAvaliable
        {
            get
            {
                //either infinite uses of more than 0     && not decayed...
                return (RemainingUses > 0 || RemainingUses == -1) && (CurrentState != DropoffState.Decayed);
            }
        }

        Rectangle gridPosition;

        DropoffProperties dropoffProperties;

        Gameplay.Grid.Buckets buckets;
        Point gridPoint;

        public Dropoff(DropoffProperties properties, Rectangle storeSlot)
            :base(storeSlot)
        {
            CurrentState = DropoffState.Unordered;
            dropoffProperties = properties;
            ResetToDefaultProperties();
        }

        public virtual void LoadContent(ContentManager content)
        {
            base.SetContent(dropoffProperties.shopTexture, dropoffProperties.draggingTexture, dropoffProperties.draggingTexture);
        }

        public virtual void PlaceDropoff(Rectangle gridRectangle, Gameplay.Grid.Buckets bucketSystem, Point gridPoint)
        {
            CurrentState = DropoffState.Placed;
            gridPosition = gridRectangle;
            this.gridPoint = gridPoint;
            buckets = bucketSystem;
        }

        public virtual void ProcessDay()
        {
            switch (CurrentState)
            {
                case DropoffState.Unordered:
                    //Nothing changes - we are in the shop
                    break;
                case DropoffState.Ordered:
                    // Probably something has gone wrong?
                    break;
                case DropoffState.Placed:
                    // 
                    --DaysToDelivery;
                    if (DaysToDelivery == 0)
                    {
                        CurrentState = DropoffState.Delivered;

                        //alert the grid to our arrival 
                        buckets.addNewDrop(this, gridPosition.Center.X, gridPosition.Center.Y);
                    }
                    break;
                case DropoffState.Delivered:
                    --DaysToDecay;
                    if (DaysToDecay == 0)
                    {
                        CurrentState = DropoffState.Decayed;
                    }
                    break;
                case DropoffState.Used:
                    break;

                case DropoffState.Decayed:
                    break;
                default:
                    break;
            }
        }

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch, bool enabled, bool inNavMode)
        {
            if (!inNavMode)
            {
                base.Draw(spriteBatch, enabled);

                if (CurrentState == DropoffState.Placed)
                {
                    //draw relevant clock
                    Texture2D clockTexture = ClockDrawer.DrawClock(ClockDrawer.GetClockType(DaysToDelivery, dropoffProperties.delay));
                    Vector2 drawPos = new Vector2(gridPosition.Center.X - (clockTexture.Width / 2.0f), gridPosition.Center.Y - (clockTexture.Height / 2.0f));
                    //drawPos.Y -= 40;
                    spriteBatch.Draw(clockTexture, drawPos, Color.White);
                }
            }
            else
            {
                // in nav mode we hide other entites
                if (CurrentState == DropoffState.Delivered)
                {
                    Vector2 drawPos = new Vector2(gridPosition.Center.X - (dropoffProperties.navTexture.Width / 2.0f), gridPosition.Center.Y - (dropoffProperties.navTexture.Height / 2.0f));
                    spriteBatch.Draw(dropoffProperties.navTexture, drawPos, Color.White);
                }
            }
        }

        public virtual void UseDropoff(Civilian character)
        {
            --RemainingUses;
            if (RemainingUses == 0)
            {
                CurrentState = DropoffState.Used;
            }
        }

        private void ResetToDefaultProperties()
        {
            DaysToDelivery = dropoffProperties.delay;
            DaysToDecay = dropoffProperties.duration;
            RemainingUses = dropoffProperties.useCount;
        }

        public static Dropoff CreateNewDropoffFromDropoff(Dropoff oldDropoff, Rectangle uiPosition)
        {
            Dropoff newDropoff = new Dropoff(Dropoff.dropoffTypes[oldDropoff.dropoffProperties.type], uiPosition);
            newDropoff.SetContent(oldDropoff.dropoffProperties.shopTexture, oldDropoff.dropoffProperties.draggingTexture, oldDropoff.dropoffProperties.draggingTexture);

            return newDropoff;
        }
    }
}
