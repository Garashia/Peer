// K1Togami 2022/10/9
using UnityEngine;

public class AutoGenerateTerrain : MonoBehaviour
{
    public float scale = 0.5f;
    public float freq = 0.01f;
    public float seed = 0;

    [ContextMenu("Generate")]
    private void makeGround()
    {
        // テレインを取得し、頂点情報を格納する配列を作る。
        TerrainData genTerrain = GetComponent<Terrain>().terrainData;
        var heights = new float[genTerrain.heightmapResolution, genTerrain.heightmapResolution];

        // テレイン平面をパーリンノイズによって隆起させる。
        for (int x = 0; x < genTerrain.heightmapResolution; x++)
        {
            for (int y = 0; y < genTerrain.heightmapResolution; y++)
            {
                // Terrainの高さをセット
                heights[x, y] = perlinNoiseHeight(x, y);
            }
        }
        // テレインに頂点情報を反映
        genTerrain.SetHeights(0, 0, heights);
    }

    // 隘路追加サンプル
    private float perlinNoiseHeight(int x, int y)
    {
        // パーリンノイズから高さのベースを算出
        float height = Mathf.PerlinNoise(x * freq + seed, y * freq + (seed / 2));

        // h=0.5 で谷部を反転する
        height -= 0.5f;
        if (height < 0f)
        {
            height *= -1;
        }

        height *= -1;
        height += 0.5f;

        // パーリンノイズを加工 0.35以下をすべて0.35にする。
        if (height < 0.35f) height = 0.35f;

        // もうひとつのパーリンノイズを作りとある範囲となった場合はフラットにする
        float roadWavet = Mathf.PerlinNoise(x * freq / 3 + seed, y * freq / 3 + (seed / 2));
        if (roadWavet > 0.45f && roadWavet < 0.5f) height = 0.35f;

        return height * scale;
    }
}