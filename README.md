# Uni Blocks

Unity エディター上（Edit Mode）で動作する軽量パズルゲーム拡張です。Play Modeへ切り替えず、開発の合間・ビルド待ち時間などに気軽に遊べます。

## 主な特徴

- Edit Modeでプレイ可能（再生ボタン不要）
- UI Toolkitによるシンプルで拡張しやすいUI
- キーバインドを `config.json` で自由に差し替え可能
- Hold / Next / Ghost（落下位置プレビュー）対応
- ハイスコア自動保存（`EditorPrefs`）
- フォーカス喪失時に自動一時停止
- 11種類の多彩なピース（3 / 4 / 5ブロック混在）

## 現行ピース一覧

| 種類 | 名前 | ブロック数 | 概要 |
|------|------|------------|------|
| 1 | L3 | 3 | 3マスL字トリオミノ |
| 2 | P5 | 5 | 凸型（Pentomino P） |
| 3 | P5_Mirror | 5 | 凸型の鏡像 |
| 4 | J4 | 4 | 4マスJ字（1111 / 1000）|
| 5 | L4 | 4 | 4マスL字（1000 / 1111）|
| 6 | T4 | 4 | 4マスT字 |
| 7 | I5 | 5 | 5マス直線 |
| 8 | T5 | 5 | 5マスT字（縦長バリエーション）|
| 9 | U5 | 5 | 5マスU字 |
|10 | W5 | 5 | 5マスW字 |
|11 | X5 | 5 | 5マス十字 |

F5 / F5_Mirror はバランス調整のため削除済みです。

## フィールド仕様

- サイズ: 幅12 × 高さ25
- ライン消去: 完成した段は下詰めで連鎖消去可能
- ゴーストピース: 現在形状を最終落下位置に半透明表示
- 衝突判定: 盤面外・既存ブロック・下端衝突で固定

## スコアと進行

| 条件 | 基本スコア（×レベル） |
|------|-------------------------|
| 1ライン | 100 |
| 2ライン | 300 |
| 3ライン | 500 |
| 4ライン | 800 |
| ハードドロップ | 落下距離 × 2 |

- レベル: 総消去ライン数 / 10 + 1
- 落下速度: スコアに応じて短縮（下限 100ms）

## 操作（デフォルト）

| アクション | キー | 説明 |
|-------------|------|------|
| Move Left | LeftArrow | 左へ移動 |
| Move Right | RightArrow | 右へ移動 |
| Soft Drop | DownArrow | 1段落下 |
| Rotate | UpArrow | 右回転 |
| Hard Drop | Space | 最下段まで即落下 |
| Hold | C | ピースを保持/交換 |
| Pause | P | 一時停止切替 |
| Restart | R | リスタート |

キーは `Assets/Editor/Uni_Blocks/config.json` を編集して変更できます。

### config.json 例

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

変更後はウィンドウを開き直すと反映されます。

## 使い方

1. このフォルダ `Assets/Editor/Uni_Blocks/` をプロジェクトへ配置
2. メニュー `Window > Uni Blocks` からウィンドウを開く
3. ウィンドウにフォーカスがある状態でキーボード操作

## ハイスコア

スコアはゲームオーバー時に `EditorPrefs`（キー: `UniBlocks_HighScore`）へ保存されます。

### リセット方法
1. Unity メニューで `Window > Uni Blocks > Reset High Score` を選択し、確認ダイアログで「削除」を押す。
2. もしくはスクリプトから:
  ```csharp
  UnityEditor.EditorPrefs.DeleteKey("UniBlocks_HighScore");
  ```
3. ウィンドウを開いている場合は表示が自動で更新されます。

## ファイル構成

```
Assets/Editor/Uni_Blocks/
├── UniBlocksWindow.cs    エディターウィンドウ（UI / 入力）
├── UniBlocksGame.cs      ゲーム進行・スコア / レベル
├── UniBlocksBoard.cs     盤面・ライン消去
├── UniBlocksPiece.cs     ピース定義（形状 / 回転）
├── KeyBindingConfig.cs   キーバインド読込
├── config.json           キーバインド設定ファイル
├── UniBlocks.uss         UI Toolkit スタイル
└── prompt.md             仕様メモ
```

## カスタマイズポイント

- 色変更: `UniBlocks.uss` の `.cell-type-*` を編集
- ピース追加/調整: `UniBlocksPiece.cs` の `PieceType` と `Shapes` を編集
- 盤面サイズ変更: `UniBlocksBoard.cs` の `Width` / `Height`
- 落下速度・スコア調整: `UniBlocksGame.cs` 内メソッド

## トラブルシューティング

| 問題 | 対処 |
|------|------|
| キー入力が効かない | ウィンドウをクリックしフォーカスを与える |
| 設定が反映されない | ウィンドウを閉じ再度開く / JSON構文確認 |
| フリーズに見える | 一時停止表示 `PAUSED` を確認（Pキーで解除） |

## 更新履歴

### v2.2.0
- F5 / F5_Mirror を削除し T4 を追加（全11種構成へ）
- ピース色と番号の再整理

### v2.1.0
- ピース種類拡張 / ゴースト表示追加
- ハイスコア表示強化

### v2.0.0
- 名称を Uni Blocks に統一
- フィールドサイズ 12x25 へ拡張

### v1.0.0
- 初期リリース

## ライセンス

MIT License（本リポジトリ内 LICENSE 参照予定）

---
不具合や改善案があればお気軽に Issue / PR してください。
