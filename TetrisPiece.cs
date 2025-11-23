using System;

namespace UniBlocks
{
    /// <summary>
    /// ゲームピース（ペントミノ）を表すクラス
    /// </summary>
    public class TetrisPiece
    {
        public enum PieceType
        {
            F = 1,
            I = 2,
            L = 3,
            N = 4,
            P = 5,
            T = 6,
            U = 7,
            V = 8,
            W = 9,
            X = 10,
            Y = 11,
            Z = 12
        }

        public PieceType Type { get; private set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Rotation { get; private set; }

        // ペントミノの形状定義 [type][rotation][y][x]
        // 5マスで構成される12種類のペントミノ
        private static readonly int[][][][] Shapes = new int[][][][]
        {
            // F piece
            new int[][][]
            {
                new int[][] { new int[] {0,1,1}, new int[] {1,1,0}, new int[] {0,1,0} },
                new int[][] { new int[] {0,1,0}, new int[] {1,1,1}, new int[] {0,0,1} },
                new int[][] { new int[] {0,1,0}, new int[] {0,1,1}, new int[] {1,1,0} },
                new int[][] { new int[] {1,0,0}, new int[] {1,1,1}, new int[] {0,1,0} }
            },
            // I piece (5マス縦長)
            new int[][][]
            {
                new int[][] { new int[] {1}, new int[] {1}, new int[] {1}, new int[] {1}, new int[] {1} },
                new int[][] { new int[] {1,1,1,1,1} },
                new int[][] { new int[] {1}, new int[] {1}, new int[] {1}, new int[] {1}, new int[] {1} },
                new int[][] { new int[] {1,1,1,1,1} }
            },
            // L piece
            new int[][][]
            {
                new int[][] { new int[] {1,0}, new int[] {1,0}, new int[] {1,0}, new int[] {1,1} },
                new int[][] { new int[] {1,1,1,1}, new int[] {1,0,0,0} },
                new int[][] { new int[] {1,1}, new int[] {0,1}, new int[] {0,1}, new int[] {0,1} },
                new int[][] { new int[] {0,0,0,1}, new int[] {1,1,1,1} }
            },
            // N piece
            new int[][][]
            {
                new int[][] { new int[] {0,1}, new int[] {1,1}, new int[] {1,0}, new int[] {1,0} },
                new int[][] { new int[] {1,1,0}, new int[] {0,1,1,1} },
                new int[][] { new int[] {0,1}, new int[] {0,1}, new int[] {1,1}, new int[] {1,0} },
                new int[][] { new int[] {1,1,1,0}, new int[] {0,0,1,1} }
            },
            // P piece
            new int[][][]
            {
                new int[][] { new int[] {1,1}, new int[] {1,1}, new int[] {1,0} },
                new int[][] { new int[] {1,1,1}, new int[] {0,1,1} },
                new int[][] { new int[] {0,1}, new int[] {1,1}, new int[] {1,1} },
                new int[][] { new int[] {1,1,0}, new int[] {1,1,1} }
            },
            // T piece
            new int[][][]
            {
                new int[][] { new int[] {1,1,1}, new int[] {0,1,0}, new int[] {0,1,0} },
                new int[][] { new int[] {0,0,1}, new int[] {1,1,1}, new int[] {0,0,1} },
                new int[][] { new int[] {0,1,0}, new int[] {0,1,0}, new int[] {1,1,1} },
                new int[][] { new int[] {1,0,0}, new int[] {1,1,1}, new int[] {1,0,0} }
            },
            // U piece
            new int[][][]
            {
                new int[][] { new int[] {1,0,1}, new int[] {1,1,1} },
                new int[][] { new int[] {1,1}, new int[] {1,0}, new int[] {1,1} },
                new int[][] { new int[] {1,1,1}, new int[] {1,0,1} },
                new int[][] { new int[] {1,1}, new int[] {0,1}, new int[] {1,1} }
            },
            // V piece
            new int[][][]
            {
                new int[][] { new int[] {1,0,0}, new int[] {1,0,0}, new int[] {1,1,1} },
                new int[][] { new int[] {1,1,1}, new int[] {1,0,0}, new int[] {1,0,0} },
                new int[][] { new int[] {1,1,1}, new int[] {0,0,1}, new int[] {0,0,1} },
                new int[][] { new int[] {0,0,1}, new int[] {0,0,1}, new int[] {1,1,1} }
            },
            // W piece
            new int[][][]
            {
                new int[][] { new int[] {1,0,0}, new int[] {1,1,0}, new int[] {0,1,1} },
                new int[][] { new int[] {0,1,1}, new int[] {1,1,0}, new int[] {1,0,0} },
                new int[][] { new int[] {1,1,0}, new int[] {0,1,1}, new int[] {0,0,1} },
                new int[][] { new int[] {0,0,1}, new int[] {0,1,1}, new int[] {1,1,0} }
            },
            // X piece (十字)
            new int[][][]
            {
                new int[][] { new int[] {0,1,0}, new int[] {1,1,1}, new int[] {0,1,0} },
                new int[][] { new int[] {0,1,0}, new int[] {1,1,1}, new int[] {0,1,0} },
                new int[][] { new int[] {0,1,0}, new int[] {1,1,1}, new int[] {0,1,0} },
                new int[][] { new int[] {0,1,0}, new int[] {1,1,1}, new int[] {0,1,0} }
            },
            // Y piece
            new int[][][]
            {
                new int[][] { new int[] {0,1}, new int[] {1,1}, new int[] {0,1}, new int[] {0,1} },
                new int[][] { new int[] {0,0,1,0}, new int[] {1,1,1,1} },
                new int[][] { new int[] {1,0}, new int[] {1,0}, new int[] {1,1}, new int[] {1,0} },
                new int[][] { new int[] {1,1,1,1}, new int[] {0,1,0,0} }
            },
            // Z piece
            new int[][][]
            {
                new int[][] { new int[] {1,1,0}, new int[] {0,1,0}, new int[] {0,1,1} },
                new int[][] { new int[] {0,0,1}, new int[] {1,1,1}, new int[] {1,0,0} },
                new int[][] { new int[] {1,1,0}, new int[] {0,1,0}, new int[] {0,1,1} },
                new int[][] { new int[] {0,0,1}, new int[] {1,1,1}, new int[] {1,0,0} }
            }
        };

        public TetrisPiece(PieceType type, int startX = 4, int startY = 0)
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
            PieceType type = (PieceType)random.Next(1, 13);
            return new TetrisPiece(type);
        }
    }
}
