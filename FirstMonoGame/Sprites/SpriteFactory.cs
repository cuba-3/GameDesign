using FirstMonoGame.Sprites;
using FirstMonoGame.States.PeachStates;
using FirstMonoGame.States2.PeachStates;
using FirstMonoGame.States.EnemyStates;
using FirstMonoGame.Interfaces;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using FirstMonoGame.Entity;
using FirstMonoGame.States.BlockStates;
using Microsoft.Xna.Framework;
using FirstMonoGame.States.ItemStates;
using System.Diagnostics;
using FirstMonoGame.States2.PowerUpStates;

namespace FirstMonoGame
{
    #region Factories
    abstract class AvatarSpriteFactory<T>
    {
        protected AvatarSpriteFactory()
        {
        }
        public abstract AvatarSprite FactoryMethod(T Avatar);
    }

    abstract class EnemySpriteFactory<T>
    {
        protected EnemySpriteFactory()
        {
        }
        public abstract EnemySprite FactoryMethod(ContentManager content, T enemy, GraphicsDevice graphics);
    }

    abstract class BlockSpriteFactory<T>
    {
        protected BlockSpriteFactory()
        {
        }
        public abstract BlockSprite FactoryMethod(ContentManager content, T block, GraphicsDevice graphics, bool underworld);
    }

    abstract class ItemSpriteFactory<T>
    {
        protected ItemSpriteFactory()
        {
        }
        public abstract ItemSprite FactoryMethod(ContentManager content, GraphicsDevice graphics, T item);
    }

    abstract class GradeSpriteFactory<T>
    {
        protected GradeSpriteFactory()
        {
        }
        public abstract ItemSprite FactoryMethod(ContentManager content, GraphicsDevice graphics);
    }
    #endregion

    #region Peach Sprite Factory
    class PeachSpriteFactory : AvatarSpriteFactory<Peach>
    {
        private IActionState<Peach> actionState;
        private static AvatarSpriteFactory<Peach> _instance;
        // Standard sprites
        private AvatarSprite walkStandardPeachSprite;
        private AvatarSprite runStandardPeachSprite;
        private AvatarSprite jumpStandardPeachSprite;
        private AvatarSprite fallStandardPeachSprite;
        private AvatarSprite idleStandardPeachSprite;
        private AvatarSprite crouchStandardPeachSprite;
        public static AvatarSpriteFactory<Peach> Instance
        {
            get
            {
                if (_instance == null) _instance = new PeachSpriteFactory();
                return _instance;
            }
        }

        #region Action States
        private AvatarSprite WalkStandardSprite(AvatarSprite previousSprite)
        {
            walkStandardPeachSprite = previousSprite;
            walkStandardPeachSprite.SwitchAnimation(1, 4);
            walkStandardPeachSprite.LoopFrame = true;

            return walkStandardPeachSprite;
        }
        private AvatarSprite RunStandardSprite(AvatarSprite previousSprite)
        {
            runStandardPeachSprite = previousSprite;
            runStandardPeachSprite.SwitchAnimation(1, 4);
            runStandardPeachSprite.LoopFrame = true;

            return runStandardPeachSprite;
        }
        private AvatarSprite JumpStandardSprite(AvatarSprite previousSprite)
        {
            jumpStandardPeachSprite = previousSprite;
            jumpStandardPeachSprite.SwitchAnimation(3, 4);
            jumpStandardPeachSprite.SetFrame(Constants.ZERO);
            jumpStandardPeachSprite.LoopFrame = false;

            return jumpStandardPeachSprite;
        }
        private AvatarSprite FallStandardSprite(AvatarSprite previousSprite)
        {
            fallStandardPeachSprite = previousSprite;
            fallStandardPeachSprite.SwitchAnimation(4, 4);
            fallStandardPeachSprite.LoopFrame = false;

            return fallStandardPeachSprite;
        }
        private AvatarSprite IdleStandardSprite(AvatarSprite previousSprite)
        {
            idleStandardPeachSprite = previousSprite;
            idleStandardPeachSprite.SwitchAnimation(Constants.ZERO, 6);
            idleStandardPeachSprite.LoopFrame = true;

            return idleStandardPeachSprite;
        }
        private AvatarSprite CrouchStandardSprite(Peach peach)
        {
            AvatarSprite previousSprite = (AvatarSprite)peach.Sprite;
            if (peach.PowerUpState is StandardState)
            {
                idleStandardPeachSprite = previousSprite;
                idleStandardPeachSprite.SwitchAnimation(Constants.ZERO, 6);
                idleStandardPeachSprite.LoopFrame = true;

                return idleStandardPeachSprite;
            }
            else
            {
                crouchStandardPeachSprite = previousSprite;
                crouchStandardPeachSprite.SwitchAnimation(2, 2);
                crouchStandardPeachSprite.SetFrame(Constants.ZERO);
                crouchStandardPeachSprite.LoopFrame = false;

                return crouchStandardPeachSprite;
            }
            
        }
        #endregion

        public override AvatarSprite FactoryMethod(Peach peach)
        {
            actionState = peach.ActionState;
            AvatarSprite newSprite = null;
            if (actionState is CrouchingState)
            {
                newSprite = CrouchStandardSprite(peach);
            }
            else if (actionState is FallingState)
            {
                newSprite = FallStandardSprite((AvatarSprite)peach.Sprite);
            }
            else if (actionState is IdleState)
            {
                newSprite = IdleStandardSprite((AvatarSprite)peach.Sprite);
            }
            else if (actionState is JumpingState)
            {
                newSprite = JumpStandardSprite((AvatarSprite)peach.Sprite);
            }
            else if (actionState is RunningState)
            {
                newSprite = RunStandardSprite((AvatarSprite)peach.Sprite);
            }
            else if (actionState is WalkingState)
            {
                newSprite = WalkStandardSprite((AvatarSprite)peach.Sprite);
            }
            return newSprite;
        }
    }
    #endregion

