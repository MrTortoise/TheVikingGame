using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommonObjects.Graphics;
using CommonObjects;
using Custom.Interfaces;
using Microsoft.Xna.Framework;


namespace VikingGameObjects
{


	public class Square 
	{
		//ToDo: implement IDispose Properly
		private GeneralTextureCell mTextureCell;
		private string mName;
		private int mID;
		private float mLayerDepth;
		private SquareType mSquareType;

		public Square(int theID, string theName, GeneralTextureCell theTexture, float theLayerDepth,SquareType  theSquareType)
		{
			mTextureCell = theTexture;
			mName = theName;
			mID = theID;
			mLayerDepth = theLayerDepth;
			mSquareType  = theSquareType;
		}

		public SquareType squareType
		{ get { return mSquareType; } }

		#region IGameObject Members

		public int ID
		{
			get { return mID; }
		}

		public string Name
		{
			get { return mName; }
		}

		#endregion	 

		

		public void Draw(DrawingArgs theDrawingArgs,Vector2 thePosition)
		{
			mTextureCell.DrawAsIs(theDrawingArgs.SpriteBatch, thePosition, mLayerDepth);
		}

	}
}

