using System;

namespace UniBlocks
{
    /// <summary>
    /// ゲームピ�Eス�E�Eマス + 5マス混合）を表すクラス
    /// </summary>
    public class UniBlocksPiece
    {
        public enum PieceType
        {
            // 3マスピース
            L3 = 1,      // 3マスL字
            
            // 4マスピース
            P5 = 2,      // 4マスP字
            P5_Mirror = 3,  // 4マスP字鏡像
            J4 = 4,      // 4マスJ字 (1111 / 1000)
            L4 = 5,      // 4マスL字 (1000 / 1111)
            T4 = 6,      // 4マスT字
            
            // 5マスピース
            I5 = 7,      // 5マス直線
            T5 = 8,      // 5マスT字
            U5 = 9,      // 5マスU字
            W5 = 10,     // 5マスW字
            X5 = 11      // 5マス十字
        }

        public PieceType Type { get; private set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Rotation { get; private set; }

        // ピ�Eスの形状定義 [type][rotation][y][x]
        private static readonly int[][][][] Shapes = new int[][][][]
        {
            // L3 piece (3マスL孁E
            new int[][][]
            {
                new int[][] { new int[] {1,0}, new int[] {1,1} },
                new int[][] { new int[] {1,1}, new int[] {1,0} },
                new int[][] { new int[] {1,1}, new int[] {0,1} },
                new int[][] { new int[] {0,1}, new int[] {1,1} }
            },
            
            // P5 piece (4マスP孁E 1 1 0 / 1 1 1 / 0 0 0)
            new int[][][]
            {
                new int[][] { new int[] {1,1,0}, new int[] {1,1,1} },
                new int[][] { new int[] {1,1}, new int[] {1,1}, new int[] {1,0} },
                new int[][] { new int[] {1,1,1}, new int[] {0,1,1} },
                new int[][] { new int[] {0,1}, new int[] {1,1}, new int[] {1,1} }
            },
            
            // P5_Mirror piece (4マスP字鏡僁E 0 1 1 / 1 1 1 / 0 0 0)
            new int[][][]
            {
                new int[][] { new int[] {0,1,1}, new int[] {1,1,1} },
                new int[][] { new int[] {1,0}, new int[] {1,1}, new int[] {1,1} },
                new int[][] { new int[] {1,1,1}, new int[] {1,1,0} },
                new int[][] { new int[] {1,1}, new int[] {1,1}, new int[] {0,1} }
            },
            
            // J4 piece (4マスJ字: 1111 / 1000)
            new int[][][]
            {
                new int[][] { new int[] {1,1,1,1}, new int[] {1,0,0,0} },
                new int[][] { new int[] {1,1}, new int[] {0,1}, new int[] {0,1}, new int[] {0,1} },
                new int[][] { new int[] {0,0,0,1}, new int[] {1,1,1,1} },
                new int[][] { new int[] {1,0}, new int[] {1,0}, new int[] {1,0}, new int[] {1,1} }
            },
            
            // L4 piece (4マスL字: 1000 / 1111)
            new int[][][]
            {
                new int[][] { new int[] {1,0,0,0}, new int[] {1,1,1,1} },
                new int[][] { new int[] {0,1}, new int[] {0,1}, new int[] {0,1}, new int[] {1,1} },
                new int[][] { new int[] {1,1,1,1}, new int[] {0,0,0,1} },
                new int[][] { new int[] {1,1}, new int[] {1,0}, new int[] {1,0}, new int[] {1,0} }
            },
            
            // T4 piece (4マスT字)
            new int[][][]
            {
                new int[][] { new int[] {1,1,1}, new int[] {0,1,0} },
                new int[][] { new int[] {0,1}, new int[] {1,1}, new int[] {0,1} },
                new int[][] { new int[] {0,1,0}, new int[] {1,1,1} },
                new int[][] { new int[] {1,0}, new int[] {1,1}, new int[] {1,0} }
            },
            
            // I5 piece (5マス直線)
            new int[][][]
            {
                new int[][] { new int[] {1}, new int[] {1}, new int[] {1}, new int[] {1}, new int[] {1} },
                new int[][] { new int[] {1,1,1,1,1} },
                new int[][] { new int[] {1}, new int[] {1}, new int[] {1}, new int[] {1}, new int[] {1} },
                new int[][] { new int[] {1,1,1,1,1} }
            },
            
            // T5 piece (5マスT孁E
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
            
            // W5 piece (5マスW字)
            new int[][][]
            {
                new int[][] { new int[] {1,0,0}, new int[] {1,1,0}, new int[] {0,1,1} },
                new int[][] { new int[] {0,1,1}, new int[] {1,1,0}, new int[] {1,0,0} },
                new int[][] { new int[] {1,1,0}, new int[] {0,1,1}, new int[] {0,0,1} },
                new int[][] { new int[] {0,0,1}, new int[] {0,1,1}, new int[] {1,1,0} }
            },
            
            // X5 piece (5マス十孁E
            new int[][][]
            {
                new int[][] { new int[] {0,1,0}, new int[] {1,1,1}, new int[] {0,1,0} },
                new int[][] { new int[] {0,1,0}, new int[] {1,1,1}, new int[] {0,1,0} },
                new int[][] { new int[] {0,1,0}, new int[] {1,1,1}, new int[] {0,1,0} },
                new int[][] { new int[] {0,1,0}, new int[] {1,1,1}, new int[] {0,1,0} }
            }
        };

        public UniBlocksPiece(PieceType type, int startX = 4, int startY = 0)
        {
            Type = type;
            X = startX;
            Y = startY;
            Rotation = 0;
        }

        /// <summary>
        /// 現在の形状を取征E
        /// </summary>
        public int[][] GetShape()
        {
            return Shapes[(int)Type - 1][Rotation];
        }

        /// <summary>
        /// 回転�E�時計回り！E
        /// </summary>
        public void Rotate()
        {
            Rotation = (Rotation + 1) % 4;
        }

        /// <summary>
        /// 回転を戻ぁE
        /// </summary>
        public void RotateBack()
        {
            Rotation = (Rotation - 1 + 4) % 4;
        }

        /// <summary>
        /// ピ�Eスのコピ�Eを作�E
        /// </summary>
        public UniBlocksPiece Clone()
        {
            return new UniBlocksPiece(Type, X, Y) { Rotation = this.Rotation };
        }

        /// <summary>
        /// ランダムなピ�Eスを生成E
        /// </summary>
        public static UniBlocksPiece CreateRandom()
        {
            Random random = new Random(Guid.NewGuid().GetHashCode());
            PieceType type = (PieceType)random.Next(1, 12); // 1-11の11種類
            return new UniBlocksPiece(type);
        }
    }
}