    #region Goomba Sprite Factory
    class GoombaSpriteFactory : EnemySpriteFactory<Goomba>
    {
        private IEnemyState<Goomba> enemyState;
        private static EnemySpriteFactory<Goomba> _instance;
        private EnemySprite deadGoombaSprite;
        private EnemySprite standardGoombaSprite;
        public static EnemySpriteFactory<Goomba> Instance
        {
            get
            {
                if (_instance == null) _instance = new GoombaSpriteFactory();
                return _instance;
            }
        }
        private EnemySprite StandardGoombaSprite(ContentManager content, GraphicsDevice graphics)
        {
            Texture2D stanGoomba = content.Load<Texture2D>("GoombaSpriteSheet");
            standardGoombaSprite = new EnemySprite(graphics, stanGoomba, 2, 2, new Vector2(900, 762),Constants.ZERO, 2);
            return standardGoombaSprite;
        }
        private EnemySprite DeadGoombaSprite(ContentManager content, GraphicsDevice graphics, Goomba oldGoomba)
        {
            Texture2D deadGoomba = content.Load<Texture2D>("GoombaSpriteSheet");
            deadGoombaSprite = new EnemySprite(graphics, deadGoomba, 2, 2, new Vector2(900, 762), 1, 1);
            deadGoombaSprite.CurrentLocation = oldGoomba.Location;
            return deadGoombaSprite;
        }

        public override EnemySprite FactoryMethod(ContentManager content, Goomba goomba, GraphicsDevice graphicsDevice)
        {
            enemyState = goomba.EnemyState;
            if (enemyState is StandardGoombaState)
            {
                return StandardGoombaSprite(content, graphicsDevice);
            }
            else if (enemyState is DeadGoombaState)
            {
                return DeadGoombaSprite(content, graphicsDevice, goomba);
            }
            return StandardGoombaSprite(content, graphicsDevice);
        }
    }
    #endregion

    #region Koopa Troopa Sprite Factory
    class KoopaTroopaSpriteFactory : EnemySpriteFactory<KoopaTroopa>
    {
        private IEnemyState<KoopaTroopa> enemyState;
        private static EnemySpriteFactory<KoopaTroopa> _instance;
        private EnemySprite shellKoopaTroopaSprite;
        private EnemySprite standardKoopaTroopaSprite;
        public static EnemySpriteFactory<KoopaTroopa> Instance
        {
            get
            {
                if (_instance == null) _instance = new KoopaTroopaSpriteFactory();
                return _instance;
            }
        }
        private EnemySprite StandardKoopaTroopaSprite(KoopaTroopa koopaTroopa)
        {
            standardKoopaTroopaSprite = (EnemySprite)koopaTroopa.Sprite;
            standardKoopaTroopaSprite.SwitchAnimation(Constants.ZERO, 2);
            return standardKoopaTroopaSprite;
        }
        private EnemySprite ShellKoopaTroopaSprite(KoopaTroopa koopaTroopa)
        {
            shellKoopaTroopaSprite = (EnemySprite)koopaTroopa.Sprite;
            shellKoopaTroopaSprite.SwitchAnimation(1, 2);
            return shellKoopaTroopaSprite;
        }

        public override EnemySprite FactoryMethod(ContentManager content, KoopaTroopa koopaTroopa, GraphicsDevice graphics)
        {
            enemyState = koopaTroopa.EnemyState;
            if (enemyState is StandardKoopaTroopaState)
            {
                return StandardKoopaTroopaSprite(koopaTroopa);
            }
            else if (enemyState is ShellKoopaState)
            {
                return ShellKoopaTroopaSprite(koopaTroopa);
            }
            return StandardKoopaTroopaSprite(koopaTroopa);
        }
    }
    #endregion

    #region Piranha 
    class PiranhaSpriteFactory : EnemySpriteFactory<Piranha>
    {
        private static EnemySpriteFactory<Piranha> _instance;
        private EnemySprite piranhaSprite;
        public static EnemySpriteFactory<Piranha> Instance
        {
            get
            {
                if (_instance == null) _instance = new PiranhaSpriteFactory();
                return _instance;
            }
        }
        private EnemySprite StandardPiranhaSprite(ContentManager content, GraphicsDevice graphics)
        {
            Texture2D piranha = content.Load<Texture2D>("piranha");
            piranhaSprite = new EnemySprite(graphics, piranha, 1, 3, new Vector2(900, 762),Constants.ZERO, 3);
            return piranhaSprite;
        }
        public override EnemySprite FactoryMethod(ContentManager content, Piranha piranha, GraphicsDevice graphics)
        {
            return StandardPiranhaSprite(content, graphics);
        }
    }
    #endregion

