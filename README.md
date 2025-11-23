# Uni Blocks

Unity エチE��タ拡張として動作する暇つぶし用パズルゲーム。エチE��タのEdit Modeで動作し、思老E��琁E��休�E時間に気軽に遊べます、E

## 特徴

- **Unity エチE��タ冁E��動佁E*: Play Modeに入る忁E��なく、Edit Modeでプレイ可能
- **UI Toolkit使用**: モダンでレスポンシブなUI
- **カスタマイズ可能なキーバインチE*: config.jsonでキー設定を自由に変更可能
- **完�Eなパズル機�E**: 
  - 3マス + 5マスの混合ピース�E�訁E種類！E
  - 3マスL孁EÁE1種顁E
  - 5マスピ�Eス ÁE8種類！E, T, U, F, F鏡僁E W, W鏡僁E X�E�E
  - Next/Hold機�E
  - スコアに応じて落下速度が上�E
  - スコアシスチE��
  - ベストスコア保孁E
  - ゴーストピース表示
  - フォーカス喪失時�E自動一時停止

## ゲーム仕槁E

- **フィールドサイズ**: 12刁EÁE25衁E
- **ピ�Eス構�E**: 
  - **3マス**: L孁E(比輁E��簡単な配置)
  - **5マス**: I(直緁E, T(T孁E, U(U孁E, F(F孁E, F鏡僁E W(W孁E, W鏡僁E X(十孁E
- **目皁E*: ラインを揃えて消去し、E��得点を目持E��

## インスト�Eル

1. `Assets/Editor/Uni_Blocks/` フォルダをUnityプロジェクトにコピ�E
2. UnityエチE��タで `Window > Uni Blocks` を選抁E
3. ゲームウィンドウが開きまぁE

## チE��ォルト操佁E

| キー | 操佁E|
|------|------|
| ↁEↁE| ピ�Eスを左右に移勁E|
| ↁE| ソフトドロチE�E�E�Eマスずつ下に移動！E|
| ↁE| ピ�Eスを回転 |
| Space | ハ�EドドロチE�E�E�一番下まで落とす！E|
| C | ホ�Eルド（ピースを保管�E�E|
| P | 一時停止 |
| R | リスターチE|

## キーバインド�Eカスタマイズ

`Assets/Editor/Uni_Blocks/config.json` を編雁E��ることで、キーバインドを変更できます、E

### config.json の編雁E��E

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

設定を変更したら、Uni Blocksウィンドウを開き直すと反映されます、E

## 利用可能なキー名一覧

### アルファベットキー
```
A, B, C, D, E, F, G, H, I, J, K, L, M, N, O, P, Q, R, S, T, U, V, W, X, Y, Z
```

### 数字キー
```
Alpha0, Alpha1, Alpha2, Alpha3, Alpha4, Alpha5, Alpha6, Alpha7, Alpha8, Alpha9
```
※チE��キー: `Keypad0` �E�E`Keypad9`

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
Space           スペ�Eスキー
Return          Enterキー
Escape          Escapeキー
Backspace       バックスペ�Eスキー
Delete          Deleteキー
Tab             Tabキー
CapsLock        CapsLockキー
```

### チE��キー
```
Keypad0, Keypad1, Keypad2, Keypad3, Keypad4
Keypad5, Keypad6, Keypad7, Keypad8, Keypad9
KeypadPeriod    チE��キーの . (ピリオチE
KeypadDivide    チE��キーの / (割り箁E
KeypadMultiply  チE��キーの * (掛け箁E
KeypadMinus     チE��キーの - (引き箁E
KeypadPlus      チE��キーの + (足し箁E
KeypadEnter     チE��キーのEnter
KeypadEquals    チE��キーの = (等号)
```

### ナビゲーションキー
```
Insert          Insertキー
Home            Homeキー
End             Endキー
PageUp          PageUpキー
PageDown        PageDownキー
```

### そ�E他よく使ぁE��ー
```
Comma           , (カンチE
Period          . (ピリオチE
Slash           / (スラチE��ュ)
Backslash       \ (バックスラチE��ュ)
Semicolon       ; (セミコロン)
Quote           ' (クォーチE
LeftBracket     [ (左ブラケチE��)
RightBracket    ] (右ブラケチE��)
Minus           - (マイナス)
Equals          = (等号)
Backquote       ` (バッククォーチE
```

### マウスボタン�E�使用は非推奨�E�E
```
Mouse0          左クリチE��
Mouse1          右クリチE��
Mouse2          中クリチE��
Mouse3, Mouse4, Mouse5, Mouse6  追加マウスボタン
```

### 完�EなリスチE

上記以外�EキーコードにつぁE��は、[Unity公式ドキュメンチE- KeyCode](https://docs.unity3d.com/ScriptReference/KeyCode.html) を参照してください、E

## ファイル構�E

```
Assets/Editor/Uni_Blocks/
├── UniBlocksWindow.cs        エチE��タウィンドウ�E�Eiew�E�E
├── UniBlocksGame.cs          ゲームロジチE���E�Eodel�E�E
├── UniBlocksBoard.cs         盤面管琁E
├── UniBlocksPiece.cs         ピ�Eス定義
├── KeyBindingConfig.cs    キーバインド設定管琁E
├── config.json            キーバインド設定ファイル
├── UniBlocks.uss             スタイルシーチE
└── prompt.md              開発仕様書
```

## ゲームルール

### スコア計箁E
- **1ライン消去**: 100点 ÁEレベル
- **2ライン消去**: 300点 ÁEレベル
- **3ライン消去**: 500点 ÁEレベル
- **4ライン消去**: 800点 ÁEレベル
- **5ライン消去**: 1200点 ÁEレベル
- **ハ�EドドロチE�E**: 落下距離 ÁE2点

### スピ�EドシスチE��
- スコアが上がるほど落下速度が速くなりまぁE
- 0点: 1.0私E1マス
- 1000点: 0.5私E1マス
- 5000点: 0.2私E1マス
- 10000点以丁E 0.1私E1マス�E�最速！E

### ゲームオーバ�E
- 新しいピ�Eスが�E現位置に配置できなぁE��合、ゲームオーバ�E
- Rキーでリスタート可能

## ハイスコア

ベストスコアは `EditorPrefs` に自動保存されます。�Eロジェクトを閉じても記録は保持されます、E
HIGH SCOREパネルには現在のスコアとベストスコアが両方表示されます、E

## ライセンス

こ�Eプロジェクト�EMITライセンスの下で公開されてぁE��す、E

## 開発老E��け情報

### アーキチE��チャ
- **MVVM パターン**を採用
- **Model**: 純粋なC#クラス�E�EniBlocksGame, UniBlocksBoard, UniBlocksPiece�E�E
- **View**: UI Toolkit�E�EniBlocksWindow�E�E
- **ViewModel**: EditorApplication.updateによるゲームルーチE

### カスタマイズ
- ピ�Eスの色は `UniBlocks.uss` で変更可能�E�E種類�Eピ�Eスに対応！E
- ゲームルール�E�落下速度、スコア倍率など�E��E `UniBlocksGame.cs` で調整可能
- フィールドサイズは `UniBlocksBoard.cs` で変更可能�E�現在: 12ÁE5�E�E
- ピ�Eス構�Eは `UniBlocksPiece.cs` で変更可能�E�現在: 3マスÁE + 5マスÁE�E�E

## トラブルシューチE��ング

### キーが反応しなぁE
- ウィンドウがフォーカスされてぁE��か確認してください
- ウィンドウを一度閉じて開き直してください

### config.jsonの変更が反映されなぁE
- Uni Blocksウィンドウを閉じて再度開いてください
- config.jsonの構文エラーがなぁE��確認してください�E�ESONは厳寁E��形式が忁E��です！E

### ハイスコアをリセチE��したぁE
UnityエチE��タのメニューから:
- **Windows**: `Edit > Preferences > Clear All Preferences`�E��E設定がクリアされます！E
- **Mac**: `Unity > Preferences > Clear All Preferences`

また�E、スクリプトから:
```csharp
EditorPrefs.DeleteKey("UniBlocks_HighScore");
```

## 更新履歴

### v2.1.0
- **難易度調整**: 3マス + 5マスの混合ピースに変更
- **ピ�Eス厳選**: 扱ぁE��すい形状に絞り込み�E�E種類！E
- **3マスピ�Eス**: L字型を追加�E�簡単なライン消去に貢献�E�E
- **5マスピ�Eス**: I, T, U, F, W, X + F/W鏡像版を採用
- **バランス改喁E*: 褁E��すぎるピースを削除し、�Eレイしやすく調整

### v2.0.0
- **権利問題対忁E*: 名称を「Uni Blocks」に変更
- **フィールド拡張**: 12ÁE5マスに変更
- **ピ�Eス変更**: 5マスペントミノ！E2種類）を採用
- **スコアベ�Eス速度**: スコアに応じて落下速度が変化
- **自動一時停止**: フォーカス喪失時に自動的に一時停止
- **ベストスコア表示**: 現在のスコアとベストスコアを同時表示

### v1.0.0
- 初回リリース
- 基本皁E��チE��リス機�E
- config.jsonによるキーバインドカスタマイズ
- UI Toolkitベ�Eスの描画
