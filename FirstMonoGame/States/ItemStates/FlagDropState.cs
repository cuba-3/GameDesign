using FirstMonoGame.Entity;
using FirstMonoGame.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace FirstMonoGame.States.ItemStates
{
    class FlagDropState : IItemState<Flag>
    {
        public void DoAction(Flag flag)
        {
            /*var state = new FlagDropState();
            flag.FlagState = state;
            flag.Sprite = FlagSpriteFactory.Instance.FactoryMethod(flag.Content, flag.Graphics, flag); */
            return;
        }
    }
}