    #region Bowser Sprite Factory
    class BowserSpriteFactory : EnemySpriteFactory<Bowser>
    {
        private EnemySprite bowserSprite;
        private static EnemySpriteFactory<Bowser> _instance;
        private EnemySprite punchingBowserSprite;
        private EnemySprite dyingBowserSprite;
        private EnemySprite walkingBowserSprite;
        private EnemySprite takingDamageBowserSprite;
        private EnemySprite breathingFireBowserSprite;
        private IEnemyState<Bowser> enemyState;
        public static EnemySpriteFactory<Bowser> Instance
        {
            get
            {
                if (_instance == null) _instance = new BowserSpriteFactory();
                return _instance;
            }
        }
        private EnemySprite WalkingBowserSprite(Bowser bowser)
        {
            walkingBowserSprite = (EnemySprite)bowser.Sprite;
            walkingBowserSprite.ScaleSprite(2f);
            walkingBowserSprite.SwitchAnimation(3, 16);
            walkingBowserSprite.LoopFrame = true;
            return walkingBowserSprite;
        }
        private EnemySprite StandardBowserSprite(Bowser bowser)
        {
            bowserSprite = (EnemySprite)bowser.Sprite;
            bowserSprite.ScaleSprite(2f);
            bowserSprite.SwitchAnimation(2, 1);
            bowserSprite.LoopFrame = false;
            return bowserSprite;
        }
        private EnemySprite PunchingBowserSprite(Bowser bowser)
        {
            punchingBowserSprite = (EnemySprite)bowser.Sprite;
            punchingBowserSprite.ScaleSprite(2f);
            punchingBowserSprite.SwitchAnimation(Constants.ZERO, 17);
            punchingBowserSprite.LoopFrame = true;
            return punchingBowserSprite;
        }
        private EnemySprite DeadBowserSprite(Bowser bowser)
        {
            dyingBowserSprite = (EnemySprite)bowser.Sprite;
            dyingBowserSprite.ScaleSprite(2f);
            dyingBowserSprite.SwitchAnimation(5, 9);
            dyingBowserSprite.LoopFrame = false;
            return dyingBowserSprite;
        }
        private EnemySprite TakingDamageBowserSprite(Bowser bowser)
        {
            takingDamageBowserSprite = (EnemySprite)bowser.Sprite;
            takingDamageBowserSprite.ScaleSprite(2f);
            takingDamageBowserSprite.SwitchAnimation(4, 7);
            takingDamageBowserSprite.LoopFrame = false;
            return takingDamageBowserSprite;
        }
        private EnemySprite BreathingFireBowserSprite(Bowser bowser)
        {
            breathingFireBowserSprite = (EnemySprite)bowser.Sprite;
            breathingFireBowserSprite.ScaleSprite(2f);
            breathingFireBowserSprite.SwitchAnimation(1, 17);
            breathingFireBowserSprite.LoopFrame = true;
            return breathingFireBowserSprite;
        }
        public override EnemySprite FactoryMethod(ContentManager content, Bowser bowser, GraphicsDevice graphicsDevice)
        {
            enemyState = bowser.EnemyState;
            if (enemyState is StandardBowserState)
            {
                return StandardBowserSprite(bowser);
            }
            else if (enemyState is DeadBowserState)
            {
                return DeadBowserSprite(bowser);
            }
            else if(enemyState is PunchingBowserState)
            {
                return PunchingBowserSprite(bowser);
            }
            else if(enemyState is WalkingBowserState)
            {
                return WalkingBowserSprite(bowser);
            }
            else if(enemyState is TakingDamageBowserState)
            {
                return TakingDamageBowserSprite(bowser);
            }
            else if(enemyState is BreathingFireBowserState)
            {
                return BreathingFireBowserSprite(bowser);
            }
            return StandardBowserSprite(bowser);
        }

    }

    #endregion

    #region Question Block Sprite Factory
    class QuestionBlockSpriteFactory : BlockSpriteFactory<QuestionBlock>
    {
        private IBlockState<QuestionBlock> blockState;
        private static BlockSpriteFactory<QuestionBlock> _instance;
        private BlockSprite bumpQuestionBlockSprite;
        private BlockSprite usedQuestionBlockSprite;
        private BlockSprite newQuestionBlockSprite;
        public static BlockSpriteFactory<QuestionBlock> Instance
        {
            get
            {
                if (_instance == null) _instance = new QuestionBlockSpriteFactory();
                return _instance;
            }
        }
        private BlockSprite UsedQuestionBlockSprite(ContentManager content, GraphicsDevice graphics, QuestionBlock block)
        {
            usedQuestionBlockSprite = (BlockSprite)block.Sprite;
            Texture2D usedQuestionBlock = content.Load<Texture2D>("Blocks/UsedBlock");

            usedQuestionBlockSprite.InitializeSprite(usedQuestionBlock, graphics, 1, 1);
            return usedQuestionBlockSprite;
        }
        private BlockSprite NewQuestionBlockSprite(ContentManager content, GraphicsDevice graphics)
        {
            Texture2D newQuestionBlock = content.Load<Texture2D>("Blocks/NewQuestionBlockState");
            newQuestionBlockSprite = new BlockSprite(graphics);
            newQuestionBlockSprite.InitializeSprite(newQuestionBlock, graphics, 1, 9);
            newQuestionBlockSprite.LoopFrame = true;
            newQuestionBlockSprite.SwitchAnimation(Constants.ZERO, 9);
            return newQuestionBlockSprite;
        }

        private BlockSprite BumpQuestionBlockSprite(ContentManager content, GraphicsDevice graphics, QuestionBlock block)
        {
            bumpQuestionBlockSprite = (BlockSprite)block.Sprite;
            Texture2D bumpQuestionBlock = content.Load<Texture2D>("Blocks/UsedBlock");
            bumpQuestionBlockSprite.LoopFrame = false;
            bumpQuestionBlockSprite.InitializeSprite(bumpQuestionBlock, graphics, 1, 1);
            bumpQuestionBlockSprite.SwitchAnimation(Constants.ZERO, 1);
            return bumpQuestionBlockSprite;
        }

        public override BlockSprite FactoryMethod(ContentManager content, QuestionBlock block, GraphicsDevice graphics, bool underworld)
        {
            blockState = block.BlockState;
            if (blockState is NewQuestionBlockState)
            {
                return NewQuestionBlockSprite(content, graphics);
            }
            else if (blockState is UsedQuestionBlockState)
            {
                return UsedQuestionBlockSprite(content, graphics, block);
            }
            else if (blockState is BumpQuestionBlockState)
            {
                return BumpQuestionBlockSprite(content, graphics, block);
            }

            return NewQuestionBlockSprite(content, graphics);
        }
    }
    #endregion

    #region Exploding Block Sprite Factory
    class ExplodingBlockSpriteFactory : BlockSpriteFactory<ExplodingBlock>
    {
        //private IBlockState<ExplodingBlock> blockState;
        private static BlockSpriteFactory<ExplodingBlock> _instance;
        private BlockSprite explodingBlockSprite;

        public static BlockSpriteFactory<ExplodingBlock> Instance
        {
            get
            {
                if (_instance == null) _instance = new ExplodingBlockSpriteFactory();
                return _instance;
            }
        }

