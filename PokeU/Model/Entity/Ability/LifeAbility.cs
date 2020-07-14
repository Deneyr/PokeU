using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeU.Model.Entity.Ability
{
    public class LifeAbility: AAbility
    {
        private int currentHP;

        //private LIFE_STATE lifeState;

        public event Action<IAbility, LifeState> LifeStateChanged;

        //public LIFE_STATE LifeState
        //{
        //    get
        //    {
        //        return this.lifeState;
        //    }
        //    protected set
        //    {
        //        if(this.lifeState != value)
        //        {
        //            this.lifeState = value;

        //            this.NotifyLifeStateChanged();
        //        }
        //    }
        //}

        public int MaxHP
        {
            get;
            protected set;
        }

        public int CurrentHP
        {
            get
            {
                return this.currentHP;
            }
            set
            {
                if(value > this.MaxHP)
                {
                    value = this.MaxHP;
                }
                else if(value < 0)
                {
                    value = 0;
                }

                if (this.currentHP != value)
                {
                    int oldValue = this.currentHP;
                    this.currentHP = value;

                    this.UpdateLifeState(oldValue);
                }
            }
        }

        public LifeAbility(int pMaxHP)
        {
            this.MaxHP = pMaxHP;
            this.currentHP = pMaxHP;

            //this.lifeState = LIFE_STATE.FULL_LIFE;
        }

        private void UpdateLifeState(int oldHP)
        {
            LifeState state = LifeState.FULL_LIFE;

            if (this.currentHP <= 0)
            {
                state = LifeState.DEAD;
            }
            else if (this.currentHP >= this.MaxHP)
            {
                state = LifeState.FULL_LIFE;
            }
            else if(oldHP < this.currentHP)
            {
                state = LifeState.INJURED;
            }
            else if(oldHP > this.currentHP)
            {
                state = LifeState.HEALED;
            }

            this.NotifyLifeStateChanged(state);
        }

        private void NotifyLifeStateChanged(LifeState newLifeState)
        {
            if(this.LifeStateChanged != null)
            {
                this.LifeStateChanged(this, newLifeState);
            }
        }
    }

    public enum LifeState
    {
        DEAD,
        INJURED,
        HEALED,
        FULL_LIFE
    }
}
