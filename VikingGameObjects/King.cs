using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using CommonObjects.Graphics;

namespace VikingGameObjects
{
	public class King   : Piece 
	{
		public event EventHandler GameWon;
		public event EventHandler GameLost;

		public King(int[] thePosition,eSide theSide,GeneralTextureCell theTexture)
			:base(thePosition,theSide,theTexture)
		{
			base.Moved += new EventHandler<MovePieceEventArgs>(King_Moved);	
		}

		void King_Moved(object sender, MovePieceEventArgs e)
		{
			Board b = e.Board;

			if (b.GetSquareTypeAt(mPosition).Name == "ExitPiece")
			{
				GameWon(this, new EventArgs());
			}
		}

		protected override bool isTaken(Board theBoard)
		{
			bool isTaken = false;	
			// test	if on top edge	
			// test	if on botom	edge
			// test	if enemy on	left and if	on left	then take

			if ((mPosition[1] == 0) || (mPosition[1] == 10) || (mPosition[0] == 0) || (mPosition[0] == 10))
			{ isTaken = false; }
			else
				// must be middle of board
				if ((HorizontalTake(theBoard)) && (VerticalTake(theBoard)))
				{
					isTaken = true;					
				}
			return isTaken;
		}

		protected override bool isLandable(SquareType sq)
		{
			return true;
		}


	}
}