        private BlockSprite ExplodingGlassSprite(ContentManager content, GraphicsDevice graphics)
        {
            Texture2D explodingBlockTexture = content.Load<Texture2D>("Blocks/ExplodingGlassBlock");
            explodingBlockSprite = new BlockSprite(graphics);
            explodingBlockSprite.InitializeSprite(explodingBlockTexture, graphics, 1, 1);
            explodingBlockSprite.SwitchAnimation(Constants.ZERO, 1);
            return explodingBlockSprite;
        }
        private BlockSprite ExplodingBlockSprite(ContentManager content, GraphicsDevice graphics)
        {
            Texture2D explodingBlockTexture = content.Load<Texture2D>("Blocks/ExplodingBrickBlock");
            explodingBlockSprite = new BlockSprite(graphics);
            explodingBlockSprite.InitializeSprite(explodingBlockTexture, graphics, 1, 1);
            explodingBlockSprite.SwitchAnimation(Constants.ZERO, 1);
            return explodingBlockSprite;
        }
        public override BlockSprite FactoryMethod(ContentManager content, ExplodingBlock block, GraphicsDevice graphics,bool Glass)
        {
            if (Glass)
            {
                return ExplodingGlassSprite(content, graphics);
            }
            else
            {
                return ExplodingBlockSprite(content, graphics);
            }
        }
    }
    #endregion 

    #region Brick Block Sprite Factory
    class BrickBlockSpriteFactory : BlockSpriteFactory<BrickBlock>
    {
        private IBlockState<BrickBlock> blockState;
        private static BlockSpriteFactory<BrickBlock> _instance;
        private BlockSprite bumpBrickBlockSprite;
        private BlockSprite newBrickBlockSprite;
        private BlockSprite explodeBrickBlockSprite1;

        public static BlockSpriteFactory<BrickBlock> Instance
        {
            get
            {
                if (_instance == null) _instance = new BrickBlockSpriteFactory();
                return _instance;
            }
        }
        private BlockSprite NewBrickBlockSprite(ContentManager content, GraphicsDevice graphics, bool underworld)
        {
            Texture2D newBrickBlock;
            if (!underworld)
            {
                newBrickBlock = content.Load<Texture2D>("Blocks/NewBrickBlock");
            }
            else
            {
                newBrickBlock = content.Load<Texture2D>("Blocks/NewBrickBlockUnderworld");
            }
            newBrickBlockSprite = new BlockSprite(graphics);
            newBrickBlockSprite.InitializeSprite(newBrickBlock, graphics, 1, 1);
            newBrickBlockSprite.SwitchAnimation(Constants.ZERO, 1);
            return newBrickBlockSprite;
        }

        private BlockSprite BumpBrickBlockSprite(ContentManager content, GraphicsDevice graphics, BrickBlock block)
        {
            bumpBrickBlockSprite = (BlockSprite)block.Sprite;
            //  bumpBrickBlockSprite.CurrentLocation = block.Location;
            Texture2D bumpBrickBlock = content.Load<Texture2D>("Blocks/UsedBlock");
            //bumpBrickBlockSprite = new BlockSprite(graphics);
            bumpBrickBlockSprite.LoopFrame = false;
            bumpBrickBlockSprite.InitializeSprite(bumpBrickBlock, graphics, 1, 1);
            bumpBrickBlockSprite.SwitchAnimation(Constants.ZERO, 1);
            return bumpBrickBlockSprite;
        }

        private BlockSprite ExplodeBrickBlockSprite(ContentManager content, GraphicsDevice graphics)
        {
            explodeBrickBlockSprite1 = newBrickBlockSprite;
            Texture2D explodingBrickBlock = content.Load<Texture2D>("Blocks/ExplodingBrickBlock");
            //explodeBrickBlockSprite = new BlockSprite(graphics);
            explodeBrickBlockSprite1.InitializeSprite(explodingBrickBlock, graphics, 1, 1);
            explodeBrickBlockSprite1.LoopFrame = false;
            explodeBrickBlockSprite1.SwitchAnimation(Constants.ZERO, 1);
            return explodeBrickBlockSprite1;
        }

        public override BlockSprite FactoryMethod(ContentManager content, BrickBlock block, GraphicsDevice graphics, bool underworld)
        {
            blockState = block.BlockState;
            if (blockState is NewBrickBlockState)
            {
                return NewBrickBlockSprite(content, graphics, block.Underworld);
            }
            else if (blockState is BumpBrickBlockState)
            {
                return BumpBrickBlockSprite(content, graphics, block);

            } else if (blockState is ExplodeBrickBlockState)
            {
                return ExplodeBrickBlockSprite(content, graphics);
            }
            return NewBrickBlockSprite(content, graphics, block.Underworld);
        }
    }
    #endregion

    #region Hidden Block Sprite Factory
    class HiddenBlockSpriteFactory : BlockSpriteFactory<HiddenBlock>
    {
        private static BlockSpriteFactory<HiddenBlock> _instance;
        private BlockSprite hiddenBlockSprite;

        public static BlockSpriteFactory<HiddenBlock> Instance
        {
            get
            {
                if (_instance == null) _instance = new HiddenBlockSpriteFactory();
                return _instance;
            }
        }
        private BlockSprite HiddenBlock(ContentManager content, GraphicsDevice graphics)
        {
            Texture2D hiddenBlock = content.Load<Texture2D>("Blocks/NewBrickBlock");
            hiddenBlockSprite = new BlockSprite(graphics);
            hiddenBlockSprite.InitializeSprite(hiddenBlock, graphics, 1, 1);
            return hiddenBlockSprite;
        }

        public override BlockSprite FactoryMethod(ContentManager content, HiddenBlock block, GraphicsDevice graphics, bool underworld)
        {
            return HiddenBlock(content, graphics);
        }
    }
    #endregion

    #region Floor Block Sprite Factory
    class FloorBlockSpriteFactory : BlockSpriteFactory<FloorBlock>
    {
        private static BlockSpriteFactory<FloorBlock> _instance;
        private BlockSprite floorBlockSprite;

