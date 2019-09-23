using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;


namespace FirstMonoGame.Commands
{
    class MuteCommand : ICommand
    {
        

        public MuteCommand()
        {
            
        }

        public void Execute()
        {
            if(SoundEffect.MasterVolume ==0.0f)
            {
                SoundEffect.MasterVolume = 1.0f;
            }
            else
            {
                SoundEffect.MasterVolume =0.0f;
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
