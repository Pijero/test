# Battle Mood Aura テスト手順（RimWorld 1.5）

## 1. MODの配置
このリポジトリを `RimWorld/Mods/BattleMoodAura` に配置します。

例（Windows）:
- `C:\Program Files (x86)\Steam\steamapps\common\RimWorld\Mods\BattleMoodAura`

## 2. DLLのビルド
`Source/BattleMoodAura` を Visual Studio で開き、`RimWorldWin64Dir` を環境変数に設定してビルドしてください。
出力DLLを `Assemblies/BattleMoodAura.dll` に配置します。

## 3. ゲーム内で有効化
1. RimWorldを起動
2. Mods で **Battle Mood Aura** を有効化
3. 再起動

## 4. 推奨テストシナリオ
- 新規コロニー開始（開発者モードON）
- 入植者A/B/Cを用意
- A-Bの好感を高く、A-Cの好感を低く変更
- A/B/Cを徴兵（Draft）
- Aの近くにBを置く → 士気高揚（弱/中/強）
- Aの近くにCを置く → 恐慌（弱/中/強）
- BとCの人数/距離を調整し段階が変化するか確認

## 5. 確認ポイント
- 設定画面スライダー反映（半径、更新間隔、しきい値）
- 戦闘状態解除（Draft解除）で効果が消える
- 1人にbuff/debuffが同時付与されない
- エラーログ（赤エラー）なし

## 6. トラブルシューティング
- MODが表示されない: `About/About.xml` の場所を確認
- 効果が出ない: DLLが `Assemblies` にあるか確認
- 反映が遅い: 設定の更新間隔（ticks）を短くする
