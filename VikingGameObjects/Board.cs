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
using CommonObjects.EventManagement;
using CommonObjects;
using CommonObjects.Graphics;
using CommonObjects.VectorDrawing;

namespace VikingGameObjects
{
	public class Board: IGameDrawable
	{

		private Piece[,] mPieces;
		private int[,] mSquaresLayout; 
		private Vector2  mHighlightSource;
		private Dictionary<int, Square> mSquares;
		private Dictionary<int, SquareType> mSquareTypes;
		private Vector2  mSquareDims;
		private int[] mSelectedPiece;
		private eSide mSideToPlay = eSide.White;

		public event EventHandler GameOver;  
		public event EventHandler TurnOver;	
		public event EventHandler<MovePieceEventArgs> MoveRequestAccepted;
		public event EventHandler<MovePieceEventArgs> AttemptMovePiece;

		public int IsGameWon;

		public Board(string theName)
		{
			mID = GameObjectIDGenerator.GetNextID();
			mName = theName;

			EventManager em = EventManager.GetInstance();

			em.MouseButtonPressed += OnMouseButtonPressed;
			em.MouseButtonReleased += OnMouseButtonReleased;

			Initialise();			   			
		}

		protected virtual void Initialise()
		{
			mPieces	= new Piece[11,	11];
			mSquaresLayout = new int[11,11];
			mSquares = new Dictionary<int, Square>();
			mHighlightSource=Vector2.Zero;
			mSquareTypes = new Dictionary<int, SquareType>();
			
		}	   

		public Vector2 SquareDims
		{
			get { return mSquareDims; }
			set { mSquareDims = value; }
		}
		public eSide SideToPlay
		{
			get
			{
				return mSideToPlay;
			}
		}	
		public Piece  GetPieceAt(int x, int y)
		{
			return mPieces[x, y];
		}	
		public void AddNewPiece( Piece thePiece)
		{
			Piece p = thePiece;
			if (mPieces[p.X , p.Y ] == null)
			{
				mPieces[p.X , p.Y ] = p;
				p.Taken += new EventHandler(p_Taken);
			}
			else
			{ throw new ArgumentException("Tried to Add a piece to a location that already has a piece."); }


		}
		public void AddNewKing(King  theKing)
		{
			King  k = theKing ;
			if (mPieces[k.X, k.Y] == null)
			{
				k.Taken += new EventHandler(p_Taken);
				k.GameWon += new EventHandler(k_GameWon);
				k.GameLost += new EventHandler(k_GameLost);
				mPieces[k.X, k.Y] = k;
				
			}
			else
			{ throw new ArgumentException("Tried to Add a piece to a location that already has a piece."); }


		}

		void k_GameLost(object sender, EventArgs e)
		{
			IsGameWon = 2;
		}

		void k_GameWon(object sender, EventArgs e)
		{
			IsGameWon = 1;
		}

		void p_Taken(object sender, EventArgs e)
		{
			// Implement Victory Conditions
			


			Piece p = (Piece)sender;
			mPieces[p.Position[0], p.Position[1]] = null;
		}

		public bool IsPathClear(Vector2 start, Vector2 end)
		{
			throw new System.NotImplementedException();
		}

		public bool IsEnemy(int[] thePosition, eSide theSide)
		{
			bool retVal = false;
			Piece p = mPieces[thePosition[0], thePosition[1]];

			if (p != null)
			{
				if (p.Side != theSide)
				{
					retVal = true;
				}
			}
			else
			{
				if (GetSquareTypeAt(thePosition).Enemy == true)
				{ retVal = true; }
			}
			return retVal;
		}

		public void SetSquare(int x, int y, int squareID)
		{
			if (squareID>mSquares.Count-1)
			{ throw new ArgumentOutOfRangeException("SquareID refers to an index that does not exist in collection of squares."); }

			mSquaresLayout[x, y] = squareID;

		}

		public void AddSquare(Square theSquare)
		{
			mSquares.Add(theSquare.ID, theSquare);
		}

		public void AddSquareType(SquareType theSquareType)
		{
			mSquareTypes.Add(theSquareType.ID, theSquareType);
		}

		public SquareType GetSquareType(int SquareTypeID)
		{
			SquareType retVal = mSquareTypes[SquareTypeID];
			return retVal;
		}

		public SquareType GetSquareTypeAt(int[] thePosition)
		{
			int sqID = mSquaresLayout[thePosition[0], thePosition[1]];
			Square sq = mSquares[sqID];
			return sq.squareType;
		}

		protected int[] CalculateSquareIndex(Vector2 thePosition)
		{
			int	x =	(int)(thePosition.X	/ mSquareDims.X);
			int	y =	(int)(thePosition.Y	/ mSquareDims.Y);  
			int[] a	= {	x, y };

			return a;
		}  


		protected void SetBoardSize(int x, int y)
		{
			//ToDo: Implement SetBoardSize 

		}

		protected void SwitchSideToPlay()
		{
			//This will switch an observer to Black (It should never be set ot observer)
			if (mSideToPlay == eSide.Black)
			{ mSideToPlay = eSide.White; }
			else
			{ mSideToPlay = eSide.Black; }			
		}

		#region EventHandlers 