        public static BlockSpriteFactory<FloorBlock> Instance
        {
            get
            {
                if (_instance == null) _instance = new FloorBlockSpriteFactory();
                return _instance;
            }
        }
        private BlockSprite FloorBlock(ContentManager content, GraphicsDevice graphics, bool underworld)
        {
            Texture2D floorBlock;
            if (!underworld)
            {
                floorBlock = content.Load<Texture2D>("Blocks/TileBrick");
            }
            else
            {
                floorBlock = content.Load<Texture2D>("Blocks/UnderworldFloorBlock");
            }
            floorBlockSprite = new BlockSprite(graphics);
            floorBlockSprite.InitializeSprite(floorBlock, graphics, 1, 1);
            return floorBlockSprite;
        }

        public override BlockSprite FactoryMethod(ContentManager content, FloorBlock block, GraphicsDevice graphics, bool underworld)
        {
            return FloorBlock(content, graphics, underworld);
        }
    }
    #endregion

    #region Glass Block SpriteFactory
    class GlassBlockSpriteFactory : BlockSpriteFactory<GlassBlock>
    {
        private static BlockSpriteFactory<GlassBlock> _instance;
        private BlockSprite glassBlockSprite;

        public static BlockSpriteFactory<GlassBlock> Instance
        {
            get
            {
                if (_instance == null) _instance = new GlassBlockSpriteFactory();
                return _instance;
            }
        }
        private BlockSprite GlassBlock(ContentManager content, GraphicsDevice graphics, GlassBlock block)
        {
            glassBlockSprite = (BlockSprite)block.Sprite;
            Texture2D glassBlock = content.Load<Texture2D>("Blocks/GlassBlock");
            glassBlockSprite = new BlockSprite(graphics);
            glassBlockSprite.InitializeSprite(glassBlock, graphics, 1, 1);
            glassBlockSprite.SwitchAnimation(Constants.ZERO, 1);
            return glassBlockSprite;
        }
        private BlockSprite CrackedGlassBlock(ContentManager content, GraphicsDevice graphics, GlassBlock block)
        {
            glassBlockSprite = (BlockSprite)block.Sprite;
            //  bumpBrickBlockSprite.CurrentLocation = block.Location;
            Texture2D crackBlock = content.Load<Texture2D>("Blocks/CrackedBlock");
            glassBlockSprite.LoopFrame = false;
            glassBlockSprite.InitializeSprite(crackBlock, graphics, 1, 1);
            glassBlockSprite.SwitchAnimation(Constants.ZERO, 1);
            return glassBlockSprite;
        }
        public override BlockSprite FactoryMethod(ContentManager content, GlassBlock block, GraphicsDevice graphics, bool underworld)
        {
            if(block.BlockState is CrackedGlassBlockState)
            {
                return CrackedGlassBlock(content, graphics, block);
            }
            else if(block.BlockState is NewGlassBlockState)
            {
                return GlassBlock(content, graphics, block);
            }
            return GlassBlock(content, graphics, block);
        }
    }
    #endregion

    #region Stair Block Sprite Factory
    class StairBlockSpriteFactory : BlockSpriteFactory<StairBlock>
    {
        private static BlockSpriteFactory<StairBlock> _instance;
        private BlockSprite stairBlockSprite;

        public static BlockSpriteFactory<StairBlock> Instance
        {
            get
            {
                if (_instance == null) _instance = new StairBlockSpriteFactory();
                return _instance;
            }
        }
        private BlockSprite FloorBlock(ContentManager content, GraphicsDevice graphics)
        {
            Texture2D stairBlock = content.Load<Texture2D>("Blocks/StairBlockOverworld");
            stairBlockSprite = new BlockSprite(graphics);
            stairBlockSprite.InitializeSprite(stairBlock, graphics, 1, 1);
            return stairBlockSprite;
        }

        public override BlockSprite FactoryMethod(ContentManager content, StairBlock block, GraphicsDevice graphics, bool underworld)
        {
            return FloorBlock(content, graphics);
        }
    }
    #endregion

    #region Used Block Sprite Factory
    //Potential use in future sprints 
    class UsedBlockSpriteFactory : BlockSpriteFactory<UsedBlock>
    {
        private static BlockSpriteFactory<UsedBlock> _instance;
        private BlockSprite usedBlockSprite;

        public static BlockSpriteFactory<UsedBlock> Instance
        {
            get
            {
                if (_instance == null) _instance = new UsedBlockSpriteFactory();
                return _instance;
            }
        }
        private BlockSprite UsedBlock(ContentManager content, GraphicsDevice graphics)
        {
            Texture2D usedBlock = content.Load<Texture2D>("Blocks/UsedBlock");
            usedBlockSprite = new BlockSprite(graphics);
            usedBlockSprite.InitializeSprite(usedBlock, graphics, 1, 1);
            return usedBlockSprite;
        }

        public override BlockSprite FactoryMethod(ContentManager content, UsedBlock block, GraphicsDevice graphics, bool underworld)
        {
            return UsedBlock(content, graphics);
        }
    }
    #endregion

    #region Health Bar Sprite Factory
    class HealthBarSpriteFactory : EnemySpriteFactory<HealthBar>
    {
        private static EnemySpriteFactory<HealthBar> _instance;
        private EnemySprite zero;
        private EnemySprite one;
        private EnemySprite two;
        private EnemySprite three;
        private EnemySprite four;
        private EnemySprite five;
        private EnemySprite six;
        private EnemySprite seven;
        private EnemySprite eight;
        private EnemySprite nine;
        public static EnemySpriteFactory<HealthBar> Instance
        {
            get
            {
                if (_instance == null) _instance = new HealthBarSpriteFactory();
                return _instance;
            }
        }

