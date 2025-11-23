# Uni Tetris

Unity エディタ拡張として動作する暇つぶし用テトリスゲーム。エディタのEdit Modeで動作し、思考整理や休憩時間に気軽に遊べます。

## 特徴

- **Unity エディタ内で動作**: Play Modeに入る必要なく、Edit Modeでプレイ可能
- **UI Toolkit使用**: モダンでレスポンシブなUI
- **カスタマイズ可能なキーバインド**: config.jsonでキー設定を自由に変更可能
- **完全なテトリス機能**: 
  - 7種類のテトリミノ（I, O, T, S, Z, J, L）
  - Next/Hold機能
  - レベル制（10ライン毎に速度上昇）
  - スコアシステム
  - ハイスコア保存
  - ゴーストピース表示

## インストール

1. `Assets/Editor/Uni_Tetris/` フォルダをUnityプロジェクトにコピー
2. Unityエディタで `Window > Uni Tetris` を選択
3. ゲームウィンドウが開きます

## デフォルト操作

| キー | 操作 |
|------|------|
| ← → | ピースを左右に移動 |
| ↓ | ソフトドロップ（1マスずつ下に移動） |
| ↑ | ピースを回転 |
| Space | ハードドロップ（一番下まで落とす） |
| C | ホールド（ピースを保管） |
| P | 一時停止 |
| R | リスタート |

## キーバインドのカスタマイズ

`Assets/Editor/Uni_Tetris/config.json` を編集することで、キーバインドを変更できます。

### config.json の編集例

```json
{
  "keyBindings": {
    "moveLeft": "A",
    "moveRight": "D",
    "softDrop": "S",
    "rotate": "W",
    "hardDrop": "Space",
    "hold": "LeftShift",
    "pause": "Escape",
    "restart": "R"
  }
}
```

設定を変更したら、Uni Tetrisウィンドウを開き直すと反映されます。

## 利用可能なキー名一覧

### アルファベットキー
```
A, B, C, D, E, F, G, H, I, J, K, L, M, N, O, P, Q, R, S, T, U, V, W, X, Y, Z
```

### 数字キー
```
Alpha0, Alpha1, Alpha2, Alpha3, Alpha4, Alpha5, Alpha6, Alpha7, Alpha8, Alpha9
```
※テンキー: `Keypad0` ～ `Keypad9`

### 矢印キー
```
UpArrow, DownArrow, LeftArrow, RightArrow
```

### 修飾キー
```
LeftShift, RightShift
LeftControl, RightControl
LeftAlt, RightAlt
LeftCommand, RightCommand (Macのみ)
```

### ファンクションキー
```
F1, F2, F3, F4, F5, F6, F7, F8, F9, F10, F11, F12, F13, F14, F15
```

### 特殊キー
```
Space           スペースキー
Return          Enterキー
Escape          Escapeキー
Backspace       バックスペースキー
Delete          Deleteキー
Tab             Tabキー
CapsLock        CapsLockキー
```

### テンキー
```
Keypad0, Keypad1, Keypad2, Keypad3, Keypad4
Keypad5, Keypad6, Keypad7, Keypad8, Keypad9
KeypadPeriod    テンキーの . (ピリオド)
KeypadDivide    テンキーの / (割り算)
KeypadMultiply  テンキーの * (掛け算)
KeypadMinus     テンキーの - (引き算)
KeypadPlus      テンキーの + (足し算)
KeypadEnter     テンキーのEnter
KeypadEquals    テンキーの = (等号)
```

### ナビゲーションキー
```
Insert          Insertキー
Home            Homeキー
End             Endキー
PageUp          PageUpキー
PageDown        PageDownキー
```

### その他よく使うキー
```
Comma           , (カンマ)
Period          . (ピリオド)
Slash           / (スラッシュ)
Backslash       \ (バックスラッシュ)
Semicolon       ; (セミコロン)
Quote           ' (クォート)
LeftBracket     [ (左ブラケット)
RightBracket    ] (右ブラケット)
Minus           - (マイナス)
Equals          = (等号)
Backquote       ` (バッククォート)
```

### マウスボタン（使用は非推奨）
```
Mouse0          左クリック
Mouse1          右クリック
Mouse2          中クリック
Mouse3, Mouse4, Mouse5, Mouse6  追加マウスボタン
```

### 完全なリスト

上記以外のキーコードについては、[Unity公式ドキュメント - KeyCode](https://docs.unity3d.com/ScriptReference/KeyCode.html) を参照してください。

## ファイル構成

```
Assets/Editor/Uni_Tetris/
├── TetrisWindow.cs        エディタウィンドウ（View）
├── TetrisGame.cs          ゲームロジック（Model）
├── TetrisBoard.cs         盤面管理
├── TetrisPiece.cs         ピース定義
├── KeyBindingConfig.cs    キーバインド設定管理
├── config.json            キーバインド設定ファイル
├── Tetris.uss             スタイルシート
└── prompt.md              開発仕様書
```

## ゲームルール

### スコア計算
- **1ライン消去**: 100点 × レベル
- **2ライン消去**: 300点 × レベル
- **3ライン消去**: 500点 × レベル
- **4ライン消去**: 800点 × レベル
- **ハードドロップ**: 落下距離 × 2点

### レベルシステム
- 10ライン消去ごとにレベルアップ
- レベルが上がると落下速度が上昇
- レベル1: 1秒/1マス → レベル10: 0.1秒/1マス

### ゲームオーバー
- 新しいピースが出現位置に配置できない場合、ゲームオーバー
- Rキーでリスタート可能

## ハイスコア

ハイスコアは `EditorPrefs` に自動保存されます。プロジェクトを閉じても記録は保持されます。

## ライセンス

このプロジェクトはMITライセンスの下で公開されています。

## 開発者向け情報

### アーキテクチャ
- **MVVM パターン**を採用
- **Model**: 純粋なC#クラス（TetrisGame, TetrisBoard, TetrisPiece）
- **View**: UI Toolkit（TetrisWindow）
- **ViewModel**: EditorApplication.updateによるゲームループ

### カスタマイズ
- ピースの色は `Tetris.uss` で変更可能
- ゲームルール（落下速度、スコア倍率など）は `TetrisGame.cs` で調整可能

## トラブルシューティング

### キーが反応しない
- ウィンドウがフォーカスされているか確認してください
- ウィンドウを一度閉じて開き直してください

### config.jsonの変更が反映されない
- Uni Tetrisウィンドウを閉じて再度開いてください
- config.jsonの構文エラーがないか確認してください（JSONは厳密な形式が必要です）

### ハイスコアをリセットしたい
Unityエディタのメニューから:
- **Windows**: `Edit > Preferences > Clear All Preferences`（全設定がクリアされます）
- **Mac**: `Unity > Preferences > Clear All Preferences`

または、スクリプトから:
```csharp
EditorPrefs.DeleteKey("UniTetris_HighScore");
```

## 更新履歴

### v1.0.0
- 初回リリース
- 基本的なテトリス機能
- config.jsonによるキーバインドカスタマイズ
- UI Toolkitベースの描画
