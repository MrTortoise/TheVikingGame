using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
using CommonObjects;
using CommonObjects.Graphics;
using CommonObjects.EventManagement;
using VikingGameObjects;
using Custom.Interfaces;
using CommonObjects.VectorDrawing;




namespace TheVikingGame
{
	/// <summary>
	/// This is the main type for your game
	/// </summary>
	public class Game1 : Microsoft.Xna.Framework.Game
	{
		GraphicsDeviceManager graphics;
		SpriteBatch spriteBatch;

		GraphicDeviceSingleton gd;
		EventManager em;

		Board mBoard;
		GeneralTexture mGeneralTexture;
		GeneralTextureList mGeneralTextureList;
		GeneralTextureCellList mTextureCellList;

		

		public Game1()
		{
			graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";
			//gd = GraphicDeviceSingleton.GetInstance(GraphicsDevice);
			em = EventManager.GetInstance();
			mBoard = new Board("theBoard");
			mGeneralTextureList = new GeneralTextureList();
			mTextureCellList = new GeneralTextureCellList();

			graphics.PreferredBackBufferWidth = 900;
			graphics.PreferredBackBufferHeight = 900;

			this.IsMouseVisible = true;
			
			
		}

		/// <summary>
		/// Allows the game to perform any initialization it needs to before starting to run.
		/// This is where it can query for any required services and load any non-graphic
		/// related content.  Calling base.Initialize will enumerate through any components
		/// and initialize them as well.
		/// </summary>
		protected override void Initialize()
		{
			base.Initialize(); 			
		}