        private EnemySprite HealthZero(HealthBar health)
        {
            zero = (EnemySprite)health.Sprite;
            zero.SwitchAnimation(9, 1);
            zero.LoopFrame = true;
            return zero;
        }
        private EnemySprite HealthOne(HealthBar health)
        {
            one = (EnemySprite)health.Sprite;
            one.SwitchAnimation(8, 1);
            one.LoopFrame = false;
            return one;
        }
        private EnemySprite HealthTwo(HealthBar health)
        {
            two = (EnemySprite)health.Sprite;
            two.SwitchAnimation(7, 1);
            two.LoopFrame = false;
            return two;
        }
        private EnemySprite HealthThree(HealthBar health)
        {
            three = (EnemySprite)health.Sprite;
            three.SwitchAnimation(6, 1);
            three.LoopFrame = false;
            return three;
        }
        private EnemySprite HealthFour(HealthBar health)
        {
            Debug.WriteLine("Four");
            four = (EnemySprite)health.Sprite;
            four.SwitchAnimation(5, 1);
            four.LoopFrame = false;
            return four;
        }
        private EnemySprite HealthFive(HealthBar health)
        {
            Debug.WriteLine("five");
            five = (EnemySprite)health.Sprite;
            five.SwitchAnimation(4, 1);
            five.LoopFrame = false;
            return five;
        }
        private EnemySprite HealthSix(HealthBar health)
        {
            Debug.WriteLine("six");
            six = (EnemySprite)health.Sprite;
            six.SwitchAnimation(3, 1);
            six.LoopFrame = false;
            return six;
        }
        private EnemySprite HealthSeven(HealthBar health)
        {
            Debug.WriteLine("seven");
            seven = (EnemySprite)health.Sprite;
            seven.SwitchAnimation(2, 1);
            seven.LoopFrame = false;
            return seven;
        }
        private EnemySprite HealthEight(HealthBar health)
        {
            Debug.WriteLine("eight");
            eight = (EnemySprite)health.Sprite;
            eight.SwitchAnimation(1, 1);
            eight.LoopFrame = false;
            return eight;
        }
        private EnemySprite HealthNine(HealthBar health)
        {
            nine = (EnemySprite)health.Sprite;
            nine.SwitchAnimation(Constants.ZERO, 1);
            nine.LoopFrame = true;
            return nine;
        }

        public override EnemySprite FactoryMethod(ContentManager Content, HealthBar health, GraphicsDevice graphics)
        {
            Debug.WriteLine("health.Life: " + health.Life);
            if(health.Life == 9)
            {
                return HealthNine(health);
            }
            else if(health.Life == 8)
            {
                 return HealthEight(health);
            }
            else if(health.Life == 7)
            {
                return HealthSeven(health);
            }
            else if(health.Life == 6)
            {
                return HealthSix(health);
            }
            else if(health.Life == 5)
            {
                return HealthFive(health);
            }
            else if(health.Life == 4)
            {
                return HealthFour(health);
            }
            else if(health.Life == 3)
            {
                return HealthThree(health);
            }
            else if(health.Life == 2)
            {
                return HealthTwo(health);
            }
            else if(health.Life == 1)
            {
                return HealthOne(health);
            }
            else if(health.Life ==Constants.ZERO)
            {
                return HealthZero(health);
            }
            else
            {
                return HealthOne(health);
            }
        }
    }
    #endregion

    #region Star Sprite Factory
    class StarSpriteFactory : ItemSpriteFactory<Star>
    {
        private static ItemSpriteFactory<Star> _instance;
        private ItemSprite starSprite;
        public static ItemSpriteFactory<Star> Instance
        {
            get
            {
                if (_instance == null) _instance = new StarSpriteFactory();
                return _instance;
            }
        }
        private ItemSprite Star(ContentManager content, GraphicsDevice graphics)
        {
            Texture2D star = content.Load<Texture2D>("ItemSpriteSheet");
            starSprite = new ItemSprite(graphics, star, 5, 10,Constants.ZERO, 4);
            return starSprite;
        }

        public override ItemSprite FactoryMethod(ContentManager content, GraphicsDevice graphics, Star star)
        {
            return Star(content, graphics);
        }
    }
    #endregion

    #region Super Mushroom Sprite Factory
    class SuperMushroomSpriteFactory : ItemSpriteFactory<SuperMushroom>
    {
        private static ItemSpriteFactory<SuperMushroom> _instance;
        private ItemSprite superMushroomSprite;
        public static ItemSpriteFactory<SuperMushroom> Instance
        {
            get
            {
                if (_instance == null) _instance = new SuperMushroomSpriteFactory();
                return _instance;
            }
        }
        private ItemSprite SuperMushroom(ContentManager content, GraphicsDevice graphics)
        {
            Texture2D superMushroom = content.Load<Texture2D>("ItemSpriteSheet");
            superMushroomSprite = new ItemSprite(graphics, superMushroom, 5, 10, 4, 1);
            return superMushroomSprite;
        }

        public override ItemSprite FactoryMethod(ContentManager content, GraphicsDevice graphics, SuperMushroom superMushroom)
        {
            return SuperMushroom(content, graphics);
        }
    }

    #endregion

    #region One Up Mushroom Sprite Factory
    class OneUpMushroomSpriteFactory : ItemSpriteFactory<OneUpMushroom>
    {
        private static ItemSpriteFactory<OneUpMushroom> _instance;
        private ItemSprite oneUpMushroomSprite;
        public static ItemSpriteFactory<OneUpMushroom> Instance
        {
            get
            {
                if (_instance == null) _instance = new OneUpMushroomSpriteFactory();
                return _instance;
            }
        }
        private ItemSprite OneUpMushroom(ContentManager content, GraphicsDevice graphics)
        {
            Texture2D oneUpMushroom = content.Load<Texture2D>("ItemSpriteSheet");
            oneUpMushroomSprite = new ItemSprite(graphics, oneUpMushroom, 5, 10, 3, 1);
            return oneUpMushroomSprite;
        }

        public override ItemSprite FactoryMethod(ContentManager content, GraphicsDevice graphics, OneUpMushroom oneUpMushroom)
        {
            return OneUpMushroom(content, graphics);
        }
    }
    #endregion

