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
        // �e���C�����擾���A���_�����i�[����z������B
        TerrainData genTerrain = GetComponent<Terrain>().terrainData;
        var heights = new float[genTerrain.heightmapResolution, genTerrain.heightmapResolution];

        // �e���C�����ʂ��p�[�����m�C�Y�ɂ���ė��N������B
        for (int x = 0; x < genTerrain.heightmapResolution; x++)
        {
            for (int y = 0; y < genTerrain.heightmapResolution; y++)
            {
                // Terrain�̍������Z�b�g
                heights[x, y] = perlinNoiseHeight(x, y);
            }
        }
        // �e���C���ɒ��_���𔽉f
        genTerrain.SetHeights(0, 0, heights);
    }

    // 襘H�ǉ��T���v��
    private float perlinNoiseHeight(int x, int y)
    {
        // �p�[�����m�C�Y���獂���̃x�[�X���Z�o
        float height = Mathf.PerlinNoise(x * freq + seed, y * freq + (seed / 2));

        // h=0.5 �ŒJ���𔽓]����
        height -= 0.5f;
        if (height < 0f)
        {
            height *= -1;
        }

        height *= -1;
        height += 0.5f;

        // �p�[�����m�C�Y�����H 0.35�ȉ������ׂ�0.35�ɂ���B
        if (height < 0.35f) height = 0.35f;

        // �����ЂƂ̃p�[�����m�C�Y�����Ƃ���͈͂ƂȂ����ꍇ�̓t���b�g�ɂ���
        float roadWavet = Mathf.PerlinNoise(x * freq / 3 + seed, y * freq / 3 + (seed / 2));
        if (roadWavet > 0.45f && roadWavet < 0.5f) height = 0.35f;

        return height * scale;
    }
}