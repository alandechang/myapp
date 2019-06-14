using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 跳马练习
{
    public class Vault
    {
        private static int[,] Move = new int[,] { { 1, 2 }, { 2, 1 }, { 2, -1 }, { 1, -2 }, { -1, -2 }, { -2, -1 }, { -2, 1 }, { -1, 2 } };//八个方向
        public class Node
        {
            public int X { get; set; }
            public int Y { get; set; }
            public int Operater { get; set; }//算子
        }
        public Vault(int rows, int cols)//传入棋盘大小
        {
            this.Rows = rows;//行
            this.Columns = cols;//列
        }
        public int Rows { get; private set; }
        public int Columns { get; private set; }
        private Stack<Node> Path;//存储路径的栈
        private int StartX;//起始位置X
        private int StartY;//起始位置Y
        private List<Node[]> Result;
        public IEnumerable<Node[]> Run(int x,int y)
        {
            this.Path = new Stack<Node>();
            this.Result = new List<Node[]>();
            this.StartX = x;
            this.StartY = y;
            if (this.IsVaildLocation(x, y))
            {
                Node n = new Node();
                n.X = StartX;
                n.Y = StartY;
                n.Operater = -1;
                Path.Push(n);//路径栈入栈
                this.Iteration();
            }
            return Result.ToArray();
        }
        private void Iteration()//循环
        {
            Node current = this.Path.Peek();//当前位置路径栈的顶部
            for(int i = 0; i < 8; i++)//八个方向
            {
                Node n = new Node();//创建一个新的位置
                n.X = current.X + Move[i, 0];
                n.Y = current.Y + Move[i, 1];
                n.Operater = i + 1;
                if(this.IsVaildLocation(n.X,n.Y)//新的坐标在不在棋盘中
                    && (!this.IsSelfIntersection(n.X, n.Y, this.Path)))//走过这一步没有
                {
                    this.Path.Push(n);//都没有就让当前位置入栈
                    if ((n.X == this.StartX) && (n.Y == this.StartY))//如果回到了起点
                    {
                        if (this.IsFindWay(this.Path))//走到了边界
                        {
                            Node[] p = this.Path.ToArray();//路径栈转数组
                            this.Result.Add(p.Reverse().ToArray());//存出结果
                        }
                        this.Path.Pop();//出栈
                    }
                    else
                    {
                        this.Iteration();
                    }
                }
            }
            this.Path.Pop();
        }
        private bool IsVaildLocation(int x,int y)//是否在棋盘中
        {
            if((1<=x)&&(x<=this.Columns)
                && (1 <= y) && (y <= this.Rows))
            {
                return true;
            }
            return false;
        }
        private bool IsSelfIntersection(int x,int y, Stack<Node> path)//是不是已经走过了这一步
        {
            if (path == null) return false;
            Node[] nodes = path.ToArray();//栈转数组
            for(int i = 0; i < nodes.Length - 1; i++)
            {
                Node n = nodes[i];
                if ((n.X == x) && (n.Y == y)) return true;//
            }
            return false;
        }
        private bool IsFindWay(Stack<Node> path)
        {
            foreach(Node n in path)
            {
                if((n.X==1)||(n.Y==1)
                    ||(n.X==this.Columns)||(n.Y==this.Rows))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
