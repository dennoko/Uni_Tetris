using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using System;
using System.Collections.Generic;

namespace UniBlocks
{
    /// <summary>
    /// UniBlocksエチE��タウィンドウ
    /// </summary>
    public class UniBlocksWindow : EditorWindow
    {
        private UniBlocksGame game;
        private VisualElement boardContainer;
        private VisualElement nextPieceContainer;
        private VisualElement holdPieceContainer;
        private Label scoreLabel;
        private Label linesLabel;
        private Label levelLabel;
        private Label gameOverLabel;
        private Label pauseLabel;
        private VisualElement[,] cells;
        
        private double lastUpdateTime;
        private const string HighScoreKey = "UniBlocks_HighScore";
        private KeyBindingConfig keyConfig;
        private Label highScoreValueLabel;
        private bool wasPausedByFocusLoss = false;

        [MenuItem("Window/Uni Blocks")]
        public static void ShowWindow()
        {
            UniBlocksWindow window = GetWindow<UniBlocksWindow>();
            window.titleContent = new GUIContent("Uni Blocks");
            window.minSize = new Vector2(450, 700);
        }

        // メニューからハイスコアをリセットするオプションを追加
        [MenuItem("Window/Uni Blocks/Reset High Score")]
        public static void ResetHighScoreMenu()
        {
            if (EditorUtility.DisplayDialog(
                "Reset High Score",
                "ハイスコアを本当に消去しますか？\n(元に戻せません)",
                "削除",
                "キャンセル"))
            {
                EditorPrefs.DeleteKey(HighScoreKey);
                // 開いているウィンドウがあれば表示更新
                UniBlocksWindow w = GetWindow<UniBlocksWindow>();
                w?.UpdateHighScoreDisplay();
            }
        }

        private void OnEnable()
        {
            keyConfig = KeyBindingConfig.Instance;
            
            game = new UniBlocksGame();
            game.OnBoardChanged += RefreshBoard;
            game.OnScoreChanged += RefreshScore;
            game.OnGameOver += OnGameOver;
            game.OnPieceChanged += RefreshNextAndHold;

            EditorApplication.update += GameUpdate;
            lastUpdateTime = EditorApplication.timeSinceStartup;
        }

        private void OnDisable()
        {
            EditorApplication.update -= GameUpdate;
            
            if (game != null)
            {
                game.OnBoardChanged -= RefreshBoard;
                game.OnScoreChanged -= RefreshScore;
                game.OnGameOver -= OnGameOver;
                game.OnPieceChanged -= RefreshNextAndHold;
            }
            
            wasPausedByFocusLoss = false;
        }

