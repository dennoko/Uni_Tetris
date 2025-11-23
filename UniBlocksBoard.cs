using System;

namespace UniBlocks
{
    /// <summary>
    /// 繧ｲ繝ｼ繝縺ｮ逶､髱｢繧堤ｮ｡逅・☆繧九け繝ｩ繧ｹ
    /// </summary>
    public class UniBlocksBoard
    {
        public const int Width = 12;
        public const int Height = 25;

        private int[,] grid;

        public UniBlocksBoard()
        {
            grid = new int[Height, Width];
            Clear();
        }

        /// <summary>
        /// 逶､髱｢繧偵け繝ｪ繧｢
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
        /// 謖・ｮ壻ｽ咲ｽｮ縺ｮ繧ｻ繝ｫ縺ｮ蛟､繧貞叙蠕・
        /// </summary>
        public int GetCell(int x, int y)
        {
            if (x < 0 || x >= Width || y < 0 || y >= Height)
                return -1;
            return grid[y, x];
        }

        /// <summary>
        /// 謖・ｮ壻ｽ咲ｽｮ縺ｮ繧ｻ繝ｫ縺ｮ蛟､繧定ｨｭ螳・
        /// </summary>
        public void SetCell(int x, int y, int value)
        {
            if (x >= 0 && x < Width && y >= 0 && y < Height)
            {
                grid[y, x] = value;
            }
        }

        /// <summary>
        /// 繝斐・繧ｹ縺碁・鄂ｮ蜿ｯ閭ｽ縺九メ繧ｧ繝・け
        /// </summary>
        public bool CheckCollision(UniBlocksPiece piece)
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

                        // 逶､髱｢螟悶メ繧ｧ繝・け
                        if (boardX < 0 || boardX >= Width || boardY >= Height)
                            return true;

                        // 荳狗ｫｯ繝√ぉ繝・け縺ｯ迚ｹ蛻･謇ｱ縺・ｼ・ < 0縺ｯ險ｱ螳ｹ・・
                        if (boardY < 0)
                            continue;

                        // 譌｢蟄倥ヶ繝ｭ繝・け縺ｨ縺ｮ陦晉ｪ√メ繧ｧ繝・け
                        if (grid[boardY, boardX] != 0)
                            return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// 繝斐・繧ｹ繧堤乢髱｢縺ｫ蝗ｺ螳・
        /// </summary>
        public void PlacePiece(UniBlocksPiece piece)
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
        /// 螳梧・縺励◆繝ｩ繧､繝ｳ繧偵け繝ｪ繧｢縺励※豸亥悉縺励◆繝ｩ繧､繝ｳ謨ｰ繧定ｿ斐☆
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
                    // 繝ｩ繧､繝ｳ繧貞炎髯､縺励※荳翫・繝ｩ繧､繝ｳ繧剃ｸ九￡繧・
                    for (int moveY = y; moveY > 0; moveY--)
                    {
                        for (int x = 0; x < Width; x++)
                        {
                            grid[moveY, x] = grid[moveY - 1, x];
                        }
                    }
                    // 譛荳願｡後ｒ繧ｯ繝ｪ繧｢
                    for (int x = 0; x < Width; x++)
                    {
                        grid[0, x] = 0;
                    }
                    // 蜷後§陦後ｒ蜀阪メ繧ｧ繝・け
                    y++;
                }
            }

            return linesCleared;
        }

        /// <summary>
        /// 逶､髱｢縺ｮ繧ｳ繝斐・繧剃ｽ懈・
        /// </summary>
        public int[,] GetGridCopy()
        {
            int[,] copy = new int[Height, Width];
            Array.Copy(grid, copy, grid.Length);
            return copy;
        }
    }
}
