using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingGameObjects
{
	public class SquareType
	{
		protected int mID;
		protected string mName;
		protected bool mLandable;
		protected bool mEnemy;

		public SquareType(int theID, string theName, bool Landability, bool Enemyishness)
		{
			mID = theID;
			mName = theName;
			mLandable = Landability;
			mEnemy = Enemyishness;
		}

		public int ID
		{ get { return mID; } }

		public string Name
		{ get { return mName; } }

		public bool Landable
		{ get { return mLandable; } }

		public bool Enemy
		{ get { return mEnemy; } }

	}
}