        private void CreateGUI()
        {
            // ルート要素
            VisualElement root = rootVisualElement;
            root.styleSheets.Add(AssetDatabase.LoadAssetAtPath<StyleSheet>(
                "Assets/Editor/Uni_Blocks/UniBlocks.uss"));

            // IMGUIコンチE��でキーイベントを処琁E
            var imguiContainer = new IMGUIContainer(() =>
            {
                HandleKeyInput();
            });
            imguiContainer.style.position = Position.Absolute;
            imguiContainer.style.width = 0;
            imguiContainer.style.height = 0;
            imguiContainer.focusable = true;
            root.Add(imguiContainer);

            // メインコンチE��
            VisualElement mainContainer = new VisualElement();
            mainContainer.AddToClassList("main-container");
            root.Add(mainContainer);

            // 左側�E�ゲームボ�EチE
            VisualElement leftPanel = new VisualElement();
            leftPanel.AddToClassList("left-panel");
            mainContainer.Add(leftPanel);

            boardContainer = new VisualElement();
            boardContainer.AddToClassList("board-container");
            leftPanel.Add(boardContainer);

            // ボ�Eド�Eセルを作�E
            cells = new VisualElement[UniBlocksBoard.Height, UniBlocksBoard.Width];
            for (int y = 0; y < UniBlocksBoard.Height; y++)
            {
                VisualElement row = new VisualElement();
                row.AddToClassList("board-row");
                boardContainer.Add(row);

                for (int x = 0; x < UniBlocksBoard.Width; x++)
                {
                    VisualElement cell = new VisualElement();
                    cell.AddToClassList("board-cell");
                    cell.AddToClassList("cell-empty");
                    cells[y, x] = cell;
                    row.Add(cell);
                }
            }

            // 右側�E�情報パネル
            VisualElement rightPanel = new VisualElement();
            rightPanel.AddToClassList("right-panel");
            mainContainer.Add(rightPanel);

            // スコア表示
            VisualElement scorePanel = CreateInfoPanel("SCORE");
            scoreLabel = scorePanel.Q<Label>("value");
            rightPanel.Add(scorePanel);

            // ライン数表示
            VisualElement linesPanel = CreateInfoPanel("LINES");
            linesLabel = linesPanel.Q<Label>("value");
            rightPanel.Add(linesPanel);

            // レベル表示
            VisualElement levelPanel = CreateInfoPanel("LEVEL");
            levelLabel = levelPanel.Q<Label>("value");
            rightPanel.Add(levelPanel);

            // ハイスコア表示
            VisualElement highScorePanel = CreateInfoPanel("HIGH SCORE");
            highScoreValueLabel = highScorePanel.Q<Label>("value");
            UpdateHighScoreDisplay();
            rightPanel.Add(highScorePanel);

            // Next表示
            VisualElement nextPanel = new VisualElement();
            nextPanel.AddToClassList("piece-panel");
            Label nextLabel = new Label("NEXT");
            nextLabel.AddToClassList("panel-title");
            nextPanel.Add(nextLabel);
            nextPieceContainer = new VisualElement();
            nextPieceContainer.AddToClassList("piece-container");
            nextPanel.Add(nextPieceContainer);
            rightPanel.Add(nextPanel);

            // Hold表示
            VisualElement holdPanel = new VisualElement();
            holdPanel.AddToClassList("piece-panel");
            Label holdLabel = new Label("HOLD");
            holdLabel.AddToClassList("panel-title");
            holdPanel.Add(holdLabel);
            holdPieceContainer = new VisualElement();
            holdPieceContainer.AddToClassList("piece-container");
            holdPanel.Add(holdPieceContainer);
            rightPanel.Add(holdPanel);

            // 操作説昁E
            VisualElement controlsPanel = new VisualElement();
            controlsPanel.AddToClassList("controls-panel");
            Label controlsTitle = new Label("CONTROLS");
            controlsTitle.AddToClassList("panel-title");
            controlsPanel.Add(controlsTitle);
            
            controlsPanel.Add(new Label($"{keyConfig.keyBindings.moveLeft} / {keyConfig.keyBindings.moveRight} : Move"));
            controlsPanel.Add(new Label($"{keyConfig.keyBindings.softDrop} : Soft Drop"));
            controlsPanel.Add(new Label($"{keyConfig.keyBindings.rotate} : Rotate"));
            controlsPanel.Add(new Label($"{keyConfig.keyBindings.hardDrop} : Hard Drop"));
            controlsPanel.Add(new Label($"{keyConfig.keyBindings.hold} : Hold"));
            controlsPanel.Add(new Label($"{keyConfig.keyBindings.pause} : Pause"));
            controlsPanel.Add(new Label($"{keyConfig.keyBindings.restart} : Restart"));
            rightPanel.Add(controlsPanel);

            // ゲームオーバ�E表示
            gameOverLabel = new Label("GAME OVER\nPress R to Restart");
            gameOverLabel.AddToClassList("game-over-label");
            gameOverLabel.style.display = DisplayStyle.None;
            root.Add(gameOverLabel);

            // 一時停止表示
            pauseLabel = new Label("PAUSED");
            pauseLabel.AddToClassList("pause-label");
            pauseLabel.style.display = DisplayStyle.None;
            root.Add(pauseLabel);

            // 初期表示
            RefreshBoard();
            RefreshScore();
            RefreshNextAndHold();

            // IMGUIコンチE��にフォーカスを設宁E
            imguiContainer.Focus();
        }

        private VisualElement CreateInfoPanel(string title)
        {
            VisualElement panel = new VisualElement();
            panel.AddToClassList("info-panel");
            
            Label titleLabel = new Label(title);
            titleLabel.AddToClassList("panel-title");
            panel.Add(titleLabel);
            
            Label valueLabel = new Label("0");
            valueLabel.AddToClassList("panel-value");
            valueLabel.name = "value";
            panel.Add(valueLabel);
            
            return panel;
        }