		/// <summary>
		/// LoadContent will be called once per game and is the place to load
		/// all of your content.
		/// </summary>
		protected override void LoadContent()
		{
			// Create a new SpriteBatch, which can be used to draw textures.
			spriteBatch = new SpriteBatch(GraphicsDevice);

			base.LoadContent();

			GraphicDeviceSingleton.GetInstance(GraphicsDevice);

			//ToDo: Move all this board loading logic into the Board?

			// Set up the Square Types
			SquareType st = new SquareType(0, "Normal", true, false);
			mBoard.AddSquareType(st);
			st = new SquareType(1, "King", false, false);
			mBoard.AddSquareType(st);
			st = new SquareType(2, "Exit", false, true);
			mBoard.AddSquareType(st);


			VectorDraw.Load("Content/pixel.png");

			//Set up the Textures
			Vector2 texturesize = new Vector2(GraphicsDevice.Viewport.Width   / (float)11, GraphicsDevice.Viewport.Height  / (float)11);
			mBoard.SquareDims = texturesize;

			GeneralTexture gt = new GeneralTexture(0, "Empty", "Content/empty.PNG", 1, 1);
			mGeneralTextureList.Add(gt);

			gt = new GeneralTexture(1, "NormalPiece", "Content/celtic_knot.png", 1, 1);
			mGeneralTextureList.Add(gt);

			gt = new GeneralTexture(2, "KingPlace", "Content/celtic_cross_small.png", 1, 1);
			mGeneralTextureList.Add(gt);

			gt = new GeneralTexture(3, "blackPiece", "Content/blackPiece.png", 1, 1);
			mGeneralTextureList.Add(gt);
			gt = new GeneralTexture(4, "whitePiece", "Content/whitePiece.png", 1, 1);
			mGeneralTextureList.Add(gt);
			gt = new GeneralTexture(5, "KingPiece", "Content/whiteking.png", 1, 1);
			mGeneralTextureList.Add(gt);

			mGeneralTextureList.Load(GraphicsDevice);  			

			GeneralTextureCell gtc =
				new GeneralTextureCell(0,
					mGeneralTextureList[0],
					Vector2.Zero,
					texturesize,
					new Vector2(mGeneralTextureList[0].ColumnWidth, mGeneralTextureList[0].RowHeight),
					0, 1);

			mTextureCellList.Add(gtc);
			gtc = new GeneralTextureCell(1,
				mGeneralTextureList[1],
				Vector2.Zero,
				texturesize,
				new Vector2(mGeneralTextureList[1].ColumnWidth, mGeneralTextureList[1].RowHeight),
				0, 1);

			mTextureCellList.Add(gtc);
			gtc = new GeneralTextureCell(2,
				mGeneralTextureList[2],
				Vector2.Zero,
				texturesize,
				new Vector2(mGeneralTextureList[2].ColumnWidth, mGeneralTextureList[2].RowHeight),
				0, 1);
			mTextureCellList.Add(gtc);

			mTextureCellList.Add(gtc);
			gtc = new GeneralTextureCell(3,
				mGeneralTextureList[3],
				Vector2.Zero,
				texturesize,
				new Vector2(mGeneralTextureList[3].ColumnWidth, mGeneralTextureList[3].RowHeight),
				0, 1);
			mTextureCellList.Add(gtc);

			mTextureCellList.Add(gtc);
			gtc = new GeneralTextureCell(4,
				mGeneralTextureList[4],
				Vector2.Zero,
				texturesize,
				new Vector2(mGeneralTextureList[4].ColumnWidth, mGeneralTextureList[4].RowHeight),
				0, 1);
			mTextureCellList.Add(gtc);
			gtc = new GeneralTextureCell(5,
				mGeneralTextureList[5],
				Vector2.Zero,
				texturesize,
				new Vector2(mGeneralTextureList[5].ColumnWidth, mGeneralTextureList[5].RowHeight),
				0, 1);
			mTextureCellList.Add(gtc);

			Square s = new Square(0, "Empty", mTextureCellList[0], (float)(0.5),mBoard.GetSquareType(0));
			mBoard.AddSquare(s);

			s = new Square(1, "NormalPiece", mTextureCellList[1], (float)(0.5),mBoard.GetSquareType(0));
			mBoard.AddSquare(s);

			s = new Square(2, "KingPiece", mTextureCellList[2], (float)(0.5),mBoard.GetSquareType(1));
			mBoard.AddSquare(s);

			s = new Square(3, "ExitPiece", mTextureCellList[2], (float)(0.5),mBoard.GetSquareType(2));
			mBoard.AddSquare(s);

			mBoard.SetSquare(0, 0, 3);
			mBoard.SetSquare(0, 10, 3);
			mBoard.SetSquare(10, 0, 3);
			mBoard.SetSquare(10, 10, 3);


			mBoard.SetSquare(5, 5, 2);

			//black
			
			mBoard.SetSquare(0, 3, 1);
			mBoard.SetSquare(0, 4, 1);
			mBoard.SetSquare(0, 5, 1);
			mBoard.SetSquare(0, 6, 1);
			mBoard.SetSquare(0, 7, 1);

			mBoard.SetSquare(10, 3, 1);
			mBoard.SetSquare(10, 4, 1);
			mBoard.SetSquare(10, 5, 1);
			mBoard.SetSquare(10, 6, 1);
			mBoard.SetSquare(10, 7, 1);


			mBoard.SetSquare(3, 0, 1);
			mBoard.SetSquare(4, 0, 1);
			mBoard.SetSquare(5, 0, 1);
			mBoard.SetSquare(6, 0, 1);
			mBoard.SetSquare(7, 0, 1);

			mBoard.SetSquare(3, 10, 1);
			mBoard.SetSquare(4, 10, 1);
			mBoard.SetSquare(5, 10, 1);
			mBoard.SetSquare(6, 10, 1);
			mBoard.SetSquare(7, 10, 1);

			mBoard.SetSquare(5, 1, 1);
			mBoard.SetSquare(1, 5, 1);
			mBoard.SetSquare(9, 5, 1);
			mBoard.SetSquare(5, 9, 1);
			
			//white
			mBoard.SetSquare(4, 5, 1);
			mBoard.SetSquare(6, 5, 1);
			mBoard.SetSquare(5, 4, 1);
			mBoard.SetSquare(5, 6, 1);

			mBoard.SetSquare(4, 4, 1);
			mBoard.SetSquare(4, 6, 1);
			mBoard.SetSquare(6, 4, 1);
			mBoard.SetSquare(6, 6, 1);
			mBoard.SetSquare(3, 5, 1);
			mBoard.SetSquare(5, 3, 1);
			mBoard.SetSquare(7, 5, 1);
			mBoard.SetSquare(5, 7, 1);

			int[] pos = new int[2];

			AddKingPiece(5,5);

			AddWhitePiece(4, 5);
			AddWhitePiece(6, 5);
			AddWhitePiece(5, 4);
			AddWhitePiece(5, 6);

			AddWhitePiece(4, 4);
			AddWhitePiece(4, 6);
			AddWhitePiece(6, 4);
			AddWhitePiece(6, 6);

			AddWhitePiece(3, 5);
			AddWhitePiece(5, 3);
			AddWhitePiece(7, 5);
			AddWhitePiece(5, 7);

			AddBlackPiece(3, 0);
			AddBlackPiece(4, 0);
			AddBlackPiece(5, 0);
			AddBlackPiece(6, 0);
			AddBlackPiece(7, 0);

			AddBlackPiece(0, 3);
			AddBlackPiece(0, 4);
			AddBlackPiece(0, 5);
			AddBlackPiece(0, 6);
			AddBlackPiece(0, 7);

			AddBlackPiece(10, 3);
			AddBlackPiece(10, 4);
			AddBlackPiece(10, 5);
			AddBlackPiece(10, 6);
			AddBlackPiece(10, 7);

			AddBlackPiece(3, 10);
			AddBlackPiece(4, 10);
			AddBlackPiece(5, 10);
			AddBlackPiece(6, 10);
			AddBlackPiece(7, 10);

			AddBlackPiece(1, 5);
			AddBlackPiece(5, 1);
			AddBlackPiece(5, 9);
			AddBlackPiece(9, 5);




		}

