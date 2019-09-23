using FirstMonoGame.Collision;
using FirstMonoGame.Entity;
using FirstMonoGame.Interfaces;
using FirstMonoGame.States.GameState;
using FirstMonoGame.WorldScrolling;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Xml;

namespace FirstMonoGame.Levels
{
    class LevelLoader
    {
        public Collection<IEntity> Entities { get; set; }
        String tag = "";
        String type = "";
        String xPosition = "";
        String yPosition = "";
        String visible = "";
        String xVelocity = "";
        String yVelocity = "";
        String checkable = "";
        String width = "";
        String height = "";
        String exitPipe = "";
        String underworld = "";
        String minigame = "";
        String vertical = "";
        public int levelWidth { get; set; }
        public int levelHeight { get; set; }
        bool visBool = false;
        bool check = true;
        bool ExitPipe = false;
        bool visibleBounding = false;
        bool Underworld = false;
        public int MiniGame =Constants.ZERO;
        bool verticalScrollingMode = false;
        bool isGlass = false;
        IEntity entity = null;
        Peach peach = null;
        Bowser bowser = null;

        public Level LoadLevel(String file, ContentManager content, GraphicsDevice graphics, CollisionDetection collisions, BackgroundControl backgroundControl)
        {
            XmlDocument reader = new XmlDocument();
            reader.Load(file);
 
            Entities = new Collection<IEntity>();

            Camera camera = Camera.Instance;

            XmlNode root = reader.DocumentElement;
            if (root.HasChildNodes)
            {
                for (int i =Constants.ZERO; i < root.ChildNodes.Count; i++)
                {
                    entity = null;
                    XmlNode x = root.ChildNodes[i];

                    GetAttributes(x);
                    if (tag.Equals("avatar"))
                    {
                        if (type.Equals("peach"))
                        {
                            peach = new Peach(content, graphics)
                            {
                                Location = new Vector2(float.Parse(xPosition), float.Parse(yPosition)),
                                Visible = visBool,
                                VisBounding = visibleBounding
                            };
                            peach.Sprite.Velocity = new Vector2(float.Parse(xVelocity), float.Parse(yVelocity));
                            peach.Sprite.Velocity += new Vector2(Constants.ZERO, float.Epsilon); // force frame 1 collision participation
                            Entities.Add(peach);
                            collisions.Register(peach);
                            camera.Peach = peach;
                            backgroundControl.Peach = peach;
                        }
                    }
                    else if (tag.Equals("block"))
                    {
                        entity = AddBlock(x, content, graphics, collisions);
                    }
                    else if (tag.Equals("enemy"))
                    {
                        entity = AddEnemy(content, graphics, peach);
                        if(entity is Mario)
                        {
                            Debug.WriteLine("adding mario to collisions");
                        }
;                    }
                    else if (tag.Equals("item"))
                    {
                        entity = AddItem(content, graphics);
                    }
                    else if (tag.Equals("levelProperties"))
                    {
                        levelWidth = int.Parse(width);
                        levelHeight = int.Parse(height);
                        if (!verticalScrollingMode)
                        {
                            camera.SetLevelBounds(Constants.ZERO, levelWidth,Constants.ZERO, 1080);
                        }
                        else
                        {
                            camera.SetLevelBounds(Constants.ZERO, 1920,Constants.ZERO, levelHeight);
                        }
                    }
                    SetProperties(collisions);
                }
            }
            return new Level(Entities);
        }

        public IEntity AddItem(ContentManager content, GraphicsDevice graphics)
        {
            switch (type)
            {
                case ("star"):
                    entity = new Star(content, graphics);
                    entity.Sprite.ScaleSprite(2.5f);
                    break;
                case ("fireflower"):
                    entity = new FireFlower(content, graphics);
                    entity.Sprite.ScaleSprite(2.5f);
                    break;
                case ("oneupmushroom"):
                    entity = new OneUpMushroom(content, graphics);
                    entity.Sprite.ScaleSprite(2.5f);
                    break;
                case ("coin"):
                    entity = new Coin(content, graphics);
                    entity.Sprite.ScaleSprite(2.5f);
                    break;
                case ("supermushroom"):
                    entity = new SuperMushroom(content, graphics);
                    entity.Sprite.ScaleSprite(2.5f);
                    break;
                case ("bush"):
                    entity = new Bush(content, graphics);
                    entity.Sprite.ScaleSprite(3f);
                    break;
                case ("flag"):
                    entity = new Flag(content, graphics);
                    entity.Sprite.ScaleSprite(3f);
                    break;
                case ("checkpointflag"):
                    entity = new CheckpointFlag(content, graphics);
                    break;
                default:
                    break;
            }
            return entity;
        }

