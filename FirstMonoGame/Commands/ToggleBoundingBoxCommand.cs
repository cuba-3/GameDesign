using FirstMonoGame.Entity;
using System.Collections.ObjectModel;

namespace FirstMonoGame.Commands
{
    class ToggleBoundingBoxCommand : ICommand
    {
        public Collection<IEntity> Entities { get; set; }

        public ToggleBoundingBoxCommand(Collection<IEntity> levelEntities)
        {
            Entities = levelEntities;
        }

        public void Execute()
        {
            foreach(IEntity entity in Entities)
            {
                if (entity.VisBounding)
                {
                    entity.VisBounding = false;
                } else
                {
                    entity.VisBounding = true;
                }
            }
        }

        public void HeldKey()
        {
            return;
        }

        public void Release()
        {
            return;
        }
    }
}
