using System;

using LibNoise.Primitive;

public class TerrainGen
{
    public static BlockType[][] GenerateWorld(int width, int height, int uneven = 6, float weight = 0.03f)
    {
        BlockType[][] world = new BlockType[height][];
        SimplexPerlin perlin = new SimplexPerlin();

        perlin.Seed = new Random().Next(0, 1000000);

        for (int y = 0; y < height; y++)
        {
            world[y] = new BlockType[width];
            for (int x = 0; x < width; x++)
            {
                double noiseValue = perlin.GetValue((float)(x * weight));
                int terrainHeight = (int)(5 + (noiseValue * uneven)); // Adjust the parameters for terrain height
                if (y > terrainHeight)
                {
                    world[y][x] = BlockType.Stone;

                    if (y > 1 && world[y - 1][x] == BlockType.Air)
                    {
                        world[y][x] = BlockType.Grass;
                    }

                    if (
                        y > 2 && world[y - 1][x] == BlockType.Grass || // grass, air ontop
                        y > 2 && world[y - 2][x] == BlockType.Grass && world[y - 1][x] == BlockType.Dirt || // dirt, grass, air ontop
                        y > 2 && world[y - 3][x] == BlockType.Grass && world[y - 2][x] == BlockType.Dirt && world[y - 1][x] == BlockType.Dirt // dirt, dirt, grass, air ontop
                        )
                    {
                        world[y][x] = BlockType.Dirt;
                    }
                }
                else
                {
                    world[y][x] = BlockType.Air;
                }
            }
        }

        return world;
    }
}