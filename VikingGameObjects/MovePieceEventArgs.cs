using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingGameObjects
{
	public class MovePieceEventArgs : EventArgs 
	{
		protected int[] mTargetPosition;
		protected Board mBoard;
		protected int[] mSourcePosition;

		public int[] TargetPosition
		{
			get { return mTargetPosition; }
			set { mTargetPosition = value; }
		}

		public int[] SourcePosition
		{
			get { return mSourcePosition; }
			set { mSourcePosition = value; }
		}

		public Board Board
		{ get { return mBoard; } }

		public MovePieceEventArgs(int[] theTargetPosition, Board theBoard)
		{
			mTargetPosition = theTargetPosition;
			mBoard = theBoard;
		}


	}
}
