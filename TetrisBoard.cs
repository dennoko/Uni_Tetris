using System;

namespace UniTetris
{
    /// <summary>
    /// テトリスの盤面を管理するクラス
    /// </summary>
    public class TetrisBoard
    {
        public const int Width = 10;
        public const int Height = 20;

        private int[,] grid;

        public TetrisBoard()
        {
            grid = new int[Height, Width];
            Clear();
        }

        /// <summary>
        /// 盤面をクリア
        /// </summary>
        public void Clear()
        {
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    grid[y, x] = 0;
                }
            }
        }

        /// <summary>
        /// 指定位置のセルの値を取得
        /// </summary>
        public int GetCell(int x, int y)
        {
            if (x < 0 || x >= Width || y < 0 || y >= Height)
                return -1;
            return grid[y, x];
        }

        /// <summary>
        /// 指定位置のセルの値を設定
        /// </summary>
        public void SetCell(int x, int y, int value)
        {
            if (x >= 0 && x < Width && y >= 0 && y < Height)
            {
                grid[y, x] = value;
            }
        }

        /// <summary>
        /// ピースが配置可能かチェック
        /// </summary>
        public bool CheckCollision(TetrisPiece piece)
        {
            int[][] shape = piece.GetShape();
            
            for (int y = 0; y < shape.Length; y++)
            {
                for (int x = 0; x < shape[y].Length; x++)
                {
                    if (shape[y][x] != 0)
                    {
                        int boardX = piece.X + x;
                        int boardY = piece.Y + y;

                        // 盤面外チェック
                        if (boardX < 0 || boardX >= Width || boardY >= Height)
                            return true;

                        // 下端チェックは特別扱い（Y < 0は許容）
                        if (boardY < 0)
                            continue;

                        // 既存ブロックとの衝突チェック
                        if (grid[boardY, boardX] != 0)
                            return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// ピースを盤面に固定
        /// </summary>
        public void PlacePiece(TetrisPiece piece)
        {
            int[][] shape = piece.GetShape();
            
            for (int y = 0; y < shape.Length; y++)
            {
                for (int x = 0; x < shape[y].Length; x++)
                {
                    if (shape[y][x] != 0)
                    {
                        int boardX = piece.X + x;
                        int boardY = piece.Y + y;

                        if (boardY >= 0 && boardY < Height && boardX >= 0 && boardX < Width)
                        {
                            grid[boardY, boardX] = (int)piece.Type;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 完成したラインをクリアして消去したライン数を返す
        /// </summary>
        public int ClearLines()
        {
            int linesCleared = 0;

            for (int y = Height - 1; y >= 0; y--)
            {
                bool lineFull = true;
                for (int x = 0; x < Width; x++)
                {
                    if (grid[y, x] == 0)
                    {
                        lineFull = false;
                        break;
                    }
                }

                if (lineFull)
                {
                    linesCleared++;
                    // ラインを削除して上のラインを下げる
                    for (int moveY = y; moveY > 0; moveY--)
                    {
                        for (int x = 0; x < Width; x++)
                        {
                            grid[moveY, x] = grid[moveY - 1, x];
                        }
                    }
                    // 最上行をクリア
                    for (int x = 0; x < Width; x++)
                    {
                        grid[0, x] = 0;
                    }
                    // 同じ行を再チェック
                    y++;
                }
            }

            return linesCleared;
        }

        /// <summary>
        /// 盤面のコピーを作成
        /// </summary>
        public int[,] GetGridCopy()
        {
            int[,] copy = new int[Height, Width];
            Array.Copy(grid, copy, grid.Length);
            return copy;
        }
    }
}