    #region Coin Sprite Factory
    class CoinSpriteFactory : ItemSpriteFactory<Coin>
    {
        private static ItemSpriteFactory<Coin> _instance;
        private ItemSprite coinSprite;
        public static ItemSpriteFactory<Coin> Instance
        {
            get
            {
                if (_instance == null) _instance = new CoinSpriteFactory();
                return _instance;
            }
        }
        private ItemSprite Coin(ContentManager content, GraphicsDevice graphics)
        {
            Texture2D coin = content.Load<Texture2D>("ItemSpriteSheet");
            coinSprite = new ItemSprite(graphics, coin, 5, 10, 2, 10);
            return coinSprite;
        }

        public override ItemSprite FactoryMethod(ContentManager content, GraphicsDevice graphics, Coin coin)
        {
            return Coin(content, graphics);
        }
    }
    #endregion

    #region Fire Flower Sprite Factory
    class FireFlowerSpriteFactory : ItemSpriteFactory<FireFlower>
    {
        private static ItemSpriteFactory<FireFlower> _instance;
        private ItemSprite fireFlowerSprite;
        public static ItemSpriteFactory<FireFlower> Instance
        {
            get
            {
                if (_instance == null) _instance = new FireFlowerSpriteFactory();
                return _instance;
            }
        }
        private ItemSprite FireFlower(ContentManager content, GraphicsDevice graphics)
        {
            Texture2D fireFlower = content.Load<Texture2D>("ItemSpriteSheet");
            fireFlowerSprite = new ItemSprite(graphics, fireFlower, 5, 10, 1, 4);
            return fireFlowerSprite;
        }

        public override ItemSprite FactoryMethod(ContentManager content, GraphicsDevice graphics, FireFlower fireFlower)
        {
            return FireFlower(content, graphics);
        }
    }
    #endregion

    #region Pipe Sprite Factory
    class PipeSpriteFactory : BlockSpriteFactory<IEntity>
    {
        private static BlockSpriteFactory<IEntity> _instance;
        private BlockSprite pipeSprite;
        public static BlockSpriteFactory<IEntity> Instance
        {
            get
            {
                if (_instance == null) _instance = new PipeSpriteFactory();
                return _instance;
            }
        }
        private BlockSprite Pipe(ContentManager content, GraphicsDevice graphics)
        {
            Texture2D pipe = content.Load<Texture2D>("pipe");
            pipeSprite = new BlockSprite(graphics);
            pipeSprite.InitializeSprite(pipe, graphics, 1, 1);
            return pipeSprite;
        }

        public override BlockSprite FactoryMethod(ContentManager content, IEntity pipe, GraphicsDevice graphics, bool underworld)
        {
            return Pipe(content, graphics);
        }
    }
    #endregion

    #region Bush Sprite Factory
    class BushSpriteFactory : ItemSpriteFactory<Bush>
    {
        private static ItemSpriteFactory<Bush> _instance;
        private ItemSprite bushSprite;
        public static ItemSpriteFactory<Bush> Instance
        {
            get
            {
                if (_instance == null) _instance = new BushSpriteFactory();
                return _instance;
            }
        }
        private ItemSprite Bush(ContentManager content, GraphicsDevice graphics)
        {
            Texture2D bush = content.Load<Texture2D>("bush");
            bushSprite = new ItemSprite(graphics, bush, 1, 1,Constants.ZERO, 1);
            return bushSprite;
        }

        public override ItemSprite FactoryMethod(ContentManager content, GraphicsDevice graphics, Bush bush)
        {
            return Bush(content, graphics);
        }
    }
    #endregion

    #region Flag Sprite Factory
    class FlagSpriteFactory : ItemSpriteFactory<Flag>
    {
        private static ItemSpriteFactory<Flag> _instance;
        private ItemSprite flagSprite;
        private ItemSprite flagDropSprite;
        public static ItemSpriteFactory<Flag> Instance
        {
            get
            {
                if (_instance == null) _instance = new FlagSpriteFactory();
                return _instance;
            }
        }
        private ItemSprite Flag(ContentManager content, GraphicsDevice graphics)
        {
            Texture2D flag = content.Load<Texture2D>("flag");
            flagSprite = new ItemSprite(graphics, flag, 1, 1,Constants.ZERO, 1);
            flagSprite.ScaleSprite(3f);
            return flagSprite;
        }

        private ItemSprite FlagDrop(ContentManager content, GraphicsDevice graphics)
        {
            Texture2D flag = content.Load<Texture2D>("FlagPoles");
            flagDropSprite = new ItemSprite(graphics, flag, 1, 5,Constants.ZERO, 5)
            {
                CurrentLocation = flagSprite.CurrentLocation,
                LoopFrame = false
            };
            flagDropSprite.ScaleSprite(3f);
            return flagDropSprite;
        }

        public override ItemSprite FactoryMethod(ContentManager content, GraphicsDevice graphics, Flag flag)
        {
            var state = flag.FlagState;

            if (state is FlagDropState)
            {
                return FlagDrop(content, graphics);
            }
            else if (state is FlagNewState)
            {
                return Flag(content, graphics);
            }
            return Flag(content, graphics);
        }
    }
    #endregion

    #region CheckpointFlag Sprite Factory
    class CheckpointFlagSpriteFactory : ItemSpriteFactory<CheckpointFlag>
    {
        private static ItemSpriteFactory<CheckpointFlag> _instance;
        private ItemSprite checkpointFlagSprite;
        private ItemSprite checkpointFlagDropSprite;
        public static ItemSpriteFactory<CheckpointFlag> Instance
        {
            get
            {
                if (_instance == null) _instance = new CheckpointFlagSpriteFactory();
                return _instance;
            }
        }
        private ItemSprite Flag(ContentManager content, GraphicsDevice graphics)
        {
            Texture2D flag = content.Load<Texture2D>("checkpointFlag");
            checkpointFlagSprite = new ItemSprite(graphics, flag, 1, 3,Constants.ZERO, 1);
            checkpointFlagSprite.ScaleSprite(3f);
            return checkpointFlagSprite;
        }