		protected void AddWhitePiece(int x, int y)
		{
			int[] pos = new int[2];

			pos[0] = x;
			pos[1] = y;
			Piece p = new Piece(pos, eSide.White  , mTextureCellList[4]);
			mBoard.AddNewPiece(p);

		}

		protected void AddKingPiece(int x, int y)
		{
			int[] pos = new int[2];

			pos[0] = x;
			pos[1] = y;
			King  p = new King(pos, eSide.White, mTextureCellList[5]);
			mBoard.AddNewKing(p);

		}

		protected void AddBlackPiece(int x, int y)
		{
			int[] pos = new int[2];

			pos[0] = x;
			pos[1] = y;
			Piece p = new Piece(pos, eSide.Black, mTextureCellList[3]);
			mBoard.AddNewPiece(p);

		}

		/// <summary>
		/// UnloadContent will be called once per game and is the place to unload
		/// all content.
		/// </summary>
		protected override void UnloadContent()
		{
			// TODO: Unload any non ContentManager content here
		}

		/// <summary>
		/// Allows the game to run logic such as updating the world,
		/// checking for collisions, gathering input, and playing audio.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Update(GameTime gameTime)
		{
			// Allows the game to exit
			if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
				this.Exit();			

			if (mBoard.IsGameWon > 0)
			{
				this.Exit();
			}


			em.ProcessInput();

			base.Update(gameTime);
		}

		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.White );

			spriteBatch.Begin(SpriteBlendMode.AlphaBlend,SpriteSortMode.BackToFront,SaveStateMode.None );
			// TODO: Add your drawing code here
			DrawingArgs da = new DrawingArgs(spriteBatch, new Camera(GraphicsDevice));
			mBoard.Draw(da);

			
			spriteBatch.End();

			base.Draw(gameTime);
		}
	}
}