        private void HandleKeyInput()
        {
            if (game == null || keyConfig == null) return;

            Event e = Event.current;
            if (e.type == EventType.KeyDown)
            {
                if (e.keyCode == keyConfig.GetKeyCode("moveLeft"))
                {
                    game.MoveLeft();
                    e.Use();
                }
                else if (e.keyCode == keyConfig.GetKeyCode("moveRight"))
                {
                    game.MoveRight();
                    e.Use();
                }
                else if (e.keyCode == keyConfig.GetKeyCode("softDrop"))
                {
                    game.MoveDown();
                    e.Use();
                }
                else if (e.keyCode == keyConfig.GetKeyCode("rotate"))
                {
                    game.Rotate();
                    e.Use();
                }
                else if (e.keyCode == keyConfig.GetKeyCode("hardDrop"))
                {
                    game.HardDrop();
                    e.Use();
                }
                else if (e.keyCode == keyConfig.GetKeyCode("hold"))
                {
                    game.Hold();
                    e.Use();
                }
                else if (e.keyCode == keyConfig.GetKeyCode("pause"))
                {
                    TogglePause();
                    e.Use();
                }
                else if (e.keyCode == keyConfig.GetKeyCode("restart"))
                {
                    RestartGame();
                    e.Use();
                }
            }
        }

        private void GameUpdate()
        {
            if (game == null || game.IsGameOver || game.IsPaused) return;

            double currentTime = EditorApplication.timeSinceStartup;
            double deltaTime = (currentTime - lastUpdateTime) * 1000; // ミリ秒に変換

            if (deltaTime >= game.GetDropSpeed())
            {
                game.Step();
                lastUpdateTime = currentTime;
            }
        }

        private void RefreshBoard()
        {
            if (cells == null || game == null) return;

            // ボ�Eド�E状態を取征E
            int[,] grid = game.Board.GetGridCopy();

            // ゴーストピースを取征E
            UniBlocksPiece ghost = game.GetGhostPiece();
            bool[,] ghostCells = new bool[UniBlocksBoard.Height, UniBlocksBoard.Width];
            if (ghost != null)
            {
                int[][] ghostShape = ghost.GetShape();
                for (int y = 0; y < ghostShape.Length; y++)
                {
                    for (int x = 0; x < ghostShape[y].Length; x++)
                    {
                        if (ghostShape[y][x] != 0)
                        {
                            int boardY = ghost.Y + y;
                            int boardX = ghost.X + x;
                            if (boardY >= 0 && boardY < UniBlocksBoard.Height && 
                                boardX >= 0 && boardX < UniBlocksBoard.Width)
                            {
                                ghostCells[boardY, boardX] = true;
                            }
                        }
                    }
                }
            }

            // 現在のピ�Eスを描画用に追加
            if (game.CurrentPiece != null)
            {
                int[][] shape = game.CurrentPiece.GetShape();
                for (int y = 0; y < shape.Length; y++)
                {
                    for (int x = 0; x < shape[y].Length; x++)
                    {
                        if (shape[y][x] != 0)
                        {
                            int boardY = game.CurrentPiece.Y + y;
                            int boardX = game.CurrentPiece.X + x;
                            if (boardY >= 0 && boardY < UniBlocksBoard.Height && 
                                boardX >= 0 && boardX < UniBlocksBoard.Width)
                            {
                                grid[boardY, boardX] = (int)game.CurrentPiece.Type;
                            }
                        }
                    }
                }
            }

            // セルを更新
            for (int y = 0; y < UniBlocksBoard.Height; y++)
            {
                for (int x = 0; x < UniBlocksBoard.Width; x++)
                {
                    VisualElement cell = cells[y, x];
                    
                    // 既存�Eクラスをクリア
                    cell.ClearClassList();
                    cell.AddToClassList("board-cell");

                    int cellValue = grid[y, x];
                    
                    if (cellValue == 0)
                    {
                        if (ghostCells[y, x])
                        {
                            cell.AddToClassList("cell-ghost");
                        }
                        else
                        {
                            cell.AddToClassList("cell-empty");
                        }
                    }
                    else
                    {
                        cell.AddToClassList($"cell-type-{cellValue}");
                    }
                }
            }
        }

