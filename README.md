# MTDLib (Test/example)

**NOTE**: This repo is a placeholder, and is simply a proof of concept. It does not actually provide any features for modding.
It implements three very minimal powerups:
1. Invincibility
2. Infinite rerolls
3. All powerups are repeatable (Any powerup can appear more than once)

If you'd like to edit this yourself, download this code and extract it somewhere. Then follow the steps [here](https://docs.bepinex.dev/articles/dev_guide/plugin_tutorial/1_setup.html) to set up `dotnet`, and run `dotnet build` inside of this folder. This will create a `.dll` file in `bin/Debug/netstandard2.0` from the code that you can put into your BepInEx plugins folder.

---

This is a BepInEx-based modding library for the game (20) Minutes Till Dawn.

## A note about Wine (For Linux users)

If you are on Wine, there is a known bug with BepInEx and some newer Unity versions that effects MTD, and occurs on only some systems. BepInEx will not load with its vanilla libraries. Unfortunately there is no convenient workaround for this issue, but it will end up being fixed. If you want an inconvenient workaround you can follow the steps in this [issue](https://github.com/BepInEx/BepInEx/issues/313) but I would recommend waiting for an official fix.
