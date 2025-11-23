using System;

namespace UniBlocks
{
    /// <summary>
    /// UniBlocksゲームのメインロジチE��
    /// </summary>
    public class UniBlocksGame
    {
        public UniBlocksBoard Board { get; private set; }
        public UniBlocksPiece CurrentPiece { get; private set; }
        public UniBlocksPiece NextPiece { get; private set; }
        public UniBlocksPiece HoldPiece { get; private set; }
        
        public int Score { get; private set; }
        public int Lines { get; private set; }
        public int Level { get; private set; }
        public bool IsGameOver { get; private set; }
        public bool IsPaused { get; set; }

        private bool canHold = true;
        private Random random;

        // イベンチE
        public event Action OnBoardChanged;
        public event Action OnScoreChanged;
        public event Action OnGameOver;
        public event Action OnPieceChanged;

        public UniBlocksGame()
        {
            Board = new UniBlocksBoard();
            // より良いランダム性を確保するため、Guidを使用してシードを生成
            random = new Random(Guid.NewGuid().GetHashCode());
            Initialize();
        }

        /// <summary>
        /// ゲームの初期匁E
        /// </summary>
        public void Initialize()
        {
            Board.Clear();
            Score = 0;
            Lines = 0;
            Level = 1;
            IsGameOver = false;
            IsPaused = false;
            canHold = true;
            HoldPiece = null;

            NextPiece = CreateRandomPiece();
            SpawnNewPiece();
        }

        /// <summary>
        /// ランダムなピースを生成
        /// </summary>
        private UniBlocksPiece CreateRandomPiece()
        {
            UniBlocksPiece.PieceType type = (UniBlocksPiece.PieceType)random.Next(1, 12); // 1-11の11種類
            UniBlocksPiece piece = new UniBlocksPiece(type);
            AlignPieceToTop(piece);
            return piece;
        }

        /// <summary>
        /// 新しいピ�Eスを�E現させめE
        /// </summary>
        private void SpawnNewPiece()
        {
            CurrentPiece = NextPiece;
            AlignPieceToTop(CurrentPiece);
            NextPiece = CreateRandomPiece();
            canHold = true;

            if (Board.CheckCollision(CurrentPiece))
            {
                IsGameOver = true;
                OnGameOver?.Invoke();
            }

            OnPieceChanged?.Invoke();
        }

        /// <summary>
        /// 1フレーム刁E�E更新�E��E然落下！E
        /// </summary>
        public void Step()
        {
            if (IsGameOver || IsPaused) return;

            if (!MoveDown())
            {
                LockPiece();
            }
        }

        /// <summary>
        /// 左に移勁E
        /// </summary>
        public bool MoveLeft()
        {
            if (IsGameOver || IsPaused) return false;

            CurrentPiece.X--;
            if (Board.CheckCollision(CurrentPiece))
            {
                CurrentPiece.X++;
                return false;
            }
            OnBoardChanged?.Invoke();
            return true;
        }

        /// <summary>
        /// 右に移勁E
        /// </summary>
        public bool MoveRight()
        {
            if (IsGameOver || IsPaused) return false;

            CurrentPiece.X++;
            if (Board.CheckCollision(CurrentPiece))
            {
                CurrentPiece.X--;
                return false;
            }
            OnBoardChanged?.Invoke();
            return true;
        }

        /// <summary>
        /// 下に移勁E
        /// </summary>
        public bool MoveDown()
        {
            if (IsGameOver || IsPaused) return false;

            CurrentPiece.Y++;
            if (Board.CheckCollision(CurrentPiece))
            {
                CurrentPiece.Y--;
                return false;
            }
            OnBoardChanged?.Invoke();
            return true;
        }

