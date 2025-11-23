using System;

namespace UniBlocks
{
    /// <summary>
    /// テトリスゲームのメインロジック
    /// </summary>
    public class TetrisGame
    {
        public TetrisBoard Board { get; private set; }
        public TetrisPiece CurrentPiece { get; private set; }
        public TetrisPiece NextPiece { get; private set; }
        public TetrisPiece HoldPiece { get; private set; }
        
        public int Score { get; private set; }
        public int Lines { get; private set; }
        public int Level { get; private set; }
        public bool IsGameOver { get; private set; }
        public bool IsPaused { get; set; }

        private bool canHold = true;
        private Random random;

        // イベント
        public event Action OnBoardChanged;
        public event Action OnScoreChanged;
        public event Action OnGameOver;
        public event Action OnPieceChanged;

        public TetrisGame()
        {
            Board = new TetrisBoard();
            random = new Random();
            Initialize();
        }

        /// <summary>
        /// ゲームの初期化
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
        private TetrisPiece CreateRandomPiece()
        {
            TetrisPiece.PieceType type = (TetrisPiece.PieceType)random.Next(1, 13);
            return new TetrisPiece(type);
        }

        /// <summary>
        /// 新しいピースを出現させる
        /// </summary>
        private void SpawnNewPiece()
        {
            CurrentPiece = NextPiece;
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
        /// 1フレーム分の更新（自然落下）
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
        /// 左に移動
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
        /// 右に移動
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
        /// 下に移動
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
        /// 回転
        /// </summary>
        public bool Rotate()
        {
            if (IsGameOver || IsPaused) return false;

            CurrentPiece.Rotate();
            if (Board.CheckCollision(CurrentPiece))
            {
                CurrentPiece.RotateBack();
                return false;
            }
            OnBoardChanged?.Invoke();
            return true;
        }

        /// <summary>
        /// ハードドロップ
        /// </summary>
        public void HardDrop()
        {
            if (IsGameOver || IsPaused) return;

            int dropDistance = 0;
            while (MoveDown())
            {
                dropDistance++;
            }
            
            // ハードドロップボーナス
            Score += dropDistance * 2;
            
            LockPiece();
            OnScoreChanged?.Invoke();
        }

        /// <summary>
        /// ホールド機能
        /// </summary>
        public void Hold()
        {
            if (IsGameOver || IsPaused || !canHold) return;

            if (HoldPiece == null)
            {
                HoldPiece = new TetrisPiece(CurrentPiece.Type);
                SpawnNewPiece();
            }
            else
            {
                TetrisPiece temp = new TetrisPiece(CurrentPiece.Type);
                CurrentPiece = new TetrisPiece(HoldPiece.Type);
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
        /// ピースを固定
        /// </summary>
        private void LockPiece()
        {
            Board.PlacePiece(CurrentPiece);
            
            int linesCleared = Board.ClearLines();
            if (linesCleared > 0)
            {
                Lines += linesCleared;
                
                // スコア計算（1ライン: 100, 2ライン: 300, 3ライン: 500, 4ライン: 800）
                int[] lineScores = { 0, 100, 300, 500, 800 };
                Score += lineScores[Math.Min(linesCleared, 4)] * Level;
                
                // レベルアップ（10ライン毎）
                Level = Lines / 10 + 1;
                
                OnScoreChanged?.Invoke();
            }

            SpawnNewPiece();
            OnBoardChanged?.Invoke();
        }

        /// <summary>
        /// ゴーストピース（落下予測位置）を取得
        /// </summary>
        public TetrisPiece GetGhostPiece()
        {
            TetrisPiece ghost = CurrentPiece.Clone();
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
        /// 落下速度を取得（ミリ秒）
        /// </summary>
        public double GetDropSpeed()
        {
            // スコアに応じて速度を上げる
            // 0点: 1000ms, 1000点: 500ms, 5000点: 200ms, 10000点以上: 100ms
            double baseSpeed = 1000;
            double speedReduction = Score / 100.0; // 100点ごとに10ms速くなる
            return Math.Max(100, baseSpeed - speedReduction);
        }
    }
}
