using System;

namespace UniTetris
{
    /// <summary>
    /// テトリスのミノ（ピース）を表すクラス
    /// </summary>
    public class TetrisPiece
    {
        public enum PieceType
        {
            I = 1,
            O = 2,
            T = 3,
            S = 4,
            Z = 5,
            J = 6,
            L = 7
        }

        public PieceType Type { get; private set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Rotation { get; private set; }

        // ミノの形状定義 [type][rotation][y][x]
        private static readonly int[][][][] Shapes = new int[][][][]
        {
            // I piece
            new int[][][]
            {
                new int[][] { new int[] {0,0,0,0}, new int[] {1,1,1,1}, new int[] {0,0,0,0}, new int[] {0,0,0,0} },
                new int[][] { new int[] {0,0,1,0}, new int[] {0,0,1,0}, new int[] {0,0,1,0}, new int[] {0,0,1,0} },
                new int[][] { new int[] {0,0,0,0}, new int[] {0,0,0,0}, new int[] {1,1,1,1}, new int[] {0,0,0,0} },
                new int[][] { new int[] {0,1,0,0}, new int[] {0,1,0,0}, new int[] {0,1,0,0}, new int[] {0,1,0,0} }
            },
            // O piece
            new int[][][]
            {
                new int[][] { new int[] {0,1,1,0}, new int[] {0,1,1,0}, new int[] {0,0,0,0} },
                new int[][] { new int[] {0,1,1,0}, new int[] {0,1,1,0}, new int[] {0,0,0,0} },
                new int[][] { new int[] {0,1,1,0}, new int[] {0,1,1,0}, new int[] {0,0,0,0} },
                new int[][] { new int[] {0,1,1,0}, new int[] {0,1,1,0}, new int[] {0,0,0,0} }
            },
            // T piece
            new int[][][]
            {
                new int[][] { new int[] {0,1,0}, new int[] {1,1,1}, new int[] {0,0,0} },
                new int[][] { new int[] {0,1,0}, new int[] {0,1,1}, new int[] {0,1,0} },
                new int[][] { new int[] {0,0,0}, new int[] {1,1,1}, new int[] {0,1,0} },
                new int[][] { new int[] {0,1,0}, new int[] {1,1,0}, new int[] {0,1,0} }
            },
            // S piece
            new int[][][]
            {
                new int[][] { new int[] {0,1,1}, new int[] {1,1,0}, new int[] {0,0,0} },
                new int[][] { new int[] {0,1,0}, new int[] {0,1,1}, new int[] {0,0,1} },
                new int[][] { new int[] {0,0,0}, new int[] {0,1,1}, new int[] {1,1,0} },
                new int[][] { new int[] {1,0,0}, new int[] {1,1,0}, new int[] {0,1,0} }
            },
            // Z piece
            new int[][][]
            {
                new int[][] { new int[] {1,1,0}, new int[] {0,1,1}, new int[] {0,0,0} },
                new int[][] { new int[] {0,0,1}, new int[] {0,1,1}, new int[] {0,1,0} },
                new int[][] { new int[] {0,0,0}, new int[] {1,1,0}, new int[] {0,1,1} },
                new int[][] { new int[] {0,1,0}, new int[] {1,1,0}, new int[] {1,0,0} }
            },
            // J piece
            new int[][][]
            {
                new int[][] { new int[] {1,0,0}, new int[] {1,1,1}, new int[] {0,0,0} },
                new int[][] { new int[] {0,1,1}, new int[] {0,1,0}, new int[] {0,1,0} },
                new int[][] { new int[] {0,0,0}, new int[] {1,1,1}, new int[] {0,0,1} },
                new int[][] { new int[] {0,1,0}, new int[] {0,1,0}, new int[] {1,1,0} }
            },
            // L piece
            new int[][][]
            {
                new int[][] { new int[] {0,0,1}, new int[] {1,1,1}, new int[] {0,0,0} },
                new int[][] { new int[] {0,1,0}, new int[] {0,1,0}, new int[] {0,1,1} },
                new int[][] { new int[] {0,0,0}, new int[] {1,1,1}, new int[] {1,0,0} },
                new int[][] { new int[] {1,1,0}, new int[] {0,1,0}, new int[] {0,1,0} }
            }
        };

        public TetrisPiece(PieceType type, int startX = 3, int startY = 0)
        {
            Type = type;
            X = startX;
            Y = startY;
            Rotation = 0;
        }

        /// <summary>
        /// 現在の形状を取得
        /// </summary>
        public int[][] GetShape()
        {
            return Shapes[(int)Type - 1][Rotation];
        }

        /// <summary>
        /// 回転（時計回り）
        /// </summary>
        public void Rotate()
        {
            Rotation = (Rotation + 1) % 4;
        }

        /// <summary>
        /// 回転を戻す
        /// </summary>
        public void RotateBack()
        {
            Rotation = (Rotation - 1 + 4) % 4;
        }

        /// <summary>
        /// ピースのコピーを作成
        /// </summary>
        public TetrisPiece Clone()
        {
            return new TetrisPiece(Type, X, Y) { Rotation = this.Rotation };
        }

        /// <summary>
        /// ランダムなピースを生成
        /// </summary>
        public static TetrisPiece CreateRandom()
        {
            Random random = new Random();
            PieceType type = (PieceType)random.Next(1, 8);
            return new TetrisPiece(type);
        }
    }
}