        /// <summary>
        /// 回転�E�壁蹴り対応！E
        /// </summary>
        public bool Rotate()
        {
            if (IsGameOver || IsPaused) return false;

            int originalX = CurrentPiece.X;
            CurrentPiece.Rotate();
            
            // 通常の回転が可能ならそのまま実衁E
            if (!Board.CheckCollision(CurrentPiece))
            {
                OnBoardChanged?.Invoke();
                return true;
            }
            
            // 壁蹴めE 左右に1マスずらして回転を試みめE
            // 右にずらして試衁E
            CurrentPiece.X = originalX + 1;
            if (!Board.CheckCollision(CurrentPiece))
            {
                OnBoardChanged?.Invoke();
                return true;
            }
            
            // 左にずらして試衁E
            CurrentPiece.X = originalX - 1;
            if (!Board.CheckCollision(CurrentPiece))
            {
                OnBoardChanged?.Invoke();
                return true;
            }
            
            // さらに右に2マスずらして試行！E5の横回転用�E�E
            CurrentPiece.X = originalX + 2;
            if (!Board.CheckCollision(CurrentPiece))
            {
                OnBoardChanged?.Invoke();
                return true;
            }
            
            // さらに左に2マスずらして試行！E5の横回転用�E�E
            CurrentPiece.X = originalX - 2;
            if (!Board.CheckCollision(CurrentPiece))
            {
                OnBoardChanged?.Invoke();
                return true;
            }
            
            // すべて失敗した場合�E回転をキャンセル
            CurrentPiece.X = originalX;
            CurrentPiece.RotateBack();
            return false;
        }

        /// <summary>
        /// ハ�EドドロチE�E
        /// </summary>
        public void HardDrop()
        {
            if (IsGameOver || IsPaused) return;

            int dropDistance = 0;
            while (MoveDown())
            {
                dropDistance++;
            }
            
            // ハ�EドドロチE�Eボ�Eナス
            Score += dropDistance * 2;
            
            LockPiece();
            OnScoreChanged?.Invoke();
        }

        /// <summary>
        /// ホ�Eルド機�E
        /// </summary>
        public void Hold()
        {
            if (IsGameOver || IsPaused || !canHold) return;

            if (HoldPiece == null)
            {
                HoldPiece = new UniBlocksPiece(CurrentPiece.Type);
                AlignPieceToTop(HoldPiece);
                SpawnNewPiece();
            }
            else
            {
                UniBlocksPiece temp = new UniBlocksPiece(CurrentPiece.Type);
                AlignPieceToTop(temp);
                CurrentPiece = new UniBlocksPiece(HoldPiece.Type);
                AlignPieceToTop(CurrentPiece);
                HoldPiece = temp;

                if (Board.CheckCollision(CurrentPiece))
                {
                    IsGameOver = true;
                    OnGameOver?.Invoke();
                    return;
                }
            }

            canHold = false;
            OnPieceChanged?.Invoke();
            OnBoardChanged?.Invoke();
        }

        /// <summary>
        /// ピ�Eスを固宁E
        /// </summary>
        private void LockPiece()
        {
            Board.PlacePiece(CurrentPiece);
            
            int linesCleared = Board.ClearLines();
            if (linesCleared > 0)
            {
                Lines += linesCleared;
                
                // スコア計算！Eライン: 100, 2ライン: 300, 3ライン: 500, 4ライン: 800�E�E
                int[] lineScores = { 0, 100, 300, 500, 800 };
                Score += lineScores[Math.Min(linesCleared, 4)] * Level;
                
                // レベルアチE�E�E�E0ライン毎！E
                Level = Lines / 10 + 1;
                
                OnScoreChanged?.Invoke();
            }

            SpawnNewPiece();
            OnBoardChanged?.Invoke();
        }

        /// <summary>
        /// ゴーストピース�E�落下予測位置�E�を取征E
        /// </summary>
        public UniBlocksPiece GetGhostPiece()
        {
            UniBlocksPiece ghost = CurrentPiece.Clone();
            while (true)
            {
                ghost.Y++;
                if (Board.CheckCollision(ghost))
                {
                    ghost.Y--;
                    break;
                }
            }
            return ghost;
        }

        /// <summary>
        /// 落下速度を取得（ミリ秒！E
        /// </summary>
        public double GetDropSpeed()
        {
            // スコアに応じて速度を上げめE
            // 0点: 1000ms, 1000点: 500ms, 5000点: 200ms, 10000点以丁E 100ms
            double baseSpeed = 1000;
            double speedReduction = Score / 100.0; // 100点ごとに10ms速くなめE
            return Math.Max(100, baseSpeed - speedReduction);
        }

        /// <summary>
        /// ブロックの下端が盤面最上段に揃うよう初期Y座標を調整
        /// </summary>
        private void AlignPieceToTop(UniBlocksPiece piece)
        {
            if (piece == null) return;
            piece.Y = -piece.GetBottomOccupiedRow();
        }
    }
}
