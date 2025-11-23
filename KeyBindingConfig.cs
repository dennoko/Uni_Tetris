using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;

namespace UniBlocks
{
    /// <summary>
    /// キーバインド設定を管琁E��るクラス
    /// </summary>
    [Serializable]
    public class KeyBindingConfig
    {
        [Serializable]
        public class KeyBindings
        {
            public string moveLeft = "LeftArrow";
            public string moveRight = "RightArrow";
            public string softDrop = "DownArrow";
            public string rotate = "UpArrow";
            public string hardDrop = "Space";
            public string hold = "C";
            public string pause = "P";
            public string restart = "R";
        }

        [Serializable]
        public class Description
        {
            public string moveLeft = "ピースを左に移動";
            public string moveRight = "ピースを右に移動";
            public string softDrop = "ピースを1マス下に移動（ソフトドロップ）";
            public string rotate = "ピースを回転";
            public string hardDrop = "ピースを一番下まで落とす（ハードドロップ）";
            public string hold = "ピースをホールド";
            public string pause = "ゲームを一時停止";
            public string restart = "ゲームをリスタート";
        }

        public KeyBindings keyBindings = new KeyBindings();
        public Description description = new Description();

        private static KeyBindingConfig instance;
        private Dictionary<string, KeyCode> keyCodeCache;

        /// <summary>
        /// 設定�Eシングルトンインスタンスを取征E
        /// </summary>
        public static KeyBindingConfig Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = LoadConfig();
                }
                return instance;
            }
        }

        /// <summary>
        /// 設定を読み込む
        /// </summary>
        private static KeyBindingConfig LoadConfig()
        {
            string configPath = "Assets/Editor/Uni_Blocks/config.json";
            
            if (File.Exists(configPath))
            {
                try
                {
                    string json = File.ReadAllText(configPath);
                    KeyBindingConfig config = JsonUtility.FromJson<KeyBindingConfig>(json);
                    config.BuildKeyCodeCache();
                    Debug.Log("[Uni Blocks] キーバインド設定を読み込みました: " + configPath);
                    return config;
                }
                catch (Exception e)
                {
                    Debug.LogError("[Uni Blocks] config.jsonの読み込みに失敗しました: " + e.Message);
                }
            }
            else
            {
                Debug.LogWarning("[Uni Blocks] config.jsonが見つかりません。デフォルト設定を使用します。");
            }

            // チE��ォルト設定を返す
            KeyBindingConfig defaultConfig = new KeyBindingConfig();
            defaultConfig.BuildKeyCodeCache();
            return defaultConfig;
        }

        /// <summary>
        /// 斁E���EからKeyCodeへの変換キャチE��ュを構篁E
        /// </summary>
        private void BuildKeyCodeCache()
        {
            keyCodeCache = new Dictionary<string, KeyCode>();
            
            keyCodeCache["moveLeft"] = ParseKeyCode(keyBindings.moveLeft);
            keyCodeCache["moveRight"] = ParseKeyCode(keyBindings.moveRight);
            keyCodeCache["softDrop"] = ParseKeyCode(keyBindings.softDrop);
            keyCodeCache["rotate"] = ParseKeyCode(keyBindings.rotate);
            keyCodeCache["hardDrop"] = ParseKeyCode(keyBindings.hardDrop);
            keyCodeCache["hold"] = ParseKeyCode(keyBindings.hold);
            keyCodeCache["pause"] = ParseKeyCode(keyBindings.pause);
            keyCodeCache["restart"] = ParseKeyCode(keyBindings.restart);
        }

        /// <summary>
        /// 斁E���EをKeyCodeに変換
        /// </summary>
        private KeyCode ParseKeyCode(string keyString)
        {
            try
            {
                return (KeyCode)Enum.Parse(typeof(KeyCode), keyString, true);
            }
            catch
            {
                Debug.LogError($"[Uni Blocks] 無効なキー吁E {keyString}");
                return KeyCode.None;
            }
        }

        /// <summary>
        /// 持E��したアクションのKeyCodeを取征E
        /// </summary>
        public KeyCode GetKeyCode(string action)
        {
            if (keyCodeCache != null && keyCodeCache.ContainsKey(action))
            {
                return keyCodeCache[action];
            }
            return KeyCode.None;
        }

        /// <summary>
        /// キーバインド�E説明を取征E
        /// </summary>
        public string GetDescription(string action)
        {
            switch (action)
            {
                case "moveLeft": return description.moveLeft;
                case "moveRight": return description.moveRight;
                case "softDrop": return description.softDrop;
                case "rotate": return description.rotate;
                case "hardDrop": return description.hardDrop;
                case "hold": return description.hold;
                case "pause": return description.pause;
                case "restart": return description.restart;
                default: return "";
            }
        }

        /// <summary>
        /// 設定を再読み込み
        /// </summary>
        public static void Reload()
        {
            instance = null;
        }
    }
}
