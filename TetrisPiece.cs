using System;

namespace UniBlocks
{
    /// <summary>
    /// ゲームピース（3マス + 5マス混合）を表すクラス
    /// </summary>
    public class TetrisPiece
    {
        public enum PieceType
        {
            // 3マスピース
            L3 = 1,      // 3マスL字
            
            // 4マスピース
            P5 = 2,      // 4マスP字
            P5_Mirror = 3,  // 4マスP字(鏡像)
            
            // 5マスピース
            I5 = 4,      // 5マス直線
            T5 = 5,      // 5マスT字
            U5 = 6,      // 5マスU字
            F5 = 7,      // 5マスF字
            F5_Mirror = 8,  // 5マスF字(鏡像)
            W5 = 9,      // 5マスW字
            X5 = 10      // 5マス十字
        }

        public PieceType Type { get; private set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Rotation { get; private set; }

        // ピースの形状定義 [type][rotation][y][x]
        private static readonly int[][][][] Shapes = new int[][][][]
        {
            // L3 piece (3マスL字)
            new int[][][]
            {
                new int[][] { new int[] {1,0}, new int[] {1,1} },
                new int[][] { new int[] {1,1}, new int[] {1,0} },
                new int[][] { new int[] {1,1}, new int[] {0,1} },
                new int[][] { new int[] {0,1}, new int[] {1,1} }
            },
            
            // P5 piece (4マスP字: 1 1 0 / 1 1 1 / 0 0 0)
            new int[][][]
            {
                new int[][] { new int[] {1,1,0}, new int[] {1,1,1} },
                new int[][] { new int[] {1,1}, new int[] {1,1}, new int[] {1,0} },
                new int[][] { new int[] {1,1,1}, new int[] {0,1,1} },
                new int[][] { new int[] {1,0}, new int[] {1,1}, new int[] {1,1} }
            },
            
            // P5_Mirror piece (4マスP字鏡像: 0 1 1 / 1 1 1 / 0 0 0)
            new int[][][]
            {
                new int[][] { new int[] {0,1,1}, new int[] {1,1,1} },
                new int[][] { new int[] {1,0}, new int[] {1,1}, new int[] {1,1} },
                new int[][] { new int[] {1,1,1}, new int[] {1,1,0} },
                new int[][] { new int[] {1,1}, new int[] {1,1}, new int[] {0,1} }
            },
            
            // I5 piece (5マス直線)
            new int[][][]
            {
                new int[][] { new int[] {1}, new int[] {1}, new int[] {1}, new int[] {1}, new int[] {1} },
                new int[][] { new int[] {1,1,1,1,1} },
                new int[][] { new int[] {1}, new int[] {1}, new int[] {1}, new int[] {1}, new int[] {1} },
                new int[][] { new int[] {1,1,1,1,1} }
            },
            
            // T5 piece (5マスT字)
            new int[][][]
            {
                new int[][] { new int[] {1,1,1}, new int[] {0,1,0}, new int[] {0,1,0} },
                new int[][] { new int[] {0,0,1}, new int[] {1,1,1}, new int[] {0,0,1} },
                new int[][] { new int[] {0,1,0}, new int[] {0,1,0}, new int[] {1,1,1} },
                new int[][] { new int[] {1,0,0}, new int[] {1,1,1}, new int[] {1,0,0} }
            },
            
            // U5 piece (5マスU字)
            new int[][][]
            {
                new int[][] { new int[] {1,0,1}, new int[] {1,1,1} },
                new int[][] { new int[] {1,1}, new int[] {1,0}, new int[] {1,1} },
                new int[][] { new int[] {1,1,1}, new int[] {1,0,1} },
                new int[][] { new int[] {1,1}, new int[] {0,1}, new int[] {1,1} }
            },
            
            // F5 piece (5マスF字)
            new int[][][]
            {
                new int[][] { new int[] {0,1,1}, new int[] {1,1,0}, new int[] {0,1,0} },
                new int[][] { new int[] {0,1,0}, new int[] {1,1,1}, new int[] {0,0,1} },
                new int[][] { new int[] {0,1,0}, new int[] {0,1,1}, new int[] {1,1,0} },
                new int[][] { new int[] {1,0,0}, new int[] {1,1,1}, new int[] {0,1,0} }
            },
            
            // F5_Mirror piece (5マスF字鏡像)
            new int[][][]
            {
                new int[][] { new int[] {1,1,0}, new int[] {0,1,1}, new int[] {0,1,0} },
                new int[][] { new int[] {0,0,1}, new int[] {1,1,1}, new int[] {0,1,0} },
                new int[][] { new int[] {0,1,0}, new int[] {1,1,0}, new int[] {0,1,1} },
                new int[][] { new int[] {0,1,0}, new int[] {1,1,1}, new int[] {1,0,0} }
            },
            
            // W5 piece (5マスW字)
            new int[][][]
            {
                new int[][] { new int[] {1,0,0}, new int[] {1,1,0}, new int[] {0,1,1} },
                new int[][] { new int[] {0,1,1}, new int[] {1,1,0}, new int[] {1,0,0} },
                new int[][] { new int[] {1,1,0}, new int[] {0,1,1}, new int[] {0,0,1} },
                new int[][] { new int[] {0,0,1}, new int[] {0,1,1}, new int[] {1,1,0} }
            },
            
            // X5 piece (5マス十字)
            new int[][][]
            {
                new int[][] { new int[] {0,1,0}, new int[] {1,1,1}, new int[] {0,1,0} },
                new int[][] { new int[] {0,1,0}, new int[] {1,1,1}, new int[] {0,1,0} },
                new int[][] { new int[] {0,1,0}, new int[] {1,1,1}, new int[] {0,1,0} },
                new int[][] { new int[] {0,1,0}, new int[] {1,1,1}, new int[] {0,1,0} }
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
            PieceType type = (PieceType)random.Next(1, 11); // 1-10の10種類
            return new TetrisPiece(type);
        }
    }
}
