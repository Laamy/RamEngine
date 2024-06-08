using System;
using System.Collections.Generic;
using System.Drawing;
using LibNoise.Primitive;

public enum WorldType
{
    Default, // vanilla
    Flat
}

public class TerrainGen
{
    public static BlockType[][] GenerateWorld(WorldType type, int width, int height, int uneven = 6, float weight = 0.03f)
    {
        BlockType[][] world = new BlockType[height][];

        switch(type)
        {
            case WorldType.Default:
                _GenerateVanilla(out world, width, height, uneven, weight);
                break;

            case WorldType.Flat:
                _GenerateFlat(out world, width, height);
                break;
        }

        return world;
    }

    internal static void GrowWorld(BlockType[][] world, int blockSize, ClientInstance Instance)
    {
        int layerCount = 100;
        foreach (BlockType[] layer in world)
        {
            int blockCount = 0;
            // foreach terrain block
            foreach (BlockType block in layer)
            {
                switch (block)
                {
                    case BlockType.Stone:
                        {
                            // stone

                            SolidObject obj = new SolidObject(
                                new Point(blockCount, layerCount),
                                new Size(blockSize, blockSize),
                                "assets\\blocks\\stone.png",
                                new List<string>() { "Breakable" }
                            );

                            Instance.GetLevel().children.Add(obj);
                        }
                        break;
                    case BlockType.Grass:
                        {
                            // grass

                            SolidObject obj = new SolidObject(
                                new Point(blockCount, layerCount),
                                new Size(blockSize, blockSize),
                                "assets\\blocks\\grass.png",
                                new List<string>() { "Breakable" }
                            );

                            Instance.GetLevel().children.Add(obj);
                        }
                        break;
                    case BlockType.Dirt:
                        {
                            // dirt

                            SolidObject obj = new SolidObject(
                                new Point(blockCount, layerCount),
                                new Size(blockSize, blockSize),
                                "assets\\blocks\\dirt.png",
                                new List<string>() { "Breakable" }
                            );

                            Instance.GetLevel().children.Add(obj);
                        }
                        break;
                }

                blockCount += blockSize;
            }
            layerCount += blockSize;
        }
    }

    private static void _GenerateFlat(out BlockType[][] world, int width, int height)
    {
        world = new BlockType[height][];

        int flatHeight = 5;

        for (int y = 0; y < height; y++)
        {
            world[y] = new BlockType[width];
            for (int x = 0; x < width; x++)
            {
                if (y > flatHeight)
                {
                    world[y][x] = BlockType.Stone;

                    if (y == flatHeight + 1)
                    {
                        world[y][x] = BlockType.Grass;
                    }
                    else if (y > flatHeight + 1 && y <= flatHeight + 3)
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
    }

    private static void _GenerateVanilla(out BlockType[][] world, int width, int height, int uneven, float weight)
    {
        world = new BlockType[height][];
        SimplexPerlin perlin = new SimplexPerlin(new Random().Next(0, 1000000), LibNoise.NoiseQuality.Best);

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

        // big cave patches
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                double wormNoiseX = perlin.GetValue((float)(x * weight) / 4, (float)(y * weight));
                double wormNoiseY = perlin.GetValue((float)(x * weight), (float)(y * weight) / 2);

                if (world[y][x] == BlockType.Stone)
                {
                    if (wormNoiseX > 0.6f || wormNoiseY > 0.9f)
                    {
                        world[y][x] = BlockType.Air;
                    }
                }
            }
        }
    }
}