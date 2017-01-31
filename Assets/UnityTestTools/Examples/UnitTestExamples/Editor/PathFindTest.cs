using System;
using System.Collections.Generic;
using System.Threading;
using NUnit.Framework;
using UnityEngine;
using Assets.Scripts.Maze;

namespace UnityTest
{
    [TestFixture]
    [Category("Maze tests")]
    public class PathFindTests
    {
        [Test]
        public void FromBootmRightPathFind()
        {
            //Arrange
            MazeHolder mzh = new MazeHolder(10, 10);
            mzh.Mazes[3, 0].type = MazeObjectType.Block;
            mzh.Mazes[3, 1].type = MazeObjectType.Block;
            mzh.Mazes[3, 2].type = MazeObjectType.Block;
            mzh.Mazes[3, 3].type = MazeObjectType.Block;
            Vector3 aim = new Vector3(5, 0, 5);
            Vector3 start = new Vector3(0, 0, 1);
            Vector3 dir = Vector3.one;
            //Act
            mzh.CalculateToPointDirection(start, aim, ref dir);
            //Assert
            Assert.Greater(dir.x,0);
            Assert.GreaterOrEqual(dir.y, 0);
        }


        [Test]
        public void FromTopLeftPathFind()
        {
            //Arrange
            MazeHolder mzh = new MazeHolder(10, 10);
            mzh.Mazes[7, 9].type = MazeObjectType.Block;
            mzh.Mazes[7, 8].type = MazeObjectType.Block;
            mzh.Mazes[6, 7].type = MazeObjectType.Block;
            mzh.Mazes[6, 6].type = MazeObjectType.Block;
            Vector3 aim = new Vector3(5, 0, 5);
            Vector3 start = new Vector3(10, 0,9);
            Vector3 dir = Vector3.one;
            //Act
            mzh.CalculateToPointDirection(start, aim, ref dir);
            //Assert
            Assert.Less(dir.x, 0);
            Assert.LessOrEqual(dir.y, 0);
        }

        [Test]
        public void OutOfBoundsStart()
        {
            //Arrange
            MazeHolder mzh = new MazeHolder(10, 10);
            mzh.Mazes[7, 9].type = MazeObjectType.Block;
            mzh.Mazes[7, 8].type = MazeObjectType.Block;
            mzh.Mazes[6, 7].type = MazeObjectType.Block;
            mzh.Mazes[6, 6].type = MazeObjectType.Block;
            Vector3 aim = new Vector3(6, 0, 6);
            Vector3 start = new Vector3(15, 0, 11);
            Vector3 dir = Vector3.one;
            //Act
            mzh.CalculateToPointDirection(start, aim, ref dir);
            //Assert

            Assert.Fail();  
        }
    }
}