        private ItemSprite FlagDrop(ContentManager content, GraphicsDevice graphics, CheckpointFlag oldFlag)
        {
            Texture2D flag = content.Load<Texture2D>("checkpointFlag");
            checkpointFlagDropSprite = new ItemSprite(graphics, flag, 1, 3,Constants.ZERO, 3)
            {
                CurrentLocation = checkpointFlagSprite.CurrentLocation,
                LoopFrame = false
            };
            checkpointFlagDropSprite.ScaleSprite(3f);
            checkpointFlagDropSprite.CurrentLocation = oldFlag.Location;
            return checkpointFlagDropSprite;
        }

        public override ItemSprite FactoryMethod(ContentManager content, GraphicsDevice graphics, CheckpointFlag flag)
        {
            var state = flag.FlagState;

            if (state is CheckpointFlagDropState)
            {
                return FlagDrop(content, graphics, flag);
            }
            else if (state is CheckpointFlagNewState)
            {
                return Flag(content, graphics);
            }
            return Flag(content, graphics);
        }
    }
    #endregion

    #region AGrade Sprite Factory
    class AGradeSpriteFactory : GradeSpriteFactory<AGrade>
    {
        private static GradeSpriteFactory<AGrade> _instance;
        private ItemSprite aGradeSprite;
        public static GradeSpriteFactory<AGrade> Instance
        {
            get
            {
                if (_instance == null) _instance = new AGradeSpriteFactory();
                return _instance;
            }
        }
        private ItemSprite AGrade(ContentManager content, GraphicsDevice graphics)
        {
            Texture2D a = content.Load<Texture2D>("Grades/A");
            aGradeSprite = new ItemSprite(graphics, a, 1, 1,Constants.ZERO, 1);
            return aGradeSprite;
        }

        public override ItemSprite FactoryMethod(ContentManager content, GraphicsDevice graphics)
        {
            return AGrade(content, graphics);
        }
    }
    #endregion

    #region BGrade Sprite Factory
    class BGradeSpriteFactory : GradeSpriteFactory<BGrade>
    {
        private static GradeSpriteFactory<BGrade> _instance;
        private ItemSprite bGradeSprite;
        public static GradeSpriteFactory<BGrade> Instance
        {
            get
            {
                if (_instance == null) _instance = new BGradeSpriteFactory();
                return _instance;
            }
        }
        private ItemSprite BGrade(ContentManager content, GraphicsDevice graphics)
        {
            Texture2D b = content.Load<Texture2D>("Grades/B");
            bGradeSprite = new ItemSprite(graphics, b, 1, 1,Constants.ZERO, 1);
            return bGradeSprite;
        }

        public override ItemSprite FactoryMethod(ContentManager content, GraphicsDevice graphics)
        {
            return BGrade(content, graphics);
        }
    }
    #endregion

    #region CGrade Sprite Factory
    class CGradeSpriteFactory : GradeSpriteFactory<CGrade>
    {
        private static GradeSpriteFactory<CGrade> _instance;
        private ItemSprite cGradeSprite;
        public static GradeSpriteFactory<CGrade> Instance
        {
            get
            {
                if (_instance == null) _instance = new CGradeSpriteFactory();
                return _instance;
            }
        }
        private ItemSprite CGrade(ContentManager content, GraphicsDevice graphics)
        {
            Texture2D c = content.Load<Texture2D>("Grades/C");
            cGradeSprite = new ItemSprite(graphics, c, 1, 1,Constants.ZERO, 1);
            return cGradeSprite;
        }

        public override ItemSprite FactoryMethod(ContentManager content, GraphicsDevice graphics)
        {
            return CGrade(content, graphics);
        }
    }
    #endregion

    #region DGrade Sprite Factory
    class DGradeSpriteFactory : GradeSpriteFactory<DGrade>
    {
        private static GradeSpriteFactory<DGrade> _instance;
        private ItemSprite dGradeSprite;
        public static GradeSpriteFactory<DGrade> Instance
        {
            get
            {
                if (_instance == null) _instance = new DGradeSpriteFactory();
                return _instance;
            }
        }
        private ItemSprite DGrade(ContentManager content, GraphicsDevice graphics)
        {
            Texture2D d = content.Load<Texture2D>("Grades/D");
            dGradeSprite = new ItemSprite(graphics, d, 1, 1,Constants.ZERO, 1);
            return dGradeSprite;
        }

        public override ItemSprite FactoryMethod(ContentManager content, GraphicsDevice graphics)
        {
            return DGrade(content, graphics);
        }
    }
    #endregion

    #region FGrade Sprite Factory
    class FGradeSpriteFactory : GradeSpriteFactory<FGrade>
    {
        private static GradeSpriteFactory<FGrade> _instance;
        private ItemSprite fGradeSprite;
        public static GradeSpriteFactory<FGrade> Instance
        {
            get
            {
                if (_instance == null) _instance = new FGradeSpriteFactory();
                return _instance;
            }
        }
        private ItemSprite FGrade(ContentManager content, GraphicsDevice graphics)
        {
            Texture2D f = content.Load<Texture2D>("Grades/F");
            fGradeSprite = new ItemSprite(graphics, f, 1, 1,Constants.ZERO, 1);
            return fGradeSprite;
        }

        public override ItemSprite FactoryMethod(ContentManager content, GraphicsDevice graphics)
        {
            return FGrade(content, graphics);
        }
    }
    #endregion

    #region Mario Sprite Factory
    class MarioSpriteFactory : EnemySpriteFactory<Mario>
    {
        private static EnemySpriteFactory<Mario> _instance;
        private EnemySprite marioSprite;
        public static EnemySpriteFactory<Mario> Instance
        {
            get
            {
                if (_instance == null) _instance = new MarioSpriteFactory();
                return _instance;
            }
        }
        private EnemySprite Mario(ContentManager content, GraphicsDevice graphics)
        {
            Texture2D mario = content.Load<Texture2D>("Mario/idle mario");
            marioSprite = new EnemySprite(graphics,mario,1,1, new Vector2(300, 711),Constants.ZERO,1);
            return marioSprite;
        }
        

        public override EnemySprite FactoryMethod(ContentManager content, Mario enemy, GraphicsDevice graphics)
        {
            return Mario(content, graphics);
        }

    }
    #endregion
    
    }