        public IEntity AddBlock(XmlNode x, ContentManager content, GraphicsDevice graphics, CollisionDetection collisions)
        {
            bool isUnderworld = false;
            switch (type)
            {
                case ("questionblock"):
                    QuestionBlock questionBlock = new QuestionBlock(content, graphics);
                    if (x.HasChildNodes)
                    {
                        for (int m =Constants.ZERO; m < x.ChildNodes.Count; m++)
                        {
                            IEntity childEntity = null;
                            XmlNode y = x.ChildNodes[m];
                            GetAttributes(y);
                            if (tag.Equals("item")) childEntity = AddItem(content, graphics);
                            else if (tag.Equals("block")) childEntity = AddBlock(y, content, graphics, collisions);
                            entity = childEntity;
                            SetProperties(collisions);
                            questionBlock.AddContainedItem(childEntity);
                        }
                    }
                    GetAttributes(x);
                    entity = questionBlock;
                    break;
                case ("floorblock"):
                    isUnderworld = false;
                    entity = new FloorBlock(content, graphics, isUnderworld);
                    break;
                case ("underworldFloor"):
                    isUnderworld = true;
                    entity = new FloorBlock(content, graphics, isUnderworld);
                    break;
                case ("glassblock"):
                    GlassBlock glassBlock = new GlassBlock(content, graphics);
                    isGlass = true;
                    if (x.HasChildNodes)
                    {
                        for (int m =Constants.ZERO; m < x.ChildNodes.Count; m++)
                        {
                            IEntity childEntity = null;
                            XmlNode y = x.ChildNodes[m];
                            GetAttributes(y);
                            if (tag.Equals("block")) childEntity = AddBlock(y, content, graphics, collisions);
                            entity = childEntity;
                            SetProperties(collisions);
                            glassBlock.AddContainedItem(childEntity);
                        }
                    }
                    GetAttributes(x);
                    entity = glassBlock;
                    break;
                case ("brickblock"):
                    isUnderworld = false;
                    BrickBlock brickBlock = new BrickBlock(content, graphics, isUnderworld);
                    if (x.HasChildNodes)
                    {
                        for (int m =Constants.ZERO; m < x.ChildNodes.Count; m++)
                        {
                            IEntity childEntity = null;
                            XmlNode y = x.ChildNodes[m];
                            GetAttributes(y);
                            if (tag.Equals("item")) childEntity = AddItem(content, graphics);
                            else if (tag.Equals("block")) childEntity = AddBlock(y, content, graphics, collisions);
                            entity = childEntity;
                            SetProperties(collisions);
                            brickBlock.AddContainedItem(childEntity);
                        }
                    }
                    GetAttributes(x);
                    entity = brickBlock;
                    break;
                case ("underworldBrick"):
                    Underworld = true;
                    BrickBlock underworldBrickBlock = new BrickBlock(content, graphics, Underworld);
                    if (x.HasChildNodes)
                    {
                        for (int m =Constants.ZERO; m < x.ChildNodes.Count; m++)
                        {
                            IEntity childEntity = null;
                            XmlNode y = x.ChildNodes[m];
                            GetAttributes(y);
                            if (tag.Equals("item")) childEntity = AddItem(content, graphics);
                            else if (tag.Equals("block")) childEntity = AddBlock(y, content, graphics, collisions);
                            entity = childEntity;
                            SetProperties(collisions);
                            underworldBrickBlock.AddContainedItem(childEntity);
                        }
                    }
                    GetAttributes(x);
                    entity = underworldBrickBlock;
                    break;
                case ("warpPipe"):
                    WarpPipe warpPipe = new WarpPipe(content, graphics, ExitPipe, Underworld, MiniGame);
                    if (x.HasChildNodes)
                    {
                        for (int m =Constants.ZERO; m < x.ChildNodes.Count; m++)
                        {
                            IEntity childEntity = null;
                            XmlNode y = x.ChildNodes[m];
                            GetAttributes(y);
                            if (tag.Equals("enemy")) childEntity = AddEnemy(content, graphics, peach);
                            else if (tag.Equals("block"))
                            {
                                childEntity = AddBlock(y, content, graphics, collisions);
                            }
                            entity = childEntity;
                            SetProperties(collisions);
                            warpPipe.AddContainedItem(childEntity);
                        }
                    }
                    GetAttributes(x);
                    entity = warpPipe;
                    break;
                case ("stairblock"):
                    entity = new StairBlock(content, graphics);
                    break;
                case ("usedblock"):
                    entity = new UsedBlock(content, graphics);
                    break;
                case ("hiddenblock"):
                    isUnderworld = false;
                    HiddenBlock hiddenBlock = new HiddenBlock(content, graphics);
                    if (x.HasChildNodes)
                    {
                        for (int m =Constants.ZERO; m < x.ChildNodes.Count; m++)
                        {
                            IEntity childEntity = null;
                            XmlNode y = x.ChildNodes[m];
                            GetAttributes(y);
                            if (tag.Equals("item")) childEntity = AddItem(content, graphics);
                            else if (tag.Equals("block")) childEntity = AddBlock(y, content, graphics, collisions);
                            entity = childEntity;
                            SetProperties(collisions);
                            hiddenBlock.AddContainedItem(childEntity);
                        }
                    }
                    GetAttributes(x);
                    entity = hiddenBlock;
                    break;
                case ("pipe"):
                    entity = new Pipe(content, graphics);
                    break;
                case ("explodingBlock"):
                    entity = new ExplodingBlock(content, graphics, isGlass);
                    break;
                case ("exitPipe"):
                    entity = new WarpPipe(content, graphics, ExitPipe, Underworld, MiniGame);
                    break;
                default:
                    break;
            }
            return entity;
        }

