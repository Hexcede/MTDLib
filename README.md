# MTDLib (Placeholder)

**NOTE**: This repo is a placeholder, and is simply a proof of concept. It does not actually provide any features for modding, however this is the goal.
It implements three very minimal powerups (for fun, and just to figure out how to get various things to work in the game):
1. Invincibility
2. Infinite rerolls
3. All powerups are repeatable (Any powerup can appear more than once)

## Some tips for modding MTD (And modifying this plugin)

If you'd like to edit this yourself, download this code and extract it somewhere. Then follow the steps [here](https://docs.bepinex.dev/articles/dev_guide/plugin_tutorial/1_setup.html) to set up `dotnet`, and run `dotnet build` inside of this folder. This will create a `.dll` file in `bin/Debug/netstandard2.0` from the code that you can put into your BepInEx plugins folder.

This has some information for inexperienced and intermediate users. My end-goal is to make MTDLib do all of the work here, but I might as well provide as much information as I can for anyone who is interested in getting into the world of Unity modding.

With BepInEx, you can use the [UnityExplorer](https://github.com/sinai-dev/UnityExplorer) plugin to inspect objects & components in the game, and test various things. This is primarily what I have used to explore the game and thus create this plugin.

I would also recommend checking out [dnSpy](https://github.com/dnSpy/dnSpy), which allows you to inspect .NET code, and integrates with UnityExplorer (somewhat). This allows you to view all of the code in the game, not just field names and method names.

The `.dll`s in the `libs` folder were taken from MTD, and they are included in the `MTDLib.csproj` file in order to allow all of this code to know about the game and be able to modify and use it. See [here](https://docs.bepinex.dev/articles/dev_guide/plugin_tutorial/2_plugin_start.html) for more information on how to add more if you need to, and also why they are there. You can grab these `dll` files yourself in order to update this to work with newer versions of the game if you need to.

Lastly, a useful trick you can do is to create a link from the generated `MTDLib.dll` file to one in your BepInEx folder, and then you can run `dotnet watch build` instead of `dotnet build` to automatically recreate the `dll`, so you don't need to manually copy it back and forth, or manually type in the build command over and over. You can combine this with the [`ScriptEngine`](https://github.com/BepInEx/BepInEx.Debug) debug plugin so you can just hit F6 to reload the plugin while the game is running (see [here](https://docs.bepinex.dev/articles/dev_guide/dev_tools.html))

---

This is a BepInEx-based modding library for the game (20) Minutes Till Dawn.

## A note about Wine (For Linux users)

If you are on Wine, there is a known bug with BepInEx and some newer Unity versions that effects MTD, and occurs on only some systems. BepInEx will not load with its vanilla libraries. Unfortunately there is no convenient workaround for this issue, but it will end up being fixed. If you want an inconvenient workaround you can follow the steps in this [issue](https://github.com/BepInEx/BepInEx/issues/313) but I would recommend waiting for an official fix.