        private void RefreshScore()
        {
            if (game == null) return;

            scoreLabel.text = game.Score.ToString();
            linesLabel.text = game.Lines.ToString();
            levelLabel.text = game.Level.ToString();
            
            // ベストスコアをリアルタイム更新
            UpdateHighScoreDisplay();
        }

        private void RefreshNextAndHold()
        {
            if (game == null) return;

            // Next表示
            DrawPiecePreview(nextPieceContainer, game.NextPiece);

            // Hold表示
            DrawPiecePreview(holdPieceContainer, game.HoldPiece);
        }

        private void DrawPiecePreview(VisualElement container, UniBlocksPiece piece)
        {
            container.Clear();

            if (piece == null) return;

            int[][] shape = piece.GetShape();
            
            for (int y = 0; y < shape.Length; y++)
            {
                VisualElement row = new VisualElement();
                row.AddToClassList("piece-row");
                container.Add(row);

                for (int x = 0; x < shape[y].Length; x++)
                {
                    VisualElement cell = new VisualElement();
                    cell.AddToClassList("piece-cell");
                    
                    if (shape[y][x] != 0)
                    {
                        cell.AddToClassList($"cell-type-{(int)piece.Type}");
                    }
                    else
                    {
                        cell.AddToClassList("cell-empty");
                    }
                    
                    row.Add(cell);
                }
            }
        }

        private void OnGameOver()
        {
            if (gameOverLabel != null)
            {
                gameOverLabel.style.display = DisplayStyle.Flex;
            }

            // ハイスコア更新
            int currentHighScore = EditorPrefs.GetInt(HighScoreKey, 0);
            if (game.Score > currentHighScore)
            {
                EditorPrefs.SetInt(HighScoreKey, game.Score);
                UpdateHighScoreDisplay();
            }
        }

        private void TogglePause()
        {
            if (game.IsGameOver) return;

            game.IsPaused = !game.IsPaused;
            
            if (pauseLabel != null)
            {
                pauseLabel.style.display = game.IsPaused ? DisplayStyle.Flex : DisplayStyle.None;
            }
        }

        private void RestartGame()
        {
            game.Initialize();
            lastUpdateTime = EditorApplication.timeSinceStartup;
            
            if (gameOverLabel != null)
            {
                gameOverLabel.style.display = DisplayStyle.None;
            }
            
            if (pauseLabel != null)
            {
                pauseLabel.style.display = DisplayStyle.None;
            }

            RefreshBoard();
            RefreshScore();
            RefreshNextAndHold();

            // フォーカスを戻ぁE
            var imguiContainer = rootVisualElement.Q<IMGUIContainer>();
            if (imguiContainer != null)
            {
                imguiContainer.Focus();
            }
        }

        private void OnFocus()
        {
            // ウィンドウがフォーカスを得たらIMGUIコンチE��にフォーカス
            if (rootVisualElement != null)
            {
                var imguiContainer = rootVisualElement.Q<IMGUIContainer>();
                if (imguiContainer != null)
                {
                    imguiContainer.Focus();
                }
            }
            
            // フォーカス喪失で一時停止してぁE��場合�E再開
            if (wasPausedByFocusLoss && game != null && !game.IsGameOver)
            {
                game.IsPaused = false;
                if (pauseLabel != null)
                {
                    pauseLabel.style.display = DisplayStyle.None;
                }
                wasPausedByFocusLoss = false;
            }
        }
        
        private void OnLostFocus()
        {
            // フォーカスを失ったら自動的に一時停止
            if (game != null && !game.IsGameOver && !game.IsPaused)
            {
                game.IsPaused = true;
                if (pauseLabel != null)
                {
                    pauseLabel.style.display = DisplayStyle.Flex;
                }
                wasPausedByFocusLoss = true;
            }
        }
        
        /// <summary>
        /// ハイスコア表示を更新
        /// </summary>
        public void UpdateHighScoreDisplay()
        {
            if (highScoreValueLabel == null) return;
            
            int bestScore = EditorPrefs.GetInt(HighScoreKey, 0);
            int currentScore = game != null ? game.Score : 0;
            
            // 現在のスコアとベストスコアを表示
            if (currentScore > bestScore)
            {
                highScoreValueLabel.text = $"{currentScore}\n(best: {currentScore})";
            }
            else
            {
                highScoreValueLabel.text = bestScore > 0 ? $"{currentScore}\n(best: {bestScore})" : "0";
            }
        }
    }
}