		protected void OnMouseButtonReleased(object sender, InputEventArgs theArgs)
		{
			int[] pos = CalculateSquareIndex(theArgs.CurrentMousePosition);
			if (pos[0] < 0)
				pos[0] = 0;
			if (pos[1] < 0)
				pos[1] = 0;

			int maxWidth = mSquaresLayout.GetLength(0);
			int maxHeight = mSquaresLayout.GetLength(1);

			if (pos[0] > maxWidth)
				pos[0] = maxWidth;
			if (pos[1] > maxHeight)
				pos[1] = maxHeight;

			RaiseEvent(AttemptMovePiece, new MovePieceEventArgs(pos, this));

			mSelectedPiece = null;
		}

		protected void OnMouseButtonPressed(object sender, InputEventArgs   theArgs)
		{
			
			int[] pos = CalculateSquareIndex(theArgs.CurrentMousePosition);
			Vector2 actPos = new Vector2(pos[0], pos[1]);
			Piece p = mPieces[pos[0], pos[1]];

			if (p != null)
			{
				if (p.Side == mSideToPlay)
				{
					p.PieceSelected += new EventHandler(p_PieceSelected);
					//if piece not selected then remoce the Piece Selected Ecent Handler
					if (!p.SelectPiece(this, mSideToPlay))
					{ p.PieceSelected -= p_PieceSelected; }
				}
			}
		}

		void p_PieceSelected(object sender, EventArgs e)
		{
			Piece  p = (Piece)sender;
			mSelectedPiece = p.Position;
			p.PieceDeSelected += new EventHandler(Board_PieceDeSelected);
			p.MoveRequest += new EventHandler<MovePieceEventArgs>(p_MoveRequest);			
			p.Moved+=new EventHandler<MovePieceEventArgs>(p_Moved);			
		}  

		void p_Moved(object sender, MovePieceEventArgs  e)
		{
			// Process Victory Conditions
			//ToDo: Implement Victory Condition Checking
			Piece p = (Piece)sender;
			mPieces[e.TargetPosition[0], e.TargetPosition[1]] = p;
			mPieces[e.SourcePosition[0], e.SourcePosition[1]] = null;
			
			// ToDo
			SwitchSideToPlay();
			
		}

		void p_MoveRequest(object sender, MovePieceEventArgs e)
		{ 
			RaiseEvent(MoveRequestAccepted, e);
		}

		/// <summary>
		/// Unsigns the board from all piece events that are exclusivley associated with the piece whilst it is selected
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void Board_PieceDeSelected(object sender, EventArgs e)
		{
			Piece p = (Piece)sender;
			mSelectedPiece = null;					   			
			p.PieceSelected -= p_PieceSelected;
			p.MoveRequest -= p_MoveRequest;
			p.Moved -= p_Moved;
			p.PieceDeSelected -= Board_PieceDeSelected;
		}

		protected void RaiseEvent(EventHandler<MovePieceEventArgs> theEvent, MovePieceEventArgs  theArgs)
		{
			EventHandler<MovePieceEventArgs> temp = theEvent;
			if (temp != null)
			{
				temp(this, theArgs);
			}

		}

		#endregion
		#region	IGameDrawable Members

		protected bool mIsVisible = true;
		public bool	IsVisible
		{
			get { return mIsVisible; }
		}

		public void	SetVisibility(bool theVisibility)
		{
			mIsVisible = theVisibility;
		}

		public void Draw(DrawingArgs theArgs)
		{

			DrawSquares(theArgs);
			DrawGrid(theArgs);
			DrawPieces(theArgs);

			//ToDo: draw all highlights


		}
		protected virtual void DrawPieces(DrawingArgs theArgs)
		{
			foreach (Piece p in mPieces)
			{
				if (p != null)
				{
					p.Draw(theArgs, mSquareDims);
				}
			}

		}

		protected virtual void DrawSquares(DrawingArgs theArgs)
		{
			Square sq;

			for (int x = 0; x < (mSquaresLayout.GetLength(0)); x++)
			{
				for (int y = 0; y < (mSquaresLayout.GetLength(1)); y++)
				{
					sq = mSquares[mSquaresLayout[x, y]];
					sq.Draw(theArgs, new Vector2(mSquareDims.X * x, mSquareDims.Y * y));

				}
			}
		}

		protected virtual void DrawGrid(DrawingArgs theArgs)
		{

			int rows = mSquaresLayout.GetLength(1);
			int columns = mSquaresLayout.GetLength(0);

			for (int i = 0; i <= columns; i++)
			{
				VectorDraw.DrawLine(new Vector2(i * mSquareDims.X, 0), new Vector2(2, mSquareDims.Y * rows), Color.Black, theArgs.SpriteBatch, (float)0.2);
			}

			for (int j = 0; j <= rows; j++)
			{
				VectorDraw.DrawLine(new Vector2(0, mSquareDims.Y * j), new Vector2(mSquareDims.X * columns, 2), Color.Black, theArgs.SpriteBatch, (float)0.2);

			}
		}

		protected virtual void DrawHighlight()
		{
			throw new System.NotImplementedException();
		}

		protected virtual void ClearHighlight()
		{
			throw new System.NotImplementedException();
		}

		#endregion	 
		#region	IGameObject	Members

		protected int mID;
		public int ID
		{
			get { return mID; }
		}

		protected string mName;
		public string Name
		{
			get { return mName; }
		}

		#endregion	 


		


	
		#region IDisposable Members

		public void Dispose()
		{
			throw new NotImplementedException();
		}

		#endregion
	}
}