        public IEntity AddEnemy(ContentManager content, GraphicsDevice graphics, Peach Peach)
        {
            switch (type)
            {
                case ("goomba"):
                    entity = new Goomba(content, graphics);
                    break;
                case ("redkoopatroopa"):
                    entity = new KoopaTroopa(content, graphics, true);
                    break;
                case ("greenkoopatroopa"):
                    entity = new KoopaTroopa(content, graphics, false);
                    break;
                case ("piranha"):
                    entity = new Piranha(content, graphics, Peach);
                    break;
                case ("bowser"):
                    entity = new Bowser(content, graphics);
                    bowser = (Bowser)entity;
                    break;
                case ("mario"):
                    entity = new Mario(content, graphics);
                    break;
                case ("healthBar"):
                    entity = new HealthBar(content, graphics, bowser);
                    break;
                default:
                    break;

            }
            return entity;
        }
        public void GetAttributes(XmlNode x)
        {
            visBool = false;
            check = true;
            checkable = "";
            XmlAttributeCollection attr = x.Attributes;

            tag = x.Name;

            if (attr != null)
            {
                for (int j =Constants.ZERO; j < attr.Count; j++)
                {
                    switch (attr[j].Name.ToString())
                    {
                        case ("type"):
                            type = attr[j].Value;
                            break;
                        case ("x"):
                            xPosition = attr[j].Value;
                            break;
                        case ("y"):
                            yPosition = attr[j].Value;
                            break;
                        case ("visible"):
                            visible = attr[j].Value;
                            break;
                        case ("xVelocity"):
                            xVelocity = attr[j].Value;
                            break;
                        case ("yVelocity"):
                            yVelocity = attr[j].Value;
                            break;
                        case ("checkable"):
                            checkable = attr[j].Value;
                            break;
                        case ("width"):
                            width = attr[j].Value;
                            break;
                        case ("height"):
                            height = attr[j].Value;
                            break;
                        case ("exitPipe"):
                            exitPipe = attr[j].Value;
                            break;
                        case ("underworld"):
                            underworld = attr[j].Value;
                            break;
                        case ("minigame"):
                            minigame = attr[j].Value;
                            break;
                        case ("vertical"):
                            vertical = attr[j].Value;
                            break;
                    }
                }
            }

            if (visible != null)
            {
                if (visible.Equals("true")) visBool = true;
            }

            if (vertical != null)
            {
                if (vertical.Equals("true")) verticalScrollingMode = true;
            }

            if (checkable != null)
            {
                if (checkable.Equals("false")) check = false;
            }

            if (exitPipe.Equals("true")) {
                ExitPipe = true;
            }
            else
            {
                ExitPipe = false;
            }
            if(underworld != null)
            {
                if (underworld.Equals("true")) Underworld = true;
                else if (underworld.Equals("false")) Underworld = false;
            }
            if (minigame != null)
            {
                if (minigame.Equals("1")) MiniGame = 1;
                else if (minigame.Equals("2")) MiniGame = 2;
            }

        }

        public void SetProperties(CollisionDetection collisions)
        {
            if (entity != null)
            {
                entity.Location = new Vector2(float.Parse(xPosition), float.Parse(yPosition));
                entity.Visible = visBool;
                entity.VisBounding = visibleBounding;
                entity.Checkable = check;
                entity.OriginalLocation = entity.Location;
                if (!string.IsNullOrEmpty(xVelocity) && !string.IsNullOrEmpty(yVelocity)) entity.Sprite.Velocity = new Vector2(float.Parse(xVelocity), float.Parse(yVelocity));
                Entities.Add(entity);
                if (!(entity is Block) || entity.Checkable) collisions.Register(entity);
            }
        }

    }
}
