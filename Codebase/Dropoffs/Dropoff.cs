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
    }


    class Dropoff : Draggable
    {
        public static Dictionary<DropoffType, DropoffProperties> dropoffTypes = new Dictionary<DropoffType, DropoffProperties>();
        public static void InitTypes(ContentManager content)
        {
            dropoffTypes.Add(DropoffType.Dropoff_Food_Low, DropoffPropertiesFile.GetPropertiesHealthLow(content));
            dropoffTypes.Add(DropoffType.Dropoff_Food_Medium, DropoffPropertiesFile.GetPropertiesHealthMedium(content));
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

        public int RemainingUses
        {
            get;
            private set;
        }


        DropoffProperties dropoffProperties;

        public Dropoff(DropoffProperties properties, Rectangle storeSlot)
            :base(storeSlot)
        {
            CurrentState = DropoffState.Unordered;
            dropoffProperties = properties;
            ResetToDefaultProperties();
        }

        public virtual void LoadContent(ContentManager content)
        {
            base.SetContent(dropoffProperties.shopTexture, dropoffProperties.draggingTexture);
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

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
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
            newDropoff.SetContent(oldDropoff.dropoffProperties.shopTexture, oldDropoff.dropoffProperties.draggingTexture);

            return newDropoff;
        }
    }
}
